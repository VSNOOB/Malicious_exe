using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	public sealed class NestedPaneCollection : ReadOnlyCollection<DockPane>
	{
		private INestedPanesContainer m_container;

		private VisibleNestedPaneCollection m_visibleNestedPanes;

		public INestedPanesContainer Container => m_container;

		public VisibleNestedPaneCollection VisibleNestedPanes => m_visibleNestedPanes;

		public DockState DockState => Container.DockState;

		public bool IsFloat => DockState == DockState.Float;

		internal NestedPaneCollection(INestedPanesContainer container)
			: base((IList<DockPane>)new List<DockPane>())
		{
			m_container = container;
			m_visibleNestedPanes = new VisibleNestedPaneCollection(this);
		}

		internal void Add(DockPane pane)
		{
			if (pane != null)
			{
				NestedPaneCollection nestedPaneCollection = ((pane.NestedPanesContainer == null) ? null : pane.NestedPanesContainer.NestedPanes);
				nestedPaneCollection?.InternalRemove(pane);
				base.Items.Add(pane);
				nestedPaneCollection?.CheckFloatWindowDispose();
			}
		}

		private void CheckFloatWindowDispose()
		{
			if (base.Count == 0 && Container.DockState == DockState.Float)
			{
				FloatWindow floatWindow = (FloatWindow)Container;
				if (!((Control)floatWindow).get_Disposing() && !((Control)floatWindow).get_IsDisposed())
				{
					NativeMethods.PostMessage(((Control)(FloatWindow)Container).get_Handle(), 1025, 0u, 0u);
				}
			}
		}

		internal void Remove(DockPane pane)
		{
			InternalRemove(pane);
			CheckFloatWindowDispose();
		}

		private void InternalRemove(DockPane pane)
		{
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			if (!Contains(pane))
			{
				return;
			}
			NestedDockingStatus nestedDockingStatus = pane.NestedDockingStatus;
			DockPane dockPane = null;
			for (int num = base.Count - 1; num > IndexOf(pane); num--)
			{
				if (base[num].NestedDockingStatus.PreviousPane == pane)
				{
					dockPane = base[num];
					break;
				}
			}
			if (dockPane != null)
			{
				int num2 = IndexOf(dockPane);
				base.Items.Remove(dockPane);
				base.Items[IndexOf(pane)] = dockPane;
				NestedDockingStatus nestedDockingStatus2 = dockPane.NestedDockingStatus;
				nestedDockingStatus2.SetStatus(this, nestedDockingStatus.PreviousPane, nestedDockingStatus.Alignment, nestedDockingStatus.Proportion);
				for (int num3 = num2 - 1; num3 > IndexOf(dockPane); num3--)
				{
					NestedDockingStatus nestedDockingStatus3 = base[num3].NestedDockingStatus;
					if (nestedDockingStatus3.PreviousPane == pane)
					{
						nestedDockingStatus3.SetStatus(this, dockPane, nestedDockingStatus3.Alignment, nestedDockingStatus3.Proportion);
					}
				}
			}
			else
			{
				base.Items.Remove(pane);
			}
			nestedDockingStatus.SetStatus(null, null, DockAlignment.Left, 0.5);
			nestedDockingStatus.SetDisplayingStatus(isDisplaying: false, null, DockAlignment.Left, 0.5);
			nestedDockingStatus.SetDisplayingBounds(Rectangle.Empty, Rectangle.Empty, Rectangle.Empty);
		}

		public DockPane GetDefaultPreviousPane(DockPane pane)
		{
			for (int num = base.Count - 1; num >= 0; num--)
			{
				if (base[num] != pane)
				{
					return base[num];
				}
			}
			return null;
		}
	}
}

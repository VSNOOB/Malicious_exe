using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	public sealed class VisibleNestedPaneCollection : ReadOnlyCollection<DockPane>
	{
		private NestedPaneCollection m_nestedPanes;

		public NestedPaneCollection NestedPanes => m_nestedPanes;

		public INestedPanesContainer Container => NestedPanes.Container;

		public DockState DockState => NestedPanes.DockState;

		public bool IsFloat => NestedPanes.IsFloat;

		internal VisibleNestedPaneCollection(NestedPaneCollection nestedPanes)
			: base((IList<DockPane>)new List<DockPane>())
		{
			m_nestedPanes = nestedPanes;
		}

		internal void Refresh()
		{
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			base.Items.Clear();
			for (int i = 0; i < NestedPanes.Count; i++)
			{
				DockPane dockPane = NestedPanes[i];
				NestedDockingStatus nestedDockingStatus = dockPane.NestedDockingStatus;
				nestedDockingStatus.SetDisplayingStatus(isDisplaying: true, nestedDockingStatus.PreviousPane, nestedDockingStatus.Alignment, nestedDockingStatus.Proportion);
				base.Items.Add(dockPane);
			}
			foreach (DockPane nestedPane in NestedPanes)
			{
				if (nestedPane.DockState != DockState || nestedPane.IsHidden)
				{
					((Control)nestedPane).set_Bounds(Rectangle.Empty);
					nestedPane.SplitterBounds = Rectangle.Empty;
					Remove(nestedPane);
				}
			}
			CalculateBounds();
			using IEnumerator<DockPane> enumerator2 = GetEnumerator();
			while (enumerator2.MoveNext())
			{
				DockPane current2 = enumerator2.Current;
				NestedDockingStatus nestedDockingStatus2 = current2.NestedDockingStatus;
				((Control)current2).set_Bounds(nestedDockingStatus2.PaneBounds);
				current2.SplitterBounds = nestedDockingStatus2.SplitterBounds;
				current2.SplitterAlignment = nestedDockingStatus2.Alignment;
			}
		}

		private void Remove(DockPane pane)
		{
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
				nestedDockingStatus2.SetDisplayingStatus(isDisplaying: true, nestedDockingStatus.DisplayingPreviousPane, nestedDockingStatus.DisplayingAlignment, nestedDockingStatus.DisplayingProportion);
				for (int num3 = num2 - 1; num3 > IndexOf(dockPane); num3--)
				{
					NestedDockingStatus nestedDockingStatus3 = base[num3].NestedDockingStatus;
					if (nestedDockingStatus3.PreviousPane == pane)
					{
						nestedDockingStatus3.SetDisplayingStatus(isDisplaying: true, dockPane, nestedDockingStatus3.DisplayingAlignment, nestedDockingStatus3.DisplayingProportion);
					}
				}
			}
			else
			{
				base.Items.Remove(pane);
			}
			nestedDockingStatus.SetDisplayingStatus(isDisplaying: false, null, DockAlignment.Left, 0.5);
		}

		private void CalculateBounds()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			if (base.Count == 0)
			{
				return;
			}
			base[0].NestedDockingStatus.SetDisplayingBounds(Container.DisplayingRectangle, Container.DisplayingRectangle, Rectangle.Empty);
			for (int i = 1; i < base.Count; i++)
			{
				DockPane dockPane = base[i];
				NestedDockingStatus nestedDockingStatus = dockPane.NestedDockingStatus;
				DockPane displayingPreviousPane = nestedDockingStatus.DisplayingPreviousPane;
				NestedDockingStatus nestedDockingStatus2 = displayingPreviousPane.NestedDockingStatus;
				Rectangle paneBounds = nestedDockingStatus2.PaneBounds;
				bool flag = nestedDockingStatus.DisplayingAlignment == DockAlignment.Left || nestedDockingStatus.DisplayingAlignment == DockAlignment.Right;
				Rectangle paneBounds2 = paneBounds;
				Rectangle paneBounds3 = paneBounds;
				Rectangle splitterBounds = paneBounds;
				if (nestedDockingStatus.DisplayingAlignment == DockAlignment.Left)
				{
					((Rectangle)(ref paneBounds2)).set_Width((int)((double)((Rectangle)(ref paneBounds)).get_Width() * nestedDockingStatus.DisplayingProportion) - 2);
					((Rectangle)(ref splitterBounds)).set_X(((Rectangle)(ref paneBounds2)).get_X() + ((Rectangle)(ref paneBounds2)).get_Width());
					((Rectangle)(ref splitterBounds)).set_Width(4);
					((Rectangle)(ref paneBounds3)).set_X(((Rectangle)(ref splitterBounds)).get_X() + ((Rectangle)(ref splitterBounds)).get_Width());
					((Rectangle)(ref paneBounds3)).set_Width(((Rectangle)(ref paneBounds)).get_Width() - ((Rectangle)(ref paneBounds2)).get_Width() - ((Rectangle)(ref splitterBounds)).get_Width());
				}
				else if (nestedDockingStatus.DisplayingAlignment == DockAlignment.Right)
				{
					((Rectangle)(ref paneBounds3)).set_Width(((Rectangle)(ref paneBounds)).get_Width() - (int)((double)((Rectangle)(ref paneBounds)).get_Width() * nestedDockingStatus.DisplayingProportion) - 2);
					((Rectangle)(ref splitterBounds)).set_X(((Rectangle)(ref paneBounds3)).get_X() + ((Rectangle)(ref paneBounds3)).get_Width());
					((Rectangle)(ref splitterBounds)).set_Width(4);
					((Rectangle)(ref paneBounds2)).set_X(((Rectangle)(ref splitterBounds)).get_X() + ((Rectangle)(ref splitterBounds)).get_Width());
					((Rectangle)(ref paneBounds2)).set_Width(((Rectangle)(ref paneBounds)).get_Width() - ((Rectangle)(ref paneBounds3)).get_Width() - ((Rectangle)(ref splitterBounds)).get_Width());
				}
				else if (nestedDockingStatus.DisplayingAlignment == DockAlignment.Top)
				{
					((Rectangle)(ref paneBounds2)).set_Height((int)((double)((Rectangle)(ref paneBounds)).get_Height() * nestedDockingStatus.DisplayingProportion) - 2);
					((Rectangle)(ref splitterBounds)).set_Y(((Rectangle)(ref paneBounds2)).get_Y() + ((Rectangle)(ref paneBounds2)).get_Height());
					((Rectangle)(ref splitterBounds)).set_Height(4);
					((Rectangle)(ref paneBounds3)).set_Y(((Rectangle)(ref splitterBounds)).get_Y() + ((Rectangle)(ref splitterBounds)).get_Height());
					((Rectangle)(ref paneBounds3)).set_Height(((Rectangle)(ref paneBounds)).get_Height() - ((Rectangle)(ref paneBounds2)).get_Height() - ((Rectangle)(ref splitterBounds)).get_Height());
				}
				else if (nestedDockingStatus.DisplayingAlignment == DockAlignment.Bottom)
				{
					((Rectangle)(ref paneBounds3)).set_Height(((Rectangle)(ref paneBounds)).get_Height() - (int)((double)((Rectangle)(ref paneBounds)).get_Height() * nestedDockingStatus.DisplayingProportion) - 2);
					((Rectangle)(ref splitterBounds)).set_Y(((Rectangle)(ref paneBounds3)).get_Y() + ((Rectangle)(ref paneBounds3)).get_Height());
					((Rectangle)(ref splitterBounds)).set_Height(4);
					((Rectangle)(ref paneBounds2)).set_Y(((Rectangle)(ref splitterBounds)).get_Y() + ((Rectangle)(ref splitterBounds)).get_Height());
					((Rectangle)(ref paneBounds2)).set_Height(((Rectangle)(ref paneBounds)).get_Height() - ((Rectangle)(ref paneBounds3)).get_Height() - ((Rectangle)(ref splitterBounds)).get_Height());
				}
				else
				{
					paneBounds2 = Rectangle.Empty;
				}
				((Rectangle)(ref splitterBounds)).Intersect(paneBounds);
				((Rectangle)(ref paneBounds2)).Intersect(paneBounds);
				((Rectangle)(ref paneBounds3)).Intersect(paneBounds);
				nestedDockingStatus.SetDisplayingBounds(paneBounds, paneBounds2, splitterBounds);
				nestedDockingStatus2.SetDisplayingBounds(nestedDockingStatus2.LogicalBounds, paneBounds3, nestedDockingStatus2.SplitterBounds);
			}
		}
	}
}

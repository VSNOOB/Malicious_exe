using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	public abstract class DockPaneCaptionBase : Control
	{
		private DockPane m_dockPane;

		protected DockPane DockPane => m_dockPane;

		protected DockPane.AppearanceStyle Appearance => DockPane.Appearance;

		protected bool HasTabPageContextMenu => DockPane.HasTabPageContextMenu;

		protected internal DockPaneCaptionBase(DockPane pane)
			: this()
		{
			m_dockPane = pane;
			((Control)this).SetStyle((ControlStyles)131072, true);
			((Control)this).SetStyle((ControlStyles)16, true);
			((Control)this).SetStyle((ControlStyles)2, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).SetStyle((ControlStyles)512, false);
		}

		protected void ShowTabPageContextMenu(Point position)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DockPane.ShowTabPageContextMenu((Control)(object)this, position);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseUp(e);
			if ((int)e.get_Button() == 2097152)
			{
				ShowTabPageContextMenu(new Point(e.get_X(), e.get_Y()));
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			((Control)this).OnMouseDown(e);
			if ((int)e.get_Button() == 1048576 && DockPane.DockPanel.AllowEndUserDocking && DockPane.AllowDockDragAndDrop && !DockHelper.IsDockStateAutoHide(DockPane.DockState))
			{
				DockPane.DockPanel.BeginDrag(DockPane);
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).get_Msg() == 515)
			{
				if (DockHelper.IsDockStateAutoHide(DockPane.DockState))
				{
					DockPane.DockPanel.ActiveAutoHideContent = null;
					return;
				}
				if (DockPane.IsFloat)
				{
					DockPane.RestoreToPanel();
				}
				else
				{
					DockPane.Float();
				}
			}
			((Control)this).WndProc(ref m);
		}

		internal void RefreshChanges()
		{
			OnRefreshChanges();
		}

		protected virtual void OnRightToLeftLayoutChanged()
		{
		}

		protected virtual void OnRefreshChanges()
		{
		}

		protected internal abstract int MeasureHeight();
	}
}

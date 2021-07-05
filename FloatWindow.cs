using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	public class FloatWindow : Form, INestedPanesContainer, IDockDragSource, IDragSource
	{
		private NestedPaneCollection m_nestedPanes;

		internal const int WM_CHECKDISPOSE = 1025;

		private bool m_allowEndUserDocking = true;

		private DockPanel m_dockPanel;

		public bool AllowEndUserDocking
		{
			get
			{
				return m_allowEndUserDocking;
			}
			set
			{
				m_allowEndUserDocking = value;
			}
		}

		public NestedPaneCollection NestedPanes => m_nestedPanes;

		public VisibleNestedPaneCollection VisibleNestedPanes => NestedPanes.VisibleNestedPanes;

		public DockPanel DockPanel => m_dockPanel;

		public DockState DockState => DockState.Float;

		public bool IsFloat => DockState == DockState.Float;

		public virtual Rectangle DisplayingRectangle => ((Control)this).get_ClientRectangle();

		Control IDragSource.DragControl => (Control)(object)this;

		protected internal FloatWindow(DockPanel dockPanel, DockPane pane)
			: this()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			InternalConstruct(dockPanel, pane, boundsSpecified: false, Rectangle.Empty);
		}

		protected internal FloatWindow(DockPanel dockPanel, DockPane pane, Rectangle bounds)
			: this()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			InternalConstruct(dockPanel, pane, boundsSpecified: true, bounds);
		}

		private void InternalConstruct(DockPanel dockPanel, DockPane pane, bool boundsSpecified, Rectangle bounds)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			if (dockPanel == null)
			{
				throw new ArgumentNullException(Strings.FloatWindow_Constructor_NullDockPanel);
			}
			m_nestedPanes = new NestedPaneCollection(this);
			((Form)this).set_FormBorderStyle((FormBorderStyle)6);
			((Form)this).set_ShowInTaskbar(false);
			if (((Control)dockPanel).get_RightToLeft() != ((Control)this).get_RightToLeft())
			{
				((Control)this).set_RightToLeft(((Control)dockPanel).get_RightToLeft());
			}
			if (((Form)this).get_RightToLeftLayout() != dockPanel.RightToLeftLayout)
			{
				((Form)this).set_RightToLeftLayout(dockPanel.RightToLeftLayout);
			}
			((Control)this).SuspendLayout();
			if (boundsSpecified)
			{
				((Control)this).set_Bounds(bounds);
				((Form)this).set_StartPosition((FormStartPosition)0);
			}
			else
			{
				((Form)this).set_StartPosition((FormStartPosition)2);
				((Form)this).set_Size(dockPanel.DefaultFloatWindowSize);
			}
			m_dockPanel = dockPanel;
			((Form)this).set_Owner(((Control)DockPanel).FindForm());
			DockPanel.AddFloatWindow(this);
			if (pane != null)
			{
				pane.FloatWindow = this;
			}
			((Control)this).ResumeLayout();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (DockPanel != null)
				{
					DockPanel.RemoveFloatWindow(this);
				}
				m_dockPanel = null;
			}
			((Form)this).Dispose(disposing);
		}

		internal bool IsDockStateValid(DockState dockState)
		{
			foreach (DockPane nestedPane in NestedPanes)
			{
				foreach (IDockContent content in nestedPane.Contents)
				{
					if (!DockHelper.IsDockStateValid(dockState, content.DockHandler.DockAreas))
					{
						return false;
					}
				}
			}
			return true;
		}

		protected override void OnActivated(EventArgs e)
		{
			DockPanel.FloatWindows.BringWindowToFront(this);
			((Form)this).OnActivated(e);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			VisibleNestedPanes.Refresh();
			RefreshChanges();
			((Control)this).set_Visible(VisibleNestedPanes.Count > 0);
			SetText();
			((Form)this).OnLayout(levent);
		}

		internal void SetText()
		{
			DockPane dockPane = ((VisibleNestedPanes.Count == 1) ? VisibleNestedPanes[0] : null);
			if (dockPane == null)
			{
				((Control)this).set_Text(" ");
			}
			else if (dockPane.ActiveContent == null)
			{
				((Control)this).set_Text(" ");
			}
			else
			{
				((Control)this).set_Text(dockPane.ActiveContent.DockHandler.TabText);
			}
		}

		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Rectangle workingArea = SystemInformation.get_WorkingArea();
			if (y + height > ((Rectangle)(ref workingArea)).get_Bottom())
			{
				y -= y + height - ((Rectangle)(ref workingArea)).get_Bottom();
			}
			if (y < ((Rectangle)(ref workingArea)).get_Top())
			{
				y += ((Rectangle)(ref workingArea)).get_Top() - y;
			}
			((Form)this).SetBoundsCore(x, y, width, height, specified);
		}

		protected override void WndProc(ref Message m)
		{
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			if (((Message)(ref m)).get_Msg() == 161)
			{
				if (!((Control)this).get_IsDisposed())
				{
					uint num = NativeMethods.SendMessage(((Control)this).get_Handle(), 132, 0u, (uint)(int)((Message)(ref m)).get_LParam());
					if (num == 2 && DockPanel.AllowEndUserDocking && AllowEndUserDocking)
					{
						((Form)this).Activate();
						m_dockPanel.BeginDrag(this);
					}
					else
					{
						((Form)this).WndProc(ref m);
					}
				}
			}
			else if (((Message)(ref m)).get_Msg() == 164)
			{
				uint num2 = NativeMethods.SendMessage(((Control)this).get_Handle(), 132, 0u, (uint)(int)((Message)(ref m)).get_LParam());
				if (num2 == 2)
				{
					DockPane dockPane = ((VisibleNestedPanes.Count == 1) ? VisibleNestedPanes[0] : null);
					if (dockPane != null && dockPane.ActiveContent != null)
					{
						dockPane.ShowTabPageContextMenu((Control)(object)this, ((Control)this).PointToClient(Control.get_MousePosition()));
						return;
					}
				}
				((Form)this).WndProc(ref m);
			}
			else if (((Message)(ref m)).get_Msg() == 16)
			{
				if (NestedPanes.Count == 0)
				{
					((Form)this).WndProc(ref m);
					return;
				}
				for (int num3 = NestedPanes.Count - 1; num3 >= 0; num3--)
				{
					DockContentCollection contents = NestedPanes[num3].Contents;
					for (int num4 = contents.Count - 1; num4 >= 0; num4--)
					{
						IDockContent dockContent = contents[num4];
						if (dockContent.DockHandler.DockState == DockState.Float && dockContent.DockHandler.CloseButton)
						{
							if (dockContent.DockHandler.HideOnClose)
							{
								dockContent.DockHandler.Hide();
							}
							else
							{
								dockContent.DockHandler.Close();
							}
						}
					}
				}
			}
			else if (((Message)(ref m)).get_Msg() == 163)
			{
				uint num5 = NativeMethods.SendMessage(((Control)this).get_Handle(), 132, 0u, (uint)(int)((Message)(ref m)).get_LParam());
				if (num5 != 2)
				{
					((Form)this).WndProc(ref m);
					return;
				}
				DockPanel.SuspendLayout(allWindows: true);
				foreach (DockPane nestedPane in NestedPanes)
				{
					if (nestedPane.DockState == DockState.Float)
					{
						nestedPane.RestoreToPanel();
					}
				}
				DockPanel.ResumeLayout(performLayout: true, allWindows: true);
			}
			else if (((Message)(ref m)).get_Msg() == 1025)
			{
				if (NestedPanes.Count == 0)
				{
					((Component)this).Dispose();
				}
			}
			else
			{
				((Form)this).WndProc(ref m);
			}
		}

		internal void RefreshChanges()
		{
			if (VisibleNestedPanes.Count == 0)
			{
				((Form)this).set_ControlBox(true);
				return;
			}
			for (int num = VisibleNestedPanes.Count - 1; num >= 0; num--)
			{
				DockContentCollection contents = VisibleNestedPanes[num].Contents;
				for (int num2 = contents.Count - 1; num2 >= 0; num2--)
				{
					IDockContent dockContent = contents[num2];
					if (dockContent.DockHandler.DockState == DockState.Float && dockContent.DockHandler.CloseButton)
					{
						((Form)this).set_ControlBox(true);
						return;
					}
				}
			}
			((Form)this).set_ControlBox(false);
		}

		internal void TestDrop(IDockDragSource dragSource, DockOutlineBase dockOutline)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (VisibleNestedPanes.Count != 1)
			{
				return;
			}
			DockPane pane = VisibleNestedPanes[0];
			if (dragSource.CanDockTo(pane))
			{
				Point mousePosition = Control.get_MousePosition();
				uint lParam = Win32Helper.MakeLong(((Point)(ref mousePosition)).get_X(), ((Point)(ref mousePosition)).get_Y());
				if (NativeMethods.SendMessage(((Control)this).get_Handle(), 132, 0u, lParam) == 2)
				{
					dockOutline.Show(VisibleNestedPanes[0], -1);
				}
			}
		}

		bool IDockDragSource.IsDockStateValid(DockState dockState)
		{
			return IsDockStateValid(dockState);
		}

		bool IDockDragSource.CanDockTo(DockPane pane)
		{
			if (!IsDockStateValid(pane.DockState))
			{
				return false;
			}
			if (pane.FloatWindow == this)
			{
				return false;
			}
			return true;
		}

		Rectangle IDockDragSource.BeginDrag(Point ptMouse)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).get_Bounds();
		}

		public void FloatAt(Rectangle floatWindowBounds)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).set_Bounds(floatWindowBounds);
		}

		public void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Invalid comparison between Unknown and I4
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Invalid comparison between Unknown and I4
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Invalid comparison between Unknown and I4
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Invalid comparison between Unknown and I4
			if ((int)dockStyle == 5)
			{
				for (int num = NestedPanes.Count - 1; num >= 0; num--)
				{
					DockPane dockPane = NestedPanes[num];
					for (int num2 = dockPane.Contents.Count - 1; num2 >= 0; num2--)
					{
						IDockContent dockContent = dockPane.Contents[num2];
						dockContent.DockHandler.Pane = pane;
						if (contentIndex != -1)
						{
							pane.SetContentIndex(dockContent, contentIndex);
						}
					}
				}
			}
			else
			{
				DockAlignment alignment = DockAlignment.Left;
				if ((int)dockStyle == 3)
				{
					alignment = DockAlignment.Left;
				}
				else if ((int)dockStyle == 4)
				{
					alignment = DockAlignment.Right;
				}
				else if ((int)dockStyle == 1)
				{
					alignment = DockAlignment.Top;
				}
				else if ((int)dockStyle == 2)
				{
					alignment = DockAlignment.Bottom;
				}
				MergeNestedPanes(VisibleNestedPanes, pane.NestedPanesContainer.NestedPanes, pane, alignment, 0.5);
			}
		}

		public void DockTo(DockPanel panel, DockStyle dockStyle)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Invalid comparison between Unknown and I4
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Invalid comparison between Unknown and I4
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Invalid comparison between Unknown and I4
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Invalid comparison between Unknown and I4
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Invalid comparison between Unknown and I4
			if (panel != DockPanel)
			{
				throw new ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, "panel");
			}
			NestedPaneCollection nestedPaneCollection = null;
			if ((int)dockStyle == 1)
			{
				nestedPaneCollection = DockPanel.DockWindows[DockState.DockTop].NestedPanes;
			}
			else if ((int)dockStyle == 2)
			{
				nestedPaneCollection = DockPanel.DockWindows[DockState.DockBottom].NestedPanes;
			}
			else if ((int)dockStyle == 3)
			{
				nestedPaneCollection = DockPanel.DockWindows[DockState.DockLeft].NestedPanes;
			}
			else if ((int)dockStyle == 4)
			{
				nestedPaneCollection = DockPanel.DockWindows[DockState.DockRight].NestedPanes;
			}
			else if ((int)dockStyle == 5)
			{
				nestedPaneCollection = DockPanel.DockWindows[DockState.Document].NestedPanes;
			}
			DockPane prevPane = null;
			for (int num = nestedPaneCollection.Count - 1; num >= 0; num--)
			{
				if (nestedPaneCollection[num] != VisibleNestedPanes[0])
				{
					prevPane = nestedPaneCollection[num];
				}
			}
			MergeNestedPanes(VisibleNestedPanes, nestedPaneCollection, prevPane, DockAlignment.Left, 0.5);
		}

		private static void MergeNestedPanes(VisibleNestedPaneCollection nestedPanesFrom, NestedPaneCollection nestedPanesTo, DockPane prevPane, DockAlignment alignment, double proportion)
		{
			if (nestedPanesFrom.Count == 0)
			{
				return;
			}
			int count = nestedPanesFrom.Count;
			DockPane[] array = new DockPane[count];
			DockPane[] array2 = new DockPane[count];
			DockAlignment[] array3 = new DockAlignment[count];
			double[] array4 = new double[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = nestedPanesFrom[i];
				array2[i] = nestedPanesFrom[i].NestedDockingStatus.PreviousPane;
				array3[i] = nestedPanesFrom[i].NestedDockingStatus.Alignment;
				array4[i] = nestedPanesFrom[i].NestedDockingStatus.Proportion;
			}
			DockPane dockPane = array[0].DockTo(nestedPanesTo.Container, prevPane, alignment, proportion);
			array[0].DockState = nestedPanesTo.DockState;
			for (int j = 1; j < count; j++)
			{
				for (int k = j; k < count; k++)
				{
					if (array2[k] == array[j - 1])
					{
						array2[k] = dockPane;
					}
				}
				dockPane = array[j].DockTo(nestedPanesTo.Container, array2[j], array3[j], array4[j]);
				array[j].DockState = nestedPanesTo.DockState;
			}
		}
	}
}

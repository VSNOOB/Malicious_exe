using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	[ToolboxItem(false)]
	public class DockWindow : Panel, INestedPanesContainer, ISplitterDragSource, IDragSource
	{
		private class SplitterControl : SplitterBase
		{
			protected override int SplitterSize => 4;

			protected override void StartDrag()
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				DockWindow dockWindow = ((Control)this).get_Parent() as DockWindow;
				dockWindow?.DockPanel.BeginDrag(dockWindow, ((Control)dockWindow).RectangleToScreen(((Control)this).get_Bounds()));
			}
		}

		private DockPanel m_dockPanel;

		private DockState m_dockState;

		private SplitterControl m_splitter;

		private NestedPaneCollection m_nestedPanes;

		public VisibleNestedPaneCollection VisibleNestedPanes => NestedPanes.VisibleNestedPanes;

		public NestedPaneCollection NestedPanes => m_nestedPanes;

		public DockPanel DockPanel => m_dockPanel;

		public DockState DockState => m_dockState;

		public bool IsFloat => DockState == DockState.Float;

		internal DockPane DefaultPane => (VisibleNestedPanes.Count == 0) ? null : VisibleNestedPanes[0];

		public virtual Rectangle DisplayingRectangle
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
				if (DockState == DockState.Document)
				{
					((Rectangle)(ref clientRectangle)).set_X(((Rectangle)(ref clientRectangle)).get_X() + 1);
					((Rectangle)(ref clientRectangle)).set_Y(((Rectangle)(ref clientRectangle)).get_Y() + 1);
					((Rectangle)(ref clientRectangle)).set_Width(((Rectangle)(ref clientRectangle)).get_Width() - 2);
					((Rectangle)(ref clientRectangle)).set_Height(((Rectangle)(ref clientRectangle)).get_Height() - 2);
				}
				else if (DockState == DockState.DockLeft)
				{
					((Rectangle)(ref clientRectangle)).set_Width(((Rectangle)(ref clientRectangle)).get_Width() - 4);
				}
				else if (DockState == DockState.DockRight)
				{
					((Rectangle)(ref clientRectangle)).set_X(((Rectangle)(ref clientRectangle)).get_X() + 4);
					((Rectangle)(ref clientRectangle)).set_Width(((Rectangle)(ref clientRectangle)).get_Width() - 4);
				}
				else if (DockState == DockState.DockTop)
				{
					((Rectangle)(ref clientRectangle)).set_Height(((Rectangle)(ref clientRectangle)).get_Height() - 4);
				}
				else if (DockState == DockState.DockBottom)
				{
					((Rectangle)(ref clientRectangle)).set_Y(((Rectangle)(ref clientRectangle)).get_Y() + 4);
					((Rectangle)(ref clientRectangle)).set_Height(((Rectangle)(ref clientRectangle)).get_Height() - 4);
				}
				return clientRectangle;
			}
		}

		bool ISplitterDragSource.IsVertical => DockState == DockState.DockLeft || DockState == DockState.DockRight;

		Rectangle ISplitterDragSource.DragLimitBounds
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Invalid comparison between Unknown and I4
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Invalid comparison between Unknown and I4
				//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00db: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dd: Invalid comparison between Unknown and I4
				//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_0100: Unknown result type (might be due to invalid IL or missing references)
				//IL_0104: Unknown result type (might be due to invalid IL or missing references)
				Rectangle dockArea = DockPanel.DockArea;
				Point location;
				if ((Control.get_ModifierKeys() & 0x10000) == 0)
				{
					location = ((Control)this).get_Location();
				}
				else
				{
					Rectangle dockArea2 = DockPanel.DockArea;
					location = ((Rectangle)(ref dockArea2)).get_Location();
				}
				if (((ISplitterDragSource)this).IsVertical)
				{
					((Rectangle)(ref dockArea)).set_X(((Rectangle)(ref dockArea)).get_X() + 24);
					((Rectangle)(ref dockArea)).set_Width(((Rectangle)(ref dockArea)).get_Width() - 48);
					((Rectangle)(ref dockArea)).set_Y(((Point)(ref location)).get_Y());
					if ((Control.get_ModifierKeys() & 0x10000) == 0)
					{
						((Rectangle)(ref dockArea)).set_Height(((Control)this).get_Height());
					}
				}
				else
				{
					((Rectangle)(ref dockArea)).set_Y(((Rectangle)(ref dockArea)).get_Y() + 24);
					((Rectangle)(ref dockArea)).set_Height(((Rectangle)(ref dockArea)).get_Height() - 48);
					((Rectangle)(ref dockArea)).set_X(((Point)(ref location)).get_X());
					if ((Control.get_ModifierKeys() & 0x10000) == 0)
					{
						((Rectangle)(ref dockArea)).set_Width(((Control)this).get_Width());
					}
				}
				return ((Control)DockPanel).RectangleToScreen(dockArea);
			}
		}

		Control IDragSource.DragControl => (Control)(object)this;

		internal DockWindow(DockPanel dockPanel, DockState dockState)
			: this()
		{
			m_nestedPanes = new NestedPaneCollection(this);
			m_dockPanel = dockPanel;
			m_dockState = dockState;
			((Control)this).set_Visible(false);
			((Control)this).SuspendLayout();
			if (DockState == DockState.DockLeft || DockState == DockState.DockRight || DockState == DockState.DockTop || DockState == DockState.DockBottom)
			{
				m_splitter = new SplitterControl();
				((Control)this).get_Controls().Add((Control)(object)m_splitter);
			}
			if (DockState == DockState.DockLeft)
			{
				((Control)this).set_Dock((DockStyle)3);
				((Control)m_splitter).set_Dock((DockStyle)4);
			}
			else if (DockState == DockState.DockRight)
			{
				((Control)this).set_Dock((DockStyle)4);
				((Control)m_splitter).set_Dock((DockStyle)3);
			}
			else if (DockState == DockState.DockTop)
			{
				((Control)this).set_Dock((DockStyle)1);
				((Control)m_splitter).set_Dock((DockStyle)2);
			}
			else if (DockState == DockState.DockBottom)
			{
				((Control)this).set_Dock((DockStyle)2);
				((Control)m_splitter).set_Dock((DockStyle)1);
			}
			else if (DockState == DockState.Document)
			{
				((Control)this).set_Dock((DockStyle)5);
			}
			((Control)this).ResumeLayout();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if (DockState == DockState.Document)
			{
				Graphics graphics = e.get_Graphics();
				Pen controlDark = SystemPens.get_ControlDark();
				Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
				int x = ((Rectangle)(ref clientRectangle)).get_X();
				clientRectangle = ((Control)this).get_ClientRectangle();
				int y = ((Rectangle)(ref clientRectangle)).get_Y();
				clientRectangle = ((Control)this).get_ClientRectangle();
				int num = ((Rectangle)(ref clientRectangle)).get_Width() - 1;
				clientRectangle = ((Control)this).get_ClientRectangle();
				graphics.DrawRectangle(controlDark, x, y, num, ((Rectangle)(ref clientRectangle)).get_Height() - 1);
			}
			((Control)this).OnPaint(e);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			VisibleNestedPanes.Refresh();
			if (VisibleNestedPanes.Count == 0)
			{
				if (((Control)this).get_Visible())
				{
					((Control)this).set_Visible(false);
				}
			}
			else if (!((Control)this).get_Visible())
			{
				((Control)this).set_Visible(true);
				VisibleNestedPanes.Refresh();
			}
			((ScrollableControl)this).OnLayout(levent);
		}

		void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
		{
		}

		void ISplitterDragSource.EndDrag()
		{
		}

		void ISplitterDragSource.MoveSplitter(int offset)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Invalid comparison between Unknown and I4
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if ((Control.get_ModifierKeys() & 0x10000) > 0)
			{
				((Control)this).SendToBack();
			}
			Rectangle dockArea = DockPanel.DockArea;
			if (DockState == DockState.DockLeft && ((Rectangle)(ref dockArea)).get_Width() > 0)
			{
				if (DockPanel.DockLeftPortion > 1.0)
				{
					DockPanel.DockLeftPortion = ((Control)this).get_Width() + offset;
				}
				else
				{
					DockPanel.DockLeftPortion += (double)offset / (double)((Rectangle)(ref dockArea)).get_Width();
				}
			}
			else if (DockState == DockState.DockRight && ((Rectangle)(ref dockArea)).get_Width() > 0)
			{
				if (DockPanel.DockRightPortion > 1.0)
				{
					DockPanel.DockRightPortion = ((Control)this).get_Width() - offset;
				}
				else
				{
					DockPanel.DockRightPortion -= (double)offset / (double)((Rectangle)(ref dockArea)).get_Width();
				}
			}
			else if (DockState == DockState.DockBottom && ((Rectangle)(ref dockArea)).get_Height() > 0)
			{
				if (DockPanel.DockBottomPortion > 1.0)
				{
					DockPanel.DockBottomPortion = ((Control)this).get_Height() - offset;
				}
				else
				{
					DockPanel.DockBottomPortion -= (double)offset / (double)((Rectangle)(ref dockArea)).get_Height();
				}
			}
			else if (DockState == DockState.DockTop && ((Rectangle)(ref dockArea)).get_Height() > 0)
			{
				if (DockPanel.DockTopPortion > 1.0)
				{
					DockPanel.DockTopPortion = ((Control)this).get_Height() + offset;
				}
				else
				{
					DockPanel.DockTopPortion += (double)offset / (double)((Rectangle)(ref dockArea)).get_Height();
				}
			}
		}
	}
}

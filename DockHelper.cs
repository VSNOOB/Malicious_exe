using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal static class DockHelper
	{
		public static bool IsDockStateAutoHide(DockState dockState)
		{
			if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide || dockState == DockState.DockTopAutoHide || dockState == DockState.DockBottomAutoHide)
			{
				return true;
			}
			return false;
		}

		public static bool IsDockStateValid(DockState dockState, DockAreas dockableAreas)
		{
			if ((dockableAreas & DockAreas.Float) == 0 && dockState == DockState.Float)
			{
				return false;
			}
			if ((dockableAreas & DockAreas.Document) == 0 && dockState == DockState.Document)
			{
				return false;
			}
			if ((dockableAreas & DockAreas.DockLeft) == 0 && (dockState == DockState.DockLeft || dockState == DockState.DockLeftAutoHide))
			{
				return false;
			}
			if ((dockableAreas & DockAreas.DockRight) == 0 && (dockState == DockState.DockRight || dockState == DockState.DockRightAutoHide))
			{
				return false;
			}
			if ((dockableAreas & DockAreas.DockTop) == 0 && (dockState == DockState.DockTop || dockState == DockState.DockTopAutoHide))
			{
				return false;
			}
			if ((dockableAreas & DockAreas.DockBottom) == 0 && (dockState == DockState.DockBottom || dockState == DockState.DockBottomAutoHide))
			{
				return false;
			}
			return true;
		}

		public static bool IsDockWindowState(DockState state)
		{
			if (state == DockState.DockTop || state == DockState.DockBottom || state == DockState.DockLeft || state == DockState.DockRight || state == DockState.Document)
			{
				return true;
			}
			return false;
		}

		public static DockState ToggleAutoHideState(DockState state)
		{
			return state switch
			{
				DockState.DockLeft => DockState.DockLeftAutoHide, 
				DockState.DockRight => DockState.DockRightAutoHide, 
				DockState.DockTop => DockState.DockTopAutoHide, 
				DockState.DockBottom => DockState.DockBottomAutoHide, 
				DockState.DockLeftAutoHide => DockState.DockLeft, 
				DockState.DockRightAutoHide => DockState.DockRight, 
				DockState.DockTopAutoHide => DockState.DockTop, 
				DockState.DockBottomAutoHide => DockState.DockBottom, 
				_ => state, 
			};
		}

		public static DockPane PaneAtPoint(Point pt, DockPanel dockPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			for (Control val = Win32Helper.ControlAtPoint(pt); val != null; val = val.get_Parent())
			{
				IDockContent dockContent = val as IDockContent;
				if (dockContent != null && dockContent.DockHandler.DockPanel == dockPanel)
				{
					return dockContent.DockHandler.Pane;
				}
				DockPane dockPane = val as DockPane;
				if (dockPane != null && dockPane.DockPanel == dockPanel)
				{
					return dockPane;
				}
			}
			return null;
		}

		public static FloatWindow FloatWindowAtPoint(Point pt, DockPanel dockPanel)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			for (Control val = Win32Helper.ControlAtPoint(pt); val != null; val = val.get_Parent())
			{
				FloatWindow floatWindow = val as FloatWindow;
				if (floatWindow != null && floatWindow.DockPanel == dockPanel)
				{
					return floatWindow;
				}
			}
			return null;
		}
	}
}

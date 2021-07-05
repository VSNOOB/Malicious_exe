using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal static class Win32Helper
	{
		public static Control ControlAtPoint(Point pt)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return Control.FromChildHandle(NativeMethods.WindowFromPoint(pt));
		}

		public static uint MakeLong(int low, int high)
		{
			return (uint)((high << 16) + low);
		}
	}
}

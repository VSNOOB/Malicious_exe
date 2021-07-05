using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal static class DrawHelper
	{
		public static Point RtlTransform(Control control, Point point)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if ((int)control.get_RightToLeft() != 1)
			{
				return point;
			}
			return new Point(control.get_Right() - ((Point)(ref point)).get_X(), ((Point)(ref point)).get_Y());
		}

		public static Rectangle RtlTransform(Control control, Rectangle rectangle)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if ((int)control.get_RightToLeft() != 1)
			{
				return rectangle;
			}
			Rectangle clientRectangle = control.get_ClientRectangle();
			return new Rectangle(((Rectangle)(ref clientRectangle)).get_Right() - ((Rectangle)(ref rectangle)).get_Right(), ((Rectangle)(ref rectangle)).get_Y(), ((Rectangle)(ref rectangle)).get_Width(), ((Rectangle)(ref rectangle)).get_Height());
		}

		public static GraphicsPath GetRoundedCornerTab(GraphicsPath graphicsPath, Rectangle rect, bool upCorner)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			if (graphicsPath == null)
			{
				graphicsPath = new GraphicsPath();
			}
			else
			{
				graphicsPath.Reset();
			}
			int num = 6;
			if (upCorner)
			{
				graphicsPath.AddLine(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom(), ((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top() + num / 2);
				graphicsPath.AddArc(new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top(), num, num), 180f, 90f);
				graphicsPath.AddLine(((Rectangle)(ref rect)).get_Left() + num / 2, ((Rectangle)(ref rect)).get_Top(), ((Rectangle)(ref rect)).get_Right() - num / 2, ((Rectangle)(ref rect)).get_Top());
				graphicsPath.AddArc(new Rectangle(((Rectangle)(ref rect)).get_Right() - num, ((Rectangle)(ref rect)).get_Top(), num, num), -90f, 90f);
				graphicsPath.AddLine(((Rectangle)(ref rect)).get_Right(), ((Rectangle)(ref rect)).get_Top() + num / 2, ((Rectangle)(ref rect)).get_Right(), ((Rectangle)(ref rect)).get_Bottom());
			}
			else
			{
				graphicsPath.AddLine(((Rectangle)(ref rect)).get_Right(), ((Rectangle)(ref rect)).get_Top(), ((Rectangle)(ref rect)).get_Right(), ((Rectangle)(ref rect)).get_Bottom() - num / 2);
				graphicsPath.AddArc(new Rectangle(((Rectangle)(ref rect)).get_Right() - num, ((Rectangle)(ref rect)).get_Bottom() - num, num, num), 0f, 90f);
				graphicsPath.AddLine(((Rectangle)(ref rect)).get_Right() - num / 2, ((Rectangle)(ref rect)).get_Bottom(), ((Rectangle)(ref rect)).get_Left() + num / 2, ((Rectangle)(ref rect)).get_Bottom());
				graphicsPath.AddArc(new Rectangle(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - num, num, num), 90f, 90f);
				graphicsPath.AddLine(((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Bottom() - num / 2, ((Rectangle)(ref rect)).get_Left(), ((Rectangle)(ref rect)).get_Top());
			}
			return graphicsPath;
		}

		public static GraphicsPath CalculateGraphicsPathFromBitmap(Bitmap bitmap)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			return CalculateGraphicsPathFromBitmap(bitmap, Color.Empty);
		}

		public static GraphicsPath CalculateGraphicsPathFromBitmap(Bitmap bitmap, Color colorTransparent)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			GraphicsPath val = new GraphicsPath();
			if (colorTransparent == Color.Empty)
			{
				colorTransparent = bitmap.GetPixel(0, 0);
			}
			for (int i = 0; i < ((Image)bitmap).get_Height(); i++)
			{
				int num = 0;
				for (int j = 0; j < ((Image)bitmap).get_Width(); j++)
				{
					if (bitmap.GetPixel(j, i) != colorTransparent)
					{
						num = j;
						int num2 = j;
						for (num2 = num; num2 < ((Image)bitmap).get_Width() && !(bitmap.GetPixel(num2, i) == colorTransparent); num2++)
						{
						}
						val.AddRectangle(new Rectangle(num, i, num2 - num, 1));
						j = num2;
					}
				}
			}
			return val;
		}
	}
}

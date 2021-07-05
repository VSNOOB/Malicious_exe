using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace TabControl
{
	public class TabItemResizeBehavior : Behavior
	{
		private TabItem tab;

		private bool resizing;

		private int startWidth;

		private Point startLocation;

		public TabItemResizeBehavior(TabItem tab)
			: this()
		{
			this.tab = tab;
		}

		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLocation)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			startLocation = mouseLocation;
			startWidth = tab.TabWidth;
			resizing = true;
			return true;
		}

		public override bool OnMouseMove(Glyph g, MouseButtons button, Point mouseLocation)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Invalid comparison between Unknown and I4
			if (resizing)
			{
				TabAlignment val = (TabAlignment)0;
				PropertyDescriptor val2 = TypeDescriptor.GetProperties((object)tab).get_Item("TabWidth");
				if (tab.Owner != null)
				{
					val = tab.Owner.TabAlignment;
				}
				int num = (((int)val != 0 && (int)val != 1) ? Math.Max(1, startWidth + (((Point)(ref mouseLocation)).get_Y() - ((Point)(ref startLocation)).get_Y())) : Math.Max(1, startWidth + (((Point)(ref mouseLocation)).get_X() - ((Point)(ref startLocation)).get_X())));
				if (num != tab.TabWidth)
				{
					val2.SetValue((object)tab, (object)num);
				}
			}
			return true;
		}

		public override bool OnMouseUp(Glyph g, MouseButtons button)
		{
			resizing = false;
			return true;
		}
	}
}

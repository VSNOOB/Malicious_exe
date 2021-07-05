using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace TabControl
{
	public class TabHostBehavior : Behavior
	{
		public override bool OnMouseDown(Glyph g, MouseButtons button, Point mouseLoc)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			TabHostGlyph tabHostGlyph = g as TabHostGlyph;
			if (tabHostGlyph == null)
			{
				return ((Behavior)this).OnMouseDown(g, button, mouseLoc);
			}
			object service = ((IServiceProvider)((Component)tabHostGlyph.TabHost).get_Site()).GetService(typeof(IDesignerHost));
			IDesignerHost val = service as IDesignerHost;
			TabItem value = val.CreateComponent(typeof(TabItem)) as TabItem;
			tabHostGlyph.TabHost.Tabs.Add(value);
			tabHostGlyph.ComputeBounds();
			tabHostGlyph.Adorner.Invalidate();
			return true;
		}

		public TabHostBehavior()
			: this()
		{
		}
	}
}

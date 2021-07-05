using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace TabControl
{
	public class TabHostGlyph : Glyph
	{
		private const int GlyphDistance = 10;

		private const int GlyphWidth = 16;

		private const int GlyphHeight = 16;

		private TabHost tabHost;

		private BehaviorService service;

		private Adorner adorner;

		private ISelectionService selectionService;

		private Rectangle glyphBounds;

		private bool selected;

		public TabHost TabHost => tabHost;

		public Adorner Adorner => adorner;

		public override Rectangle Bounds => glyphBounds;

		public TabHostGlyph(BehaviorService behaviorSvc, Control control, Adorner glyphAdorner, ISelectionService selectionService)
			: this((Behavior)(object)new TabHostBehavior())
		{
			service = behaviorSvc;
			tabHost = control as TabHost;
			adorner = glyphAdorner;
			this.selectionService = selectionService;
			selectionService.add_SelectionChanged((EventHandler)OnSelectionChanged);
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (selectionService.get_PrimarySelection() == tabHost)
			{
				ComputeBounds();
				adorner.set_Enabled(true);
				selected = true;
			}
			else
			{
				adorner.set_Enabled(false);
				selected = false;
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			Rectangle bounds = ((Glyph)this).get_Bounds();
			if (((Rectangle)(ref bounds)).Contains(p))
			{
				return Cursors.get_Hand();
			}
			return null;
		}

		public void ComputeBounds()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Invalid comparison between Unknown and I4
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Invalid comparison between Unknown and I4
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Invalid comparison between Unknown and I4
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			Point val = service.ControlToAdornerWindow((Control)(object)tabHost);
			int num = 0;
			int num2 = 0;
			if (tabHost.Tabs.Count > 0)
			{
				if ((int)tabHost.TabAlignment == 0)
				{
					num = ((Point)(ref val)).get_X() + ((Control)tabHost.Tabs[tabHost.Tabs.Count - 1]).get_Left() + tabHost.Tabs[tabHost.Tabs.Count - 1].Width + 10;
					num2 = ((Point)(ref val)).get_Y() + ((Control)tabHost).get_Height() - 16 - (tabHost.TabHeight - 16) / 2;
				}
				else if ((int)tabHost.TabAlignment == 1)
				{
					num = ((Point)(ref val)).get_X() + ((Control)tabHost.Tabs[tabHost.Tabs.Count - 1]).get_Left() + tabHost.Tabs[tabHost.Tabs.Count - 1].Width + 10;
					num2 = ((Point)(ref val)).get_Y() + tabHost.TabHeight / 2 - 8;
				}
				else if ((int)tabHost.TabAlignment == 2)
				{
					num = ((Point)(ref val)).get_X() + tabHost.TabHeight / 2 - 8;
					num2 = ((Point)(ref val)).get_Y() + ((Control)tabHost.Tabs[tabHost.Tabs.Count - 1]).get_Top() + tabHost.Tabs[tabHost.Tabs.Count - 1].Height + 10;
				}
				else
				{
					num = ((Point)(ref val)).get_X() + ((Control)tabHost).get_Width() - 16 - (tabHost.TabHeight - 16) / 2;
					num2 = ((Point)(ref val)).get_Y() + ((Control)tabHost.Tabs[tabHost.Tabs.Count - 1]).get_Top() + tabHost.Tabs[tabHost.Tabs.Count - 1].Height + 10;
				}
			}
			glyphBounds = new Rectangle(num, num2, 16, 16);
		}

		public override void Paint(PaintEventArgs pe)
		{
			if (!adorner.get_Enabled() || !selected)
			{
			}
		}
	}
}

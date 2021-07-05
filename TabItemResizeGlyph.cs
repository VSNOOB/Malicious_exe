using System;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design.Behavior;

namespace TabControl
{
	public class TabItemResizeGlyph : Glyph
	{
		private const int GlyphWidth = 26;

		private const int GlyphHeight = 26;

		private TabItem tab;

		private BehaviorService service;

		private Adorner adorner;

		private ISelectionService selectionService;

		private IComponentChangeService changeService;

		private Rectangle glyphBounds;

		private bool selected;

		public TabItem Tab => tab;

		public Adorner Adorner => adorner;

		public override Rectangle Bounds => glyphBounds;

		public TabItemResizeGlyph(BehaviorService behaviorSvc, Control control, Adorner glyphAdorner, ISelectionService selectionService, IComponentChangeService changeService)
			: this((Behavior)(object)new TabItemResizeBehavior(control as TabItem))
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			service = behaviorSvc;
			tab = control as TabItem;
			adorner = glyphAdorner;
			this.selectionService = selectionService;
			this.changeService = changeService;
			selectionService.add_SelectionChanged((EventHandler)OnSelectionChanged);
			changeService.add_ComponentChanged(new ComponentChangedEventHandler(OnComponentChanged));
		}

		private void ComputeBounds()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Invalid comparison between Unknown and I4
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			Point val = service.ControlToAdornerWindow((Control)(object)tab);
			TabAlignment val2 = (TabAlignment)0;
			if (tab.Owner != null)
			{
				val2 = tab.Owner.TabAlignment;
			}
			if ((int)val2 == 0 || (int)val2 == 1)
			{
				glyphBounds = new Rectangle(((Point)(ref val)).get_X() + tab.Width - 13 + 1, ((Point)(ref val)).get_Y() + tab.Height / 2 - 13, 26, 26);
			}
			else
			{
				glyphBounds = new Rectangle(((Point)(ref val)).get_X() + tab.Width / 2 - 13, ((Point)(ref val)).get_Y() + tab.Height - 13 + 1, 26, 26);
			}
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			if (selectionService.get_PrimarySelection() == tab)
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

		private void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
			if (e.get_Component() == tab && (e.get_Member().get_Name() == "TabWidth" || e.get_Member().get_Name() == "TabHeight" || e.get_Member().get_Name() == "Location"))
			{
				ComputeBounds();
				adorner.Invalidate();
			}
		}

		public override Cursor GetHitTest(Point p)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Invalid comparison between Unknown and I4
			Rectangle bounds = ((Glyph)this).get_Bounds();
			if (((Rectangle)(ref bounds)).Contains(p))
			{
				TabAlignment val = (TabAlignment)0;
				if (tab.Owner != null)
				{
					val = tab.Owner.TabAlignment;
				}
				if ((int)val == 0 || (int)val == 1)
				{
					return Cursors.get_SizeWE();
				}
				return Cursors.get_SizeNS();
			}
			return null;
		}

		public override void Paint(PaintEventArgs pe)
		{
		}
	}
}

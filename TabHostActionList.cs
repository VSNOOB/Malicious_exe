using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;

namespace TabControl
{
	public class TabHostActionList : DesignerActionList
	{
		private TabHost tabHost;

		private DesignerActionUIService service;

		public bool ShowCloseButtons
		{
			get
			{
				return tabHost.ShowCloseButtons;
			}
			set
			{
				GetPropertyByName("ShowCloseButtons").SetValue((object)tabHost, (object)value);
			}
		}

		public Color CloseButtonColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabHost.CloseButtonColor;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("CloseButtonColor").SetValue((object)tabHost, (object)value);
			}
		}

		public Color CloseButtonColorSelected
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabHost.CloseButtonColorSelected;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("CloseButtonColorSelected").SetValue((object)tabHost, (object)value);
			}
		}

		public Color CloseButtonOverColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabHost.CloseButtonOverColor;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("CloseButtonOverColor").SetValue((object)tabHost, (object)value);
			}
		}

		public Color CloseButtonOverColorSelected
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabHost.CloseButtonOverColorSelected;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("CloseButtonOverColorSelected").SetValue((object)tabHost, (object)value);
			}
		}

		public Color CloseButtonPressedColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabHost.CloseButtonPressedColor;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("CloseButtonPressedColor").SetValue((object)tabHost, (object)value);
			}
		}

		public CloseButtonAlignment CloseButtonAlignment
		{
			get
			{
				return tabHost.CloseButtonAlignment;
			}
			set
			{
				GetPropertyByName("CloseButtonAlignment").SetValue((object)tabHost, (object)value);
			}
		}

		public int CloseButtonBorderOpacity
		{
			get
			{
				return tabHost.CloseButtonBorderOpacity;
			}
			set
			{
				GetPropertyByName("CloseButtonBorderOpacity").SetValue((object)tabHost, (object)value);
			}
		}

		public int CloseButtonBorderOpacitySelected
		{
			get
			{
				return tabHost.CloseButtonBorderOpacitySelected;
			}
			set
			{
				GetPropertyByName("CloseButtonBorderOpacitySelected").SetValue((object)tabHost, (object)value);
			}
		}

		public TabItemCollection Tabs
		{
			get
			{
				return tabHost.Tabs;
			}
			set
			{
				tabHost.Tabs = value;
			}
		}

		private PropertyDescriptor GetPropertyByName(string name)
		{
			return TypeDescriptor.GetProperties((object)tabHost).get_Item(name);
		}

		public TabHostActionList(IComponent component)
			: this(component)
		{
			tabHost = component as TabHost;
			ref DesignerActionUIService val = ref service;
			object obj = ((DesignerActionList)this).GetService(typeof(DesignerActionUIService));
			val = obj as DesignerActionUIService;
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Expected O, but got Unknown
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Expected O, but got Unknown
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Expected O, but got Unknown
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Expected O, but got Unknown
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Expected O, but got Unknown
			DesignerActionItemCollection val = new DesignerActionItemCollection();
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Behavior"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("ShowCloseButtons", "Show Close Buttons", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonColor", "Color", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonColorSelected", "Color (selected)", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonOverColor", "Over Color", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonOverColorSelected", "Over Color (selected)", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonPressedColor", "Pressed Color", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonAlignment", "Alignment", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonBorderOpacity", "Border Opacity", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("CloseButtonBorderOpacitySelected", "Border Opacity (selected)", "Close Buttons"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("Tabs", "Tab Items", "Behavior"));
			return val;
		}
	}
}

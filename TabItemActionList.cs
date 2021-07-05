using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace TabControl
{
	public class TabItemActionList : DesignerActionList
	{
		private TabItem tabItem;

		private DesignerActionUIService service;

		private TabItemDesigner tabItemDesigner;

		public bool AutoEllipsis
		{
			get
			{
				return tabItem.AutoEllipsis;
			}
			set
			{
				GetPropertyByName("AutoEllipsis").SetValue((object)tabItem, (object)value);
			}
		}

		public Color BackColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)tabItem).get_BackColor();
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("BackColor").SetValue((object)tabItem, (object)value);
			}
		}

		public Color ForeColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)tabItem).get_ForeColor();
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("ForeColor").SetValue((object)tabItem, (object)value);
			}
		}

		public Color SelectedBackColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabItem.SelectedBackColor;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("SelectedBackColor").SetValue((object)tabItem, (object)value);
			}
		}

		public Color SelectedForeColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabItem.SelectedForeColor;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("SelectedForeColor").SetValue((object)tabItem, (object)value);
			}
		}

		public Color HighlightBackColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabItem.HighlightBackColor;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("HighlightBackColor").SetValue((object)tabItem, (object)value);
			}
		}

		public Color HighlightForeColor
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				return tabItem.HighlightForeColor;
			}
			set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				GetPropertyByName("HighlightForeColor").SetValue((object)tabItem, (object)value);
			}
		}

		public Image Image
		{
			get
			{
				return tabItem.Image;
			}
			set
			{
				GetPropertyByName("Image").SetValue((object)tabItem, (object)value);
			}
		}

		private PropertyDescriptor GetPropertyByName(string name)
		{
			return TypeDescriptor.GetProperties((object)tabItem).get_Item(name);
		}

		public TabItemActionList(IComponent component)
			: this(component)
		{
			tabItem = component as TabItem;
			ref DesignerActionUIService val = ref service;
			object obj = ((DesignerActionList)this).GetService(typeof(DesignerActionUIService));
			val = obj as DesignerActionUIService;
			object obj2 = ((IServiceProvider)((DesignerActionList)this).get_Component().get_Site()).GetService(typeof(IDesignerHost));
			IDesignerHost val2 = obj2 as IDesignerHost;
			tabItemDesigner = val2.GetDesigner(component) as TabItemDesigner;
		}

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Expected O, but got Unknown
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Expected O, but got Unknown
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Expected O, but got Unknown
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			DesignerActionItemCollection val = new DesignerActionItemCollection();
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Normal State"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Selected State"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Highlighted State"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Misc"));
			val.Add((DesignerActionItem)new DesignerActionHeaderItem("Tab Order"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("BackColor", "Back Color", "Normal State"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("ForeColor", "Fore Color", "Normal State"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("SelectedBackColor", "Back Color", "Selected State"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("SelectedForeColor", "Fore Color", "Selected State"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("HighlightBackColor", "Back Color", "Highlighted State"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("HighlightForeColor", "Fore Color", "Highlighted State"));
			val.Add((DesignerActionItem)new DesignerActionPropertyItem("Image", "Image", "Misc"));
			val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "MoveLeft", "Move Left", "Tab Order"));
			val.Add((DesignerActionItem)new DesignerActionMethodItem((DesignerActionList)(object)this, "MoveRight", "Move Right", "Tab Order"));
			return val;
		}

		public void MoveLeft()
		{
			TabHost owner = tabItem.Owner;
			if (owner != null)
			{
				int num = owner.Tabs.IndexOf(tabItem);
				if (num > 0)
				{
					int index = num - 1;
					owner.Tabs.RemoveAt(num);
					owner.Tabs.Insert(index, tabItem);
					((Control)owner).Refresh();
					TypeDescriptor.GetProperties((object)owner).get_Item("Tabs").SetValue((object)owner, (object)owner.Tabs);
					tabItemDesigner.ReselectTab();
					service.HideUI((IComponent)(object)tabItem);
					service.ShowUI((IComponent)(object)tabItem);
				}
			}
		}

		public void MoveRight()
		{
			TabHost owner = tabItem.Owner;
			if (owner != null)
			{
				int num = owner.Tabs.IndexOf(tabItem);
				if (num < owner.Tabs.Count - 1)
				{
					int index = num + 1;
					owner.Tabs.RemoveAt(num);
					owner.Tabs.Insert(index, tabItem);
					((Control)owner).Refresh();
					TypeDescriptor.GetProperties((object)owner).get_Item("Tabs").SetValue((object)owner, (object)owner.Tabs);
					tabItemDesigner.ReselectTab();
					service.HideUI((IComponent)(object)tabItem);
					service.ShowUI((IComponent)(object)tabItem);
				}
			}
		}
	}
}

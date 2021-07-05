using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace TabControl
{
	public class TabItemDesigner : ControlDesigner
	{
		private DesignerActionListCollection actionList;

		private ISelectionService selectionService;

		private IComponentChangeService changeService;

		private TabItem tabItem;

		private Adorner adorner;

		private Glyph selectionGlyph;

		private Glyph resizeGlyph;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				if (actionList == null)
				{
					actionList = new DesignerActionListCollection();
					actionList.Add((DesignerActionList)(object)new TabItemActionList(((ComponentDesigner)this).get_Component()));
				}
				return actionList;
			}
		}

		public override void Initialize(IComponent component)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			((ControlDesigner)this).Initialize(component);
			tabItem = component as TabItem;
			InitializeServices();
			adorner = new Adorner();
			((ControlDesigner)this).get_BehaviorService().get_Adorners().Add(adorner);
			((ControlDesigner)this).set_AutoResizeHandles(true);
			selectionGlyph = (Glyph)(object)new TabItemResizeGlyph(((ControlDesigner)this).get_BehaviorService(), (Control)(object)tabItem, adorner, selectionService, changeService);
			adorner.get_Glyphs().Add(selectionGlyph);
		}

		public void ReselectTab()
		{
			List<TabItem> list = new List<TabItem>(1);
			list.Add(tabItem);
			selectionService.SetSelectedComponents((ICollection)null);
			selectionService.SetSelectedComponents((ICollection)list);
		}

		private void InitializeServices()
		{
			ref ISelectionService val = ref selectionService;
			object service = ((ComponentDesigner)this).GetService(typeof(ISelectionService));
			val = service as ISelectionService;
			ref IComponentChangeService val2 = ref changeService;
			object service2 = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			val2 = service2 as IComponentChangeService;
		}

		public TabItemDesigner()
			: this()
		{
		}
	}
}

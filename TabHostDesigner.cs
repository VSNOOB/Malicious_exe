using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace TabControl
{
	public class TabHostDesigner : ControlDesigner
	{
		private DesignerActionListCollection actionList;

		private IEventBindingService eventBindingService;

		private IDesignerHost host;

		private IComponentChangeService changeService;

		private ISelectionService selectionService;

		private TabHost tabHost;

		private Adorner adorner;

		private TabHostGlyph hostGlyph;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Expected O, but got Unknown
				if (actionList == null)
				{
					actionList = new DesignerActionListCollection();
					actionList.Add((DesignerActionList)(object)new TabHostActionList(((ComponentDesigner)this).get_Component()));
				}
				return actionList;
			}
		}

		public override void Initialize(IComponent component)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Expected O, but got Unknown
			((ControlDesigner)this).Initialize(component);
			tabHost = component as TabHost;
			InitializeServices();
			adorner = new Adorner();
			((ControlDesigner)this).get_BehaviorService().get_Adorners().Add(adorner);
			hostGlyph = new TabHostGlyph(((ControlDesigner)this).get_BehaviorService(), (Control)(object)tabHost, adorner, selectionService);
			adorner.get_Glyphs().Add((Glyph)(object)hostGlyph);
			changeService.add_ComponentRemoved(new ComponentEventHandler(OnComponentRemoved));
		}

		protected override void Dispose(bool disposing)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			if (disposing && adorner != null)
			{
				BehaviorService behaviorService = ((ControlDesigner)this).get_BehaviorService();
				if (behaviorService != null && adorner != null)
				{
					behaviorService.get_Adorners().Remove(adorner);
				}
				changeService.remove_ComponentRemoved(new ComponentEventHandler(OnComponentRemoved));
			}
			((ControlDesigner)this).Dispose(disposing);
		}

		private void InitializeServices()
		{
			ref IDesignerHost val = ref host;
			object service = ((ComponentDesigner)this).GetService(typeof(IDesignerHost));
			val = service as IDesignerHost;
			ref IComponentChangeService val2 = ref changeService;
			object service2 = ((ComponentDesigner)this).GetService(typeof(IComponentChangeService));
			val2 = service2 as IComponentChangeService;
			ref ISelectionService val3 = ref selectionService;
			object service3 = ((ComponentDesigner)this).GetService(typeof(ISelectionService));
			val3 = service3 as ISelectionService;
		}

		private void OnComponentRemoved(object sender, ComponentEventArgs ce)
		{
			TabItem tabItem = ce.get_Component() as TabItem;
			if (tabItem != null)
			{
				tabHost.Tabs.Remove(tabItem);
				hostGlyph.ComputeBounds();
				adorner.Invalidate();
			}
		}

		public TabHostDesigner()
			: this()
		{
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	public class DockContent : Form, IDockContent
	{
		private DockContentHandler m_dockHandler = null;

		private static readonly object DockStateChangedEvent = new object();

		[Browsable(false)]
		public DockContentHandler DockHandler => m_dockHandler;

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_AllowEndUserDocking_Description")]
		[DefaultValue(true)]
		public bool AllowEndUserDocking
		{
			get
			{
				return DockHandler.AllowEndUserDocking;
			}
			set
			{
				DockHandler.AllowEndUserDocking = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_DockAreas_Description")]
		[DefaultValue(DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom | DockAreas.Document)]
		public DockAreas DockAreas
		{
			get
			{
				return DockHandler.DockAreas;
			}
			set
			{
				DockHandler.DockAreas = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_AutoHidePortion_Description")]
		[DefaultValue(0.25)]
		public double AutoHidePortion
		{
			get
			{
				return DockHandler.AutoHidePortion;
			}
			set
			{
				DockHandler.AutoHidePortion = value;
			}
		}

		[Localizable(true)]
		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_TabText_Description")]
		[DefaultValue(null)]
		public string TabText
		{
			get
			{
				return DockHandler.TabText;
			}
			set
			{
				DockHandler.TabText = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_CloseButton_Description")]
		[DefaultValue(true)]
		public bool CloseButton
		{
			get
			{
				return DockHandler.CloseButton;
			}
			set
			{
				DockHandler.CloseButton = value;
			}
		}

		[Browsable(false)]
		public DockPanel DockPanel
		{
			get
			{
				return DockHandler.DockPanel;
			}
			set
			{
				DockHandler.DockPanel = value;
			}
		}

		[Browsable(false)]
		public DockState DockState
		{
			get
			{
				return DockHandler.DockState;
			}
			set
			{
				DockHandler.DockState = value;
			}
		}

		[Browsable(false)]
		public DockPane Pane
		{
			get
			{
				return DockHandler.Pane;
			}
			set
			{
				DockHandler.Pane = value;
			}
		}

		[Browsable(false)]
		public bool IsHidden
		{
			get
			{
				return DockHandler.IsHidden;
			}
			set
			{
				DockHandler.IsHidden = value;
			}
		}

		[Browsable(false)]
		public DockState VisibleState
		{
			get
			{
				return DockHandler.VisibleState;
			}
			set
			{
				DockHandler.VisibleState = value;
			}
		}

		[Browsable(false)]
		public bool IsFloat
		{
			get
			{
				return DockHandler.IsFloat;
			}
			set
			{
				DockHandler.IsFloat = value;
			}
		}

		[Browsable(false)]
		public DockPane PanelPane
		{
			get
			{
				return DockHandler.PanelPane;
			}
			set
			{
				DockHandler.PanelPane = value;
			}
		}

		[Browsable(false)]
		public DockPane FloatPane
		{
			get
			{
				return DockHandler.FloatPane;
			}
			set
			{
				DockHandler.FloatPane = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_HideOnClose_Description")]
		[DefaultValue(false)]
		public bool HideOnClose
		{
			get
			{
				return DockHandler.HideOnClose;
			}
			set
			{
				DockHandler.HideOnClose = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_ShowHint_Description")]
		[DefaultValue(DockState.Unknown)]
		public DockState ShowHint
		{
			get
			{
				return DockHandler.ShowHint;
			}
			set
			{
				DockHandler.ShowHint = value;
			}
		}

		[Browsable(false)]
		public bool IsActivated => DockHandler.IsActivated;

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_TabPageContextMenu_Description")]
		[DefaultValue(null)]
		public ContextMenu TabPageContextMenu
		{
			get
			{
				return DockHandler.TabPageContextMenu;
			}
			set
			{
				DockHandler.TabPageContextMenu = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockContent_TabPageContextMenuStrip_Description")]
		[DefaultValue(null)]
		public ContextMenuStrip TabPageContextMenuStrip
		{
			get
			{
				return DockHandler.TabPageContextMenuStrip;
			}
			set
			{
				DockHandler.TabPageContextMenuStrip = value;
			}
		}

		[Localizable(true)]
		[Category("Appearance")]
		[LocalizedDescription("DockContent_ToolTipText_Description")]
		[DefaultValue(null)]
		public string ToolTipText
		{
			get
			{
				return DockHandler.ToolTipText;
			}
			set
			{
				DockHandler.ToolTipText = value;
			}
		}

		[LocalizedCategory("Category_PropertyChanged")]
		[LocalizedDescription("Pane_DockStateChanged_Description")]
		public event EventHandler DockStateChanged
		{
			add
			{
				((Component)this).get_Events().AddHandler(DockStateChangedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(DockStateChangedEvent, (Delegate)value);
			}
		}

		public DockContent()
			: this()
		{
			m_dockHandler = new DockContentHandler((Form)(object)this, GetPersistString);
			m_dockHandler.DockStateChanged += DockHandler_DockStateChanged;
		}

		private bool ShouldSerializeTabText()
		{
			return DockHandler.TabText != null;
		}

		protected virtual string GetPersistString()
		{
			return base.GetType().ToString();
		}

		public bool IsDockStateValid(DockState dockState)
		{
			return DockHandler.IsDockStateValid(dockState);
		}

		public void Activate()
		{
			DockHandler.Activate();
		}

		public void Hide()
		{
			DockHandler.Hide();
		}

		public void Show()
		{
			DockHandler.Show();
		}

		public void Show(DockPanel dockPanel)
		{
			DockHandler.Show(dockPanel);
		}

		public void Show(DockPanel dockPanel, DockState dockState)
		{
			DockHandler.Show(dockPanel, dockState);
		}

		public void Show(DockPanel dockPanel, Rectangle floatWindowBounds)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DockHandler.Show(dockPanel, floatWindowBounds);
		}

		public void Show(DockPane pane, IDockContent beforeContent)
		{
			DockHandler.Show(pane, beforeContent);
		}

		public void Show(DockPane previousPane, DockAlignment alignment, double proportion)
		{
			DockHandler.Show(previousPane, alignment, proportion);
		}

		public void FloatAt(Rectangle floatWindowBounds)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			DockHandler.FloatAt(floatWindowBounds);
		}

		public void DockTo(DockPane paneTo, DockStyle dockStyle, int contentIndex)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DockHandler.DockTo(paneTo, dockStyle, contentIndex);
		}

		public void DockTo(DockPanel panel, DockStyle dockStyle)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DockHandler.DockTo(panel, dockStyle);
		}

		private void DockHandler_DockStateChanged(object sender, EventArgs e)
		{
			OnDockStateChanged(e);
		}

		protected virtual void OnDockStateChanged(EventArgs e)
		{
			((EventHandler)((Component)this).get_Events().get_Item(DockStateChangedEvent))?.Invoke(this, e);
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking.Win32;

namespace WeifenLuo.WinFormsUI.Docking
{
	public class DockContentHandler : IDisposable, IDockDragSource, IDragSource
	{
		private Form m_form;

		private IDockContent m_previousActive = null;

		private IDockContent m_nextActive = null;

		private EventHandlerList m_events;

		private bool m_allowEndUserDocking = true;

		private double m_autoHidePortion = 0.25;

		private bool m_closeButton = true;

		private DockAreas m_allowedAreas = DockAreas.Float | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.DockBottom | DockAreas.Document;

		private DockState m_dockState = DockState.Unknown;

		private DockPanel m_dockPanel = null;

		private bool m_isHidden = true;

		private string m_tabText = null;

		private DockState m_visibleState = DockState.Unknown;

		private bool m_isFloat = false;

		private DockPane m_panelPane = null;

		private DockPane m_floatPane = null;

		private int m_countSetDockState = 0;

		private GetPersistStringCallback m_getPersistStringCallback = null;

		private bool m_hideOnClose = false;

		private DockState m_showHint = DockState.Unknown;

		private bool m_isActivated = false;

		private ContextMenu m_tabPageContextMenu = null;

		private string m_toolTipText = null;

		private IntPtr m_activeWindowHandle = IntPtr.Zero;

		private DockPaneStripBase.Tab m_tab = null;

		private IDisposable m_autoHideTab = null;

		private static readonly object DockStateChangedEvent = new object();

		private bool m_flagClipWindow = false;

		private ContextMenuStrip m_tabPageContextMenuStrip = null;

		public Form Form => m_form;

		public IDockContent Content => Form as IDockContent;

		public IDockContent PreviousActive
		{
			get
			{
				return m_previousActive;
			}
			internal set
			{
				m_previousActive = value;
			}
		}

		public IDockContent NextActive
		{
			get
			{
				return m_nextActive;
			}
			internal set
			{
				m_nextActive = value;
			}
		}

		private EventHandlerList Events => m_events;

		public bool AllowEndUserDocking
		{
			get
			{
				return m_allowEndUserDocking;
			}
			set
			{
				m_allowEndUserDocking = value;
			}
		}

		public double AutoHidePortion
		{
			get
			{
				return m_autoHidePortion;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentOutOfRangeException(Strings.DockContentHandler_AutoHidePortion_OutOfRange);
				}
				if (m_autoHidePortion != value)
				{
					m_autoHidePortion = value;
					if (DockPanel != null && DockPanel.ActiveAutoHideContent == Content)
					{
						((Control)DockPanel).PerformLayout();
					}
				}
			}
		}

		public bool CloseButton
		{
			get
			{
				return m_closeButton;
			}
			set
			{
				if (m_closeButton != value)
				{
					m_closeButton = value;
					if (Pane != null && Pane.ActiveContent.DockHandler == this)
					{
						Pane.RefreshChanges();
					}
				}
			}
		}

		private DockState DefaultDockState
		{
			get
			{
				if (ShowHint != 0 && ShowHint != DockState.Hidden)
				{
					return ShowHint;
				}
				if ((DockAreas & DockAreas.Document) != 0)
				{
					return DockState.Document;
				}
				if ((DockAreas & DockAreas.DockRight) != 0)
				{
					return DockState.DockRight;
				}
				if ((DockAreas & DockAreas.DockLeft) != 0)
				{
					return DockState.DockLeft;
				}
				if ((DockAreas & DockAreas.DockBottom) != 0)
				{
					return DockState.DockBottom;
				}
				if ((DockAreas & DockAreas.DockTop) != 0)
				{
					return DockState.DockTop;
				}
				return DockState.Unknown;
			}
		}

		private DockState DefaultShowState
		{
			get
			{
				if (ShowHint != 0)
				{
					return ShowHint;
				}
				if ((DockAreas & DockAreas.Document) != 0)
				{
					return DockState.Document;
				}
				if ((DockAreas & DockAreas.DockRight) != 0)
				{
					return DockState.DockRight;
				}
				if ((DockAreas & DockAreas.DockLeft) != 0)
				{
					return DockState.DockLeft;
				}
				if ((DockAreas & DockAreas.DockBottom) != 0)
				{
					return DockState.DockBottom;
				}
				if ((DockAreas & DockAreas.DockTop) != 0)
				{
					return DockState.DockTop;
				}
				if ((DockAreas & DockAreas.Float) != 0)
				{
					return DockState.Float;
				}
				return DockState.Unknown;
			}
		}

		public DockAreas DockAreas
		{
			get
			{
				return m_allowedAreas;
			}
			set
			{
				if (m_allowedAreas != value)
				{
					if (!DockHelper.IsDockStateValid(DockState, value))
					{
						throw new InvalidOperationException(Strings.DockContentHandler_DockAreas_InvalidValue);
					}
					m_allowedAreas = value;
					if (!DockHelper.IsDockStateValid(ShowHint, m_allowedAreas))
					{
						ShowHint = DockState.Unknown;
					}
				}
			}
		}

		public DockState DockState
		{
			get
			{
				return m_dockState;
			}
			set
			{
				if (m_dockState != value)
				{
					DockPanel.SuspendLayout(allWindows: true);
					if (value == DockState.Hidden)
					{
						IsHidden = true;
					}
					else
					{
						SetDockState(isHidden: false, value, Pane);
					}
					DockPanel.ResumeLayout(performLayout: true, allWindows: true);
				}
			}
		}

		public DockPanel DockPanel
		{
			get
			{
				return m_dockPanel;
			}
			set
			{
				if (m_dockPanel != value)
				{
					Pane = null;
					if (m_dockPanel != null)
					{
						m_dockPanel.RemoveContent(Content);
					}
					if (m_tab != null)
					{
						m_tab.Dispose();
						m_tab = null;
					}
					if (m_autoHideTab != null)
					{
						m_autoHideTab.Dispose();
						m_autoHideTab = null;
					}
					m_dockPanel = value;
					if (m_dockPanel != null)
					{
						m_dockPanel.AddContent(Content);
						Form.set_TopLevel(false);
						Form.set_FormBorderStyle((FormBorderStyle)0);
						Form.set_ShowInTaskbar(false);
						Form.set_WindowState((FormWindowState)0);
						NativeMethods.SetWindowPos(((Control)Form).get_Handle(), IntPtr.Zero, 0, 0, 0, 0, FlagsSetWindowPos.SWP_NOSIZE | FlagsSetWindowPos.SWP_NOMOVE | FlagsSetWindowPos.SWP_NOZORDER | FlagsSetWindowPos.SWP_NOACTIVATE | FlagsSetWindowPos.SWP_FRAMECHANGED | FlagsSetWindowPos.SWP_NOOWNERZORDER);
					}
				}
			}
		}

		public Icon Icon => Form.get_Icon();

		public DockPane Pane
		{
			get
			{
				return IsFloat ? FloatPane : PanelPane;
			}
			set
			{
				if (Pane != value)
				{
					DockPanel.SuspendLayout(allWindows: true);
					DockPane pane = Pane;
					SuspendSetDockState();
					FloatPane = ((value == null) ? null : (value.IsFloat ? value : FloatPane));
					PanelPane = ((value == null) ? null : (value.IsFloat ? PanelPane : value));
					ResumeSetDockState(IsHidden, value?.DockState ?? DockState.Unknown, pane);
					DockPanel.ResumeLayout(performLayout: true, allWindows: true);
				}
			}
		}

		public bool IsHidden
		{
			get
			{
				return m_isHidden;
			}
			set
			{
				if (m_isHidden != value)
				{
					SetDockState(value, VisibleState, Pane);
				}
			}
		}

		public string TabText
		{
			get
			{
				return (m_tabText == null) ? ((Control)Form).get_Text() : m_tabText;
			}
			set
			{
				if (!(m_tabText == value))
				{
					m_tabText = value;
					if (Pane != null)
					{
						Pane.RefreshChanges();
					}
				}
			}
		}

		public DockState VisibleState
		{
			get
			{
				return m_visibleState;
			}
			set
			{
				if (m_visibleState != value)
				{
					SetDockState(IsHidden, value, Pane);
				}
			}
		}

		public bool IsFloat
		{
			get
			{
				return m_isFloat;
			}
			set
			{
				if (m_isFloat != value)
				{
					DockState dockState = CheckDockState(value);
					if (dockState == DockState.Unknown)
					{
						throw new InvalidOperationException(Strings.DockContentHandler_IsFloat_InvalidValue);
					}
					SetDockState(IsHidden, dockState, Pane);
				}
			}
		}

		public DockPane PanelPane
		{
			get
			{
				return m_panelPane;
			}
			set
			{
				if (m_panelPane != value)
				{
					if (value != null && (value.IsFloat || value.DockPanel != DockPanel))
					{
						throw new InvalidOperationException(Strings.DockContentHandler_DockPane_InvalidValue);
					}
					DockPane pane = Pane;
					if (m_panelPane != null)
					{
						RemoveFromPane(m_panelPane);
					}
					m_panelPane = value;
					if (m_panelPane != null)
					{
						m_panelPane.AddContent(Content);
						SetDockState(IsHidden, IsFloat ? DockState.Float : m_panelPane.DockState, pane);
					}
					else
					{
						SetDockState(IsHidden, DockState.Unknown, pane);
					}
				}
			}
		}

		public DockPane FloatPane
		{
			get
			{
				return m_floatPane;
			}
			set
			{
				if (m_floatPane != value)
				{
					if (value != null && (!value.IsFloat || value.DockPanel != DockPanel))
					{
						throw new InvalidOperationException(Strings.DockContentHandler_FloatPane_InvalidValue);
					}
					DockPane pane = Pane;
					if (m_floatPane != null)
					{
						RemoveFromPane(m_floatPane);
					}
					m_floatPane = value;
					if (m_floatPane != null)
					{
						m_floatPane.AddContent(Content);
						SetDockState(IsHidden, IsFloat ? DockState.Float : VisibleState, pane);
					}
					else
					{
						SetDockState(IsHidden, DockState.Unknown, pane);
					}
				}
			}
		}

		internal bool IsSuspendSetDockState => m_countSetDockState != 0;

		internal string PersistString => (GetPersistStringCallback == null) ? ((object)Form).GetType().ToString() : GetPersistStringCallback();

		public GetPersistStringCallback GetPersistStringCallback
		{
			get
			{
				return m_getPersistStringCallback;
			}
			set
			{
				m_getPersistStringCallback = value;
			}
		}

		public bool HideOnClose
		{
			get
			{
				return m_hideOnClose;
			}
			set
			{
				m_hideOnClose = value;
			}
		}

		public DockState ShowHint
		{
			get
			{
				return m_showHint;
			}
			set
			{
				if (!DockHelper.IsDockStateValid(value, DockAreas))
				{
					throw new InvalidOperationException(Strings.DockContentHandler_ShowHint_InvalidValue);
				}
				if (m_showHint != value)
				{
					m_showHint = value;
				}
			}
		}

		public bool IsActivated
		{
			get
			{
				return m_isActivated;
			}
			internal set
			{
				if (m_isActivated != value)
				{
					m_isActivated = value;
				}
			}
		}

		public ContextMenu TabPageContextMenu
		{
			get
			{
				return m_tabPageContextMenu;
			}
			set
			{
				m_tabPageContextMenu = value;
			}
		}

		public string ToolTipText
		{
			get
			{
				return m_toolTipText;
			}
			set
			{
				m_toolTipText = value;
			}
		}

		internal IntPtr ActiveWindowHandle
		{
			get
			{
				return m_activeWindowHandle;
			}
			set
			{
				m_activeWindowHandle = value;
			}
		}

		internal IDisposable AutoHideTab
		{
			get
			{
				return m_autoHideTab;
			}
			set
			{
				m_autoHideTab = value;
			}
		}

		internal bool FlagClipWindow
		{
			get
			{
				return m_flagClipWindow;
			}
			set
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Expected O, but got Unknown
				if (m_flagClipWindow != value)
				{
					m_flagClipWindow = value;
					if (m_flagClipWindow)
					{
						((Control)Form).set_Region(new Region(Rectangle.Empty));
					}
					else
					{
						((Control)Form).set_Region((Region)null);
					}
				}
			}
		}

		public ContextMenuStrip TabPageContextMenuStrip
		{
			get
			{
				return m_tabPageContextMenuStrip;
			}
			set
			{
				m_tabPageContextMenuStrip = value;
			}
		}

		Control IDragSource.DragControl => (Control)(object)Form;

		public event EventHandler DockStateChanged
		{
			add
			{
				Events.AddHandler(DockStateChangedEvent, (Delegate)value);
			}
			remove
			{
				Events.RemoveHandler(DockStateChangedEvent, (Delegate)value);
			}
		}

		public DockContentHandler(Form form)
			: this(form, null)
		{
		}

		public DockContentHandler(Form form, GetPersistStringCallback getPersistStringCallback)
		{
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected O, but got Unknown
			if (!(form is IDockContent))
			{
				throw new ArgumentException(Strings.DockContent_Constructor_InvalidForm, "form");
			}
			m_form = form;
			m_getPersistStringCallback = getPersistStringCallback;
			m_events = new EventHandlerList();
			((Component)Form).add_Disposed((EventHandler)Form_Disposed);
			((Control)Form).add_TextChanged((EventHandler)Form_TextChanged);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			lock (this)
			{
				DockPanel = null;
				if (m_autoHideTab != null)
				{
					m_autoHideTab.Dispose();
				}
				if (m_tab != null)
				{
					m_tab.Dispose();
				}
				((Component)Form).remove_Disposed((EventHandler)Form_Disposed);
				((Control)Form).remove_TextChanged((EventHandler)Form_TextChanged);
				m_events.Dispose();
			}
		}

		public DockState CheckDockState(bool isFloat)
		{
			if (isFloat)
			{
				if (!IsDockStateValid(DockState.Float))
				{
					return DockState.Unknown;
				}
				return DockState.Float;
			}
			return (PanelPane != null) ? PanelPane.DockState : DefaultDockState;
		}

		private void RemoveFromPane(DockPane pane)
		{
			pane.RemoveContent(Content);
			SetPane(null);
			if (pane.Contents.Count == 0)
			{
				((Component)pane).Dispose();
			}
		}

		private void SuspendSetDockState()
		{
			m_countSetDockState++;
		}

		private void ResumeSetDockState()
		{
			m_countSetDockState--;
			if (m_countSetDockState < 0)
			{
				m_countSetDockState = 0;
			}
		}

		private void ResumeSetDockState(bool isHidden, DockState visibleState, DockPane oldPane)
		{
			ResumeSetDockState();
			SetDockState(isHidden, visibleState, oldPane);
		}

		internal void SetDockState(bool isHidden, DockState visibleState, DockPane oldPane)
		{
			if (IsSuspendSetDockState)
			{
				return;
			}
			if (DockPanel == null && visibleState != 0)
			{
				throw new InvalidOperationException(Strings.DockContentHandler_SetDockState_NullPanel);
			}
			if (visibleState switch
			{
				DockState.Unknown => false, 
				DockState.Hidden => true, 
				_ => !IsDockStateValid(visibleState), 
			})
			{
				throw new InvalidOperationException(Strings.DockContentHandler_SetDockState_InvalidState);
			}
			DockPanel dockPanel = DockPanel;
			dockPanel?.SuspendLayout(allWindows: true);
			SuspendSetDockState();
			DockState dockState = DockState;
			if (m_isHidden != isHidden || dockState == DockState.Unknown)
			{
				m_isHidden = isHidden;
			}
			m_visibleState = visibleState;
			m_dockState = (isHidden ? DockState.Hidden : visibleState);
			if (visibleState == DockState.Unknown)
			{
				Pane = null;
			}
			else
			{
				m_isFloat = m_visibleState == DockState.Float;
				if (Pane == null)
				{
					Pane = DockPanel.DockPaneFactory.CreateDockPane(Content, visibleState, show: true);
				}
				else if (Pane.DockState != visibleState)
				{
					if (Pane.Contents.Count == 1)
					{
						Pane.SetDockState(visibleState);
					}
					else
					{
						Pane = DockPanel.DockPaneFactory.CreateDockPane(Content, visibleState, show: true);
					}
				}
			}
			if (((Control)Form).get_ContainsFocus() && (DockState == DockState.Hidden || DockState == DockState.Unknown))
			{
				DockPanel.ContentFocusManager.GiveUpFocus(Content);
			}
			SetPaneAndVisible(Pane);
			if (oldPane != null && !((Control)oldPane).get_IsDisposed() && dockState == oldPane.DockState)
			{
				RefreshDockPane(oldPane);
			}
			if (Pane != null && DockState == Pane.DockState && (Pane != oldPane || (Pane == oldPane && dockState != oldPane.DockState)))
			{
				RefreshDockPane(Pane);
			}
			if (dockState != DockState)
			{
				if (DockState == DockState.Hidden || DockState == DockState.Unknown || DockHelper.IsDockStateAutoHide(DockState))
				{
					DockPanel.ContentFocusManager.RemoveFromList(Content);
				}
				else
				{
					DockPanel.ContentFocusManager.AddToList(Content);
				}
				OnDockStateChanged(EventArgs.Empty);
			}
			ResumeSetDockState();
			dockPanel?.ResumeLayout(performLayout: true, allWindows: true);
		}

		private static void RefreshDockPane(DockPane pane)
		{
			pane.RefreshChanges();
			pane.ValidateActiveContent();
		}

		public bool IsDockStateValid(DockState dockState)
		{
			if (DockPanel != null && dockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
			{
				return false;
			}
			return DockHelper.IsDockStateValid(dockState, DockAreas);
		}

		public void Activate()
		{
			if (DockPanel == null)
			{
				Form.Activate();
				return;
			}
			if (Pane == null)
			{
				Show(DockPanel);
				return;
			}
			IsHidden = false;
			Pane.ActiveContent = Content;
			if (DockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.SystemMdi)
			{
				Form.Activate();
				return;
			}
			if (DockHelper.IsDockStateAutoHide(DockState))
			{
				DockPanel.ActiveAutoHideContent = Content;
			}
			if (!((Control)Form).get_ContainsFocus())
			{
				DockPanel.ContentFocusManager.Activate(Content);
			}
		}

		public void GiveUpFocus()
		{
			DockPanel.ContentFocusManager.GiveUpFocus(Content);
		}

		public void Hide()
		{
			IsHidden = true;
		}

		internal void SetPaneAndVisible(DockPane pane)
		{
			SetPane(pane);
			SetVisible();
		}

		private void SetPane(DockPane pane)
		{
			if (pane != null && pane.DockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.DockingMdi)
			{
				if (((Control)Form).get_Parent() is DockPane)
				{
					SetParent(null);
				}
				if (Form.get_MdiParent() != DockPanel.ParentForm)
				{
					FlagClipWindow = true;
					Form.set_MdiParent(DockPanel.ParentForm);
				}
				return;
			}
			FlagClipWindow = true;
			if (Form.get_MdiParent() != null)
			{
				Form.set_MdiParent((Form)null);
			}
			if (Form.get_TopLevel())
			{
				Form.set_TopLevel(false);
			}
			SetParent((Control)(object)pane);
		}

		internal void SetVisible()
		{
			bool flag = !IsHidden && ((Pane != null && Pane.DockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.DockingMdi) || (Pane != null && Pane.ActiveContent == Content) || ((Pane == null || Pane.ActiveContent == Content) && ((Control)Form).get_Visible()));
			if (((Control)Form).get_Visible() != flag)
			{
				((Control)Form).set_Visible(flag);
			}
		}

		private void SetParent(Control value)
		{
			if (((Control)Form).get_Parent() != value)
			{
				bool flag = false;
				if (((Control)Form).get_ContainsFocus())
				{
					DockPanel.SaveFocus();
					flag = true;
				}
				((Control)Form).set_Parent(value);
				if (flag)
				{
					Activate();
				}
			}
		}

		public void Show()
		{
			if (DockPanel == null)
			{
				((Control)Form).Show();
			}
			else
			{
				Show(DockPanel);
			}
		}

		public void Show(DockPanel dockPanel)
		{
			if (dockPanel == null)
			{
				throw new ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel);
			}
			if (DockState == DockState.Unknown)
			{
				Show(dockPanel, DefaultShowState);
			}
			else
			{
				Activate();
			}
		}

		public void Show(DockPanel dockPanel, DockState dockState)
		{
			if (dockPanel == null)
			{
				throw new ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel);
			}
			if (dockState == DockState.Unknown || dockState == DockState.Hidden)
			{
				throw new ArgumentException(Strings.DockContentHandler_Show_InvalidDockState);
			}
			dockPanel.SuspendLayout(allWindows: true);
			DockPanel = dockPanel;
			if (dockState == DockState.Float && FloatPane == null)
			{
				Pane = DockPanel.DockPaneFactory.CreateDockPane(Content, DockState.Float, show: true);
			}
			else if (PanelPane == null)
			{
				DockPane dockPane = null;
				foreach (DockPane pane in DockPanel.Panes)
				{
					if (pane.DockState == dockState)
					{
						dockPane = pane;
						break;
					}
				}
				if (dockPane == null)
				{
					Pane = DockPanel.DockPaneFactory.CreateDockPane(Content, dockState, show: true);
				}
				else
				{
					Pane = dockPane;
				}
			}
			DockState = dockState;
			Activate();
			dockPanel.ResumeLayout(performLayout: true, allWindows: true);
		}

		public void Show(DockPanel dockPanel, Rectangle floatWindowBounds)
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			if (dockPanel == null)
			{
				throw new ArgumentNullException(Strings.DockContentHandler_Show_NullDockPanel);
			}
			dockPanel.SuspendLayout(allWindows: true);
			DockPanel = dockPanel;
			if (FloatPane == null)
			{
				IsHidden = true;
				FloatPane = DockPanel.DockPaneFactory.CreateDockPane(Content, DockState.Float, show: false);
				((Form)FloatPane.FloatWindow).set_StartPosition((FormStartPosition)0);
			}
			((Control)FloatPane.FloatWindow).set_Bounds(floatWindowBounds);
			Show(dockPanel, DockState.Float);
			Activate();
			dockPanel.ResumeLayout(performLayout: true, allWindows: true);
		}

		public void Show(DockPane pane, IDockContent beforeContent)
		{
			if (pane == null)
			{
				throw new ArgumentNullException(Strings.DockContentHandler_Show_NullPane);
			}
			if (beforeContent != null && pane.Contents.IndexOf(beforeContent) == -1)
			{
				throw new ArgumentException(Strings.DockContentHandler_Show_InvalidBeforeContent);
			}
			pane.DockPanel.SuspendLayout(allWindows: true);
			DockPanel = pane.DockPanel;
			Pane = pane;
			pane.SetContentIndex(Content, pane.Contents.IndexOf(beforeContent));
			Show();
			pane.DockPanel.ResumeLayout(performLayout: true, allWindows: true);
		}

		public void Show(DockPane previousPane, DockAlignment alignment, double proportion)
		{
			if (previousPane == null)
			{
				throw new ArgumentException(Strings.DockContentHandler_Show_InvalidPrevPane);
			}
			if (DockHelper.IsDockStateAutoHide(previousPane.DockState))
			{
				throw new ArgumentException(Strings.DockContentHandler_Show_InvalidPrevPane);
			}
			previousPane.DockPanel.SuspendLayout(allWindows: true);
			DockPanel = previousPane.DockPanel;
			DockPanel.DockPaneFactory.CreateDockPane(Content, previousPane, alignment, proportion, show: true);
			Show();
			previousPane.DockPanel.ResumeLayout(performLayout: true, allWindows: true);
		}

		public void Close()
		{
			DockPanel dockPanel = DockPanel;
			dockPanel?.SuspendLayout(allWindows: true);
			Form.Close();
			dockPanel?.ResumeLayout(performLayout: true, allWindows: true);
		}

		internal DockPaneStripBase.Tab GetTab(DockPaneStripBase dockPaneStrip)
		{
			if (m_tab == null)
			{
				m_tab = dockPaneStrip.CreateTab(Content);
			}
			return m_tab;
		}

		protected virtual void OnDockStateChanged(EventArgs e)
		{
			((EventHandler)Events.get_Item(DockStateChangedEvent))?.Invoke(this, e);
		}

		private void Form_Disposed(object sender, EventArgs e)
		{
			Dispose();
		}

		private void Form_TextChanged(object sender, EventArgs e)
		{
			if (DockHelper.IsDockStateAutoHide(DockState))
			{
				DockPanel.RefreshAutoHideStrip();
			}
			else if (Pane != null)
			{
				if (Pane.FloatWindow != null)
				{
					Pane.FloatWindow.SetText();
				}
				Pane.RefreshChanges();
			}
		}

		bool IDockDragSource.CanDockTo(DockPane pane)
		{
			if (!IsDockStateValid(pane.DockState))
			{
				return false;
			}
			if (Pane == pane && pane.DisplayingContents.Count == 1)
			{
				return false;
			}
			return true;
		}

		Rectangle IDockDragSource.BeginDrag(Point ptMouse)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			DockPane floatPane = FloatPane;
			Size val = ((DockState != DockState.Float && floatPane != null && floatPane.FloatWindow.NestedPanes.Count == 1) ? ((Form)floatPane.FloatWindow).get_Size() : DockPanel.DefaultFloatWindowSize);
			Rectangle clientRectangle = ((Control)Pane).get_ClientRectangle();
			Point val2 = default(Point);
			if (DockState == DockState.Document)
			{
				((Point)(ref val2))._002Ector(((Rectangle)(ref clientRectangle)).get_Left(), ((Rectangle)(ref clientRectangle)).get_Top());
			}
			else
			{
				((Point)(ref val2))._002Ector(((Rectangle)(ref clientRectangle)).get_Left(), ((Rectangle)(ref clientRectangle)).get_Bottom());
				((Point)(ref val2)).set_Y(((Point)(ref val2)).get_Y() - ((Size)(ref val)).get_Height());
			}
			val2 = ((Control)Pane).PointToScreen(val2);
			if (((Point)(ref ptMouse)).get_X() > ((Point)(ref val2)).get_X() + ((Size)(ref val)).get_Width())
			{
				((Point)(ref val2)).set_X(((Point)(ref val2)).get_X() + (((Point)(ref ptMouse)).get_X() - (((Point)(ref val2)).get_X() + ((Size)(ref val)).get_Width()) + 4));
			}
			return new Rectangle(val2, val);
		}

		public void FloatAt(Rectangle floatWindowBounds)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			DockPane dockPane = DockPanel.DockPaneFactory.CreateDockPane(Content, floatWindowBounds, show: true);
		}

		public void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Invalid comparison between Unknown and I4
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Invalid comparison between Unknown and I4
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Invalid comparison between Unknown and I4
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Invalid comparison between Unknown and I4
			if ((int)dockStyle == 5)
			{
				bool flag = Pane == pane;
				if (!flag)
				{
					Pane = pane;
				}
				if (contentIndex == -1 || !flag)
				{
					pane.SetContentIndex(Content, contentIndex);
					return;
				}
				DockContentCollection contents = pane.Contents;
				int num = contents.IndexOf(Content);
				int num2 = contentIndex;
				if (num < num2)
				{
					num2++;
					if (num2 > contents.Count - 1)
					{
						num2 = -1;
					}
				}
				pane.SetContentIndex(Content, num2);
			}
			else
			{
				DockPane dockPane = DockPanel.DockPaneFactory.CreateDockPane(Content, pane.DockState, show: true);
				INestedPanesContainer nestedPanesContainer = pane.NestedPanesContainer;
				if ((int)dockStyle == 3)
				{
					dockPane.DockTo(nestedPanesContainer, pane, DockAlignment.Left, 0.5);
				}
				else if ((int)dockStyle == 4)
				{
					dockPane.DockTo(nestedPanesContainer, pane, DockAlignment.Right, 0.5);
				}
				else if ((int)dockStyle == 1)
				{
					dockPane.DockTo(nestedPanesContainer, pane, DockAlignment.Top, 0.5);
				}
				else if ((int)dockStyle == 2)
				{
					dockPane.DockTo(nestedPanesContainer, pane, DockAlignment.Bottom, 0.5);
				}
				dockPane.DockState = pane.DockState;
			}
		}

		public void DockTo(DockPanel panel, DockStyle dockStyle)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Invalid comparison between Unknown and I4
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Invalid comparison between Unknown and I4
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Invalid comparison between Unknown and I4
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Invalid comparison between Unknown and I4
			if (panel != DockPanel)
			{
				throw new ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, "panel");
			}
			if ((int)dockStyle == 1)
			{
				DockPane dockPane = DockPanel.DockPaneFactory.CreateDockPane(Content, DockState.DockTop, show: true);
			}
			else if ((int)dockStyle == 2)
			{
				DockPane dockPane = DockPanel.DockPaneFactory.CreateDockPane(Content, DockState.DockBottom, show: true);
			}
			else if ((int)dockStyle == 3)
			{
				DockPane dockPane = DockPanel.DockPaneFactory.CreateDockPane(Content, DockState.DockLeft, show: true);
			}
			else if ((int)dockStyle == 4)
			{
				DockPane dockPane = DockPanel.DockPaneFactory.CreateDockPane(Content, DockState.DockRight, show: true);
			}
			else if ((int)dockStyle == 5)
			{
				DockPane dockPane = DockPanel.DockPaneFactory.CreateDockPane(Content, DockState.Document, show: true);
			}
		}
	}
}

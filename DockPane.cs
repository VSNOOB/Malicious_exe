using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	[ToolboxItem(false)]
	public class DockPane : UserControl, IDockDragSource, IDragSource
	{
		public enum AppearanceStyle
		{
			ToolWindow,
			Document
		}

		private enum HitTestArea
		{
			Caption,
			TabStrip,
			Content,
			None
		}

		private struct HitTestResult
		{
			public HitTestArea HitArea;

			public int Index;

			public HitTestResult(HitTestArea hitTestArea, int index)
			{
				HitArea = hitTestArea;
				Index = index;
			}
		}

		private class SplitterControl : Control, ISplitterDragSource, IDragSource
		{
			private DockPane m_pane;

			private DockAlignment m_alignment;

			public DockPane DockPane => m_pane;

			public DockAlignment Alignment
			{
				get
				{
					return m_alignment;
				}
				set
				{
					m_alignment = value;
					if (m_alignment == DockAlignment.Left || m_alignment == DockAlignment.Right)
					{
						((Control)this).set_Cursor(Cursors.get_VSplit());
					}
					else if (m_alignment == DockAlignment.Top || m_alignment == DockAlignment.Bottom)
					{
						((Control)this).set_Cursor(Cursors.get_HSplit());
					}
					else
					{
						((Control)this).set_Cursor(Cursors.get_Default());
					}
					if (DockPane.DockState == DockState.Document)
					{
						((Control)this).Invalidate();
					}
				}
			}

			bool ISplitterDragSource.IsVertical
			{
				get
				{
					NestedDockingStatus nestedDockingStatus = DockPane.NestedDockingStatus;
					return nestedDockingStatus.DisplayingAlignment == DockAlignment.Left || nestedDockingStatus.DisplayingAlignment == DockAlignment.Right;
				}
			}

			Rectangle ISplitterDragSource.DragLimitBounds
			{
				get
				{
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0073: Unknown result type (might be due to invalid IL or missing references)
					//IL_0074: Unknown result type (might be due to invalid IL or missing references)
					//IL_0077: Unknown result type (might be due to invalid IL or missing references)
					NestedDockingStatus nestedDockingStatus = DockPane.NestedDockingStatus;
					Rectangle result = ((Control)this).get_Parent().RectangleToScreen(nestedDockingStatus.LogicalBounds);
					if (((ISplitterDragSource)this).IsVertical)
					{
						((Rectangle)(ref result)).set_X(((Rectangle)(ref result)).get_X() + 24);
						((Rectangle)(ref result)).set_Width(((Rectangle)(ref result)).get_Width() - 48);
					}
					else
					{
						((Rectangle)(ref result)).set_Y(((Rectangle)(ref result)).get_Y() + 24);
						((Rectangle)(ref result)).set_Height(((Rectangle)(ref result)).get_Height() - 48);
					}
					return result;
				}
			}

			Control IDragSource.DragControl => (Control)(object)this;

			public SplitterControl(DockPane pane)
				: this()
			{
				((Control)this).SetStyle((ControlStyles)512, false);
				m_pane = pane;
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).OnPaint(e);
				if (DockPane.DockState == DockState.Document)
				{
					Graphics graphics = e.get_Graphics();
					Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
					if (Alignment == DockAlignment.Top || Alignment == DockAlignment.Bottom)
					{
						graphics.DrawLine(SystemPens.get_ControlDark(), ((Rectangle)(ref clientRectangle)).get_Left(), ((Rectangle)(ref clientRectangle)).get_Bottom() - 1, ((Rectangle)(ref clientRectangle)).get_Right(), ((Rectangle)(ref clientRectangle)).get_Bottom() - 1);
					}
					else if (Alignment == DockAlignment.Left || Alignment == DockAlignment.Right)
					{
						graphics.DrawLine(SystemPens.get_ControlDarkDark(), ((Rectangle)(ref clientRectangle)).get_Right() - 1, ((Rectangle)(ref clientRectangle)).get_Top(), ((Rectangle)(ref clientRectangle)).get_Right() - 1, ((Rectangle)(ref clientRectangle)).get_Bottom());
					}
				}
			}

			protected override void OnMouseDown(MouseEventArgs e)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Invalid comparison between Unknown and I4
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).OnMouseDown(e);
				if ((int)e.get_Button() == 1048576)
				{
					DockPane.DockPanel.BeginDrag(this, ((Control)this).get_Parent().RectangleToScreen(((Control)this).get_Bounds()));
				}
			}

			void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
			{
			}

			void ISplitterDragSource.EndDrag()
			{
			}

			void ISplitterDragSource.MoveSplitter(int offset)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				NestedDockingStatus nestedDockingStatus = DockPane.NestedDockingStatus;
				double proportion = nestedDockingStatus.Proportion;
				Rectangle logicalBounds = nestedDockingStatus.LogicalBounds;
				int num;
				if (((Rectangle)(ref logicalBounds)).get_Width() > 0)
				{
					logicalBounds = nestedDockingStatus.LogicalBounds;
					num = ((((Rectangle)(ref logicalBounds)).get_Height() <= 0) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				if (num == 0)
				{
					if (nestedDockingStatus.DisplayingAlignment == DockAlignment.Left)
					{
						double num2 = proportion;
						double num3 = offset;
						logicalBounds = nestedDockingStatus.LogicalBounds;
						proportion = num2 + num3 / (double)((Rectangle)(ref logicalBounds)).get_Width();
					}
					else if (nestedDockingStatus.DisplayingAlignment == DockAlignment.Right)
					{
						double num4 = proportion;
						double num5 = offset;
						logicalBounds = nestedDockingStatus.LogicalBounds;
						proportion = num4 - num5 / (double)((Rectangle)(ref logicalBounds)).get_Width();
					}
					else if (nestedDockingStatus.DisplayingAlignment == DockAlignment.Top)
					{
						double num6 = proportion;
						double num7 = offset;
						logicalBounds = nestedDockingStatus.LogicalBounds;
						proportion = num6 + num7 / (double)((Rectangle)(ref logicalBounds)).get_Height();
					}
					else
					{
						double num8 = proportion;
						double num9 = offset;
						logicalBounds = nestedDockingStatus.LogicalBounds;
						proportion = num8 - num9 / (double)((Rectangle)(ref logicalBounds)).get_Height();
					}
					DockPane.SetNestedDockingProportion(proportion);
				}
			}
		}

		private DockPaneCaptionBase m_captionControl;

		private DockPaneStripBase m_tabStripControl;

		private IDockContent m_activeContent = null;

		private bool m_allowDockDragAndDrop = true;

		private IDisposable m_autoHidePane = null;

		private object m_autoHideTabs = null;

		private DockContentCollection m_contents;

		private DockContentCollection m_displayingContents;

		private DockPanel m_dockPanel;

		private bool m_isActivated = false;

		private bool m_isActiveDocumentPane = false;

		private bool m_isHidden = true;

		private static readonly object DockStateChangedEvent = new object();

		private static readonly object IsActivatedChangedEvent = new object();

		private static readonly object IsActiveDocumentPaneChangedEvent = new object();

		private NestedDockingStatus m_nestedDockingStatus;

		private bool m_isFloat;

		private DockState m_dockState = DockState.Unknown;

		private int m_countRefreshStateChange = 0;

		private SplitterControl m_splitter;

		private DockPaneCaptionBase CaptionControl => m_captionControl;

		internal DockPaneStripBase TabStripControl => m_tabStripControl;

		public virtual IDockContent ActiveContent
		{
			get
			{
				return m_activeContent;
			}
			set
			{
				if (ActiveContent == value)
				{
					return;
				}
				if (value != null)
				{
					if (!DisplayingContents.Contains(value))
					{
						throw new InvalidOperationException(Strings.DockPane_ActiveContent_InvalidValue);
					}
				}
				else if (DisplayingContents.Count != 0)
				{
					throw new InvalidOperationException(Strings.DockPane_ActiveContent_InvalidValue);
				}
				IDockContent activeContent = m_activeContent;
				if (DockPanel.ActiveAutoHideContent == activeContent)
				{
					DockPanel.ActiveAutoHideContent = null;
				}
				m_activeContent = value;
				if (DockPanel.DocumentStyle == DocumentStyle.DockingMdi && DockState == DockState.Document)
				{
					if (m_activeContent != null)
					{
						((Control)m_activeContent.DockHandler.Form).BringToFront();
					}
				}
				else
				{
					if (m_activeContent != null)
					{
						m_activeContent.DockHandler.SetVisible();
					}
					if (activeContent != null && DisplayingContents.Contains(activeContent))
					{
						activeContent.DockHandler.SetVisible();
					}
				}
				if (FloatWindow != null)
				{
					FloatWindow.SetText();
				}
				if (DockPanel.DocumentStyle == DocumentStyle.DockingMdi && DockState == DockState.Document)
				{
					RefreshChanges(performLayout: false);
				}
				else
				{
					RefreshChanges();
				}
				if (m_activeContent != null)
				{
					TabStripControl.EnsureTabVisible(m_activeContent);
				}
			}
		}

		public virtual bool AllowDockDragAndDrop
		{
			get
			{
				return m_allowDockDragAndDrop;
			}
			set
			{
				m_allowDockDragAndDrop = value;
			}
		}

		internal IDisposable AutoHidePane
		{
			get
			{
				return m_autoHidePane;
			}
			set
			{
				m_autoHidePane = value;
			}
		}

		internal object AutoHideTabs
		{
			get
			{
				return m_autoHideTabs;
			}
			set
			{
				m_autoHideTabs = value;
			}
		}

		private object TabPageContextMenu
		{
			get
			{
				IDockContent activeContent = ActiveContent;
				if (activeContent == null)
				{
					return null;
				}
				if (activeContent.DockHandler.TabPageContextMenuStrip != null)
				{
					return activeContent.DockHandler.TabPageContextMenuStrip;
				}
				if (activeContent.DockHandler.TabPageContextMenu != null)
				{
					return activeContent.DockHandler.TabPageContextMenu;
				}
				return null;
			}
		}

		internal bool HasTabPageContextMenu => TabPageContextMenu != null;

		private Rectangle CaptionRectangle
		{
			get
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				if (!HasCaption)
				{
					return Rectangle.Empty;
				}
				Rectangle displayingRectangle = DisplayingRectangle;
				int x = ((Rectangle)(ref displayingRectangle)).get_X();
				int y = ((Rectangle)(ref displayingRectangle)).get_Y();
				int width = ((Rectangle)(ref displayingRectangle)).get_Width();
				int num = CaptionControl.MeasureHeight();
				return new Rectangle(x, y, width, num);
			}
		}

		internal Rectangle ContentRectangle
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0078: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0081: Unknown result type (might be due to invalid IL or missing references)
				Rectangle displayingRectangle = DisplayingRectangle;
				Rectangle captionRectangle = CaptionRectangle;
				Rectangle tabStripRectangle = TabStripRectangle;
				int x = ((Rectangle)(ref displayingRectangle)).get_X();
				int num = ((Rectangle)(ref displayingRectangle)).get_Y() + ((!((Rectangle)(ref captionRectangle)).get_IsEmpty()) ? ((Rectangle)(ref captionRectangle)).get_Height() : 0) + ((DockState == DockState.Document) ? ((Rectangle)(ref tabStripRectangle)).get_Height() : 0);
				int width = ((Rectangle)(ref displayingRectangle)).get_Width();
				int num2 = ((Rectangle)(ref displayingRectangle)).get_Height() - ((Rectangle)(ref captionRectangle)).get_Height() - ((Rectangle)(ref tabStripRectangle)).get_Height();
				return new Rectangle(x, num, width, num2);
			}
		}

		internal Rectangle TabStripRectangle
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				if (Appearance == AppearanceStyle.ToolWindow)
				{
					return TabStripRectangle_ToolWindow;
				}
				return TabStripRectangle_Document;
			}
		}

		private Rectangle TabStripRectangle_ToolWindow
		{
			get
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				if (DisplayingContents.Count <= 1 || IsAutoHide)
				{
					return Rectangle.Empty;
				}
				Rectangle displayingRectangle = DisplayingRectangle;
				int width = ((Rectangle)(ref displayingRectangle)).get_Width();
				int num = TabStripControl.MeasureHeight();
				int x = ((Rectangle)(ref displayingRectangle)).get_X();
				int num2 = ((Rectangle)(ref displayingRectangle)).get_Bottom() - num;
				Rectangle captionRectangle = CaptionRectangle;
				if (((Rectangle)(ref captionRectangle)).Contains(x, num2))
				{
					num2 = ((Rectangle)(ref captionRectangle)).get_Y() + ((Rectangle)(ref captionRectangle)).get_Height();
				}
				return new Rectangle(x, num2, width, num);
			}
		}

		private Rectangle TabStripRectangle_Document
		{
			get
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0082: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				if (DisplayingContents.Count == 0)
				{
					return Rectangle.Empty;
				}
				if (DisplayingContents.Count == 1 && DockPanel.DocumentStyle == DocumentStyle.DockingSdi)
				{
					return Rectangle.Empty;
				}
				Rectangle displayingRectangle = DisplayingRectangle;
				int x = ((Rectangle)(ref displayingRectangle)).get_X();
				int y = ((Rectangle)(ref displayingRectangle)).get_Y();
				int width = ((Rectangle)(ref displayingRectangle)).get_Width();
				int num = TabStripControl.MeasureHeight();
				return new Rectangle(x, y, width, num);
			}
		}

		public virtual string CaptionText => (ActiveContent == null) ? string.Empty : ActiveContent.DockHandler.TabText;

		public DockContentCollection Contents => m_contents;

		public DockContentCollection DisplayingContents => m_displayingContents;

		public DockPanel DockPanel => m_dockPanel;

		private bool HasCaption
		{
			get
			{
				if (DockState == DockState.Document || DockState == DockState.Hidden || DockState == DockState.Unknown || (DockState == DockState.Float && FloatWindow.VisibleNestedPanes.Count <= 1))
				{
					return false;
				}
				return true;
			}
		}

		public bool IsActivated => m_isActivated;

		public bool IsActiveDocumentPane => m_isActiveDocumentPane;

		public bool IsAutoHide => DockHelper.IsDockStateAutoHide(DockState);

		public AppearanceStyle Appearance => (DockState == DockState.Document) ? AppearanceStyle.Document : AppearanceStyle.ToolWindow;

		internal Rectangle DisplayingRectangle => ((Control)this).get_ClientRectangle();

		public bool IsHidden => m_isHidden;

		public DockWindow DockWindow
		{
			get
			{
				return (m_nestedDockingStatus.NestedPanes == null) ? null : (m_nestedDockingStatus.NestedPanes.Container as DockWindow);
			}
			set
			{
				DockWindow dockWindow = DockWindow;
				if (dockWindow != value)
				{
					DockTo(value);
				}
			}
		}

		public FloatWindow FloatWindow
		{
			get
			{
				return (m_nestedDockingStatus.NestedPanes == null) ? null : (m_nestedDockingStatus.NestedPanes.Container as FloatWindow);
			}
			set
			{
				FloatWindow floatWindow = FloatWindow;
				if (floatWindow != value)
				{
					DockTo(value);
				}
			}
		}

		public NestedDockingStatus NestedDockingStatus => m_nestedDockingStatus;

		public bool IsFloat => m_isFloat;

		public INestedPanesContainer NestedPanesContainer
		{
			get
			{
				if (NestedDockingStatus.NestedPanes == null)
				{
					return null;
				}
				return NestedDockingStatus.NestedPanes.Container;
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
				SetDockState(value);
			}
		}

		private bool IsRefreshStateChangeSuspended => m_countRefreshStateChange != 0;

		Control IDragSource.DragControl => (Control)(object)this;

		private SplitterControl Splitter => m_splitter;

		internal Rectangle SplitterBounds
		{
			set
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				((Control)Splitter).set_Bounds(value);
			}
		}

		internal DockAlignment SplitterAlignment
		{
			set
			{
				Splitter.Alignment = value;
			}
		}

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

		public event EventHandler IsActivatedChanged
		{
			add
			{
				((Component)this).get_Events().AddHandler(IsActivatedChangedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(IsActivatedChangedEvent, (Delegate)value);
			}
		}

		public event EventHandler IsActiveDocumentPaneChanged
		{
			add
			{
				((Component)this).get_Events().AddHandler(IsActiveDocumentPaneChangedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(IsActiveDocumentPaneChangedEvent, (Delegate)value);
			}
		}

		protected internal DockPane(IDockContent content, DockState visibleState, bool show)
			: this()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			InternalConstruct(content, visibleState, flagBounds: false, Rectangle.Empty, null, DockAlignment.Right, 0.5, show);
		}

		protected internal DockPane(IDockContent content, FloatWindow floatWindow, bool show)
			: this()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			if (floatWindow == null)
			{
				throw new ArgumentNullException("floatWindow");
			}
			InternalConstruct(content, DockState.Float, flagBounds: false, Rectangle.Empty, floatWindow.NestedPanes.GetDefaultPreviousPane(this), DockAlignment.Right, 0.5, show);
		}

		protected internal DockPane(IDockContent content, DockPane previousPane, DockAlignment alignment, double proportion, bool show)
			: this()
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			if (previousPane == null)
			{
				throw new ArgumentNullException("previousPane");
			}
			InternalConstruct(content, previousPane.DockState, flagBounds: false, Rectangle.Empty, previousPane, alignment, proportion, show);
		}

		protected internal DockPane(IDockContent content, Rectangle floatWindowBounds, bool show)
			: this()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			InternalConstruct(content, DockState.Float, flagBounds: true, floatWindowBounds, null, DockAlignment.Right, 0.5, show);
		}

		private void InternalConstruct(IDockContent content, DockState dockState, bool flagBounds, Rectangle floatWindowBounds, DockPane prevPane, DockAlignment alignment, double proportion, bool show)
		{
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			if (dockState == DockState.Hidden || dockState == DockState.Unknown)
			{
				throw new ArgumentException(Strings.DockPane_SetDockState_InvalidState);
			}
			if (content == null)
			{
				throw new ArgumentNullException(Strings.DockPane_Constructor_NullContent);
			}
			if (content.DockHandler.DockPanel == null)
			{
				throw new ArgumentException(Strings.DockPane_Constructor_NullDockPanel);
			}
			((Control)this).SuspendLayout();
			((Control)this).SetStyle((ControlStyles)512, false);
			m_isFloat = dockState == DockState.Float;
			m_contents = new DockContentCollection();
			m_displayingContents = new DockContentCollection(this);
			m_dockPanel = content.DockHandler.DockPanel;
			m_dockPanel.AddPane(this);
			m_splitter = new SplitterControl(this);
			m_nestedDockingStatus = new NestedDockingStatus(this);
			m_captionControl = DockPanel.DockPaneCaptionFactory.CreateDockPaneCaption(this);
			m_tabStripControl = DockPanel.DockPaneStripFactory.CreateDockPaneStrip(this);
			((Control)this).get_Controls().AddRange((Control[])(object)new Control[2]
			{
				m_captionControl,
				m_tabStripControl
			});
			DockPanel.SuspendLayout(allWindows: true);
			if (flagBounds)
			{
				FloatWindow = DockPanel.FloatWindowFactory.CreateFloatWindow(DockPanel, this, floatWindowBounds);
			}
			else if (prevPane != null)
			{
				DockTo(prevPane.NestedPanesContainer, prevPane, alignment, proportion);
			}
			SetDockState(dockState);
			if (show)
			{
				content.DockHandler.Pane = this;
			}
			else if (IsFloat)
			{
				content.DockHandler.FloatPane = this;
			}
			else
			{
				content.DockHandler.PanelPane = this;
			}
			((Control)this).ResumeLayout();
			DockPanel.ResumeLayout(performLayout: true, allWindows: true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_dockState = DockState.Unknown;
				if (NestedPanesContainer != null)
				{
					NestedPanesContainer.NestedPanes.Remove(this);
				}
				if (DockPanel != null)
				{
					DockPanel.RemovePane(this);
					m_dockPanel = null;
				}
				((Component)Splitter).Dispose();
				if (m_autoHidePane != null)
				{
					m_autoHidePane.Dispose();
				}
			}
			((ContainerControl)this).Dispose(disposing);
		}

		internal void ShowTabPageContextMenu(Control control, Point position)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			object tabPageContextMenu = TabPageContextMenu;
			if (tabPageContextMenu == null)
			{
				return;
			}
			ContextMenuStrip val = tabPageContextMenu as ContextMenuStrip;
			if (val != null)
			{
				((ToolStripDropDown)val).Show(control, position);
				return;
			}
			ContextMenu val2 = tabPageContextMenu as ContextMenu;
			if (val2 != null)
			{
				val2.Show((Control)(object)this, position);
			}
		}

		internal void SetIsActivated(bool value)
		{
			if (m_isActivated != value)
			{
				m_isActivated = value;
				if (DockState != DockState.Document)
				{
					RefreshChanges(performLayout: false);
				}
				OnIsActivatedChanged(EventArgs.Empty);
			}
		}

		internal void SetIsActiveDocumentPane(bool value)
		{
			if (m_isActiveDocumentPane != value)
			{
				m_isActiveDocumentPane = value;
				if (DockState == DockState.Document)
				{
					RefreshChanges();
				}
				OnIsActiveDocumentPaneChanged(EventArgs.Empty);
			}
		}

		public bool IsDockStateValid(DockState dockState)
		{
			foreach (IDockContent content in Contents)
			{
				if (!content.DockHandler.IsDockStateValid(dockState))
				{
					return false;
				}
			}
			return true;
		}

		public void Activate()
		{
			if (DockHelper.IsDockStateAutoHide(DockState) && DockPanel.ActiveAutoHideContent != ActiveContent)
			{
				DockPanel.ActiveAutoHideContent = ActiveContent;
			}
			else if (!IsActivated && ActiveContent != null)
			{
				ActiveContent.DockHandler.Activate();
			}
		}

		internal void AddContent(IDockContent content)
		{
			if (!Contents.Contains(content))
			{
				Contents.Add(content);
			}
		}

		internal void Close()
		{
			((Component)this).Dispose();
		}

		public void CloseActiveContent()
		{
			CloseContent(ActiveContent);
		}

		internal void CloseContent(IDockContent content)
		{
			DockPanel dockPanel = DockPanel;
			dockPanel.SuspendLayout(allWindows: true);
			if (content != null && content.DockHandler.CloseButton)
			{
				if (content.DockHandler.HideOnClose)
				{
					content.DockHandler.Hide();
				}
				else
				{
					content.DockHandler.Close();
				}
				dockPanel.ResumeLayout(performLayout: true, allWindows: true);
			}
		}

		private HitTestResult GetHitTest(Point ptMouse)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			Point val = ((Control)this).PointToClient(ptMouse);
			Rectangle captionRectangle = CaptionRectangle;
			if (((Rectangle)(ref captionRectangle)).Contains(val))
			{
				return new HitTestResult(HitTestArea.Caption, -1);
			}
			Rectangle contentRectangle = ContentRectangle;
			if (((Rectangle)(ref contentRectangle)).Contains(val))
			{
				return new HitTestResult(HitTestArea.Content, -1);
			}
			Rectangle tabStripRectangle = TabStripRectangle;
			if (((Rectangle)(ref tabStripRectangle)).Contains(val))
			{
				return new HitTestResult(HitTestArea.TabStrip, TabStripControl.HitTest(((Control)TabStripControl).PointToClient(ptMouse)));
			}
			return new HitTestResult(HitTestArea.None, -1);
		}

		private void SetIsHidden(bool value)
		{
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (m_isHidden != value)
			{
				m_isHidden = value;
				if (DockHelper.IsDockStateAutoHide(DockState))
				{
					DockPanel.RefreshAutoHideStrip();
					((Control)DockPanel).PerformLayout();
				}
				else if (NestedPanesContainer != null)
				{
					((Control)NestedPanesContainer).PerformLayout();
				}
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			SetIsHidden(DisplayingContents.Count == 0);
			if (!IsHidden)
			{
				((Control)CaptionControl).set_Bounds(CaptionRectangle);
				((Control)TabStripControl).set_Bounds(TabStripRectangle);
				SetContentBounds();
				foreach (IDockContent content in Contents)
				{
					if (DisplayingContents.Contains(content) && content.DockHandler.FlagClipWindow && ((Control)content.DockHandler.Form).get_Visible())
					{
						content.DockHandler.FlagClipWindow = false;
					}
				}
			}
			((ContainerControl)this).OnLayout(levent);
		}

		internal void SetContentBounds()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = ContentRectangle;
			if (DockState == DockState.Document && DockPanel.DocumentStyle == DocumentStyle.DockingMdi)
			{
				val = DockPanel.RectangleToMdiClient(((Control)this).RectangleToScreen(val));
			}
			Rectangle bounds = default(Rectangle);
			((Rectangle)(ref bounds))._002Ector(-((Rectangle)(ref val)).get_Width(), ((Rectangle)(ref val)).get_Y(), ((Rectangle)(ref val)).get_Width(), ((Rectangle)(ref val)).get_Height());
			foreach (IDockContent content in Contents)
			{
				if (content.DockHandler.Pane == this)
				{
					if (content == ActiveContent)
					{
						((Control)content.DockHandler.Form).set_Bounds(val);
					}
					else
					{
						((Control)content.DockHandler.Form).set_Bounds(bounds);
					}
				}
			}
		}

		internal void RefreshChanges()
		{
			RefreshChanges(performLayout: true);
		}

		private void RefreshChanges(bool performLayout)
		{
			if (!((Control)this).get_IsDisposed())
			{
				CaptionControl.RefreshChanges();
				TabStripControl.RefreshChanges();
				if (DockState == DockState.Float)
				{
					FloatWindow.RefreshChanges();
				}
				if (DockHelper.IsDockStateAutoHide(DockState) && DockPanel != null)
				{
					DockPanel.RefreshAutoHideStrip();
					((Control)DockPanel).PerformLayout();
				}
				if (performLayout)
				{
					((Control)this).PerformLayout();
				}
			}
		}

		internal void RemoveContent(IDockContent content)
		{
			if (Contents.Contains(content))
			{
				Contents.Remove(content);
			}
		}

		public void SetContentIndex(IDockContent content, int index)
		{
			int num = Contents.IndexOf(content);
			if (num == -1)
			{
				throw new ArgumentException(Strings.DockPane_SetContentIndex_InvalidContent);
			}
			if ((index < 0 || index > Contents.Count - 1) && index != -1)
			{
				throw new ArgumentOutOfRangeException(Strings.DockPane_SetContentIndex_InvalidIndex);
			}
			if (num != index && (num != Contents.Count - 1 || index != -1))
			{
				Contents.Remove(content);
				if (index == -1)
				{
					Contents.Add(content);
				}
				else if (num < index)
				{
					Contents.AddAt(content, index - 1);
				}
				else
				{
					Contents.AddAt(content, index);
				}
				RefreshChanges();
			}
		}

		private void SetParent()
		{
			if (DockState == DockState.Unknown || DockState == DockState.Hidden)
			{
				SetParent(null);
				((Control)Splitter).set_Parent((Control)null);
			}
			else if (DockState == DockState.Float)
			{
				SetParent((Control)(object)FloatWindow);
				((Control)Splitter).set_Parent((Control)(object)FloatWindow);
			}
			else if (DockHelper.IsDockStateAutoHide(DockState))
			{
				SetParent(DockPanel.AutoHideControl);
				((Control)Splitter).set_Parent((Control)null);
			}
			else
			{
				SetParent((Control)(object)DockPanel.DockWindows[DockState]);
				((Control)Splitter).set_Parent(((Control)this).get_Parent());
			}
		}

		private void SetParent(Control value)
		{
			if (((Control)this).get_Parent() != value)
			{
				IDockContent focusedContent = GetFocusedContent();
				if (focusedContent != null)
				{
					DockPanel.SaveFocus();
				}
				((Control)this).set_Parent(value);
				focusedContent?.DockHandler.Activate();
			}
		}

		public void Show()
		{
			Activate();
		}

		internal void TestDrop(IDockDragSource dragSource, DockOutlineBase dockOutline)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (dragSource.CanDockTo(this))
			{
				Point mousePosition = Control.get_MousePosition();
				HitTestResult hitTest = GetHitTest(mousePosition);
				if (hitTest.HitArea == HitTestArea.Caption)
				{
					dockOutline.Show(this, -1);
				}
				else if (hitTest.HitArea == HitTestArea.TabStrip && hitTest.Index != -1)
				{
					dockOutline.Show(this, hitTest.Index);
				}
			}
		}

		internal void ValidateActiveContent()
		{
			if (ActiveContent == null)
			{
				if (DisplayingContents.Count != 0)
				{
					ActiveContent = DisplayingContents[0];
				}
			}
			else
			{
				if (DisplayingContents.IndexOf(ActiveContent) >= 0)
				{
					return;
				}
				IDockContent dockContent = null;
				for (int num = Contents.IndexOf(ActiveContent) - 1; num >= 0; num--)
				{
					if (Contents[num].DockHandler.DockState == DockState)
					{
						dockContent = Contents[num];
						break;
					}
				}
				IDockContent dockContent2 = null;
				for (int i = Contents.IndexOf(ActiveContent) + 1; i < Contents.Count; i++)
				{
					if (Contents[i].DockHandler.DockState == DockState)
					{
						dockContent2 = Contents[i];
						break;
					}
				}
				if (dockContent != null)
				{
					ActiveContent = dockContent;
				}
				else if (dockContent2 != null)
				{
					ActiveContent = dockContent2;
				}
				else
				{
					ActiveContent = null;
				}
			}
		}

		protected virtual void OnDockStateChanged(EventArgs e)
		{
			((EventHandler)((Component)this).get_Events().get_Item(DockStateChangedEvent))?.Invoke(this, e);
		}

		protected virtual void OnIsActivatedChanged(EventArgs e)
		{
			((EventHandler)((Component)this).get_Events().get_Item(IsActivatedChangedEvent))?.Invoke(this, e);
		}

		protected virtual void OnIsActiveDocumentPaneChanged(EventArgs e)
		{
			((EventHandler)((Component)this).get_Events().get_Item(IsActiveDocumentPaneChangedEvent))?.Invoke(this, e);
		}

		public DockPane SetDockState(DockState value)
		{
			if (value == DockState.Unknown || value == DockState.Hidden)
			{
				throw new InvalidOperationException(Strings.DockPane_SetDockState_InvalidState);
			}
			if (value == DockState.Float == IsFloat)
			{
				InternalSetDockState(value);
				return this;
			}
			if (DisplayingContents.Count == 0)
			{
				return null;
			}
			IDockContent dockContent = null;
			for (int i = 0; i < DisplayingContents.Count; i++)
			{
				IDockContent dockContent2 = DisplayingContents[i];
				if (dockContent2.DockHandler.IsDockStateValid(value))
				{
					dockContent = dockContent2;
					break;
				}
			}
			if (dockContent == null)
			{
				return null;
			}
			dockContent.DockHandler.DockState = value;
			DockPane pane = dockContent.DockHandler.Pane;
			DockPanel.SuspendLayout(allWindows: true);
			for (int j = 0; j < DisplayingContents.Count; j++)
			{
				IDockContent dockContent3 = DisplayingContents[j];
				if (dockContent3.DockHandler.IsDockStateValid(value))
				{
					dockContent3.DockHandler.Pane = pane;
				}
			}
			DockPanel.ResumeLayout(performLayout: true, allWindows: true);
			return pane;
		}

		private void InternalSetDockState(DockState value)
		{
			if (m_dockState != value)
			{
				DockState dockState = m_dockState;
				INestedPanesContainer nestedPanesContainer = NestedPanesContainer;
				m_dockState = value;
				SuspendRefreshStateChange();
				IDockContent focusedContent = GetFocusedContent();
				if (focusedContent != null)
				{
					DockPanel.SaveFocus();
				}
				if (!IsFloat)
				{
					DockWindow = DockPanel.DockWindows[DockState];
				}
				else if (FloatWindow == null)
				{
					FloatWindow = DockPanel.FloatWindowFactory.CreateFloatWindow(DockPanel, this);
				}
				focusedContent?.DockHandler.Activate();
				ResumeRefreshStateChange(nestedPanesContainer, dockState);
			}
		}

		private void SuspendRefreshStateChange()
		{
			m_countRefreshStateChange++;
			DockPanel.SuspendLayout(allWindows: true);
		}

		private void ResumeRefreshStateChange()
		{
			m_countRefreshStateChange--;
			if (m_countRefreshStateChange < 0)
			{
				throw new InvalidOperationException();
			}
			if (m_countRefreshStateChange < 0)
			{
				m_countRefreshStateChange = 0;
			}
			DockPanel.ResumeLayout(performLayout: true, allWindows: true);
		}

		private void ResumeRefreshStateChange(INestedPanesContainer oldContainer, DockState oldDockState)
		{
			ResumeRefreshStateChange();
			RefreshStateChange(oldContainer, oldDockState);
		}

		private void RefreshStateChange(INestedPanesContainer oldContainer, DockState oldDockState)
		{
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Expected O, but got Unknown
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			lock (this)
			{
				if (IsRefreshStateChangeSuspended)
				{
					return;
				}
				SuspendRefreshStateChange();
			}
			DockPanel.SuspendLayout(allWindows: true);
			IDockContent focusedContent = GetFocusedContent();
			if (focusedContent != null)
			{
				DockPanel.SaveFocus();
			}
			SetParent();
			if (ActiveContent != null)
			{
				ActiveContent.DockHandler.SetDockState(ActiveContent.DockHandler.IsHidden, DockState, ActiveContent.DockHandler.Pane);
			}
			foreach (IDockContent content in Contents)
			{
				if (content.DockHandler.Pane == this)
				{
					content.DockHandler.SetDockState(content.DockHandler.IsHidden, DockState, content.DockHandler.Pane);
				}
			}
			if (oldContainer != null)
			{
				Control val = (Control)oldContainer;
				if (oldContainer.DockState == oldDockState && !val.get_IsDisposed())
				{
					val.PerformLayout();
				}
			}
			if (DockHelper.IsDockStateAutoHide(oldDockState))
			{
				DockPanel.RefreshActiveAutoHideContent();
			}
			if (NestedPanesContainer.DockState == DockState)
			{
				((Control)NestedPanesContainer).PerformLayout();
			}
			if (DockHelper.IsDockStateAutoHide(DockState))
			{
				DockPanel.RefreshActiveAutoHideContent();
			}
			if (DockHelper.IsDockStateAutoHide(oldDockState) || DockHelper.IsDockStateAutoHide(DockState))
			{
				DockPanel.RefreshAutoHideStrip();
				((Control)DockPanel).PerformLayout();
			}
			ResumeRefreshStateChange();
			focusedContent?.DockHandler.Activate();
			DockPanel.ResumeLayout(performLayout: true, allWindows: true);
			if (oldDockState != DockState)
			{
				OnDockStateChanged(EventArgs.Empty);
			}
		}

		private IDockContent GetFocusedContent()
		{
			IDockContent result = null;
			foreach (IDockContent content in Contents)
			{
				if (((Control)content.DockHandler.Form).get_ContainsFocus())
				{
					result = content;
					break;
				}
			}
			return result;
		}

		public DockPane DockTo(INestedPanesContainer container)
		{
			if (container == null)
			{
				throw new InvalidOperationException(Strings.DockPane_DockTo_NullContainer);
			}
			return DockTo(alignment: (container.DockState != DockState.DockLeft && container.DockState != DockState.DockRight) ? DockAlignment.Right : DockAlignment.Bottom, container: container, previousPane: container.NestedPanes.GetDefaultPreviousPane(this), proportion: 0.5);
		}

		public DockPane DockTo(INestedPanesContainer container, DockPane previousPane, DockAlignment alignment, double proportion)
		{
			if (container == null)
			{
				throw new InvalidOperationException(Strings.DockPane_DockTo_NullContainer);
			}
			if (container.IsFloat == IsFloat)
			{
				InternalAddToDockList(container, previousPane, alignment, proportion);
				return this;
			}
			IDockContent firstContent = GetFirstContent(container.DockState);
			if (firstContent == null)
			{
				return null;
			}
			DockPanel.DummyContent.DockPanel = DockPanel;
			DockPane dockPane = ((!container.IsFloat) ? DockPanel.DockPaneFactory.CreateDockPane(DockPanel.DummyContent, container.DockState, show: true) : DockPanel.DockPaneFactory.CreateDockPane(DockPanel.DummyContent, (FloatWindow)container, show: true));
			dockPane.DockTo(container, previousPane, alignment, proportion);
			SetVisibleContentsToPane(dockPane);
			DockPanel.DummyContent.DockPanel = null;
			return dockPane;
		}

		private void SetVisibleContentsToPane(DockPane pane)
		{
			SetVisibleContentsToPane(pane, ActiveContent);
		}

		private void SetVisibleContentsToPane(DockPane pane, IDockContent activeContent)
		{
			for (int i = 0; i < DisplayingContents.Count; i++)
			{
				IDockContent dockContent = DisplayingContents[i];
				if (dockContent.DockHandler.IsDockStateValid(pane.DockState))
				{
					dockContent.DockHandler.Pane = pane;
					i--;
				}
			}
			if (activeContent.DockHandler.Pane == pane)
			{
				pane.ActiveContent = activeContent;
			}
		}

		private void InternalAddToDockList(INestedPanesContainer container, DockPane prevPane, DockAlignment alignment, double proportion)
		{
			if (container.DockState == DockState.Float != IsFloat)
			{
				throw new InvalidOperationException(Strings.DockPane_DockTo_InvalidContainer);
			}
			int num = container.NestedPanes.Count;
			if (container.NestedPanes.Contains(this))
			{
				num--;
			}
			if (prevPane == null && num > 0)
			{
				throw new InvalidOperationException(Strings.DockPane_DockTo_NullPrevPane);
			}
			if (prevPane != null && !container.NestedPanes.Contains(prevPane))
			{
				throw new InvalidOperationException(Strings.DockPane_DockTo_NoPrevPane);
			}
			if (prevPane == this)
			{
				throw new InvalidOperationException(Strings.DockPane_DockTo_SelfPrevPane);
			}
			INestedPanesContainer nestedPanesContainer = NestedPanesContainer;
			DockState dockState = DockState;
			container.NestedPanes.Add(this);
			NestedDockingStatus.SetStatus(container.NestedPanes, prevPane, alignment, proportion);
			if (DockHelper.IsDockWindowState(DockState))
			{
				m_dockState = container.DockState;
			}
			RefreshStateChange(nestedPanesContainer, dockState);
		}

		public void SetNestedDockingProportion(double proportion)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			NestedDockingStatus.SetStatus(NestedDockingStatus.NestedPanes, NestedDockingStatus.PreviousPane, NestedDockingStatus.Alignment, proportion);
			if (NestedPanesContainer != null)
			{
				((Control)NestedPanesContainer).PerformLayout();
			}
		}

		public DockPane Float()
		{
			DockPanel.SuspendLayout(allWindows: true);
			IDockContent activeContent = ActiveContent;
			DockPane dockPane = GetFloatPaneFromContents();
			if (dockPane == null)
			{
				IDockContent firstContent = GetFirstContent(DockState.Float);
				if (firstContent == null)
				{
					DockPanel.ResumeLayout(performLayout: true, allWindows: true);
					return null;
				}
				dockPane = DockPanel.DockPaneFactory.CreateDockPane(firstContent, DockState.Float, show: true);
			}
			SetVisibleContentsToPane(dockPane, activeContent);
			DockPanel.ResumeLayout(performLayout: true, allWindows: true);
			return dockPane;
		}

		private DockPane GetFloatPaneFromContents()
		{
			DockPane dockPane = null;
			for (int i = 0; i < DisplayingContents.Count; i++)
			{
				IDockContent dockContent = DisplayingContents[i];
				if (dockContent.DockHandler.IsDockStateValid(DockState.Float))
				{
					if (dockPane != null && dockContent.DockHandler.FloatPane != dockPane)
					{
						return null;
					}
					dockPane = dockContent.DockHandler.FloatPane;
				}
			}
			return dockPane;
		}

		private IDockContent GetFirstContent(DockState dockState)
		{
			for (int i = 0; i < DisplayingContents.Count; i++)
			{
				IDockContent dockContent = DisplayingContents[i];
				if (dockContent.DockHandler.IsDockStateValid(dockState))
				{
					return dockContent;
				}
			}
			return null;
		}

		public void RestoreToPanel()
		{
			DockPanel.SuspendLayout(allWindows: true);
			IDockContent activeContent = DockPanel.ActiveContent;
			for (int num = DisplayingContents.Count - 1; num >= 0; num--)
			{
				IDockContent dockContent = DisplayingContents[num];
				if (dockContent.DockHandler.CheckDockState(isFloat: false) != 0)
				{
					dockContent.DockHandler.IsFloat = false;
				}
			}
			DockPanel.ResumeLayout(performLayout: true, allWindows: true);
		}

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).get_Msg() == 33)
			{
				Activate();
			}
			((UserControl)this).WndProc(ref m);
		}

		bool IDockDragSource.IsDockStateValid(DockState dockState)
		{
			return IsDockStateValid(dockState);
		}

		bool IDockDragSource.CanDockTo(DockPane pane)
		{
			if (!IsDockStateValid(pane.DockState))
			{
				return false;
			}
			if (pane == this)
			{
				return false;
			}
			return true;
		}

		Rectangle IDockDragSource.BeginDrag(Point ptMouse)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			Point val = ((Control)this).PointToScreen(new Point(0, 0));
			DockPane floatPane = ActiveContent.DockHandler.FloatPane;
			Size val2 = ((DockState != DockState.Float && floatPane != null && floatPane.FloatWindow.NestedPanes.Count == 1) ? ((Form)floatPane.FloatWindow).get_Size() : DockPanel.DefaultFloatWindowSize);
			if (((Point)(ref ptMouse)).get_X() > ((Point)(ref val)).get_X() + ((Size)(ref val2)).get_Width())
			{
				((Point)(ref val)).set_X(((Point)(ref val)).get_X() + (((Point)(ref ptMouse)).get_X() - (((Point)(ref val)).get_X() + ((Size)(ref val2)).get_Width()) + 4));
			}
			return new Rectangle(val, val2);
		}

		public void FloatAt(Rectangle floatWindowBounds)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			if (FloatWindow == null || FloatWindow.NestedPanes.Count != 1)
			{
				FloatWindow = DockPanel.FloatWindowFactory.CreateFloatWindow(DockPanel, this, floatWindowBounds);
			}
			else
			{
				((Control)FloatWindow).set_Bounds(floatWindowBounds);
			}
			DockState = DockState.Float;
		}

		public void DockTo(DockPane pane, DockStyle dockStyle, int contentIndex)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Invalid comparison between Unknown and I4
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Invalid comparison between Unknown and I4
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Invalid comparison between Unknown and I4
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Invalid comparison between Unknown and I4
			if ((int)dockStyle == 5)
			{
				IDockContent activeContent = ActiveContent;
				for (int num = Contents.Count - 1; num >= 0; num--)
				{
					IDockContent dockContent = Contents[num];
					dockContent.DockHandler.Pane = pane;
					if (contentIndex != -1)
					{
						pane.SetContentIndex(dockContent, contentIndex);
					}
				}
				pane.ActiveContent = activeContent;
			}
			else
			{
				if ((int)dockStyle == 3)
				{
					DockTo(pane.NestedPanesContainer, pane, DockAlignment.Left, 0.5);
				}
				else if ((int)dockStyle == 4)
				{
					DockTo(pane.NestedPanesContainer, pane, DockAlignment.Right, 0.5);
				}
				else if ((int)dockStyle == 1)
				{
					DockTo(pane.NestedPanesContainer, pane, DockAlignment.Top, 0.5);
				}
				else if ((int)dockStyle == 2)
				{
					DockTo(pane.NestedPanesContainer, pane, DockAlignment.Bottom, 0.5);
				}
				DockState = pane.DockState;
			}
		}

		public void DockTo(DockPanel panel, DockStyle dockStyle)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Invalid comparison between Unknown and I4
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Invalid comparison between Unknown and I4
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Invalid comparison between Unknown and I4
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Invalid comparison between Unknown and I4
			if (panel != DockPanel)
			{
				throw new ArgumentException(Strings.IDockDragSource_DockTo_InvalidPanel, "panel");
			}
			if ((int)dockStyle == 1)
			{
				DockState = DockState.DockTop;
			}
			else if ((int)dockStyle == 2)
			{
				DockState = DockState.DockBottom;
			}
			else if ((int)dockStyle == 3)
			{
				DockState = DockState.DockLeft;
			}
			else if ((int)dockStyle == 4)
			{
				DockState = DockState.DockRight;
			}
			else if ((int)dockStyle == 5)
			{
				DockState = DockState.Document;
			}
		}
	}
}

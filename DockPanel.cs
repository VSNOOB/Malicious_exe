using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Layout;
using System.Xml;
using WeifenLuo.WinFormsUI.Docking.Win32;

namespace WeifenLuo.WinFormsUI.Docking
{
	[Designer(typeof(ControlDesigner))]
	[ToolboxBitmap(typeof(DockPanel), "Resources.DockPanel.bmp")]
	public class DockPanel : Panel
	{
		private sealed class SplitterDragHandler : DragHandler
		{
			private class SplitterOutline
			{
				private DragForm m_dragForm;

				private DragForm DragForm => m_dragForm;

				public SplitterOutline()
				{
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					m_dragForm = new DragForm();
					SetDragForm(Rectangle.Empty);
					((Control)DragForm).set_BackColor(Color.get_Black());
					((Form)DragForm).set_Opacity(0.7);
					DragForm.Show(bActivate: false);
				}

				public void Show(Rectangle rect)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					SetDragForm(rect);
				}

				public void Close()
				{
					((Form)DragForm).Close();
				}

				private void SetDragForm(Rectangle rect)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Expected O, but got Unknown
					((Control)DragForm).set_Bounds(rect);
					if (rect == Rectangle.Empty)
					{
						((Control)DragForm).set_Region(new Region(Rectangle.Empty));
					}
					else if (((Control)DragForm).get_Region() != null)
					{
						((Control)DragForm).set_Region((Region)null);
					}
				}
			}

			private SplitterOutline m_outline;

			private Rectangle m_rectSplitter;

			public new ISplitterDragSource DragSource
			{
				get
				{
					return base.DragSource as ISplitterDragSource;
				}
				private set
				{
					base.DragSource = value;
				}
			}

			private SplitterOutline Outline
			{
				get
				{
					return m_outline;
				}
				set
				{
					m_outline = value;
				}
			}

			private Rectangle RectSplitter
			{
				get
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					return m_rectSplitter;
				}
				set
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0003: Unknown result type (might be due to invalid IL or missing references)
					m_rectSplitter = value;
				}
			}

			public SplitterDragHandler(DockPanel dockPanel)
				: base(dockPanel)
			{
			}

			public void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
			{
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				DragSource = dragSource;
				RectSplitter = rectSplitter;
				if (!BeginDrag())
				{
					DragSource = null;
					return;
				}
				Outline = new SplitterOutline();
				Outline.Show(rectSplitter);
				DragSource.BeginDrag(rectSplitter);
			}

			protected override void OnDragging()
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				Outline.Show(GetSplitterOutlineBounds(Control.get_MousePosition()));
			}

			protected override void OnEndDrag(bool abort)
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				base.DockPanel.SuspendLayout(allWindows: true);
				Outline.Close();
				if (!abort)
				{
					DragSource.MoveSplitter(GetMovingOffset(Control.get_MousePosition()));
				}
				DragSource.EndDrag();
				base.DockPanel.ResumeLayout(performLayout: true, allWindows: true);
			}

			private int GetMovingOffset(Point ptMouse)
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				Rectangle splitterOutlineBounds = GetSplitterOutlineBounds(ptMouse);
				Rectangle rectSplitter;
				if (DragSource.IsVertical)
				{
					int x = ((Rectangle)(ref splitterOutlineBounds)).get_X();
					rectSplitter = RectSplitter;
					return x - ((Rectangle)(ref rectSplitter)).get_X();
				}
				int y = ((Rectangle)(ref splitterOutlineBounds)).get_Y();
				rectSplitter = RectSplitter;
				return y - ((Rectangle)(ref rectSplitter)).get_Y();
			}

			private Rectangle GetSplitterOutlineBounds(Point ptMouse)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_005b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Unknown result type (might be due to invalid IL or missing references)
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_0099: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				//IL_016d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				Rectangle dragLimitBounds = DragSource.DragLimitBounds;
				Rectangle rectSplitter = RectSplitter;
				if (((Rectangle)(ref dragLimitBounds)).get_Width() <= 0 || ((Rectangle)(ref dragLimitBounds)).get_Height() <= 0)
				{
					return rectSplitter;
				}
				Point startMousePosition;
				if (DragSource.IsVertical)
				{
					int x = ((Rectangle)(ref rectSplitter)).get_X();
					int x2 = ((Point)(ref ptMouse)).get_X();
					startMousePosition = base.StartMousePosition;
					((Rectangle)(ref rectSplitter)).set_X(x + (x2 - ((Point)(ref startMousePosition)).get_X()));
					((Rectangle)(ref rectSplitter)).set_Height(((Rectangle)(ref dragLimitBounds)).get_Height());
				}
				else
				{
					int y = ((Rectangle)(ref rectSplitter)).get_Y();
					int y2 = ((Point)(ref ptMouse)).get_Y();
					startMousePosition = base.StartMousePosition;
					((Rectangle)(ref rectSplitter)).set_Y(y + (y2 - ((Point)(ref startMousePosition)).get_Y()));
					((Rectangle)(ref rectSplitter)).set_Width(((Rectangle)(ref dragLimitBounds)).get_Width());
				}
				if (((Rectangle)(ref rectSplitter)).get_Left() < ((Rectangle)(ref dragLimitBounds)).get_Left())
				{
					((Rectangle)(ref rectSplitter)).set_X(((Rectangle)(ref dragLimitBounds)).get_X());
				}
				if (((Rectangle)(ref rectSplitter)).get_Top() < ((Rectangle)(ref dragLimitBounds)).get_Top())
				{
					((Rectangle)(ref rectSplitter)).set_Y(((Rectangle)(ref dragLimitBounds)).get_Y());
				}
				if (((Rectangle)(ref rectSplitter)).get_Right() > ((Rectangle)(ref dragLimitBounds)).get_Right())
				{
					((Rectangle)(ref rectSplitter)).set_X(((Rectangle)(ref rectSplitter)).get_X() - (((Rectangle)(ref rectSplitter)).get_Right() - ((Rectangle)(ref dragLimitBounds)).get_Right()));
				}
				if (((Rectangle)(ref rectSplitter)).get_Bottom() > ((Rectangle)(ref dragLimitBounds)).get_Bottom())
				{
					((Rectangle)(ref rectSplitter)).set_Y(((Rectangle)(ref rectSplitter)).get_Y() - (((Rectangle)(ref rectSplitter)).get_Bottom() - ((Rectangle)(ref dragLimitBounds)).get_Bottom()));
				}
				return rectSplitter;
			}
		}

		private abstract class DragHandlerBase : NativeWindow, IMessageFilter
		{
			private Point m_startMousePosition = Point.Empty;

			protected abstract Control DragControl
			{
				get;
			}

			protected Point StartMousePosition
			{
				get
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					return m_startMousePosition;
				}
				private set
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0003: Unknown result type (might be due to invalid IL or missing references)
					m_startMousePosition = value;
				}
			}

			protected DragHandlerBase()
				: this()
			{
			}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)


			protected bool BeginDrag()
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				lock (this)
				{
					if (DragControl == null)
					{
						return false;
					}
					StartMousePosition = Control.get_MousePosition();
					if (!NativeMethods.DragDetect(DragControl.get_Handle(), StartMousePosition))
					{
						return false;
					}
					((Control)DragControl.FindForm()).set_Capture(true);
					((NativeWindow)this).AssignHandle(((Control)DragControl.FindForm()).get_Handle());
					Application.AddMessageFilter((IMessageFilter)(object)this);
					return true;
				}
			}

			protected abstract void OnDragging();

			protected abstract void OnEndDrag(bool abort);

			private void EndDrag(bool abort)
			{
				((NativeWindow)this).ReleaseHandle();
				Application.RemoveMessageFilter((IMessageFilter)(object)this);
				((Control)DragControl.FindForm()).set_Capture(false);
				OnEndDrag(abort);
			}

			bool IMessageFilter.PreFilterMessage(ref Message m)
			{
				if (((Message)(ref m)).get_Msg() == 512)
				{
					OnDragging();
				}
				else if (((Message)(ref m)).get_Msg() == 514)
				{
					EndDrag(abort: false);
				}
				else if (((Message)(ref m)).get_Msg() == 533)
				{
					EndDrag(abort: true);
				}
				else if (((Message)(ref m)).get_Msg() == 256 && (int)((Message)(ref m)).get_WParam() == 27)
				{
					EndDrag(abort: true);
				}
				return OnPreFilterMessage(ref m);
			}

			protected virtual bool OnPreFilterMessage(ref Message m)
			{
				return false;
			}

			protected sealed override void WndProc(ref Message m)
			{
				if (((Message)(ref m)).get_Msg() == 31 || ((Message)(ref m)).get_Msg() == 533)
				{
					EndDrag(abort: true);
				}
				((NativeWindow)this).WndProc(ref m);
			}
		}

		private abstract class DragHandler : DragHandlerBase
		{
			private DockPanel m_dockPanel;

			private IDragSource m_dragSource;

			public DockPanel DockPanel => m_dockPanel;

			protected IDragSource DragSource
			{
				get
				{
					return m_dragSource;
				}
				set
				{
					m_dragSource = value;
				}
			}

			protected sealed override Control DragControl => (DragSource == null) ? null : DragSource.DragControl;

			protected DragHandler(DockPanel dockPanel)
			{
				m_dockPanel = dockPanel;
			}

			protected sealed override bool OnPreFilterMessage(ref Message m)
			{
				if ((((Message)(ref m)).get_Msg() == 256 || ((Message)(ref m)).get_Msg() == 257) && ((int)((Message)(ref m)).get_WParam() == 17 || (int)((Message)(ref m)).get_WParam() == 16))
				{
					OnDragging();
				}
				return base.OnPreFilterMessage(ref m);
			}
		}

		private class MdiClientController : NativeWindow, IComponent, IDisposable
		{
			private bool m_autoScroll = true;

			private BorderStyle m_borderStyle = (BorderStyle)2;

			private MdiClient m_mdiClient = null;

			private Form m_parentForm = null;

			private ISite m_site = null;

			public bool AutoScroll
			{
				get
				{
					return m_autoScroll;
				}
				set
				{
					m_autoScroll = value;
					if (MdiClient != null)
					{
						UpdateStyles();
					}
				}
			}

			public BorderStyle BorderStyle
			{
				set
				{
					//IL_000b: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0024: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_0084: Unknown result type (might be due to invalid IL or missing references)
					//IL_0089: Unknown result type (might be due to invalid IL or missing references)
					//IL_008b: Unknown result type (might be due to invalid IL or missing references)
					//IL_008d: Unknown result type (might be due to invalid IL or missing references)
					//IL_008f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a2: Expected I4, but got Unknown
					if (!Enum.IsDefined(typeof(BorderStyle), value))
					{
						throw new InvalidEnumArgumentException();
					}
					m_borderStyle = value;
					if (MdiClient != null && (Site == null || !Site.get_DesignMode()))
					{
						int num = NativeMethods.GetWindowLong(((Control)MdiClient).get_Handle(), -16);
						int num2 = NativeMethods.GetWindowLong(((Control)MdiClient).get_Handle(), -20);
						BorderStyle borderStyle = m_borderStyle;
						BorderStyle val = borderStyle;
						switch ((int)val)
						{
						case 2:
							num2 |= 0x200;
							num &= -8388609;
							break;
						case 1:
							num2 &= -513;
							num |= 0x800000;
							break;
						case 0:
							num &= -8388609;
							num2 &= -513;
							break;
						}
						NativeMethods.SetWindowLong(((Control)MdiClient).get_Handle(), -16, num);
						NativeMethods.SetWindowLong(((Control)MdiClient).get_Handle(), -20, num2);
						UpdateStyles();
					}
				}
			}

			public MdiClient MdiClient => m_mdiClient;

			[Browsable(false)]
			public Form ParentForm
			{
				get
				{
					return m_parentForm;
				}
				set
				{
					if (m_parentForm != null)
					{
						((Control)m_parentForm).remove_HandleCreated((EventHandler)ParentFormHandleCreated);
						m_parentForm.remove_MdiChildActivate((EventHandler)ParentFormMdiChildActivate);
					}
					m_parentForm = value;
					if (m_parentForm != null)
					{
						if (((Control)m_parentForm).get_IsHandleCreated())
						{
							InitializeMdiClient();
							RefreshProperties();
						}
						else
						{
							((Control)m_parentForm).add_HandleCreated((EventHandler)ParentFormHandleCreated);
						}
						m_parentForm.add_MdiChildActivate((EventHandler)ParentFormMdiChildActivate);
					}
				}
			}

			public ISite Site
			{
				get
				{
					return m_site;
				}
				set
				{
					m_site = value;
					if (m_site == null)
					{
						return;
					}
					object service = ((IServiceProvider)value).GetService(typeof(IDesignerHost));
					IDesignerHost val = service as IDesignerHost;
					if (val != null)
					{
						IComponent rootComponent = val.get_RootComponent();
						Form val2 = rootComponent as Form;
						if (val2 != null)
						{
							ParentForm = val2;
						}
					}
				}
			}

			public event EventHandler Disposed;

			public event EventHandler HandleAssigned;

			public event EventHandler MdiChildActivate;

			public event LayoutEventHandler Layout
			{
				[CompilerGenerated]
				add
				{
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Expected O, but got Unknown
					LayoutEventHandler val = this.Layout;
					LayoutEventHandler val2;
					do
					{
						val2 = val;
						LayoutEventHandler value2 = (LayoutEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<LayoutEventHandler, LayoutEventHandler>(ref this.Layout), value2, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Expected O, but got Unknown
					LayoutEventHandler val = this.Layout;
					LayoutEventHandler val2;
					do
					{
						val2 = val;
						LayoutEventHandler value2 = (LayoutEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<LayoutEventHandler, LayoutEventHandler>(ref this.Layout), value2, val2);
					}
					while (val != val2);
				}
			}

			public event PaintEventHandler Paint
			{
				[CompilerGenerated]
				add
				{
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Expected O, but got Unknown
					PaintEventHandler val = this.Paint;
					PaintEventHandler val2;
					do
					{
						val2 = val;
						PaintEventHandler value2 = (PaintEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<PaintEventHandler, PaintEventHandler>(ref this.Paint), value2, val2);
					}
					while (val != val2);
				}
				[CompilerGenerated]
				remove
				{
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Expected O, but got Unknown
					PaintEventHandler val = this.Paint;
					PaintEventHandler val2;
					do
					{
						val2 = val;
						PaintEventHandler value2 = (PaintEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
						val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<PaintEventHandler, PaintEventHandler>(ref this.Paint), value2, val2);
					}
					while (val != val2);
				}
			}

			public MdiClientController()
				: this()
			{
			}//IL_0009: Unknown result type (might be due to invalid IL or missing references)


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
					if (Site != null && Site.get_Container() != null)
					{
						Site.get_Container().Remove((IComponent)(object)this);
					}
					if (this.Disposed != null)
					{
						this.Disposed(this, EventArgs.Empty);
					}
				}
			}

			public void RenewMdiClient()
			{
				InitializeMdiClient();
				RefreshProperties();
			}

			protected virtual void OnHandleAssigned(EventArgs e)
			{
				if (this.HandleAssigned != null)
				{
					this.HandleAssigned(this, e);
				}
			}

			protected virtual void OnMdiChildActivate(EventArgs e)
			{
				if (this.MdiChildActivate != null)
				{
					this.MdiChildActivate(this, e);
				}
			}

			protected virtual void OnLayout(LayoutEventArgs e)
			{
				if ((object)this.Layout != null)
				{
					this.Layout.Invoke((object)this, e);
				}
			}

			protected virtual void OnPaint(PaintEventArgs e)
			{
				if ((object)this.Paint != null)
				{
					this.Paint.Invoke((object)this, e);
				}
			}

			protected override void WndProc(ref Message m)
			{
				int msg = ((Message)(ref m)).get_Msg();
				int num = msg;
				if (num == 131 && !AutoScroll)
				{
					NativeMethods.ShowScrollBar(((Message)(ref m)).get_HWnd(), 3, 0);
				}
				((NativeWindow)this).WndProc(ref m);
			}

			private void ParentFormHandleCreated(object sender, EventArgs e)
			{
				((Control)m_parentForm).remove_HandleCreated((EventHandler)ParentFormHandleCreated);
				InitializeMdiClient();
				RefreshProperties();
			}

			private void ParentFormMdiChildActivate(object sender, EventArgs e)
			{
				OnMdiChildActivate(e);
			}

			private void MdiClientLayout(object sender, LayoutEventArgs e)
			{
				OnLayout(e);
			}

			private void MdiClientHandleDestroyed(object sender, EventArgs e)
			{
				if (m_mdiClient != null)
				{
					((Control)m_mdiClient).remove_HandleDestroyed((EventHandler)MdiClientHandleDestroyed);
					m_mdiClient = null;
				}
				((NativeWindow)this).ReleaseHandle();
			}

			private void InitializeMdiClient()
			{
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Expected O, but got Unknown
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Expected O, but got Unknown
				//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e7: Expected O, but got Unknown
				if (MdiClient != null)
				{
					((Control)MdiClient).remove_HandleDestroyed((EventHandler)MdiClientHandleDestroyed);
					((Control)MdiClient).remove_Layout(new LayoutEventHandler(MdiClientLayout));
				}
				if (ParentForm == null)
				{
					return;
				}
				foreach (Control item in (ArrangedElementCollection)((Control)ParentForm).get_Controls())
				{
					Control val = item;
					m_mdiClient = val as MdiClient;
					if (m_mdiClient == null)
					{
						continue;
					}
					((NativeWindow)this).ReleaseHandle();
					((NativeWindow)this).AssignHandle(((Control)MdiClient).get_Handle());
					OnHandleAssigned(EventArgs.Empty);
					((Control)MdiClient).add_HandleDestroyed((EventHandler)MdiClientHandleDestroyed);
					((Control)MdiClient).add_Layout(new LayoutEventHandler(MdiClientLayout));
					break;
				}
			}

			private void RefreshProperties()
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				BorderStyle = m_borderStyle;
				AutoScroll = m_autoScroll;
			}

			private void UpdateStyles()
			{
				NativeMethods.SetWindowPos(((Control)MdiClient).get_Handle(), IntPtr.Zero, 0, 0, 0, 0, FlagsSetWindowPos.SWP_NOSIZE | FlagsSetWindowPos.SWP_NOMOVE | FlagsSetWindowPos.SWP_NOZORDER | FlagsSetWindowPos.SWP_NOACTIVATE | FlagsSetWindowPos.SWP_FRAMECHANGED | FlagsSetWindowPos.SWP_NOOWNERZORDER);
			}
		}

		private class AutoHideWindowControl : Panel, ISplitterDragSource, IDragSource
		{
			private class SplitterControl : SplitterBase
			{
				private AutoHideWindowControl m_autoHideWindow;

				private AutoHideWindowControl AutoHideWindow => m_autoHideWindow;

				protected override int SplitterSize => 4;

				public SplitterControl(AutoHideWindowControl autoHideWindow)
				{
					m_autoHideWindow = autoHideWindow;
				}

				protected override void StartDrag()
				{
					//IL_0019: Unknown result type (might be due to invalid IL or missing references)
					//IL_001e: Unknown result type (might be due to invalid IL or missing references)
					AutoHideWindow.DockPanel.BeginDrag(AutoHideWindow, ((Control)AutoHideWindow).RectangleToScreen(((Control)this).get_Bounds()));
				}
			}

			private const int ANIMATE_TIME = 100;

			private Timer m_timerMouseTrack;

			private SplitterControl m_splitter;

			private DockPanel m_dockPanel = null;

			private DockPane m_activePane = null;

			private IDockContent m_activeContent = null;

			private bool m_flagAnimate = true;

			private bool m_flagDragging = false;

			public DockPanel DockPanel => m_dockPanel;

			public DockPane ActivePane => m_activePane;

			public IDockContent ActiveContent
			{
				get
				{
					return m_activeContent;
				}
				set
				{
					if (value == m_activeContent)
					{
						return;
					}
					if (value != null && (!DockHelper.IsDockStateAutoHide(value.DockHandler.DockState) || value.DockHandler.DockPanel != DockPanel))
					{
						throw new InvalidOperationException(Strings.DockPanel_ActiveAutoHideContent_InvalidValue);
					}
					((Control)DockPanel).SuspendLayout();
					if (m_activeContent != null)
					{
						if (((Control)m_activeContent.DockHandler.Form).get_ContainsFocus())
						{
							DockPanel.ContentFocusManager.GiveUpFocus(m_activeContent);
						}
						AnimateWindow(show: false);
					}
					m_activeContent = value;
					SetActivePane();
					if (ActivePane != null)
					{
						ActivePane.ActiveContent = m_activeContent;
					}
					if (m_activeContent != null)
					{
						AnimateWindow(show: true);
					}
					((Control)DockPanel).ResumeLayout();
					DockPanel.RefreshAutoHideStrip();
					SetTimerMouseTrack();
				}
			}

			public DockState DockState => (ActiveContent != null) ? ActiveContent.DockHandler.DockState : DockState.Unknown;

			private bool FlagAnimate
			{
				get
				{
					return m_flagAnimate;
				}
				set
				{
					m_flagAnimate = value;
				}
			}

			internal bool FlagDragging
			{
				get
				{
					return m_flagDragging;
				}
				set
				{
					if (m_flagDragging != value)
					{
						m_flagDragging = value;
						SetTimerMouseTrack();
					}
				}
			}

			protected virtual Rectangle DisplayingRectangle
			{
				get
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
					Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
					if (DockState == DockState.DockBottomAutoHide)
					{
						((Rectangle)(ref clientRectangle)).set_Y(((Rectangle)(ref clientRectangle)).get_Y() + 6);
						((Rectangle)(ref clientRectangle)).set_Height(((Rectangle)(ref clientRectangle)).get_Height() - 6);
					}
					else if (DockState == DockState.DockRightAutoHide)
					{
						((Rectangle)(ref clientRectangle)).set_X(((Rectangle)(ref clientRectangle)).get_X() + 6);
						((Rectangle)(ref clientRectangle)).set_Width(((Rectangle)(ref clientRectangle)).get_Width() - 6);
					}
					else if (DockState == DockState.DockTopAutoHide)
					{
						((Rectangle)(ref clientRectangle)).set_Height(((Rectangle)(ref clientRectangle)).get_Height() - 6);
					}
					else if (DockState == DockState.DockLeftAutoHide)
					{
						((Rectangle)(ref clientRectangle)).set_Width(((Rectangle)(ref clientRectangle)).get_Width() - 6);
					}
					return clientRectangle;
				}
			}

			bool ISplitterDragSource.IsVertical => DockState == DockState.DockLeftAutoHide || DockState == DockState.DockRightAutoHide;

			Rectangle ISplitterDragSource.DragLimitBounds
			{
				get
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0067: Unknown result type (might be due to invalid IL or missing references)
					//IL_0068: Unknown result type (might be due to invalid IL or missing references)
					//IL_006d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0070: Unknown result type (might be due to invalid IL or missing references)
					Rectangle dockArea = DockPanel.DockArea;
					if (((ISplitterDragSource)this).IsVertical)
					{
						((Rectangle)(ref dockArea)).set_X(((Rectangle)(ref dockArea)).get_X() + 24);
						((Rectangle)(ref dockArea)).set_Width(((Rectangle)(ref dockArea)).get_Width() - 48);
					}
					else
					{
						((Rectangle)(ref dockArea)).set_Y(((Rectangle)(ref dockArea)).get_Y() + 24);
						((Rectangle)(ref dockArea)).set_Height(((Rectangle)(ref dockArea)).get_Height() - 48);
					}
					return ((Control)DockPanel).RectangleToScreen(dockArea);
				}
			}

			Control IDragSource.DragControl => (Control)(object)this;

			public AutoHideWindowControl(DockPanel dockPanel)
				: this()
			{
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Expected O, but got Unknown
				m_dockPanel = dockPanel;
				m_timerMouseTrack = new Timer();
				m_timerMouseTrack.add_Tick((EventHandler)TimerMouseTrack_Tick);
				((Control)this).set_Visible(false);
				m_splitter = new SplitterControl(this);
				((Control)this).get_Controls().Add((Control)(object)m_splitter);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					((Component)m_timerMouseTrack).Dispose();
				}
				((Control)this).Dispose(disposing);
			}

			private void SetActivePane()
			{
				DockPane dockPane = ((ActiveContent == null) ? null : ActiveContent.DockHandler.Pane);
				if (dockPane != m_activePane)
				{
					m_activePane = dockPane;
				}
			}

			private void AnimateWindow(bool show)
			{
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0133: Unknown result type (might be due to invalid IL or missing references)
				//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0382: Unknown result type (might be due to invalid IL or missing references)
				//IL_0383: Unknown result type (might be due to invalid IL or missing references)
				if (!FlagAnimate && ((Control)this).get_Visible() != show)
				{
					((Control)this).set_Visible(show);
					return;
				}
				((Control)this).get_Parent().SuspendLayout();
				Rectangle rectangle = GetRectangle(!show);
				Rectangle rectangle2 = GetRectangle(show);
				int num3;
				int num2;
				int num;
				int num4 = (num3 = (num2 = (num = 0)));
				if (DockState == DockState.DockTopAutoHide)
				{
					num = (show ? 1 : (-1));
				}
				else if (DockState == DockState.DockLeftAutoHide)
				{
					num2 = (show ? 1 : (-1));
				}
				else if (DockState == DockState.DockRightAutoHide)
				{
					num4 = ((!show) ? 1 : (-1));
					num2 = (show ? 1 : (-1));
				}
				else if (DockState == DockState.DockBottomAutoHide)
				{
					num3 = ((!show) ? 1 : (-1));
					num = (show ? 1 : (-1));
				}
				if (show)
				{
					((Control)this).set_Bounds(DockPanel.GetAutoHideWindowBounds(new Rectangle(-((Rectangle)(ref rectangle2)).get_Width(), -((Rectangle)(ref rectangle2)).get_Height(), ((Rectangle)(ref rectangle2)).get_Width(), ((Rectangle)(ref rectangle2)).get_Height())));
					if (!((Control)this).get_Visible())
					{
						((Control)this).set_Visible(true);
					}
					((Control)this).PerformLayout();
				}
				((Control)this).SuspendLayout();
				LayoutAnimateWindow(rectangle);
				if (!((Control)this).get_Visible())
				{
					((Control)this).set_Visible(true);
				}
				int num5 = 1;
				int num6 = ((((Rectangle)(ref rectangle)).get_Width() != ((Rectangle)(ref rectangle2)).get_Width()) ? Math.Abs(((Rectangle)(ref rectangle)).get_Width() - ((Rectangle)(ref rectangle2)).get_Width()) : Math.Abs(((Rectangle)(ref rectangle)).get_Height() - ((Rectangle)(ref rectangle2)).get_Height()));
				int num7 = num6;
				DateTime now = DateTime.Now;
				while (rectangle != rectangle2)
				{
					DateTime now2 = DateTime.Now;
					((Rectangle)(ref rectangle)).set_X(((Rectangle)(ref rectangle)).get_X() + num4 * num5);
					((Rectangle)(ref rectangle)).set_Y(((Rectangle)(ref rectangle)).get_Y() + num3 * num5);
					((Rectangle)(ref rectangle)).set_Width(((Rectangle)(ref rectangle)).get_Width() + num2 * num5);
					((Rectangle)(ref rectangle)).set_Height(((Rectangle)(ref rectangle)).get_Height() + num * num5);
					if (Math.Sign(((Rectangle)(ref rectangle2)).get_X() - ((Rectangle)(ref rectangle)).get_X()) != Math.Sign(num4))
					{
						((Rectangle)(ref rectangle)).set_X(((Rectangle)(ref rectangle2)).get_X());
					}
					if (Math.Sign(((Rectangle)(ref rectangle2)).get_Y() - ((Rectangle)(ref rectangle)).get_Y()) != Math.Sign(num3))
					{
						((Rectangle)(ref rectangle)).set_Y(((Rectangle)(ref rectangle2)).get_Y());
					}
					if (Math.Sign(((Rectangle)(ref rectangle2)).get_Width() - ((Rectangle)(ref rectangle)).get_Width()) != Math.Sign(num2))
					{
						((Rectangle)(ref rectangle)).set_Width(((Rectangle)(ref rectangle2)).get_Width());
					}
					if (Math.Sign(((Rectangle)(ref rectangle2)).get_Height() - ((Rectangle)(ref rectangle)).get_Height()) != Math.Sign(num))
					{
						((Rectangle)(ref rectangle)).set_Height(((Rectangle)(ref rectangle2)).get_Height());
					}
					LayoutAnimateWindow(rectangle);
					if (((Control)this).get_Parent() != null)
					{
						((Control)this).get_Parent().Update();
					}
					num7 -= num5;
					do
					{
						TimeSpan t = new TimeSpan(0, 0, 0, 0, 100);
						TimeSpan timeSpan = DateTime.Now - now2;
						TimeSpan t2 = DateTime.Now - now;
						if ((int)(t - t2).TotalMilliseconds <= 0)
						{
							num5 = num7;
							break;
						}
						num5 = num7 * (int)timeSpan.TotalMilliseconds / (int)(t - t2).TotalMilliseconds;
					}
					while (num5 < 1);
				}
				((Control)this).ResumeLayout();
				((Control)this).get_Parent().ResumeLayout();
			}

			private void LayoutAnimateWindow(Rectangle rect)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_0058: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
				((Control)this).set_Bounds(DockPanel.GetAutoHideWindowBounds(rect));
				Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
				Point location;
				if (DockState == DockState.DockLeftAutoHide)
				{
					DockPane activePane = ActivePane;
					int num = ((Rectangle)(ref clientRectangle)).get_Right() - 2 - 4 - ((Control)ActivePane).get_Width();
					location = ((Control)ActivePane).get_Location();
					((Control)activePane).set_Location(new Point(num, ((Point)(ref location)).get_Y()));
				}
				else if (DockState == DockState.DockTopAutoHide)
				{
					DockPane activePane2 = ActivePane;
					location = ((Control)ActivePane).get_Location();
					((Control)activePane2).set_Location(new Point(((Point)(ref location)).get_X(), ((Rectangle)(ref clientRectangle)).get_Bottom() - 2 - 4 - ((Control)ActivePane).get_Height()));
				}
			}

			private Rectangle GetRectangle(bool show)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
				if (DockState == DockState.Unknown)
				{
					return Rectangle.Empty;
				}
				Rectangle autoHideWindowRectangle = DockPanel.AutoHideWindowRectangle;
				if (show)
				{
					return autoHideWindowRectangle;
				}
				if (DockState == DockState.DockLeftAutoHide)
				{
					((Rectangle)(ref autoHideWindowRectangle)).set_Width(0);
				}
				else if (DockState == DockState.DockRightAutoHide)
				{
					((Rectangle)(ref autoHideWindowRectangle)).set_X(((Rectangle)(ref autoHideWindowRectangle)).get_X() + ((Rectangle)(ref autoHideWindowRectangle)).get_Width());
					((Rectangle)(ref autoHideWindowRectangle)).set_Width(0);
				}
				else if (DockState == DockState.DockTopAutoHide)
				{
					((Rectangle)(ref autoHideWindowRectangle)).set_Height(0);
				}
				else
				{
					((Rectangle)(ref autoHideWindowRectangle)).set_Y(((Rectangle)(ref autoHideWindowRectangle)).get_Y() + ((Rectangle)(ref autoHideWindowRectangle)).get_Height());
					((Rectangle)(ref autoHideWindowRectangle)).set_Height(0);
				}
				return autoHideWindowRectangle;
			}

			private void SetTimerMouseTrack()
			{
				if (ActivePane == null || ActivePane.IsActivated || FlagDragging)
				{
					m_timerMouseTrack.set_Enabled(false);
					return;
				}
				int num = SystemInformation.get_MouseHoverTime();
				if (num <= 0)
				{
					num = 400;
				}
				m_timerMouseTrack.set_Interval(2 * num);
				m_timerMouseTrack.set_Enabled(true);
			}

			protected override void OnLayout(LayoutEventArgs levent)
			{
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0101: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Expected O, but got Unknown
				//IL_0131: Unknown result type (might be due to invalid IL or missing references)
				//IL_013c: Unknown result type (might be due to invalid IL or missing references)
				((ScrollableControl)this).get_DockPadding().set_All(0);
				if (DockState == DockState.DockLeftAutoHide)
				{
					((ScrollableControl)this).get_DockPadding().set_Right(2);
					((Control)m_splitter).set_Dock((DockStyle)4);
				}
				else if (DockState == DockState.DockRightAutoHide)
				{
					((ScrollableControl)this).get_DockPadding().set_Left(2);
					((Control)m_splitter).set_Dock((DockStyle)3);
				}
				else if (DockState == DockState.DockTopAutoHide)
				{
					((ScrollableControl)this).get_DockPadding().set_Bottom(2);
					((Control)m_splitter).set_Dock((DockStyle)2);
				}
				else if (DockState == DockState.DockBottomAutoHide)
				{
					((ScrollableControl)this).get_DockPadding().set_Top(2);
					((Control)m_splitter).set_Dock((DockStyle)1);
				}
				Rectangle displayingRectangle = DisplayingRectangle;
				Rectangle bounds = default(Rectangle);
				((Rectangle)(ref bounds))._002Ector(-((Rectangle)(ref displayingRectangle)).get_Width(), ((Rectangle)(ref displayingRectangle)).get_Y(), ((Rectangle)(ref displayingRectangle)).get_Width(), ((Rectangle)(ref displayingRectangle)).get_Height());
				foreach (Control item in (ArrangedElementCollection)((Control)this).get_Controls())
				{
					Control val = item;
					DockPane dockPane = val as DockPane;
					if (dockPane != null)
					{
						if (dockPane == ActivePane)
						{
							((Control)dockPane).set_Bounds(displayingRectangle);
						}
						else
						{
							((Control)dockPane).set_Bounds(bounds);
						}
					}
				}
				((ScrollableControl)this).OnLayout(levent);
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_008e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0093: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00be: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
				//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
				//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
				//IL_0109: Unknown result type (might be due to invalid IL or missing references)
				//IL_010e: Unknown result type (might be due to invalid IL or missing references)
				//IL_011a: Unknown result type (might be due to invalid IL or missing references)
				//IL_011f: Unknown result type (might be due to invalid IL or missing references)
				//IL_012a: Unknown result type (might be due to invalid IL or missing references)
				//IL_012f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0144: Unknown result type (might be due to invalid IL or missing references)
				//IL_0149: Unknown result type (might be due to invalid IL or missing references)
				//IL_0155: Unknown result type (might be due to invalid IL or missing references)
				//IL_015a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0165: Unknown result type (might be due to invalid IL or missing references)
				//IL_016a: Unknown result type (might be due to invalid IL or missing references)
				Graphics graphics = e.get_Graphics();
				Rectangle clientRectangle;
				if (DockState == DockState.DockBottomAutoHide)
				{
					Pen controlLightLight = SystemPens.get_ControlLightLight();
					clientRectangle = ((Control)this).get_ClientRectangle();
					graphics.DrawLine(controlLightLight, 0, 1, ((Rectangle)(ref clientRectangle)).get_Right(), 1);
				}
				else if (DockState == DockState.DockRightAutoHide)
				{
					Pen controlLightLight2 = SystemPens.get_ControlLightLight();
					clientRectangle = ((Control)this).get_ClientRectangle();
					graphics.DrawLine(controlLightLight2, 1, 0, 1, ((Rectangle)(ref clientRectangle)).get_Bottom());
				}
				else if (DockState == DockState.DockTopAutoHide)
				{
					Pen controlDark = SystemPens.get_ControlDark();
					clientRectangle = ((Control)this).get_ClientRectangle();
					int num = ((Rectangle)(ref clientRectangle)).get_Height() - 2;
					clientRectangle = ((Control)this).get_ClientRectangle();
					int right = ((Rectangle)(ref clientRectangle)).get_Right();
					clientRectangle = ((Control)this).get_ClientRectangle();
					graphics.DrawLine(controlDark, 0, num, right, ((Rectangle)(ref clientRectangle)).get_Height() - 2);
					Pen controlDarkDark = SystemPens.get_ControlDarkDark();
					clientRectangle = ((Control)this).get_ClientRectangle();
					int num2 = ((Rectangle)(ref clientRectangle)).get_Height() - 1;
					clientRectangle = ((Control)this).get_ClientRectangle();
					int right2 = ((Rectangle)(ref clientRectangle)).get_Right();
					clientRectangle = ((Control)this).get_ClientRectangle();
					graphics.DrawLine(controlDarkDark, 0, num2, right2, ((Rectangle)(ref clientRectangle)).get_Height() - 1);
				}
				else if (DockState == DockState.DockLeftAutoHide)
				{
					Pen controlDark2 = SystemPens.get_ControlDark();
					clientRectangle = ((Control)this).get_ClientRectangle();
					int num3 = ((Rectangle)(ref clientRectangle)).get_Width() - 2;
					clientRectangle = ((Control)this).get_ClientRectangle();
					int num4 = ((Rectangle)(ref clientRectangle)).get_Width() - 2;
					clientRectangle = ((Control)this).get_ClientRectangle();
					graphics.DrawLine(controlDark2, num3, 0, num4, ((Rectangle)(ref clientRectangle)).get_Bottom());
					Pen controlDarkDark2 = SystemPens.get_ControlDarkDark();
					clientRectangle = ((Control)this).get_ClientRectangle();
					int num5 = ((Rectangle)(ref clientRectangle)).get_Width() - 1;
					clientRectangle = ((Control)this).get_ClientRectangle();
					int num6 = ((Rectangle)(ref clientRectangle)).get_Width() - 1;
					clientRectangle = ((Control)this).get_ClientRectangle();
					graphics.DrawLine(controlDarkDark2, num5, 0, num6, ((Rectangle)(ref clientRectangle)).get_Bottom());
				}
				((Control)this).OnPaint(e);
			}

			public void RefreshActiveContent()
			{
				if (ActiveContent != null && !DockHelper.IsDockStateAutoHide(ActiveContent.DockHandler.DockState))
				{
					FlagAnimate = false;
					ActiveContent = null;
					FlagAnimate = true;
				}
			}

			public void RefreshActivePane()
			{
				SetTimerMouseTrack();
			}

			private void TimerMouseTrack_Tick(object sender, EventArgs e)
			{
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0064: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				if (ActivePane == null || ActivePane.IsActivated)
				{
					m_timerMouseTrack.set_Enabled(false);
					return;
				}
				DockPane activePane = ActivePane;
				Point val = ((Control)this).PointToClient(Control.get_MousePosition());
				Point val2 = ((Control)DockPanel).PointToClient(Control.get_MousePosition());
				Rectangle tabStripRectangle = DockPanel.GetTabStripRectangle(activePane.DockState);
				Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
				if (!((Rectangle)(ref clientRectangle)).Contains(val) && !((Rectangle)(ref tabStripRectangle)).Contains(val2))
				{
					ActiveContent = null;
					m_timerMouseTrack.set_Enabled(false);
				}
			}

			void ISplitterDragSource.BeginDrag(Rectangle rectSplitter)
			{
				FlagDragging = true;
			}

			void ISplitterDragSource.EndDrag()
			{
				FlagDragging = false;
			}

			void ISplitterDragSource.MoveSplitter(int offset)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				Rectangle dockArea = DockPanel.DockArea;
				IDockContent activeContent = ActiveContent;
				if (DockState == DockState.DockLeftAutoHide && ((Rectangle)(ref dockArea)).get_Width() > 0)
				{
					if (activeContent.DockHandler.AutoHidePortion < 1.0)
					{
						activeContent.DockHandler.AutoHidePortion += (double)offset / (double)((Rectangle)(ref dockArea)).get_Width();
					}
					else
					{
						activeContent.DockHandler.AutoHidePortion = ((Control)this).get_Width() + offset;
					}
				}
				else if (DockState == DockState.DockRightAutoHide && ((Rectangle)(ref dockArea)).get_Width() > 0)
				{
					if (activeContent.DockHandler.AutoHidePortion < 1.0)
					{
						activeContent.DockHandler.AutoHidePortion -= (double)offset / (double)((Rectangle)(ref dockArea)).get_Width();
					}
					else
					{
						activeContent.DockHandler.AutoHidePortion = ((Control)this).get_Width() - offset;
					}
				}
				else if (DockState == DockState.DockBottomAutoHide && ((Rectangle)(ref dockArea)).get_Height() > 0)
				{
					if (activeContent.DockHandler.AutoHidePortion < 1.0)
					{
						activeContent.DockHandler.AutoHidePortion -= (double)offset / (double)((Rectangle)(ref dockArea)).get_Height();
					}
					else
					{
						activeContent.DockHandler.AutoHidePortion = ((Control)this).get_Height() - offset;
					}
				}
				else if (DockState == DockState.DockTopAutoHide && ((Rectangle)(ref dockArea)).get_Height() > 0)
				{
					if (activeContent.DockHandler.AutoHidePortion < 1.0)
					{
						activeContent.DockHandler.AutoHidePortion += (double)offset / (double)((Rectangle)(ref dockArea)).get_Height();
					}
					else
					{
						activeContent.DockHandler.AutoHidePortion = ((Control)this).get_Height() + offset;
					}
				}
			}
		}

		private sealed class DockDragHandler : DragHandler
		{
			private class DockIndicator : DragForm
			{
				private interface IHitTest
				{
					DockStyle Status
					{
						get;
						set;
					}

					DockStyle HitTest(Point pt);
				}

				private class PanelIndicator : PictureBox, IHitTest
				{
					private static Image _imagePanelLeft;

					private static Image _imagePanelRight;

					private static Image _imagePanelTop;

					private static Image _imagePanelBottom;

					private static Image _imagePanelFill;

					private static Image _imagePanelLeftActive;

					private static Image _imagePanelRightActive;

					private static Image _imagePanelTopActive;

					private static Image _imagePanelBottomActive;

					private static Image _imagePanelFillActive;

					private DockStyle m_dockStyle;

					private DockStyle m_status;

					private bool m_isActivated = false;

					private DockStyle DockStyle => m_dockStyle;

					public DockStyle Status
					{
						get
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							//IL_000a: Unknown result type (might be due to invalid IL or missing references)
							return m_status;
						}
						set
						{
							//IL_0001: Unknown result type (might be due to invalid IL or missing references)
							//IL_0003: Unknown result type (might be due to invalid IL or missing references)
							//IL_000a: Unknown result type (might be due to invalid IL or missing references)
							//IL_000c: Invalid comparison between Unknown and I4
							//IL_0015: Unknown result type (might be due to invalid IL or missing references)
							//IL_001c: Unknown result type (might be due to invalid IL or missing references)
							//IL_0021: Unknown result type (might be due to invalid IL or missing references)
							//IL_002b: Unknown result type (might be due to invalid IL or missing references)
							//IL_002c: Unknown result type (might be due to invalid IL or missing references)
							//IL_0033: Unknown result type (might be due to invalid IL or missing references)
							//IL_0039: Invalid comparison between Unknown and I4
							if (value != DockStyle && (int)value > 0)
							{
								throw new InvalidEnumArgumentException();
							}
							if (m_status != value)
							{
								m_status = value;
								IsActivated = (int)m_status > 0;
							}
						}
					}

					private Image ImageInactive
					{
						get
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0008: Invalid comparison between Unknown and I4
							//IL_0017: Unknown result type (might be due to invalid IL or missing references)
							//IL_001d: Invalid comparison between Unknown and I4
							//IL_002c: Unknown result type (might be due to invalid IL or missing references)
							//IL_0032: Invalid comparison between Unknown and I4
							//IL_0041: Unknown result type (might be due to invalid IL or missing references)
							//IL_0047: Invalid comparison between Unknown and I4
							//IL_0058: Unknown result type (might be due to invalid IL or missing references)
							//IL_005e: Invalid comparison between Unknown and I4
							if ((int)DockStyle == 3)
							{
								return _imagePanelLeft;
							}
							if ((int)DockStyle == 4)
							{
								return _imagePanelRight;
							}
							if ((int)DockStyle == 1)
							{
								return _imagePanelTop;
							}
							if ((int)DockStyle == 2)
							{
								return _imagePanelBottom;
							}
							if ((int)DockStyle == 5)
							{
								return _imagePanelFill;
							}
							return null;
						}
					}

					private Image ImageActive
					{
						get
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0008: Invalid comparison between Unknown and I4
							//IL_0017: Unknown result type (might be due to invalid IL or missing references)
							//IL_001d: Invalid comparison between Unknown and I4
							//IL_002c: Unknown result type (might be due to invalid IL or missing references)
							//IL_0032: Invalid comparison between Unknown and I4
							//IL_0041: Unknown result type (might be due to invalid IL or missing references)
							//IL_0047: Invalid comparison between Unknown and I4
							//IL_0058: Unknown result type (might be due to invalid IL or missing references)
							//IL_005e: Invalid comparison between Unknown and I4
							if ((int)DockStyle == 3)
							{
								return _imagePanelLeftActive;
							}
							if ((int)DockStyle == 4)
							{
								return _imagePanelRightActive;
							}
							if ((int)DockStyle == 1)
							{
								return _imagePanelTopActive;
							}
							if ((int)DockStyle == 2)
							{
								return _imagePanelBottomActive;
							}
							if ((int)DockStyle == 5)
							{
								return _imagePanelFillActive;
							}
							return null;
						}
					}

					private bool IsActivated
					{
						get
						{
							return m_isActivated;
						}
						set
						{
							m_isActivated = value;
							((PictureBox)this).set_Image(IsActivated ? ImageActive : ImageInactive);
						}
					}

					public PanelIndicator(DockStyle dockStyle)
						: this()
					{
						//IL_0010: Unknown result type (might be due to invalid IL or missing references)
						//IL_0011: Unknown result type (might be due to invalid IL or missing references)
						m_dockStyle = dockStyle;
						((PictureBox)this).set_SizeMode((PictureBoxSizeMode)2);
						((PictureBox)this).set_Image(ImageInactive);
					}

					public DockStyle HitTest(Point pt)
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						//IL_000b: Unknown result type (might be due to invalid IL or missing references)
						//IL_000c: Unknown result type (might be due to invalid IL or missing references)
						//IL_001c: Unknown result type (might be due to invalid IL or missing references)
						//IL_0021: Unknown result type (might be due to invalid IL or missing references)
						//IL_0024: Unknown result type (might be due to invalid IL or missing references)
						Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
						return (DockStyle)(((Rectangle)(ref clientRectangle)).Contains(((Control)this).PointToClient(pt)) ? ((int)DockStyle) : 0);
					}
				}

				private class PaneIndicator : PictureBox, IHitTest
				{
					private struct HotSpotIndex
					{
						private int m_x;

						private int m_y;

						private DockStyle m_dockStyle;

						public int X => m_x;

						public int Y => m_y;

						public DockStyle DockStyle => m_dockStyle;

						public HotSpotIndex(int x, int y, DockStyle dockStyle)
						{
							//IL_0010: Unknown result type (might be due to invalid IL or missing references)
							//IL_0011: Unknown result type (might be due to invalid IL or missing references)
							m_x = x;
							m_y = y;
							m_dockStyle = dockStyle;
						}
					}

					private static Bitmap _bitmapPaneDiamond;

					private static Bitmap _bitmapPaneDiamondLeft;

					private static Bitmap _bitmapPaneDiamondRight;

					private static Bitmap _bitmapPaneDiamondTop;

					private static Bitmap _bitmapPaneDiamondBottom;

					private static Bitmap _bitmapPaneDiamondFill;

					private static Bitmap _bitmapPaneDiamondHotSpot;

					private static Bitmap _bitmapPaneDiamondHotSpotIndex;

					private static HotSpotIndex[] _hotSpots = new HotSpotIndex[5]
					{
						new HotSpotIndex(1, 0, (DockStyle)1),
						new HotSpotIndex(0, 1, (DockStyle)3),
						new HotSpotIndex(1, 1, (DockStyle)5),
						new HotSpotIndex(2, 1, (DockStyle)4),
						new HotSpotIndex(1, 2, (DockStyle)2)
					};

					private static GraphicsPath _displayingGraphicsPath = DrawHelper.CalculateGraphicsPathFromBitmap(_bitmapPaneDiamond);

					private DockStyle m_status = (DockStyle)0;

					public static GraphicsPath DisplayingGraphicsPath => _displayingGraphicsPath;

					public DockStyle Status
					{
						get
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0007: Unknown result type (might be due to invalid IL or missing references)
							//IL_000a: Unknown result type (might be due to invalid IL or missing references)
							return m_status;
						}
						set
						{
							//IL_0002: Unknown result type (might be due to invalid IL or missing references)
							//IL_0003: Unknown result type (might be due to invalid IL or missing references)
							//IL_0009: Unknown result type (might be due to invalid IL or missing references)
							//IL_000f: Invalid comparison between Unknown and I4
							//IL_0027: Unknown result type (might be due to invalid IL or missing references)
							//IL_002d: Invalid comparison between Unknown and I4
							//IL_0042: Unknown result type (might be due to invalid IL or missing references)
							//IL_0048: Invalid comparison between Unknown and I4
							//IL_005d: Unknown result type (might be due to invalid IL or missing references)
							//IL_0063: Invalid comparison between Unknown and I4
							//IL_0078: Unknown result type (might be due to invalid IL or missing references)
							//IL_007e: Invalid comparison between Unknown and I4
							//IL_0095: Unknown result type (might be due to invalid IL or missing references)
							//IL_009b: Invalid comparison between Unknown and I4
							m_status = value;
							if ((int)m_status == 0)
							{
								((PictureBox)this).set_Image((Image)(object)_bitmapPaneDiamond);
							}
							else if ((int)m_status == 3)
							{
								((PictureBox)this).set_Image((Image)(object)_bitmapPaneDiamondLeft);
							}
							else if ((int)m_status == 4)
							{
								((PictureBox)this).set_Image((Image)(object)_bitmapPaneDiamondRight);
							}
							else if ((int)m_status == 1)
							{
								((PictureBox)this).set_Image((Image)(object)_bitmapPaneDiamondTop);
							}
							else if ((int)m_status == 2)
							{
								((PictureBox)this).set_Image((Image)(object)_bitmapPaneDiamondBottom);
							}
							else if ((int)m_status == 5)
							{
								((PictureBox)this).set_Image((Image)(object)_bitmapPaneDiamondFill);
							}
						}
					}

					public PaneIndicator()
						: this()
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0029: Unknown result type (might be due to invalid IL or missing references)
						//IL_0033: Expected O, but got Unknown
						((PictureBox)this).set_SizeMode((PictureBoxSizeMode)2);
						((PictureBox)this).set_Image((Image)(object)_bitmapPaneDiamond);
						((Control)this).set_Region(new Region(DisplayingGraphicsPath));
					}

					public DockStyle HitTest(Point pt)
					{
						//IL_000f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0016: Unknown result type (might be due to invalid IL or missing references)
						//IL_0017: Unknown result type (might be due to invalid IL or missing references)
						//IL_001c: Unknown result type (might be due to invalid IL or missing references)
						//IL_001f: Unknown result type (might be due to invalid IL or missing references)
						//IL_0024: Unknown result type (might be due to invalid IL or missing references)
						//IL_0027: Unknown result type (might be due to invalid IL or missing references)
						//IL_0035: Unknown result type (might be due to invalid IL or missing references)
						//IL_005e: Unknown result type (might be due to invalid IL or missing references)
						//IL_008a: Unknown result type (might be due to invalid IL or missing references)
						//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
						//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
						//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
						if (!((Control)this).get_Visible())
						{
							return (DockStyle)0;
						}
						pt = ((Control)this).PointToClient(pt);
						Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
						if (!((Rectangle)(ref clientRectangle)).Contains(pt))
						{
							return (DockStyle)0;
						}
						for (int i = _hotSpots.GetLowerBound(0); i <= _hotSpots.GetUpperBound(0); i++)
						{
							if (_bitmapPaneDiamondHotSpot.GetPixel(((Point)(ref pt)).get_X(), ((Point)(ref pt)).get_Y()) == _bitmapPaneDiamondHotSpotIndex.GetPixel(_hotSpots[i].X, _hotSpots[i].Y))
							{
								return _hotSpots[i].DockStyle;
							}
						}
						return (DockStyle)0;
					}
				}

				private int _PanelIndicatorMargin = 10;

				private DockDragHandler m_dragHandler;

				private PaneIndicator m_paneDiamond = null;

				private PanelIndicator m_panelLeft = null;

				private PanelIndicator m_panelRight = null;

				private PanelIndicator m_panelTop = null;

				private PanelIndicator m_panelBottom = null;

				private PanelIndicator m_panelFill = null;

				private bool m_fullPanelEdge = false;

				private DockPane m_dockPane = null;

				private IHitTest m_hitTest = null;

				private PaneIndicator PaneDiamond
				{
					get
					{
						if (m_paneDiamond == null)
						{
							m_paneDiamond = new PaneIndicator();
						}
						return m_paneDiamond;
					}
				}

				private PanelIndicator PanelLeft
				{
					get
					{
						if (m_panelLeft == null)
						{
							m_panelLeft = new PanelIndicator((DockStyle)3);
						}
						return m_panelLeft;
					}
				}

				private PanelIndicator PanelRight
				{
					get
					{
						if (m_panelRight == null)
						{
							m_panelRight = new PanelIndicator((DockStyle)4);
						}
						return m_panelRight;
					}
				}

				private PanelIndicator PanelTop
				{
					get
					{
						if (m_panelTop == null)
						{
							m_panelTop = new PanelIndicator((DockStyle)1);
						}
						return m_panelTop;
					}
				}

				private PanelIndicator PanelBottom
				{
					get
					{
						if (m_panelBottom == null)
						{
							m_panelBottom = new PanelIndicator((DockStyle)2);
						}
						return m_panelBottom;
					}
				}

				private PanelIndicator PanelFill
				{
					get
					{
						if (m_panelFill == null)
						{
							m_panelFill = new PanelIndicator((DockStyle)5);
						}
						return m_panelFill;
					}
				}

				public bool FullPanelEdge
				{
					get
					{
						return m_fullPanelEdge;
					}
					set
					{
						if (m_fullPanelEdge != value)
						{
							m_fullPanelEdge = value;
							RefreshChanges();
						}
					}
				}

				public DockDragHandler DragHandler => m_dragHandler;

				public DockPanel DockPanel => DragHandler.DockPanel;

				public DockPane DockPane
				{
					get
					{
						return m_dockPane;
					}
					internal set
					{
						if (m_dockPane != value)
						{
							DockPane displayingPane = DisplayingPane;
							m_dockPane = value;
							if (displayingPane != DisplayingPane)
							{
								RefreshChanges();
							}
						}
					}
				}

				private IHitTest HitTestResult
				{
					get
					{
						return m_hitTest;
					}
					set
					{
						if (m_hitTest != value)
						{
							if (m_hitTest != null)
							{
								m_hitTest.Status = (DockStyle)0;
							}
							m_hitTest = value;
						}
					}
				}

				private DockPane DisplayingPane => ShouldPaneDiamondVisible() ? DockPane : null;

				public DockIndicator(DockDragHandler dragHandler)
				{
					//IL_009f: Unknown result type (might be due to invalid IL or missing references)
					//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00ae: Expected O, but got Unknown
					m_dragHandler = dragHandler;
					((Control)this).get_Controls().AddRange((Control[])(object)new Control[6]
					{
						(Control)PaneDiamond,
						(Control)PanelLeft,
						(Control)PanelRight,
						(Control)PanelTop,
						(Control)PanelBottom,
						(Control)PanelFill
					});
					((Control)this).set_Region(new Region(Rectangle.Empty));
				}

				private void RefreshChanges()
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_000c: Expected O, but got Unknown
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					//IL_002c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0034: Unknown result type (might be due to invalid IL or missing references)
					//IL_0035: Unknown result type (might be due to invalid IL or missing references)
					//IL_003a: Unknown result type (might be due to invalid IL or missing references)
					//IL_003f: Unknown result type (might be due to invalid IL or missing references)
					//IL_007d: Unknown result type (might be due to invalid IL or missing references)
					//IL_009c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0109: Unknown result type (might be due to invalid IL or missing references)
					//IL_0128: Unknown result type (might be due to invalid IL or missing references)
					//IL_0182: Unknown result type (might be due to invalid IL or missing references)
					//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
					//IL_0210: Unknown result type (might be due to invalid IL or missing references)
					//IL_022f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0268: Unknown result type (might be due to invalid IL or missing references)
					//IL_026d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0272: Unknown result type (might be due to invalid IL or missing references)
					//IL_0277: Unknown result type (might be due to invalid IL or missing references)
					//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
					//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
					//IL_0310: Unknown result type (might be due to invalid IL or missing references)
					//IL_0315: Unknown result type (might be due to invalid IL or missing references)
					//IL_031a: Unknown result type (might be due to invalid IL or missing references)
					//IL_031f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0361: Unknown result type (might be due to invalid IL or missing references)
					//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
					//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
					//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
					//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
					//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
					//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
					//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
					//IL_0406: Unknown result type (might be due to invalid IL or missing references)
					//IL_040d: Expected O, but got Unknown
					Region val = new Region(Rectangle.Empty);
					Rectangle val2 = (FullPanelEdge ? DockPanel.DockArea : DockPanel.DocumentWindowBounds);
					val2 = ((Control)this).RectangleToClient(((Control)DockPanel).RectangleToScreen(val2));
					if (ShouldPanelIndicatorVisible(DockState.DockLeft))
					{
						((Control)PanelLeft).set_Location(new Point(((Rectangle)(ref val2)).get_X() + _PanelIndicatorMargin, ((Rectangle)(ref val2)).get_Y() + (((Rectangle)(ref val2)).get_Height() - ((Control)PanelRight).get_Height()) / 2));
						((Control)PanelLeft).set_Visible(true);
						val.Union(((Control)PanelLeft).get_Bounds());
					}
					else
					{
						((Control)PanelLeft).set_Visible(false);
					}
					if (ShouldPanelIndicatorVisible(DockState.DockRight))
					{
						((Control)PanelRight).set_Location(new Point(((Rectangle)(ref val2)).get_X() + ((Rectangle)(ref val2)).get_Width() - ((Control)PanelRight).get_Width() - _PanelIndicatorMargin, ((Rectangle)(ref val2)).get_Y() + (((Rectangle)(ref val2)).get_Height() - ((Control)PanelRight).get_Height()) / 2));
						((Control)PanelRight).set_Visible(true);
						val.Union(((Control)PanelRight).get_Bounds());
					}
					else
					{
						((Control)PanelRight).set_Visible(false);
					}
					if (ShouldPanelIndicatorVisible(DockState.DockTop))
					{
						((Control)PanelTop).set_Location(new Point(((Rectangle)(ref val2)).get_X() + (((Rectangle)(ref val2)).get_Width() - ((Control)PanelTop).get_Width()) / 2, ((Rectangle)(ref val2)).get_Y() + _PanelIndicatorMargin));
						((Control)PanelTop).set_Visible(true);
						val.Union(((Control)PanelTop).get_Bounds());
					}
					else
					{
						((Control)PanelTop).set_Visible(false);
					}
					if (ShouldPanelIndicatorVisible(DockState.DockBottom))
					{
						((Control)PanelBottom).set_Location(new Point(((Rectangle)(ref val2)).get_X() + (((Rectangle)(ref val2)).get_Width() - ((Control)PanelBottom).get_Width()) / 2, ((Rectangle)(ref val2)).get_Y() + ((Rectangle)(ref val2)).get_Height() - ((Control)PanelBottom).get_Height() - _PanelIndicatorMargin));
						((Control)PanelBottom).set_Visible(true);
						val.Union(((Control)PanelBottom).get_Bounds());
					}
					else
					{
						((Control)PanelBottom).set_Visible(false);
					}
					if (ShouldPanelIndicatorVisible(DockState.Document))
					{
						Rectangle val3 = ((Control)this).RectangleToClient(((Control)DockPanel).RectangleToScreen(DockPanel.DocumentWindowBounds));
						((Control)PanelFill).set_Location(new Point(((Rectangle)(ref val3)).get_X() + (((Rectangle)(ref val3)).get_Width() - ((Control)PanelFill).get_Width()) / 2, ((Rectangle)(ref val3)).get_Y() + (((Rectangle)(ref val3)).get_Height() - ((Control)PanelFill).get_Height()) / 2));
						((Control)PanelFill).set_Visible(true);
						val.Union(((Control)PanelFill).get_Bounds());
					}
					else
					{
						((Control)PanelFill).set_Visible(false);
					}
					if (ShouldPaneDiamondVisible())
					{
						Rectangle val4 = ((Control)this).RectangleToClient(((Control)DockPane).RectangleToScreen(((Control)DockPane).get_ClientRectangle()));
						((Control)PaneDiamond).set_Location(new Point(((Rectangle)(ref val4)).get_Left() + (((Rectangle)(ref val4)).get_Width() - ((Control)PaneDiamond).get_Width()) / 2, ((Rectangle)(ref val4)).get_Top() + (((Rectangle)(ref val4)).get_Height() - ((Control)PaneDiamond).get_Height()) / 2));
						((Control)PaneDiamond).set_Visible(true);
						object obj = PaneIndicator.DisplayingGraphicsPath.Clone();
						GraphicsPath val5 = obj as GraphicsPath;
						try
						{
							Point[] array = (Point[])(object)new Point[3]
							{
								new Point(((Control)PaneDiamond).get_Left(), ((Control)PaneDiamond).get_Top()),
								new Point(((Control)PaneDiamond).get_Right(), ((Control)PaneDiamond).get_Top()),
								new Point(((Control)PaneDiamond).get_Left(), ((Control)PaneDiamond).get_Bottom())
							};
							Matrix val6 = new Matrix(((Control)PaneDiamond).get_ClientRectangle(), array);
							try
							{
								val5.Transform(val6);
							}
							finally
							{
								((IDisposable)val6)?.Dispose();
							}
							val.Union(val5);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
					}
					else
					{
						((Control)PaneDiamond).set_Visible(false);
					}
					((Control)this).set_Region(val);
				}

				private bool ShouldPanelIndicatorVisible(DockState dockState)
				{
					if (!((Control)this).get_Visible())
					{
						return false;
					}
					if (((Control)DockPanel.DockWindows[dockState]).get_Visible())
					{
						return false;
					}
					return DragHandler.DragSource.IsDockStateValid(dockState);
				}

				private bool ShouldPaneDiamondVisible()
				{
					if (DockPane == null)
					{
						return false;
					}
					if (!DockPanel.AllowEndUserNestedDocking)
					{
						return false;
					}
					return DragHandler.DragSource.CanDockTo(DockPane);
				}

				public override void Show(bool bActivate)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					((Control)this).set_Bounds(GetAllScreenBounds());
					base.Show(bActivate);
					RefreshChanges();
				}

				public void TestDrop()
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0006: Unknown result type (might be due to invalid IL or missing references)
					//IL_0008: Unknown result type (might be due to invalid IL or missing references)
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_0021: Unknown result type (might be due to invalid IL or missing references)
					//IL_0027: Invalid comparison between Unknown and I4
					//IL_0045: Unknown result type (might be due to invalid IL or missing references)
					//IL_0046: Unknown result type (might be due to invalid IL or missing references)
					//IL_004c: Invalid comparison between Unknown and I4
					//IL_006a: Unknown result type (might be due to invalid IL or missing references)
					//IL_006b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0071: Invalid comparison between Unknown and I4
					//IL_008c: Unknown result type (might be due to invalid IL or missing references)
					//IL_008d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0093: Invalid comparison between Unknown and I4
					//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b7: Invalid comparison between Unknown and I4
					//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
					//IL_00db: Invalid comparison between Unknown and I4
					//IL_0135: Unknown result type (might be due to invalid IL or missing references)
					//IL_0159: Unknown result type (might be due to invalid IL or missing references)
					Point mousePosition = Control.get_MousePosition();
					DockPane = DockHelper.PaneAtPoint(mousePosition, DockPanel);
					if ((int)TestDrop(PanelLeft, mousePosition) > 0)
					{
						HitTestResult = PanelLeft;
					}
					else if ((int)TestDrop(PanelRight, mousePosition) > 0)
					{
						HitTestResult = PanelRight;
					}
					else if ((int)TestDrop(PanelTop, mousePosition) > 0)
					{
						HitTestResult = PanelTop;
					}
					else if ((int)TestDrop(PanelBottom, mousePosition) > 0)
					{
						HitTestResult = PanelBottom;
					}
					else if ((int)TestDrop(PanelFill, mousePosition) > 0)
					{
						HitTestResult = PanelFill;
					}
					else if ((int)TestDrop(PaneDiamond, mousePosition) > 0)
					{
						HitTestResult = PaneDiamond;
					}
					else
					{
						HitTestResult = null;
					}
					if (HitTestResult != null)
					{
						if (HitTestResult is PaneIndicator)
						{
							DragHandler.Outline.Show(DockPane, HitTestResult.Status);
						}
						else
						{
							DragHandler.Outline.Show(DockPanel, HitTestResult.Status, FullPanelEdge);
						}
					}
				}

				private static DockStyle TestDrop(IHitTest hitTest, Point pt)
				{
					//IL_0003: Unknown result type (might be due to invalid IL or missing references)
					//IL_0004: Unknown result type (might be due to invalid IL or missing references)
					//IL_0009: Unknown result type (might be due to invalid IL or missing references)
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					return hitTest.Status = hitTest.HitTest(pt);
				}

				private static Rectangle GetAllScreenBounds()
				{
					//IL_0020: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					//IL_0127: Unknown result type (might be due to invalid IL or missing references)
					//IL_0128: Unknown result type (might be due to invalid IL or missing references)
					//IL_012c: Unknown result type (might be due to invalid IL or missing references)
					Rectangle result = default(Rectangle);
					((Rectangle)(ref result))._002Ector(0, 0, 0, 0);
					Screen[] allScreens = Screen.get_AllScreens();
					foreach (Screen val in allScreens)
					{
						Rectangle bounds = val.get_Bounds();
						if (((Rectangle)(ref bounds)).get_Left() < ((Rectangle)(ref result)).get_Left())
						{
							((Rectangle)(ref result)).set_Width(((Rectangle)(ref result)).get_Width() + (((Rectangle)(ref result)).get_Left() - ((Rectangle)(ref bounds)).get_Left()));
							((Rectangle)(ref result)).set_X(((Rectangle)(ref bounds)).get_X());
						}
						if (((Rectangle)(ref bounds)).get_Right() > ((Rectangle)(ref result)).get_Right())
						{
							((Rectangle)(ref result)).set_Width(((Rectangle)(ref result)).get_Width() + (((Rectangle)(ref bounds)).get_Right() - ((Rectangle)(ref result)).get_Right()));
						}
						if (((Rectangle)(ref bounds)).get_Top() < ((Rectangle)(ref result)).get_Top())
						{
							((Rectangle)(ref result)).set_Height(((Rectangle)(ref result)).get_Height() + (((Rectangle)(ref result)).get_Top() - ((Rectangle)(ref bounds)).get_Top()));
							((Rectangle)(ref result)).set_Y(((Rectangle)(ref bounds)).get_Y());
						}
						if (((Rectangle)(ref bounds)).get_Bottom() > ((Rectangle)(ref result)).get_Bottom())
						{
							((Rectangle)(ref result)).set_Height(((Rectangle)(ref result)).get_Height() + (((Rectangle)(ref bounds)).get_Bottom() - ((Rectangle)(ref result)).get_Bottom()));
						}
					}
					return result;
				}
			}

			private class DockOutline : DockOutlineBase
			{
				private DragForm m_dragForm;

				private DragForm DragForm => m_dragForm;

				public DockOutline()
				{
					//IL_0014: Unknown result type (might be due to invalid IL or missing references)
					//IL_0025: Unknown result type (might be due to invalid IL or missing references)
					m_dragForm = new DragForm();
					SetDragForm(Rectangle.Empty);
					((Control)DragForm).set_BackColor(SystemColors.get_ActiveCaption());
					((Form)DragForm).set_Opacity(0.5);
					DragForm.Show(bActivate: false);
				}

				protected override void OnShow()
				{
					CalculateRegion();
				}

				protected override void OnClose()
				{
					((Form)DragForm).Close();
				}

				private void CalculateRegion()
				{
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_0027: Unknown result type (might be due to invalid IL or missing references)
					//IL_0053: Unknown result type (might be due to invalid IL or missing references)
					//IL_008a: Unknown result type (might be due to invalid IL or missing references)
					if (!base.SameAsOldValue)
					{
						Rectangle floatWindowBounds = base.FloatWindowBounds;
						if (!((Rectangle)(ref floatWindowBounds)).get_IsEmpty())
						{
							SetOutline(base.FloatWindowBounds);
						}
						else if (base.DockTo is DockPanel)
						{
							SetOutline(base.DockTo as DockPanel, base.Dock, base.ContentIndex != 0);
						}
						else if (base.DockTo is DockPane)
						{
							SetOutline(base.DockTo as DockPane, base.Dock, base.ContentIndex);
						}
						else
						{
							SetOutline();
						}
					}
				}

				private void SetOutline()
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					SetDragForm(Rectangle.Empty);
				}

				private void SetOutline(Rectangle floatWindowBounds)
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					SetDragForm(floatWindowBounds);
				}

				private void SetOutline(DockPanel dockPanel, DockStyle dock, bool fullPanelEdge)
				{
					//IL_0005: Unknown result type (might be due to invalid IL or missing references)
					//IL_000d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0012: Unknown result type (might be due to invalid IL or missing references)
					//IL_0018: Unknown result type (might be due to invalid IL or missing references)
					//IL_001d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_002a: Invalid comparison between Unknown and I4
					//IL_005c: Unknown result type (might be due to invalid IL or missing references)
					//IL_005e: Invalid comparison between Unknown and I4
					//IL_0096: Unknown result type (might be due to invalid IL or missing references)
					//IL_0098: Invalid comparison between Unknown and I4
					//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
					//IL_00cd: Invalid comparison between Unknown and I4
					//IL_0104: Unknown result type (might be due to invalid IL or missing references)
					//IL_0106: Invalid comparison between Unknown and I4
					//IL_0110: Unknown result type (might be due to invalid IL or missing references)
					//IL_0115: Unknown result type (might be due to invalid IL or missing references)
					//IL_011b: Unknown result type (might be due to invalid IL or missing references)
					//IL_0120: Unknown result type (might be due to invalid IL or missing references)
					//IL_012d: Unknown result type (might be due to invalid IL or missing references)
					Rectangle dragForm = (fullPanelEdge ? dockPanel.DockArea : dockPanel.DocumentWindowBounds);
					((Rectangle)(ref dragForm)).set_Location(((Control)dockPanel).PointToScreen(((Rectangle)(ref dragForm)).get_Location()));
					if ((int)dock == 1)
					{
						int dockWindowSize = dockPanel.GetDockWindowSize(DockState.DockTop);
						((Rectangle)(ref dragForm))._002Ector(((Rectangle)(ref dragForm)).get_X(), ((Rectangle)(ref dragForm)).get_Y(), ((Rectangle)(ref dragForm)).get_Width(), dockWindowSize);
					}
					else if ((int)dock == 2)
					{
						int dockWindowSize2 = dockPanel.GetDockWindowSize(DockState.DockBottom);
						((Rectangle)(ref dragForm))._002Ector(((Rectangle)(ref dragForm)).get_X(), ((Rectangle)(ref dragForm)).get_Bottom() - dockWindowSize2, ((Rectangle)(ref dragForm)).get_Width(), dockWindowSize2);
					}
					else if ((int)dock == 3)
					{
						int dockWindowSize3 = dockPanel.GetDockWindowSize(DockState.DockLeft);
						((Rectangle)(ref dragForm))._002Ector(((Rectangle)(ref dragForm)).get_X(), ((Rectangle)(ref dragForm)).get_Y(), dockWindowSize3, ((Rectangle)(ref dragForm)).get_Height());
					}
					else if ((int)dock == 4)
					{
						int dockWindowSize4 = dockPanel.GetDockWindowSize(DockState.DockRight);
						((Rectangle)(ref dragForm))._002Ector(((Rectangle)(ref dragForm)).get_Right() - dockWindowSize4, ((Rectangle)(ref dragForm)).get_Y(), dockWindowSize4, ((Rectangle)(ref dragForm)).get_Height());
					}
					else if ((int)dock == 5)
					{
						dragForm = dockPanel.DocumentWindowBounds;
						((Rectangle)(ref dragForm)).set_Location(((Control)dockPanel).PointToScreen(((Rectangle)(ref dragForm)).get_Location()));
					}
					SetDragForm(dragForm);
				}

				private void SetOutline(DockPane pane, DockStyle dock, int contentIndex)
				{
					//IL_0001: Unknown result type (might be due to invalid IL or missing references)
					//IL_0003: Invalid comparison between Unknown and I4
					//IL_0011: Unknown result type (might be due to invalid IL or missing references)
					//IL_0016: Unknown result type (might be due to invalid IL or missing references)
					//IL_0017: Unknown result type (might be due to invalid IL or missing references)
					//IL_0019: Invalid comparison between Unknown and I4
					//IL_0037: Unknown result type (might be due to invalid IL or missing references)
					//IL_0039: Invalid comparison between Unknown and I4
					//IL_0057: Unknown result type (might be due to invalid IL or missing references)
					//IL_0059: Invalid comparison between Unknown and I4
					//IL_005b: Unknown result type (might be due to invalid IL or missing references)
					//IL_005d: Invalid comparison between Unknown and I4
					//IL_0080: Unknown result type (might be due to invalid IL or missing references)
					//IL_0082: Invalid comparison between Unknown and I4
					//IL_0084: Unknown result type (might be due to invalid IL or missing references)
					//IL_0086: Invalid comparison between Unknown and I4
					//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
					//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
					//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
					//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
					//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
					//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
					//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
					//IL_0115: Unknown result type (might be due to invalid IL or missing references)
					//IL_011a: Unknown result type (might be due to invalid IL or missing references)
					//IL_0143: Unknown result type (might be due to invalid IL or missing references)
					//IL_014f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0154: Unknown result type (might be due to invalid IL or missing references)
					//IL_0163: Unknown result type (might be due to invalid IL or missing references)
					//IL_0168: Unknown result type (might be due to invalid IL or missing references)
					//IL_0177: Unknown result type (might be due to invalid IL or missing references)
					//IL_017c: Unknown result type (might be due to invalid IL or missing references)
					//IL_0181: Unknown result type (might be due to invalid IL or missing references)
					//IL_0188: Expected O, but got Unknown
					//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
					//IL_01ac: Expected O, but got Unknown
					//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
					if ((int)dock != 5)
					{
						Rectangle displayingRectangle = pane.DisplayingRectangle;
						if ((int)dock == 4)
						{
							((Rectangle)(ref displayingRectangle)).set_X(((Rectangle)(ref displayingRectangle)).get_X() + ((Rectangle)(ref displayingRectangle)).get_Width() / 2);
						}
						if ((int)dock == 2)
						{
							((Rectangle)(ref displayingRectangle)).set_Y(((Rectangle)(ref displayingRectangle)).get_Y() + ((Rectangle)(ref displayingRectangle)).get_Height() / 2);
						}
						if ((int)dock == 3 || (int)dock == 4)
						{
							((Rectangle)(ref displayingRectangle)).set_Width(((Rectangle)(ref displayingRectangle)).get_Width() - ((Rectangle)(ref displayingRectangle)).get_Width() / 2);
						}
						if ((int)dock == 1 || (int)dock == 2)
						{
							((Rectangle)(ref displayingRectangle)).set_Height(((Rectangle)(ref displayingRectangle)).get_Height() - ((Rectangle)(ref displayingRectangle)).get_Height() / 2);
						}
						((Rectangle)(ref displayingRectangle)).set_Location(((Control)pane).PointToScreen(((Rectangle)(ref displayingRectangle)).get_Location()));
						SetDragForm(displayingRectangle);
						return;
					}
					if (contentIndex == -1)
					{
						Rectangle displayingRectangle2 = pane.DisplayingRectangle;
						((Rectangle)(ref displayingRectangle2)).set_Location(((Control)pane).PointToScreen(((Rectangle)(ref displayingRectangle2)).get_Location()));
						SetDragForm(displayingRectangle2);
						return;
					}
					GraphicsPath outline = pane.TabStripControl.GetOutline(contentIndex);
					try
					{
						RectangleF bounds = outline.GetBounds();
						Rectangle val = default(Rectangle);
						((Rectangle)(ref val))._002Ector((int)((RectangleF)(ref bounds)).get_X(), (int)((RectangleF)(ref bounds)).get_Y(), (int)((RectangleF)(ref bounds)).get_Width(), (int)((RectangleF)(ref bounds)).get_Height());
						Matrix val2 = new Matrix(val, (Point[])(object)new Point[3]
						{
							new Point(0, 0),
							new Point(((Rectangle)(ref val)).get_Width(), 0),
							new Point(0, ((Rectangle)(ref val)).get_Height())
						});
						try
						{
							outline.Transform(val2);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
						Region region = new Region(outline);
						SetDragForm(val, region);
					}
					finally
					{
						((IDisposable)outline)?.Dispose();
					}
				}

				private void SetDragForm(Rectangle rect)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					//IL_0023: Unknown result type (might be due to invalid IL or missing references)
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_0032: Expected O, but got Unknown
					((Control)DragForm).set_Bounds(rect);
					if (rect == Rectangle.Empty)
					{
						((Control)DragForm).set_Region(new Region(Rectangle.Empty));
					}
					else if (((Control)DragForm).get_Region() != null)
					{
						((Control)DragForm).set_Region((Region)null);
					}
				}

				private void SetDragForm(Rectangle rect, Region region)
				{
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					((Control)DragForm).set_Bounds(rect);
					((Control)DragForm).set_Region(region);
				}
			}

			private DockOutlineBase m_outline;

			private DockIndicator m_indicator;

			private Rectangle m_floatOutlineBounds;

			public new IDockDragSource DragSource
			{
				get
				{
					return base.DragSource as IDockDragSource;
				}
				set
				{
					base.DragSource = value;
				}
			}

			public DockOutlineBase Outline
			{
				get
				{
					return m_outline;
				}
				private set
				{
					m_outline = value;
				}
			}

			private DockIndicator Indicator
			{
				get
				{
					return m_indicator;
				}
				set
				{
					m_indicator = value;
				}
			}

			private Rectangle FloatOutlineBounds
			{
				get
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0007: Unknown result type (might be due to invalid IL or missing references)
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					return m_floatOutlineBounds;
				}
				set
				{
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_0003: Unknown result type (might be due to invalid IL or missing references)
					m_floatOutlineBounds = value;
				}
			}

			public DockDragHandler(DockPanel panel)
				: base(panel)
			{
			}

			public void BeginDrag(IDockDragSource dragSource)
			{
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				DragSource = dragSource;
				if (!BeginDrag())
				{
					DragSource = null;
					return;
				}
				Outline = new DockOutline();
				Indicator = new DockIndicator(this);
				Indicator.Show(bActivate: false);
				FloatOutlineBounds = DragSource.BeginDrag(base.StartMousePosition);
			}

			protected override void OnDragging()
			{
				TestDrop();
			}

			protected override void OnEndDrag(bool abort)
			{
				base.DockPanel.SuspendLayout(allWindows: true);
				Outline.Close();
				((Form)Indicator).Close();
				EndDrag(abort);
				base.DockPanel.ResumeLayout(performLayout: true, allWindows: true);
				DragSource = null;
			}

			private void TestDrop()
			{
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Invalid comparison between Unknown and I4
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Invalid comparison between Unknown and I4
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
				//IL_013a: Unknown result type (might be due to invalid IL or missing references)
				//IL_013f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0143: Unknown result type (might be due to invalid IL or missing references)
				//IL_0148: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_0157: Unknown result type (might be due to invalid IL or missing references)
				//IL_0161: Unknown result type (might be due to invalid IL or missing references)
				//IL_0166: Unknown result type (might be due to invalid IL or missing references)
				//IL_0170: Unknown result type (might be due to invalid IL or missing references)
				//IL_0175: Unknown result type (might be due to invalid IL or missing references)
				//IL_018b: Unknown result type (might be due to invalid IL or missing references)
				Outline.FlagTestDrop = false;
				Indicator.FullPanelEdge = (Control.get_ModifierKeys() & 0x10000) > 0;
				if ((Control.get_ModifierKeys() & 0x20000) == 0)
				{
					Indicator.TestDrop();
					if (!Outline.FlagTestDrop)
					{
						DockPane dockPane = DockHelper.PaneAtPoint(Control.get_MousePosition(), base.DockPanel);
						if (dockPane != null && DragSource.IsDockStateValid(dockPane.DockState))
						{
							dockPane.TestDrop(DragSource, Outline);
						}
					}
					if (!Outline.FlagTestDrop && DragSource.IsDockStateValid(DockState.Float))
					{
						DockHelper.FloatWindowAtPoint(Control.get_MousePosition(), base.DockPanel)?.TestDrop(DragSource, Outline);
					}
				}
				else
				{
					Indicator.DockPane = DockHelper.PaneAtPoint(Control.get_MousePosition(), base.DockPanel);
				}
				if (!Outline.FlagTestDrop && DragSource.IsDockStateValid(DockState.Float))
				{
					Rectangle floatOutlineBounds = FloatOutlineBounds;
					Point val = Control.get_MousePosition();
					int x = ((Point)(ref val)).get_X();
					val = base.StartMousePosition;
					int num = x - ((Point)(ref val)).get_X();
					val = Control.get_MousePosition();
					int y = ((Point)(ref val)).get_Y();
					val = base.StartMousePosition;
					((Rectangle)(ref floatOutlineBounds)).Offset(num, y - ((Point)(ref val)).get_Y());
					Outline.Show(floatOutlineBounds);
				}
				if (!Outline.FlagTestDrop)
				{
					Cursor.set_Current(Cursors.get_No());
					Outline.Show();
				}
				else
				{
					Cursor.set_Current(DragControl.get_Cursor());
				}
			}

			private void EndDrag(bool abort)
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0031: Unknown result type (might be due to invalid IL or missing references)
				//IL_0079: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
				if (!abort)
				{
					Rectangle floatWindowBounds = Outline.FloatWindowBounds;
					if (!((Rectangle)(ref floatWindowBounds)).get_IsEmpty())
					{
						DragSource.FloatAt(Outline.FloatWindowBounds);
					}
					else if (Outline.DockTo is DockPane)
					{
						DockPane pane = Outline.DockTo as DockPane;
						DragSource.DockTo(pane, Outline.Dock, Outline.ContentIndex);
					}
					else if (Outline.DockTo is DockPanel)
					{
						DockPanel dockPanel = Outline.DockTo as DockPanel;
						dockPanel.UpdateDockWindowZOrder(Outline.Dock, Outline.FlagFullEdge);
						DragSource.DockTo(dockPanel, Outline.Dock);
					}
				}
			}
		}

		private static class Persistor
		{
			private class DummyContent : DockContent
			{
			}

			private struct DockPanelStruct
			{
				private double m_dockLeftPortion;

				private double m_dockRightPortion;

				private double m_dockTopPortion;

				private double m_dockBottomPortion;

				private int m_indexActiveDocumentPane;

				private int m_indexActivePane;

				public double DockLeftPortion
				{
					get
					{
						return m_dockLeftPortion;
					}
					set
					{
						m_dockLeftPortion = value;
					}
				}

				public double DockRightPortion
				{
					get
					{
						return m_dockRightPortion;
					}
					set
					{
						m_dockRightPortion = value;
					}
				}

				public double DockTopPortion
				{
					get
					{
						return m_dockTopPortion;
					}
					set
					{
						m_dockTopPortion = value;
					}
				}

				public double DockBottomPortion
				{
					get
					{
						return m_dockBottomPortion;
					}
					set
					{
						m_dockBottomPortion = value;
					}
				}

				public int IndexActiveDocumentPane
				{
					get
					{
						return m_indexActiveDocumentPane;
					}
					set
					{
						m_indexActiveDocumentPane = value;
					}
				}

				public int IndexActivePane
				{
					get
					{
						return m_indexActivePane;
					}
					set
					{
						m_indexActivePane = value;
					}
				}
			}

			private struct ContentStruct
			{
				private string m_persistString;

				private double m_autoHidePortion;

				private bool m_isHidden;

				private bool m_isFloat;

				public string PersistString
				{
					get
					{
						return m_persistString;
					}
					set
					{
						m_persistString = value;
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
						m_autoHidePortion = value;
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
						m_isHidden = value;
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
						m_isFloat = value;
					}
				}
			}

			private struct PaneStruct
			{
				private DockState m_dockState;

				private int m_indexActiveContent;

				private int[] m_indexContents;

				private int m_zOrderIndex;

				public DockState DockState
				{
					get
					{
						return m_dockState;
					}
					set
					{
						m_dockState = value;
					}
				}

				public int IndexActiveContent
				{
					get
					{
						return m_indexActiveContent;
					}
					set
					{
						m_indexActiveContent = value;
					}
				}

				public int[] IndexContents
				{
					get
					{
						return m_indexContents;
					}
					set
					{
						m_indexContents = value;
					}
				}

				public int ZOrderIndex
				{
					get
					{
						return m_zOrderIndex;
					}
					set
					{
						m_zOrderIndex = value;
					}
				}
			}

			private struct NestedPane
			{
				private int m_indexPane;

				private int m_indexPrevPane;

				private DockAlignment m_alignment;

				private double m_proportion;

				public int IndexPane
				{
					get
					{
						return m_indexPane;
					}
					set
					{
						m_indexPane = value;
					}
				}

				public int IndexPrevPane
				{
					get
					{
						return m_indexPrevPane;
					}
					set
					{
						m_indexPrevPane = value;
					}
				}

				public DockAlignment Alignment
				{
					get
					{
						return m_alignment;
					}
					set
					{
						m_alignment = value;
					}
				}

				public double Proportion
				{
					get
					{
						return m_proportion;
					}
					set
					{
						m_proportion = value;
					}
				}
			}

			private struct DockWindowStruct
			{
				private DockState m_dockState;

				private int m_zOrderIndex;

				private NestedPane[] m_nestedPanes;

				public DockState DockState
				{
					get
					{
						return m_dockState;
					}
					set
					{
						m_dockState = value;
					}
				}

				public int ZOrderIndex
				{
					get
					{
						return m_zOrderIndex;
					}
					set
					{
						m_zOrderIndex = value;
					}
				}

				public NestedPane[] NestedPanes
				{
					get
					{
						return m_nestedPanes;
					}
					set
					{
						m_nestedPanes = value;
					}
				}
			}

			private struct FloatWindowStruct
			{
				private Rectangle m_bounds;

				private int m_zOrderIndex;

				private NestedPane[] m_nestedPanes;

				public Rectangle Bounds
				{
					get
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0007: Unknown result type (might be due to invalid IL or missing references)
						//IL_000a: Unknown result type (might be due to invalid IL or missing references)
						return m_bounds;
					}
					set
					{
						//IL_0002: Unknown result type (might be due to invalid IL or missing references)
						//IL_0003: Unknown result type (might be due to invalid IL or missing references)
						m_bounds = value;
					}
				}

				public int ZOrderIndex
				{
					get
					{
						return m_zOrderIndex;
					}
					set
					{
						m_zOrderIndex = value;
					}
				}

				public NestedPane[] NestedPanes
				{
					get
					{
						return m_nestedPanes;
					}
					set
					{
						m_nestedPanes = value;
					}
				}
			}

			private const string ConfigFileVersion = "1.0";

			private static string[] CompatibleConfigFileVersions = new string[0];

			public static void SaveAsXml(DockPanel dockPanel, string fileName)
			{
				SaveAsXml(dockPanel, fileName, Encoding.Unicode);
			}

			public static void SaveAsXml(DockPanel dockPanel, string fileName, Encoding encoding)
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Create);
				try
				{
					SaveAsXml(dockPanel, fileStream, encoding);
				}
				finally
				{
					fileStream.Close();
				}
			}

			public static void SaveAsXml(DockPanel dockPanel, Stream stream, Encoding encoding)
			{
				SaveAsXml(dockPanel, stream, encoding, upstream: false);
			}

			public static void SaveAsXml(DockPanel dockPanel, Stream stream, Encoding encoding, bool upstream)
			{
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Expected O, but got Unknown
				//IL_063b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0641: Expected O, but got Unknown
				//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
				XmlTextWriter val = new XmlTextWriter(stream, encoding);
				val.set_Formatting((Formatting)1);
				if (!upstream)
				{
					((XmlWriter)val).WriteStartDocument();
				}
				((XmlWriter)val).WriteComment(Strings.DockPanel_Persistor_XmlFileComment1);
				((XmlWriter)val).WriteComment(Strings.DockPanel_Persistor_XmlFileComment2);
				((XmlWriter)val).WriteStartElement("DockPanel");
				((XmlWriter)val).WriteAttributeString("FormatVersion", "1.0");
				((XmlWriter)val).WriteAttributeString("DockLeftPortion", dockPanel.DockLeftPortion.ToString(CultureInfo.InvariantCulture));
				((XmlWriter)val).WriteAttributeString("DockRightPortion", dockPanel.DockRightPortion.ToString(CultureInfo.InvariantCulture));
				((XmlWriter)val).WriteAttributeString("DockTopPortion", dockPanel.DockTopPortion.ToString(CultureInfo.InvariantCulture));
				((XmlWriter)val).WriteAttributeString("DockBottomPortion", dockPanel.DockBottomPortion.ToString(CultureInfo.InvariantCulture));
				((XmlWriter)val).WriteAttributeString("ActiveDocumentPane", dockPanel.Panes.IndexOf(dockPanel.ActiveDocumentPane).ToString(CultureInfo.InvariantCulture));
				((XmlWriter)val).WriteAttributeString("ActivePane", dockPanel.Panes.IndexOf(dockPanel.ActivePane).ToString(CultureInfo.InvariantCulture));
				((XmlWriter)val).WriteStartElement("Contents");
				((XmlWriter)val).WriteAttributeString("Count", dockPanel.Contents.Count.ToString(CultureInfo.InvariantCulture));
				foreach (IDockContent content in dockPanel.Contents)
				{
					((XmlWriter)val).WriteStartElement("Content");
					((XmlWriter)val).WriteAttributeString("ID", dockPanel.Contents.IndexOf(content).ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteAttributeString("PersistString", content.DockHandler.PersistString);
					((XmlWriter)val).WriteAttributeString("AutoHidePortion", content.DockHandler.AutoHidePortion.ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteAttributeString("IsHidden", content.DockHandler.IsHidden.ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteAttributeString("IsFloat", content.DockHandler.IsFloat.ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteEndElement();
				}
				((XmlWriter)val).WriteEndElement();
				((XmlWriter)val).WriteStartElement("Panes");
				((XmlWriter)val).WriteAttributeString("Count", dockPanel.Panes.Count.ToString(CultureInfo.InvariantCulture));
				foreach (DockPane pane in dockPanel.Panes)
				{
					((XmlWriter)val).WriteStartElement("Pane");
					((XmlWriter)val).WriteAttributeString("ID", dockPanel.Panes.IndexOf(pane).ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteAttributeString("DockState", pane.DockState.ToString());
					((XmlWriter)val).WriteAttributeString("ActiveContent", dockPanel.Contents.IndexOf(pane.ActiveContent).ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteStartElement("Contents");
					((XmlWriter)val).WriteAttributeString("Count", pane.Contents.Count.ToString(CultureInfo.InvariantCulture));
					foreach (IDockContent content2 in pane.Contents)
					{
						((XmlWriter)val).WriteStartElement("Content");
						((XmlWriter)val).WriteAttributeString("ID", pane.Contents.IndexOf(content2).ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteAttributeString("RefID", dockPanel.Contents.IndexOf(content2).ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteEndElement();
					}
					((XmlWriter)val).WriteEndElement();
					((XmlWriter)val).WriteEndElement();
				}
				((XmlWriter)val).WriteEndElement();
				((XmlWriter)val).WriteStartElement("DockWindows");
				int num = 0;
				foreach (DockWindow dockWindow in dockPanel.DockWindows)
				{
					((XmlWriter)val).WriteStartElement("DockWindow");
					((XmlWriter)val).WriteAttributeString("ID", num.ToString(CultureInfo.InvariantCulture));
					num++;
					((XmlWriter)val).WriteAttributeString("DockState", dockWindow.DockState.ToString());
					((XmlWriter)val).WriteAttributeString("ZOrderIndex", ((Control)dockPanel).get_Controls().IndexOf((Control)(object)dockWindow).ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteStartElement("NestedPanes");
					((XmlWriter)val).WriteAttributeString("Count", dockWindow.NestedPanes.Count.ToString(CultureInfo.InvariantCulture));
					foreach (DockPane nestedPane in dockWindow.NestedPanes)
					{
						((XmlWriter)val).WriteStartElement("Pane");
						((XmlWriter)val).WriteAttributeString("ID", dockWindow.NestedPanes.IndexOf(nestedPane).ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteAttributeString("RefID", dockPanel.Panes.IndexOf(nestedPane).ToString(CultureInfo.InvariantCulture));
						NestedDockingStatus nestedDockingStatus = nestedPane.NestedDockingStatus;
						((XmlWriter)val).WriteAttributeString("PrevPane", dockPanel.Panes.IndexOf(nestedDockingStatus.PreviousPane).ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteAttributeString("Alignment", nestedDockingStatus.Alignment.ToString());
						((XmlWriter)val).WriteAttributeString("Proportion", nestedDockingStatus.Proportion.ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteEndElement();
					}
					((XmlWriter)val).WriteEndElement();
					((XmlWriter)val).WriteEndElement();
				}
				((XmlWriter)val).WriteEndElement();
				RectangleConverter val2 = new RectangleConverter();
				((XmlWriter)val).WriteStartElement("FloatWindows");
				((XmlWriter)val).WriteAttributeString("Count", dockPanel.FloatWindows.Count.ToString(CultureInfo.InvariantCulture));
				foreach (FloatWindow floatWindow in dockPanel.FloatWindows)
				{
					((XmlWriter)val).WriteStartElement("FloatWindow");
					((XmlWriter)val).WriteAttributeString("ID", dockPanel.FloatWindows.IndexOf(floatWindow).ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteAttributeString("Bounds", ((TypeConverter)val2).ConvertToInvariantString((object)((Control)floatWindow).get_Bounds()));
					((XmlWriter)val).WriteAttributeString("ZOrderIndex", floatWindow.DockPanel.FloatWindows.IndexOf(floatWindow).ToString(CultureInfo.InvariantCulture));
					((XmlWriter)val).WriteStartElement("NestedPanes");
					((XmlWriter)val).WriteAttributeString("Count", floatWindow.NestedPanes.Count.ToString(CultureInfo.InvariantCulture));
					foreach (DockPane nestedPane2 in floatWindow.NestedPanes)
					{
						((XmlWriter)val).WriteStartElement("Pane");
						((XmlWriter)val).WriteAttributeString("ID", floatWindow.NestedPanes.IndexOf(nestedPane2).ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteAttributeString("RefID", dockPanel.Panes.IndexOf(nestedPane2).ToString(CultureInfo.InvariantCulture));
						NestedDockingStatus nestedDockingStatus2 = nestedPane2.NestedDockingStatus;
						((XmlWriter)val).WriteAttributeString("PrevPane", dockPanel.Panes.IndexOf(nestedDockingStatus2.PreviousPane).ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteAttributeString("Alignment", nestedDockingStatus2.Alignment.ToString());
						((XmlWriter)val).WriteAttributeString("Proportion", nestedDockingStatus2.Proportion.ToString(CultureInfo.InvariantCulture));
						((XmlWriter)val).WriteEndElement();
					}
					((XmlWriter)val).WriteEndElement();
					((XmlWriter)val).WriteEndElement();
				}
				((XmlWriter)val).WriteEndElement();
				((XmlWriter)val).WriteEndElement();
				if (!upstream)
				{
					((XmlWriter)val).WriteEndDocument();
					((XmlWriter)val).Close();
				}
				else
				{
					((XmlWriter)val).Flush();
				}
			}

			public static void LoadFromXml(DockPanel dockPanel, string fileName, DeserializeDockContent deserializeContent)
			{
				FileStream fileStream = new FileStream(fileName, FileMode.Open);
				try
				{
					LoadFromXml(dockPanel, fileStream, deserializeContent);
				}
				finally
				{
					fileStream.Close();
				}
			}

			public static void LoadFromXml(DockPanel dockPanel, Stream stream, DeserializeDockContent deserializeContent)
			{
				LoadFromXml(dockPanel, stream, deserializeContent, closeStream: true);
			}

			private static ContentStruct[] LoadContents(XmlTextReader xmlIn)
			{
				int num = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("Count"), CultureInfo.InvariantCulture);
				ContentStruct[] array = new ContentStruct[num];
				MoveToNextElement(xmlIn);
				for (int i = 0; i < num; i++)
				{
					int num2 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ID"), CultureInfo.InvariantCulture);
					if (((XmlReader)xmlIn).get_Name() != "Content" || num2 != i)
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
					array[i].PersistString = ((XmlReader)xmlIn).GetAttribute("PersistString");
					array[i].AutoHidePortion = Convert.ToDouble(((XmlReader)xmlIn).GetAttribute("AutoHidePortion"), CultureInfo.InvariantCulture);
					array[i].IsHidden = Convert.ToBoolean(((XmlReader)xmlIn).GetAttribute("IsHidden"), CultureInfo.InvariantCulture);
					array[i].IsFloat = Convert.ToBoolean(((XmlReader)xmlIn).GetAttribute("IsFloat"), CultureInfo.InvariantCulture);
					MoveToNextElement(xmlIn);
				}
				return array;
			}

			private static PaneStruct[] LoadPanes(XmlTextReader xmlIn)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				EnumConverter val = new EnumConverter(typeof(DockState));
				int num = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("Count"), CultureInfo.InvariantCulture);
				PaneStruct[] array = new PaneStruct[num];
				MoveToNextElement(xmlIn);
				for (int i = 0; i < num; i++)
				{
					int num2 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ID"), CultureInfo.InvariantCulture);
					if (((XmlReader)xmlIn).get_Name() != "Pane" || num2 != i)
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
					array[i].DockState = (DockState)((TypeConverter)val).ConvertFrom((object)((XmlReader)xmlIn).GetAttribute("DockState"));
					array[i].IndexActiveContent = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ActiveContent"), CultureInfo.InvariantCulture);
					array[i].ZOrderIndex = -1;
					MoveToNextElement(xmlIn);
					if (((XmlReader)xmlIn).get_Name() != "Contents")
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
					int num3 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("Count"), CultureInfo.InvariantCulture);
					array[i].IndexContents = new int[num3];
					MoveToNextElement(xmlIn);
					for (int j = 0; j < num3; j++)
					{
						int num4 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ID"), CultureInfo.InvariantCulture);
						if (((XmlReader)xmlIn).get_Name() != "Content" || num4 != j)
						{
							throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
						}
						array[i].IndexContents[j] = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("RefID"), CultureInfo.InvariantCulture);
						MoveToNextElement(xmlIn);
					}
				}
				return array;
			}

			private static DockWindowStruct[] LoadDockWindows(XmlTextReader xmlIn, DockPanel dockPanel)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Expected O, but got Unknown
				EnumConverter val = new EnumConverter(typeof(DockState));
				EnumConverter val2 = new EnumConverter(typeof(DockAlignment));
				int count = dockPanel.DockWindows.Count;
				DockWindowStruct[] array = new DockWindowStruct[count];
				MoveToNextElement(xmlIn);
				for (int i = 0; i < count; i++)
				{
					int num = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ID"), CultureInfo.InvariantCulture);
					if (((XmlReader)xmlIn).get_Name() != "DockWindow" || num != i)
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
					array[i].DockState = (DockState)((TypeConverter)val).ConvertFrom((object)((XmlReader)xmlIn).GetAttribute("DockState"));
					array[i].ZOrderIndex = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ZOrderIndex"), CultureInfo.InvariantCulture);
					MoveToNextElement(xmlIn);
					if (((XmlReader)xmlIn).get_Name() != "DockList" && ((XmlReader)xmlIn).get_Name() != "NestedPanes")
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
					int num2 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("Count"), CultureInfo.InvariantCulture);
					array[i].NestedPanes = new NestedPane[num2];
					MoveToNextElement(xmlIn);
					for (int j = 0; j < num2; j++)
					{
						int num3 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ID"), CultureInfo.InvariantCulture);
						if (((XmlReader)xmlIn).get_Name() != "Pane" || num3 != j)
						{
							throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
						}
						array[i].NestedPanes[j].IndexPane = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("RefID"), CultureInfo.InvariantCulture);
						array[i].NestedPanes[j].IndexPrevPane = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("PrevPane"), CultureInfo.InvariantCulture);
						array[i].NestedPanes[j].Alignment = (DockAlignment)((TypeConverter)val2).ConvertFrom((object)((XmlReader)xmlIn).GetAttribute("Alignment"));
						array[i].NestedPanes[j].Proportion = Convert.ToDouble(((XmlReader)xmlIn).GetAttribute("Proportion"), CultureInfo.InvariantCulture);
						MoveToNextElement(xmlIn);
					}
				}
				return array;
			}

			private static FloatWindowStruct[] LoadFloatWindows(XmlTextReader xmlIn)
			{
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Expected O, but got Unknown
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Expected O, but got Unknown
				//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
				EnumConverter val = new EnumConverter(typeof(DockAlignment));
				RectangleConverter val2 = new RectangleConverter();
				int num = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("Count"), CultureInfo.InvariantCulture);
				FloatWindowStruct[] array = new FloatWindowStruct[num];
				MoveToNextElement(xmlIn);
				for (int i = 0; i < num; i++)
				{
					int num2 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ID"), CultureInfo.InvariantCulture);
					if (((XmlReader)xmlIn).get_Name() != "FloatWindow" || num2 != i)
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
					array[i].Bounds = (Rectangle)((TypeConverter)val2).ConvertFromInvariantString(((XmlReader)xmlIn).GetAttribute("Bounds"));
					array[i].ZOrderIndex = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ZOrderIndex"), CultureInfo.InvariantCulture);
					MoveToNextElement(xmlIn);
					if (((XmlReader)xmlIn).get_Name() != "DockList" && ((XmlReader)xmlIn).get_Name() != "NestedPanes")
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
					int num3 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("Count"), CultureInfo.InvariantCulture);
					array[i].NestedPanes = new NestedPane[num3];
					MoveToNextElement(xmlIn);
					for (int j = 0; j < num3; j++)
					{
						int num4 = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("ID"), CultureInfo.InvariantCulture);
						if (((XmlReader)xmlIn).get_Name() != "Pane" || num4 != j)
						{
							throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
						}
						array[i].NestedPanes[j].IndexPane = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("RefID"), CultureInfo.InvariantCulture);
						array[i].NestedPanes[j].IndexPrevPane = Convert.ToInt32(((XmlReader)xmlIn).GetAttribute("PrevPane"), CultureInfo.InvariantCulture);
						array[i].NestedPanes[j].Alignment = (DockAlignment)((TypeConverter)val).ConvertFrom((object)((XmlReader)xmlIn).GetAttribute("Alignment"));
						array[i].NestedPanes[j].Proportion = Convert.ToDouble(((XmlReader)xmlIn).GetAttribute("Proportion"), CultureInfo.InvariantCulture);
						MoveToNextElement(xmlIn);
					}
				}
				return array;
			}

			public static void LoadFromXml(DockPanel dockPanel, Stream stream, DeserializeDockContent deserializeContent, bool closeStream)
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Expected O, but got Unknown
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0609: Unknown result type (might be due to invalid IL or missing references)
				if (dockPanel.Contents.Count != 0)
				{
					throw new InvalidOperationException(Strings.DockPanel_LoadFromXml_AlreadyInitialized);
				}
				XmlTextReader val = new XmlTextReader(stream);
				val.set_WhitespaceHandling((WhitespaceHandling)2);
				((XmlReader)val).MoveToContent();
				while (!((XmlReader)val).get_Name().Equals("DockPanel"))
				{
					if (!MoveToNextElement(val))
					{
						throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
					}
				}
				string attribute = ((XmlReader)val).GetAttribute("FormatVersion");
				if (!IsFormatVersionValid(attribute))
				{
					throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidFormatVersion);
				}
				DockPanelStruct dockPanelStruct = default(DockPanelStruct);
				dockPanelStruct.DockLeftPortion = Convert.ToDouble(((XmlReader)val).GetAttribute("DockLeftPortion"), CultureInfo.InvariantCulture);
				dockPanelStruct.DockRightPortion = Convert.ToDouble(((XmlReader)val).GetAttribute("DockRightPortion"), CultureInfo.InvariantCulture);
				dockPanelStruct.DockTopPortion = Convert.ToDouble(((XmlReader)val).GetAttribute("DockTopPortion"), CultureInfo.InvariantCulture);
				dockPanelStruct.DockBottomPortion = Convert.ToDouble(((XmlReader)val).GetAttribute("DockBottomPortion"), CultureInfo.InvariantCulture);
				dockPanelStruct.IndexActiveDocumentPane = Convert.ToInt32(((XmlReader)val).GetAttribute("ActiveDocumentPane"), CultureInfo.InvariantCulture);
				dockPanelStruct.IndexActivePane = Convert.ToInt32(((XmlReader)val).GetAttribute("ActivePane"), CultureInfo.InvariantCulture);
				MoveToNextElement(val);
				if (((XmlReader)val).get_Name() != "Contents")
				{
					throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
				}
				ContentStruct[] array = LoadContents(val);
				if (((XmlReader)val).get_Name() != "Panes")
				{
					throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
				}
				PaneStruct[] array2 = LoadPanes(val);
				if (((XmlReader)val).get_Name() != "DockWindows")
				{
					throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
				}
				DockWindowStruct[] array3 = LoadDockWindows(val, dockPanel);
				if (((XmlReader)val).get_Name() != "FloatWindows")
				{
					throw new ArgumentException(Strings.DockPanel_LoadFromXml_InvalidXmlFormat);
				}
				FloatWindowStruct[] array4 = LoadFloatWindows(val);
				if (closeStream)
				{
					((XmlReader)val).Close();
				}
				dockPanel.SuspendLayout(allWindows: true);
				dockPanel.DockLeftPortion = dockPanelStruct.DockLeftPortion;
				dockPanel.DockRightPortion = dockPanelStruct.DockRightPortion;
				dockPanel.DockTopPortion = dockPanelStruct.DockTopPortion;
				dockPanel.DockBottomPortion = dockPanelStruct.DockBottomPortion;
				int num = int.MaxValue;
				for (int i = 0; i < array3.Length; i++)
				{
					int num2 = -1;
					int num3 = -1;
					for (int j = 0; j < array3.Length; j++)
					{
						if (array3[j].ZOrderIndex > num2 && array3[j].ZOrderIndex < num)
						{
							num2 = array3[j].ZOrderIndex;
							num3 = j;
						}
					}
					((Control)dockPanel.DockWindows[array3[num3].DockState]).BringToFront();
					num = num2;
				}
				for (int k = 0; k < array.Length; k++)
				{
					IDockContent dockContent = deserializeContent(array[k].PersistString);
					if (dockContent == null)
					{
						dockContent = new DummyContent();
					}
					dockContent.DockHandler.DockPanel = dockPanel;
					dockContent.DockHandler.AutoHidePortion = array[k].AutoHidePortion;
					dockContent.DockHandler.IsHidden = true;
					dockContent.DockHandler.IsFloat = array[k].IsFloat;
				}
				for (int l = 0; l < array2.Length; l++)
				{
					DockPane dockPane = null;
					for (int m = 0; m < array2[l].IndexContents.Length; m++)
					{
						IDockContent dockContent2 = dockPanel.Contents[array2[l].IndexContents[m]];
						if (m == 0)
						{
							dockPane = dockPanel.DockPaneFactory.CreateDockPane(dockContent2, array2[l].DockState, show: false);
						}
						else if (array2[l].DockState == DockState.Float)
						{
							dockContent2.DockHandler.FloatPane = dockPane;
						}
						else
						{
							dockContent2.DockHandler.PanelPane = dockPane;
						}
					}
				}
				for (int n = 0; n < array3.Length; n++)
				{
					for (int num4 = 0; num4 < array3[n].NestedPanes.Length; num4++)
					{
						DockWindow dockWindow = dockPanel.DockWindows[array3[n].DockState];
						int indexPane = array3[n].NestedPanes[num4].IndexPane;
						DockPane dockPane2 = dockPanel.Panes[indexPane];
						int indexPrevPane = array3[n].NestedPanes[num4].IndexPrevPane;
						DockPane previousPane = ((indexPrevPane == -1) ? dockWindow.NestedPanes.GetDefaultPreviousPane(dockPane2) : dockPanel.Panes[indexPrevPane]);
						DockAlignment alignment = array3[n].NestedPanes[num4].Alignment;
						double proportion = array3[n].NestedPanes[num4].Proportion;
						dockPane2.DockTo(dockWindow, previousPane, alignment, proportion);
						if (array2[indexPane].DockState == dockWindow.DockState)
						{
							array2[indexPane].ZOrderIndex = array3[n].ZOrderIndex;
						}
					}
				}
				for (int num5 = 0; num5 < array4.Length; num5++)
				{
					FloatWindow floatWindow = null;
					for (int num6 = 0; num6 < array4[num5].NestedPanes.Length; num6++)
					{
						int indexPane2 = array4[num5].NestedPanes[num6].IndexPane;
						DockPane dockPane3 = dockPanel.Panes[indexPane2];
						if (num6 == 0)
						{
							floatWindow = dockPanel.FloatWindowFactory.CreateFloatWindow(dockPanel, dockPane3, array4[num5].Bounds);
							continue;
						}
						int indexPrevPane2 = array4[num5].NestedPanes[num6].IndexPrevPane;
						DockPane previousPane2 = ((indexPrevPane2 == -1) ? null : dockPanel.Panes[indexPrevPane2]);
						DockAlignment alignment2 = array4[num5].NestedPanes[num6].Alignment;
						double proportion2 = array4[num5].NestedPanes[num6].Proportion;
						dockPane3.DockTo(floatWindow, previousPane2, alignment2, proportion2);
						if (array2[indexPane2].DockState == floatWindow.DockState)
						{
							array2[indexPane2].ZOrderIndex = array4[num5].ZOrderIndex;
						}
					}
				}
				int[] array5 = null;
				if (array.Length != 0)
				{
					array5 = new int[array.Length];
					for (int num7 = 0; num7 < array.Length; num7++)
					{
						array5[num7] = num7;
					}
					int num8 = array.Length;
					for (int num9 = 0; num9 < array.Length - 1; num9++)
					{
						for (int num10 = num9 + 1; num10 < array.Length; num10++)
						{
							DockPane pane = dockPanel.Contents[array5[num9]].DockHandler.Pane;
							int num11 = ((pane != null) ? array2[dockPanel.Panes.IndexOf(pane)].ZOrderIndex : 0);
							DockPane pane2 = dockPanel.Contents[array5[num10]].DockHandler.Pane;
							int num12 = ((pane2 != null) ? array2[dockPanel.Panes.IndexOf(pane2)].ZOrderIndex : 0);
							if (num11 > num12)
							{
								int num13 = array5[num9];
								array5[num9] = array5[num10];
								array5[num10] = num13;
							}
						}
					}
				}
				for (int num14 = 0; num14 < array.Length; num14++)
				{
					IDockContent dockContent3 = dockPanel.Contents[array5[num14]];
					if (dockContent3.DockHandler.Pane != null && dockContent3.DockHandler.Pane.DockState != DockState.Document)
					{
						dockContent3.DockHandler.IsHidden = array[array5[num14]].IsHidden;
					}
				}
				for (int num15 = 0; num15 < array.Length; num15++)
				{
					IDockContent dockContent4 = dockPanel.Contents[array5[num15]];
					if (dockContent4.DockHandler.Pane != null && dockContent4.DockHandler.Pane.DockState == DockState.Document)
					{
						dockContent4.DockHandler.IsHidden = array[array5[num15]].IsHidden;
					}
				}
				for (int num16 = 0; num16 < array2.Length; num16++)
				{
					dockPanel.Panes[num16].ActiveContent = ((array2[num16].IndexActiveContent == -1) ? null : dockPanel.Contents[array2[num16].IndexActiveContent]);
				}
				if (dockPanelStruct.IndexActiveDocumentPane != -1)
				{
					dockPanel.Panes[dockPanelStruct.IndexActiveDocumentPane].Activate();
				}
				if (dockPanelStruct.IndexActivePane != -1)
				{
					dockPanel.Panes[dockPanelStruct.IndexActivePane].Activate();
				}
				for (int num17 = dockPanel.Contents.Count - 1; num17 >= 0; num17--)
				{
					if (dockPanel.Contents[num17] is DummyContent)
					{
						dockPanel.Contents[num17].DockHandler.Form.Close();
					}
				}
				dockPanel.ResumeLayout(performLayout: true, allWindows: true);
			}

			private static bool MoveToNextElement(XmlTextReader xmlIn)
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Invalid comparison between Unknown and I4
				if (!((XmlReader)xmlIn).Read())
				{
					return false;
				}
				while ((int)((XmlReader)xmlIn).get_NodeType() == 15)
				{
					if (!((XmlReader)xmlIn).Read())
					{
						return false;
					}
				}
				return true;
			}

			private static bool IsFormatVersionValid(string formatVersion)
			{
				if (formatVersion == "1.0")
				{
					return true;
				}
				string[] compatibleConfigFileVersions = CompatibleConfigFileVersions;
				foreach (string a in compatibleConfigFileVersions)
				{
					if (a == formatVersion)
					{
						return true;
					}
				}
				return false;
			}
		}

		private interface IFocusManager
		{
			bool IsFocusTrackingSuspended
			{
				get;
			}

			IDockContent ActiveContent
			{
				get;
			}

			DockPane ActivePane
			{
				get;
			}

			IDockContent ActiveDocument
			{
				get;
			}

			DockPane ActiveDocumentPane
			{
				get;
			}

			void SuspendFocusTracking();

			void ResumeFocusTracking();
		}

		private class FocusManagerImpl : Component, IContentFocusManager, IFocusManager
		{
			private class HookEventArgs : EventArgs
			{
				public int HookCode;

				public IntPtr wParam;

				public IntPtr lParam;
			}

			private class LocalWindowsHook : IDisposable
			{
				public delegate void HookEventHandler(object sender, HookEventArgs e);

				private IntPtr m_hHook = IntPtr.Zero;

				private NativeMethods.HookProc m_filterFunc = null;

				private HookType m_hookType;

				public event HookEventHandler HookInvoked;

				protected void OnHookInvoked(HookEventArgs e)
				{
					if (this.HookInvoked != null)
					{
						this.HookInvoked(this, e);
					}
				}

				public LocalWindowsHook(HookType hook)
				{
					m_hookType = hook;
					m_filterFunc = CoreHookProc;
				}

				public IntPtr CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
				{
					if (code < 0)
					{
						return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);
					}
					HookEventArgs hookEventArgs = new HookEventArgs();
					hookEventArgs.HookCode = code;
					hookEventArgs.wParam = wParam;
					hookEventArgs.lParam = lParam;
					OnHookInvoked(hookEventArgs);
					return NativeMethods.CallNextHookEx(m_hHook, code, wParam, lParam);
				}

				public void Install()
				{
					if (m_hHook != IntPtr.Zero)
					{
						Uninstall();
					}
					int currentThreadId = NativeMethods.GetCurrentThreadId();
					m_hHook = NativeMethods.SetWindowsHookEx(m_hookType, m_filterFunc, IntPtr.Zero, currentThreadId);
				}

				public void Uninstall()
				{
					if (m_hHook != IntPtr.Zero)
					{
						NativeMethods.UnhookWindowsHookEx(m_hHook);
						m_hHook = IntPtr.Zero;
					}
				}

				~LocalWindowsHook()
				{
					Dispose(disposing: false);
				}

				public void Dispose()
				{
					Dispose(disposing: true);
					GC.SuppressFinalize(this);
				}

				protected virtual void Dispose(bool disposing)
				{
					Uninstall();
				}
			}

			private LocalWindowsHook m_localWindowsHook;

			private LocalWindowsHook.HookEventHandler m_hookEventHandler;

			private DockPanel m_dockPanel;

			private bool m_disposed = false;

			private IDockContent m_contentActivating = null;

			private List<IDockContent> m_listContent = new List<IDockContent>();

			private IDockContent m_lastActiveContent = null;

			private int m_countSuspendFocusTracking = 0;

			private bool m_inRefreshActiveWindow = false;

			private DockPane m_activePane = null;

			private IDockContent m_activeContent = null;

			private DockPane m_activeDocumentPane = null;

			private IDockContent m_activeDocument = null;

			public DockPanel DockPanel => m_dockPanel;

			private IDockContent ContentActivating
			{
				get
				{
					return m_contentActivating;
				}
				set
				{
					m_contentActivating = value;
				}
			}

			private List<IDockContent> ListContent => m_listContent;

			private IDockContent LastActiveContent
			{
				get
				{
					return m_lastActiveContent;
				}
				set
				{
					m_lastActiveContent = value;
				}
			}

			public bool IsFocusTrackingSuspended => m_countSuspendFocusTracking != 0;

			private bool InRefreshActiveWindow => m_inRefreshActiveWindow;

			public DockPane ActivePane => m_activePane;

			public IDockContent ActiveContent => m_activeContent;

			public DockPane ActiveDocumentPane => m_activeDocumentPane;

			public IDockContent ActiveDocument => m_activeDocument;

			public FocusManagerImpl(DockPanel dockPanel)
				: this()
			{
				m_dockPanel = dockPanel;
				m_localWindowsHook = new LocalWindowsHook(HookType.WH_CALLWNDPROCRET);
				m_hookEventHandler = HookEventHandler;
				m_localWindowsHook.HookInvoked += m_hookEventHandler;
				m_localWindowsHook.Install();
			}

			protected override void Dispose(bool disposing)
			{
				lock (this)
				{
					if (!m_disposed && disposing)
					{
						m_localWindowsHook.Dispose();
						m_disposed = true;
					}
					((Component)this).Dispose(disposing);
				}
			}

			public void Activate(IDockContent content)
			{
				if (IsFocusTrackingSuspended)
				{
					ContentActivating = content;
					return;
				}
				DockContentHandler dockHandler = content.DockHandler;
				if (ContentContains(content, dockHandler.ActiveWindowHandle))
				{
					NativeMethods.SetFocus(dockHandler.ActiveWindowHandle);
				}
				if (!((Control)dockHandler.Form).get_ContainsFocus() && !((Control)dockHandler.Form).SelectNextControl(((ContainerControl)dockHandler.Form).get_ActiveControl(), true, true, true, true))
				{
					NativeMethods.SetFocus(((Control)dockHandler.Form).get_Handle());
				}
			}

			public void AddToList(IDockContent content)
			{
				if (!ListContent.Contains(content) && !IsInActiveList(content))
				{
					ListContent.Add(content);
				}
			}

			public void RemoveFromList(IDockContent content)
			{
				if (IsInActiveList(content))
				{
					RemoveFromActiveList(content);
				}
				if (ListContent.Contains(content))
				{
					ListContent.Remove(content);
				}
			}

			private bool IsInActiveList(IDockContent content)
			{
				return content.DockHandler.NextActive != null || LastActiveContent == content;
			}

			private void AddLastToActiveList(IDockContent content)
			{
				IDockContent lastActiveContent = LastActiveContent;
				if (lastActiveContent != content)
				{
					DockContentHandler dockHandler = content.DockHandler;
					if (IsInActiveList(content))
					{
						RemoveFromActiveList(content);
					}
					dockHandler.PreviousActive = lastActiveContent;
					dockHandler.NextActive = null;
					LastActiveContent = content;
					if (lastActiveContent != null)
					{
						lastActiveContent.DockHandler.NextActive = LastActiveContent;
					}
				}
			}

			private void RemoveFromActiveList(IDockContent content)
			{
				if (LastActiveContent == content)
				{
					LastActiveContent = content.DockHandler.PreviousActive;
				}
				IDockContent previousActive = content.DockHandler.PreviousActive;
				IDockContent nextActive = content.DockHandler.NextActive;
				if (previousActive != null)
				{
					previousActive.DockHandler.NextActive = nextActive;
				}
				if (nextActive != null)
				{
					nextActive.DockHandler.PreviousActive = previousActive;
				}
				content.DockHandler.PreviousActive = null;
				content.DockHandler.NextActive = null;
			}

			public void GiveUpFocus(IDockContent content)
			{
				DockContentHandler dockHandler = content.DockHandler;
				if (!((Control)dockHandler.Form).get_ContainsFocus())
				{
					return;
				}
				if (IsFocusTrackingSuspended)
				{
					DockPanel.DummyControl.Focus();
				}
				if (LastActiveContent == content)
				{
					IDockContent previousActive = dockHandler.PreviousActive;
					if (previousActive != null)
					{
						previousActive.DockHandler.Activate();
					}
					else if (ListContent.Count > 0)
					{
						ListContent[ListContent.Count - 1].DockHandler.Activate();
					}
				}
				else if (LastActiveContent != null)
				{
					LastActiveContent.DockHandler.Activate();
				}
				else if (ListContent.Count > 0)
				{
					ListContent[ListContent.Count - 1].DockHandler.Activate();
				}
			}

			private static bool ContentContains(IDockContent content, IntPtr hWnd)
			{
				Control val = Control.FromChildHandle(hWnd);
				for (Control val2 = val; val2 != null; val2 = val2.get_Parent())
				{
					if (val2 == content.DockHandler.Form)
					{
						return true;
					}
				}
				return false;
			}

			public void SuspendFocusTracking()
			{
				m_countSuspendFocusTracking++;
				m_localWindowsHook.HookInvoked -= m_hookEventHandler;
			}

			public void ResumeFocusTracking()
			{
				if (m_countSuspendFocusTracking > 0)
				{
					m_countSuspendFocusTracking--;
				}
				if (m_countSuspendFocusTracking == 0)
				{
					if (ContentActivating != null)
					{
						Activate(ContentActivating);
						ContentActivating = null;
					}
					m_localWindowsHook.HookInvoked += m_hookEventHandler;
					if (!InRefreshActiveWindow)
					{
						RefreshActiveWindow();
					}
				}
			}

			private void HookEventHandler(object sender, HookEventArgs e)
			{
				switch (Marshal.ReadInt32(e.lParam, IntPtr.Size * 3))
				{
				case 8:
				{
					IntPtr hWnd = Marshal.ReadIntPtr(e.lParam, IntPtr.Size * 2);
					DockPane paneFromHandle = GetPaneFromHandle(hWnd);
					if (paneFromHandle == null)
					{
						RefreshActiveWindow();
					}
					break;
				}
				case 7:
					RefreshActiveWindow();
					break;
				}
			}

			private DockPane GetPaneFromHandle(IntPtr hWnd)
			{
				Control val = Control.FromChildHandle(hWnd);
				IDockContent dockContent = null;
				DockPane dockPane = null;
				while (val != null)
				{
					dockContent = val as IDockContent;
					if (dockContent != null)
					{
						dockContent.DockHandler.ActiveWindowHandle = hWnd;
					}
					if (dockContent != null && dockContent.DockHandler.DockPanel == DockPanel)
					{
						return dockContent.DockHandler.Pane;
					}
					dockPane = val as DockPane;
					if (dockPane != null && dockPane.DockPanel == DockPanel)
					{
						break;
					}
					val = val.get_Parent();
				}
				return dockPane;
			}

			private void RefreshActiveWindow()
			{
				SuspendFocusTracking();
				m_inRefreshActiveWindow = true;
				DockPane activePane = ActivePane;
				IDockContent activeContent = ActiveContent;
				IDockContent activeDocument = ActiveDocument;
				SetActivePane();
				SetActiveContent();
				SetActiveDocumentPane();
				SetActiveDocument();
				DockPanel.AutoHideWindow.RefreshActivePane();
				ResumeFocusTracking();
				m_inRefreshActiveWindow = false;
				if (activeContent != ActiveContent)
				{
					DockPanel.OnActiveContentChanged(EventArgs.Empty);
				}
				if (activeDocument != ActiveDocument)
				{
					DockPanel.OnActiveDocumentChanged(EventArgs.Empty);
				}
				if (activePane != ActivePane)
				{
					DockPanel.OnActivePaneChanged(EventArgs.Empty);
				}
			}

			private void SetActivePane()
			{
				DockPane paneFromHandle = GetPaneFromHandle(NativeMethods.GetFocus());
				if (m_activePane != paneFromHandle)
				{
					if (m_activePane != null)
					{
						m_activePane.SetIsActivated(value: false);
					}
					m_activePane = paneFromHandle;
					if (m_activePane != null)
					{
						m_activePane.SetIsActivated(value: true);
					}
				}
			}

			internal void SetActiveContent()
			{
				IDockContent dockContent = ((ActivePane == null) ? null : ActivePane.ActiveContent);
				if (m_activeContent == dockContent)
				{
					return;
				}
				if (m_activeContent != null)
				{
					m_activeContent.DockHandler.IsActivated = false;
				}
				m_activeContent = dockContent;
				if (m_activeContent != null)
				{
					m_activeContent.DockHandler.IsActivated = true;
					if (!DockHelper.IsDockStateAutoHide(m_activeContent.DockHandler.DockState))
					{
						AddLastToActiveList(m_activeContent);
					}
				}
			}

			private void SetActiveDocumentPane()
			{
				DockPane dockPane = null;
				if (ActivePane != null && ActivePane.DockState == DockState.Document)
				{
					dockPane = ActivePane;
				}
				if (dockPane == null)
				{
					dockPane = ((DockPanel.DockWindows == null) ? null : ((ActiveDocumentPane == null) ? DockPanel.DockWindows[DockState.Document].DefaultPane : ((ActiveDocumentPane.DockPanel == DockPanel && ActiveDocumentPane.DockState == DockState.Document) ? ActiveDocumentPane : DockPanel.DockWindows[DockState.Document].DefaultPane)));
				}
				if (m_activeDocumentPane != dockPane)
				{
					if (m_activeDocumentPane != null)
					{
						m_activeDocumentPane.SetIsActiveDocumentPane(value: false);
					}
					m_activeDocumentPane = dockPane;
					if (m_activeDocumentPane != null)
					{
						m_activeDocumentPane.SetIsActiveDocumentPane(value: true);
					}
				}
			}

			private void SetActiveDocument()
			{
				IDockContent dockContent = ((ActiveDocumentPane == null) ? null : ActiveDocumentPane.ActiveContent);
				if (m_activeDocument != dockContent)
				{
					m_activeDocument = dockContent;
				}
			}
		}

		private SplitterDragHandler m_splitterDragHandler = null;

		private MdiClientController m_mdiClientController = null;

		private DockDragHandler m_dockDragHandler = null;

		private FocusManagerImpl m_focusManager;

		private DockPanelExtender m_extender;

		private DockPaneCollection m_panes;

		private FloatWindowCollection m_floatWindows;

		private AutoHideWindowControl m_autoHideWindow;

		private DockWindowCollection m_dockWindows;

		private DockContent m_dummyContent;

		private Control m_dummyControl;

		private AutoHideStripBase m_autoHideStripControl = null;

		private bool m_disposed = false;

		private bool m_allowEndUserDocking = true;

		private bool m_allowEndUserNestedDocking = true;

		private DockContentCollection m_contents = new DockContentCollection();

		private bool m_rightToLeftLayout = false;

		private bool m_showDocumentIcon = false;

		private double m_dockBottomPortion = 0.25;

		private double m_dockLeftPortion = 0.25;

		private double m_dockRightPortion = 0.25;

		private double m_dockTopPortion = 0.25;

		private Size m_defaultFloatWindowSize = new Size(300, 300);

		private DocumentStyle m_documentStyle = DocumentStyle.DockingMdi;

		private PaintEventHandler m_dummyControlPaintEventHandler = null;

		private Rectangle[] m_clipRects = null;

		private static readonly object ContentAddedEvent = new object();

		private static readonly object ContentRemovedEvent = new object();

		private static readonly object ActiveDocumentChangedEvent = new object();

		private static readonly object ActiveContentChangedEvent = new object();

		private static readonly object ActivePaneChangedEvent = new object();

		private bool MdiClientExists => GetMdiClientController().MdiClient != null;

		private AutoHideWindowControl AutoHideWindow => m_autoHideWindow;

		internal Control AutoHideControl => (Control)(object)m_autoHideWindow;

		internal Rectangle AutoHideWindowRectangle
		{
			get
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_028c: Unknown result type (might be due to invalid IL or missing references)
				//IL_028d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0291: Unknown result type (might be due to invalid IL or missing references)
				DockState dockState = AutoHideWindow.DockState;
				Rectangle dockArea = DockArea;
				if (ActiveAutoHideContent == null)
				{
					return Rectangle.Empty;
				}
				if (((Control)this).get_Parent() == null)
				{
					return Rectangle.Empty;
				}
				Rectangle empty = Rectangle.Empty;
				double num = ActiveAutoHideContent.DockHandler.AutoHidePortion;
				switch (dockState)
				{
				case DockState.DockLeftAutoHide:
					if (num < 1.0)
					{
						num = (double)((Rectangle)(ref dockArea)).get_Width() * num;
					}
					if (num > (double)(((Rectangle)(ref dockArea)).get_Width() - 24))
					{
						num = ((Rectangle)(ref dockArea)).get_Width() - 24;
					}
					((Rectangle)(ref empty)).set_X(((Rectangle)(ref dockArea)).get_X());
					((Rectangle)(ref empty)).set_Y(((Rectangle)(ref dockArea)).get_Y());
					((Rectangle)(ref empty)).set_Width((int)num);
					((Rectangle)(ref empty)).set_Height(((Rectangle)(ref dockArea)).get_Height());
					break;
				case DockState.DockRightAutoHide:
					if (num < 1.0)
					{
						num = (double)((Rectangle)(ref dockArea)).get_Width() * num;
					}
					if (num > (double)(((Rectangle)(ref dockArea)).get_Width() - 24))
					{
						num = ((Rectangle)(ref dockArea)).get_Width() - 24;
					}
					((Rectangle)(ref empty)).set_X(((Rectangle)(ref dockArea)).get_X() + ((Rectangle)(ref dockArea)).get_Width() - (int)num);
					((Rectangle)(ref empty)).set_Y(((Rectangle)(ref dockArea)).get_Y());
					((Rectangle)(ref empty)).set_Width((int)num);
					((Rectangle)(ref empty)).set_Height(((Rectangle)(ref dockArea)).get_Height());
					break;
				case DockState.DockTopAutoHide:
					if (num < 1.0)
					{
						num = (double)((Rectangle)(ref dockArea)).get_Height() * num;
					}
					if (num > (double)(((Rectangle)(ref dockArea)).get_Height() - 24))
					{
						num = ((Rectangle)(ref dockArea)).get_Height() - 24;
					}
					((Rectangle)(ref empty)).set_X(((Rectangle)(ref dockArea)).get_X());
					((Rectangle)(ref empty)).set_Y(((Rectangle)(ref dockArea)).get_Y());
					((Rectangle)(ref empty)).set_Width(((Rectangle)(ref dockArea)).get_Width());
					((Rectangle)(ref empty)).set_Height((int)num);
					break;
				case DockState.DockBottomAutoHide:
					if (num < 1.0)
					{
						num = (double)((Rectangle)(ref dockArea)).get_Height() * num;
					}
					if (num > (double)(((Rectangle)(ref dockArea)).get_Height() - 24))
					{
						num = ((Rectangle)(ref dockArea)).get_Height() - 24;
					}
					((Rectangle)(ref empty)).set_X(((Rectangle)(ref dockArea)).get_X());
					((Rectangle)(ref empty)).set_Y(((Rectangle)(ref dockArea)).get_Y() + ((Rectangle)(ref dockArea)).get_Height() - (int)num);
					((Rectangle)(ref empty)).set_Width(((Rectangle)(ref dockArea)).get_Width());
					((Rectangle)(ref empty)).set_Height((int)num);
					break;
				}
				return empty;
			}
		}

		internal AutoHideStripBase AutoHideStripControl
		{
			get
			{
				if (m_autoHideStripControl == null)
				{
					m_autoHideStripControl = AutoHideStripFactory.CreateAutoHideStrip(this);
					((Control)this).get_Controls().Add((Control)(object)m_autoHideStripControl);
				}
				return m_autoHideStripControl;
			}
		}

		[Browsable(false)]
		public IDockContent ActiveAutoHideContent
		{
			get
			{
				return AutoHideWindow.ActiveContent;
			}
			set
			{
				AutoHideWindow.ActiveContent = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_AllowEndUserDocking_Description")]
		[DefaultValue(true)]
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

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_AllowEndUserNestedDocking_Description")]
		[DefaultValue(true)]
		public bool AllowEndUserNestedDocking
		{
			get
			{
				return m_allowEndUserNestedDocking;
			}
			set
			{
				m_allowEndUserNestedDocking = value;
			}
		}

		[Browsable(false)]
		public DockContentCollection Contents => m_contents;

		internal DockContent DummyContent => m_dummyContent;

		[DefaultValue(false)]
		[LocalizedCategory("Appearance")]
		[LocalizedDescription("DockPanel_RightToLeftLayout_Description")]
		public bool RightToLeftLayout
		{
			get
			{
				return m_rightToLeftLayout;
			}
			set
			{
				if (m_rightToLeftLayout == value)
				{
					return;
				}
				m_rightToLeftLayout = value;
				foreach (FloatWindow floatWindow in FloatWindows)
				{
					((Form)floatWindow).set_RightToLeftLayout(value);
				}
			}
		}

		[DefaultValue(false)]
		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_ShowDocumentIcon_Description")]
		public bool ShowDocumentIcon
		{
			get
			{
				return m_showDocumentIcon;
			}
			set
			{
				if (m_showDocumentIcon != value)
				{
					m_showDocumentIcon = value;
					((Control)this).Refresh();
				}
			}
		}

		[Browsable(false)]
		public DockPanelExtender Extender => m_extender;

		public DockPanelExtender.IDockPaneFactory DockPaneFactory => Extender.DockPaneFactory;

		public DockPanelExtender.IFloatWindowFactory FloatWindowFactory => Extender.FloatWindowFactory;

		internal DockPanelExtender.IDockPaneCaptionFactory DockPaneCaptionFactory => Extender.DockPaneCaptionFactory;

		internal DockPanelExtender.IDockPaneStripFactory DockPaneStripFactory => Extender.DockPaneStripFactory;

		internal DockPanelExtender.IAutoHideStripFactory AutoHideStripFactory => Extender.AutoHideStripFactory;

		[Browsable(false)]
		public DockPaneCollection Panes => m_panes;

		internal Rectangle DockArea
		{
			get
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_0068: Unknown result type (might be due to invalid IL or missing references)
				//IL_006b: Unknown result type (might be due to invalid IL or missing references)
				int left = ((ScrollableControl)this).get_DockPadding().get_Left();
				int top = ((ScrollableControl)this).get_DockPadding().get_Top();
				Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
				int num = ((Rectangle)(ref clientRectangle)).get_Width() - ((ScrollableControl)this).get_DockPadding().get_Left() - ((ScrollableControl)this).get_DockPadding().get_Right();
				clientRectangle = ((Control)this).get_ClientRectangle();
				return new Rectangle(left, top, num, ((Rectangle)(ref clientRectangle)).get_Height() - ((ScrollableControl)this).get_DockPadding().get_Top() - ((ScrollableControl)this).get_DockPadding().get_Bottom());
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_DockBottomPortion_Description")]
		[DefaultValue(0.25)]
		public double DockBottomPortion
		{
			get
			{
				return m_dockBottomPortion;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != m_dockBottomPortion)
				{
					m_dockBottomPortion = value;
					if (m_dockBottomPortion < 1.0 && m_dockTopPortion < 1.0 && m_dockTopPortion + m_dockBottomPortion > 1.0)
					{
						m_dockTopPortion = 1.0 - m_dockBottomPortion;
					}
					((Control)this).PerformLayout();
				}
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_DockLeftPortion_Description")]
		[DefaultValue(0.25)]
		public double DockLeftPortion
		{
			get
			{
				return m_dockLeftPortion;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != m_dockLeftPortion)
				{
					m_dockLeftPortion = value;
					if (m_dockLeftPortion < 1.0 && m_dockRightPortion < 1.0 && m_dockLeftPortion + m_dockRightPortion > 1.0)
					{
						m_dockRightPortion = 1.0 - m_dockLeftPortion;
					}
					((Control)this).PerformLayout();
				}
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_DockRightPortion_Description")]
		[DefaultValue(0.25)]
		public double DockRightPortion
		{
			get
			{
				return m_dockRightPortion;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != m_dockRightPortion)
				{
					m_dockRightPortion = value;
					if (m_dockLeftPortion < 1.0 && m_dockRightPortion < 1.0 && m_dockLeftPortion + m_dockRightPortion > 1.0)
					{
						m_dockLeftPortion = 1.0 - m_dockRightPortion;
					}
					((Control)this).PerformLayout();
				}
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_DockTopPortion_Description")]
		[DefaultValue(0.25)]
		public double DockTopPortion
		{
			get
			{
				return m_dockTopPortion;
			}
			set
			{
				if (value <= 0.0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value != m_dockTopPortion)
				{
					m_dockTopPortion = value;
					if (m_dockTopPortion < 1.0 && m_dockBottomPortion < 1.0 && m_dockTopPortion + m_dockBottomPortion > 1.0)
					{
						m_dockBottomPortion = 1.0 - m_dockTopPortion;
					}
					((Control)this).PerformLayout();
				}
			}
		}

		[Browsable(false)]
		public DockWindowCollection DockWindows => m_dockWindows;

		public int DocumentsCount
		{
			get
			{
				int num = 0;
				foreach (DockContent document in Documents)
				{
					num++;
				}
				return num;
			}
		}

		public IEnumerable<IDockContent> Documents
		{
			get
			{
				foreach (IDockContent content in Contents)
				{
					if (content.DockHandler.DockState == DockState.Document)
					{
						yield return content;
					}
				}
			}
		}

		private Rectangle DocumentRectangle
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0055: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
				//IL_0106: Unknown result type (might be due to invalid IL or missing references)
				//IL_010b: Unknown result type (might be due to invalid IL or missing references)
				//IL_014d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0152: Unknown result type (might be due to invalid IL or missing references)
				//IL_016a: Unknown result type (might be due to invalid IL or missing references)
				//IL_016b: Unknown result type (might be due to invalid IL or missing references)
				//IL_016f: Unknown result type (might be due to invalid IL or missing references)
				Rectangle dockArea = DockArea;
				Rectangle dockArea2;
				if (DockWindows[DockState.DockLeft].VisibleNestedPanes.Count != 0)
				{
					int x = ((Rectangle)(ref dockArea)).get_X();
					dockArea2 = DockArea;
					((Rectangle)(ref dockArea)).set_X(x + (int)((double)((Rectangle)(ref dockArea2)).get_Width() * DockLeftPortion));
					int width = ((Rectangle)(ref dockArea)).get_Width();
					dockArea2 = DockArea;
					((Rectangle)(ref dockArea)).set_Width(width - (int)((double)((Rectangle)(ref dockArea2)).get_Width() * DockLeftPortion));
				}
				if (DockWindows[DockState.DockRight].VisibleNestedPanes.Count != 0)
				{
					int width2 = ((Rectangle)(ref dockArea)).get_Width();
					dockArea2 = DockArea;
					((Rectangle)(ref dockArea)).set_Width(width2 - (int)((double)((Rectangle)(ref dockArea2)).get_Width() * DockRightPortion));
				}
				if (DockWindows[DockState.DockTop].VisibleNestedPanes.Count != 0)
				{
					int y = ((Rectangle)(ref dockArea)).get_Y();
					dockArea2 = DockArea;
					((Rectangle)(ref dockArea)).set_Y(y + (int)((double)((Rectangle)(ref dockArea2)).get_Height() * DockTopPortion));
					int height = ((Rectangle)(ref dockArea)).get_Height();
					dockArea2 = DockArea;
					((Rectangle)(ref dockArea)).set_Height(height - (int)((double)((Rectangle)(ref dockArea2)).get_Height() * DockTopPortion));
				}
				if (DockWindows[DockState.DockBottom].VisibleNestedPanes.Count != 0)
				{
					int height2 = ((Rectangle)(ref dockArea)).get_Height();
					dockArea2 = DockArea;
					((Rectangle)(ref dockArea)).set_Height(height2 - (int)((double)((Rectangle)(ref dockArea2)).get_Height() * DockBottomPortion));
				}
				return dockArea;
			}
		}

		private Control DummyControl => m_dummyControl;

		[Browsable(false)]
		public FloatWindowCollection FloatWindows => m_floatWindows;

		[Category("Layout")]
		[LocalizedDescription("DockPanel_DefaultFloatWindowSize_Description")]
		public Size DefaultFloatWindowSize
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return m_defaultFloatWindowSize;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				m_defaultFloatWindowSize = value;
			}
		}

		[LocalizedCategory("Category_Docking")]
		[LocalizedDescription("DockPanel_DocumentStyle_Description")]
		[DefaultValue(DocumentStyle.DockingMdi)]
		public DocumentStyle DocumentStyle
		{
			get
			{
				return m_documentStyle;
			}
			set
			{
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				if (value == m_documentStyle)
				{
					return;
				}
				if (!Enum.IsDefined(typeof(DocumentStyle), value))
				{
					throw new InvalidEnumArgumentException();
				}
				if (value == DocumentStyle.SystemMdi && DockWindows[DockState.Document].VisibleNestedPanes.Count > 0)
				{
					throw new InvalidEnumArgumentException();
				}
				m_documentStyle = value;
				SuspendLayout(allWindows: true);
				SetMdiClient();
				InvalidateWindowRegion();
				foreach (IDockContent content in Contents)
				{
					if (content.DockHandler.DockState == DockState.Document)
					{
						content.DockHandler.SetPaneAndVisible(content.DockHandler.Pane);
					}
				}
				PerformMdiClientLayout();
				ResumeLayout(performLayout: true, allWindows: true);
			}
		}

		internal Form ParentForm
		{
			get
			{
				if (!IsParentFormValid())
				{
					throw new InvalidOperationException(Strings.DockPanel_ParentForm_Invalid);
				}
				return GetMdiClientController().ParentForm;
			}
		}

		private Rectangle SystemMdiClientBounds
		{
			get
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				if (!IsParentFormValid() || !((Control)this).get_Visible())
				{
					return Rectangle.Empty;
				}
				return ((Control)ParentForm).RectangleToClient(((Control)this).RectangleToScreen(DocumentWindowBounds));
			}
		}

		internal Rectangle DocumentWindowBounds
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0126: Unknown result type (might be due to invalid IL or missing references)
				//IL_0127: Unknown result type (might be due to invalid IL or missing references)
				//IL_012b: Unknown result type (might be due to invalid IL or missing references)
				Rectangle displayRectangle = ((Control)this).get_DisplayRectangle();
				if (((Control)DockWindows[DockState.DockLeft]).get_Visible())
				{
					((Rectangle)(ref displayRectangle)).set_X(((Rectangle)(ref displayRectangle)).get_X() + ((Control)DockWindows[DockState.DockLeft]).get_Width());
					((Rectangle)(ref displayRectangle)).set_Width(((Rectangle)(ref displayRectangle)).get_Width() - ((Control)DockWindows[DockState.DockLeft]).get_Width());
				}
				if (((Control)DockWindows[DockState.DockRight]).get_Visible())
				{
					((Rectangle)(ref displayRectangle)).set_Width(((Rectangle)(ref displayRectangle)).get_Width() - ((Control)DockWindows[DockState.DockRight]).get_Width());
				}
				if (((Control)DockWindows[DockState.DockTop]).get_Visible())
				{
					((Rectangle)(ref displayRectangle)).set_Y(((Rectangle)(ref displayRectangle)).get_Y() + ((Control)DockWindows[DockState.DockTop]).get_Height());
					((Rectangle)(ref displayRectangle)).set_Height(((Rectangle)(ref displayRectangle)).get_Height() - ((Control)DockWindows[DockState.DockTop]).get_Height());
				}
				if (((Control)DockWindows[DockState.DockBottom]).get_Visible())
				{
					((Rectangle)(ref displayRectangle)).set_Height(((Rectangle)(ref displayRectangle)).get_Height() - ((Control)DockWindows[DockState.DockBottom]).get_Height());
				}
				return displayRectangle;
			}
		}

		private IFocusManager FocusManager => m_focusManager;

		internal IContentFocusManager ContentFocusManager => m_focusManager;

		[Browsable(false)]
		public IDockContent ActiveContent => FocusManager.ActiveContent;

		[Browsable(false)]
		public DockPane ActivePane => FocusManager.ActivePane;

		[Browsable(false)]
		public IDockContent ActiveDocument => FocusManager.ActiveDocument;

		[Browsable(false)]
		public DockPane ActiveDocumentPane => FocusManager.ActiveDocumentPane;

		[LocalizedCategory("Category_DockingNotification")]
		[LocalizedDescription("DockPanel_ContentAdded_Description")]
		public event EventHandler<DockContentEventArgs> ContentAdded
		{
			add
			{
				((Component)this).get_Events().AddHandler(ContentAddedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(ContentAddedEvent, (Delegate)value);
			}
		}

		[LocalizedCategory("Category_DockingNotification")]
		[LocalizedDescription("DockPanel_ContentRemoved_Description")]
		public event EventHandler<DockContentEventArgs> ContentRemoved
		{
			add
			{
				((Component)this).get_Events().AddHandler(ContentRemovedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(ContentRemovedEvent, (Delegate)value);
			}
		}

		[LocalizedCategory("Category_PropertyChanged")]
		[LocalizedDescription("DockPanel_ActiveDocumentChanged_Description")]
		public event EventHandler ActiveDocumentChanged
		{
			add
			{
				((Component)this).get_Events().AddHandler(ActiveDocumentChangedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(ActiveDocumentChangedEvent, (Delegate)value);
			}
		}

		[LocalizedCategory("Category_PropertyChanged")]
		[LocalizedDescription("DockPanel_ActiveContentChanged_Description")]
		public event EventHandler ActiveContentChanged
		{
			add
			{
				((Component)this).get_Events().AddHandler(ActiveContentChangedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(ActiveContentChangedEvent, (Delegate)value);
			}
		}

		[LocalizedCategory("Category_PropertyChanged")]
		[LocalizedDescription("DockPanel_ActivePaneChanged_Description")]
		public event EventHandler ActivePaneChanged
		{
			add
			{
				((Component)this).get_Events().AddHandler(ActivePaneChangedEvent, (Delegate)value);
			}
			remove
			{
				((Component)this).get_Events().RemoveHandler(ActivePaneChangedEvent, (Delegate)value);
			}
		}

		private SplitterDragHandler GetSplitterDragHandler()
		{
			if (m_splitterDragHandler == null)
			{
				m_splitterDragHandler = new SplitterDragHandler(this);
			}
			return m_splitterDragHandler;
		}

		internal void BeginDrag(ISplitterDragSource dragSource, Rectangle rectSplitter)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			GetSplitterDragHandler().BeginDrag(dragSource, rectSplitter);
		}

		private MdiClientController GetMdiClientController()
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			if (m_mdiClientController == null)
			{
				m_mdiClientController = new MdiClientController();
				m_mdiClientController.HandleAssigned += MdiClientHandleAssigned;
				m_mdiClientController.MdiChildActivate += ParentFormMdiChildActivate;
				m_mdiClientController.Layout += new LayoutEventHandler(MdiClient_Layout);
			}
			return m_mdiClientController;
		}

		private void ParentFormMdiChildActivate(object sender, EventArgs e)
		{
			if (GetMdiClientController().ParentForm != null)
			{
				IDockContent dockContent = GetMdiClientController().ParentForm.get_ActiveMdiChild() as IDockContent;
				if (dockContent != null && dockContent.DockHandler.DockPanel == this && dockContent.DockHandler.Pane != null)
				{
					dockContent.DockHandler.Pane.ActiveContent = dockContent;
				}
			}
		}

		private void SetMdiClientBounds(Rectangle bounds)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((Control)GetMdiClientController().MdiClient).set_Bounds(bounds);
		}

		private void SuspendMdiClientLayout()
		{
			if (GetMdiClientController().MdiClient != null)
			{
				((Control)GetMdiClientController().MdiClient).PerformLayout();
			}
		}

		private void ResumeMdiClientLayout(bool perform)
		{
			if (GetMdiClientController().MdiClient != null)
			{
				((Control)GetMdiClientController().MdiClient).ResumeLayout(perform);
			}
		}

		private void PerformMdiClientLayout()
		{
			if (GetMdiClientController().MdiClient != null)
			{
				((Control)GetMdiClientController().MdiClient).PerformLayout();
			}
		}

		private void SetMdiClient()
		{
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			MdiClientController mdiClientController = GetMdiClientController();
			if (DocumentStyle == DocumentStyle.DockingMdi)
			{
				mdiClientController.AutoScroll = false;
				mdiClientController.BorderStyle = (BorderStyle)0;
				if (MdiClientExists)
				{
					((Control)mdiClientController.MdiClient).set_Dock((DockStyle)5);
				}
			}
			else if (DocumentStyle == DocumentStyle.DockingSdi || DocumentStyle == DocumentStyle.DockingWindow)
			{
				mdiClientController.AutoScroll = true;
				mdiClientController.BorderStyle = (BorderStyle)2;
				if (MdiClientExists)
				{
					((Control)mdiClientController.MdiClient).set_Dock((DockStyle)5);
				}
			}
			else if (DocumentStyle == DocumentStyle.SystemMdi)
			{
				mdiClientController.AutoScroll = true;
				mdiClientController.BorderStyle = (BorderStyle)2;
				if (mdiClientController.MdiClient != null)
				{
					((Control)mdiClientController.MdiClient).set_Dock((DockStyle)0);
					((Control)mdiClientController.MdiClient).set_Bounds(SystemMdiClientBounds);
				}
			}
		}

		internal Rectangle RectangleToMdiClient(Rectangle rect)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (MdiClientExists)
			{
				return ((Control)GetMdiClientController().MdiClient).RectangleToClient(rect);
			}
			return Rectangle.Empty;
		}

		internal void RefreshActiveAutoHideContent()
		{
			AutoHideWindow.RefreshActiveContent();
		}

		internal Rectangle GetAutoHideWindowBounds(Rectangle rectAutoHideWindow)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			if (((Control)this).get_Parent() == null)
			{
				return Rectangle.Empty;
			}
			return ((Control)this).get_Parent().RectangleToClient(((Control)this).RectangleToScreen(rectAutoHideWindow));
		}

		internal void RefreshAutoHideStrip()
		{
			AutoHideStripControl.RefreshChanges();
		}

		private DockDragHandler GetDockDragHandler()
		{
			if (m_dockDragHandler == null)
			{
				m_dockDragHandler = new DockDragHandler(this);
			}
			return m_dockDragHandler;
		}

		internal void BeginDrag(IDockDragSource dragSource)
		{
			GetDockDragHandler().BeginDrag(dragSource);
		}

		public DockPanel()
			: this()
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			m_focusManager = new FocusManagerImpl(this);
			m_extender = new DockPanelExtender(this);
			m_panes = new DockPaneCollection();
			m_floatWindows = new FloatWindowCollection();
			((Control)this).SetStyle((ControlStyles)8210, true);
			((Control)this).SuspendLayout();
			((Control)this).set_Font(SystemInformation.get_MenuFont());
			m_autoHideWindow = new AutoHideWindowControl(this);
			((Control)m_autoHideWindow).set_Visible(false);
			m_dummyControl = (Control)(object)new DummyControl();
			m_dummyControl.set_Bounds(new Rectangle(0, 0, 1, 1));
			((Control)this).get_Controls().Add(m_dummyControl);
			m_dockWindows = new DockWindowCollection(this);
			((Control)this).get_Controls().AddRange((Control[])(object)new Control[5]
			{
				(Control)DockWindows[DockState.Document],
				(Control)DockWindows[DockState.DockLeft],
				(Control)DockWindows[DockState.DockRight],
				(Control)DockWindows[DockState.DockTop],
				(Control)DockWindows[DockState.DockBottom]
			});
			m_dummyContent = new DockContent();
			((Control)this).ResumeLayout();
		}

		private void MdiClientHandleAssigned(object sender, EventArgs e)
		{
			SetMdiClient();
			((Control)this).PerformLayout();
		}

		private void MdiClient_Layout(object sender, LayoutEventArgs e)
		{
			if (DocumentStyle != 0)
			{
				return;
			}
			foreach (DockPane pane in Panes)
			{
				if (pane.DockState == DockState.Document)
				{
					pane.SetContentBounds();
				}
			}
			InvalidateWindowRegion();
		}

		protected override void Dispose(bool disposing)
		{
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Expected O, but got Unknown
			lock (this)
			{
				if (!m_disposed && disposing)
				{
					((Component)m_focusManager).Dispose();
					if (m_mdiClientController != null)
					{
						m_mdiClientController.HandleAssigned -= MdiClientHandleAssigned;
						m_mdiClientController.MdiChildActivate -= ParentFormMdiChildActivate;
						m_mdiClientController.Layout -= new LayoutEventHandler(MdiClient_Layout);
						m_mdiClientController.Dispose();
					}
					FloatWindows.Dispose();
					Panes.Dispose();
					((Component)DummyContent).Dispose();
					m_disposed = true;
				}
				((Control)this).Dispose(disposing);
			}
		}

		protected override void OnRightToLeftChanged(EventArgs e)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			((ScrollableControl)this).OnRightToLeftChanged(e);
			foreach (FloatWindow floatWindow in FloatWindows)
			{
				if (((Control)floatWindow).get_RightToLeft() != ((Control)this).get_RightToLeft())
				{
					((Control)floatWindow).set_RightToLeft(((Control)this).get_RightToLeft());
				}
			}
		}

		public void UpdateDockWindowZOrder(DockStyle dockStyle, bool fullPanelEdge)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Invalid comparison between Unknown and I4
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Invalid comparison between Unknown and I4
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Invalid comparison between Unknown and I4
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Invalid comparison between Unknown and I4
			if ((int)dockStyle == 3)
			{
				if (fullPanelEdge)
				{
					((Control)DockWindows[DockState.DockLeft]).SendToBack();
				}
				else
				{
					((Control)DockWindows[DockState.DockLeft]).BringToFront();
				}
			}
			else if ((int)dockStyle == 4)
			{
				if (fullPanelEdge)
				{
					((Control)DockWindows[DockState.DockRight]).SendToBack();
				}
				else
				{
					((Control)DockWindows[DockState.DockRight]).BringToFront();
				}
			}
			else if ((int)dockStyle == 1)
			{
				if (fullPanelEdge)
				{
					((Control)DockWindows[DockState.DockTop]).SendToBack();
				}
				else
				{
					((Control)DockWindows[DockState.DockTop]).BringToFront();
				}
			}
			else if ((int)dockStyle == 2)
			{
				if (fullPanelEdge)
				{
					((Control)DockWindows[DockState.DockBottom]).SendToBack();
				}
				else
				{
					((Control)DockWindows[DockState.DockBottom]).BringToFront();
				}
			}
		}

		public IDockContent[] GetDocuments()
		{
			int documentsCount = DocumentsCount;
			IDockContent[] array = new IDockContent[documentsCount];
			int num = 0;
			foreach (IDockContent document in Documents)
			{
				IDockContent dockContent = (array[num] = document);
				num++;
			}
			return array;
		}

		private bool ShouldSerializeDefaultFloatWindowSize()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return DefaultFloatWindowSize != new Size(300, 300);
		}

		private int GetDockWindowSize(DockState dockState)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			Rectangle clientRectangle;
			if (dockState == DockState.DockLeft || dockState == DockState.DockRight)
			{
				clientRectangle = ((Control)this).get_ClientRectangle();
				int num = ((Rectangle)(ref clientRectangle)).get_Width() - ((ScrollableControl)this).get_DockPadding().get_Left() - ((ScrollableControl)this).get_DockPadding().get_Right();
				int num2 = ((m_dockLeftPortion >= 1.0) ? ((int)m_dockLeftPortion) : ((int)((double)num * m_dockLeftPortion)));
				int num3 = ((m_dockRightPortion >= 1.0) ? ((int)m_dockRightPortion) : ((int)((double)num * m_dockRightPortion)));
				if (num2 < 24)
				{
					num2 = 24;
				}
				if (num3 < 24)
				{
					num3 = 24;
				}
				if (num2 + num3 > num - 24)
				{
					int num4 = num2 + num3 - (num - 24);
					num2 -= num4 / 2;
					num3 -= num4 / 2;
				}
				return (dockState == DockState.DockLeft) ? num2 : num3;
			}
			if (dockState == DockState.DockTop || dockState == DockState.DockBottom)
			{
				clientRectangle = ((Control)this).get_ClientRectangle();
				int num5 = ((Rectangle)(ref clientRectangle)).get_Height() - ((ScrollableControl)this).get_DockPadding().get_Top() - ((ScrollableControl)this).get_DockPadding().get_Bottom();
				int num6 = ((m_dockTopPortion >= 1.0) ? ((int)m_dockTopPortion) : ((int)((double)num5 * m_dockTopPortion)));
				int num7 = ((m_dockBottomPortion >= 1.0) ? ((int)m_dockBottomPortion) : ((int)((double)num5 * m_dockBottomPortion)));
				if (num6 < 24)
				{
					num6 = 24;
				}
				if (num7 < 24)
				{
					num7 = 24;
				}
				if (num6 + num7 > num5 - 24)
				{
					int num8 = num6 + num7 - (num5 - 24);
					num6 -= num8 / 2;
					num7 -= num8 / 2;
				}
				return (dockState == DockState.DockTop) ? num6 : num7;
			}
			return 0;
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			SuspendLayout(allWindows: true);
			((Control)AutoHideStripControl).set_Bounds(((Control)this).get_ClientRectangle());
			CalculateDockPadding();
			((Control)DockWindows[DockState.DockLeft]).set_Width(GetDockWindowSize(DockState.DockLeft));
			((Control)DockWindows[DockState.DockRight]).set_Width(GetDockWindowSize(DockState.DockRight));
			((Control)DockWindows[DockState.DockTop]).set_Height(GetDockWindowSize(DockState.DockTop));
			((Control)DockWindows[DockState.DockBottom]).set_Height(GetDockWindowSize(DockState.DockBottom));
			((Control)AutoHideWindow).set_Bounds(GetAutoHideWindowBounds(AutoHideWindowRectangle));
			((Control)DockWindows[DockState.Document]).BringToFront();
			((Control)AutoHideWindow).BringToFront();
			((ScrollableControl)this).OnLayout(levent);
			if (DocumentStyle == DocumentStyle.SystemMdi && MdiClientExists)
			{
				SetMdiClientBounds(SystemMdiClientBounds);
				InvalidateWindowRegion();
			}
			else if (DocumentStyle == DocumentStyle.DockingMdi)
			{
				InvalidateWindowRegion();
			}
			if (((Control)this).get_Parent() != null)
			{
				((Control)this).get_Parent().ResumeLayout();
			}
			ResumeLayout(performLayout: true, allWindows: true);
		}

		internal Rectangle GetTabStripRectangle(DockState dockState)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			return AutoHideStripControl.GetTabStripRectangle(dockState);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnPaint(e);
			Graphics graphics = e.get_Graphics();
			graphics.FillRectangle(SystemBrushes.get_AppWorkspace(), ((Control)this).get_ClientRectangle());
		}

		internal void AddContent(IDockContent content)
		{
			if (content == null)
			{
				throw new ArgumentNullException();
			}
			if (!Contents.Contains(content))
			{
				Contents.Add(content);
				OnContentAdded(new DockContentEventArgs(content));
			}
		}

		internal void AddPane(DockPane pane)
		{
			if (!Panes.Contains(pane))
			{
				Panes.Add(pane);
			}
		}

		internal void AddFloatWindow(FloatWindow floatWindow)
		{
			if (!FloatWindows.Contains(floatWindow))
			{
				FloatWindows.Add(floatWindow);
			}
		}

		private void CalculateDockPadding()
		{
			((ScrollableControl)this).get_DockPadding().set_All(0);
			int num = AutoHideStripControl.MeasureHeight();
			if (AutoHideStripControl.GetNumberOfPanes(DockState.DockLeftAutoHide) > 0)
			{
				((ScrollableControl)this).get_DockPadding().set_Left(num);
			}
			if (AutoHideStripControl.GetNumberOfPanes(DockState.DockRightAutoHide) > 0)
			{
				((ScrollableControl)this).get_DockPadding().set_Right(num);
			}
			if (AutoHideStripControl.GetNumberOfPanes(DockState.DockTopAutoHide) > 0)
			{
				((ScrollableControl)this).get_DockPadding().set_Top(num);
			}
			if (AutoHideStripControl.GetNumberOfPanes(DockState.DockBottomAutoHide) > 0)
			{
				((ScrollableControl)this).get_DockPadding().set_Bottom(num);
			}
		}

		internal void RemoveContent(IDockContent content)
		{
			if (content == null)
			{
				throw new ArgumentNullException();
			}
			if (Contents.Contains(content))
			{
				Contents.Remove(content);
				OnContentRemoved(new DockContentEventArgs(content));
			}
		}

		internal void RemovePane(DockPane pane)
		{
			if (Panes.Contains(pane))
			{
				Panes.Remove(pane);
			}
		}

		internal void RemoveFloatWindow(FloatWindow floatWindow)
		{
			if (FloatWindows.Contains(floatWindow))
			{
				FloatWindows.Remove(floatWindow);
			}
		}

		public void SetPaneIndex(DockPane pane, int index)
		{
			int num = Panes.IndexOf(pane);
			if (num == -1)
			{
				throw new ArgumentException(Strings.DockPanel_SetPaneIndex_InvalidPane);
			}
			if ((index < 0 || index > Panes.Count - 1) && index != -1)
			{
				throw new ArgumentOutOfRangeException(Strings.DockPanel_SetPaneIndex_InvalidIndex);
			}
			if (num != index && (num != Panes.Count - 1 || index != -1))
			{
				Panes.Remove(pane);
				if (index == -1)
				{
					Panes.Add(pane);
				}
				else if (num < index)
				{
					Panes.AddAt(pane, index - 1);
				}
				else
				{
					Panes.AddAt(pane, index);
				}
			}
		}

		public void SuspendLayout(bool allWindows)
		{
			FocusManager.SuspendFocusTracking();
			((Control)this).SuspendLayout();
			if (allWindows)
			{
				SuspendMdiClientLayout();
			}
		}

		public void ResumeLayout(bool performLayout, bool allWindows)
		{
			FocusManager.ResumeFocusTracking();
			((Control)this).ResumeLayout(performLayout);
			if (allWindows)
			{
				ResumeMdiClientLayout(performLayout);
			}
		}

		private bool IsParentFormValid()
		{
			if (DocumentStyle == DocumentStyle.DockingSdi || DocumentStyle == DocumentStyle.DockingWindow)
			{
				return true;
			}
			if (!MdiClientExists)
			{
				GetMdiClientController().RenewMdiClient();
			}
			return MdiClientExists;
		}

		protected override void OnParentChanged(EventArgs e)
		{
			((Control)AutoHideWindow).set_Parent(((Control)this).get_Parent());
			MdiClientController mdiClientController = GetMdiClientController();
			Control parent = ((Control)this).get_Parent();
			mdiClientController.ParentForm = parent as Form;
			((Control)AutoHideWindow).BringToFront();
			((Control)this).OnParentChanged(e);
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			((ScrollableControl)this).OnVisibleChanged(e);
			if (((Control)this).get_Visible())
			{
				SetMdiClient();
			}
		}

		private void InvalidateWindowRegion()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			if (!((Component)this).get_DesignMode())
			{
				if (m_dummyControlPaintEventHandler == null)
				{
					m_dummyControlPaintEventHandler = new PaintEventHandler(DummyControl_Paint);
				}
				DummyControl.add_Paint(m_dummyControlPaintEventHandler);
				DummyControl.Invalidate();
			}
		}

		private void DummyControl_Paint(object sender, PaintEventArgs e)
		{
			DummyControl.remove_Paint(m_dummyControlPaintEventHandler);
			UpdateWindowRegion();
		}

		private void UpdateWindowRegion()
		{
			if (DocumentStyle == DocumentStyle.DockingMdi)
			{
				UpdateWindowRegion_ClipContent();
			}
			else if (DocumentStyle == DocumentStyle.DockingSdi || DocumentStyle == DocumentStyle.DockingWindow)
			{
				UpdateWindowRegion_FullDocumentArea();
			}
			else if (DocumentStyle == DocumentStyle.SystemMdi)
			{
				UpdateWindowRegion_EmptyDocumentArea();
			}
		}

		private void UpdateWindowRegion_FullDocumentArea()
		{
			SetRegion(null);
		}

		private void UpdateWindowRegion_EmptyDocumentArea()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Rectangle documentWindowBounds = DocumentWindowBounds;
			SetRegion((Rectangle[])(object)new Rectangle[1]
			{
				documentWindowBounds
			});
		}

		private void UpdateWindowRegion_ClipContent()
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			foreach (DockPane pane in Panes)
			{
				if (pane.DockState == DockState.Document)
				{
					num++;
				}
			}
			if (num == 0)
			{
				SetRegion(null);
				return;
			}
			Rectangle[] array = (Rectangle[])(object)new Rectangle[num];
			int num2 = 0;
			foreach (DockPane pane2 in Panes)
			{
				if (pane2.DockState == DockState.Document)
				{
					array[num2] = ((Control)this).RectangleToClient(((Control)pane2).RectangleToScreen(pane2.ContentRectangle));
					num2++;
				}
			}
			SetRegion(array);
		}

		private void SetRegion(Rectangle[] clipRects)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (!IsClipRectsChanged(clipRects))
			{
				return;
			}
			m_clipRects = clipRects;
			if (m_clipRects == null || m_clipRects.GetLength(0) == 0)
			{
				((Control)this).set_Region((Region)null);
				return;
			}
			Region val = new Region(new Rectangle(0, 0, ((Control)this).get_Width(), ((Control)this).get_Height()));
			Rectangle[] clipRects2 = m_clipRects;
			foreach (Rectangle val2 in clipRects2)
			{
				val.Exclude(val2);
			}
			((Control)this).set_Region(val);
		}

		private bool IsClipRectsChanged(Rectangle[] clipRects)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			if (clipRects == null && m_clipRects == null)
			{
				return false;
			}
			if (clipRects == null != (m_clipRects == null))
			{
				return true;
			}
			foreach (Rectangle val in clipRects)
			{
				bool flag = false;
				Rectangle[] clipRects2 = m_clipRects;
				foreach (Rectangle val2 in clipRects2)
				{
					if (val == val2)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return true;
				}
			}
			Rectangle[] clipRects3 = m_clipRects;
			foreach (Rectangle val3 in clipRects3)
			{
				bool flag2 = false;
				foreach (Rectangle val4 in clipRects)
				{
					if (val4 == val3)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					return true;
				}
			}
			return false;
		}

		protected virtual void OnContentAdded(DockContentEventArgs e)
		{
			((EventHandler<DockContentEventArgs>)((Component)this).get_Events().get_Item(ContentAddedEvent))?.Invoke(this, e);
		}

		protected virtual void OnContentRemoved(DockContentEventArgs e)
		{
			((EventHandler<DockContentEventArgs>)((Component)this).get_Events().get_Item(ContentRemovedEvent))?.Invoke(this, e);
		}

		public void SaveAsXml(string fileName)
		{
			Persistor.SaveAsXml(this, fileName);
		}

		public void SaveAsXml(string fileName, Encoding encoding)
		{
			Persistor.SaveAsXml(this, fileName, encoding);
		}

		public void SaveAsXml(Stream stream, Encoding encoding)
		{
			Persistor.SaveAsXml(this, stream, encoding);
		}

		public void SaveAsXml(Stream stream, Encoding encoding, bool upstream)
		{
			Persistor.SaveAsXml(this, stream, encoding, upstream);
		}

		public void LoadFromXml(string fileName, DeserializeDockContent deserializeContent)
		{
			Persistor.LoadFromXml(this, fileName, deserializeContent);
		}

		public void LoadFromXml(Stream stream, DeserializeDockContent deserializeContent)
		{
			Persistor.LoadFromXml(this, stream, deserializeContent);
		}

		public void LoadFromXml(Stream stream, DeserializeDockContent deserializeContent, bool closeStream)
		{
			Persistor.LoadFromXml(this, stream, deserializeContent, closeStream);
		}

		internal void SaveFocus()
		{
			DummyControl.Focus();
		}

		protected virtual void OnActiveDocumentChanged(EventArgs e)
		{
			((EventHandler)((Component)this).get_Events().get_Item(ActiveDocumentChangedEvent))?.Invoke(this, e);
		}

		protected void OnActiveContentChanged(EventArgs e)
		{
			((EventHandler)((Component)this).get_Events().get_Item(ActiveContentChangedEvent))?.Invoke(this, e);
		}

		protected virtual void OnActivePaneChanged(EventArgs e)
		{
			((EventHandler)((Component)this).get_Events().get_Item(ActivePaneChangedEvent))?.Invoke(this, e);
		}
	}
}

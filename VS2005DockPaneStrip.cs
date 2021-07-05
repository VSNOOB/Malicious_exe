using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal class VS2005DockPaneStrip : DockPaneStripBase
	{
		private class TabVS2005 : Tab
		{
			private int m_tabX;

			private int m_tabWidth;

			private int m_maxWidth;

			private bool m_flag;

			public int TabX
			{
				get
				{
					return m_tabX;
				}
				set
				{
					m_tabX = value;
				}
			}

			public int TabWidth
			{
				get
				{
					return m_tabWidth;
				}
				set
				{
					m_tabWidth = value;
				}
			}

			public int MaxWidth
			{
				get
				{
					return m_maxWidth;
				}
				set
				{
					m_maxWidth = value;
				}
			}

			protected internal bool Flag
			{
				get
				{
					return m_flag;
				}
				set
				{
					m_flag = value;
				}
			}

			public TabVS2005(IDockContent content)
				: base(content)
			{
			}
		}

		private sealed class InertButton : InertButtonBase
		{
			private Bitmap m_image0;

			private Bitmap m_image1;

			private int m_imageCategory = 0;

			public int ImageCategory
			{
				get
				{
					return m_imageCategory;
				}
				set
				{
					if (m_imageCategory != value)
					{
						m_imageCategory = value;
						((Control)this).Invalidate();
					}
				}
			}

			public override Bitmap Image => (ImageCategory == 0) ? m_image0 : m_image1;

			public InertButton(Bitmap image0, Bitmap image1)
			{
				m_image0 = image0;
				m_image1 = image1;
			}

			protected override void OnRefreshChanges()
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				if (ColorDocumentActiveText != ((Control)this).get_ForeColor())
				{
					((Control)this).set_ForeColor(ColorDocumentActiveText);
					((Control)this).Invalidate();
				}
			}
		}

		private const int _ToolWindowStripGapTop = 0;

		private const int _ToolWindowStripGapBottom = 1;

		private const int _ToolWindowStripGapLeft = 0;

		private const int _ToolWindowStripGapRight = 0;

		private const int _ToolWindowImageHeight = 16;

		private const int _ToolWindowImageWidth = 16;

		private const int _ToolWindowImageGapTop = 3;

		private const int _ToolWindowImageGapBottom = 1;

		private const int _ToolWindowImageGapLeft = 2;

		private const int _ToolWindowImageGapRight = 0;

		private const int _ToolWindowTextGapRight = 3;

		private const int _ToolWindowTabSeperatorGapTop = 3;

		private const int _ToolWindowTabSeperatorGapBottom = 3;

		private const int _DocumentStripGapTop = 0;

		private const int _DocumentStripGapBottom = 1;

		private const int _DocumentTabMaxWidth = 200;

		private const int _DocumentButtonGapTop = 4;

		private const int _DocumentButtonGapBottom = 4;

		private const int _DocumentButtonGapBetween = 0;

		private const int _DocumentButtonGapRight = 3;

		private const int _DocumentTabGapTop = 3;

		private const int _DocumentTabGapLeft = 3;

		private const int _DocumentTabGapRight = 3;

		private const int _DocumentIconGapBottom = 2;

		private const int _DocumentIconGapLeft = 8;

		private const int _DocumentIconGapRight = 0;

		private const int _DocumentIconHeight = 16;

		private const int _DocumentIconWidth = 16;

		private const int _DocumentTextGapRight = 3;

		private static Bitmap _imageButtonClose;

		private InertButton m_buttonClose;

		private static Bitmap _imageButtonWindowList;

		private static Bitmap _imageButtonWindowListOverflow;

		private InertButton m_buttonWindowList;

		private IContainer m_components;

		private ToolTip m_toolTip;

		private static string _toolTipClose;

		private static string _toolTipSelect;

		private Font m_boldFont;

		private int m_startDisplayingTab = 0;

		private int m_endDisplayingTab = 0;

		private bool m_documentTabsOverflow = false;

		private ContextMenuStrip m_selectMenu;

		private static Bitmap ImageButtonClose => _imageButtonClose;

		private InertButton ButtonClose
		{
			get
			{
				if (m_buttonClose == null)
				{
					m_buttonClose = new InertButton(ImageButtonClose, ImageButtonClose);
					m_toolTip.SetToolTip((Control)(object)m_buttonClose, ToolTipClose);
					((Control)m_buttonClose).add_Click((EventHandler)Close_Click);
					((Control)this).get_Controls().Add((Control)(object)m_buttonClose);
				}
				return m_buttonClose;
			}
		}

		private static Bitmap ImageButtonWindowList => _imageButtonWindowList;

		private static Bitmap ImageButtonWindowListOverflow => _imageButtonWindowListOverflow;

		private InertButton ButtonWindowList
		{
			get
			{
				if (m_buttonWindowList == null)
				{
					m_buttonWindowList = new InertButton(ImageButtonWindowList, ImageButtonWindowListOverflow);
					m_toolTip.SetToolTip((Control)(object)m_buttonWindowList, ToolTipSelect);
					((Control)m_buttonWindowList).add_Click((EventHandler)WindowList_Click);
					((Control)this).get_Controls().Add((Control)(object)m_buttonWindowList);
				}
				return m_buttonWindowList;
			}
		}

		private static GraphicsPath GraphicsPath => VS2005AutoHideStrip.GraphicsPath;

		private IContainer Components => m_components;

		private static int ToolWindowStripGapTop => 0;

		private static int ToolWindowStripGapBottom => 1;

		private static int ToolWindowStripGapLeft => 0;

		private static int ToolWindowStripGapRight => 0;

		private static int ToolWindowImageHeight => 16;

		private static int ToolWindowImageWidth => 16;

		private static int ToolWindowImageGapTop => 3;

		private static int ToolWindowImageGapBottom => 1;

		private static int ToolWindowImageGapLeft => 2;

		private static int ToolWindowImageGapRight => 0;

		private static int ToolWindowTextGapRight => 3;

		private static int ToolWindowTabSeperatorGapTop => 3;

		private static int ToolWindowTabSeperatorGapBottom => 3;

		private static string ToolTipClose
		{
			get
			{
				if (_toolTipClose == null)
				{
					_toolTipClose = Strings.DockPaneStrip_ToolTipClose;
				}
				return _toolTipClose;
			}
		}

		private static string ToolTipSelect
		{
			get
			{
				if (_toolTipSelect == null)
				{
					_toolTipSelect = Strings.DockPaneStrip_ToolTipWindowList;
				}
				return _toolTipSelect;
			}
		}

		private TextFormatFlags ToolWindowTextFormat
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Invalid comparison between Unknown and I4
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				TextFormatFlags val = (TextFormatFlags)32805;
				if ((int)((Control)this).get_RightToLeft() == 1)
				{
					return (TextFormatFlags)(val | 0x20000 | 2);
				}
				return val;
			}
		}

		private static int DocumentStripGapTop => 0;

		private static int DocumentStripGapBottom => 1;

		private TextFormatFlags DocumentTextFormat
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Invalid comparison between Unknown and I4
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				TextFormatFlags val = (TextFormatFlags)16793637;
				if ((int)((Control)this).get_RightToLeft() == 1)
				{
					return (TextFormatFlags)(val | 0x20000);
				}
				return val;
			}
		}

		private static int DocumentTabMaxWidth => 200;

		private static int DocumentButtonGapTop => 4;

		private static int DocumentButtonGapBottom => 4;

		private static int DocumentButtonGapBetween => 0;

		private static int DocumentButtonGapRight => 3;

		private static int DocumentTabGapTop => 3;

		private static int DocumentTabGapLeft => 3;

		private static int DocumentTabGapRight => 3;

		private static int DocumentIconGapBottom => 2;

		private static int DocumentIconGapLeft => 8;

		private static int DocumentIconGapRight => 0;

		private static int DocumentIconWidth => 16;

		private static int DocumentIconHeight => 16;

		private static int DocumentTextGapRight => 3;

		private static Pen PenToolWindowTabBorder => SystemPens.get_GrayText();

		private static Pen PenDocumentTabActiveBorder => SystemPens.get_ControlDarkDark();

		private static Pen PenDocumentTabInactiveBorder => SystemPens.get_GrayText();

		private static Brush BrushToolWindowActiveBackground => SystemBrushes.get_Control();

		private static Brush BrushDocumentActiveBackground => SystemBrushes.get_ControlLightLight();

		private static Brush BrushDocumentInactiveBackground => SystemBrushes.get_ControlLight();

		private static Color ColorToolWindowActiveText => SystemColors.get_ControlText();

		private static Color ColorDocumentActiveText => SystemColors.get_ControlText();

		private static Color ColorToolWindowInactiveText => SystemColors.get_ControlDarkDark();

		private static Color ColorDocumentInactiveText => SystemColors.get_ControlText();

		private Font BoldFont
		{
			get
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Expected O, but got Unknown
				if (m_boldFont == null)
				{
					m_boldFont = new Font(((Control)this).get_Font(), (FontStyle)1);
				}
				return m_boldFont;
			}
		}

		private int StartDisplayingTab
		{
			get
			{
				return m_startDisplayingTab;
			}
			set
			{
				m_startDisplayingTab = value;
				((Control)this).Invalidate();
			}
		}

		private int EndDisplayingTab
		{
			get
			{
				return m_endDisplayingTab;
			}
			set
			{
				m_endDisplayingTab = value;
			}
		}

		private bool DocumentTabsOverflow
		{
			set
			{
				if (m_documentTabsOverflow != value)
				{
					m_documentTabsOverflow = value;
					if (value)
					{
						ButtonWindowList.ImageCategory = 1;
					}
					else
					{
						ButtonWindowList.ImageCategory = 0;
					}
				}
			}
		}

		private Rectangle TabStripRectangle
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				if (base.Appearance == DockPane.AppearanceStyle.Document)
				{
					return TabStripRectangle_Document;
				}
				return TabStripRectangle_ToolWindow;
			}
		}

		private Rectangle TabStripRectangle_ToolWindow
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
				return new Rectangle(((Rectangle)(ref clientRectangle)).get_X(), ((Rectangle)(ref clientRectangle)).get_Top() + ToolWindowStripGapTop, ((Rectangle)(ref clientRectangle)).get_Width(), ((Rectangle)(ref clientRectangle)).get_Height() - ToolWindowStripGapTop - ToolWindowStripGapBottom);
			}
		}

		private Rectangle TabStripRectangle_Document
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
				return new Rectangle(((Rectangle)(ref clientRectangle)).get_X(), ((Rectangle)(ref clientRectangle)).get_Top() + DocumentStripGapTop, ((Rectangle)(ref clientRectangle)).get_Width(), ((Rectangle)(ref clientRectangle)).get_Height() - DocumentStripGapTop - ToolWindowStripGapBottom);
			}
		}

		private Rectangle TabsRectangle
		{
			get
			{
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_008c: Unknown result type (might be due to invalid IL or missing references)
				if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
				{
					return TabStripRectangle;
				}
				Rectangle tabStripRectangle = TabStripRectangle;
				int x = ((Rectangle)(ref tabStripRectangle)).get_X();
				int y = ((Rectangle)(ref tabStripRectangle)).get_Y();
				int width = ((Rectangle)(ref tabStripRectangle)).get_Width();
				int height = ((Rectangle)(ref tabStripRectangle)).get_Height();
				x += DocumentTabGapLeft;
				width -= DocumentTabGapLeft + DocumentTabGapRight + DocumentButtonGapRight + ((Control)ButtonClose).get_Width() + ((Control)ButtonWindowList).get_Width() + 2 * DocumentButtonGapBetween;
				return new Rectangle(x, y, width, height);
			}
		}

		private ContextMenuStrip SelectMenu => m_selectMenu;

		protected internal override Tab CreateTab(IDockContent content)
		{
			return new TabVS2005(content);
		}

		public VS2005DockPaneStrip(DockPane pane)
			: base(pane)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			((Control)this).SetStyle((ControlStyles)16, true);
			((Control)this).SetStyle((ControlStyles)2, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).SuspendLayout();
			((Control)this).set_Font(SystemInformation.get_MenuFont());
			m_components = (IContainer)new Container();
			m_toolTip = new ToolTip(Components);
			m_selectMenu = new ContextMenuStrip(Components);
			((Control)this).ResumeLayout();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				((IDisposable)Components).Dispose();
			}
			((Control)this).Dispose(disposing);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			((Control)this).OnFontChanged(e);
			if (m_boldFont != null)
			{
				m_boldFont.Dispose();
				m_boldFont = null;
			}
		}

		protected internal override int MeasureHeight()
		{
			if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				return MeasureHeight_ToolWindow();
			}
			return MeasureHeight_Document();
		}

		private int MeasureHeight_ToolWindow()
		{
			if (base.DockPane.IsAutoHide || base.Tabs.Count <= 1)
			{
				return 0;
			}
			return Math.Max(((Control)this).get_Font().get_Height(), ToolWindowImageHeight + ToolWindowImageGapTop + ToolWindowImageGapBottom) + ToolWindowStripGapTop + ToolWindowStripGapBottom;
		}

		private int MeasureHeight_Document()
		{
			return Math.Max(((Control)this).get_Font().get_Height() + DocumentTabGapTop, ((Control)ButtonClose).get_Height() + DocumentButtonGapTop + DocumentButtonGapBottom) + DocumentStripGapBottom + DocumentStripGapTop;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (base.Appearance == DockPane.AppearanceStyle.Document)
			{
				if (((Control)this).get_BackColor() != SystemColors.get_Control())
				{
					((Control)this).set_BackColor(SystemColors.get_Control());
				}
			}
			else if (((Control)this).get_BackColor() != SystemColors.get_ControlLight())
			{
				((Control)this).set_BackColor(SystemColors.get_ControlLight());
			}
			((Control)this).OnPaint(e);
			CalculateTabs();
			if (base.Appearance == DockPane.AppearanceStyle.Document && base.DockPane.ActiveContent != null && EnsureDocumentTabVisible(base.DockPane.ActiveContent, repaint: false))
			{
				CalculateTabs();
			}
			DrawTabStrip(e.get_Graphics());
		}

		protected override void OnRefreshChanges()
		{
			SetInertButtons();
			((Control)this).Invalidate();
		}

		protected internal override GraphicsPath GetOutline(int index)
		{
			if (base.Appearance == DockPane.AppearanceStyle.Document)
			{
				return GetOutline_Document(index);
			}
			return GetOutline_ToolWindow(index);
		}

		private GraphicsPath GetOutline_Document(int index)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			Rectangle rectangle = GetTabRectangle(index);
			((Rectangle)(ref rectangle)).set_X(((Rectangle)(ref rectangle)).get_X() - ((Rectangle)(ref rectangle)).get_Height() / 2);
			((Rectangle)(ref rectangle)).Intersect(TabsRectangle);
			rectangle = ((Control)this).RectangleToScreen(DrawHelper.RtlTransform((Control)(object)this, rectangle));
			int top = ((Rectangle)(ref rectangle)).get_Top();
			Rectangle val = ((Control)base.DockPane).RectangleToScreen(((Control)base.DockPane).get_ClientRectangle());
			GraphicsPath val2 = new GraphicsPath();
			GraphicsPath tabOutline_Document = GetTabOutline_Document(base.Tabs[index], rtlTransform: true, toScreen: true, full: true);
			val2.AddPath(tabOutline_Document, true);
			val2.AddLine(((Rectangle)(ref rectangle)).get_Right(), ((Rectangle)(ref rectangle)).get_Bottom(), ((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref rectangle)).get_Bottom());
			val2.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref rectangle)).get_Bottom(), ((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Bottom());
			val2.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Bottom());
			val2.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref rectangle)).get_Bottom());
			val2.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref rectangle)).get_Bottom(), ((Rectangle)(ref rectangle)).get_Right(), ((Rectangle)(ref rectangle)).get_Bottom());
			return val2;
		}

		private GraphicsPath GetOutline_ToolWindow(int index)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			Rectangle rectangle = GetTabRectangle(index);
			((Rectangle)(ref rectangle)).Intersect(TabsRectangle);
			rectangle = ((Control)this).RectangleToScreen(DrawHelper.RtlTransform((Control)(object)this, rectangle));
			int top = ((Rectangle)(ref rectangle)).get_Top();
			Rectangle val = ((Control)base.DockPane).RectangleToScreen(((Control)base.DockPane).get_ClientRectangle());
			GraphicsPath val2 = new GraphicsPath();
			GraphicsPath tabOutline = GetTabOutline(base.Tabs[index], rtlTransform: true, toScreen: true);
			val2.AddPath(tabOutline, true);
			val2.AddLine(((Rectangle)(ref rectangle)).get_Left(), ((Rectangle)(ref rectangle)).get_Top(), ((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref rectangle)).get_Top());
			val2.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref rectangle)).get_Top(), ((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Top());
			val2.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Top(), ((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Top());
			val2.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Top(), ((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref rectangle)).get_Top());
			val2.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref rectangle)).get_Top(), ((Rectangle)(ref rectangle)).get_Right(), ((Rectangle)(ref rectangle)).get_Top());
			return val2;
		}

		private void CalculateTabs()
		{
			if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				CalculateTabs_ToolWindow();
			}
			else
			{
				CalculateTabs_Document();
			}
		}

		private void CalculateTabs_ToolWindow()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (base.Tabs.Count <= 1 || base.DockPane.IsAutoHide)
			{
				return;
			}
			Rectangle tabStripRectangle = TabStripRectangle;
			int count = base.Tabs.Count;
			foreach (TabVS2005 item in (IEnumerable<Tab>)base.Tabs)
			{
				item.MaxWidth = GetMaxTabWidth(base.Tabs.IndexOf(item));
				item.Flag = false;
			}
			bool flag = true;
			int num = ((Rectangle)(ref tabStripRectangle)).get_Width() - ToolWindowStripGapLeft - ToolWindowStripGapRight;
			int num2 = 0;
			int num3 = num / count;
			int num4 = count;
			flag = true;
			while (flag && num4 > 0)
			{
				flag = false;
				foreach (TabVS2005 item2 in (IEnumerable<Tab>)base.Tabs)
				{
					if (!item2.Flag && item2.MaxWidth <= num3)
					{
						item2.Flag = true;
						item2.TabWidth = item2.MaxWidth;
						num2 += item2.TabWidth;
						flag = true;
						num4--;
					}
				}
				if (num4 != 0)
				{
					num3 = (num - num2) / num4;
				}
			}
			if (num4 > 0)
			{
				int num5 = num - num2 - num3 * num4;
				foreach (TabVS2005 item3 in (IEnumerable<Tab>)base.Tabs)
				{
					if (!item3.Flag)
					{
						item3.Flag = true;
						if (num5 > 0)
						{
							item3.TabWidth = num3 + 1;
							num5--;
						}
						else
						{
							item3.TabWidth = num3;
						}
					}
				}
			}
			int num6 = ((Rectangle)(ref tabStripRectangle)).get_X() + ToolWindowStripGapLeft;
			foreach (TabVS2005 item4 in (IEnumerable<Tab>)base.Tabs)
			{
				item4.TabX = num6;
				num6 += item4.TabWidth;
			}
		}

		private bool CalculateDocumentTab(Rectangle rectTabStrip, ref int x, int index)
		{
			bool result = false;
			TabVS2005 tabVS = base.Tabs[index] as TabVS2005;
			tabVS.MaxWidth = GetMaxTabWidth(index);
			int num = Math.Min(tabVS.MaxWidth, DocumentTabMaxWidth);
			if (x + num < ((Rectangle)(ref rectTabStrip)).get_Right() || index == StartDisplayingTab)
			{
				tabVS.TabX = x;
				tabVS.TabWidth = num;
				EndDisplayingTab = index;
			}
			else
			{
				tabVS.TabX = 0;
				tabVS.TabWidth = 0;
				result = true;
			}
			x += num;
			return result;
		}

		private void CalculateTabs_Document()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			if (m_startDisplayingTab >= base.Tabs.Count)
			{
				m_startDisplayingTab = 0;
			}
			Rectangle tabsRectangle = TabsRectangle;
			int x = ((Rectangle)(ref tabsRectangle)).get_X() + ((Rectangle)(ref tabsRectangle)).get_Height() / 2;
			bool flag = false;
			for (int i = StartDisplayingTab; i < base.Tabs.Count; i++)
			{
				flag = CalculateDocumentTab(tabsRectangle, ref x, i);
			}
			for (int j = 0; j < StartDisplayingTab; j++)
			{
				flag = CalculateDocumentTab(tabsRectangle, ref x, j);
			}
			if (!flag)
			{
				m_startDisplayingTab = 0;
				x = ((Rectangle)(ref tabsRectangle)).get_X() + ((Rectangle)(ref tabsRectangle)).get_Height() / 2;
				foreach (TabVS2005 item in (IEnumerable<Tab>)base.Tabs)
				{
					item.TabX = x;
					x += item.TabWidth;
				}
			}
			DocumentTabsOverflow = flag;
		}

		protected internal override void EnsureTabVisible(IDockContent content)
		{
			if (base.Appearance == DockPane.AppearanceStyle.Document && base.Tabs.Contains(content))
			{
				CalculateTabs();
				EnsureDocumentTabVisible(content, repaint: true);
			}
		}

		private bool EnsureDocumentTabVisible(IDockContent content, bool repaint)
		{
			int num = base.Tabs.IndexOf(content);
			TabVS2005 tabVS = base.Tabs[num] as TabVS2005;
			if (tabVS.TabWidth != 0)
			{
				return false;
			}
			StartDisplayingTab = num;
			if (repaint)
			{
				((Control)this).Invalidate();
			}
			return true;
		}

		private int GetMaxTabWidth(int index)
		{
			if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				return GetMaxTabWidth_ToolWindow(index);
			}
			return GetMaxTabWidth_Document(index);
		}

		private int GetMaxTabWidth_ToolWindow(int index)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			IDockContent content = base.Tabs[index].Content;
			Size val = TextRenderer.MeasureText(content.DockHandler.TabText, ((Control)this).get_Font());
			return ToolWindowImageWidth + ((Size)(ref val)).get_Width() + ToolWindowImageGapLeft + ToolWindowImageGapRight + ToolWindowTextGapRight;
		}

		private int GetMaxTabWidth_Document(int index)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			IDockContent content = base.Tabs[index].Content;
			Rectangle tabRectangle_Document = GetTabRectangle_Document(index);
			int height = ((Rectangle)(ref tabRectangle_Document)).get_Height();
			Size val = TextRenderer.MeasureText(content.DockHandler.TabText, BoldFont, new Size(DocumentTabMaxWidth, height), DocumentTextFormat);
			if (base.DockPane.DockPanel.ShowDocumentIcon)
			{
				return ((Size)(ref val)).get_Width() + DocumentIconWidth + DocumentIconGapLeft + DocumentIconGapRight + DocumentTextGapRight;
			}
			return ((Size)(ref val)).get_Width() + DocumentIconGapLeft + DocumentTextGapRight;
		}

		private void DrawTabStrip(Graphics g)
		{
			if (base.Appearance == DockPane.AppearanceStyle.Document)
			{
				DrawTabStrip_Document(g);
			}
			else
			{
				DrawTabStrip_ToolWindow(g);
			}
		}

		private void DrawTabStrip_Document(Graphics g)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			int count = base.Tabs.Count;
			if (count == 0)
			{
				return;
			}
			Rectangle tabStripRectangle = TabStripRectangle;
			Rectangle tabsRectangle = TabsRectangle;
			Rectangle rect = Rectangle.Empty;
			TabVS2005 tabVS = null;
			g.SetClip(DrawHelper.RtlTransform((Control)(object)this, tabsRectangle));
			for (int i = 0; i < count; i++)
			{
				rect = GetTabRectangle(i);
				if (base.Tabs[i].Content == base.DockPane.ActiveContent)
				{
					tabVS = base.Tabs[i] as TabVS2005;
				}
				else if (((Rectangle)(ref rect)).IntersectsWith(tabsRectangle))
				{
					DrawTab(g, base.Tabs[i] as TabVS2005, rect);
				}
			}
			g.SetClip(tabStripRectangle);
			g.DrawLine(PenDocumentTabActiveBorder, ((Rectangle)(ref tabStripRectangle)).get_Left(), ((Rectangle)(ref tabStripRectangle)).get_Bottom() - 1, ((Rectangle)(ref tabStripRectangle)).get_Right(), ((Rectangle)(ref tabStripRectangle)).get_Bottom() - 1);
			g.SetClip(DrawHelper.RtlTransform((Control)(object)this, tabsRectangle));
			if (tabVS != null)
			{
				rect = GetTabRectangle(base.Tabs.IndexOf(tabVS));
				if (((Rectangle)(ref rect)).IntersectsWith(tabsRectangle))
				{
					DrawTab(g, tabVS, rect);
				}
			}
		}

		private void DrawTabStrip_ToolWindow(Graphics g)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Rectangle tabStripRectangle = TabStripRectangle;
			g.DrawLine(PenToolWindowTabBorder, ((Rectangle)(ref tabStripRectangle)).get_Left(), ((Rectangle)(ref tabStripRectangle)).get_Top(), ((Rectangle)(ref tabStripRectangle)).get_Right(), ((Rectangle)(ref tabStripRectangle)).get_Top());
			for (int i = 0; i < base.Tabs.Count; i++)
			{
				DrawTab(g, base.Tabs[i] as TabVS2005, GetTabRectangle(i));
			}
		}

		private Rectangle GetTabRectangle(int index)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				return GetTabRectangle_ToolWindow(index);
			}
			return GetTabRectangle_Document(index);
		}

		private Rectangle GetTabRectangle_ToolWindow(int index)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Rectangle tabStripRectangle = TabStripRectangle;
			TabVS2005 tabVS = (TabVS2005)base.Tabs[index];
			return new Rectangle(tabVS.TabX, ((Rectangle)(ref tabStripRectangle)).get_Y(), tabVS.TabWidth, ((Rectangle)(ref tabStripRectangle)).get_Height());
		}

		private Rectangle GetTabRectangle_Document(int index)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Rectangle tabStripRectangle = TabStripRectangle;
			TabVS2005 tabVS = (TabVS2005)base.Tabs[index];
			return new Rectangle(tabVS.TabX, ((Rectangle)(ref tabStripRectangle)).get_Y() + DocumentTabGapTop, tabVS.TabWidth, ((Rectangle)(ref tabStripRectangle)).get_Height() - DocumentTabGapTop);
		}

		private void DrawTab(Graphics g, TabVS2005 tab, Rectangle rect)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				DrawTab_ToolWindow(g, tab, rect);
			}
			else
			{
				DrawTab_Document(g, tab, rect);
			}
		}

		private GraphicsPath GetTabOutline(Tab tab, bool rtlTransform, bool toScreen)
		{
			if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				return GetTabOutline_ToolWindow(tab, rtlTransform, toScreen);
			}
			return GetTabOutline_Document(tab, rtlTransform, toScreen, full: false);
		}

		private GraphicsPath GetTabOutline_ToolWindow(Tab tab, bool rtlTransform, bool toScreen)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = GetTabRectangle(base.Tabs.IndexOf(tab));
			if (rtlTransform)
			{
				val = DrawHelper.RtlTransform((Control)(object)this, val);
			}
			if (toScreen)
			{
				val = ((Control)this).RectangleToScreen(val);
			}
			DrawHelper.GetRoundedCornerTab(GraphicsPath, val, upCorner: false);
			return GraphicsPath;
		}

		private GraphicsPath GetTabOutline_Document(Tab tab, bool rtlTransform, bool toScreen, bool full)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Invalid comparison between Unknown and I4
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Invalid comparison between Unknown and I4
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Invalid comparison between Unknown and I4
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c4: Invalid comparison between Unknown and I4
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bd: Invalid comparison between Unknown and I4
			int num = 6;
			GraphicsPath.Reset();
			Rectangle val = GetTabRectangle(base.Tabs.IndexOf(tab));
			if (rtlTransform)
			{
				val = DrawHelper.RtlTransform((Control)(object)this, val);
			}
			if (toScreen)
			{
				val = ((Control)this).RectangleToScreen(val);
			}
			if (tab.Content == base.DockPane.ActiveContent || base.Tabs.IndexOf(tab) == StartDisplayingTab || full)
			{
				if ((int)((Control)this).get_RightToLeft() == 1)
				{
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Right() + ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Bottom());
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right() + ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Right() - ((Rectangle)(ref val)).get_Height() / 2 + num / 2, ((Rectangle)(ref val)).get_Top() + num / 2);
				}
				else
				{
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Left() - ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Bottom());
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left() - ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Left() + ((Rectangle)(ref val)).get_Height() / 2 - num / 2, ((Rectangle)(ref val)).get_Top() + num / 2);
				}
			}
			else if ((int)((Control)this).get_RightToLeft() == 1)
			{
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Bottom() - ((Rectangle)(ref val)).get_Height() / 2);
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Bottom() - ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Right() - ((Rectangle)(ref val)).get_Height() / 2 + num / 2, ((Rectangle)(ref val)).get_Top() + num / 2);
			}
			else
			{
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Bottom(), ((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Bottom() - ((Rectangle)(ref val)).get_Height() / 2);
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Bottom() - ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Left() + ((Rectangle)(ref val)).get_Height() / 2 - num / 2, ((Rectangle)(ref val)).get_Top() + num / 2);
			}
			if ((int)((Control)this).get_RightToLeft() == 1)
			{
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right() - ((Rectangle)(ref val)).get_Height() / 2 - num / 2, ((Rectangle)(ref val)).get_Top(), ((Rectangle)(ref val)).get_Left() + num / 2, ((Rectangle)(ref val)).get_Top());
				GraphicsPath.AddArc(new Rectangle(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Top(), num, num), 180f, 90f);
			}
			else
			{
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left() + ((Rectangle)(ref val)).get_Height() / 2 + num / 2, ((Rectangle)(ref val)).get_Top(), ((Rectangle)(ref val)).get_Right() - num / 2, ((Rectangle)(ref val)).get_Top());
				GraphicsPath.AddArc(new Rectangle(((Rectangle)(ref val)).get_Right() - num, ((Rectangle)(ref val)).get_Top(), num, num), -90f, 90f);
			}
			if (base.Tabs.IndexOf(tab) != EndDisplayingTab && base.Tabs.IndexOf(tab) != base.Tabs.Count - 1 && base.Tabs[base.Tabs.IndexOf(tab) + 1].Content == base.DockPane.ActiveContent && !full)
			{
				if ((int)((Control)this).get_RightToLeft() == 1)
				{
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Top() + num / 2, ((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Top() + ((Rectangle)(ref val)).get_Height() / 2);
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Top() + ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Left() + ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Bottom());
				}
				else
				{
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Top() + num / 2, ((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Top() + ((Rectangle)(ref val)).get_Height() / 2);
					GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Top() + ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Right() - ((Rectangle)(ref val)).get_Height() / 2, ((Rectangle)(ref val)).get_Bottom());
				}
			}
			else if ((int)((Control)this).get_RightToLeft() == 1)
			{
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Top() + num / 2, ((Rectangle)(ref val)).get_Left(), ((Rectangle)(ref val)).get_Bottom());
			}
			else
			{
				GraphicsPath.AddLine(((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Top() + num / 2, ((Rectangle)(ref val)).get_Right(), ((Rectangle)(ref val)).get_Bottom());
			}
			return GraphicsPath;
		}

		private void DrawTab_ToolWindow(Graphics g, TabVS2005 tab, Rectangle rect)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = default(Rectangle);
			((Rectangle)(ref val))._002Ector(((Rectangle)(ref rect)).get_X() + ToolWindowImageGapLeft, ((Rectangle)(ref rect)).get_Y() + ((Rectangle)(ref rect)).get_Height() - 1 - ToolWindowImageGapBottom - ToolWindowImageHeight, ToolWindowImageWidth, ToolWindowImageHeight);
			Rectangle val2 = val;
			((Rectangle)(ref val2)).set_X(((Rectangle)(ref val2)).get_X() + (((Rectangle)(ref val)).get_Width() + ToolWindowImageGapRight));
			((Rectangle)(ref val2)).set_Width(((Rectangle)(ref rect)).get_Width() - ((Rectangle)(ref val)).get_Width() - ToolWindowImageGapLeft - ToolWindowImageGapRight - ToolWindowTextGapRight);
			Rectangle val3 = DrawHelper.RtlTransform((Control)(object)this, rect);
			val2 = DrawHelper.RtlTransform((Control)(object)this, val2);
			val = DrawHelper.RtlTransform((Control)(object)this, val);
			GraphicsPath tabOutline = GetTabOutline(tab, rtlTransform: true, toScreen: false);
			if (base.DockPane.ActiveContent == tab.Content)
			{
				g.FillPath(BrushToolWindowActiveBackground, tabOutline);
				g.DrawPath(PenToolWindowTabBorder, tabOutline);
				TextRenderer.DrawText((IDeviceContext)(object)g, tab.Content.DockHandler.TabText, ((Control)this).get_Font(), val2, ColorToolWindowActiveText, ToolWindowTextFormat);
			}
			else
			{
				if (base.Tabs.IndexOf(base.DockPane.ActiveContent) != base.Tabs.IndexOf(tab) + 1)
				{
					Point point = default(Point);
					((Point)(ref point))._002Ector(((Rectangle)(ref rect)).get_Right(), ((Rectangle)(ref rect)).get_Top() + ToolWindowTabSeperatorGapTop);
					Point point2 = default(Point);
					((Point)(ref point2))._002Ector(((Rectangle)(ref rect)).get_Right(), ((Rectangle)(ref rect)).get_Bottom() - ToolWindowTabSeperatorGapBottom);
					g.DrawLine(PenToolWindowTabBorder, DrawHelper.RtlTransform((Control)(object)this, point), DrawHelper.RtlTransform((Control)(object)this, point2));
				}
				TextRenderer.DrawText((IDeviceContext)(object)g, tab.Content.DockHandler.TabText, ((Control)this).get_Font(), val2, ColorToolWindowInactiveText, ToolWindowTextFormat);
			}
			if (((Rectangle)(ref val3)).Contains(val))
			{
				g.DrawIcon(tab.Content.DockHandler.Icon, val);
			}
		}

		private void DrawTab_Document(Graphics g, TabVS2005 tab, Rectangle rect)
		{
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			if (tab.TabWidth == 0)
			{
				return;
			}
			Rectangle val = default(Rectangle);
			((Rectangle)(ref val))._002Ector(((Rectangle)(ref rect)).get_X() + DocumentIconGapLeft, ((Rectangle)(ref rect)).get_Y() + ((Rectangle)(ref rect)).get_Height() - 1 - DocumentIconGapBottom - DocumentIconHeight, DocumentIconWidth, DocumentIconHeight);
			Rectangle val2 = val;
			if (base.DockPane.DockPanel.ShowDocumentIcon)
			{
				((Rectangle)(ref val2)).set_X(((Rectangle)(ref val2)).get_X() + (((Rectangle)(ref val)).get_Width() + DocumentIconGapRight));
				((Rectangle)(ref val2)).set_Y(((Rectangle)(ref rect)).get_Y());
				((Rectangle)(ref val2)).set_Width(((Rectangle)(ref rect)).get_Width() - ((Rectangle)(ref val)).get_Width() - DocumentIconGapLeft - DocumentIconGapRight - DocumentTextGapRight);
				((Rectangle)(ref val2)).set_Height(((Rectangle)(ref rect)).get_Height());
			}
			else
			{
				((Rectangle)(ref val2)).set_Width(((Rectangle)(ref rect)).get_Width() - DocumentIconGapLeft - DocumentTextGapRight);
			}
			Rectangle val3 = DrawHelper.RtlTransform((Control)(object)this, rect);
			val2 = DrawHelper.RtlTransform((Control)(object)this, val2);
			val = DrawHelper.RtlTransform((Control)(object)this, val);
			GraphicsPath tabOutline = GetTabOutline(tab, rtlTransform: true, toScreen: false);
			if (base.DockPane.ActiveContent == tab.Content)
			{
				g.FillPath(BrushDocumentActiveBackground, tabOutline);
				g.DrawPath(PenDocumentTabActiveBorder, tabOutline);
				if (base.DockPane.IsActiveDocumentPane)
				{
					TextRenderer.DrawText((IDeviceContext)(object)g, tab.Content.DockHandler.TabText, BoldFont, val2, ColorDocumentActiveText, DocumentTextFormat);
				}
				else
				{
					TextRenderer.DrawText((IDeviceContext)(object)g, tab.Content.DockHandler.TabText, ((Control)this).get_Font(), val2, ColorDocumentActiveText, DocumentTextFormat);
				}
			}
			else
			{
				g.FillPath(BrushDocumentInactiveBackground, tabOutline);
				g.DrawPath(PenDocumentTabInactiveBorder, tabOutline);
				TextRenderer.DrawText((IDeviceContext)(object)g, tab.Content.DockHandler.TabText, ((Control)this).get_Font(), val2, ColorDocumentInactiveText, DocumentTextFormat);
			}
			if (((Rectangle)(ref val3)).Contains(val) && base.DockPane.DockPanel.ShowDocumentIcon)
			{
				g.DrawIcon(tab.Content.DockHandler.Icon, val);
			}
		}

		private void WindowList_Click(object sender, EventArgs e)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			Point location = ((Control)ButtonWindowList).get_Location();
			int num2 = ((Point)(ref location)).get_Y() + ((Control)ButtonWindowList).get_Height();
			((ToolStrip)SelectMenu).get_Items().Clear();
			foreach (TabVS2005 item in (IEnumerable<Tab>)base.Tabs)
			{
				IDockContent content = item.Content;
				ToolStripItem val = ((ToolStrip)SelectMenu).get_Items().Add(content.DockHandler.TabText, (Image)(object)content.DockHandler.Icon.ToBitmap());
				val.set_Tag((object)item.Content);
				val.add_Click((EventHandler)ContextMenuItem_Click);
			}
			((ToolStripDropDown)SelectMenu).Show((Control)(object)ButtonWindowList, num, num2);
		}

		private void ContextMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem val = sender as ToolStripMenuItem;
			if (val != null)
			{
				IDockContent activeContent = (IDockContent)((ToolStripItem)val).get_Tag();
				base.DockPane.ActiveContent = activeContent;
			}
		}

		private void SetInertButtons()
		{
			if (base.Appearance == DockPane.AppearanceStyle.ToolWindow)
			{
				if (m_buttonClose != null)
				{
					((Control)m_buttonClose).set_Left(-((Control)m_buttonClose).get_Width());
				}
				if (m_buttonWindowList != null)
				{
					((Control)m_buttonWindowList).set_Left(-((Control)m_buttonWindowList).get_Width());
				}
			}
			else
			{
				bool enabled = base.DockPane.ActiveContent == null || base.DockPane.ActiveContent.DockHandler.CloseButton;
				((Control)ButtonClose).set_Enabled(enabled);
				ButtonClose.RefreshChanges();
				ButtonWindowList.RefreshChanges();
			}
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			if (base.Appearance != DockPane.AppearanceStyle.Document)
			{
				((Control)this).OnLayout(levent);
				return;
			}
			Rectangle tabStripRectangle = TabStripRectangle;
			int num = ((Image)ButtonClose.Image).get_Width();
			int num2 = ((Image)ButtonClose.Image).get_Height();
			int num3 = ((Rectangle)(ref tabStripRectangle)).get_Height() - DocumentButtonGapTop - DocumentButtonGapBottom;
			if (num2 < num3)
			{
				num *= num3 / num2;
				num2 = num3;
			}
			Size val = default(Size);
			((Size)(ref val))._002Ector(num, num2);
			int num4 = ((Rectangle)(ref tabStripRectangle)).get_X() + ((Rectangle)(ref tabStripRectangle)).get_Width() - DocumentTabGapLeft - DocumentButtonGapRight - num;
			int num5 = ((Rectangle)(ref tabStripRectangle)).get_Y() + DocumentButtonGapTop;
			Point val2 = default(Point);
			((Point)(ref val2))._002Ector(num4, num5);
			((Control)ButtonClose).set_Bounds(DrawHelper.RtlTransform((Control)(object)this, new Rectangle(val2, val)));
			((Point)(ref val2)).Offset(-(DocumentButtonGapBetween + num), 0);
			((Control)ButtonWindowList).set_Bounds(DrawHelper.RtlTransform((Control)(object)this, new Rectangle(val2, val)));
			OnRefreshChanges();
			((Control)this).OnLayout(levent);
		}

		private void Close_Click(object sender, EventArgs e)
		{
			base.DockPane.CloseActiveContent();
		}

		protected internal override int HitTest(Point ptMouse)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			Rectangle tabsRectangle = TabsRectangle;
			Rectangle tabsRectangle2 = TabsRectangle;
			if (!((Rectangle)(ref tabsRectangle2)).Contains(ptMouse))
			{
				return -1;
			}
			foreach (Tab item in (IEnumerable<Tab>)base.Tabs)
			{
				GraphicsPath tabOutline = GetTabOutline(item, rtlTransform: true, toScreen: false);
				if (tabOutline.IsVisible(ptMouse))
				{
					return base.Tabs.IndexOf(item);
				}
			}
			return -1;
		}

		protected override void OnMouseHover(EventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			int num = HitTest(((Control)this).PointToClient(Control.get_MousePosition()));
			string text = string.Empty;
			((Control)this).OnMouseHover(e);
			if (num != -1)
			{
				TabVS2005 tabVS = base.Tabs[num] as TabVS2005;
				if (!string.IsNullOrEmpty(tabVS.Content.DockHandler.ToolTipText))
				{
					text = tabVS.Content.DockHandler.ToolTipText;
				}
				else if (tabVS.MaxWidth > tabVS.TabWidth)
				{
					text = tabVS.Content.DockHandler.TabText;
				}
			}
			if (m_toolTip.GetToolTip((Control)(object)this) != text)
			{
				m_toolTip.set_Active(false);
				m_toolTip.SetToolTip((Control)(object)this, text);
				m_toolTip.set_Active(true);
			}
			((Control)this).ResetMouseEventArgs();
		}

		protected override void OnRightToLeftChanged(EventArgs e)
		{
			((Control)this).OnRightToLeftChanged(e);
			((Control)this).PerformLayout();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal class VS2005AutoHideStrip : AutoHideStripBase
	{
		private class TabVS2005 : Tab
		{
			private int m_tabX = 0;

			private int m_tabWidth = 0;

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

			internal TabVS2005(IDockContent content)
				: base(content)
			{
			}
		}

		private const int _ImageHeight = 16;

		private const int _ImageWidth = 16;

		private const int _ImageGapTop = 2;

		private const int _ImageGapLeft = 4;

		private const int _ImageGapRight = 2;

		private const int _ImageGapBottom = 2;

		private const int _TextGapLeft = 0;

		private const int _TextGapRight = 0;

		private const int _TabGapTop = 3;

		private const int _TabGapLeft = 4;

		private const int _TabGapBetween = 10;

		private static StringFormat _stringFormatTabHorizontal;

		private static StringFormat _stringFormatTabVertical;

		private static Matrix _matrixIdentity = new Matrix();

		private static DockState[] _dockStates;

		private static GraphicsPath _graphicsPath;

		private StringFormat StringFormatTabHorizontal
		{
			get
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Expected O, but got Unknown
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Invalid comparison between Unknown and I4
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				if (_stringFormatTabHorizontal == null)
				{
					_stringFormatTabHorizontal = new StringFormat();
					_stringFormatTabHorizontal.set_Alignment((StringAlignment)0);
					_stringFormatTabHorizontal.set_LineAlignment((StringAlignment)1);
					_stringFormatTabHorizontal.set_FormatFlags((StringFormatFlags)4096);
				}
				if ((int)((Control)this).get_RightToLeft() == 1)
				{
					StringFormat stringFormatTabHorizontal = _stringFormatTabHorizontal;
					stringFormatTabHorizontal.set_FormatFlags((StringFormatFlags)(stringFormatTabHorizontal.get_FormatFlags() | 1));
				}
				else
				{
					StringFormat stringFormatTabHorizontal2 = _stringFormatTabHorizontal;
					stringFormatTabHorizontal2.set_FormatFlags((StringFormatFlags)(stringFormatTabHorizontal2.get_FormatFlags() & -2));
				}
				return _stringFormatTabHorizontal;
			}
		}

		private StringFormat StringFormatTabVertical
		{
			get
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Expected O, but got Unknown
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Invalid comparison between Unknown and I4
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_0070: Unknown result type (might be due to invalid IL or missing references)
				if (_stringFormatTabVertical == null)
				{
					_stringFormatTabVertical = new StringFormat();
					_stringFormatTabVertical.set_Alignment((StringAlignment)0);
					_stringFormatTabVertical.set_LineAlignment((StringAlignment)1);
					_stringFormatTabVertical.set_FormatFlags((StringFormatFlags)4098);
				}
				if ((int)((Control)this).get_RightToLeft() == 1)
				{
					StringFormat stringFormatTabVertical = _stringFormatTabVertical;
					stringFormatTabVertical.set_FormatFlags((StringFormatFlags)(stringFormatTabVertical.get_FormatFlags() | 1));
				}
				else
				{
					StringFormat stringFormatTabVertical2 = _stringFormatTabVertical;
					stringFormatTabVertical2.set_FormatFlags((StringFormatFlags)(stringFormatTabVertical2.get_FormatFlags() & -2));
				}
				return _stringFormatTabVertical;
			}
		}

		private static int ImageHeight => 16;

		private static int ImageWidth => 16;

		private static int ImageGapTop => 2;

		private static int ImageGapLeft => 4;

		private static int ImageGapRight => 2;

		private static int ImageGapBottom => 2;

		private static int TextGapLeft => 0;

		private static int TextGapRight => 0;

		private static int TabGapTop => 3;

		private static int TabGapLeft => 4;

		private static int TabGapBetween => 10;

		private static Brush BrushTabBackground => SystemBrushes.get_Control();

		private static Pen PenTabBorder => SystemPens.get_GrayText();

		private static Brush BrushTabText => SystemBrushes.FromSystemColor(SystemColors.get_ControlDarkDark());

		private static Matrix MatrixIdentity => _matrixIdentity;

		private static DockState[] DockStates
		{
			get
			{
				if (_dockStates == null)
				{
					_dockStates = new DockState[4];
					_dockStates[0] = DockState.DockLeftAutoHide;
					_dockStates[1] = DockState.DockRightAutoHide;
					_dockStates[2] = DockState.DockTopAutoHide;
					_dockStates[3] = DockState.DockBottomAutoHide;
				}
				return _dockStates;
			}
		}

		internal static GraphicsPath GraphicsPath
		{
			get
			{
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Expected O, but got Unknown
				if (_graphicsPath == null)
				{
					_graphicsPath = new GraphicsPath();
				}
				return _graphicsPath;
			}
		}

		public VS2005AutoHideStrip(DockPanel panel)
			: base(panel)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).SetStyle((ControlStyles)16, true);
			((Control)this).SetStyle((ControlStyles)2, true);
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).set_BackColor(SystemColors.get_ControlLight());
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics graphics = e.get_Graphics();
			DrawTabStrip(graphics);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			CalculateTabs();
			base.OnLayout(levent);
		}

		private void DrawTabStrip(Graphics g)
		{
			DrawTabStrip(g, DockState.DockTopAutoHide);
			DrawTabStrip(g, DockState.DockBottomAutoHide);
			DrawTabStrip(g, DockState.DockLeftAutoHide);
			DrawTabStrip(g, DockState.DockRightAutoHide);
		}

		private void DrawTabStrip(Graphics g, DockState dockState)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			Rectangle logicalTabStripRectangle = GetLogicalTabStripRectangle(dockState);
			if (((Rectangle)(ref logicalTabStripRectangle)).get_IsEmpty())
			{
				return;
			}
			Matrix transform = g.get_Transform();
			if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
			{
				Matrix val = new Matrix();
				val.RotateAt(90f, new PointF((float)((Rectangle)(ref logicalTabStripRectangle)).get_X() + (float)((Rectangle)(ref logicalTabStripRectangle)).get_Height() / 2f, (float)((Rectangle)(ref logicalTabStripRectangle)).get_Y() + (float)((Rectangle)(ref logicalTabStripRectangle)).get_Height() / 2f));
				g.set_Transform(val);
			}
			foreach (Pane item in (IEnumerable<Pane>)GetPanes(dockState))
			{
				foreach (TabVS2005 item2 in (IEnumerable<Tab>)item.AutoHideTabs)
				{
					DrawTab(g, item2);
				}
			}
			g.set_Transform(transform);
		}

		private void CalculateTabs()
		{
			CalculateTabs(DockState.DockTopAutoHide);
			CalculateTabs(DockState.DockBottomAutoHide);
			CalculateTabs(DockState.DockLeftAutoHide);
			CalculateTabs(DockState.DockRightAutoHide);
		}

		private void CalculateTabs(DockState dockState)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			Rectangle logicalTabStripRectangle = GetLogicalTabStripRectangle(dockState);
			int num = ((Rectangle)(ref logicalTabStripRectangle)).get_Height() - ImageGapTop - ImageGapBottom;
			int num2 = ImageWidth;
			if (num > ImageHeight)
			{
				num2 = ImageWidth * (num / ImageHeight);
			}
			int num3 = TabGapLeft + ((Rectangle)(ref logicalTabStripRectangle)).get_X();
			foreach (Pane item in (IEnumerable<Pane>)GetPanes(dockState))
			{
				foreach (TabVS2005 item2 in (IEnumerable<Tab>)item.AutoHideTabs)
				{
					int num4 = num2 + ImageGapLeft + ImageGapRight;
					Size val = TextRenderer.MeasureText(item2.Content.DockHandler.TabText, ((Control)this).get_Font());
					int num5 = num4 + ((Size)(ref val)).get_Width() + TextGapLeft + TextGapRight;
					item2.TabX = num3;
					item2.TabWidth = num5;
					num3 += num5;
				}
				num3 += TabGapBetween;
			}
		}

		private Rectangle RtlTransform(Rectangle rect, DockState dockState)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
			{
				return rect;
			}
			return DrawHelper.RtlTransform((Control)(object)this, rect);
		}

		private GraphicsPath GetTabOutline(TabVS2005 tab, bool transformed, bool rtlTransform)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			DockState dockState = tab.Content.DockHandler.DockState;
			Rectangle rect = GetTabRectangle(tab, transformed);
			if (rtlTransform)
			{
				rect = RtlTransform(rect, dockState);
			}
			bool upCorner = dockState == DockState.DockLeftAutoHide || dockState == DockState.DockBottomAutoHide;
			DrawHelper.GetRoundedCornerTab(GraphicsPath, rect, upCorner);
			return GraphicsPath;
		}

		private void DrawTab(Graphics g, TabVS2005 tab)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			Rectangle tabRectangle = GetTabRectangle(tab);
			if (!((Rectangle)(ref tabRectangle)).get_IsEmpty())
			{
				DockState dockState = tab.Content.DockHandler.DockState;
				IDockContent content = tab.Content;
				GraphicsPath tabOutline = GetTabOutline(tab, transformed: false, rtlTransform: true);
				g.FillPath(BrushTabBackground, tabOutline);
				g.DrawPath(PenTabBorder, tabOutline);
				Matrix transform = g.get_Transform();
				g.set_Transform(MatrixIdentity);
				Rectangle rect = tabRectangle;
				((Rectangle)(ref rect)).set_X(((Rectangle)(ref rect)).get_X() + ImageGapLeft);
				((Rectangle)(ref rect)).set_Y(((Rectangle)(ref rect)).get_Y() + ImageGapTop);
				int num = ((Rectangle)(ref tabRectangle)).get_Height() - ImageGapTop - ImageGapBottom;
				int num2 = ImageWidth;
				if (num > ImageHeight)
				{
					num2 = ImageWidth * (num / ImageHeight);
				}
				((Rectangle)(ref rect)).set_Height(num);
				((Rectangle)(ref rect)).set_Width(num2);
				rect = GetTransformedRectangle(dockState, rect);
				g.DrawIcon(((Form)content).get_Icon(), RtlTransform(rect, dockState));
				Rectangle val = tabRectangle;
				((Rectangle)(ref val)).set_X(((Rectangle)(ref val)).get_X() + (ImageGapLeft + num2 + ImageGapRight + TextGapLeft));
				((Rectangle)(ref val)).set_Width(((Rectangle)(ref val)).get_Width() - (ImageGapLeft + num2 + ImageGapRight + TextGapLeft));
				val = RtlTransform(GetTransformedRectangle(dockState, val), dockState);
				if (dockState == DockState.DockLeftAutoHide || dockState == DockState.DockRightAutoHide)
				{
					g.DrawString(content.DockHandler.TabText, ((Control)this).get_Font(), BrushTabText, RectangleF.op_Implicit(val), StringFormatTabVertical);
				}
				else
				{
					g.DrawString(content.DockHandler.TabText, ((Control)this).get_Font(), BrushTabText, RectangleF.op_Implicit(val), StringFormatTabHorizontal);
				}
				g.set_Transform(transform);
			}
		}

		private Rectangle GetLogicalTabStripRectangle(DockState dockState)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return GetLogicalTabStripRectangle(dockState, transformed: false);
		}

		private Rectangle GetLogicalTabStripRectangle(DockState dockState, bool transformed)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			if (!DockHelper.IsDockStateAutoHide(dockState))
			{
				return Rectangle.Empty;
			}
			int count = GetPanes(DockState.DockLeftAutoHide).Count;
			int count2 = GetPanes(DockState.DockRightAutoHide).Count;
			int count3 = GetPanes(DockState.DockTopAutoHide).Count;
			int count4 = GetPanes(DockState.DockBottomAutoHide).Count;
			int num = MeasureHeight();
			int num2;
			int num3;
			int num4;
			if (dockState == DockState.DockLeftAutoHide && count > 0)
			{
				num2 = 0;
				num3 = ((count3 != 0) ? num : 0);
				num4 = ((Control)this).get_Height() - ((count3 != 0) ? num : 0) - ((count4 != 0) ? num : 0);
			}
			else if (dockState == DockState.DockRightAutoHide && count2 > 0)
			{
				num2 = ((Control)this).get_Width() - num;
				if (count != 0 && num2 < num)
				{
					num2 = num;
				}
				num3 = ((count3 != 0) ? num : 0);
				num4 = ((Control)this).get_Height() - ((count3 != 0) ? num : 0) - ((count4 != 0) ? num : 0);
			}
			else if (dockState == DockState.DockTopAutoHide && count3 > 0)
			{
				num2 = ((count != 0) ? num : 0);
				num3 = 0;
				num4 = ((Control)this).get_Width() - ((count != 0) ? num : 0) - ((count2 != 0) ? num : 0);
			}
			else
			{
				if (dockState != DockState.DockBottomAutoHide || count4 <= 0)
				{
					return Rectangle.Empty;
				}
				num2 = ((count != 0) ? num : 0);
				num3 = ((Control)this).get_Height() - num;
				if (count3 != 0 && num3 < num)
				{
					num3 = num;
				}
				num4 = ((Control)this).get_Width() - ((count != 0) ? num : 0) - ((count2 != 0) ? num : 0);
			}
			if (!transformed)
			{
				return new Rectangle(num2, num3, num4, num);
			}
			return GetTransformedRectangle(dockState, new Rectangle(num2, num3, num4, num));
		}

		private Rectangle GetTabRectangle(TabVS2005 tab)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return GetTabRectangle(tab, transformed: false);
		}

		private Rectangle GetTabRectangle(TabVS2005 tab, bool transformed)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			DockState dockState = tab.Content.DockHandler.DockState;
			Rectangle logicalTabStripRectangle = GetLogicalTabStripRectangle(dockState);
			if (((Rectangle)(ref logicalTabStripRectangle)).get_IsEmpty())
			{
				return Rectangle.Empty;
			}
			int tabX = tab.TabX;
			int num = ((Rectangle)(ref logicalTabStripRectangle)).get_Y() + ((dockState != DockState.DockTopAutoHide && dockState != DockState.DockRightAutoHide) ? TabGapTop : 0);
			int tabWidth = tab.TabWidth;
			int num2 = ((Rectangle)(ref logicalTabStripRectangle)).get_Height() - TabGapTop;
			if (!transformed)
			{
				return new Rectangle(tabX, num, tabWidth, num2);
			}
			return GetTransformedRectangle(dockState, new Rectangle(tabX, num, tabWidth, num2));
		}

		private Rectangle GetTransformedRectangle(DockState dockState, Rectangle rect)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			if (dockState != DockState.DockLeftAutoHide && dockState != DockState.DockRightAutoHide)
			{
				return rect;
			}
			PointF[] array = (PointF[])(object)new PointF[1];
			((PointF)(ref array[0])).set_X((float)((Rectangle)(ref rect)).get_X() + (float)((Rectangle)(ref rect)).get_Width() / 2f);
			((PointF)(ref array[0])).set_Y((float)((Rectangle)(ref rect)).get_Y() + (float)((Rectangle)(ref rect)).get_Height() / 2f);
			Rectangle logicalTabStripRectangle = GetLogicalTabStripRectangle(dockState);
			Matrix val = new Matrix();
			val.RotateAt(90f, new PointF((float)((Rectangle)(ref logicalTabStripRectangle)).get_X() + (float)((Rectangle)(ref logicalTabStripRectangle)).get_Height() / 2f, (float)((Rectangle)(ref logicalTabStripRectangle)).get_Y() + (float)((Rectangle)(ref logicalTabStripRectangle)).get_Height() / 2f));
			val.TransformPoints(array);
			return new Rectangle((int)(((PointF)(ref array[0])).get_X() - (float)((Rectangle)(ref rect)).get_Height() / 2f + 0.5f), (int)(((PointF)(ref array[0])).get_Y() - (float)((Rectangle)(ref rect)).get_Width() / 2f + 0.5f), ((Rectangle)(ref rect)).get_Height(), ((Rectangle)(ref rect)).get_Width());
		}

		protected override IDockContent HitTest(Point ptMouse)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			DockState[] dockStates = DockStates;
			foreach (DockState dockState in dockStates)
			{
				Rectangle logicalTabStripRectangle = GetLogicalTabStripRectangle(dockState, transformed: true);
				if (!((Rectangle)(ref logicalTabStripRectangle)).Contains(ptMouse))
				{
					continue;
				}
				foreach (Pane item in (IEnumerable<Pane>)GetPanes(dockState))
				{
					DockState dockState2 = item.DockPane.DockState;
					foreach (TabVS2005 item2 in (IEnumerable<Tab>)item.AutoHideTabs)
					{
						GraphicsPath tabOutline = GetTabOutline(item2, transformed: true, rtlTransform: true);
						if (tabOutline.IsVisible(ptMouse))
						{
							return item2.Content;
						}
					}
				}
			}
			return null;
		}

		protected internal override int MeasureHeight()
		{
			return Math.Max(ImageGapBottom + ImageGapTop + ImageHeight, ((Control)this).get_Font().get_Height()) + TabGapTop;
		}

		protected override void OnRefreshChanges()
		{
			CalculateTabs();
			((Control)this).Invalidate();
		}

		protected override Tab CreateTab(IDockContent content)
		{
			return new TabVS2005(content);
		}
	}
}

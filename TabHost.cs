using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TabControl
{
	[Designer(typeof(TabHostDesigner))]
	public class TabHost : UserControl
	{
		private enum InsertDirection
		{
			Left,
			Rigth,
			None
		}

		public const int DefaultTabWidth = 100;

		private const int DefaultTabHeight = 26;

		private const int DefaultSelectedTabHeight = 30;

		private const int DefaultTabDistance = 1;

		private const int DefaultCloseButtonBorderOpacity = 150;

		private const int InsertionMarkerWidth = 10;

		private const int InsertionMarkerHeight = 5;

		private TabItemCollection tabs;

		private bool reordering;

		private DragDropTabVisual reorderingVisual;

		private TabItem reorderingTab;

		private int lastInsertIndex;

		private InsertDirection lastInsertDirection;

		private bool _showCloseButtons;

		private bool _closeButtonsOnlyForSelected;

		private CloseButtonAlignment _closeButtonAlignment;

		private Color _closeButtonColor;

		private Color _closeButtonColorSelected;

		private Color _closeButtonOverColor;

		private Color _closeButtonOverColorSelected;

		private Color _closeButtonPressedColor;

		private int _closeButtonBorderOpacity;

		private int _closeButtonBorderOpacitySelected;

		private int _tabHeight;

		private int _selectedTabHeight;

		private int _tabDistance;

		private TabAlignment _tabAlignment;

		private bool _allowTabReordering;

		private Color _insertionMarkerColor;

		private IContainer components = null;

		[Category("Appearance")]
		[Browsable(true)]
		public bool ShowCloseButtons
		{
			get
			{
				return _showCloseButtons;
			}
			set
			{
				if (_showCloseButtons != value)
				{
					_showCloseButtons = value;
					ForceUpdate();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public bool CloseButtonsOnlyForSelected
		{
			get
			{
				return _closeButtonsOnlyForSelected;
			}
			set
			{
				if (_closeButtonsOnlyForSelected != value)
				{
					_closeButtonsOnlyForSelected = value;
					ForceUpdate();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public CloseButtonAlignment CloseButtonAlignment
		{
			get
			{
				return _closeButtonAlignment;
			}
			set
			{
				if (_closeButtonAlignment != value)
				{
					_closeButtonAlignment = value;
					if (_showCloseButtons)
					{
						ForceUpdate();
					}
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public Color CloseButtonColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _closeButtonColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (_closeButtonColor != value)
				{
					_closeButtonColor = value;
					if (_showCloseButtons)
					{
						ForceUpdate();
					}
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public Color CloseButtonColorSelected
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _closeButtonColorSelected;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (_closeButtonColorSelected != value)
				{
					_closeButtonColorSelected = value;
					if (_showCloseButtons)
					{
						ForceUpdate();
					}
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public Color CloseButtonOverColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _closeButtonOverColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_closeButtonOverColor = value;
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public Color CloseButtonOverColorSelected
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _closeButtonOverColorSelected;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_closeButtonOverColorSelected = value;
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public Color CloseButtonPressedColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _closeButtonPressedColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_closeButtonPressedColor = value;
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public int CloseButtonBorderOpacity
		{
			get
			{
				return _closeButtonBorderOpacity;
			}
			set
			{
				if (_closeButtonBorderOpacity != value)
				{
					_closeButtonBorderOpacity = value;
					if (_showCloseButtons)
					{
						ForceUpdate();
					}
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public int CloseButtonBorderOpacitySelected
		{
			get
			{
				return _closeButtonBorderOpacitySelected;
			}
			set
			{
				if (_closeButtonBorderOpacitySelected != value)
				{
					_closeButtonBorderOpacitySelected = value;
					if (_showCloseButtons)
					{
						ForceUpdate();
					}
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public int TabHeight
		{
			get
			{
				return _tabHeight;
			}
			set
			{
				if (_tabHeight != value)
				{
					_tabHeight = value;
					Relayout();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public int SelectedTabHeight
		{
			get
			{
				return _selectedTabHeight;
			}
			set
			{
				if (_selectedTabHeight != value)
				{
					_selectedTabHeight = value;
					Relayout();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public int TabDistance
		{
			get
			{
				return _tabDistance;
			}
			set
			{
				if (_tabDistance != value)
				{
					_tabDistance = value;
					Relayout();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public TabAlignment TabAlignment
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _tabAlignment;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (_tabAlignment != value)
				{
					_tabAlignment = value;
					Relayout();
				}
			}
		}

		[Category("Behavior")]
		[Browsable(true)]
		public bool AllowTabReordering
		{
			get
			{
				return _allowTabReordering;
			}
			set
			{
				_allowTabReordering = value;
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public Color InsertionMarkerColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _insertionMarkerColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_insertionMarkerColor = value;
			}
		}

		[DesignerSerializationVisibility(/*Could not decode attribute arguments.*/)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		[Browsable(true)]
		public TabItemCollection Tabs
		{
			get
			{
				return tabs;
			}
			set
			{
				tabs = value;
			}
		}

		public TabHost()
			: this()
		{
			InitializeComponent();
			tabs = new TabItemCollection(this);
			TabAlignment = (TabAlignment)0;
			TabDistance = 1;
			TabHeight = 26;
			SelectedTabHeight = 30;
			((Control)this).SetStyle((ControlStyles)73730, true);
			((Control)this).UpdateStyles();
		}

		protected override void OnResize(EventArgs e)
		{
			((UserControl)this).OnResize(e);
			LayoutTabs();
			ForceUpdate();
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			((Control)this).OnBackColorChanged(e);
			ForceUpdate();
		}

		protected override void OnPaddingChanged(EventArgs e)
		{
			((ScrollableControl)this).OnPaddingChanged(e);
			Relayout();
		}

		private int GetTabStartPosition()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Invalid comparison between Unknown and I4
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Invalid comparison between Unknown and I4
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Invalid comparison between Unknown and I4
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Invalid comparison between Unknown and I4
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			Padding padding;
			if ((int)_tabAlignment == 0)
			{
				padding = ((Control)this).get_Padding();
				return ((Padding)(ref padding)).get_Left();
			}
			if ((int)_tabAlignment == 1)
			{
				padding = ((Control)this).get_Padding();
				return ((Padding)(ref padding)).get_Left();
			}
			if ((int)_tabAlignment == 2)
			{
				padding = ((Control)this).get_Padding();
				return ((Padding)(ref padding)).get_Top();
			}
			if ((int)_tabAlignment == 3)
			{
				padding = ((Control)this).get_Padding();
				return ((Padding)(ref padding)).get_Top();
			}
			return 0;
		}

		private int GetTabHeight(TabItem item)
		{
			if (item.Selected)
			{
				return (item.SelectedTabHeigth == 0) ? _selectedTabHeight : item.SelectedTabHeigth;
			}
			return (item.TabHeigth == 0) ? _tabHeight : item.TabHeigth;
		}

		private void LayoutTabs()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected I4, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			int num = GetTabStartPosition();
			for (int i = 0; i < tabs.Count; i++)
			{
				TabItem tabItem = tabs[i];
				int tabWidth = tabItem.TabWidth;
				int tabHeight = GetTabHeight(tabItem);
				TabAlignment tabAlignment = _tabAlignment;
				TabAlignment val = tabAlignment;
				Padding padding;
				switch ((int)val)
				{
				case 0:
				{
					((Control)tabItem).set_Left(num);
					int num3 = ((Control)this).get_Height() - tabHeight;
					padding = ((Control)this).get_Padding();
					((Control)tabItem).set_Top(num3 - ((Padding)(ref padding)).get_Bottom());
					tabItem.Width = tabWidth;
					tabItem.Height = tabHeight;
					break;
				}
				case 1:
					((Control)tabItem).set_Left(num);
					padding = ((Control)this).get_Padding();
					((Control)tabItem).set_Top(((Padding)(ref padding)).get_Top());
					tabItem.Width = tabWidth;
					tabItem.Height = tabHeight;
					break;
				case 2:
					padding = ((Control)this).get_Padding();
					((Control)tabItem).set_Left(((Padding)(ref padding)).get_Left());
					((Control)tabItem).set_Top(num);
					tabItem.Width = tabHeight;
					tabItem.Height = tabWidth;
					break;
				case 3:
				{
					int num2 = ((Control)this).get_Width() - tabHeight;
					padding = ((Control)this).get_Padding();
					((Control)tabItem).set_Left(num2 - ((Padding)(ref padding)).get_Right());
					((Control)tabItem).set_Top(num);
					tabItem.Width = tabHeight;
					tabItem.Height = tabWidth;
					break;
				}
				}
				num += tabWidth + _tabDistance;
			}
		}

		private void ForceUpdate()
		{
			((Control)this).Refresh();
		}

		private Point GetVisualLocation(TabItem tab, Point mousePosition)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected I4, but got Unknown
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			Point empty = Point.Empty;
			TabAlignment tabAlignment = _tabAlignment;
			TabAlignment val = tabAlignment;
			switch ((int)val)
			{
			case 0:
				((Point)(ref empty))._002Ector(KeepInHostWidth(((Control)tab).get_Left() + ((Point)(ref mousePosition)).get_X() - tab.Width / 2, tab.Width), ((Control)this).get_Height() - tab.Height);
				break;
			case 1:
				((Point)(ref empty))._002Ector(KeepInHostWidth(((Control)tab).get_Left() + ((Point)(ref mousePosition)).get_X() - tab.Width / 2, tab.Width), 0);
				break;
			case 2:
				((Point)(ref empty))._002Ector(0, KeepInHostHeight(((Control)tab).get_Top() + ((Point)(ref mousePosition)).get_Y() - tab.Height / 2, tab.Height));
				break;
			case 3:
				((Point)(ref empty))._002Ector(((Control)this).get_Width() - tab.Width, KeepInHostHeight(((Control)tab).get_Top() + ((Point)(ref mousePosition)).get_Y() - tab.Height / 2, tab.Height));
				break;
			}
			return ((Control)this).PointToScreen(empty);
		}

		private Point MousePositionToHost(TabItem tab, Point mousePosition)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			Point result = mousePosition;
			if ((int)_tabAlignment == 0 || (int)_tabAlignment == 1)
			{
				((Point)(ref result))._002Ector(((Point)(ref mousePosition)).get_X() + ((Control)tab).get_Left(), ((Point)(ref mousePosition)).get_Y());
			}
			else
			{
				((Point)(ref result))._002Ector(((Point)(ref mousePosition)).get_X(), ((Point)(ref mousePosition)).get_Y() + ((Control)tab).get_Top());
			}
			return result;
		}

		private Point ConstrainPositionToHost(Point mousePosition)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected I4, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			TabItem tabItem = tabs[tabs.Count - 1];
			TabAlignment tabAlignment = _tabAlignment;
			TabAlignment val = tabAlignment;
			Padding padding;
			switch ((int)val)
			{
			case 0:
				padding = ((Control)this).get_Padding();
				return new Point(Math.Max(((Padding)(ref padding)).get_Left() + 1, Math.Min(((Control)tabItem).get_Left() + tabItem.Width - 1, ((Point)(ref mousePosition)).get_X())), ((Control)this).get_Height() - 1);
			case 1:
				padding = ((Control)this).get_Padding();
				return new Point(Math.Max(((Padding)(ref padding)).get_Left() + 1, Math.Min(((Control)tabItem).get_Left() + tabItem.Width - 1, ((Point)(ref mousePosition)).get_X())), 1);
			case 2:
				padding = ((Control)this).get_Padding();
				return new Point(1, Math.Max(((Padding)(ref padding)).get_Top() + 1, Math.Min(((Control)tabItem).get_Top() + tabItem.Height - 1, ((Point)(ref mousePosition)).get_Y())));
			case 3:
			{
				int num = ((Control)this).get_Width() - 1;
				padding = ((Control)this).get_Padding();
				return new Point(num, Math.Max(((Padding)(ref padding)).get_Top() + 1, Math.Min(((Control)tabItem).get_Top() + tabItem.Height - 1, ((Point)(ref mousePosition)).get_Y())));
			}
			default:
				return Point.Empty;
			}
		}

		private int GetReorderInsertLocation(TabItem tab, Point mousePosition)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Point mousePosition2 = MousePositionToHost(tab, mousePosition);
			mousePosition2 = ConstrainPositionToHost(mousePosition2);
			TabItem tabItem = ((Control)this).GetChildAtPoint(mousePosition2) as TabItem;
			if (tabItem != null)
			{
				return tabs.IndexOf(tabItem);
			}
			return -1;
		}

		private InsertDirection GetInsertDirection(TabItem tab, Point mousePosition, int insertIndex)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Invalid comparison between Unknown and I4
			Point val = MousePositionToHost(tab, mousePosition);
			int num = tabs.IndexOf(tab);
			if (insertIndex == num || insertIndex == -1)
			{
				return InsertDirection.None;
			}
			TabItem tabItem = tabs[insertIndex];
			if ((int)_tabAlignment == 0 || (int)_tabAlignment == 1)
			{
				if (((Point)(ref val)).get_X() - ((Control)tabItem).get_Left() <= tabItem.Width / 2)
				{
					if (insertIndex != num + 1)
					{
						return InsertDirection.Left;
					}
					return InsertDirection.None;
				}
				if (insertIndex != num - 1)
				{
					return InsertDirection.Rigth;
				}
				return InsertDirection.None;
			}
			if (((Point)(ref val)).get_Y() - ((Control)tabItem).get_Top() <= tabItem.Height / 2)
			{
				if (insertIndex != num + 1)
				{
					return InsertDirection.Left;
				}
				return InsertDirection.None;
			}
			if (insertIndex != num - 1)
			{
				return InsertDirection.Rigth;
			}
			return InsertDirection.None;
		}

		private Color GetCurrentTabBackColor(TabItem tab)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (tab.Selected)
			{
				return tab.SelectedBackColor;
			}
			return ((Control)tab).get_BackColor();
		}

		private int KeepInHostWidth(int left, int width)
		{
			return Math.Max(0, Math.Min(((Control)this).get_Width() - width, left));
		}

		private int KeepInHostHeight(int top, int height)
		{
			return Math.Max(0, Math.Min(((Control)this).get_Height() - height, top));
		}

		private void InsertTabAt(TabItem tab, int index)
		{
			int num = tabs.IndexOf(tab);
			tabs.RemoveAt(num);
			if (index > num)
			{
				tabs.Insert(index - 1, tab);
			}
			else
			{
				tabs.Insert(index, tab);
			}
			Relayout();
		}

		private void TabHost_Paint(object sender, PaintEventArgs e)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Invalid comparison between Unknown and I4
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Invalid comparison between Unknown and I4
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Invalid comparison between Unknown and I4
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			if (!reordering)
			{
				return;
			}
			if (lastInsertDirection == InsertDirection.None)
			{
				e.get_Graphics().FillRectangle((Brush)new SolidBrush(((Control)this).get_BackColor()), ((Control)this).get_Bounds());
				return;
			}
			int num = 0;
			int num2 = 0;
			TabItem tabItem = tabs[lastInsertIndex];
			if ((int)_tabAlignment != 0 && (int)_tabAlignment != 1)
			{
				num2 = ((lastInsertDirection != 0) ? KeepInHostHeight(((Control)tabItem).get_Top() + tabItem.Height - 2 + _tabDistance / 2 + ((_tabDistance % 2 == 1) ? 1 : 0), 5) : KeepInHostHeight(((Control)tabItem).get_Top() - 2 - _tabDistance / 2, 5));
				num = (((int)_tabAlignment == 2) ? (((Control)this).get_Width() - 5) : 0);
			}
			else
			{
				num = ((lastInsertDirection != 0) ? KeepInHostWidth(((Control)tabItem).get_Left() + tabItem.Width - 5 + _tabDistance / 2 + ((_tabDistance % 2 == 1) ? 1 : 0), 10) : KeepInHostWidth(((Control)tabItem).get_Left() - 5 - _tabDistance / 2, 10));
				num2 = (((int)_tabAlignment != 0) ? (((Control)this).get_Height() - 5) : 0);
			}
			Rectangle bounds = default(Rectangle);
			((Rectangle)(ref bounds))._002Ector(num, num2, 10, 5);
			DrawInsertionMarker(bounds, e.get_Graphics());
		}

		protected void DrawInsertionMarker(Rectangle bounds, Graphics g)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Expected I4, but got Unknown
			Color val = ((_insertionMarkerColor != Color.Empty) ? _insertionMarkerColor : Color.get_Black());
			Brush val2 = (Brush)new SolidBrush(val);
			GraphicsPath val3 = new GraphicsPath();
			val3.AddLine(0, 0, 10, 0);
			val3.AddLine(10, 0, 5, 5);
			val3.AddLine(5, 5, 0, 0);
			Bitmap val4 = new Bitmap(10, 5, (PixelFormat)2498570);
			try
			{
				Graphics val5 = Graphics.FromImage((Image)(object)val4);
				try
				{
					val5.FillPath(val2, val3);
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
				TabAlignment tabAlignment = _tabAlignment;
				TabAlignment val6 = tabAlignment;
				switch ((int)val6)
				{
				case 0:
					g.DrawImageUnscaled((Image)(object)val4, ((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top());
					break;
				case 1:
					((Image)val4).RotateFlip((RotateFlipType)2);
					g.DrawImageUnscaled((Image)(object)val4, ((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top());
					break;
				case 2:
					((Image)val4).RotateFlip((RotateFlipType)1);
					g.DrawImageUnscaled((Image)(object)val4, ((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top());
					break;
				case 3:
					((Image)val4).RotateFlip((RotateFlipType)3);
					g.DrawImageUnscaled((Image)(object)val4, ((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top());
					break;
				}
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
		}

		public void ItemListChanged(TabItem item, bool remove)
		{
			if (remove)
			{
				((Control)this).get_Controls().Remove((Control)(object)item);
			}
			else
			{
				item.Owner = this;
				if (!((Control)this).get_Controls().Contains((Control)(object)item))
				{
					((Control)this).get_Controls().Add((Control)(object)item);
				}
				((Control)this).PerformLayout();
			}
			LayoutTabs();
		}

		public void ItemListChanged(TabItem[] items, bool remove)
		{
			foreach (TabItem tabItem in items)
			{
				if (remove)
				{
					((Control)this).get_Controls().Remove((Control)(object)tabItem);
				}
				else if (!((Control)this).get_Controls().Contains((Control)(object)tabItem))
				{
					((Control)this).get_Controls().Add((Control)(object)tabItem);
				}
			}
			LayoutTabs();
		}

		public void TabItemSelected(TabItem item)
		{
			for (int i = 0; i < tabs.Count; i++)
			{
				if (tabs[i] != item)
				{
					tabs[i].Selected = false;
				}
			}
			Relayout();
		}

		public void Relayout()
		{
			LayoutTabs();
			ForceUpdate();
		}

		public void TabStartDrag(TabItem tab, Point mousePosition)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			reorderingVisual = new DragDropTabVisual(tab);
			((Form)reorderingVisual).set_Location(GetVisualLocation(tab, mousePosition));
			((Control)reorderingVisual).Show();
			Form val = ((Control)this).FindForm();
			if (val != null)
			{
				val.Activate();
			}
			reorderingTab = tab;
			reordering = true;
		}

		public void TabEndDrag(TabItem tab, Point mousePosition)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			if (reorderingVisual != null)
			{
				((Control)reorderingVisual).Hide();
				((Component)reorderingVisual).Dispose();
				reorderingVisual = null;
			}
			int reorderInsertLocation = GetReorderInsertLocation(tab, mousePosition);
			lastInsertDirection = GetInsertDirection(tab, mousePosition, reorderInsertLocation);
			if (lastInsertDirection != InsertDirection.None)
			{
				if (lastInsertDirection == InsertDirection.Left)
				{
					InsertTabAt(reorderingTab, reorderInsertLocation);
				}
				else
				{
					InsertTabAt(reorderingTab, reorderInsertLocation + 1);
				}
			}
			reorderingTab = null;
			lastInsertDirection = InsertDirection.None;
			((Control)this).Invalidate();
			reordering = false;
		}

		public void TabUpdateDrag(TabItem tab, Point mousePosition)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			if (reorderingVisual != null)
			{
				((Form)reorderingVisual).set_Location(GetVisualLocation(tab, mousePosition));
			}
			int reorderInsertLocation = GetReorderInsertLocation(tab, mousePosition);
			lastInsertIndex = ((reorderInsertLocation == -1) ? lastInsertIndex : reorderInsertLocation);
			lastInsertDirection = GetInsertDirection(tab, mousePosition, lastInsertIndex);
			((Control)this).Invalidate();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				((IDisposable)components).Dispose();
			}
			((ContainerControl)this).Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Expected O, but got Unknown
			((Control)this).SuspendLayout();
			((ContainerControl)this).set_AutoScaleDimensions(new SizeF(6f, 13f));
			((ContainerControl)this).set_AutoScaleMode((AutoScaleMode)1);
			((Control)this).set_Name("TabHost");
			((Control)this).add_Paint(new PaintEventHandler(TabHost_Paint));
			((Control)this).ResumeLayout(false);
		}
	}
}

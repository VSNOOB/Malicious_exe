using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace TabControl
{
	[DefaultEvent("TabSelected")]
	public class TabItem : UserControl
	{
		private const int CornerRoundness = 5;

		private const int CloseButtonWidth = 12;

		private const int CloseButtonHeight = 12;

		private const int CloseButtonDistance = 12;

		private static ColorMatrix grayscaleMatrix;

		private Brush backBrush;

		private Brush foreBrush;

		private Brush currentBackBrush;

		private Brush currentForeBrush;

		private Pen borderPen;

		private GraphicsPath backgroundPath;

		private CloseButtonState closeButtonState;

		private Rectangle closeButtonLocation;

		private bool reordering;

		private Point lastMouseDownLocation;

		private bool ignoreNextWidthValue;

		private bool ignoreNextHeightValue;

		private TabHost _owner;

		private Color _borderColor;

		private Color _highlightBackColor;

		private Color _highlightForeColor;

		private Color _selectedBackColor;

		private Color _selectedForeColor;

		private string _tabText;

		private ContentAlignment _textAlignment;

		private Image _image;

		private TextImageRelation _textImageRelation;

		private int _tabWidth;

		private int _tabHeigth;

		private int _selectedTabHeigth;

		private bool _selected;

		private bool _autoEllipsis;

		private IContainer components = null;

		private static ColorMatrix DisabledImageMatrix
		{
			get
			{
				if (grayscaleMatrix == null)
				{
					grayscaleMatrix = MultiplyColorMatrix(matrix2: new float[5][]
					{
						new float[5]
						{
							0.2125f,
							0.2125f,
							0.2125f,
							0f,
							0f
						},
						new float[5]
						{
							0.2577f,
							0.2577f,
							0.2577f,
							0f,
							0f
						},
						new float[5]
						{
							0.0361f,
							0.0361f,
							0.0361f,
							0f,
							0f
						},
						new float[5]
						{
							0f,
							0f,
							0f,
							1f,
							0f
						},
						new float[5]
						{
							0.38f,
							0.38f,
							0.38f,
							0f,
							1f
						}
					}, matrix1: new float[5][]
					{
						new float[5]
						{
							1f,
							0f,
							0f,
							0f,
							0f
						},
						new float[5]
						{
							0f,
							1f,
							0f,
							0f,
							0f
						},
						new float[5]
						{
							0f,
							0f,
							1f,
							0f,
							0f
						},
						new float[5]
						{
							0f,
							0f,
							0f,
							0.7f,
							0f
						},
						new float[5]
					});
				}
				return grayscaleMatrix;
			}
		}

		[DesignerSerializationVisibility(/*Could not decode attribute arguments.*/)]
		[Browsable(false)]
		public TabHost Owner
		{
			get
			{
				return _owner;
			}
			set
			{
				_owner = value;
			}
		}

		public Color BorderColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _borderColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Expected O, but got Unknown
				if (_borderColor != value)
				{
					_borderColor = value;
					_ = _borderColor;
					if (_borderColor != Color.Empty)
					{
						borderPen = new Pen(_borderColor);
					}
					else
					{
						borderPen = null;
					}
					ForceUpdate();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual Color HighlightBackColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _highlightBackColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_highlightBackColor = value;
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual Color HighlightForeColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _highlightForeColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0003: Unknown result type (might be due to invalid IL or missing references)
				_highlightForeColor = value;
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual Color SelectedBackColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _selectedBackColor;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				if (((Color)(ref _selectedBackColor)).ToArgb() != ((Color)(ref value)).ToArgb())
				{
					_selectedBackColor = value;
					UpdateSelectedState();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual Color SelectedForeColor
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _selectedForeColor;
			}
			set
			{
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				if (((Color)(ref _selectedForeColor)).ToArgb() != ((Color)(ref value)).ToArgb())
				{
					_selectedForeColor = value;
					UpdateSelectedState();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public string TabText
		{
			get
			{
				return _tabText;
			}
			set
			{
				if (_tabText != value)
				{
					_tabText = value;
					ForceUpdate();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual ContentAlignment TextAlignment
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _textAlignment;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (_textAlignment != value)
				{
					_textAlignment = value;
					ForceUpdate();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;
					ForceUpdate();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual TextImageRelation TextImageRelation
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return _textImageRelation;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				if (_textImageRelation != value)
				{
					_textImageRelation = value;
					ForceUpdate();
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual int TabWidth
		{
			get
			{
				return _tabWidth;
			}
			set
			{
				if (_tabWidth != value)
				{
					_tabWidth = value;
					backgroundPath = null;
					if (_owner != null)
					{
						_owner.Relayout();
					}
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual int TabHeigth
		{
			get
			{
				return _tabHeigth;
			}
			set
			{
				if (_tabHeigth != value)
				{
					_tabHeigth = value;
					backgroundPath = null;
					if (_owner != null)
					{
						_owner.Relayout();
					}
				}
			}
		}

		[Category("Appearance")]
		[Browsable(true)]
		public virtual int SelectedTabHeigth
		{
			get
			{
				return _selectedTabHeigth;
			}
			set
			{
				if (_selectedTabHeigth != value)
				{
					_selectedTabHeigth = value;
					backgroundPath = null;
					if (_owner != null && _selected)
					{
						_owner.Relayout();
					}
				}
			}
		}

		[Browsable(false)]
		public int Width
		{
			get
			{
				return ((Control)this).get_Width();
			}
			set
			{
				((Control)this).set_Width(value);
			}
		}

		[Browsable(false)]
		public int Height
		{
			get
			{
				return ((Control)this).get_Height();
			}
			set
			{
				((Control)this).set_Height(value);
			}
		}

		[Category("Behavior")]
		[Browsable(true)]
		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Expected O, but got Unknown
				if (value == _selected)
				{
					return;
				}
				_selected = value;
				backgroundPath = null;
				if (_selected)
				{
					CancelEventArgs val = new CancelEventArgs();
					OnBeforeTabSelected(val);
					if (!val.get_Cancel())
					{
						UpdateSelectedState();
						if (_owner != null)
						{
							_owner.TabItemSelected(this);
						}
						OnTabSelected(EventArgs.Empty);
					}
				}
				else
				{
					UpdateSelectedState();
				}
			}
		}

		public bool AutoEllipsis
		{
			get
			{
				return _autoEllipsis;
			}
			set
			{
				if (value != _autoEllipsis)
				{
					_autoEllipsis = value;
					ForceUpdate();
				}
			}
		}

		public event CancelEventHandler BeforeTabSelected
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				CancelEventHandler val = this.BeforeTabSelected;
				CancelEventHandler val2;
				do
				{
					val2 = val;
					CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<CancelEventHandler, CancelEventHandler>(ref this.BeforeTabSelected), value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				CancelEventHandler val = this.BeforeTabSelected;
				CancelEventHandler val2;
				do
				{
					val2 = val;
					CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<CancelEventHandler, CancelEventHandler>(ref this.BeforeTabSelected), value2, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler TabSelected;

		public event CancelEventHandler BeforeCloseButtonPressed
		{
			[CompilerGenerated]
			add
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				CancelEventHandler val = this.BeforeCloseButtonPressed;
				CancelEventHandler val2;
				do
				{
					val2 = val;
					CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<CancelEventHandler, CancelEventHandler>(ref this.BeforeCloseButtonPressed), value2, val2);
				}
				while (val != val2);
			}
			[CompilerGenerated]
			remove
			{
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Expected O, but got Unknown
				CancelEventHandler val = this.BeforeCloseButtonPressed;
				CancelEventHandler val2;
				do
				{
					val2 = val;
					CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
					val = Interlocked.CompareExchange(ref System.Runtime.CompilerServices.Unsafe.As<CancelEventHandler, CancelEventHandler>(ref this.BeforeCloseButtonPressed), value2, val2);
				}
				while (val != val2);
			}
		}

		public event EventHandler CloseButtonPressed;

		private static ColorMatrix MultiplyColorMatrix(float[][] matrix1, float[][] matrix2)
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Expected O, but got Unknown
			int num = 5;
			float[][] array = new float[num][];
			for (int i = 0; i < num; i++)
			{
				array[i] = new float[num];
			}
			float[] array2 = new float[num];
			for (int j = 0; j < num; j++)
			{
				for (int k = 0; k < num; k++)
				{
					array2[k] = matrix1[k][j];
				}
				for (int l = 0; l < num; l++)
				{
					float[] array3 = matrix2[l];
					float num2 = 0f;
					for (int m = 0; m < num; m++)
					{
						num2 += array3[m] * array2[m];
					}
					array[l][j] = num2;
				}
			}
			return new ColorMatrix(array);
		}

		public TabItem()
			: this()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			InitializeComponent();
			TabWidth = 100;
			((Control)this).set_BackColor(Color.get_SlateGray());
			((Control)this).set_ForeColor(Color.get_White());
			HighlightBackColor = Color.get_Gold();
			HighlightForeColor = Color.get_Black();
			SelectedBackColor = Color.get_DeepPink();
			SelectedForeColor = Color.get_White();
			TextAlignment = (ContentAlignment)16;
			TextImageRelation = (TextImageRelation)4;
			TabText = ((Control)this).get_Name();
			((Control)this).SetStyle((ControlStyles)73730, true);
			((Control)this).UpdateStyles();
		}

		public TabItem(string name)
			: this()
		{
			((Control)this).set_Name(name);
			((Control)this).set_Text(name);
		}

		protected void OnBeforeTabSelected(CancelEventArgs e)
		{
			CancelEventHandler beforeTabSelected = this.BeforeTabSelected;
			if (beforeTabSelected != null)
			{
				beforeTabSelected.Invoke((object)this, e);
			}
		}

		protected void OnTabSelected(EventArgs e)
		{
			this.TabSelected?.Invoke(this, e);
		}

		protected void OnBeforeCloseButtonPressed(CancelEventArgs e)
		{
			CancelEventHandler beforeCloseButtonPressed = this.BeforeCloseButtonPressed;
			if (beforeCloseButtonPressed != null)
			{
				beforeCloseButtonPressed.Invoke((object)this, e);
			}
		}

		protected void OnCloseButtonPressed(EventArgs e)
		{
			this.CloseButtonPressed?.Invoke(this, e);
		}

		protected override void OnBackColorChanged(EventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			backBrush = (Brush)new SolidBrush(((Control)this).get_BackColor());
			UpdateSelectedState();
		}

		protected override void OnForeColorChanged(EventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			foreBrush = (Brush)new SolidBrush(((Control)this).get_ForeColor());
			UpdateSelectedState();
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			ForceUpdate();
			((Control)this).OnEnabledChanged(e);
		}

		protected override void OnPaddingChanged(EventArgs e)
		{
			ForceUpdate();
			((ScrollableControl)this).OnPaddingChanged(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Invalid comparison between Unknown and I4
			if (_owner.ShowCloseButtons && closeButtonState == CloseButtonState.Over)
			{
				EnterPressedCloseButtonState();
			}
			else
			{
				if (_owner.AllowTabReordering)
				{
					lastMouseDownLocation = e.get_Location();
				}
				if ((int)e.get_Button() != 2097152 || ((Control)this).get_ContextMenuStrip() == null)
				{
					Selected = true;
				}
			}
			((UserControl)this).OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			if (_owner.ShowCloseButtons && closeButtonState == CloseButtonState.Pressed)
			{
				CancelEventArgs val = new CancelEventArgs();
				OnBeforeCloseButtonPressed(val);
				if (!val.get_Cancel())
				{
					OnCloseButtonPressed(EventArgs.Empty);
				}
				ExitPressedCloseButtonState();
			}
			if (reordering)
			{
				((Control)this).set_Capture(false);
				reordering = false;
				_owner.TabEndDrag(this, e.get_Location());
			}
			((Control)this).OnMouseUp(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			if (CanReorder(e.get_Button()))
			{
				if (reordering)
				{
					_owner.TabUpdateDrag(this, e.get_Location());
				}
				else
				{
					Point location = e.get_Location();
					int num = Math.Abs(((Point)(ref location)).get_X() - ((Point)(ref lastMouseDownLocation)).get_X());
					Size dragSize = SystemInformation.get_DragSize();
					int num3;
					if (num < ((Size)(ref dragSize)).get_Width())
					{
						location = e.get_Location();
						int num2 = Math.Abs(((Point)(ref location)).get_Y() - ((Point)(ref lastMouseDownLocation)).get_Y());
						dragSize = SystemInformation.get_DragSize();
						num3 = ((num2 >= ((Size)(ref dragSize)).get_Height()) ? 1 : 0);
					}
					else
					{
						num3 = 1;
					}
					if (num3 != 0)
					{
						reordering = true;
						((Control)this).set_Capture(true);
						_owner.TabStartDrag(this, e.get_Location());
					}
				}
			}
			else if (_owner.ShowCloseButtons)
			{
				if (((Rectangle)(ref closeButtonLocation)).Contains(e.get_X(), e.get_Y()))
				{
					EnterOverCloseButtonState();
				}
				else
				{
					ExitOverCloseButtonState();
				}
			}
			((Control)this).OnMouseMove(e);
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			EnterHighlightState();
			((Control)this).OnMouseEnter(e);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			ExitHighlightState();
			EnterNormalCloseButtonState();
			((Control)this).OnMouseLeave(e);
		}

		private void EnterHighlightState()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Expected O, but got Unknown
			if (!_selected)
			{
				currentBackBrush = (Brush)new SolidBrush(_highlightBackColor);
				currentForeBrush = (Brush)new SolidBrush(_highlightForeColor);
				ForceUpdate();
			}
		}

		private void ExitHighlightState()
		{
			UpdateSelectedState();
		}

		private void EnterOverCloseButtonState()
		{
			if (closeButtonState == CloseButtonState.Normal)
			{
				closeButtonState = CloseButtonState.Over;
				ForceUpdate();
			}
		}

		private void ExitOverCloseButtonState()
		{
			if (closeButtonState == CloseButtonState.Over)
			{
				closeButtonState = CloseButtonState.Normal;
				ForceUpdate();
			}
		}

		private void EnterPressedCloseButtonState()
		{
			if (closeButtonState == CloseButtonState.Over)
			{
				closeButtonState = CloseButtonState.Pressed;
				ForceUpdate();
			}
		}

		private void ExitPressedCloseButtonState()
		{
			if (closeButtonState == CloseButtonState.Pressed)
			{
				closeButtonState = CloseButtonState.Normal;
				ForceUpdate();
			}
		}

		private void EnterNormalCloseButtonState()
		{
			if (_owner.ShowCloseButtons && closeButtonState != 0)
			{
				closeButtonState = CloseButtonState.Normal;
				ForceUpdate();
			}
		}

		private void UpdateSelectedState()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (_selected)
			{
				currentBackBrush = (Brush)new SolidBrush(_selectedBackColor);
				currentForeBrush = (Brush)new SolidBrush(_selectedForeColor);
			}
			else
			{
				currentBackBrush = backBrush;
				currentForeBrush = foreBrush;
			}
			ForceUpdate();
		}

		private void ForceUpdate()
		{
			((Control)this).Invalidate();
		}

		private bool CanReorder(MouseButtons button)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return _owner.AllowTabReordering && (int)button != 0 && closeButtonState == CloseButtonState.Normal;
		}

		private Color MakeTransparentColor(Color baseColor, int transparentValue)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			return Color.FromArgb(transparentValue, (int)((Color)(ref baseColor)).get_R(), (int)((Color)(ref baseColor)).get_G(), (int)((Color)(ref baseColor)).get_B());
		}

		private void DrawCloseButtonInternal(int x, int y, Pen linePen, Brush backBrush, Graphics g)
		{
			g.FillRectangle(backBrush, x, y, 12, 12);
			g.DrawRectangle(linePen, x, y, 12, 12);
			g.DrawLine(linePen, x + 3, y + 3, x + 12 - 3, y + 12 - 3);
			g.DrawLine(linePen, x + 12 - 3, y + 3, x + 3, y + 12 - 3);
		}

		protected Rectangle ComputeCloseButtonBounds(ref Rectangle availableSpace)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Invalid comparison between Unknown and I4
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Invalid comparison between Unknown and I4
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Invalid comparison between Unknown and I4
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			if (!_owner.ShowCloseButtons || (!_selected && _owner.ShowCloseButtons && _owner.CloseButtonsOnlyForSelected))
			{
				return availableSpace;
			}
			Rectangle empty = Rectangle.Empty;
			if ((int)_owner.TabAlignment == 0 || (int)_owner.TabAlignment == 1)
			{
				if (_owner.CloseButtonAlignment == CloseButtonAlignment.Left)
				{
					((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), 24, ((Rectangle)(ref availableSpace)).get_Height());
					availableSpace = new Rectangle(24, ((Rectangle)(ref availableSpace)).get_Top(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Width() - 12 - 12), ((Rectangle)(ref availableSpace)).get_Height());
				}
				else
				{
					((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left() + ((Rectangle)(ref availableSpace)).get_Width() - 12 - 12, ((Rectangle)(ref availableSpace)).get_Top(), 24, ((Rectangle)(ref availableSpace)).get_Height());
					availableSpace = new Rectangle(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Width() - 12 - 12), ((Rectangle)(ref availableSpace)).get_Height());
				}
			}
			else if ((int)_owner.TabAlignment == 2 || (int)_owner.TabAlignment == 3)
			{
				if (_owner.CloseButtonAlignment == CloseButtonAlignment.Left)
				{
					((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), ((Rectangle)(ref availableSpace)).get_Width(), 24);
					availableSpace = new Rectangle(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top() + 12 + 12, ((Rectangle)(ref availableSpace)).get_Width(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Height() - 12 - 12));
				}
				else
				{
					((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top() + ((Rectangle)(ref availableSpace)).get_Height() - 12 - 12, ((Rectangle)(ref availableSpace)).get_Width(), 24);
					availableSpace = new Rectangle(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), ((Rectangle)(ref availableSpace)).get_Width(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Height() - 12 - 12));
				}
			}
			return empty;
		}

		protected Rectangle ComputeImageBounds(ref Rectangle availableSpace)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Invalid comparison between Unknown and I4
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Invalid comparison between Unknown and I4
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Invalid comparison between Unknown and I4
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Invalid comparison between Unknown and I4
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Invalid comparison between Unknown and I4
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Invalid comparison between Unknown and I4
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Invalid comparison between Unknown and I4
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			Rectangle empty = Rectangle.Empty;
			if (_image == null)
			{
				return empty;
			}
			int width = _image.get_Width();
			int height = _image.get_Height();
			if ((int)_owner.TabAlignment == 0 || (int)_owner.TabAlignment == 1)
			{
				TextImageRelation textImageRelation = _textImageRelation;
				TextImageRelation val = textImageRelation;
				if ((int)val != 4)
				{
					if ((int)val == 8)
					{
						((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left() + ((Rectangle)(ref availableSpace)).get_Width() - width, ((Rectangle)(ref availableSpace)).get_Top(), width, ((Rectangle)(ref availableSpace)).get_Height());
						availableSpace = new Rectangle(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Width() - width), ((Rectangle)(ref availableSpace)).get_Height());
					}
				}
				else
				{
					((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), width, ((Rectangle)(ref availableSpace)).get_Height());
					availableSpace = new Rectangle(((Rectangle)(ref availableSpace)).get_Left() + width, ((Rectangle)(ref availableSpace)).get_Top(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Width() - width), ((Rectangle)(ref availableSpace)).get_Height());
				}
			}
			else if ((int)_owner.TabAlignment == 2 || (int)_owner.TabAlignment == 3)
			{
				TextImageRelation textImageRelation2 = _textImageRelation;
				TextImageRelation val2 = textImageRelation2;
				if ((int)val2 != 4)
				{
					if ((int)val2 == 8)
					{
						((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top() + ((Rectangle)(ref availableSpace)).get_Height() - height, ((Rectangle)(ref availableSpace)).get_Width(), height);
						availableSpace = new Rectangle(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), ((Rectangle)(ref availableSpace)).get_Width(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Height() - height));
					}
				}
				else
				{
					((Rectangle)(ref empty))._002Ector(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top(), ((Rectangle)(ref availableSpace)).get_Width(), height);
					availableSpace = new Rectangle(((Rectangle)(ref availableSpace)).get_Left(), ((Rectangle)(ref availableSpace)).get_Top() + height, ((Rectangle)(ref availableSpace)).get_Width(), Math.Max(1, ((Rectangle)(ref availableSpace)).get_Height() - height));
				}
			}
			return empty;
		}

		protected Rectangle ComputeTextBounds(ref Rectangle availableSpace)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			return availableSpace;
		}

		protected void DrawCloseButton(Rectangle bounds, CloseButtonState state, Graphics g)
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Invalid comparison between Unknown and I4
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Expected O, but got Unknown
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Expected O, but got Unknown
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Expected O, but got Unknown
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0218: Expected O, but got Unknown
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Expected O, but got Unknown
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Expected O, but got Unknown
			if (_owner.ShowCloseButtons && (_selected || !_owner.ShowCloseButtons || !_owner.CloseButtonsOnlyForSelected) && currentBackBrush != null && currentForeBrush != null)
			{
				int num;
				int num2;
				if ((int)_owner.TabAlignment == 0 || (int)_owner.TabAlignment == 1)
				{
					num = ((_owner.CloseButtonAlignment != 0) ? (((Rectangle)(ref bounds)).get_Left() + ((Rectangle)(ref bounds)).get_Width() - 12 - 6) : ((Rectangle)(ref bounds)).get_Left());
					num2 = ((Rectangle)(ref bounds)).get_Top() + ((Rectangle)(ref bounds)).get_Height() / 2 - 6;
				}
				else
				{
					num2 = ((_owner.CloseButtonAlignment != 0) ? (((Rectangle)(ref bounds)).get_Top() + ((Rectangle)(ref bounds)).get_Height() - 12 - 6) : ((Rectangle)(ref bounds)).get_Top());
					num = ((Rectangle)(ref bounds)).get_Left() + ((Rectangle)(ref bounds)).get_Width() / 2 - 6 - 1;
				}
				if (state == CloseButtonState.Normal)
				{
					closeButtonLocation = new Rectangle(num, num2, 12, 12);
				}
				switch (state)
				{
				case CloseButtonState.Normal:
				{
					Brush val2 = (((Control)this).get_Enabled() ? currentForeBrush : Brushes.get_Silver());
					Brush val3 = (Brush)new SolidBrush(_selected ? _owner.CloseButtonColorSelected : _owner.CloseButtonColor);
					int transparentValue = (_selected ? _owner.CloseButtonBorderOpacitySelected : _owner.CloseButtonBorderOpacity);
					Pen linePen3 = new Pen(MakeTransparentColor(((SolidBrush)val2).get_Color(), transparentValue));
					DrawCloseButtonInternal(num, num2, linePen3, val3, g);
					break;
				}
				case CloseButtonState.Over:
				{
					Pen linePen2 = new Pen(((SolidBrush)currentForeBrush).get_Color());
					Brush val = (Brush)new SolidBrush(_selected ? _owner.CloseButtonOverColorSelected : _owner.CloseButtonOverColor);
					DrawCloseButtonInternal(num, num2, linePen2, val, g);
					break;
				}
				case CloseButtonState.Pressed:
				{
					Pen linePen = new Pen(((SolidBrush)currentForeBrush).get_Color());
					DrawCloseButtonInternal(num, num2, linePen, (Brush)new SolidBrush(_owner.CloseButtonPressedColor), g);
					break;
				}
				}
			}
		}

		private int GetRealWidth(Rectangle rect)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Invalid comparison between Unknown and I4
			if (_owner != null)
			{
				if ((int)_owner.TabAlignment == 0 || (int)_owner.TabAlignment == 1)
				{
					return ((Rectangle)(ref rect)).get_Width();
				}
				return ((Rectangle)(ref rect)).get_Height();
			}
			return ((Rectangle)(ref rect)).get_Width();
		}

		private int GetReaHeight(Rectangle rect)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Invalid comparison between Unknown and I4
			if (_owner != null)
			{
				if ((int)_owner.TabAlignment == 0 || (int)_owner.TabAlignment == 1)
				{
					return ((Rectangle)(ref rect)).get_Height();
				}
				return ((Rectangle)(ref rect)).get_Width();
			}
			return ((Rectangle)(ref rect)).get_Height();
		}

		protected void DrawText(Rectangle bounds, Graphics g)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Invalid comparison between Unknown and I4
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected I4, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Invalid comparison between Unknown and I4
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Invalid comparison between Unknown and I4
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Invalid comparison between Unknown and I4
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Invalid comparison between Unknown and I4
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Invalid comparison between Unknown and I4
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Invalid comparison between Unknown and I4
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Invalid comparison between Unknown and I4
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Invalid comparison between Unknown and I4
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Expected O, but got Unknown
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Invalid comparison between Unknown and I4
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Invalid comparison between Unknown and I4
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Expected O, but got Unknown
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Unknown result type (might be due to invalid IL or missing references)
			//IL_036d: Unknown result type (might be due to invalid IL or missing references)
			//IL_037b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Invalid comparison between Unknown and I4
			if (currentForeBrush == null || ((Control)this).get_Text() == null)
			{
				return;
			}
			SizeF val = g.MeasureString(_tabText, ((Control)this).get_Font());
			float num = ((Rectangle)(ref bounds)).get_Left();
			float num2 = ((Rectangle)(ref bounds)).get_Top();
			float num3 = GetRealWidth(bounds);
			float num4 = GetReaHeight(bounds);
			ContentAlignment textAlignment = _textAlignment;
			ContentAlignment val2 = textAlignment;
			if ((int)val2 <= 32)
			{
				switch (val2 - 1)
				{
				default:
					if ((int)val2 != 16)
					{
						if ((int)val2 == 32)
						{
							num += num3 / 2f - ((SizeF)(ref val)).get_Width() / 2f;
							num2 += num4 / 2f - ((SizeF)(ref val)).get_Height() / 2f;
						}
					}
					else
					{
						num += 0f;
						num2 += num4 / 2f - ((SizeF)(ref val)).get_Height() / 2f;
					}
					break;
				case 1:
					num += num3 / 2f - ((SizeF)(ref val)).get_Width() / 2f;
					num2 += 0f;
					break;
				case 0:
					num += 0f;
					num2 += 0f;
					break;
				case 3:
					num += num3 - ((SizeF)(ref val)).get_Width();
					num2 += 0f;
					break;
				case 2:
					break;
				}
			}
			else if ((int)val2 <= 256)
			{
				if ((int)val2 != 64)
				{
					if ((int)val2 == 256)
					{
						num += 0f;
						num2 += num4 - ((SizeF)(ref val)).get_Height();
					}
				}
				else
				{
					num += num3 - ((SizeF)(ref val)).get_Width();
					num2 += num4 / 2f - ((SizeF)(ref val)).get_Height() / 2f;
				}
			}
			else if ((int)val2 != 512)
			{
				if ((int)val2 == 1024)
				{
					num += num3 - ((SizeF)(ref val)).get_Width();
					num2 += num4 - ((SizeF)(ref val)).get_Height();
				}
			}
			else
			{
				num += num3 / 2f - ((SizeF)(ref val)).get_Width() / 2f;
				num2 += num4 - ((SizeF)(ref val)).get_Height();
			}
			Brush silver = currentForeBrush;
			if (!((Control)this).get_Enabled())
			{
				silver = Brushes.get_Silver();
			}
			if ((int)_owner.TabAlignment == 0 || (int)_owner.TabAlignment == 1)
			{
				if (_autoEllipsis)
				{
					StringFormat val3 = new StringFormat();
					val3.set_Trimming((StringTrimming)3);
					RectangleF val4 = default(RectangleF);
					((RectangleF)(ref val4))._002Ector(num, num2, (float)((Rectangle)(ref bounds)).get_Width(), ((SizeF)(ref val)).get_Height());
					g.DrawString(_tabText, ((Control)this).get_Font(), silver, val4, val3);
				}
				else
				{
					g.DrawString(_tabText, ((Control)this).get_Font(), silver, num, num2);
				}
			}
			else
			{
				if ((int)_owner.TabAlignment != 2 && (int)_owner.TabAlignment != 3)
				{
					return;
				}
				Bitmap val5 = new Bitmap((int)num3, (int)num4, (PixelFormat)2498570);
				try
				{
					Graphics val6 = Graphics.FromImage((Image)(object)val5);
					try
					{
						val6.set_TextRenderingHint((TextRenderingHint)4);
						TextFormatFlags val7 = (TextFormatFlags)0;
						Color color = ((SolidBrush)currentForeBrush).get_Color();
						if (_autoEllipsis)
						{
							val7 = (TextFormatFlags)32768;
						}
						Rectangle val8 = default(Rectangle);
						((Rectangle)(ref val8))._002Ector((int)num - ((Rectangle)(ref bounds)).get_Left(), (int)num2 - ((Rectangle)(ref bounds)).get_Top(), (int)num3, (int)num4);
						TextRenderer.DrawText((IDeviceContext)(object)val6, _tabText, ((Control)this).get_Font(), bounds, color, val7);
						if ((int)_owner.TabAlignment == 2)
						{
							((Image)val5).RotateFlip((RotateFlipType)1);
							g.DrawImage((Image)(object)val5, ((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), ((Image)val5).get_Width(), ((Image)val5).get_Height());
						}
						else
						{
							((Image)val5).RotateFlip((RotateFlipType)3);
							g.DrawImage((Image)(object)val5, ((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top(), ((Image)val5).get_Width(), ((Image)val5).get_Height());
						}
					}
					finally
					{
						((IDisposable)val6)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val5)?.Dispose();
				}
			}
		}

		protected void DrawImage(Rectangle bounds, Graphics g)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			if (_image == null)
			{
				return;
			}
			Image val = _image;
			if (!((Control)this).get_Enabled())
			{
				Bitmap val2 = new Bitmap(_image);
				Graphics val3 = Graphics.FromImage((Image)(object)val2);
				try
				{
					ImageAttributes val4 = new ImageAttributes();
					val4.SetColorMatrix(DisabledImageMatrix);
					val3.DrawImage(_image, new Rectangle(0, 0, _image.get_Width(), _image.get_Height()), 0, 0, _image.get_Width(), _image.get_Height(), (GraphicsUnit)2, val4);
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
				val = (Image)(object)val2;
			}
			int width = _image.get_Width();
			int height = _image.get_Height();
			if (height > ((Rectangle)(ref bounds)).get_Height())
			{
				width = (int)((double)width * ((double)height / (double)((Rectangle)(ref bounds)).get_Height()));
				height = ((Rectangle)(ref bounds)).get_Height();
				int num = ((Rectangle)(ref bounds)).get_Left() + ((Rectangle)(ref bounds)).get_Width() / 2 - width / 2;
				int num2 = ((Rectangle)(ref bounds)).get_Top() + ((Rectangle)(ref bounds)).get_Height() / 2 - height / 2;
				g.DrawImage(val, num, num2, width, height);
			}
			else
			{
				int num = ((Rectangle)(ref bounds)).get_Left() + ((Rectangle)(ref bounds)).get_Width() / 2 - width / 2;
				int num2 = ((Rectangle)(ref bounds)).get_Top() + ((Rectangle)(ref bounds)).get_Height() / 2 - height / 2;
				g.DrawImageUnscaled(val, num, num2);
			}
			if (!((Control)this).get_Enabled())
			{
				val.Dispose();
			}
		}

		protected void DrawTabBackground(Rectangle bounds, Brush brush, Graphics g)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Expected I4, but got Unknown
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Expected O, but got Unknown
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Expected O, but got Unknown
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Expected O, but got Unknown
			GraphicsPath val = backgroundPath;
			int left = ((Rectangle)(ref bounds)).get_Left();
			int top = ((Rectangle)(ref bounds)).get_Top();
			int realWidth = GetRealWidth(bounds);
			int reaHeight = GetReaHeight(bounds);
			if (val == null)
			{
				val = new GraphicsPath();
				val.AddLine(left, top + 5, left, top + reaHeight);
				val.AddBezier(left, top + 5, left + 1, top + 1, left + 1, top + 1, left + 5, top);
				val.AddLine(left + 5, top, left + realWidth - 5, top);
				val.AddBezier(left + realWidth - 5, top, left + realWidth - 1, top + 1, left + realWidth - 1, top + 1, left + realWidth, top + 5);
				val.AddLine(left + realWidth, top + 5, left + realWidth, top + reaHeight);
				val.AddLine(left + realWidth, top + reaHeight, left, top + reaHeight);
			}
			TabAlignment tabAlignment = _owner.TabAlignment;
			TabAlignment val2 = tabAlignment;
			switch ((int)val2)
			{
			case 0:
				g.FillPath(brush, val);
				if (borderPen != null)
				{
					g.DrawPath(borderPen, val);
					g.DrawLine(new Pen(brush), left + 1, top + reaHeight, left + realWidth - 1, top + reaHeight);
				}
				break;
			case 1:
				g.TranslateTransform((float)((Rectangle)(ref bounds)).get_Width(), (float)((Rectangle)(ref bounds)).get_Height());
				g.RotateTransform(180f);
				g.FillPath(brush, val);
				if (borderPen != null)
				{
					g.DrawPath(borderPen, val);
					g.DrawLine(new Pen(brush), left + 1, top + reaHeight, left + realWidth - 1, top + reaHeight);
				}
				g.ResetTransform();
				break;
			case 2:
				g.TranslateTransform((float)((Rectangle)(ref bounds)).get_Width(), 0f);
				g.RotateTransform(90f);
				g.FillPath(brush, val);
				if (borderPen != null)
				{
					g.DrawPath(borderPen, val);
					g.DrawLine(new Pen(brush), left + 1, top + reaHeight, left + realWidth - 1, top + reaHeight);
				}
				g.ResetTransform();
				break;
			case 3:
				g.TranslateTransform(0f, (float)((Rectangle)(ref bounds)).get_Height());
				g.RotateTransform(-90f);
				g.FillPath(brush, val);
				if (borderPen != null)
				{
					g.DrawPath(borderPen, val);
				}
				g.ResetTransform();
				break;
			}
		}

		protected Rectangle ComputeTabBackgroundBounds()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			_ = _borderColor;
			Padding padding;
			if (_borderColor != Color.Empty)
			{
				padding = ((Control)this).get_Padding();
				int left = ((Padding)(ref padding)).get_Left();
				padding = ((Control)this).get_Padding();
				int top = ((Padding)(ref padding)).get_Top();
				int width = Width;
				padding = ((Control)this).get_Padding();
				int num = width - ((Padding)(ref padding)).get_Right();
				padding = ((Control)this).get_Padding();
				int num2 = num - ((Padding)(ref padding)).get_Left() - 1;
				int height = Height;
				padding = ((Control)this).get_Padding();
				int num3 = height - ((Padding)(ref padding)).get_Bottom();
				padding = ((Control)this).get_Padding();
				return new Rectangle(left, top, num2, num3 - ((Padding)(ref padding)).get_Top() - 1);
			}
			padding = ((Control)this).get_Padding();
			int left2 = ((Padding)(ref padding)).get_Left();
			padding = ((Control)this).get_Padding();
			int top2 = ((Padding)(ref padding)).get_Top();
			int width2 = Width;
			padding = ((Control)this).get_Padding();
			int num4 = width2 - ((Padding)(ref padding)).get_Right();
			padding = ((Control)this).get_Padding();
			int num5 = num4 - ((Padding)(ref padding)).get_Left();
			int height2 = Height;
			padding = ((Control)this).get_Padding();
			int num6 = height2 - ((Padding)(ref padding)).get_Bottom();
			padding = ((Control)this).get_Padding();
			return new Rectangle(left2, top2, num5, num6 - ((Padding)(ref padding)).get_Top());
		}

		protected Rectangle ComputeElementSpace(Rectangle bounds)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Invalid comparison between Unknown and I4
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Invalid comparison between Unknown and I4
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			Rectangle empty = Rectangle.Empty;
			if ((int)_owner.TabAlignment == 0 || (int)_owner.TabAlignment == 1)
			{
				((Rectangle)(ref empty))._002Ector(((Rectangle)(ref bounds)).get_Left() + 5, ((Rectangle)(ref bounds)).get_Top(), ((Rectangle)(ref bounds)).get_Width() - 10, ((Rectangle)(ref bounds)).get_Height());
			}
			else if ((int)_owner.TabAlignment == 2 || (int)_owner.TabAlignment == 3)
			{
				((Rectangle)(ref empty))._002Ector(((Rectangle)(ref bounds)).get_Left(), ((Rectangle)(ref bounds)).get_Top() + 5, ((Rectangle)(ref bounds)).get_Width(), ((Rectangle)(ref bounds)).get_Height() - 10);
			}
			return empty;
		}

		private void TabItem_Paint(object sender, PaintEventArgs e)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			if (_owner != null)
			{
				e.get_Graphics().FillRectangle((Brush)new SolidBrush(((Control)_owner).get_BackColor()), 0, 0, Width, Height);
				Rectangle bounds = ComputeTabBackgroundBounds();
				DrawTabBackground(bounds, currentBackBrush, e.get_Graphics());
				Rectangle availableSpace = ComputeElementSpace(bounds);
				Rectangle bounds2 = ComputeCloseButtonBounds(ref availableSpace);
				DrawCloseButton(bounds2, closeButtonState, e.get_Graphics());
				Rectangle bounds3 = ComputeImageBounds(ref availableSpace);
				DrawImage(bounds3, e.get_Graphics());
				Rectangle bounds4 = ComputeTextBounds(ref availableSpace);
				DrawText(bounds4, e.get_Graphics());
			}
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
			((Control)this).set_Name("TabItem");
			((Control)this).add_Paint(new PaintEventHandler(TabItem_Paint));
			((Control)this).ResumeLayout(false);
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal class VS2005DockPaneCaption : DockPaneCaptionBase
	{
		private sealed class InertButton : InertButtonBase
		{
			private Bitmap m_image;

			private Bitmap m_imageAutoHide;

			private VS2005DockPaneCaption m_dockPaneCaption;

			private VS2005DockPaneCaption DockPaneCaption => m_dockPaneCaption;

			public bool IsAutoHide => DockPaneCaption.DockPane.IsAutoHide;

			public override Bitmap Image => IsAutoHide ? m_imageAutoHide : m_image;

			public InertButton(VS2005DockPaneCaption dockPaneCaption, Bitmap image, Bitmap imageAutoHide)
			{
				m_dockPaneCaption = dockPaneCaption;
				m_image = image;
				m_imageAutoHide = imageAutoHide;
				RefreshChanges();
			}

			protected override void OnRefreshChanges()
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				if (DockPaneCaption.TextColor != ((Control)this).get_ForeColor())
				{
					((Control)this).set_ForeColor(DockPaneCaption.TextColor);
					((Control)this).Invalidate();
				}
			}
		}

		private const int _TextGapTop = 2;

		private const int _TextGapBottom = 0;

		private const int _TextGapLeft = 3;

		private const int _TextGapRight = 3;

		private const int _ButtonGapTop = 2;

		private const int _ButtonGapBottom = 1;

		private const int _ButtonGapBetween = 1;

		private const int _ButtonGapLeft = 1;

		private const int _ButtonGapRight = 2;

		private static Bitmap _imageButtonClose;

		private InertButton m_buttonClose;

		private static Bitmap _imageButtonAutoHide;

		private static Bitmap _imageButtonDock;

		private InertButton m_buttonAutoHide;

		private static Bitmap _imageButtonOptions;

		private InertButton m_buttonOptions;

		private IContainer m_components;

		private ToolTip m_toolTip;

		private static string _toolTipClose;

		private static string _toolTipOptions;

		private static string _toolTipAutoHide;

		private static Blend _activeBackColorGradientBlend;

		private static TextFormatFlags _textFormat = (TextFormatFlags)32804;

		private static Bitmap ImageButtonClose => _imageButtonClose;

		private InertButton ButtonClose
		{
			get
			{
				if (m_buttonClose == null)
				{
					m_buttonClose = new InertButton(this, ImageButtonClose, ImageButtonClose);
					m_toolTip.SetToolTip((Control)(object)m_buttonClose, ToolTipClose);
					((Control)m_buttonClose).add_Click((EventHandler)Close_Click);
					((Control)this).get_Controls().Add((Control)(object)m_buttonClose);
				}
				return m_buttonClose;
			}
		}

		private static Bitmap ImageButtonAutoHide => _imageButtonAutoHide;

		private static Bitmap ImageButtonDock => _imageButtonDock;

		private InertButton ButtonAutoHide
		{
			get
			{
				if (m_buttonAutoHide == null)
				{
					m_buttonAutoHide = new InertButton(this, ImageButtonDock, ImageButtonAutoHide);
					m_toolTip.SetToolTip((Control)(object)m_buttonAutoHide, ToolTipAutoHide);
					((Control)m_buttonAutoHide).add_Click((EventHandler)AutoHide_Click);
					((Control)this).get_Controls().Add((Control)(object)m_buttonAutoHide);
				}
				return m_buttonAutoHide;
			}
		}

		private static Bitmap ImageButtonOptions => _imageButtonOptions;

		private InertButton ButtonOptions
		{
			get
			{
				if (m_buttonOptions == null)
				{
					m_buttonOptions = new InertButton(this, ImageButtonOptions, ImageButtonOptions);
					m_toolTip.SetToolTip((Control)(object)m_buttonOptions, ToolTipOptions);
					((Control)m_buttonOptions).add_Click((EventHandler)Options_Click);
					((Control)this).get_Controls().Add((Control)(object)m_buttonOptions);
				}
				return m_buttonOptions;
			}
		}

		private IContainer Components => m_components;

		private static int TextGapTop => 2;

		private static int TextGapBottom => 0;

		private static int TextGapLeft => 3;

		private static int TextGapRight => 3;

		private static int ButtonGapTop => 2;

		private static int ButtonGapBottom => 1;

		private static int ButtonGapLeft => 1;

		private static int ButtonGapRight => 2;

		private static int ButtonGapBetween => 1;

		private static string ToolTipClose
		{
			get
			{
				if (_toolTipClose == null)
				{
					_toolTipClose = Strings.DockPaneCaption_ToolTipClose;
				}
				return _toolTipClose;
			}
		}

		private static string ToolTipOptions
		{
			get
			{
				if (_toolTipOptions == null)
				{
					_toolTipOptions = Strings.DockPaneCaption_ToolTipOptions;
				}
				return _toolTipOptions;
			}
		}

		private static string ToolTipAutoHide
		{
			get
			{
				if (_toolTipAutoHide == null)
				{
					_toolTipAutoHide = Strings.DockPaneCaption_ToolTipAutoHide;
				}
				return _toolTipAutoHide;
			}
		}

		private static Blend ActiveBackColorGradientBlend
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Expected O, but got Unknown
				if (_activeBackColorGradientBlend == null)
				{
					Blend val = new Blend(2);
					val.set_Factors(new float[2]
					{
						0.5f,
						1f
					});
					val.set_Positions(new float[2]
					{
						0f,
						1f
					});
					_activeBackColorGradientBlend = val;
				}
				return _activeBackColorGradientBlend;
			}
		}

		private static Color ActiveBackColorGradientBegin => SystemColors.get_GradientActiveCaption();

		private static Color ActiveBackColorGradientEnd => SystemColors.get_ActiveCaption();

		private static Color InactiveBackColor
		{
			get
			{
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				string colorScheme = VisualStyleInformation.get_ColorScheme();
				if (colorScheme == "HomeStead" || colorScheme == "Metallic")
				{
					return SystemColors.get_GradientInactiveCaption();
				}
				return SystemColors.get_GrayText();
			}
		}

		private static Color ActiveTextColor => SystemColors.get_ActiveCaptionText();

		private static Color InactiveTextColor => SystemColors.get_ControlText();

		private Color TextColor => base.DockPane.IsActivated ? ActiveTextColor : InactiveTextColor;

		private TextFormatFlags TextFormat
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Invalid comparison between Unknown and I4
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				if ((int)((Control)this).get_RightToLeft() == 0)
				{
					return _textFormat;
				}
				return (TextFormatFlags)(_textFormat | 0x20000 | 2);
			}
		}

		private bool CloseButtonEnabled => base.DockPane.ActiveContent != null && base.DockPane.ActiveContent.DockHandler.CloseButton;

		private bool ShouldShowAutoHideButton => !base.DockPane.IsFloat;

		public VS2005DockPaneCaption(DockPane pane)
			: base(pane)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			((Control)this).SuspendLayout();
			((Control)this).set_Font(SystemInformation.get_MenuFont());
			m_components = (IContainer)new Container();
			m_toolTip = new ToolTip(Components);
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

		protected internal override int MeasureHeight()
		{
			int num = ((Control)this).get_Font().get_Height() + TextGapTop + TextGapBottom;
			if (num < ((Image)ButtonClose.Image).get_Height() + ButtonGapTop + ButtonGapBottom)
			{
				num = ((Image)ButtonClose.Image).get_Height() + ButtonGapTop + ButtonGapBottom;
			}
			return num;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			((Control)this).OnPaint(e);
			DrawCaption(e.get_Graphics());
		}

		private void DrawCaption(Graphics g)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
			if (((Rectangle)(ref clientRectangle)).get_IsEmpty())
			{
				return;
			}
			if (base.DockPane.IsActivated)
			{
				LinearGradientBrush val = new LinearGradientBrush(((Control)this).get_ClientRectangle(), ActiveBackColorGradientBegin, ActiveBackColorGradientEnd, (LinearGradientMode)1);
				try
				{
					val.set_Blend(ActiveBackColorGradientBlend);
					g.FillRectangle((Brush)(object)val, ((Control)this).get_ClientRectangle());
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			else
			{
				SolidBrush val2 = new SolidBrush(InactiveBackColor);
				try
				{
					g.FillRectangle((Brush)(object)val2, ((Control)this).get_ClientRectangle());
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			Rectangle clientRectangle2 = ((Control)this).get_ClientRectangle();
			Rectangle rectangle = clientRectangle2;
			((Rectangle)(ref rectangle)).set_X(((Rectangle)(ref rectangle)).get_X() + TextGapLeft);
			((Rectangle)(ref rectangle)).set_Width(((Rectangle)(ref rectangle)).get_Width() - (TextGapLeft + TextGapRight));
			((Rectangle)(ref rectangle)).set_Width(((Rectangle)(ref rectangle)).get_Width() - (ButtonGapLeft + ((Control)ButtonClose).get_Width() + ButtonGapRight));
			if (ShouldShowAutoHideButton)
			{
				((Rectangle)(ref rectangle)).set_Width(((Rectangle)(ref rectangle)).get_Width() - (((Control)ButtonAutoHide).get_Width() + ButtonGapBetween));
			}
			if (base.HasTabPageContextMenu)
			{
				((Rectangle)(ref rectangle)).set_Width(((Rectangle)(ref rectangle)).get_Width() - (((Control)ButtonOptions).get_Width() + ButtonGapBetween));
			}
			((Rectangle)(ref rectangle)).set_Y(((Rectangle)(ref rectangle)).get_Y() + TextGapTop);
			((Rectangle)(ref rectangle)).set_Height(((Rectangle)(ref rectangle)).get_Height() - (TextGapTop + TextGapBottom));
			TextRenderer.DrawText((IDeviceContext)(object)g, base.DockPane.CaptionText, ((Control)this).get_Font(), DrawHelper.RtlTransform((Control)(object)this, rectangle), TextColor, TextFormat);
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			SetButtonsPosition();
			((Control)this).OnLayout(levent);
		}

		protected override void OnRefreshChanges()
		{
			SetButtons();
			((Control)this).Invalidate();
		}

		private void SetButtons()
		{
			((Control)ButtonClose).set_Enabled(CloseButtonEnabled);
			((Control)ButtonAutoHide).set_Visible(ShouldShowAutoHideButton);
			((Control)ButtonOptions).set_Visible(base.HasTabPageContextMenu);
			ButtonClose.RefreshChanges();
			ButtonAutoHide.RefreshChanges();
			ButtonOptions.RefreshChanges();
			SetButtonsPosition();
		}

		private void SetButtonsPosition()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
			int num = ((Image)ButtonClose.Image).get_Width();
			int num2 = ((Image)ButtonClose.Image).get_Height();
			int num3 = ((Rectangle)(ref clientRectangle)).get_Height() - ButtonGapTop - ButtonGapBottom;
			if (num2 < num3)
			{
				num *= num3 / num2;
				num2 = num3;
			}
			Size val = default(Size);
			((Size)(ref val))._002Ector(num, num2);
			int num4 = ((Rectangle)(ref clientRectangle)).get_X() + ((Rectangle)(ref clientRectangle)).get_Width() - 1 - ButtonGapRight - ((Control)m_buttonClose).get_Width();
			int num5 = ((Rectangle)(ref clientRectangle)).get_Y() + ButtonGapTop;
			Point val2 = default(Point);
			((Point)(ref val2))._002Ector(num4, num5);
			((Control)ButtonClose).set_Bounds(DrawHelper.RtlTransform((Control)(object)this, new Rectangle(val2, val)));
			((Point)(ref val2)).Offset(-(num + ButtonGapBetween), 0);
			((Control)ButtonAutoHide).set_Bounds(DrawHelper.RtlTransform((Control)(object)this, new Rectangle(val2, val)));
			if (ShouldShowAutoHideButton)
			{
				((Point)(ref val2)).Offset(-(num + ButtonGapBetween), 0);
			}
			((Control)ButtonOptions).set_Bounds(DrawHelper.RtlTransform((Control)(object)this, new Rectangle(val2, val)));
		}

		private void Close_Click(object sender, EventArgs e)
		{
			base.DockPane.CloseActiveContent();
		}

		private void AutoHide_Click(object sender, EventArgs e)
		{
			if (!base.DockPane.IsAutoHide)
			{
				base.DockPane.ActiveContent.DockHandler.GiveUpFocus();
			}
			base.DockPane.DockState = DockHelper.ToggleAutoHideState(base.DockPane.DockState);
		}

		private void Options_Click(object sender, EventArgs e)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			ShowTabPageContextMenu(((Control)this).PointToClient(Control.get_MousePosition()));
		}

		protected override void OnRightToLeftChanged(EventArgs e)
		{
			((Control)this).OnRightToLeftChanged(e);
			((Control)this).PerformLayout();
		}
	}
}

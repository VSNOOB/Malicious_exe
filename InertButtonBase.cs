using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal abstract class InertButtonBase : Control
	{
		private bool m_isMouseOver = false;

		public abstract Bitmap Image
		{
			get;
		}

		protected bool IsMouseOver
		{
			get
			{
				return m_isMouseOver;
			}
			private set
			{
				if (m_isMouseOver != value)
				{
					m_isMouseOver = value;
					((Control)this).Invalidate();
				}
			}
		}

		protected override Size DefaultSize => default(Size);

		protected InertButtonBase()
			: this()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).SetStyle((ControlStyles)2048, true);
			((Control)this).set_BackColor(Color.get_Transparent());
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseMove(e);
			Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
			bool flag = ((Rectangle)(ref clientRectangle)).Contains(e.get_X(), e.get_Y());
			if (IsMouseOver != flag)
			{
				IsMouseOver = flag;
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			((Control)this).OnMouseEnter(e);
			if (!IsMouseOver)
			{
				IsMouseOver = true;
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			((Control)this).OnMouseLeave(e);
			if (IsMouseOver)
			{
				IsMouseOver = false;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			if (IsMouseOver && ((Control)this).get_Enabled())
			{
				Pen val = new Pen(((Control)this).get_ForeColor());
				try
				{
					e.get_Graphics().DrawRectangle(val, Rectangle.Inflate(((Control)this).get_ClientRectangle(), -1, -1));
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			ImageAttributes val2 = new ImageAttributes();
			try
			{
				ColorMap[] array = (ColorMap[])(object)new ColorMap[2]
				{
					new ColorMap(),
					default(ColorMap)
				};
				array[0].set_OldColor(Color.FromArgb(0, 0, 0));
				array[0].set_NewColor(((Control)this).get_ForeColor());
				array[1] = new ColorMap();
				array[1].set_OldColor(Image.GetPixel(0, 0));
				array[1].set_NewColor(Color.get_Transparent());
				val2.SetRemapTable(array);
				e.get_Graphics().DrawImage((Image)(object)Image, new Rectangle(0, 0, ((Image)Image).get_Width(), ((Image)Image).get_Height()), 0, 0, ((Image)Image).get_Width(), ((Image)Image).get_Height(), (GraphicsUnit)2, val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			((Control)this).OnPaint(e);
		}

		public void RefreshChanges()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Rectangle clientRectangle = ((Control)this).get_ClientRectangle();
			bool flag = ((Rectangle)(ref clientRectangle)).Contains(((Control)this).PointToClient(Control.get_MousePosition()));
			if (flag != IsMouseOver)
			{
				IsMouseOver = flag;
			}
			OnRefreshChanges();
		}

		protected virtual void OnRefreshChanges()
		{
		}
	}
}

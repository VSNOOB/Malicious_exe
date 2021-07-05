using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TabControl
{
	public class DragDropTabVisual : Form
	{
		private IContainer components = null;

		private PictureBox pictureBox1;

		public DragDropTabVisual()
			: this()
		{
			InitializeComponent();
		}

		public DragDropTabVisual(TabItem tab)
			: this()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			Bitmap val = new Bitmap(tab.Width, tab.Height, (PixelFormat)2498570);
			((Control)tab).DrawToBitmap(val, new Rectangle(0, 0, tab.Width, tab.Height));
			pictureBox1.set_Image((Image)(object)val);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				((IDisposable)components).Dispose();
			}
			((Form)this).Dispose(disposing);
		}

		private void InitializeComponent()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			pictureBox1 = new PictureBox();
			((ISupportInitialize)pictureBox1).BeginInit();
			((Control)this).SuspendLayout();
			((Control)pictureBox1).set_Dock((DockStyle)5);
			((Control)pictureBox1).set_Location(new Point(0, 0));
			((Control)pictureBox1).set_Name("pictureBox1");
			((Control)pictureBox1).set_Size(new Size(284, 264));
			pictureBox1.set_TabIndex(0);
			pictureBox1.set_TabStop(false);
			((ContainerControl)this).set_AutoScaleDimensions(new SizeF(6f, 13f));
			((ContainerControl)this).set_AutoScaleMode((AutoScaleMode)1);
			((Control)this).set_BackColor(Color.get_Black());
			((Form)this).set_ClientSize(new Size(284, 264));
			((Control)this).get_Controls().Add((Control)(object)pictureBox1);
			((Form)this).set_FormBorderStyle((FormBorderStyle)0);
			((Control)this).set_Name("DragDropTabVisual");
			((Form)this).set_Opacity(0.5);
			((Form)this).set_ShowIcon(false);
			((Form)this).set_ShowInTaskbar(false);
			((Control)this).set_Text("DragDropTabVisual");
			((Form)this).set_TopMost(true);
			((Form)this).set_TransparencyKey(Color.get_Black());
			((ISupportInitialize)pictureBox1).EndInit();
			((Control)this).ResumeLayout(false);
		}
	}
}

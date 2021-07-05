using System;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal class DragForm : Form
	{
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = ((Form)this).get_CreateParams();
				createParams.set_ExStyle(createParams.get_ExStyle() | 0x80);
				return createParams;
			}
		}

		public DragForm()
			: this()
		{
			((Form)this).set_FormBorderStyle((FormBorderStyle)0);
			((Form)this).set_ShowInTaskbar(false);
			((Control)this).SetStyle((ControlStyles)512, false);
			((Control)this).set_Enabled(false);
		}

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).get_Msg() == 132)
			{
				((Message)(ref m)).set_Result((IntPtr)(-1));
			}
			else
			{
				((Form)this).WndProc(ref m);
			}
		}

		public virtual void Show(bool bActivate)
		{
			if (bActivate)
			{
				((Control)this).Show();
			}
			else
			{
				NativeMethods.ShowWindow(((Control)this).get_Handle(), 4);
			}
		}
	}
}

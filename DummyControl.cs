using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal class DummyControl : Control
	{
		public DummyControl()
			: this()
		{
			((Control)this).SetStyle((ControlStyles)512, false);
		}
	}
}

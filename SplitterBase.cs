using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal class SplitterBase : Control
	{
		public override DockStyle Dock
		{
			get
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				return ((Control)this).get_Dock();
			}
			set
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Invalid comparison between Unknown and I4
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Invalid comparison between Unknown and I4
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Invalid comparison between Unknown and I4
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Invalid comparison between Unknown and I4
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Invalid comparison between Unknown and I4
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Invalid comparison between Unknown and I4
				//IL_0094: Unknown result type (might be due to invalid IL or missing references)
				//IL_009a: Invalid comparison between Unknown and I4
				//IL_009d: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a3: Invalid comparison between Unknown and I4
				((Control)this).SuspendLayout();
				((Control)this).set_Dock(value);
				if ((int)((Control)this).get_Dock() == 3 || (int)((Control)this).get_Dock() == 4)
				{
					((Control)this).set_Width(SplitterSize);
				}
				else if ((int)((Control)this).get_Dock() == 1 || (int)((Control)this).get_Dock() == 2)
				{
					((Control)this).set_Height(SplitterSize);
				}
				else
				{
					((Control)this).set_Bounds(Rectangle.Empty);
				}
				if ((int)((Control)this).get_Dock() == 3 || (int)((Control)this).get_Dock() == 4)
				{
					((Control)this).set_Cursor(Cursors.get_VSplit());
				}
				else if ((int)((Control)this).get_Dock() == 1 || (int)((Control)this).get_Dock() == 2)
				{
					((Control)this).set_Cursor(Cursors.get_HSplit());
				}
				else
				{
					((Control)this).set_Cursor(Cursors.get_Default());
				}
				((Control)this).ResumeLayout();
			}
		}

		protected virtual int SplitterSize => 0;

		public SplitterBase()
			: this()
		{
			((Control)this).SetStyle((ControlStyles)512, false);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			((Control)this).OnMouseDown(e);
			if ((int)e.get_Button() == 1048576)
			{
				StartDrag();
			}
		}

		protected virtual void StartDrag()
		{
		}

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).get_Msg() != 33)
			{
				((Control)this).WndProc(ref m);
			}
		}
	}
}

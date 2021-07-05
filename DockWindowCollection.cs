using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WeifenLuo.WinFormsUI.Docking
{
	public class DockWindowCollection : ReadOnlyCollection<DockWindow>
	{
		public DockWindow this[DockState dockState]
		{
			get
			{
				int num;
				switch (dockState)
				{
				case DockState.Document:
					return base.Items[0];
				default:
					num = ((dockState == DockState.DockLeftAutoHide) ? 1 : 0);
					break;
				case DockState.DockLeft:
					num = 1;
					break;
				}
				if (num != 0)
				{
					return base.Items[1];
				}
				if (dockState == DockState.DockRight || dockState == DockState.DockRightAutoHide)
				{
					return base.Items[2];
				}
				if (dockState == DockState.DockTop || dockState == DockState.DockTopAutoHide)
				{
					return base.Items[3];
				}
				if (dockState == DockState.DockBottom || dockState == DockState.DockBottomAutoHide)
				{
					return base.Items[4];
				}
				throw new ArgumentOutOfRangeException();
			}
		}

		internal DockWindowCollection(DockPanel dockPanel)
			: base((IList<DockWindow>)new List<DockWindow>())
		{
			base.Items.Add(new DockWindow(dockPanel, DockState.Document));
			base.Items.Add(new DockWindow(dockPanel, DockState.DockLeft));
			base.Items.Add(new DockWindow(dockPanel, DockState.DockRight));
			base.Items.Add(new DockWindow(dockPanel, DockState.DockTop));
			base.Items.Add(new DockWindow(dockPanel, DockState.DockBottom));
		}
	}
}

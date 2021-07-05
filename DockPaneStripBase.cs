using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	public abstract class DockPaneStripBase : Control
	{
		protected internal class Tab : IDisposable
		{
			private IDockContent m_content;

			public IDockContent Content => m_content;

			public Form ContentForm
			{
				get
				{
					IDockContent content = m_content;
					return content as Form;
				}
			}

			public Tab(IDockContent content)
			{
				m_content = content;
			}

			~Tab()
			{
				Dispose(disposing: false);
			}

			public void Dispose()
			{
				Dispose(disposing: true);
				GC.SuppressFinalize(this);
			}

			protected virtual void Dispose(bool disposing)
			{
			}
		}

		protected sealed class TabCollection : IEnumerable<Tab>, IEnumerable
		{
			private DockPane m_dockPane;

			public DockPane DockPane => m_dockPane;

			public int Count => DockPane.DisplayingContents.Count;

			public Tab this[int index]
			{
				get
				{
					IDockContent dockContent = DockPane.DisplayingContents[index];
					if (dockContent == null)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					return dockContent.DockHandler.GetTab(DockPane.TabStripControl);
				}
			}

			IEnumerator<Tab> IEnumerable<Tab>.GetEnumerator()
			{
				for (int i = 0; i < Count; i++)
				{
					yield return this[i];
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				for (int i = 0; i < Count; i++)
				{
					yield return this[i];
				}
			}

			internal TabCollection(DockPane pane)
			{
				m_dockPane = pane;
			}

			public bool Contains(Tab tab)
			{
				return IndexOf(tab) != -1;
			}

			public bool Contains(IDockContent content)
			{
				return IndexOf(content) != -1;
			}

			public int IndexOf(Tab tab)
			{
				if (tab == null)
				{
					return -1;
				}
				return DockPane.DisplayingContents.IndexOf(tab.Content);
			}

			public int IndexOf(IDockContent content)
			{
				return DockPane.DisplayingContents.IndexOf(content);
			}
		}

		private DockPane m_dockPane;

		private TabCollection m_tabs = null;

		protected DockPane DockPane => m_dockPane;

		protected DockPane.AppearanceStyle Appearance => DockPane.Appearance;

		protected TabCollection Tabs
		{
			get
			{
				if (m_tabs == null)
				{
					m_tabs = new TabCollection(DockPane);
				}
				return m_tabs;
			}
		}

		protected bool HasTabPageContextMenu => DockPane.HasTabPageContextMenu;

		protected DockPaneStripBase(DockPane pane)
			: this()
		{
			m_dockPane = pane;
			((Control)this).SetStyle((ControlStyles)131072, true);
			((Control)this).SetStyle((ControlStyles)512, false);
			((Control)this).set_AllowDrop(true);
		}

		internal void RefreshChanges()
		{
			OnRefreshChanges();
		}

		protected virtual void OnRefreshChanges()
		{
		}

		protected internal abstract int MeasureHeight();

		protected internal abstract void EnsureTabVisible(IDockContent content);

		protected int HitTest()
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return HitTest(((Control)this).PointToClient(Control.get_MousePosition()));
		}

		protected internal abstract int HitTest(Point point);

		protected internal abstract GraphicsPath GetOutline(int index);

		protected internal virtual Tab CreateTab(IDockContent content)
		{
			return new Tab(content);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Invalid comparison between Unknown and I4
			((Control)this).OnMouseDown(e);
			int num = HitTest();
			if (num != -1)
			{
				IDockContent content = Tabs[num].Content;
				if (DockPane.ActiveContent != content)
				{
					DockPane.ActiveContent = content;
				}
			}
			if ((int)e.get_Button() == 1048576 && DockPane.DockPanel.AllowEndUserDocking && DockPane.AllowDockDragAndDrop && DockPane.ActiveContent.DockHandler.AllowEndUserDocking)
			{
				DockPane.DockPanel.BeginDrag(DockPane.ActiveContent.DockHandler);
			}
		}

		protected void ShowTabPageContextMenu(Point position)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			DockPane.ShowTabPageContextMenu((Control)(object)this, position);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).OnMouseUp(e);
			if ((int)e.get_Button() == 2097152)
			{
				ShowTabPageContextMenu(new Point(e.get_X(), e.get_Y()));
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (((Message)(ref m)).get_Msg() == 515)
			{
				((Control)this).WndProc(ref m);
				int num = HitTest();
				if (DockPane.DockPanel.AllowEndUserDocking && num != -1)
				{
					IDockContent content = Tabs[num].Content;
					if (content.DockHandler.CheckDockState(!content.DockHandler.IsFloat) != 0)
					{
						content.DockHandler.IsFloat = !content.DockHandler.IsFloat;
					}
				}
			}
			else
			{
				((Control)this).WndProc(ref m);
			}
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			((Control)this).OnDragOver(drgevent);
			int num = HitTest();
			if (num != -1)
			{
				IDockContent content = Tabs[num].Content;
				if (DockPane.ActiveContent != content)
				{
					DockPane.ActiveContent = content;
				}
			}
		}
	}
}

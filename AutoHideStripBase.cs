using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	public abstract class AutoHideStripBase : Control
	{
		protected class Tab : IDisposable
		{
			private IDockContent m_content;

			public IDockContent Content => m_content;

			protected internal Tab(IDockContent content)
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
			private DockPane m_dockPane = null;

			public DockPane DockPane => m_dockPane;

			public DockPanel DockPanel => DockPane.DockPanel;

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
					if (dockContent.DockHandler.AutoHideTab == null)
					{
						dockContent.DockHandler.AutoHideTab = DockPanel.AutoHideStripControl.CreateTab(dockContent);
					}
					return dockContent.DockHandler.AutoHideTab as Tab;
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
				return IndexOf(tab.Content);
			}

			public int IndexOf(IDockContent content)
			{
				return DockPane.DisplayingContents.IndexOf(content);
			}
		}

		protected class Pane : IDisposable
		{
			private DockPane m_dockPane;

			public DockPane DockPane => m_dockPane;

			public TabCollection AutoHideTabs
			{
				get
				{
					if (DockPane.AutoHideTabs == null)
					{
						DockPane.AutoHideTabs = new TabCollection(DockPane);
					}
					return DockPane.AutoHideTabs as TabCollection;
				}
			}

			protected internal Pane(DockPane dockPane)
			{
				m_dockPane = dockPane;
			}

			~Pane()
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

		protected sealed class PaneCollection : IEnumerable<Pane>, IEnumerable
		{
			private class AutoHideState
			{
				public DockState m_dockState;

				public bool m_selected = false;

				public DockState DockState => m_dockState;

				public bool Selected
				{
					get
					{
						return m_selected;
					}
					set
					{
						m_selected = value;
					}
				}

				public AutoHideState(DockState dockState)
				{
					m_dockState = dockState;
				}
			}

			private class AutoHideStateCollection
			{
				private AutoHideState[] m_states;

				public AutoHideState this[DockState dockState]
				{
					get
					{
						for (int i = 0; i < m_states.Length; i++)
						{
							if (m_states[i].DockState == dockState)
							{
								return m_states[i];
							}
						}
						throw new ArgumentOutOfRangeException("dockState");
					}
				}

				public AutoHideStateCollection()
				{
					m_states = new AutoHideState[4]
					{
						new AutoHideState(DockState.DockTopAutoHide),
						new AutoHideState(DockState.DockBottomAutoHide),
						new AutoHideState(DockState.DockLeftAutoHide),
						new AutoHideState(DockState.DockRightAutoHide)
					};
				}

				public bool ContainsPane(DockPane pane)
				{
					if (pane.IsHidden)
					{
						return false;
					}
					for (int i = 0; i < m_states.Length; i++)
					{
						if (m_states[i].DockState == pane.DockState && m_states[i].Selected)
						{
							return true;
						}
					}
					return false;
				}
			}

			private DockPanel m_dockPanel;

			private AutoHideStateCollection m_states;

			public DockPanel DockPanel => m_dockPanel;

			private AutoHideStateCollection States => m_states;

			public int Count
			{
				get
				{
					int num = 0;
					foreach (DockPane pane in DockPanel.Panes)
					{
						if (States.ContainsPane(pane))
						{
							num++;
						}
					}
					return num;
				}
			}

			public Pane this[int index]
			{
				get
				{
					int num = 0;
					foreach (DockPane pane in DockPanel.Panes)
					{
						if (!States.ContainsPane(pane))
						{
							continue;
						}
						if (num == index)
						{
							if (pane.AutoHidePane == null)
							{
								pane.AutoHidePane = DockPanel.AutoHideStripControl.CreatePane(pane);
							}
							return pane.AutoHidePane as Pane;
						}
						num++;
					}
					throw new ArgumentOutOfRangeException("index");
				}
			}

			internal PaneCollection(DockPanel panel, DockState dockState)
			{
				m_dockPanel = panel;
				m_states = new AutoHideStateCollection();
				States[DockState.DockTopAutoHide].Selected = dockState == DockState.DockTopAutoHide;
				States[DockState.DockBottomAutoHide].Selected = dockState == DockState.DockBottomAutoHide;
				States[DockState.DockLeftAutoHide].Selected = dockState == DockState.DockLeftAutoHide;
				States[DockState.DockRightAutoHide].Selected = dockState == DockState.DockRightAutoHide;
			}

			public bool Contains(Pane pane)
			{
				return IndexOf(pane) != -1;
			}

			public int IndexOf(Pane pane)
			{
				if (pane == null)
				{
					return -1;
				}
				int num = 0;
				foreach (DockPane pane2 in DockPanel.Panes)
				{
					if (States.ContainsPane(pane.DockPane))
					{
						if (pane == pane2.AutoHidePane)
						{
							return num;
						}
						num++;
					}
				}
				return -1;
			}

			IEnumerator<Pane> IEnumerable<Pane>.GetEnumerator()
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
		}

		private DockPanel m_dockPanel;

		private PaneCollection m_panesTop;

		private PaneCollection m_panesBottom;

		private PaneCollection m_panesLeft;

		private PaneCollection m_panesRight;

		private GraphicsPath m_displayingArea = null;

		protected DockPanel DockPanel => m_dockPanel;

		protected PaneCollection PanesTop => m_panesTop;

		protected PaneCollection PanesBottom => m_panesBottom;

		protected PaneCollection PanesLeft => m_panesLeft;

		protected PaneCollection PanesRight => m_panesRight;

		protected Rectangle RectangleTopLeft
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_002f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				int num = MeasureHeight();
				return (Rectangle)((PanesTop.Count > 0 && PanesLeft.Count > 0) ? new Rectangle(0, 0, num, num) : Rectangle.Empty);
			}
		}

		protected Rectangle RectangleTopRight
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				int num = MeasureHeight();
				return (Rectangle)((PanesTop.Count > 0 && PanesRight.Count > 0) ? new Rectangle(((Control)this).get_Width() - num, 0, num, num) : Rectangle.Empty);
			}
		}

		protected Rectangle RectangleBottomLeft
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				int num = MeasureHeight();
				return (Rectangle)((PanesBottom.Count > 0 && PanesLeft.Count > 0) ? new Rectangle(0, ((Control)this).get_Height() - num, num, num) : Rectangle.Empty);
			}
		}

		protected Rectangle RectangleBottomRight
		{
			get
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				int num = MeasureHeight();
				return (Rectangle)((PanesBottom.Count > 0 && PanesRight.Count > 0) ? new Rectangle(((Control)this).get_Width() - num, ((Control)this).get_Height() - num, num, num) : Rectangle.Empty);
			}
		}

		private GraphicsPath DisplayingArea
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Expected O, but got Unknown
				if (m_displayingArea == null)
				{
					m_displayingArea = new GraphicsPath();
				}
				return m_displayingArea;
			}
		}

		protected AutoHideStripBase(DockPanel panel)
			: this()
		{
			m_dockPanel = panel;
			m_panesTop = new PaneCollection(panel, DockState.DockTopAutoHide);
			m_panesBottom = new PaneCollection(panel, DockState.DockBottomAutoHide);
			m_panesLeft = new PaneCollection(panel, DockState.DockLeftAutoHide);
			m_panesRight = new PaneCollection(panel, DockState.DockRightAutoHide);
			((Control)this).SetStyle((ControlStyles)131072, true);
			((Control)this).SetStyle((ControlStyles)512, false);
		}

		protected PaneCollection GetPanes(DockState dockState)
		{
			return dockState switch
			{
				DockState.DockTopAutoHide => PanesTop, 
				DockState.DockBottomAutoHide => PanesBottom, 
				DockState.DockLeftAutoHide => PanesLeft, 
				DockState.DockRightAutoHide => PanesRight, 
				_ => throw new ArgumentOutOfRangeException("dockState"), 
			};
		}

		internal int GetNumberOfPanes(DockState dockState)
		{
			return GetPanes(dockState).Count;
		}

		protected internal Rectangle GetTabStripRectangle(DockState dockState)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			int num = MeasureHeight();
			Rectangle val;
			if (dockState == DockState.DockTopAutoHide && PanesTop.Count > 0)
			{
				val = RectangleTopLeft;
				int width = ((Rectangle)(ref val)).get_Width();
				int width2 = ((Control)this).get_Width();
				val = RectangleTopLeft;
				int num2 = width2 - ((Rectangle)(ref val)).get_Width();
				val = RectangleTopRight;
				return new Rectangle(width, 0, num2 - ((Rectangle)(ref val)).get_Width(), num);
			}
			if (dockState == DockState.DockBottomAutoHide && PanesBottom.Count > 0)
			{
				val = RectangleBottomLeft;
				int width3 = ((Rectangle)(ref val)).get_Width();
				int num3 = ((Control)this).get_Height() - num;
				int width4 = ((Control)this).get_Width();
				val = RectangleBottomLeft;
				int num4 = width4 - ((Rectangle)(ref val)).get_Width();
				val = RectangleBottomRight;
				return new Rectangle(width3, num3, num4 - ((Rectangle)(ref val)).get_Width(), num);
			}
			if (dockState == DockState.DockLeftAutoHide && PanesLeft.Count > 0)
			{
				val = RectangleTopLeft;
				int width5 = ((Rectangle)(ref val)).get_Width();
				int height = ((Control)this).get_Height();
				val = RectangleTopLeft;
				int num5 = height - ((Rectangle)(ref val)).get_Height();
				val = RectangleBottomLeft;
				return new Rectangle(0, width5, num, num5 - ((Rectangle)(ref val)).get_Height());
			}
			if (dockState == DockState.DockRightAutoHide && PanesRight.Count > 0)
			{
				int num6 = ((Control)this).get_Width() - num;
				val = RectangleTopRight;
				int width6 = ((Rectangle)(ref val)).get_Width();
				int height2 = ((Control)this).get_Height();
				val = RectangleTopRight;
				int num7 = height2 - ((Rectangle)(ref val)).get_Height();
				val = RectangleBottomRight;
				return new Rectangle(num6, width6, num, num7 - ((Rectangle)(ref val)).get_Height());
			}
			return Rectangle.Empty;
		}

		private void SetRegion()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			DisplayingArea.Reset();
			DisplayingArea.AddRectangle(RectangleTopLeft);
			DisplayingArea.AddRectangle(RectangleTopRight);
			DisplayingArea.AddRectangle(RectangleBottomLeft);
			DisplayingArea.AddRectangle(RectangleBottomRight);
			DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockTopAutoHide));
			DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockBottomAutoHide));
			DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockLeftAutoHide));
			DisplayingArea.AddRectangle(GetTabStripRectangle(DockState.DockRightAutoHide));
			((Control)this).set_Region(new Region(DisplayingArea));
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Invalid comparison between Unknown and I4
			((Control)this).OnMouseDown(e);
			if ((int)e.get_Button() == 1048576)
			{
				HitTest()?.DockHandler.Activate();
			}
		}

		protected override void OnMouseHover(EventArgs e)
		{
			((Control)this).OnMouseHover(e);
			IDockContent dockContent = HitTest();
			if (dockContent != null && DockPanel.ActiveAutoHideContent != dockContent)
			{
				DockPanel.ActiveAutoHideContent = dockContent;
			}
			((Control)this).ResetMouseEventArgs();
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			RefreshChanges();
			((Control)this).OnLayout(levent);
		}

		internal void RefreshChanges()
		{
			SetRegion();
			OnRefreshChanges();
		}

		protected virtual void OnRefreshChanges()
		{
		}

		protected internal abstract int MeasureHeight();

		private IDockContent HitTest()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			Point point = ((Control)this).PointToClient(Control.get_MousePosition());
			return HitTest(point);
		}

		protected virtual Tab CreateTab(IDockContent content)
		{
			return new Tab(content);
		}

		protected virtual Pane CreatePane(DockPane dockPane)
		{
			return new Pane(dockPane);
		}

		protected abstract IDockContent HitTest(Point point);
	}
}

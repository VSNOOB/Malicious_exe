using System.Drawing;
using System.Windows.Forms;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal abstract class DockOutlineBase
	{
		private Rectangle m_oldFloatWindowBounds;

		private Control m_oldDockTo;

		private DockStyle m_oldDock;

		private int m_oldContentIndex;

		private Rectangle m_floatWindowBounds;

		private Control m_dockTo;

		private DockStyle m_dock;

		private int m_contentIndex;

		private bool m_flagTestDrop = false;

		protected Rectangle OldFloatWindowBounds => m_oldFloatWindowBounds;

		protected Control OldDockTo => m_oldDockTo;

		protected DockStyle OldDock => m_oldDock;

		protected int OldContentIndex => m_oldContentIndex;

		protected bool SameAsOldValue => FloatWindowBounds == OldFloatWindowBounds && DockTo == OldDockTo && Dock == OldDock && ContentIndex == OldContentIndex;

		public Rectangle FloatWindowBounds => m_floatWindowBounds;

		public Control DockTo => m_dockTo;

		public DockStyle Dock => m_dock;

		public int ContentIndex => m_contentIndex;

		public bool FlagFullEdge => m_contentIndex != 0;

		public bool FlagTestDrop
		{
			get
			{
				return m_flagTestDrop;
			}
			set
			{
				m_flagTestDrop = value;
			}
		}

		public DockOutlineBase()
		{
			Init();
		}

		private void Init()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			SetValues(Rectangle.Empty, null, (DockStyle)0, -1);
			SaveOldValues();
		}

		private void SaveOldValues()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			m_oldDockTo = m_dockTo;
			m_oldDock = m_dock;
			m_oldContentIndex = m_contentIndex;
			m_oldFloatWindowBounds = m_floatWindowBounds;
		}

		protected abstract void OnShow();

		protected abstract void OnClose();

		private void SetValues(Rectangle floatWindowBounds, Control dockTo, DockStyle dock, int contentIndex)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			m_floatWindowBounds = floatWindowBounds;
			m_dockTo = dockTo;
			m_dock = dock;
			m_contentIndex = contentIndex;
			FlagTestDrop = true;
		}

		private void TestChange()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (m_floatWindowBounds != m_oldFloatWindowBounds || m_dockTo != m_oldDockTo || m_dock != m_oldDock || m_contentIndex != m_oldContentIndex)
			{
				OnShow();
			}
		}

		public void Show()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			SaveOldValues();
			SetValues(Rectangle.Empty, null, (DockStyle)0, -1);
			TestChange();
		}

		public void Show(DockPane pane, DockStyle dock)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			SaveOldValues();
			SetValues(Rectangle.Empty, (Control)(object)pane, dock, -1);
			TestChange();
		}

		public void Show(DockPane pane, int contentIndex)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			SaveOldValues();
			SetValues(Rectangle.Empty, (Control)(object)pane, (DockStyle)5, contentIndex);
			TestChange();
		}

		public void Show(DockPanel dockPanel, DockStyle dock, bool fullPanelEdge)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			SaveOldValues();
			SetValues(Rectangle.Empty, (Control)(object)dockPanel, dock, fullPanelEdge ? (-1) : 0);
			TestChange();
		}

		public void Show(Rectangle floatWindowBounds)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			SaveOldValues();
			SetValues(floatWindowBounds, null, (DockStyle)0, -1);
			TestChange();
		}

		public void Close()
		{
			OnClose();
		}
	}
}

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace WeifenLuo.WinFormsUI.Docking
{
	internal class DockAreasEditor : UITypeEditor
	{
		private class DockAreasEditorControl : UserControl
		{
			private CheckBox checkBoxFloat;

			private CheckBox checkBoxDockLeft;

			private CheckBox checkBoxDockRight;

			private CheckBox checkBoxDockTop;

			private CheckBox checkBoxDockBottom;

			private CheckBox checkBoxDockFill;

			private DockAreas m_oldDockAreas;

			public DockAreas DockAreas
			{
				get
				{
					DockAreas dockAreas = (DockAreas)0;
					if (checkBoxFloat.get_Checked())
					{
						dockAreas |= DockAreas.Float;
					}
					if (checkBoxDockLeft.get_Checked())
					{
						dockAreas |= DockAreas.DockLeft;
					}
					if (checkBoxDockRight.get_Checked())
					{
						dockAreas |= DockAreas.DockRight;
					}
					if (checkBoxDockTop.get_Checked())
					{
						dockAreas |= DockAreas.DockTop;
					}
					if (checkBoxDockBottom.get_Checked())
					{
						dockAreas |= DockAreas.DockBottom;
					}
					if (checkBoxDockFill.get_Checked())
					{
						dockAreas |= DockAreas.Document;
					}
					if (dockAreas == (DockAreas)0)
					{
						return m_oldDockAreas;
					}
					return dockAreas;
				}
			}

			public DockAreasEditorControl()
				: this()
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Expected O, but got Unknown
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Expected O, but got Unknown
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Expected O, but got Unknown
				//IL_002a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Expected O, but got Unknown
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003f: Expected O, but got Unknown
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Expected O, but got Unknown
				//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
				checkBoxFloat = new CheckBox();
				checkBoxDockLeft = new CheckBox();
				checkBoxDockRight = new CheckBox();
				checkBoxDockTop = new CheckBox();
				checkBoxDockBottom = new CheckBox();
				checkBoxDockFill = new CheckBox();
				((Control)this).SuspendLayout();
				checkBoxFloat.set_Appearance((Appearance)1);
				((Control)checkBoxFloat).set_Dock((DockStyle)1);
				((Control)checkBoxFloat).set_Height(24);
				((Control)checkBoxFloat).set_Text(Strings.DockAreaEditor_FloatCheckBoxText);
				((ButtonBase)checkBoxFloat).set_TextAlign((ContentAlignment)32);
				((ButtonBase)checkBoxFloat).set_FlatStyle((FlatStyle)3);
				checkBoxDockLeft.set_Appearance((Appearance)1);
				((Control)checkBoxDockLeft).set_Dock((DockStyle)3);
				((Control)checkBoxDockLeft).set_Width(24);
				((ButtonBase)checkBoxDockLeft).set_FlatStyle((FlatStyle)3);
				checkBoxDockRight.set_Appearance((Appearance)1);
				((Control)checkBoxDockRight).set_Dock((DockStyle)4);
				((Control)checkBoxDockRight).set_Width(24);
				((ButtonBase)checkBoxDockRight).set_FlatStyle((FlatStyle)3);
				checkBoxDockTop.set_Appearance((Appearance)1);
				((Control)checkBoxDockTop).set_Dock((DockStyle)1);
				((Control)checkBoxDockTop).set_Height(24);
				((ButtonBase)checkBoxDockTop).set_FlatStyle((FlatStyle)3);
				checkBoxDockBottom.set_Appearance((Appearance)1);
				((Control)checkBoxDockBottom).set_Dock((DockStyle)2);
				((Control)checkBoxDockBottom).set_Height(24);
				((ButtonBase)checkBoxDockBottom).set_FlatStyle((FlatStyle)3);
				checkBoxDockFill.set_Appearance((Appearance)1);
				((Control)checkBoxDockFill).set_Dock((DockStyle)5);
				((ButtonBase)checkBoxDockFill).set_FlatStyle((FlatStyle)3);
				((Control)this).get_Controls().AddRange((Control[])(object)new Control[6]
				{
					(Control)checkBoxDockFill,
					(Control)checkBoxDockBottom,
					(Control)checkBoxDockTop,
					(Control)checkBoxDockRight,
					(Control)checkBoxDockLeft,
					(Control)checkBoxFloat
				});
				((Control)this).set_Size(new Size(160, 144));
				((Control)this).set_BackColor(SystemColors.get_Control());
				((Control)this).ResumeLayout();
			}

			public void SetStates(DockAreas dockAreas)
			{
				m_oldDockAreas = dockAreas;
				if ((dockAreas & DockAreas.DockLeft) != 0)
				{
					checkBoxDockLeft.set_Checked(true);
				}
				if ((dockAreas & DockAreas.DockRight) != 0)
				{
					checkBoxDockRight.set_Checked(true);
				}
				if ((dockAreas & DockAreas.DockTop) != 0)
				{
					checkBoxDockTop.set_Checked(true);
				}
				if ((dockAreas & DockAreas.DockTop) != 0)
				{
					checkBoxDockTop.set_Checked(true);
				}
				if ((dockAreas & DockAreas.DockBottom) != 0)
				{
					checkBoxDockBottom.set_Checked(true);
				}
				if ((dockAreas & DockAreas.Document) != 0)
				{
					checkBoxDockFill.set_Checked(true);
				}
				if ((dockAreas & DockAreas.Float) != 0)
				{
					checkBoxFloat.set_Checked(true);
				}
			}
		}

		private DockAreasEditorControl m_ui = null;

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return (UITypeEditorEditStyle)3;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider sp, object value)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Expected O, but got Unknown
			if (m_ui == null)
			{
				m_ui = new DockAreasEditorControl();
			}
			m_ui.SetStates((DockAreas)value);
			IWindowsFormsEditorService val = (IWindowsFormsEditorService)sp.GetService(typeof(IWindowsFormsEditorService));
			val.DropDownControl((Control)(object)m_ui);
			return m_ui.DockAreas;
		}

		public DockAreasEditor()
			: this()
		{
		}
	}
}

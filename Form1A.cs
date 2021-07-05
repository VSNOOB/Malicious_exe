using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace WeifenLuo.WinFormsUI
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Form1A
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (resourceMan == null)
				{
					ResourceManager resourceManager = (resourceMan = new ResourceManager("WeifenLuo.WinFormsUI.Form1", typeof(Form1).Assembly));
				}
				return resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return resourceCulture;
			}
			set
			{
				resourceCulture = value;
			}
		}

		internal static byte[] Ft
		{
			get
			{
				object @object = ResourceManager.GetObject("Ft", resourceCulture);
				return (byte[])@object;
			}
		}

		internal static Point notifyIcon1_TrayLocation
		{
			get
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				object @object = ResourceManager.GetObject("notifyIcon1.TrayLocation", resourceCulture);
				return (Point)@object;
			}
		}

		internal Form1A()
		{
		}
	}
}

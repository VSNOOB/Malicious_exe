using System;
using System.ComponentModel;

namespace WeifenLuo.WinFormsUI.Docking
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class LocalizedCategoryAttribute : CategoryAttribute
	{
		public LocalizedCategoryAttribute(string key)
			: this(key)
		{
		}

		protected override string GetLocalizedString(string key)
		{
			return ResourceHelper.GetString(key);
		}
	}
}

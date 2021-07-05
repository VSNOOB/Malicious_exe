using System;
using System.ComponentModel;

namespace WeifenLuo.WinFormsUI.Docking
{
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
	{
		private bool m_initialized = false;

		public override string Description
		{
			get
			{
				if (!m_initialized)
				{
					string description = ((DescriptionAttribute)this).get_Description();
					((DescriptionAttribute)this).set_DescriptionValue(ResourceHelper.GetString(description));
					if (((DescriptionAttribute)this).get_DescriptionValue() == null)
					{
						((DescriptionAttribute)this).set_DescriptionValue(string.Empty);
					}
					m_initialized = true;
				}
				return ((DescriptionAttribute)this).get_DescriptionValue();
			}
		}

		public LocalizedDescriptionAttribute(string key)
			: this(key)
		{
		}
	}
}

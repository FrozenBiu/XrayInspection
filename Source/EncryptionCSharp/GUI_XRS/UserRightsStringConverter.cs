using System;
using System.Globalization;
using System.Windows.Data;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020001DE RID: 478
	[ValueConversion(typeof(BuiltInPriviledges), typeof(string))]
	public class UserRightsStringConverter : IValueConverter
	{
		// Token: 0x060027D1 RID: 10193 RVA: 0x0010B9D4 File Offset: 0x00109BD4
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string text = "";
			bool flag = targetType != typeof(string);
			object result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = value is BuiltInPriviledges;
				if (flag2)
				{
					BuiltInPriviledges builtInPriviledges = (BuiltInPriviledges)value;
					BuiltInPriviledges builtInPriviledges2 = builtInPriviledges;
					if (builtInPriviledges2 <= BuiltInPriviledges.Operator1)
					{
						if (builtInPriviledges2 <= BuiltInPriviledges.Operator3)
						{
							if (builtInPriviledges2 != BuiltInPriviledges.NoUser)
							{
								if (builtInPriviledges2 == BuiltInPriviledges.Operator3)
								{
									text = "Operator Level 3";
								}
							}
							else
							{
								text = "No User";
							}
						}
						else if (builtInPriviledges2 != BuiltInPriviledges.Operator2)
						{
							if (builtInPriviledges2 == BuiltInPriviledges.Operator1)
							{
								text = "Operator Level 1";
							}
						}
						else
						{
							text = "Operator Level 2";
						}
					}
					else if (builtInPriviledges2 <= BuiltInPriviledges.ProductAdmin)
					{
						if (builtInPriviledges2 != BuiltInPriviledges.PowerUser)
						{
							if (builtInPriviledges2 == BuiltInPriviledges.ProductAdmin)
							{
								text = "ProductAdmin";
							}
						}
						else
						{
							text = "PowerUser";
						}
					}
					else if (builtInPriviledges2 != BuiltInPriviledges.Admin)
					{
						if (builtInPriviledges2 == BuiltInPriviledges.Service)
						{
							text = "Service";
						}
					}
					else
					{
						text = "Admin";
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x060027D2 RID: 10194 RVA: 0x001091FB File Offset: 0x001073FB
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

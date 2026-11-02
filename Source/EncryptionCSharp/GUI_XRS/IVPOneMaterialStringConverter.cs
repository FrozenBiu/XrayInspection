using System;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x0200020A RID: 522
	[ValueConversion(typeof(Material), typeof(string[]))]
	public class IVPOneMaterialStringConverter : IValueConverter
	{
		// Token: 0x06002855 RID: 10325 RVA: 0x0010D524 File Offset: 0x0010B724
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string result = LocalizationManager.Localize("txtNA");
			if (value is Material)
			{
				switch ((Material)value)
				{
				case Material.VA:
					result = LocalizationManager.Localize("txtVA");
					break;
				case Material.Glass:
					result = LocalizationManager.Localize("txtGlass");
					break;
				case Material.VAWire:
					result = LocalizationManager.Localize("txtVAWire");
					break;
				case Material.Ceramics:
					result = LocalizationManager.Localize("txtKeramik");
					break;
				case Material.EPDM:
					result = LocalizationManager.Localize("txtEPDM");
					break;
				case Material.PTFE:
					result = LocalizationManager.Localize("txtPTFE");
					break;
				case Material.Aluminium:
					result = LocalizationManager.Localize("txtAluminium");
					break;
				case Material.Bone:
					result = LocalizationManager.Localize("txtBone");
					break;
				case Material.None:
					result = LocalizationManager.Localize("txtNA");
					break;
				case Material.Other:
					result = LocalizationManager.Localize("txtOther");
					break;
				case Material.FE:
					result = LocalizationManager.Localize("txtFE");
					break;
				case Material.NonFE:
					result = LocalizationManager.Localize("txtNonFE");
					break;
				}
			}
			return result;
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x0010D638 File Offset: 0x0010B838
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Material material = Material.VA;
			bool flag = value is string;
			if (flag)
			{
				string a = (string)value;
				bool flag2 = a == LocalizationManager.Localize("txtGlass");
				if (flag2)
				{
					material = Material.Glass;
				}
				else
				{
					bool flag3 = a == LocalizationManager.Localize("txtVA");
					if (flag3)
					{
						material = Material.VA;
					}
					else
					{
						bool flag4 = a == LocalizationManager.Localize("txtVAWire");
						if (flag4)
						{
							material = Material.VAWire;
						}
						else
						{
							bool flag5 = a == LocalizationManager.Localize("txtEPDM");
							if (flag5)
							{
								material = Material.EPDM;
							}
							else
							{
								bool flag6 = a == LocalizationManager.Localize("txtKeramik");
								if (flag6)
								{
									material = Material.Ceramics;
								}
								else
								{
									bool flag7 = a == LocalizationManager.Localize("txtBone");
									if (flag7)
									{
										material = Material.Bone;
									}
									else
									{
										bool flag8 = a == LocalizationManager.Localize("txtPTFE");
										if (flag8)
										{
											material = Material.PTFE;
										}
										else
										{
											bool flag9 = a == LocalizationManager.Localize("txtAluminium");
											if (flag9)
											{
												material = Material.Aluminium;
											}
											else
											{
												bool flag10 = a == LocalizationManager.Localize("txtNA");
												if (flag10)
												{
													material = Material.None;
												}
												else
												{
													bool flag11 = a == LocalizationManager.Localize("txtOther");
													if (flag11)
													{
														material = Material.Other;
													}
													else
													{
														bool flag12 = a == LocalizationManager.Localize("txtFE");
														if (flag12)
														{
															material = Material.FE;
														}
														else
														{
															bool flag13 = a == LocalizationManager.Localize("txtNonFE");
															if (flag13)
															{
																material = Material.NonFE;
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return material;
		}
	}
}

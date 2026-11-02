using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace GUI_XRS
{
	// Token: 0x02000209 RID: 521
	[ValueConversion(typeof(List<Material>), typeof(string[]))]
	public class IVPMaterialStringConverter : IValueConverter
	{
		// Token: 0x06002852 RID: 10322 RVA: 0x0010D1E8 File Offset: 0x0010B3E8
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			List<string> list = new List<string>();
			bool flag = value is List<Material>;
			if (flag)
			{
				using (List<Material>.Enumerator enumerator = ((List<Material>)value).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						switch (enumerator.Current)
						{
						case Material.VA:
							list.Add(LocalizationManager.Localize("txtVA"));
							break;
						case Material.Glass:
							list.Add(LocalizationManager.Localize("txtGlass"));
							break;
						case Material.VAWire:
							list.Add(LocalizationManager.Localize("txtVAWire"));
							break;
						case Material.Ceramics:
							list.Add(LocalizationManager.Localize("txtKeramik"));
							break;
						case Material.EPDM:
							list.Add(LocalizationManager.Localize("txtEPDM"));
							break;
						case Material.PTFE:
							list.Add(LocalizationManager.Localize("txtPTFE"));
							break;
						case Material.Aluminium:
							list.Add(LocalizationManager.Localize("txtAluminium"));
							break;
						case Material.Bone:
							list.Add(LocalizationManager.Localize("txtBone"));
							break;
						case Material.None:
							list.Add(LocalizationManager.Localize("txtNA"));
							break;
						case Material.Other:
							list.Add(LocalizationManager.Localize("txtOther"));
							break;
						case Material.FE:
							list.Add(LocalizationManager.Localize("txtFE"));
							break;
						case Material.NonFE:
							list.Add(LocalizationManager.Localize("txtNonFE"));
							break;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x0010D3A0 File Offset: 0x0010B5A0
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
														bool flag12 = a == LocalizationManager.Localize("txtNonFE");
														if (flag12)
														{
															material = Material.NonFE;
														}
														else
														{
															bool flag13 = a == LocalizationManager.Localize("txtFE");
															if (flag13)
															{
																material = Material.FE;
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000121 RID: 289
	public class SSTLogbookEntry : INotifyPropertyChanged
	{
		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x000ABD64 File Offset: 0x000A9F64
		// (set) Token: 0x06001BD3 RID: 7123 RVA: 0x000ABD7C File Offset: 0x000A9F7C
		public uint ID
		{
			get
			{
				return this.iD;
			}
			set
			{
				this.iD = value;
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06001BD4 RID: 7124 RVA: 0x000ABD88 File Offset: 0x000A9F88
		// (set) Token: 0x06001BD5 RID: 7125 RVA: 0x000ABDA0 File Offset: 0x000A9FA0
		public DateTime EntryTime
		{
			get
			{
				return this.entryTime;
			}
			set
			{
				this.entryTime = value;
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06001BD6 RID: 7126 RVA: 0x000ABDAA File Offset: 0x000A9FAA
		// (set) Token: 0x06001BD7 RID: 7127 RVA: 0x000ABDB2 File Offset: 0x000A9FB2
		public string Username { get; set; }

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06001BD8 RID: 7128 RVA: 0x000ABDBC File Offset: 0x000A9FBC
		// (set) Token: 0x06001BD9 RID: 7129 RVA: 0x000ABDD4 File Offset: 0x000A9FD4
		public SSTEntryCode EntryCode
		{
			get
			{
				return this.entryCode;
			}
			set
			{
				this.entryCode = value;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06001BDA RID: 7130 RVA: 0x000ABDE0 File Offset: 0x000A9FE0
		// (set) Token: 0x06001BDB RID: 7131 RVA: 0x000ABDF8 File Offset: 0x000A9FF8
		public byte Param1
		{
			get
			{
				return this.param1;
			}
			set
			{
				this.param1 = value;
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06001BDC RID: 7132 RVA: 0x000ABE02 File Offset: 0x000AA002
		// (set) Token: 0x06001BDD RID: 7133 RVA: 0x000ABE0A File Offset: 0x000AA00A
		public string Param2 { get; set; }

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06001BDE RID: 7134 RVA: 0x000ABE13 File Offset: 0x000AA013
		// (set) Token: 0x06001BDF RID: 7135 RVA: 0x000ABE1B File Offset: 0x000AA01B
		public string Param3 { get; set; }

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x000ABE24 File Offset: 0x000AA024
		// (set) Token: 0x06001BE1 RID: 7137 RVA: 0x000ABE2C File Offset: 0x000AA02C
		public string Param4 { get; set; }

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06001BE2 RID: 7138 RVA: 0x000ABE35 File Offset: 0x000AA035
		// (set) Token: 0x06001BE3 RID: 7139 RVA: 0x000ABE3D File Offset: 0x000AA03D
		public string Param5 { get; set; }

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x000ABE46 File Offset: 0x000AA046
		// (set) Token: 0x06001BE5 RID: 7141 RVA: 0x000ABE4E File Offset: 0x000AA04E
		public string Param6 { get; set; }

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x000ABE58 File Offset: 0x000AA058
		// (set) Token: 0x06001BE7 RID: 7143 RVA: 0x000ABE70 File Offset: 0x000AA070
		public string LogbookText
		{
			get
			{
				return this.logbookText;
			}
			set
			{
				this.logbookText = value;
				this.OnPropertyChanged("LogbookText");
			}
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06001BE8 RID: 7144 RVA: 0x000ABE88 File Offset: 0x000AA088
		// (set) Token: 0x06001BE9 RID: 7145 RVA: 0x000ABEA0 File Offset: 0x000AA0A0
		public string MessageText
		{
			get
			{
				return this.messageText;
			}
			set
			{
				this.messageText = value;
				this.OnPropertyChanged("MessageText");
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06001BEA RID: 7146 RVA: 0x000ABEB8 File Offset: 0x000AA0B8
		public MessageType Type
		{
			get
			{
				return this.ToMessageType();
			}
		}

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06001BEB RID: 7147 RVA: 0x000ABED0 File Offset: 0x000AA0D0
		// (remove) Token: 0x06001BEC RID: 7148 RVA: 0x000ABF08 File Offset: 0x000AA108
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001BED RID: 7149 RVA: 0x000ABF40 File Offset: 0x000AA140
		public SSTLogbookEntry(List<string> dbLogbookEntry)
		{
			bool flag = dbLogbookEntry.Count > 0;
			if (flag)
			{
				uint.TryParse(dbLogbookEntry[0], NumberStyles.Integer, null, out this.iD);
			}
			bool flag2 = dbLogbookEntry.Count > 1;
			if (flag2)
			{
				DateTime.TryParse(dbLogbookEntry[1], new CultureInfo("de-DE"), DateTimeStyles.None, out this.entryTime);
			}
			bool flag3 = dbLogbookEntry.Count > 2;
			if (flag3)
			{
				this.Username = dbLogbookEntry[2];
			}
			bool flag4 = dbLogbookEntry.Count > 3;
			if (flag4)
			{
				byte b = 0;
				byte.TryParse(dbLogbookEntry[3], NumberStyles.HexNumber, null, out b);
				this.entryCode = (SSTEntryCode)b;
			}
			bool flag5 = dbLogbookEntry.Count > 4;
			if (flag5)
			{
				byte.TryParse(dbLogbookEntry[4], NumberStyles.HexNumber, null, out this.param1);
			}
			bool flag6 = dbLogbookEntry.Count > 5;
			if (flag6)
			{
				this.Param2 = dbLogbookEntry[5];
			}
			bool flag7 = dbLogbookEntry.Count > 6;
			if (flag7)
			{
				this.Param3 = dbLogbookEntry[6];
			}
			bool flag8 = dbLogbookEntry.Count > 7;
			if (flag8)
			{
				this.Param4 = dbLogbookEntry[7];
			}
			bool flag9 = dbLogbookEntry.Count > 8;
			if (flag9)
			{
				this.Param5 = dbLogbookEntry[8];
			}
			bool flag10 = dbLogbookEntry.Count > 9;
			if (flag10)
			{
				this.Param6 = dbLogbookEntry[9];
			}
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000AC0C0 File Offset: 0x000AA2C0
		public SSTLogbookEntry(uint id, DateTime date, string username, SSTEntryCode code, byte param1, string param2, string param3, string param4, string param5, string param6)
		{
			this.iD = id;
			this.entryTime = date;
			this.Username = username;
			this.entryCode = code;
			this.Param1 = param1;
			this.Param2 = param2;
			this.Param3 = param3;
			this.Param4 = param4;
			this.Param5 = param5;
			this.Param6 = param6;
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000AC140 File Offset: 0x000AA340
		public string GetSSTAnswerString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			int num = (int)this.entryCode;
			stringBuilder2.Append(num.ToString("X2"));
			stringBuilder.Append((this.entryTime.Year % 2000).ToString("X2"));
			stringBuilder.Append((this.entryTime.Month % 2000).ToString("X2"));
			stringBuilder.Append((this.entryTime.Day % 2000).ToString("X2"));
			stringBuilder.Append((this.entryTime.Hour % 2000).ToString("X2"));
			stringBuilder.Append((this.entryTime.Minute % 2000).ToString("X2"));
			stringBuilder.Append((this.entryTime.Second % 2000).ToString("X2"));
			string text = this.Username;
			bool flag = text.Length > 40;
			if (flag)
			{
				text = text.Substring(0, 40);
			}
			text = text.PadRight(40, ' ');
			stringBuilder.Append(text);
			stringBuilder.Append(this.param1.ToString("X2"));
			string text2 = this.Param2;
			bool flag2 = text2.Length > 40;
			if (flag2)
			{
				text2 = text2.Substring(0, 40);
			}
			text2 = text2.PadRight(40, ' ');
			string text3 = this.Param3;
			bool flag3 = text3.Length > 40;
			if (flag3)
			{
				text3 = text3.Substring(0, 40);
			}
			text3 = text3.PadRight(40, ' ');
			string text4 = this.Param4;
			bool flag4 = text4.Length > 40;
			if (flag4)
			{
				text4 = text4.Substring(0, 40);
			}
			text4 = text4.PadRight(40, ' ');
			string text5 = this.Param5;
			bool flag5 = text5.Length > 40;
			if (flag5)
			{
				text5 = text5.Substring(0, 40);
			}
			text5 = text5.PadRight(40, ' ');
			string text6 = this.Param6;
			bool flag6 = text6.Length > 10;
			if (flag6)
			{
				text6 = text6.Substring(0, 10);
			}
			text6 = text6.PadRight(10, ' ');
			stringBuilder.Append(text2);
			stringBuilder.Append(text3);
			stringBuilder.Append(text4);
			stringBuilder.Append(text5);
			stringBuilder.Append(text6);
			stringBuilder.Append(this.ID.ToString("X8"));
			return stringBuilder.ToString();
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000AC3E8 File Offset: 0x000AA5E8
		public void CreateLogbookText()
		{
			this.CreateLogbookText(null);
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000AC3F4 File Offset: 0x000AA5F4
		public void CreateLogbookText(DBConnection dbConnection)
		{
			bool flag = this.Username != "" && this.Username != "NoUser" && !this.noUserCodes.Contains((byte)this.entryCode);
			if (flag)
			{
				this.LogbookText = LocalizationManager.Localize("txtUserName") + ": " + this.Username + Environment.NewLine;
			}
			else
			{
				this.LogbookText = "";
			}
			bool flag2 = this.entryCode == SSTEntryCode.Metal;
			if (flag2)
			{
				this.SetLBMetalText(dbConnection);
			}
			bool flag3 = (byte)this.entryCode >= 2 && (byte)this.entryCode < 64;
			if (flag3)
			{
				this.SetLBErrorText();
			}
			else
			{
				bool flag4 = (byte)this.entryCode >= 64 && (byte)this.entryCode < 128;
				if (flag4)
				{
					this.SetLBWarningText();
				}
				else
				{
					bool flag5 = (byte)this.entryCode >= 128;
					if (flag5)
					{
						this.SetLBInfoText(dbConnection);
					}
				}
			}
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000AC4F8 File Offset: 0x000AA6F8
		private void SetLBMetalText(DBConnection dbConnection)
		{
			int num = 0;
			int num2 = 0;
			int.TryParse(this.Param2, out num);
			int.TryParse(this.Param3, out num2);
			bool flag = this.param1 == 0;
			if (flag)
			{
				this.LogbookText += ((this.entryCode != SSTEntryCode.ProductDuringTest) ? LocalizationManager.Localize("txtContaminationDetected") : LocalizationManager.Localize("txtProductDuringTest"));
				this.LogbookText = string.Concat(new string[]
				{
					this.LogbookText,
					(this.entryCode == SSTEntryCode.MetalDuringAudit) ? (" (" + LocalizationManager.Localize("txtDuringAudit") + ")") : "",
					Environment.NewLine,
					LocalizationManager.Localize("txtThroughput"),
					" ",
					num.ToString(),
					", ",
					LocalizationManager.Localize("txtReject"),
					" ",
					num2.ToString()
				});
			}
			else
			{
				bool flag2 = this.param1 == 1;
				if (flag2)
				{
					WeightUnit weightUnit = WeightUnit.Gramm;
					int num3 = 1;
					double num4 = 0.0;
					double num5 = 0.0;
					string text = "g";
					double.TryParse(this.Param4, out num4);
					double.TryParse(this.Param5, out num5);
					num4 /= 1000.0;
					num5 /= 1000.0;
					dbConnection.GetWeightUnit(ref weightUnit);
					bool flag3 = weightUnit == WeightUnit.Ounce;
					if (flag3)
					{
						num3 = 2;
						text = "oz";
						num4 *= XRSImageDefs.grammToOunceFactor;
						num5 *= XRSImageDefs.grammToOunceFactor;
					}
					this.LogbookText += ((this.entryCode != SSTEntryCode.ProductDuringTest) ? LocalizationManager.Localize("lbFalseweightDetected") : LocalizationManager.Localize("txtProductDuringTest"));
					this.LogbookText = string.Concat(new string[]
					{
						this.LogbookText,
						(this.entryCode == SSTEntryCode.MetalDuringAudit) ? (" (" + LocalizationManager.Localize("txtDuringAudit") + ")") : "",
						": ",
						LocalizationManager.Localize("txtCurrentWeight"),
						": ",
						num4.ToString("F" + num3.ToString(), LocalizationManager.CurrentCulture),
						" ",
						text,
						Environment.NewLine,
						LocalizationManager.Localize("txtWeight"),
						": ",
						num5.ToString("F" + num3.ToString(), LocalizationManager.CurrentCulture),
						" ",
						text,
						Environment.NewLine,
						LocalizationManager.Localize("txtThroughput"),
						" ",
						num.ToString(),
						", ",
						LocalizationManager.Localize("txtReject"),
						" ",
						num2.ToString()
					});
				}
				else
				{
					bool flag4 = this.param1 == 2;
					if (flag4)
					{
						int num6 = 0;
						int num7 = 0;
						int num8 = 0;
						int.TryParse(this.Param4, out num6);
						int.TryParse(this.Param5, out num7);
						int.TryParse(this.Param6, out num8);
						this.LogbookText += ((this.entryCode != SSTEntryCode.ProductDuringTest) ? LocalizationManager.Localize("lbFalseObjectCountDetected") : LocalizationManager.Localize("txtProductDuringTest"));
						this.LogbookText = string.Concat(new string[]
						{
							this.LogbookText,
							(this.entryCode == SSTEntryCode.MetalDuringAudit) ? (" (" + LocalizationManager.Localize("txtDuringAudit") + ")") : "",
							": ",
							LocalizationManager.Localize("txtCount"),
							": ",
							num6.ToString(),
							Environment.NewLine,
							LocalizationManager.Localize("txtMinCount"),
							": ",
							num7.ToString(),
							", ",
							LocalizationManager.Localize("txtMaxCount"),
							": ",
							num8.ToString(),
							Environment.NewLine,
							LocalizationManager.Localize("txtThroughput"),
							" ",
							num.ToString(),
							", ",
							LocalizationManager.Localize("txtReject"),
							" ",
							num2.ToString()
						});
					}
					else
					{
						bool flag5 = this.param1 == 3;
						if (flag5)
						{
							this.LogbookText = string.Concat(new string[]
							{
								this.LogbookText,
								LocalizationManager.Localize("lbProductNotRecognized"),
								Environment.NewLine,
								LocalizationManager.Localize("txtThroughput"),
								" ",
								num.ToString(),
								", ",
								LocalizationManager.Localize("txtReject"),
								" ",
								num2.ToString()
							});
						}
						else
						{
							bool flag6 = this.param1 == 4;
							if (flag6)
							{
								this.LogbookText = string.Concat(new string[]
								{
									this.LogbookText,
									LocalizationManager.Localize("txtWrongImageSizeDetected"),
									Environment.NewLine,
									LocalizationManager.Localize("txtThroughput"),
									" ",
									num.ToString(),
									", ",
									LocalizationManager.Localize("txtReject"),
									" ",
									num2.ToString()
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000ACAD4 File Offset: 0x000AACD4
		private void SetLBErrorText()
		{
			int num = 0;
			int num2 = 0;
			this.LogbookText = this.LogbookText + LocalizationManager.Localize("txtError") + ": ";
			switch (this.entryCode)
			{
			case SSTEntryCode.DetectorComm:
				this.MessageText = LocalizationManager.Localize("lbDetectorCommError");
				break;
			case SSTEntryCode.CalibrationFault:
			{
				int num3 = 0;
				int num4 = 0;
				int.TryParse(this.Param2, out num3);
				int.TryParse(this.Param3, out num4);
				this.MessageText = string.Concat(new string[]
				{
					"CalibrationError",
					Environment.NewLine,
					"Limit: ",
					num3.ToString(),
					", Value: ",
					num4.ToString()
				});
				break;
			}
			case SSTEntryCode.XRayComm:
				this.MessageText = LocalizationManager.Localize("lbXRayCommError");
				break;
			case SSTEntryCode.XRayFault:
			{
				int num5 = (int)this.param1;
				string[] array = Definition.xraySpellmanFaults;
				num = (int)this.param1;
				bool flag = this.param1 > 7;
				if (flag)
				{
					num5 -= 8;
					array = Definition.xrayVJFaults;
				}
				this.MessageText = LocalizationManager.Localize("parItemXRaySource") + ": " + array[num5];
				break;
			}
			case SSTEntryCode.ProcessingDelay:
				this.MessageText = LocalizationManager.Localize("lbProcessingDelay");
				break;
			case SSTEntryCode.MemoryUsage:
				this.MessageText = LocalizationManager.Localize("lbMemoryUsage");
				break;
			case SSTEntryCode.MotorError:
			{
				bool flag2 = this.Param2 != null && this.Param2 != "";
				if (flag2)
				{
					this.MessageText = LocalizationManager.Localize("parItemErrorEngine") + this.Param2;
				}
				else
				{
					this.MessageText = LocalizationManager.Localize("parItemErrorEngine");
				}
				break;
			}
			case SSTEntryCode.LowAirPressure:
				this.MessageText = LocalizationManager.Localize("parItemLowAirPressure");
				break;
			case SSTEntryCode.CoolingError:
				this.MessageText = LocalizationManager.Localize("parItemErrorCooling");
				break;
			case SSTEntryCode.BinFull:
				this.MessageText = LocalizationManager.Localize("parItemBinFull" + ((int)(this.param1 - 1)).ToString());
				num = (int)this.param1;
				break;
			case SSTEntryCode.ParamComm:
				this.MessageText = LocalizationManager.Localize("msgErrorComParamServer");
				break;
			case SSTEntryCode.ImageComm:
				this.MessageText = LocalizationManager.Localize("msgErrorComImgServer");
				break;
			case SSTEntryCode.IOComm:
				this.MessageText = LocalizationManager.Localize("lbWAGOCommError");
				break;
			case SSTEntryCode.IOController:
			{
				bool flag3 = this.Param1 == 0;
				if (flag3)
				{
					this.MessageText = string.Concat(new string[]
					{
						LocalizationManager.Localize("lbWAGOCommError"),
						Environment.NewLine,
						"Error code: ",
						this.Param2,
						", Error argument: ",
						this.Param3
					});
					int.TryParse(this.Param2, out num);
					int.TryParse(this.Param3, out num2);
				}
				else
				{
					bool flag4 = this.Param1 == 1;
					if (flag4)
					{
						int num6 = 0;
						int num7 = -1;
						int.TryParse(this.Param2, out num6);
						int.TryParse(this.Param3, out num7);
						bool flag5 = num6 == 5 && num7 >= 0;
						if (flag5)
						{
							string str = LocalizationManager.Localize("txtErrorAnalogInput");
							bool flag6 = num7 == 0;
							if (flag6)
							{
								this.MessageText = str + " 00.";
							}
							else
							{
								bool flag7 = num7 == 1;
								if (flag7)
								{
									this.MessageText = str + " 01.";
								}
								else
								{
									bool flag8 = num7 == 2;
									if (flag8)
									{
										this.MessageText = str + " 02.";
									}
									else
									{
										bool flag9 = num7 == 3;
										if (flag9)
										{
											this.MessageText = str + " 03.";
										}
									}
								}
							}
						}
						else
						{
							bool flag10 = num6 == 4 && num7 >= 0;
							if (flag10)
							{
								string str2 = LocalizationManager.Localize("txtErrorDigitalOutput");
								bool flag11 = num7 == 0;
								if (flag11)
								{
									this.MessageText = str2 + " 00 - 07.";
								}
								else
								{
									bool flag12 = num7 == 1;
									if (flag12)
									{
										this.MessageText = str2 + " 08.";
									}
									else
									{
										bool flag13 = num7 == 2;
										if (flag13)
										{
											this.MessageText = str2 + " 09.";
										}
										else
										{
											bool flag14 = num7 == 3;
											if (flag14)
											{
												this.MessageText = str2 + " 10.";
											}
											else
											{
												bool flag15 = num7 == 4;
												if (flag15)
												{
													this.MessageText = str2 + " 11.";
												}
												else
												{
													bool flag16 = num7 == 5;
													if (flag16)
													{
														this.MessageText = str2 + " 16 - 23.";
													}
													else
													{
														bool flag17 = num7 == 6;
														if (flag17)
														{
															this.MessageText = str2 + " 24.";
														}
														else
														{
															bool flag18 = num7 == 7;
															if (flag18)
															{
																this.MessageText = str2 + " 25.";
															}
															else
															{
																bool flag19 = num7 == 8;
																if (flag19)
																{
																	this.MessageText = str2 + " 26.";
																}
																else
																{
																	bool flag20 = num7 == 9;
																	if (flag20)
																	{
																		this.MessageText = str2 + " 27.";
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
							else
							{
								bool flag21 = num6 == 6;
								if (flag21)
								{
									this.MessageText = LocalizationManager.Localize("txtErrorSecondaryComm");
								}
							}
						}
					}
				}
				break;
			}
			case SSTEntryCode.RejectVerify:
			{
				int num8 = 0;
				int.TryParse(this.Param2, out num8);
				this.MessageText = string.Concat(new string[]
				{
					"Rejecter ",
					this.param1.ToString(),
					" Verify: ",
					num8.ToString(),
					" ",
					LocalizationManager.Localize("txtMissingProducts")
				});
				num = (int)this.param1;
				break;
			}
			case SSTEntryCode.XRayLamp:
				this.MessageText = LocalizationManager.Localize("msgLampCurrentLow");
				break;
			case SSTEntryCode.Temperature:
				this.MessageText = LocalizationManager.Localize("parItemTempSensor" + this.param1.ToString()) + ": " + LocalizationManager.Localize("msgTempTooHigh");
				break;
			case SSTEntryCode.Voltage:
			{
				string resourceKey = (this.param1 == 0) ? "parItemVoltage12V" : "parItemVoltage24V";
				string resourceKey2 = (this.Param2 == "0") ? "msgVoltageTooLow" : "msgVoltageTooHigh";
				num = (int)(this.param1 + 1);
				int.TryParse(this.Param2, out num2);
				this.MessageText = LocalizationManager.Localize(resourceKey) + ": " + LocalizationManager.Localize(resourceKey2);
				break;
			}
			case SSTEntryCode.ImageProcPC:
				this.MessageText = LocalizationManager.Localize("lbImageProcessingPcOffline");
				break;
			case SSTEntryCode.LightBarrier:
				this.MessageText = LocalizationManager.Localize("parItemLightBarrier" + ((int)(this.param1 - 1)).ToString());
				num = (int)this.param1;
				break;
			case SSTEntryCode.SVTComm:
				this.MessageText = LocalizationManager.Localize("lbSVTCommError");
				break;
			case SSTEntryCode.SVTError:
			{
				this.MessageText = LocalizationManager.Localize("lbSVTFault") + ": ";
				bool flag22 = this.param1 > 0 && (int)this.param1 <= Definition.svtFaults.Length;
				if (flag22)
				{
					this.MessageText += Definition.svtFaults[(int)(this.param1 - 1)];
					num = (int)this.param1;
				}
				break;
			}
			case SSTEntryCode.XRayMotor:
			{
				string resourceKey3 = (this.param1 == 0) ? "lbSshOvercurrent" : "lbSShError";
				this.MessageText = LocalizationManager.Localize(resourceKey3);
				num = (int)(this.param1 + 1);
				break;
			}
			case SSTEntryCode.AuditFailedError:
				this.MessageText = LocalizationManager.Localize("menuItemAuditCheck") + ": " + LocalizationManager.Localize("txtTestFailed");
				break;
			case SSTEntryCode.XrayArcs:
				this.MessageText = LocalizationManager.Localize("parItemXRaySource") + ": " + LocalizationManager.Localize("txtSeveralArcs");
				break;
			case SSTEntryCode.ProductsMissing:
				this.MessageText = "Rejecter " + ((int)(this.param1 + 1)).ToString() + ": " + LocalizationManager.Localize("lbProductsMissing");
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					Environment.NewLine,
					LocalizationManager.Localize("txtNumProductsMissing"),
					": ",
					this.Param2
				});
				num = (int)(this.param1 + 1);
				break;
			case SSTEntryCode.SensorBlockedPhotogating:
				this.MessageText = "Rejecter " + ((int)(this.param1 + 1)).ToString() + ": " + LocalizationManager.Localize("lbSensorBlockedPhotogating");
				num = (int)(this.param1 + 1);
				break;
			case SSTEntryCode.SensorBlockedRejectCheck:
				this.MessageText = "Rejecter " + ((int)(this.param1 + 1)).ToString() + ": " + LocalizationManager.Localize("lbSensorBlockedRejectCheck");
				num = (int)(this.param1 + 1);
				break;
			case SSTEntryCode.ErrorConsistency:
				this.MessageText = "Rejecter " + ((int)(this.param1 + 1)).ToString() + ": " + LocalizationManager.Localize("lbErrorConsistency");
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					Environment.NewLine,
					LocalizationManager.Localize("txtCountsPhotogatingSensor"),
					": ",
					this.Param2
				});
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					Environment.NewLine,
					LocalizationManager.Localize("txtCountsRejectCheckSensor"),
					": ",
					this.Param3
				});
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					Environment.NewLine,
					LocalizationManager.Localize("txtCountsRejectVerifySensor"),
					": ",
					this.Param4
				});
				num = (int)(this.param1 + 1);
				break;
			case SSTEntryCode.BinMissing:
				this.MessageText = string.Concat(new string[]
				{
					"Rejecter ",
					((int)(this.param1 + 1)).ToString(),
					": ",
					LocalizationManager.Localize("lbBinMissing"),
					"."
				});
				num = (int)(this.param1 + 1);
				break;
			case SSTEntryCode.ProductNotInspected:
				this.MessageText = "Rejecter " + ((int)(this.param1 + 1)).ToString() + ": " + LocalizationManager.Localize("lbProductNotInspected");
				num = (int)(this.param1 + 1);
				break;
			case SSTEntryCode.TooManyRejects:
				this.MessageText = LocalizationManager.Localize("txtTooManyRejects");
				break;
			case SSTEntryCode.InsightNetConnection:
				this.MessageText = LocalizationManager.Localize("lbInsightNetConnectionBroken");
				break;
			case SSTEntryCode.DiskSpaceLow:
				this.MessageText = LocalizationManager.Localize("lbDiskSpaceLow");
				break;
			case SSTEntryCode.InOutletSensorBlocked:
				this.MessageText = ((this.param1 == 0) ? (LocalizationManager.Localize("txtInletSensorBlocked") + ".") : (LocalizationManager.Localize("txtOutletSensorBlocked") + "."));
				num = (int)(this.param1 + 1);
				break;
			case SSTEntryCode.MeanValueHELEforDual:
				this.MessageText = LocalizationManager.Localize("txtMeanValueHELE");
				break;
			case SSTEntryCode.ErrorXRayOperatingHours:
				this.MessageText = string.Concat(new string[]
				{
					LocalizationManager.Localize("txtLimitXRayOperatingHours"),
					" ",
					this.Param2,
					" h ",
					LocalizationManager.Localize("txtErrorLimitXRayOperatingHours"),
					" ",
					this.Param3,
					" h."
				});
				break;
			case SSTEntryCode.Printer:
			{
				string str3 = "Fehler Drucker: ";
				Definition.PrinterType printerType = (Definition.PrinterType)this.param1;
				this.MessageText = str3 + printerType.ToString();
				break;
			}
			case SSTEntryCode.SafetySwitchOpenError:
			{
				bool flag23 = this.param1 != 16;
				if (flag23)
				{
					this.MessageText = this.MessageText + LocalizationManager.Localize("lbSafetySwitchOpen") + ": ";
				}
				bool flag24 = (int)this.param1 < SSTLogbookEntry.StrLocalizedSafetySwitches.Length;
				if (flag24)
				{
					this.MessageText += LocalizationManager.Localize(SSTLogbookEntry.StrLocalizedSafetySwitches[(int)this.param1]);
				}
				break;
			}
			case SSTEntryCode.EmergencyStopPressedError:
			{
				bool flag25 = this.Param2 != null && this.Param2 != "";
				if (flag25)
				{
					bool flag26 = this.Param2 == "1";
					if (flag26)
					{
						this.MessageText += LocalizationManager.Localize("parItemEmergencyStopPressed");
					}
					else
					{
						bool flag27 = this.Param2 == "2";
						if (flag27)
						{
							this.MessageText += LocalizationManager.Localize("parItemExternalEmergencyStopPressed");
						}
						else
						{
							bool flag28 = this.Param2 == "3";
							if (flag28)
							{
								this.MessageText = this.MessageText + LocalizationManager.Localize("parItemEmergencyStopPressed") + ". " + LocalizationManager.Localize("parItemExternalEmergencyStopPressed");
							}
						}
					}
				}
				else
				{
					this.MessageText += LocalizationManager.Localize("parItemEmergencyStopPressed");
				}
				break;
			}
			case SSTEntryCode.ErrorActSensTestcardSim:
			{
				string text = "";
				string text2 = "";
				bool flag29 = this.Param1 == 1;
				if (flag29)
				{
					text2 = LocalizationManager.Localize("txtChangedTestcardSim");
				}
				else
				{
					bool flag30 = this.Param1 == 2;
					if (flag30)
					{
						text2 = LocalizationManager.Localize("txtNotChangedTestcardSim");
						text = LocalizationManager.Localize("txtComplianceMsg");
					}
					else
					{
						bool flag31 = this.Param1 == 3;
						if (flag31)
						{
							text2 = LocalizationManager.Localize("txtChangedTestcardSim");
							text = LocalizationManager.Localize("txtComplianceMsg");
						}
						else
						{
							bool flag32 = this.Param1 == 4;
							if (flag32)
							{
								text2 = LocalizationManager.Localize("txtNotChangedTestcardSim");
								text = LocalizationManager.Localize("txtNoComplianceMsg");
							}
							else
							{
								bool flag33 = this.Param1 == 5;
								if (flag33)
								{
									text2 = LocalizationManager.Localize("txtChangedTestcardSim");
									text = LocalizationManager.Localize("txtNoComplianceMsg");
								}
							}
						}
					}
					text = string.Concat(new string[]
					{
						text,
						".",
						Environment.NewLine,
						LocalizationManager.Localize("txtThreshold"),
						" ",
						LocalizationManager.Localize("txtVA"),
						": ",
						this.Param4,
						" ",
						LocalizationManager.Localize("txtThreshold"),
						" ",
						LocalizationManager.Localize("txtGlass"),
						": ",
						this.Param5
					});
				}
				this.MessageText = string.Concat(new string[]
				{
					text2,
					": ",
					Environment.NewLine,
					LocalizationManager.Localize("txtVA"),
					": ",
					this.Param2,
					"  ",
					LocalizationManager.Localize("txtGlass"),
					": ",
					this.Param3,
					Environment.NewLine,
					text
				});
				break;
			}
			case SSTEntryCode.LimitsError:
			{
				bool flag34 = this.Param3.Equals("");
				if (flag34)
				{
					bool flag35 = this.Param1 > 0;
					if (flag35)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				else
				{
					bool flag36 = this.Param1 > 0;
					if (flag36)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				break;
			}
			case SSTEntryCode.BackgroundMonitoringError:
			{
				bool flag37 = this.Param1 == 0;
				if (flag37)
				{
					this.MessageText = string.Concat(new string[]
					{
						LocalizationManager.Localize("txtBeltContamination"),
						" ",
						LocalizationManager.Localize("txtLimitReached"),
						": ",
						LocalizationManager.Localize(this.Param2),
						Environment.NewLine,
						LocalizationManager.Localize("txtLimitMonitoring"),
						": ",
						this.Param4,
						", ",
						LocalizationManager.Localize("txtCurrentValue"),
						": ",
						this.Param5
					});
				}
				else
				{
					bool flag38 = this.Param1 == 1;
					if (flag38)
					{
						this.MessageText = LocalizationManager.Localize("txtBackgroundMonitoring") + ": " + LocalizationManager.Localize(this.Param2);
					}
					else
					{
						bool flag39 = this.Param1 == 2;
						if (flag39)
						{
							this.MessageText = string.Concat(new string[]
							{
								LocalizationManager.Localize("txtBeltContamination"),
								": ",
								Environment.NewLine,
								LocalizationManager.Localize("txtLimitMonitoring"),
								": ",
								this.Param4,
								", ",
								LocalizationManager.Localize("txtMarkedPixel"),
								": ",
								this.Param5
							});
						}
					}
				}
				break;
			}
			case SSTEntryCode.AIDeploymentError:
			{
				bool flag40 = this.Param1 == 0;
				if (flag40)
				{
					this.MessageText += LocalizationManager.Localize("lbAnomalyDetectionDeploymentError");
				}
				else
				{
					bool flag41 = this.Param1 == 1;
					if (flag41)
					{
						this.MessageText += LocalizationManager.Localize("lbAnomalyDetectionCorruptedFileTrain");
					}
					else
					{
						bool flag42 = this.Param1 == 2;
						if (flag42)
						{
							this.MessageText += LocalizationManager.Localize("lbAnomalyDetectionCorruptedFileInference");
						}
						else
						{
							bool flag43 = this.Param1 == 3;
							if (flag43)
							{
								this.MessageText += LocalizationManager.Localize("lbAnomalyDetectionMissingFileInference");
							}
							else
							{
								bool flag44 = this.Param1 == 4;
								if (flag44)
								{
									this.MessageText += LocalizationManager.Localize("lbAnomalyDetectionMissingMachineSubType");
								}
							}
						}
					}
				}
				break;
			}
			case SSTEntryCode.AIInferenceError:
			{
				bool flag45 = this.Param1 == 0;
				if (flag45)
				{
					this.MessageText = LocalizationManager.Localize("lbAnomalyDetectionInferenceError");
				}
				else
				{
					bool flag46 = this.Param1 == 1;
					if (flag46)
					{
						this.MessageText = LocalizationManager.Localize("lbObjectDetectionInferenceError");
					}
				}
				break;
			}
			case SSTEntryCode.NoConnectionToExternalStorageLoc:
				this.MessageText = string.Concat(new string[]
				{
					LocalizationManager.Localize("txtNoExternalConnection"),
					": ",
					this.Param2,
					this.Param3,
					this.Param4
				});
				break;
			case SSTEntryCode.ImageNotSaveToExternalStorageLoc:
				this.MessageText = string.Concat(new string[]
				{
					LocalizationManager.Localize("txtNoImageTransferToExternalConnection"),
					": ",
					this.Param2,
					" --> ",
					this.Param3,
					this.Param4,
					this.Param5
				});
				break;
			case SSTEntryCode.PathExternalStorageLocTooLong:
				this.MessageText = LocalizationManager.Localize("txtPathExternalConnectionTooLong") + ": " + this.Param2 + " > 120";
				break;
			case SSTEntryCode.Limit100PercentExternalStorageLoc:
				this.MessageText = string.Concat(new string[]
				{
					LocalizationManager.Localize("txtLimit100ExternalStorage"),
					": ",
					this.Param2,
					this.Param3,
					this.Param4
				});
				break;
			}
			this.LogbookText += this.MessageText;
			string str4 = this.LogbookText;
			string newLine = Environment.NewLine;
			string str5 = "E";
			int num9 = (int)this.entryCode;
			this.LogbookText = str4 + newLine + str5 + num9.ToString("D3");
			bool flag47 = num > 0;
			if (flag47)
			{
				this.LogbookText = this.LogbookText + "; A" + num.ToString("D3");
			}
			bool flag48 = num2 > 0;
			if (flag48)
			{
				this.LogbookText = this.LogbookText + "; A" + num2.ToString("D3");
			}
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000AE1B0 File Offset: 0x000AC3B0
		private void SetLBWarningText()
		{
			this.LogbookText = this.LogbookText + LocalizationManager.Localize("txtWarning") + ": ";
			switch (this.entryCode)
			{
			case SSTEntryCode.DatabaseVersion:
				this.MessageText += LocalizationManager.Localize("lbDatabaseOutOfDate");
				break;
			case SSTEntryCode.WagoOutOfDate:
				this.MessageText += LocalizationManager.Localize("msgWagoOutOfDate");
				break;
			case SSTEntryCode.WagoVersionWrong:
				this.MessageText += LocalizationManager.Localize("msgWagoWrongVersion");
				break;
			case SSTEntryCode.VibroInManualMode:
				this.MessageText += LocalizationManager.Localize("msgVibroInManualMode");
				break;
			case SSTEntryCode.WarningXRayOperatingHours:
				this.MessageText = string.Concat(new string[]
				{
					LocalizationManager.Localize("txtLimitXRayOperatingHours"),
					" ",
					this.Param2,
					" h ",
					LocalizationManager.Localize("txtWarningLimitXRayOperatingHours"),
					" ",
					this.Param3,
					" h."
				});
				break;
			case SSTEntryCode.WarningActSensTestcardSim:
			{
				string text = "";
				string text2 = "";
				bool flag = this.Param1 == 1;
				if (flag)
				{
					text2 = LocalizationManager.Localize("txtChangedTestcardSim");
				}
				else
				{
					bool flag2 = this.Param1 == 2;
					if (flag2)
					{
						text2 = LocalizationManager.Localize("txtNotChangedTestcardSim");
						text = Environment.NewLine + LocalizationManager.Localize("txtComplianceMsg");
					}
					else
					{
						bool flag3 = this.Param1 == 3;
						if (flag3)
						{
							text2 = LocalizationManager.Localize("txtChangedTestcardSim");
							text = Environment.NewLine + LocalizationManager.Localize("txtComplianceMsg");
						}
						else
						{
							bool flag4 = this.Param1 == 4;
							if (flag4)
							{
								text2 = LocalizationManager.Localize("txtNotChangedTestcardSim");
								text = Environment.NewLine + LocalizationManager.Localize("txtNoComplianceMsg");
							}
							else
							{
								bool flag5 = this.Param1 == 5;
								if (flag5)
								{
									text2 = LocalizationManager.Localize("txtChangedTestcardSim");
									text = Environment.NewLine + LocalizationManager.Localize("txtNoComplianceMsg");
								}
							}
						}
					}
					text = string.Concat(new string[]
					{
						text,
						".",
						Environment.NewLine,
						LocalizationManager.Localize("txtThreshold"),
						" ",
						LocalizationManager.Localize("txtVA"),
						": ",
						this.Param4,
						" ",
						LocalizationManager.Localize("txtThreshold"),
						" ",
						LocalizationManager.Localize("txtGlass"),
						": ",
						this.Param5
					});
				}
				this.MessageText = string.Concat(new string[]
				{
					text2,
					": ",
					Environment.NewLine,
					LocalizationManager.Localize("txtVA"),
					": ",
					this.Param2,
					"  ",
					LocalizationManager.Localize("txtGlass"),
					": ",
					this.Param3,
					text
				});
				break;
			}
			case SSTEntryCode.IVPCancelled:
				this.MessageText += LocalizationManager.Localize("txtIVPCancelled");
				break;
			case SSTEntryCode.IVPNeeded:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtIVPNeeded"),
					" ",
					this.Param2,
					"/",
					this.Param3,
					"/",
					this.Param4
				});
				break;
			case SSTEntryCode.LimitsWarning:
			{
				bool flag6 = this.Param3.Equals("");
				if (flag6)
				{
					bool flag7 = this.Param1 > 0;
					if (flag7)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				else
				{
					bool flag8 = this.Param1 > 0;
					if (flag8)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				break;
			}
			case SSTEntryCode.Limits90PerWarning:
			{
				bool flag9 = this.Param3.Equals("");
				if (flag9)
				{
					bool flag10 = this.Param1 > 0;
					if (flag10)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				else
				{
					bool flag11 = this.Param1 > 0;
					if (flag11)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				break;
			}
			case SSTEntryCode.BackgroundMonitoringWarning:
			{
				bool flag12 = this.Param1 == 0;
				if (flag12)
				{
					this.MessageText = string.Concat(new string[]
					{
						LocalizationManager.Localize("txtBackgroundMonitoring"),
						" ",
						LocalizationManager.Localize("txtLimitReached"),
						": ",
						LocalizationManager.Localize(this.Param2),
						Environment.NewLine,
						LocalizationManager.Localize("txtLimitMonitoring"),
						": ",
						this.Param4,
						", ",
						LocalizationManager.Localize("txtCurrentValue"),
						": ",
						this.Param5
					});
				}
				else
				{
					bool flag13 = this.Param1 == 0;
					if (flag13)
					{
						this.MessageText = LocalizationManager.Localize("txtBackgroundMonitoring") + ": " + LocalizationManager.Localize(this.Param2);
					}
					else
					{
						bool flag14 = this.Param1 == 2;
						if (flag14)
						{
							this.MessageText = string.Concat(new string[]
							{
								LocalizationManager.Localize("txtBeltContamination"),
								": ",
								Environment.NewLine,
								LocalizationManager.Localize("txtLimitMonitoring"),
								": ",
								this.Param4,
								", ",
								LocalizationManager.Localize("txtMarkedPixel"),
								": ",
								this.Param5
							});
						}
					}
				}
				break;
			}
			case SSTEntryCode.ElinosVersion:
				this.MessageText += LocalizationManager.Localize("lbElinosOutOfDate");
				break;
			case SSTEntryCode.Limit90PercentExternalStorageLoc:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtLimit90ExternalStorage"),
					": ",
					this.Param2,
					this.Param3,
					this.Param4
				});
				break;
			}
			this.LogbookText += this.MessageText;
			string str = this.LogbookText;
			string newLine = Environment.NewLine;
			string str2 = "W";
			int num = (int)this.entryCode;
			this.LogbookText = str + newLine + str2 + num.ToString("D3");
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000AED6C File Offset: 0x000ACF6C
		private void SetLBInfoText(DBConnection dbConnection)
		{
			DateTime dateTime = DateTime.MinValue;
			switch (this.entryCode)
			{
			case SSTEntryCode.MachineTurnedOn:
				this.MessageText += LocalizationManager.Localize("lbMachineTurnedOn");
				break;
			case SSTEntryCode.MachineTurnedOff:
				this.MessageText += LocalizationManager.Localize("lbMachineTurnedOff");
				break;
			case SSTEntryCode.ProductChanged:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtPerProductChange"),
					": P",
					this.Param2,
					" ",
					this.Param3,
					" --> P",
					this.Param4,
					" ",
					this.Param5
				});
				break;
			case SSTEntryCode.ProductEnded:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtProductEnded"),
					": P",
					this.Param2,
					" ",
					this.Param3,
					": ",
					LocalizationManager.Localize("txtThroughput"),
					": ",
					this.Param4,
					", ",
					LocalizationManager.Localize("txtReject"),
					": ",
					this.Param5
				});
				break;
			case SSTEntryCode.ProductCreated:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbNewProductCreated"),
					" P",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.ProductCopied:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbProductCopied"),
					" P",
					this.Param2,
					" ",
					this.Param3,
					" --> P",
					this.Param4,
					" ",
					this.Param5
				});
				break;
			case SSTEntryCode.ProductDeleted:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbProductDeleted"),
					" P",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.ProductRenamed:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbProductRenamed"),
					" P",
					this.Param2,
					" ",
					this.Param3,
					" --> P",
					this.Param2,
					" ",
					this.Param4
				});
				break;
			case SSTEntryCode.ProductNumberChanged:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbProductNumberChanged"),
					" P",
					this.Param2,
					" ",
					this.Param3,
					" --> P",
					this.Param4,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.SafetySwitchOpenInfo:
			{
				bool flag = this.param1 < 39 || this.param1 > 43;
				if (flag)
				{
					bool flag2 = this.param1 != 16;
					if (flag2)
					{
						this.MessageText = this.MessageText + LocalizationManager.Localize("lbSafetySwitchOpen") + ": ";
					}
					bool flag3 = (int)this.param1 < SSTLogbookEntry.StrLocalizedSafetySwitches.Length;
					if (flag3)
					{
						this.MessageText += LocalizationManager.Localize(SSTLogbookEntry.StrLocalizedSafetySwitches[(int)this.param1]);
					}
				}
				else
				{
					bool flag4 = this.param1 >= 39 && this.param1 <= 43;
					if (flag4)
					{
						this.MessageText += LocalizationManager.Localize(SSTLogbookEntry.StrLocalizedSafetySwitches[(int)this.param1]);
						bool flag5 = this.param1 == 39;
						if (flag5)
						{
							this.MessageText += ": DI27";
						}
						bool flag6 = this.param1 == 40;
						if (flag6)
						{
							this.MessageText += ": DI28";
						}
						bool flag7 = this.param1 == 41;
						if (flag7)
						{
							this.MessageText += ": DI29";
						}
						bool flag8 = this.param1 == 42;
						if (flag8)
						{
							this.MessageText += ": DI30";
						}
						bool flag9 = this.param1 == 43;
						if (flag9)
						{
							this.MessageText += ": DI31";
						}
					}
				}
				break;
			}
			case SSTEntryCode.EmergencyStopPressedInfo:
			{
				bool flag10 = this.Param2 != null && this.Param2 != "";
				if (flag10)
				{
					bool flag11 = this.Param2 == "1";
					if (flag11)
					{
						this.MessageText += LocalizationManager.Localize("parItemEmergencyStopPressed");
					}
					else
					{
						bool flag12 = this.Param2 == "2";
						if (flag12)
						{
							this.MessageText += LocalizationManager.Localize("parItemExternalEmergencyStopPressed");
						}
						else
						{
							bool flag13 = this.Param2 == "3";
							if (flag13)
							{
								this.MessageText = this.MessageText + LocalizationManager.Localize("parItemEmergencyStopPressed") + ". " + LocalizationManager.Localize("parItemExternalEmergencyStopPressed");
							}
						}
					}
				}
				else
				{
					this.MessageText += LocalizationManager.Localize("parItemEmergencyStopPressed");
				}
				break;
			}
			case SSTEntryCode.AutotrainStarted:
			{
				bool flag14 = this.Param1 == 1;
				if (flag14)
				{
					this.MessageText = this.MessageText + LocalizationManager.Localize("lbWeigher") + ": ";
				}
				this.MessageText += LocalizationManager.Localize("lbAutolearnStarted");
				break;
			}
			case SSTEntryCode.AutotrainEnded:
				this.MessageText += LocalizationManager.Localize("txtAfterAutotrain");
				break;
			case SSTEntryCode.AutotrainAborted:
				this.MessageText += LocalizationManager.Localize("txtAbortAutotrain");
				break;
			case SSTEntryCode.Retrain:
			{
				bool flag15 = this.param1 == 0;
				if (flag15)
				{
					this.MessageText += "Retrain: ";
				}
				else
				{
					this.MessageText += "Retrain region: ";
				}
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					"P",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			}
			case SSTEntryCode.ConveyorSpeed:
			case SSTEntryCode.RatedConveyorSpeed:
			{
				int num = 1;
				double num2 = 0.0;
				double num3 = 0.0;
				string text = "m / s";
				double.TryParse(this.Param2, out num2);
				double.TryParse(this.Param3, out num3);
				num2 /= 100.0;
				num3 /= 100.0;
				string text2 = string.Format("{0:0.00}", num2);
				string text3 = string.Format("{0:0.00}", num3);
				dbConnection.GetSpeedMetric(ref num);
				bool flag16 = num == 0;
				if (flag16)
				{
					text = "ft / min";
					num2 *= Definition.meterToFeetFactor * 60.0;
					num3 *= Definition.meterToFeetFactor * 60.0;
					text2 = string.Format("{0:0.0}", num2);
					text3 = string.Format("{0:0.0}", num3);
				}
				bool flag17 = num == 2;
				if (flag17)
				{
					text = "m / min";
					num2 *= 60.0;
					num3 *= 60.0;
					text2 = string.Format("{0:0.00}", num2);
					text3 = string.Format("{0:0.00}", num3);
				}
				bool flag18 = this.entryCode == SSTEntryCode.ConveyorSpeed;
				if (flag18)
				{
					this.MessageText += LocalizationManager.Localize("txtConveyorSpeed");
				}
				else
				{
					this.MessageText += LocalizationManager.Localize("txtRatedConveyorSpeed");
				}
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					": ",
					text2,
					" ",
					text,
					" --> ",
					text3,
					" ",
					text
				});
				break;
			}
			case SSTEntryCode.FilterActivated:
			{
				bool flag19 = this.Param1 == 0;
				if (flag19)
				{
					this.MessageText += LocalizationManager.Localize("lbEdgeFilterActivated");
				}
				else
				{
					bool flag20 = this.Param1 == 1;
					if (flag20)
					{
						this.MessageText += LocalizationManager.Localize("lbWeigherActivated");
					}
					else
					{
						bool flag21 = this.Param1 == 2;
						if (flag21)
						{
							this.MessageText += LocalizationManager.Localize("lbObjectCountActivated");
						}
						else
						{
							bool flag22 = this.Param1 == 3;
							if (flag22)
							{
								this.MessageText += LocalizationManager.Localize("lbIgnoreClipActivated");
							}
							else
							{
								bool flag23 = this.Param1 == 4;
								if (flag23)
								{
									this.MessageText += LocalizationManager.Localize("lbImageSizeAcitvated");
								}
							}
						}
					}
				}
				break;
			}
			case SSTEntryCode.FilterDeactivated:
			{
				bool flag24 = this.Param1 == 0;
				if (flag24)
				{
					this.MessageText += LocalizationManager.Localize("lbEdgeFilterDeactivated");
				}
				else
				{
					bool flag25 = this.Param1 == 1;
					if (flag25)
					{
						this.MessageText += LocalizationManager.Localize("lbWeigherDeactivated");
					}
					else
					{
						bool flag26 = this.Param1 == 2;
						if (flag26)
						{
							this.MessageText += LocalizationManager.Localize("lbObjectCountDeactivated");
						}
						else
						{
							bool flag27 = this.Param1 == 3;
							if (flag27)
							{
								this.MessageText += LocalizationManager.Localize("lbIgnoreClipDeactivated");
							}
							else
							{
								bool flag28 = this.Param1 == 4;
								if (flag28)
								{
									this.MessageText += LocalizationManager.Localize("lbImageSizeDeacitvated");
								}
							}
						}
					}
				}
				break;
			}
			case SSTEntryCode.EdgeParameterChanged:
			{
				this.LogbookText = this.LogbookText + LocalizationManager.Localize("lbEdgeFilter") + ": ";
				bool flag29 = this.Param1 == 0;
				if (flag29)
				{
					this.MessageText += LocalizationManager.Localize("txtNumLeftPixel");
				}
				else
				{
					bool flag30 = this.Param1 == 1;
					if (flag30)
					{
						this.MessageText += LocalizationManager.Localize("txtNumRightPixel");
					}
					else
					{
						bool flag31 = this.Param1 == 2;
						if (flag31)
						{
							this.MessageText += LocalizationManager.Localize("txtNumTopPixel");
						}
						else
						{
							bool flag32 = this.Param1 == 3;
							if (flag32)
							{
								this.MessageText += LocalizationManager.Localize("txtNumBottomPixel");
							}
							else
							{
								bool flag33 = this.Param1 == 4;
								if (flag33)
								{
									this.MessageText += LocalizationManager.Localize("txtThreshold");
								}
							}
						}
					}
				}
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					": ",
					this.Param2,
					" --> ",
					this.Param3
				});
				break;
			}
			case SSTEntryCode.SystemTimeChanged:
				this.MessageText += LocalizationManager.Localize("lbSystemTimeChanged");
				break;
			case SSTEntryCode.CounterParameterChanged:
			{
				this.MessageText = this.MessageText + LocalizationManager.Localize("lbObjectCounter") + ": ";
				bool flag34 = this.Param1 == 0;
				if (flag34)
				{
					this.MessageText += "Autotrain";
				}
				else
				{
					bool flag35 = this.Param1 == 1;
					if (flag35)
					{
						this.MessageText += LocalizationManager.Localize("txtMinCount");
					}
					else
					{
						bool flag36 = this.Param1 == 2;
						if (flag36)
						{
							this.MessageText += LocalizationManager.Localize("txtMaxCount");
						}
						else
						{
							bool flag37 = this.Param1 == 3;
							if (flag37)
							{
								this.MessageText += LocalizationManager.Localize("txtLowThreshold");
							}
							else
							{
								bool flag38 = this.Param1 == 4;
								if (flag38)
								{
									this.MessageText += LocalizationManager.Localize("txtHighThreshold");
								}
								else
								{
									bool flag39 = this.Param1 == 5;
									if (flag39)
									{
										this.MessageText += LocalizationManager.Localize("txtMinSize");
									}
									else
									{
										bool flag40 = this.Param1 == 6;
										if (flag40)
										{
											this.MessageText += LocalizationManager.Localize("txtMaxSize");
										}
										else
										{
											bool flag41 = this.Param1 == 7;
											if (flag41)
											{
												this.MessageText += LocalizationManager.Localize("txtFilterSize");
											}
											else
											{
												bool flag42 = this.Param1 == 8;
												if (flag42)
												{
													this.MessageText += LocalizationManager.Localize("txtObjectUnmaskForConEnabled");
												}
											}
										}
									}
								}
							}
						}
					}
				}
				bool flag43 = this.Param1 > 0;
				if (flag43)
				{
					this.MessageText = string.Concat(new string[]
					{
						this.MessageText,
						": ",
						this.Param2,
						" --> ",
						this.Param3
					});
				}
				break;
			}
			case SSTEntryCode.MetalDuringAudit:
				this.SetLBMetalText(dbConnection);
				break;
			case SSTEntryCode.XrayTurnedOn:
				this.MessageText += LocalizationManager.Localize("lbXRayTurnedOn");
				break;
			case SSTEntryCode.XrayTurnedOff:
				this.MessageText += LocalizationManager.Localize("lbXRayTurnedOff");
				break;
			case SSTEntryCode.WeigherParameterChanged:
			{
				WeightUnit weightUnit = WeightUnit.Gramm;
				int num4 = 1;
				double num5 = 0.0;
				double num6 = 0.0;
				string text4 = "g";
				double.TryParse(this.Param2, out num5);
				double.TryParse(this.Param3, out num6);
				num5 /= 1000.0;
				num6 /= 1000.0;
				dbConnection.GetWeightUnit(ref weightUnit);
				bool flag44 = weightUnit == WeightUnit.Ounce;
				if (flag44)
				{
					num4 = 2;
					text4 = "oz";
					num5 *= XRSImageDefs.grammToOunceFactor;
					num6 *= XRSImageDefs.grammToOunceFactor;
				}
				this.MessageText = this.MessageText + LocalizationManager.Localize("lbWeigher") + ": ";
				bool flag45 = this.Param1 == 0;
				if (flag45)
				{
					this.MessageText += LocalizationManager.Localize("txtWeight");
				}
				else
				{
					bool flag46 = this.Param1 == 1;
					if (flag46)
					{
						this.MessageText += LocalizationManager.Localize("txtMinimum");
					}
					else
					{
						bool flag47 = this.Param1 == 2;
						if (flag47)
						{
							this.MessageText += LocalizationManager.Localize("txtMaximum");
						}
					}
				}
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					": ",
					num5.ToString("F" + num4.ToString(), LocalizationManager.CurrentCulture),
					" ",
					text4,
					" --> ",
					num6.ToString("F" + num4.ToString(), LocalizationManager.CurrentCulture),
					" ",
					text4
				});
				break;
			}
			case SSTEntryCode.ParameterChanged:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					this.Param2,
					": ",
					this.Param3,
					" --> ",
					this.Param4
				});
				break;
			case SSTEntryCode.UserLoggedIn:
				this.MessageText = this.MessageText + LocalizationManager.Localize("lbUserLoggedIn") + ": " + this.Param2;
				break;
			case SSTEntryCode.UserLoggedOut:
				this.MessageText = this.MessageText + LocalizationManager.Localize("lbUserLoggedOut") + ": " + this.Param2;
				break;
			case SSTEntryCode.UserChanged:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbUserChange"),
					": ",
					this.Param2,
					" --> ",
					this.Param3
				});
				break;
			case SSTEntryCode.UserCreated:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbNewUserCreated"),
					": ID ",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.UserParameterChanged:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbUserParameterChanged"),
					": ID ",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.UserDeleted:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbUserDeleted"),
					": ID ",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.RetrainGroup:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					"Retrain group: P",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.RetrainGroupFailed:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					"Retrain group failed: P",
					this.Param2,
					" ",
					this.Param3
				});
				break;
			case SSTEntryCode.SensitivityChanged:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("lbSensitivityChanged"),
					": ",
					this.Param2,
					" --> ",
					this.Param3
				});
				break;
			case SSTEntryCode.AuditParameterChanged:
			{
				this.MessageText = this.MessageText + LocalizationManager.Localize("lbAuditCheck") + ": ";
				bool flag48 = this.Param1 == 0;
				if (flag48)
				{
					this.MessageText += LocalizationManager.Localize("txtAuditMode");
				}
				else
				{
					bool flag49 = this.Param1 == 1;
					if (flag49)
					{
						this.MessageText += LocalizationManager.Localize("txtAuditTimeHour");
					}
					else
					{
						bool flag50 = this.Param1 == 2;
						if (flag50)
						{
							this.MessageText += LocalizationManager.Localize("txtAuditTimeMinute");
						}
						else
						{
							bool flag51 = this.Param1 == 3;
							if (flag51)
							{
								this.MessageText += LocalizationManager.Localize("txtAuditDay");
							}
							else
							{
								bool flag52 = this.Param1 == 4;
								if (flag52)
								{
									this.MessageText += LocalizationManager.Localize("txtAuditIntervalHour");
								}
								else
								{
									bool flag53 = this.Param1 == 5;
									if (flag53)
									{
										this.MessageText += LocalizationManager.Localize("txtAuditIntervalMinute");
									}
									else
									{
										bool flag54 = this.Param1 == 6;
										if (flag54)
										{
											this.MessageText += LocalizationManager.Localize("txtTimeToStartAudit");
										}
										else
										{
											bool flag55 = this.Param1 == 7;
											if (flag55)
											{
												this.MessageText += LocalizationManager.Localize("txtNumContaminations");
											}
										}
									}
								}
							}
						}
					}
				}
				bool flag56 = this.Param1 == 0;
				if (flag56)
				{
					AlarmMode alarmMode = AlarmMode.PerHour;
					AlarmMode alarmMode2 = AlarmMode.PerHour;
					try
					{
						alarmMode = (AlarmMode)Enum.Parse(typeof(AlarmMode), this.Param2);
						alarmMode2 = (AlarmMode)Enum.Parse(typeof(AlarmMode), this.Param3);
					}
					catch
					{
					}
					this.MessageText = string.Concat(new string[]
					{
						this.MessageText,
						": ",
						alarmMode.ToString(),
						" --> ",
						alarmMode2.ToString()
					});
				}
				else
				{
					bool flag57 = this.Param1 == 3;
					if (flag57)
					{
						DayOfWeek dayOfWeek = DayOfWeek.Monday;
						DayOfWeek dayOfWeek2 = DayOfWeek.Monday;
						try
						{
							dayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), this.Param2);
							dayOfWeek2 = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), this.Param3);
						}
						catch
						{
						}
						this.MessageText = string.Concat(new string[]
						{
							this.MessageText,
							": ",
							LocalizationManager.CurrentCulture.DateTimeFormat.DayNames[(int)dayOfWeek],
							" --> ",
							LocalizationManager.CurrentCulture.DateTimeFormat.DayNames[(int)dayOfWeek2]
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							this.MessageText,
							": ",
							this.Param2,
							" --> ",
							this.Param3
						});
					}
				}
				break;
			}
			case SSTEntryCode.AuditActivated:
			{
				bool flag58 = this.Param1 == 0;
				if (flag58)
				{
					this.MessageText += LocalizationManager.Localize("lbAuditContaminationActivated");
				}
				else
				{
					bool flag59 = this.Param1 == 1;
					if (flag59)
					{
						this.MessageText += LocalizationManager.Localize("lbAuditWeightActivated");
					}
					else
					{
						bool flag60 = this.Param1 == 2;
						if (flag60)
						{
							this.MessageText += LocalizationManager.Localize("lbAuditObjectcountActivated");
						}
					}
				}
				break;
			}
			case SSTEntryCode.AuditDeactivated:
			{
				bool flag61 = this.Param1 == 0;
				if (flag61)
				{
					this.MessageText += LocalizationManager.Localize("lbAuditContaminationDeactivated");
				}
				else
				{
					bool flag62 = this.Param1 == 1;
					if (flag62)
					{
						this.MessageText += LocalizationManager.Localize("lbAuditWeightDeactivated");
					}
					else
					{
						bool flag63 = this.Param1 == 2;
						if (flag63)
						{
							this.MessageText += LocalizationManager.Localize("lbAuditObjectcountDeactivated");
						}
					}
				}
				break;
			}
			case SSTEntryCode.AuditNeeded:
				this.MessageText += LocalizationManager.Localize("txtAuditNeeded");
				break;
			case SSTEntryCode.AuditOK:
				this.MessageText = this.MessageText + LocalizationManager.Localize("menuItemAuditCheck") + ": " + LocalizationManager.Localize("txtTestOk");
				break;
			case SSTEntryCode.AuditFailedInfo:
				this.MessageText = this.MessageText + LocalizationManager.Localize("menuItemAuditCheck") + ": " + LocalizationManager.Localize("txtTestFailed");
				break;
			case SSTEntryCode.AuditStart:
				this.MessageText += LocalizationManager.Localize("lbAuditStarted");
				break;
			case SSTEntryCode.CheckpointRestored:
				try
				{
					dateTime = DateTime.Parse(this.Param2, new CultureInfo("en-GB"));
				}
				catch
				{
				}
				this.MessageText = this.MessageText + LocalizationManager.Localize("lbCheckpointRestored") + Environment.NewLine;
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtCheckpoint"),
					" ",
					this.param1.ToString(),
					": ",
					dateTime.ToString(LocalizationManager.CurrentCulture)
				});
				break;
			case SSTEntryCode.CheckpointDeleted:
				try
				{
					dateTime = DateTime.Parse(this.Param2, new CultureInfo("en-GB"));
				}
				catch
				{
				}
				this.MessageText = this.MessageText + LocalizationManager.Localize("txtCheckpointDeleted") + Environment.NewLine;
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtCheckpoint"),
					" ",
					this.param1.ToString(),
					": ",
					dateTime.ToString(LocalizationManager.CurrentCulture)
				});
				break;
			case SSTEntryCode.CheckpointCreated:
				try
				{
					dateTime = DateTime.Parse(this.Param2, new CultureInfo("en-GB"));
				}
				catch
				{
				}
				this.MessageText = this.MessageText + LocalizationManager.Localize("txtCheckpointCreated") + Environment.NewLine;
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtCheckpoint"),
					" ",
					this.param1.ToString(),
					": ",
					dateTime.ToString(LocalizationManager.CurrentCulture)
				});
				break;
			case SSTEntryCode.ProductDuringTest:
				this.SetLBMetalText(dbConnection);
				break;
			case SSTEntryCode.BatchEnded:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					"Batch ended: P",
					this.Param2,
					" ",
					this.Param3,
					" (",
					this.Param6,
					"): ",
					LocalizationManager.Localize("txtThroughput"),
					": ",
					this.Param4,
					", ",
					LocalizationManager.Localize("txtReject"),
					": ",
					this.Param5
				});
				break;
			case SSTEntryCode.WeightStat:
			{
				int num4 = 1;
				double num7 = 0.0;
				double num8 = 0.0;
				double num9 = 0.0;
				double num10 = 0.0;
				string text4 = "g";
				WeightUnit weightUnit = WeightUnit.Gramm;
				string text5 = "";
				double.TryParse(this.Param2, out num7);
				double.TryParse(this.Param3, out num8);
				double.TryParse(this.Param4, out num9);
				double.TryParse(this.Param5, out num10);
				num7 /= 1000.0;
				num8 /= 1000.0;
				num9 /= 1000.0;
				num10 /= 1000.0;
				dbConnection.GetWeightUnit(ref weightUnit);
				bool flag64 = weightUnit == WeightUnit.Ounce;
				if (flag64)
				{
					num4 = 2;
					text4 = "oz";
					num7 *= XRSImageDefs.grammToOunceFactor;
					num8 *= XRSImageDefs.grammToOunceFactor;
					num9 *= XRSImageDefs.grammToOunceFactor;
					num10 *= XRSImageDefs.grammToOunceFactor;
				}
				else
				{
					num8 /= 1000.0;
					text5 = "k";
				}
				string text6 = (this.param1 == 0) ? "Product reference weight: " : "Batch reference weight: ";
				string text7 = (this.param1 == 0) ? "Total product weight: " : "Total batch weight: ";
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					text6,
					num7.ToString("F" + num4.ToString(), LocalizationManager.CurrentCulture),
					" ",
					text4,
					Environment.NewLine,
					text7,
					num8.ToString("F" + num4.ToString(), LocalizationManager.CurrentCulture),
					" ",
					text5,
					text4,
					Environment.NewLine,
					"Average weight: ",
					num9.ToString("F" + num4.ToString(), LocalizationManager.CurrentCulture),
					" ",
					text4,
					Environment.NewLine,
					"Standard deviation: ",
					num10.ToString("F" + num4.ToString(), LocalizationManager.CurrentCulture),
					" ",
					text4
				});
				break;
			}
			case SSTEntryCode.BatchChanged:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtBatchChange"),
					": ",
					this.Param2,
					(!string.IsNullOrEmpty(this.Param2)) ? " --> " : "",
					this.Param3
				});
				break;
			case SSTEntryCode.MachineEnableExtern:
				this.MessageText += LocalizationManager.Localize("parItemMachineEnableExtern");
				break;
			case SSTEntryCode.InfoInitSensTestcardSim:
				this.MessageText = string.Concat(new string[]
				{
					LocalizationManager.Localize("txtInitTestcardSim"),
					": ",
					Environment.NewLine,
					LocalizationManager.Localize("txtVA"),
					": ",
					this.Param2,
					"  ",
					LocalizationManager.Localize("txtGlass"),
					": ",
					this.Param3
				});
				break;
			case SSTEntryCode.InfoActSensTestcardSim:
			{
				string text8 = "";
				string text9 = "";
				bool flag65 = this.Param1 == 1;
				if (flag65)
				{
					text9 = LocalizationManager.Localize("txtChangedTestcardSim");
				}
				else
				{
					bool flag66 = this.Param1 == 2;
					if (flag66)
					{
						text9 = LocalizationManager.Localize("txtNotChangedTestcardSim");
						text8 = LocalizationManager.Localize("txtComplianceMsg");
					}
					else
					{
						bool flag67 = this.Param1 == 3;
						if (flag67)
						{
							text9 = LocalizationManager.Localize("txtChangedTestcardSim");
							text8 = LocalizationManager.Localize("txtComplianceMsg");
						}
						else
						{
							bool flag68 = this.Param1 == 4;
							if (flag68)
							{
								text9 = LocalizationManager.Localize("txtNotChangedTestcardSim");
								text8 = LocalizationManager.Localize("txtNoComplianceMsg");
							}
							else
							{
								bool flag69 = this.Param1 == 5;
								if (flag69)
								{
									text9 = LocalizationManager.Localize("txtChangedTestcardSim");
									text8 = LocalizationManager.Localize("txtNoComplianceMsg");
								}
							}
						}
					}
					text8 = string.Concat(new string[]
					{
						text8,
						".",
						Environment.NewLine,
						LocalizationManager.Localize("txtThreshold"),
						" ",
						LocalizationManager.Localize("txtVA"),
						": ",
						this.Param4,
						" ",
						LocalizationManager.Localize("txtThreshold"),
						" ",
						LocalizationManager.Localize("txtGlass"),
						": ",
						this.Param5
					});
				}
				this.MessageText = string.Concat(new string[]
				{
					text9,
					": ",
					Environment.NewLine,
					LocalizationManager.Localize("txtVA"),
					": ",
					this.Param2,
					"  ",
					LocalizationManager.Localize("txtGlass"),
					": ",
					this.Param3,
					Environment.NewLine,
					text8
				});
				break;
			}
			case SSTEntryCode.IVPActivated:
				this.MessageText += LocalizationManager.Localize("txtIVPActivated");
				break;
			case SSTEntryCode.IVPDeactivated:
				this.MessageText += LocalizationManager.Localize("txtIVPDeactivated");
				break;
			case SSTEntryCode.IVPTestballFinished:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtIVPTestballFinished"),
					"\n",
					this.Param2,
					", ",
					this.Param3,
					"  --> ",
					LocalizationManager.Localize("txtFound"),
					": ",
					this.Param4,
					"/",
					this.Param6,
					", ",
					LocalizationManager.Localize("txtRejected"),
					": ",
					this.Param5,
					"/",
					this.Param6
				});
				break;
			case SSTEntryCode.IVPOK:
				this.MessageText = this.MessageText + LocalizationManager.Localize("txtValidationOk") + ": " + this.Param2;
				break;
			case SSTEntryCode.IVPFailed:
				this.MessageText = this.MessageText + LocalizationManager.Localize("txtValidationFailed") + ": " + this.Param2;
				break;
			case SSTEntryCode.IVPStarted:
				this.MessageText += LocalizationManager.Localize("txtIVPStarted");
				break;
			case SSTEntryCode.AuditTestballFinished:
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					LocalizationManager.Localize("txtAuditTestballFinished"),
					"\n",
					this.Param2,
					", ",
					this.Param3,
					"  --> ",
					LocalizationManager.Localize("txtFound"),
					": ",
					this.Param4,
					"/",
					this.Param5
				});
				break;
			case SSTEntryCode.UndoRetrain:
				this.MessageText += LocalizationManager.Localize("txtUndoRetrainLogbook");
				break;
			case SSTEntryCode.LimitsInfo:
			{
				bool flag70 = this.Param3.Equals("");
				if (flag70)
				{
					bool flag71 = this.Param1 > 0;
					if (flag71)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				else
				{
					bool flag72 = this.Param1 > 0;
					if (flag72)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimitReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				break;
			}
			case SSTEntryCode.Limits90PerInfo:
			{
				bool flag73 = this.Param3.Equals("");
				if (flag73)
				{
					bool flag74 = this.Param1 > 0;
					if (flag74)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				else
				{
					bool flag75 = this.Param1 > 0;
					if (flag75)
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							this.Param1.ToString(),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
					else
					{
						this.MessageText = string.Concat(new string[]
						{
							LocalizationManager.Localize("txtLimit90PercReached"),
							": ",
							LocalizationManager.Localize(this.Param2),
							" ",
							LocalizationManager.Localize(this.Param3),
							Environment.NewLine,
							LocalizationManager.Localize("txtLimitMonitoring"),
							": ",
							this.Param4,
							", ",
							LocalizationManager.Localize("txtCurrentValue"),
							": ",
							this.Param5
						});
					}
				}
				break;
			}
			case SSTEntryCode.MonitoringActivated:
				this.MessageText = LocalizationManager.Localize("txtMonitoringActivated");
				break;
			case SSTEntryCode.MonitoringDeactivated:
				this.MessageText = LocalizationManager.Localize("txtMonitoringDeactivated");
				break;
			case SSTEntryCode.BackgroundMonitoringInfo:
			{
				bool flag76 = this.Param1 == 0;
				if (flag76)
				{
					this.MessageText = string.Concat(new string[]
					{
						LocalizationManager.Localize("txtBackgroundMonitoring"),
						" ",
						LocalizationManager.Localize("txtLimitReached"),
						": ",
						LocalizationManager.Localize(this.Param2),
						Environment.NewLine,
						LocalizationManager.Localize("txtLimitMonitoring"),
						": ",
						this.Param4,
						", ",
						LocalizationManager.Localize("txtCurrentValue"),
						": ",
						this.Param5
					});
				}
				else
				{
					bool flag77 = this.Param1 == 1;
					if (flag77)
					{
						this.MessageText = LocalizationManager.Localize("txtBackgroundMonitoring") + ": " + LocalizationManager.Localize(this.Param2);
					}
					else
					{
						bool flag78 = this.Param1 == 2;
						if (flag78)
						{
							this.MessageText = string.Concat(new string[]
							{
								LocalizationManager.Localize("txtBeltContamination"),
								": ",
								Environment.NewLine,
								LocalizationManager.Localize("txtLimitMonitoring"),
								": ",
								this.Param4,
								", ",
								LocalizationManager.Localize("txtMarkedPixel"),
								": ",
								this.Param5
							});
						}
					}
				}
				break;
			}
			case SSTEntryCode.ICMsuccesful:
				this.MessageText = LocalizationManager.Localize("txtICMsuccessful");
				break;
			case SSTEntryCode.SortingSwitchedOn:
				this.MessageText = LocalizationManager.Localize("txtSortingSwitchedOn");
				break;
			case SSTEntryCode.SortingSwitchedOff:
				this.MessageText = LocalizationManager.Localize("txtSortingSwitchedOff");
				break;
			case SSTEntryCode.SafetyCircutEx2A:
				this.MessageText = LocalizationManager.Localize("txtSafetyCircutEx2A");
				break;
			case SSTEntryCode.AIDetectionActivated:
			{
				bool flag79 = this.Param1 == 0;
				if (flag79)
				{
					this.MessageText = string.Concat(new string[]
					{
						this.MessageText,
						LocalizationManager.Localize("lbAnomalyDetectionInferenceActivated"),
						": P",
						this.Param2,
						" ",
						this.Param3
					});
				}
				else
				{
					bool flag80 = this.Param1 == 1;
					if (flag80)
					{
						this.MessageText = string.Concat(new string[]
						{
							this.MessageText,
							LocalizationManager.Localize("lbAnomalyDetectionTrainActivated"),
							": P",
							this.Param2,
							" ",
							this.Param3
						});
					}
					else
					{
						bool flag81 = this.Param1 == 2;
						if (flag81)
						{
							this.MessageText = string.Concat(new string[]
							{
								this.MessageText,
								LocalizationManager.Localize("lbAnomalyDetectionTrainTransferData"),
								": P",
								this.Param2,
								" ",
								this.Param3
							});
						}
						else
						{
							bool flag82 = this.Param1 == 3;
							if (flag82)
							{
								this.MessageText = string.Concat(new string[]
								{
									this.MessageText,
									LocalizationManager.Localize("lbObjectDetectionInferenceActivated"),
									": P",
									this.Param2,
									" ",
									this.Param3
								});
							}
							else
							{
								bool flag83 = this.Param1 == 4;
								if (flag83)
								{
									this.MessageText = string.Concat(new string[]
									{
										this.MessageText,
										LocalizationManager.Localize("lbAnomalyDetectionRestoreModel"),
										": P",
										this.Param2,
										" ",
										this.Param3
									});
								}
							}
						}
					}
				}
				break;
			}
			case SSTEntryCode.AIDetectionDeactivated:
			{
				bool flag84 = this.Param1 == 0;
				if (flag84)
				{
					this.MessageText = string.Concat(new string[]
					{
						this.MessageText,
						LocalizationManager.Localize("lbAnomalyDetectionInferenceDeactivated"),
						": P",
						this.Param2,
						" ",
						this.Param3
					});
				}
				else
				{
					bool flag85 = this.Param1 == 1;
					if (flag85)
					{
						this.MessageText = string.Concat(new string[]
						{
							this.MessageText,
							LocalizationManager.Localize("lbAnomalyDetectionTrainDeactivated"),
							": P",
							this.Param2,
							" ",
							this.Param3
						});
					}
					else
					{
						bool flag86 = this.Param1 == 2;
						if (flag86)
						{
							this.MessageText = string.Concat(new string[]
							{
								this.MessageText,
								LocalizationManager.Localize("lbAnomalyDetectionTrainFinished"),
								": P",
								this.Param2,
								" ",
								this.Param3
							});
						}
						else
						{
							bool flag87 = this.Param1 == 3;
							if (flag87)
							{
								this.MessageText = string.Concat(new string[]
								{
									this.MessageText,
									LocalizationManager.Localize("lbObjectDetectionInferenceDeactivated"),
									": P",
									this.Param2,
									" ",
									this.Param3
								});
							}
							else
							{
								bool flag88 = this.Param1 == 4;
								if (flag88)
								{
									this.MessageText = string.Concat(new string[]
									{
										this.MessageText,
										LocalizationManager.Localize("lbAnomalyDetectionDeleteModel"),
										": P",
										this.Param2,
										" ",
										this.Param3
									});
								}
								else
								{
									bool flag89 = this.Param1 == 5;
									if (flag89)
									{
										this.MessageText = string.Concat(new string[]
										{
											this.MessageText,
											this.Param2,
											": P",
											this.Param3,
											" ",
											this.Param4,
											": ",
											LocalizationManager.Localize("txtBackupRestoredThink"),
											" ",
											this.Param5,
											": ",
											this.Param6
										});
									}
								}
							}
						}
					}
				}
				break;
			}
			case SSTEntryCode.ObjectDetectionParameterChanged:
			{
				this.MessageText = string.Concat(new string[]
				{
					this.MessageText,
					"THiNK XO: P",
					this.Param2,
					" ",
					this.Param3,
					": "
				});
				bool flag90 = this.Param1 == 0;
				if (flag90)
				{
					this.MessageText += "TrainToTrigger";
				}
				else
				{
					bool flag91 = this.Param1 == 1;
					if (flag91)
					{
						this.MessageText = string.Concat(new string[]
						{
							this.MessageText,
							"Filter ",
							this.Param4,
							": ",
							LocalizationManager.Localize("txtObjectClassEnabled")
						});
					}
					else
					{
						bool flag92 = this.Param1 == 2;
						if (flag92)
						{
							this.MessageText = string.Concat(new string[]
							{
								this.MessageText,
								"Filter ",
								this.Param4,
								": ",
								LocalizationManager.Localize("txtObjectDetectionType")
							});
						}
						else
						{
							bool flag93 = this.Param1 == 3;
							if (flag93)
							{
								this.MessageText = string.Concat(new string[]
								{
									this.MessageText,
									"Filter ",
									this.Param4,
									": ",
									LocalizationManager.Localize("txtObjectGroupId")
								});
							}
							else
							{
								bool flag94 = this.Param1 == 4;
								if (flag94)
								{
									this.MessageText = string.Concat(new string[]
									{
										this.MessageText,
										"Filter ",
										this.Param4,
										": ",
										LocalizationManager.Localize("txtObjectConfThreshold")
									});
								}
								else
								{
									bool flag95 = this.Param1 == 5;
									if (flag95)
									{
										this.MessageText = string.Concat(new string[]
										{
											this.MessageText,
											"Filter ",
											this.Param4,
											": ",
											LocalizationManager.Localize("txtMinCount")
										});
									}
									else
									{
										bool flag96 = this.Param1 == 6;
										if (flag96)
										{
											this.MessageText = string.Concat(new string[]
											{
												this.MessageText,
												"Filter ",
												this.Param4,
												": ",
												LocalizationManager.Localize("txtMaxCount")
											});
										}
										else
										{
											bool flag97 = this.Param1 == 7;
											if (flag97)
											{
												this.MessageText = string.Concat(new string[]
												{
													this.MessageText,
													"Filter ",
													this.Param4,
													": ",
													LocalizationManager.Localize("txtObjectUnmaskForConEnabled")
												});
											}
											else
											{
												bool flag98 = this.Param1 == 8;
												if (flag98)
												{
													this.MessageText = string.Concat(new string[]
													{
														this.MessageText,
														"Filter ",
														this.Param4,
														": ",
														LocalizationManager.Localize("txtObjectUnmaskForConMethod")
													});
												}
												else
												{
													bool flag99 = this.Param1 == 9;
													if (flag99)
													{
														this.MessageText += "RetrainRegion";
													}
													else
													{
														bool flag100 = this.Param1 == 10;
														if (flag100)
														{
															this.MessageText = string.Concat(new string[]
															{
																this.MessageText,
																"Filter ",
																this.Param4,
																": ",
																LocalizationManager.Localize("txtFilterSize")
															});
														}
														else
														{
															bool flag101 = this.Param1 == 11;
															if (flag101)
															{
																this.MessageText = string.Concat(new string[]
																{
																	this.MessageText,
																	"Filter ",
																	this.Param4,
																	": ",
																	LocalizationManager.Localize("txtObjectMinGroupSize")
																});
															}
															else
															{
																bool flag102 = this.Param1 == 12;
																if (flag102)
																{
																	this.MessageText = string.Concat(new string[]
																	{
																		this.MessageText,
																		"Filter ",
																		this.Param4,
																		": ",
																		LocalizationManager.Localize("txtObjectConfThreshold"),
																		" 1"
																	});
																}
																else
																{
																	bool flag103 = this.Param1 == 13;
																	if (flag103)
																	{
																		this.MessageText = string.Concat(new string[]
																		{
																			this.MessageText,
																			"Filter ",
																			this.Param4,
																			": ",
																			LocalizationManager.Localize("txtObjectConfThreshold"),
																			" 2"
																		});
																	}
																	else
																	{
																		bool flag104 = this.Param1 == 14;
																		if (flag104)
																		{
																			this.MessageText = string.Concat(new string[]
																			{
																				this.MessageText,
																				"Filter ",
																				this.Param4,
																				": ",
																				LocalizationManager.Localize("txtObjectConfThreshold"),
																				" 3"
																			});
																		}
																		else
																		{
																			bool flag105 = this.Param1 == 15;
																			if (flag105)
																			{
																				this.MessageText = string.Concat(new string[]
																				{
																					this.MessageText,
																					"Filter ",
																					this.Param4,
																					": ",
																					LocalizationManager.Localize("txtObjectTTAEnabled")
																				});
																			}
																			else
																			{
																				bool flag106 = this.Param1 == 16;
																				if (flag106)
																				{
																					this.MessageText = string.Concat(new string[]
																					{
																						this.MessageText,
																						"Filter ",
																						this.Param4,
																						": ",
																						LocalizationManager.Localize("txtLowThreshold")
																					});
																				}
																				else
																				{
																					bool flag107 = this.Param1 == 17;
																					if (flag107)
																					{
																						this.MessageText = string.Concat(new string[]
																						{
																							this.MessageText,
																							"Filter ",
																							this.Param4,
																							": ",
																							LocalizationManager.Localize("txtHighThreshold")
																						});
																					}
																					else
																					{
																						bool flag108 = this.Param1 == 18;
																						if (flag108)
																						{
																							this.MessageText = string.Concat(new string[]
																							{
																								this.MessageText,
																								"Filter ",
																								this.Param4,
																								": ",
																								LocalizationManager.Localize("txtPercentileThreshold")
																							});
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
									}
								}
							}
						}
					}
				}
				bool flag109 = this.Param1 != 0 && this.Param1 != 9;
				if (flag109)
				{
					this.MessageText = string.Concat(new string[]
					{
						this.MessageText,
						": ",
						this.Param5,
						" --> ",
						this.Param6
					});
				}
				break;
			}
			}
			this.LogbookText += this.MessageText;
			string str = this.LogbookText;
			string newLine = Environment.NewLine;
			string str2 = "I";
			int num11 = (int)this.entryCode;
			this.LogbookText = str + newLine + str2 + num11.ToString("D3");
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000B23A4 File Offset: 0x000B05A4
		private MessageType ToMessageType()
		{
			MessageType result = MessageType.Info;
			bool flag = this.entryCode == SSTEntryCode.Metal;
			if (flag)
			{
				result = MessageType.Contamination;
			}
			else
			{
				bool flag2 = (byte)this.entryCode > 1 && (byte)this.entryCode < 64;
				if (flag2)
				{
					bool flag3 = this.entryCode != SSTEntryCode.XRayFault || (this.param1 != 0 && this.param1 != 11);
					if (flag3)
					{
						result = MessageType.Error;
					}
					else
					{
						result = MessageType.Warning;
					}
				}
				else
				{
					bool flag4 = (byte)this.entryCode >= 64 && (byte)this.entryCode < 128;
					if (flag4)
					{
						result = MessageType.Warning;
					}
				}
			}
			return result;
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000B2440 File Offset: 0x000B0640
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000B2470 File Offset: 0x000B0670
		public static void GenerateErrorTranslationText()
		{
			string text = "Fehlerliste Raycon";
			text = text + Environment.NewLine + "GUI Version: " + Definition.guiVersion.ToString();
			text = text + Environment.NewLine + "Sprache: " + LocalizationManager.CurrentCulture.ToString();
			text = string.Concat(new string[]
			{
				text,
				Environment.NewLine,
				"Datei generiert am: ",
				DateTime.Now.ToString(),
				Environment.NewLine
			});
			text = string.Concat(new string[]
			{
				text,
				Environment.NewLine,
				"Übersetzung für Fehler: ",
				LocalizationManager.Localize("txtError"),
				Environment.NewLine
			});
			for (int i = 2; i <= 57; i++)
			{
				text = string.Concat(new string[]
				{
					text,
					Environment.NewLine,
					"E",
					i.ToString("D3"),
					";"
				});
				switch (i)
				{
				case 2:
					text += LocalizationManager.Localize("lbDetectorCommError");
					break;
				case 3:
					text += "CalibrationError";
					break;
				case 4:
					text += LocalizationManager.Localize("lbXRayCommError");
					break;
				case 5:
					for (int j = 0; j < Definition.xrayVJFaults.Length; j++)
					{
						text = text + "A" + (j + 8).ToString("D3") + ";";
						text = text + LocalizationManager.Localize("parItemXRaySource") + "; " + Definition.xrayVJFaults[j].ToString();
						bool flag = j < Definition.xrayVJFaults.Length - 1;
						if (flag)
						{
							text = string.Concat(new string[]
							{
								text,
								Environment.NewLine,
								"E",
								i.ToString("D3"),
								";"
							});
						}
					}
					break;
				case 6:
					text += LocalizationManager.Localize("lbProcessingDelay");
					break;
				case 7:
					text += LocalizationManager.Localize("lbMemoryUsage");
					break;
				case 8:
					text += LocalizationManager.Localize("parItemErrorEngine");
					break;
				case 9:
					text += LocalizationManager.Localize("parItemLowAirPressure");
					break;
				case 10:
					text += LocalizationManager.Localize("parItemErrorCooling");
					break;
				case 11:
					text = text + "A00x;" + LocalizationManager.Localize("parItemBinFull") + " x";
					break;
				case 12:
					text += LocalizationManager.Localize("msgErrorComParamServer");
					break;
				case 13:
					text += LocalizationManager.Localize("msgErrorComImgServer");
					break;
				case 14:
					text += LocalizationManager.Localize("lbWAGOCommError");
					break;
				case 15:
					text += LocalizationManager.Localize("txtErrorSecondaryComm");
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorAnalogInput") + " " + SesotecIOErrorsAI.AI_00.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorAnalogInput") + " " + SesotecIOErrorsAI.AI_01.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorAnalogInput") + " " + SesotecIOErrorsAI.AI_02.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorAnalogInput") + " " + SesotecIOErrorsAI.AI_03.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_00_07.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_08.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_09.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_10.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_11.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_16_23.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_24.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_25.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_26.ToString();
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("txtErrorDigitalOutput") + " " + SesotecIOErrorsDO.DO_27.ToString();
					break;
				case 16:
					text = text + "A00x;Rejecter x Verify: x " + LocalizationManager.Localize("txtMissingProducts");
					break;
				case 17:
					text += LocalizationManager.Localize("msgLampCurrentLow");
					break;
				case 18:
					text = text + LocalizationManager.Localize("parItemTempSensor1") + ": " + LocalizationManager.Localize("msgTempTooHigh");
					break;
				case 19:
					text = text + LocalizationManager.Localize("parItemVoltage12V") + ": " + LocalizationManager.Localize("msgVoltageTooLow");
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("parItemVoltage12V") + ": " + LocalizationManager.Localize("msgVoltageTooHigh");
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("parItemVoltage24V") + ": " + LocalizationManager.Localize("msgVoltageTooLow");
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";"
					});
					text = text + LocalizationManager.Localize("parItemVoltage24V") + ": " + LocalizationManager.Localize("msgVoltageTooHigh");
					break;
				case 20:
					text += LocalizationManager.Localize("lbImageProcessingPcOffline");
					break;
				case 21:
					text = text + "A00x;" + LocalizationManager.Localize("parItemLightBarrier0").Replace("1", "x");
					break;
				case 22:
					text += LocalizationManager.Localize("lbSVTCommError");
					break;
				case 23:
					for (int k = 0; k < Definition.svtFaults.Length; k++)
					{
						text = text + "A" + (k + 1).ToString("D3") + ";";
						text = text + LocalizationManager.Localize("lbSVTFault") + "; " + Definition.svtFaults[k].ToString();
						bool flag2 = k < Definition.svtFaults.Length - 1;
						if (flag2)
						{
							text = string.Concat(new string[]
							{
								text,
								Environment.NewLine,
								"E",
								i.ToString("D3"),
								";"
							});
						}
					}
					break;
				case 24:
					text += "A000;";
					text += LocalizationManager.Localize("lbSshOvercurrent");
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";A001;"
					});
					text += LocalizationManager.Localize("lbSShError");
					break;
				case 25:
					text = text + LocalizationManager.Localize("menuItemAuditCheck") + ": " + LocalizationManager.Localize("txtTestFailed");
					break;
				case 26:
					text = text + LocalizationManager.Localize("parItemXRaySource") + ": " + LocalizationManager.Localize("txtSeveralArcs");
					break;
				case 27:
					text = text + "A00x;Rejecter x: " + LocalizationManager.Localize("lbProductsMissing");
					break;
				case 28:
					text = text + "A00x;Rejecter x: " + LocalizationManager.Localize("lbSensorBlockedPhotogating");
					break;
				case 29:
					text = text + "A00x;Rejecter x: " + LocalizationManager.Localize("lbSensorBlockedRejectCheck");
					break;
				case 30:
					text = text + "A00x;Rejecter x: " + LocalizationManager.Localize("lbErrorConsistency");
					break;
				case 31:
					text = text + "A00x;Rejecter x: " + LocalizationManager.Localize("lbBinMissing") + ".";
					break;
				case 32:
					text = text + "A00x;Rejecter x: " + LocalizationManager.Localize("lbProductNotInspected");
					break;
				case 33:
					text += LocalizationManager.Localize("txtTooManyRejects");
					break;
				case 34:
					text += LocalizationManager.Localize("lbInsightNetConnectionBroken");
					break;
				case 35:
					text += LocalizationManager.Localize("lbDiskSpaceLow");
					break;
				case 36:
					text = text + "A001;" + LocalizationManager.Localize("txtInletSensorBlocked") + ".";
					text = string.Concat(new string[]
					{
						text,
						Environment.NewLine,
						"E",
						i.ToString("D3"),
						";A002;"
					});
					text = text + LocalizationManager.Localize("txtOutletSensorBlocked") + ".";
					break;
				case 37:
					text += LocalizationManager.Localize("txtMeanValueHELE");
					break;
				case 38:
					text += LocalizationManager.Localize("txtLimitXRayOperatingHours");
					text += LocalizationManager.Localize("txtErrorLimitXRayOperatingHours");
					break;
				case 42:
					text += LocalizationManager.Localize("txtChangedTestcardSim");
					text += LocalizationManager.Localize("txtVA");
					text += LocalizationManager.Localize("txtGlass");
					text += LocalizationManager.Localize("txtNoComplianceMsg");
					text += LocalizationManager.Localize("txtThreshold");
					break;
				case 68:
					text += LocalizationManager.Localize("txtLimitXRayOperatingHours");
					text += LocalizationManager.Localize("txtWarnLimitXRayOperatingHours");
					break;
				case 69:
					text += LocalizationManager.Localize("txtChangedTestcardSim");
					text += LocalizationManager.Localize("txtVA");
					text += LocalizationManager.Localize("txtGlass");
					text += LocalizationManager.Localize("txtNoComplianceMsg");
					text += LocalizationManager.Localize("txtThreshold");
					break;
				}
			}
			File.WriteAllText("RayconErrorList " + LocalizationManager.CurrentCulture.ToString() + ".txt", text);
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x000B3360 File Offset: 0x000B1560
		public static void GenerateWarningTranslationText()
		{
			string text = "Warnungsliste Raycon";
			text = text + Environment.NewLine + "GUI Version: " + Definition.guiVersion.ToString();
			text = text + Environment.NewLine + "Sprache: " + LocalizationManager.CurrentCulture.ToString();
			text = string.Concat(new string[]
			{
				text,
				Environment.NewLine,
				"Datei generiert am: ",
				DateTime.Now.ToString(),
				Environment.NewLine
			});
			text = string.Concat(new string[]
			{
				text,
				Environment.NewLine,
				"Übersetzung für Warnung: ",
				LocalizationManager.Localize("txtError"),
				Environment.NewLine
			});
			for (int i = 64; i <= 69; i++)
			{
				text = string.Concat(new string[]
				{
					text,
					Environment.NewLine,
					"W",
					i.ToString("D3"),
					";"
				});
				switch (i)
				{
				case 64:
					text += LocalizationManager.Localize("lbDatabaseOutOfDate");
					break;
				case 65:
					text += LocalizationManager.Localize("msgWagoOutOfDate");
					break;
				case 66:
					text += LocalizationManager.Localize("msgWagoWrongVersion");
					break;
				case 67:
					text += LocalizationManager.Localize("msgVibroInManualMode");
					break;
				case 68:
					text += LocalizationManager.Localize("txtLimitXRayOperatingHours");
					break;
				}
			}
			File.WriteAllText("RayconWarningList " + LocalizationManager.CurrentCulture.ToString() + ".txt", text);
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x000B3514 File Offset: 0x000B1714
		public static void GenerateInfoTranslationText()
		{
			string text = "Infoliste Raycon";
			text = text + Environment.NewLine + "GUI Version: " + Definition.guiVersion.ToString();
			text = text + Environment.NewLine + "Sprache: " + LocalizationManager.CurrentCulture.ToString();
			text = string.Concat(new string[]
			{
				text,
				Environment.NewLine,
				"Datei generiert am: ",
				DateTime.Now.ToString(),
				Environment.NewLine
			});
			text = string.Concat(new string[]
			{
				text,
				Environment.NewLine,
				"Übersetzung für Info: ",
				LocalizationManager.Localize("txtInfo"),
				Environment.NewLine
			});
			for (int i = 128; i <= 180; i++)
			{
				text = string.Concat(new string[]
				{
					text,
					Environment.NewLine,
					"I",
					i.ToString("D3"),
					";"
				});
				switch (i)
				{
				case 128:
					text += LocalizationManager.Localize("lbMachineTurnedOn");
					break;
				case 129:
					text += LocalizationManager.Localize("lbMachineTurnedOff");
					break;
				case 130:
					text = text + LocalizationManager.Localize("txtPerProductChange") + ": P1  --> P2";
					break;
				case 131:
					text = string.Concat(new string[]
					{
						text,
						LocalizationManager.Localize("txtProductEnded"),
						": P1: ",
						LocalizationManager.Localize("txtThroughput"),
						": 0 ",
						LocalizationManager.Localize("txtReject"),
						": 0"
					});
					break;
				case 132:
					text = text + LocalizationManager.Localize("lbNewProductCreated") + " P1";
					break;
				case 133:
					text = text + LocalizationManager.Localize("lbProductCopied") + " P1 --> P2";
					break;
				case 134:
					text = text + LocalizationManager.Localize("lbProductDeleted") + " P1 ";
					break;
				case 135:
					text = text + LocalizationManager.Localize("lbProductRenamed") + " P1 --> P2";
					break;
				case 136:
					text = text + LocalizationManager.Localize("lbProductNumberChanged") + " P1 --> P2";
					break;
				case 137:
					text = text + LocalizationManager.Localize("lbSafetySwitchOpen") + ": " + LocalizationManager.Localize("parItemShieldRHTopFront");
					break;
				case 138:
					text += LocalizationManager.Localize("parItemEmergencyStopPressed");
					break;
				case 139:
					text += LocalizationManager.Localize("lbAutolearnStarted");
					break;
				case 140:
					text += LocalizationManager.Localize("txtAfterAutotrain");
					break;
				case 141:
					text += LocalizationManager.Localize("txtAbortAutotrain");
					break;
				case 142:
					text += "Retrain: P1";
					break;
				case 143:
					text += "---";
					break;
				case 144:
					text += LocalizationManager.Localize("txtConveyorSpeed");
					text += ": V1 --> V2";
					break;
				case 145:
					text = text + LocalizationManager.Localize("lbEdgeFilterActivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbWeigherActivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbObjectCountActivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbIgnoreClipActivated") + Environment.NewLine + ";";
					text += LocalizationManager.Localize("lbImageSizeAcitvated");
					break;
				case 146:
					text = text + LocalizationManager.Localize("lbEdgeFilterDeactivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbWeigherDeactivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbObjectCountDeactivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbIgnoreClipDeactivated") + Environment.NewLine + ";";
					text += LocalizationManager.Localize("lbImageSizeDeacitvated");
					break;
				case 147:
					text = text + LocalizationManager.Localize("lbEdgeFilter") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtNumLeftPixel") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtNumLeftPixel") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtNumRightPixel") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtNumTopPixel") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtNumBottomPixel") + Environment.NewLine + ";";
					text += LocalizationManager.Localize("txtThreshold");
					text += ": P1 --> P2 ";
					break;
				case 148:
					text += LocalizationManager.Localize("lbSystemTimeChanged");
					break;
				case 149:
					text = string.Concat(new string[]
					{
						text,
						LocalizationManager.Localize("lbObjectCounter"),
						": ",
						Environment.NewLine,
						";"
					});
					text = text + LocalizationManager.Localize("txtMinCount") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtMaxCount") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtLowThreshold") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtHighThreshold") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtMinSize") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtMaxSize") + ": V1 --> V2 ";
					break;
				case 150:
					text = string.Concat(new string[]
					{
						text,
						LocalizationManager.Localize("txtContaminationDetected"),
						" ",
						LocalizationManager.Localize("txtDuringAudit"),
						Environment.NewLine,
						";",
						LocalizationManager.Localize("lbFalseweightDetected"),
						" ",
						LocalizationManager.Localize("txtDuringAudit"),
						Environment.NewLine,
						";",
						LocalizationManager.Localize("lbFalseObjectCountDetected"),
						" ",
						LocalizationManager.Localize("txtDuringAudit"),
						Environment.NewLine,
						";",
						LocalizationManager.Localize("lbProductNotRecognized"),
						". ",
						LocalizationManager.Localize("txtThroughput"),
						": V1 ",
						LocalizationManager.Localize("txtReject"),
						": V2",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtWrongImageSizeDetected"),
						" ",
						LocalizationManager.Localize("txtThroughput"),
						": V1 ",
						LocalizationManager.Localize("txtReject"),
						": V2"
					});
					break;
				case 151:
					text += LocalizationManager.Localize("lbXRayTurnedOn");
					break;
				case 152:
					text += LocalizationManager.Localize("lbXRayTurnedOff");
					break;
				case 153:
					text = string.Concat(new string[]
					{
						text,
						LocalizationManager.Localize("lbWeigher"),
						": ",
						Environment.NewLine,
						";"
					});
					text = text + LocalizationManager.Localize("txtWeight") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtMinimum") + Environment.NewLine + ";";
					text += LocalizationManager.Localize("txtMaximum");
					text += ": V1-- > V2 ";
					break;
				case 154:
					text += "Path: P1  --> P2";
					break;
				case 155:
					text = text + LocalizationManager.Localize("lbUserLoggedIn") + ": U1 ";
					break;
				case 156:
					text = text + LocalizationManager.Localize("lbUserLoggedOut") + ": U2 ";
					break;
				case 157:
					text = text + LocalizationManager.Localize("lbUserChange") + ": U1 --> U2 ";
					break;
				case 158:
					text = text + LocalizationManager.Localize("lbNewUserCreated") + ": ID U1";
					break;
				case 159:
					text = text + LocalizationManager.Localize("lbUserParameterChanged") + ": ID U1";
					break;
				case 160:
					text = text + LocalizationManager.Localize("lbUserDeleted") + ": ID U1";
					break;
				case 161:
					text += "Retrain group: P1 V1 V2";
					break;
				case 162:
					text += "Retrain group failed: P1 V1 V2";
					break;
				case 163:
					text = text + LocalizationManager.Localize("lbSensitivityChanged") + ": V1 --> V2 ";
					break;
				case 164:
					text = string.Concat(new string[]
					{
						text,
						LocalizationManager.Localize("lbAuditCheck"),
						": ",
						Environment.NewLine,
						";"
					});
					text = text + LocalizationManager.Localize("txtAuditMode") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtAuditTimeHour") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtAuditTimeMinute") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtAuditDay") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtAuditIntervalHour") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtAuditIntervalMinute") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtTimeToStartAudit") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtNumContaminations") + Environment.NewLine + ";";
					text += "V1 --> V2 ";
					break;
				case 165:
					text = text + LocalizationManager.Localize("lbAuditContaminationActivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbAuditWeightActivated") + Environment.NewLine + ";";
					text += LocalizationManager.Localize("lbAuditObjectcountActivated");
					break;
				case 166:
					text = text + LocalizationManager.Localize("lbAuditContaminationDeactivated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("lbAuditWeightDeactivated") + Environment.NewLine + ";";
					text += LocalizationManager.Localize("lbAuditObjectcountDeactivated");
					break;
				case 167:
					text += LocalizationManager.Localize("txtAuditNeeded");
					break;
				case 168:
					text = text + LocalizationManager.Localize("menuItemAuditCheck") + ": " + LocalizationManager.Localize("txtTestOk");
					break;
				case 169:
					text = text + LocalizationManager.Localize("menuItemAuditCheck") + ": " + LocalizationManager.Localize("txtTestFailed");
					break;
				case 170:
					text += LocalizationManager.Localize("lbAuditStarted");
					break;
				case 171:
					text = text + LocalizationManager.Localize("lbCheckpointRestored") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtCheckpoint") + " V1: T1";
					break;
				case 172:
					text = text + LocalizationManager.Localize("txtCheckpointDeleted") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtCheckpoint") + " V1: T1";
					break;
				case 173:
					text = text + LocalizationManager.Localize("txtCheckpointCreated") + Environment.NewLine + ";";
					text = text + LocalizationManager.Localize("txtCheckpoint") + " V1: T1";
					break;
				case 174:
					text = string.Concat(new string[]
					{
						text,
						LocalizationManager.Localize("txtProductDuringTest"),
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtThroughput"),
						": V1 ",
						LocalizationManager.Localize("txtReject"),
						": V2",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtProductDuringTest"),
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtCurrentWeight"),
						": V1",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtWeight"),
						": V2",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtThroughput"),
						": V3 ",
						LocalizationManager.Localize("txtReject"),
						": V4",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtProductDuringTest"),
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtCount"),
						": V1",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtMaxCount"),
						": V2",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtMinCount"),
						": V3",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtThroughput"),
						": V4 ",
						LocalizationManager.Localize("txtReject"),
						": V5"
					});
					break;
				case 175:
					text = string.Concat(new string[]
					{
						text,
						"Batch ended: P1 (Batch): ",
						Environment.NewLine,
						";",
						LocalizationManager.Localize("txtThroughput"),
						": V1 ",
						LocalizationManager.Localize("txtReject"),
						": V2 "
					});
					break;
				case 176:
					text = string.Concat(new string[]
					{
						text,
						"Product reference weight: V1",
						Environment.NewLine,
						";Batch reference weight: V2",
						Environment.NewLine,
						";Total product weight: V3 ",
						Environment.NewLine,
						";Total batch weight: V4 ",
						Environment.NewLine,
						";Average weight: V5 ",
						Environment.NewLine,
						";Standard deviation: V6 "
					});
					break;
				case 177:
					text = text + LocalizationManager.Localize("txtBatchChange") + ": V1 --> V2 ";
					break;
				case 178:
					text += LocalizationManager.Localize("parItemMachineEnableExtern");
					break;
				}
			}
			File.WriteAllText("RayconInfoList " + LocalizationManager.CurrentCulture.ToString() + ".txt", text);
		}

		// Token: 0x04001285 RID: 4741
		public static readonly string[] StrLocalizedSafetySwitches = new string[]
		{
			"parItemServiceDoor",
			"parItemDetector",
			"parItemXRaySource",
			"parItemPanelTop",
			"parItemPanelLeft",
			"parItemPanelRight",
			"parItemCleaningDoorLeft",
			"parItemCleaningDoorRight",
			"parItemShieldRHTopBack",
			"parItemShieldLHTopBack",
			"parItemShieldRHBotBack",
			"parItemShieldLHBotBack",
			"parItemShieldRHTopFront",
			"parItemShieldLHTopFront",
			"parItemShieldRHBotFront",
			"parItemShieldLHBotFront",
			"txtRejecterDeactivated",
			"parItemPanelTopRH",
			"parItemPanelTopLH",
			"parItemHopperWasteProduct",
			"parItemHopperGoodProduct",
			"parItemHopperInfeed",
			"parItemShieldBottom",
			"parItemCleaningDoorRH",
			"parItemCleaningDoorBackRH",
			"parItemCleaningDoorFrontRH",
			"parItemCleaningDoorLH",
			"parItemCleaningDoorBackLH",
			"parItemCleaningDoorFrontLH",
			"parItemCleaningDoorFront",
			"parItemServiceDoorRejectionFront",
			"parItemServiceDoorRejectionBack",
			"parItemServiceDoorRejectionMiddle",
			"parItemSlidingPlate",
			"parConveyorDoor",
			"parShieldingRightBack",
			"parShieldingLeftBack",
			"parShieldingRightFront",
			"parShieldingLeftFront",
			"txtHardwareBrigeDPlus1Module",
			"txtHardwareBrigeDPlus1Module",
			"txtHardwareBrigeDPlus1Module",
			"txtHardwareBrigeDPlus1Module",
			"txtHardwareBrigeDPlus1Module",
			"parItemConveyorBackNotDocked",
			"parItemRejecterUnitNotAvailable",
			"parItemRejecterUnitCoverTop",
			"parItemRejecterUnitCoverFront"
		};

		// Token: 0x04001286 RID: 4742
		private const int lbStrParamLength = 40;

		// Token: 0x04001287 RID: 4743
		private const int lbStrParam6Length = 10;

		// Token: 0x04001288 RID: 4744
		private byte[] noUserCodes = new byte[]
		{
			155,
			156,
			157
		};

		// Token: 0x04001289 RID: 4745
		private DateTime entryTime;

		// Token: 0x0400128A RID: 4746
		private SSTEntryCode entryCode;

		// Token: 0x0400128B RID: 4747
		private byte param1;

		// Token: 0x0400128C RID: 4748
		private uint iD;

		// Token: 0x0400128D RID: 4749
		private string logbookText;

		// Token: 0x0400128E RID: 4750
		private string messageText;
	}
}

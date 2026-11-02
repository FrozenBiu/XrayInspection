using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x02000054 RID: 84
	public class ValveActivityViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x000367FD File Offset: 0x000349FD
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x00036805 File Offset: 0x00034A05
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x0003680E File Offset: 0x00034A0E
		// (set) Token: 0x06000886 RID: 2182 RVA: 0x00036816 File Offset: 0x00034A16
		public RelayCommand ZoomInYCommand { get; set; }

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x0003681F File Offset: 0x00034A1F
		// (set) Token: 0x06000888 RID: 2184 RVA: 0x00036827 File Offset: 0x00034A27
		public RelayCommand ZoomOutYCommand { get; set; }

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00036830 File Offset: 0x00034A30
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x00036838 File Offset: 0x00034A38
		public BoundLocalizedText StrValveActivity { get; set; }

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x0600088B RID: 2187 RVA: 0x00036844 File Offset: 0x00034A44
		// (remove) Token: 0x0600088C RID: 2188 RVA: 0x0003687C File Offset: 0x00034A7C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x000368B4 File Offset: 0x00034AB4
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x000368CC File Offset: 0x00034ACC
		public List<Point> Flap1Activity
		{
			get
			{
				return this.flap1Activity;
			}
			set
			{
				this.flap1Activity = value;
				this.OnPropertyChanged("Flap1Activity");
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x000368E4 File Offset: 0x00034AE4
		// (set) Token: 0x06000890 RID: 2192 RVA: 0x000368FC File Offset: 0x00034AFC
		public List<Point> Flap2Activity
		{
			get
			{
				return this.flap2Activity;
			}
			set
			{
				this.flap2Activity = value;
				this.OnPropertyChanged("Flap2Activity");
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00036914 File Offset: 0x00034B14
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x0003692C File Offset: 0x00034B2C
		public List<Point> Flap3Activity
		{
			get
			{
				return this.flap3Activity;
			}
			set
			{
				this.flap3Activity = value;
				this.OnPropertyChanged("Flap3Activity");
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00036944 File Offset: 0x00034B44
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x0003695C File Offset: 0x00034B5C
		public List<Point> Flap4Activity
		{
			get
			{
				return this.flap4Activity;
			}
			set
			{
				this.flap4Activity = value;
				this.OnPropertyChanged("Flap4Activity");
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00036974 File Offset: 0x00034B74
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x0003698C File Offset: 0x00034B8C
		public List<Point> SumOfActivities
		{
			get
			{
				return this.sumOfActivities;
			}
			set
			{
				this.sumOfActivities = value;
				this.OnPropertyChanged("SumOfActivities");
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x000369A4 File Offset: 0x00034BA4
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x000369BC File Offset: 0x00034BBC
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
			set
			{
				this.maxY = value;
				this.OnPropertyChanged("MaxY");
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x000369D4 File Offset: 0x00034BD4
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x000369EC File Offset: 0x00034BEC
		public double ZoomFactorX
		{
			get
			{
				return this.zoomFactorX;
			}
			set
			{
				this.zoomFactorX = value;
				this.OnPropertyChanged("ZoomFactorX");
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x00036A04 File Offset: 0x00034C04
		// (set) Token: 0x0600089C RID: 2204 RVA: 0x00036A1C File Offset: 0x00034C1C
		public double ZoomFactorY
		{
			get
			{
				return this.zoomFactorY;
			}
			set
			{
				this.zoomFactorY = value;
				this.OnPropertyChanged("ZoomFactorY");
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x00036A34 File Offset: 0x00034C34
		// (set) Token: 0x0600089E RID: 2206 RVA: 0x00036A4C File Offset: 0x00034C4C
		public int TicksY
		{
			get
			{
				return this.ticksY;
			}
			set
			{
				this.ticksY = value;
				this.OnPropertyChanged("TicksY");
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x00036A62 File Offset: 0x00034C62
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00036A6A File Offset: 0x00034C6A
		public int MaxYInit { get; set; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00036A73 File Offset: 0x00034C73
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00036A7B File Offset: 0x00034C7B
		public int TicksYInit { get; set; }

		// Token: 0x060008A3 RID: 2211 RVA: 0x00036A84 File Offset: 0x00034C84
		public ValveActivityViewModel(ValveActivity valveActivity, SetFooterMainWindow setFooterMainWindow)
		{
			this.valveActivity = valveActivity;
			this.setFooterMainWindow = setFooterMainWindow;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.ZoomInYCommand = new RelayCommand(new Action<object>(this.ZoomInY));
			this.ZoomOutYCommand = new RelayCommand(new Action<object>(this.ZoomOutY));
			this.actualModeToSavePNG = "Sum";
			this.RegisterLocalizedText();
			setFooterMainWindow.SetFooterDeactivation();
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00036B70 File Offset: 0x00034D70
		public void setTicksY()
		{
			bool flag = this.MaxY <= 20;
			if (flag)
			{
				this.TicksY = 1;
			}
			else
			{
				bool flag2 = this.MaxY <= 40;
				if (flag2)
				{
					this.TicksY = 2;
				}
				else
				{
					bool flag3 = this.MaxY <= 60;
					if (flag3)
					{
						this.TicksY = 3;
					}
					else
					{
						bool flag4 = this.MaxY <= 80;
						if (flag4)
						{
							this.TicksY = 4;
						}
						else
						{
							bool flag5 = this.MaxY <= 100;
							if (flag5)
							{
								this.TicksY = 5;
							}
							else
							{
								bool flag6 = this.MaxY <= 200;
								if (flag6)
								{
									this.TicksY = 10;
								}
								else
								{
									bool flag7 = this.MaxY <= 400;
									if (flag7)
									{
										this.TicksY = 20;
									}
									else
									{
										bool flag8 = this.MaxY <= 800;
										if (flag8)
										{
											this.TicksY = 40;
										}
										else
										{
											bool flag9 = this.MaxY <= 1000;
											if (flag9)
											{
												this.TicksY = 50;
											}
											else
											{
												bool flag10 = this.MaxY <= 2000;
												if (flag10)
												{
													this.TicksY = 100;
												}
												else
												{
													bool flag11 = this.MaxY <= 4000;
													if (flag11)
													{
														this.TicksY = 200;
													}
													else
													{
														bool flag12 = this.MaxY <= 8000;
														if (flag12)
														{
															this.TicksY = 400;
														}
														else
														{
															bool flag13 = this.MaxY <= 10000;
															if (flag13)
															{
																this.TicksY = 500;
															}
															else
															{
																this.ticksY = 1000;
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
			bool flag14 = this.MaxY <= 20;
			if (flag14)
			{
				int num = this.MaxY;
				this.MaxY = num + 1;
			}
			else
			{
				bool flag15 = this.MaxY <= 100;
				if (flag15)
				{
					this.MaxY += 5;
				}
				else
				{
					bool flag16 = this.MaxY <= 1000;
					if (flag16)
					{
						this.MaxY += 10;
					}
					else
					{
						bool flag17 = this.MaxY <= 10000;
						if (flag17)
						{
							this.MaxY += 100;
						}
					}
				}
			}
			bool flag18 = this.setMaxYInit;
			if (flag18)
			{
				this.MaxYInit = this.MaxY;
				this.TicksYInit = this.TicksY;
				this.setMaxYInit = false;
			}
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00036E07 File Offset: 0x00035007
		public void ZoomIn()
		{
			this.ZoomFactorX *= 2.0;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00036E21 File Offset: 0x00035021
		public void ZoomOut()
		{
			this.ZoomFactorX /= 2.0;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00036E3B File Offset: 0x0003503B
		public void ZoomInY(object obj)
		{
			this.ZoomFactorY *= 2.0;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00036E55 File Offset: 0x00035055
		public void ZoomOutY(object obj)
		{
			this.ZoomFactorY /= 2.0;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00036E70 File Offset: 0x00035070
		public void SaveActualGraphPNG(Grid chartGrid, Rect bounds, ProductManager productManager)
		{
			string text = "";
			string text2 = "";
			string text3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ValveActivity\\";
			string text4 = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			bool flag = !Directory.Exists(text3);
			if (flag)
			{
				Directory.CreateDirectory(text3);
			}
			this.valveActivity.DbConnection.GetUsbDriveLetter(ref text);
			try
			{
				string str = string.Concat(new string[]
				{
					"ValveActivity_",
					this.actualModeToSavePNG,
					"_",
					productManager.CurrentLoadedProduct.ProductName,
					"_",
					text4,
					".png"
				});
				text2 = text + "\\ValveActivity";
				bool flag2 = Directory.Exists(text);
				if (flag2)
				{
					bool flag3 = !Directory.Exists(text2);
					if (flag3)
					{
						Directory.CreateDirectory(text2);
					}
				}
				text3 = text3 + "\\" + str;
				text2 = text + "\\ValveActivity\\" + str;
				FileInfo fileInfo = new FileInfo(text2);
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
				DrawingVisual drawingVisual = new DrawingVisual();
				using (DrawingContext drawingContext = drawingVisual.RenderOpen())
				{
					VisualBrush brush = new VisualBrush(chartGrid);
					Pen pen = new Pen();
					drawingContext.DrawRectangle(brush, pen, new Rect(default(Point), bounds.Size));
				}
				renderTargetBitmap.Render(drawingVisual);
				FileStream fileStream = new FileStream(text3, FileMode.Create);
				new PngBitmapEncoder
				{
					Frames = 
					{
						BitmapFrame.Create(renderTargetBitmap)
					}
				}.Save(fileStream);
				fileStream.Close();
				bool flag4 = Directory.Exists(text + "\\");
				if (flag4)
				{
					fileStream = new FileStream(text2, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(renderTargetBitmap)
						}
					}.Save(fileStream);
					fileStream.Close();
				}
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
			catch (UnauthorizedAccessException)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x000371AC File Offset: 0x000353AC
		public void SaveAllGraphesCSV(ProductManager productManager)
		{
			string text = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			string text2 = "";
			string text3 = "ValveActivity";
			List<ValveActivityEntry> list = new List<ValveActivityEntry>();
			string text4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ValveActivity\\";
			string text5 = string.Concat(new string[]
			{
				text4,
				"\\ValveActivity_",
				productManager.CurrentLoadedProduct.ProductName,
				"_",
				text,
				".csv"
			});
			bool flag = !Directory.Exists(text4);
			if (flag)
			{
				Directory.CreateDirectory(text4);
			}
			this.csvwriter = File.CreateText(text5);
			try
			{
				this.valveActivity.DbConnection.GetValveActivities(list);
				list.Sort((ValveActivityEntry a, ValveActivityEntry b) => a.Date.CompareTo(b.Date));
				string[] array = new string[list.Count + 1];
				array[0] = "Date; Flap1; Flap2; Flap3; Flap4; Sum; Images per min";
				for (int i = 0; i < list.Count; i++)
				{
					int value = list[i].Activity1 + list[i].Activity2 + list[i].Activity3 + list[i].Activity4;
					array[i + 1] = string.Concat(new string[]
					{
						Convert.ToString(list[i].Date),
						"; ",
						Convert.ToString(list[i].Activity1),
						"; ",
						Convert.ToString(list[i].Activity2),
						"; ",
						Convert.ToString(list[i].Activity3),
						"; ",
						Convert.ToString(list[i].Activity4),
						"; ",
						Convert.ToString(value),
						"; ",
						Convert.ToString(list[i].Throughput)
					});
				}
				for (int j = 0; j <= list.Count; j++)
				{
					this.csvwriter.WriteLine(array[j]);
				}
				this.csvwriter.Close();
				this.csvwriter.Dispose();
				this.valveActivity.DbConnection.GetUsbDriveLetter(ref text2);
				bool flag2 = Directory.Exists(text2);
				if (flag2)
				{
					bool flag3 = !Directory.Exists(text2 + text3);
					if (flag3)
					{
						Directory.CreateDirectory(text2 + "\\" + text3);
					}
					File.Copy(text5, string.Concat(new string[]
					{
						text2,
						text3,
						"\\",
						text3,
						"_",
						productManager.CurrentLoadedProduct.ProductName,
						"_",
						text,
						".csv"
					}));
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
				}
			}
			catch (UnauthorizedAccessException)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2 + "\\ValveActivity"),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error);
			}
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x000375B0 File Offset: 0x000357B0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000375DD File Offset: 0x000357DD
		private void RegisterLocalizedText()
		{
			this.StrValveActivity = new BoundLocalizedText("msgValveActivity");
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x000375F1 File Offset: 0x000357F1
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrValveActivity);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00037605 File Offset: 0x00035805
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			this.setFooterMainWindow.UnSetFooterDeactivation();
		}

		// Token: 0x040005D5 RID: 1493
		private ValveActivity valveActivity;

		// Token: 0x040005DB RID: 1499
		private List<Point> flap1Activity = new List<Point>();

		// Token: 0x040005DC RID: 1500
		private List<Point> flap2Activity = new List<Point>();

		// Token: 0x040005DD RID: 1501
		private List<Point> flap3Activity = new List<Point>();

		// Token: 0x040005DE RID: 1502
		private List<Point> flap4Activity = new List<Point>();

		// Token: 0x040005DF RID: 1503
		private List<Point> sumOfActivities = new List<Point>();

		// Token: 0x040005E0 RID: 1504
		private int maxY;

		// Token: 0x040005E1 RID: 1505
		private double zoomFactorX = 1.0;

		// Token: 0x040005E2 RID: 1506
		private double zoomFactorY = 1.0;

		// Token: 0x040005E3 RID: 1507
		private int ticksY;

		// Token: 0x040005E4 RID: 1508
		public string actualModeToSavePNG = "";

		// Token: 0x040005E5 RID: 1509
		private StreamWriter csvwriter;

		// Token: 0x040005E6 RID: 1510
		private bool setMaxYInit = true;

		// Token: 0x040005E9 RID: 1513
		private SetFooterMainWindow setFooterMainWindow;
	}
}

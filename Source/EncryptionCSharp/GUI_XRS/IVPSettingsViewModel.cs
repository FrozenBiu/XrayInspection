using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000072 RID: 114
	public class IVPSettingsViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000A57 RID: 2647 RVA: 0x0003E548 File Offset: 0x0003C748
		// (remove) Token: 0x06000A58 RID: 2648 RVA: 0x0003E580 File Offset: 0x0003C780
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0003E5B5 File Offset: 0x0003C7B5
		// (set) Token: 0x06000A5A RID: 2650 RVA: 0x0003E5BD File Offset: 0x0003C7BD
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x0003E5C6 File Offset: 0x0003C7C6
		// (set) Token: 0x06000A5C RID: 2652 RVA: 0x0003E5CE File Offset: 0x0003C7CE
		public BoundLocalizedText StrOnOff { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000A5D RID: 2653 RVA: 0x0003E5D7 File Offset: 0x0003C7D7
		// (set) Token: 0x06000A5E RID: 2654 RVA: 0x0003E5DF File Offset: 0x0003C7DF
		public BoundLocalizedText StrNumberTestsamples { get; set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0003E5E8 File Offset: 0x0003C7E8
		// (set) Token: 0x06000A60 RID: 2656 RVA: 0x0003E5F0 File Offset: 0x0003C7F0
		public BoundLocalizedText StrDate { get; set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0003E5F9 File Offset: 0x0003C7F9
		// (set) Token: 0x06000A62 RID: 2658 RVA: 0x0003E601 File Offset: 0x0003C801
		public BoundLocalizedText StrIntervallInMonths { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x0003E60A File Offset: 0x0003C80A
		// (set) Token: 0x06000A64 RID: 2660 RVA: 0x0003E612 File Offset: 0x0003C812
		public BoundLocalizedText StrIntervall { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x0003E61B File Offset: 0x0003C81B
		// (set) Token: 0x06000A66 RID: 2662 RVA: 0x0003E623 File Offset: 0x0003C823
		public BoundLocalizedText StrManually { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0003E62C File Offset: 0x0003C82C
		// (set) Token: 0x06000A68 RID: 2664 RVA: 0x0003E634 File Offset: 0x0003C834
		public BoundLocalizedText StrLastDate { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x0003E63D File Offset: 0x0003C83D
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x0003E645 File Offset: 0x0003C845
		public BoundLocalizedText StrSettings { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000A6B RID: 2667 RVA: 0x0003E64E File Offset: 0x0003C84E
		// (set) Token: 0x06000A6C RID: 2668 RVA: 0x0003E656 File Offset: 0x0003C856
		public BoundLocalizedText StrRejectAll { get; set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x0003E65F File Offset: 0x0003C85F
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x0003E667 File Offset: 0x0003C867
		public BoundLocalizedText StrSettingsMenu { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0003E670 File Offset: 0x0003C870
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x0003E678 File Offset: 0x0003C878
		public RelayCommand LowerIntervallCommand { get; set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x0003E681 File Offset: 0x0003C881
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x0003E689 File Offset: 0x0003C889
		public RelayCommand HigherIntervallCommand { get; set; }

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0003E694 File Offset: 0x0003C894
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x0003E6AC File Offset: 0x0003C8AC
		public string IVPDate
		{
			get
			{
				return this.iVPDate;
			}
			set
			{
				bool flag = this.iVPDate != value;
				if (flag)
				{
					this.iVPDate = value;
					this.dBConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, this.iVPDate);
					this.OnPropertyChanged("IVPDate");
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0003E6F8 File Offset: 0x0003C8F8
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x0003E710 File Offset: 0x0003C910
		public string IVPDateLast
		{
			get
			{
				return this.iVPDateLast;
			}
			set
			{
				bool flag = this.iVPDateLast != value;
				if (flag)
				{
					this.iVPDateLast = value;
					this.OnPropertyChanged("IVPDateLast");
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0003E743 File Offset: 0x0003C943
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0003E74B File Offset: 0x0003C94B
		public IVPTimer IVPTimer { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0003E754 File Offset: 0x0003C954
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x0003E76C File Offset: 0x0003C96C
		public bool OnOff
		{
			get
			{
				return this.onOff;
			}
			set
			{
				bool flag = this.onOff != value;
				if (flag)
				{
					this.onOff = value;
					this.IVPTimer.IVPIsEnabled = value;
					this.dBConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.ValidationIsActive, this.onOff ? "1" : "0");
					bool flag2 = !this.initialize;
					if (flag2)
					{
						if (value)
						{
							LogbookManager.AddEntry(SSTEntryCode.IVPActivated);
						}
						else
						{
							LogbookManager.AddEntry(SSTEntryCode.IVPDeactivated);
						}
					}
					double difference = (this.IVPDateTime - DateTime.Today).TotalDays;
					bool flag3 = false;
					this.dBConnection.GetIntegratedValidationProcess(ref flag3);
					bool flag4 = this.OnOff && flag3;
					if (flag4)
					{
						bool flag5 = this.IVPDateTime.Year == 2999;
						if (flag5)
						{
							this.IVPDateTime = DateTime.Today;
							this.IVPDate = this.IVPDateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
						}
						else
						{
							bool flag6 = difference < 14.0 && difference >= 0.0;
							if (flag6)
							{
								LogbookManager.AddEntry(SSTEntryCode.IVPNeeded, 0, this.IVPDateTime.Year.ToString(), this.IVPDateTime.Month.ToString(), this.IVPDateTime.Day.ToString());
								bool flag7 = Application.Current != null;
								if (flag7)
								{
									Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
									{
										Dialog.Show(LocalizationManager.Localize("txtInfo"), string.Concat(new string[]
										{
											LocalizationManager.Localize("txtValidationIsDue"),
											" ",
											((int)difference).ToString(),
											" ",
											LocalizationManager.Localize("txtDays")
										}), DialogStyle.Info, DialogButtons.OK);
									}));
								}
							}
							else
							{
								bool flag8 = difference < 0.0;
								if (flag8)
								{
									LogbookManager.AddEntry(SSTEntryCode.IVPNeeded, 0, this.IVPDateTime.Year.ToString(), this.IVPDateTime.Month.ToString(), this.IVPDateTime.Day.ToString());
									bool flag9 = Application.Current != null;
									if (flag9)
									{
										Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(delegate()
										{
											Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
											{
												LocalizationManager.Localize("txtValidationWasDue"),
												" ",
												(difference * -1.0).ToString(),
												" ",
												LocalizationManager.Localize("txtDays")
											}), DialogStyle.Error, DialogButtons.OK);
										}));
									}
								}
							}
						}
					}
					this.OnPropertyChanged("onOff");
				}
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0003E9C8 File Offset: 0x0003CBC8
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x0003E9E0 File Offset: 0x0003CBE0
		public int NumberSamples
		{
			get
			{
				return this.numberSamples;
			}
			set
			{
				bool flag = this.numberSamples != value;
				if (flag)
				{
					this.numberSamples = value;
					this.dBConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.NumberOfProductsToConvey, Convert.ToString(this.numberSamples));
					this.OnPropertyChanged("numberSamples");
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0003EA30 File Offset: 0x0003CC30
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x0003EA48 File Offset: 0x0003CC48
		public int Intervall
		{
			get
			{
				return this.intervall;
			}
			set
			{
				this.intervall = value;
				bool flag = this.isIntervall;
				if (flag)
				{
					this.dateDay = this.dateDayLast;
					this.dateMonth = this.dateMonthLast + this.intervall;
					this.dateYear = this.dateYearLast;
					while (this.dateMonth > 12)
					{
						this.dateMonth -= 12;
						this.dateYear++;
					}
					bool flag2 = this.dateYear > 0 && this.dateYear <= 9999 && this.dateMonth > 0 && this.dateMonth <= 12;
					if (flag2)
					{
						this.IVPDateTime = new DateTime(this.dateYear, Math.Min(this.dateMonth, 12), Math.Min(this.dateDay, DateTime.DaysInMonth(this.dateYear, this.dateMonth)));
						bool flag3 = this.IVPDateTime >= DateTime.Now;
						if (flag3)
						{
							this.dBConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.IntervallMonths, Convert.ToString(this.intervall));
							this.IVPDate = this.IVPDateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
							int num = this.dateMonth - 1;
							bool flag4 = num <= 0;
							if (flag4)
							{
								num = 12;
							}
							bool flag5 = (this.IVPDateTime - DateTime.Today).TotalDays > (double)DateTime.DaysInMonth(this.dateYear, num);
							if (flag5)
							{
								this.MinusValid = true;
							}
							else
							{
								this.MinusValid = false;
							}
						}
						else
						{
							this.MinusValid = false;
						}
					}
				}
				this.OnPropertyChanged("Intervall");
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0003EC00 File Offset: 0x0003CE00
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x0003EC18 File Offset: 0x0003CE18
		public bool IsIntervall
		{
			get
			{
				return this.isIntervall;
			}
			set
			{
				this.isIntervall = value;
				bool flag = this.isIntervall;
				if (flag)
				{
					this.Intervall = this.Intervall;
				}
				this.dBConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.IntervallOnOff, this.isIntervall ? "1" : "0");
				this.OnPropertyChanged("IsIntervall");
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0003EC78 File Offset: 0x0003CE78
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x0003EC90 File Offset: 0x0003CE90
		public bool IsManually
		{
			get
			{
				return this.isManually;
			}
			set
			{
				this.isManually = value;
				this.OnPropertyChanged("IsManually");
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x0003ECA8 File Offset: 0x0003CEA8
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x0003ECC0 File Offset: 0x0003CEC0
		public bool RejectAll
		{
			get
			{
				return this.rejectAll;
			}
			set
			{
				bool flag = value && !this.initialize;
				if (flag)
				{
					bool flag2 = this.parHandling != null;
					if (flag2)
					{
						bool flag3 = this.parHandling.ReadPar(Definition.rejectEnable1Path).Equals("1") || this.parHandling.ReadPar(Definition.rejectEnable2Path).Equals("1") || this.parHandling.ReadPar(Definition.rejectEnable3Path).Equals("1") || this.parHandling.ReadPar(Definition.rejectEnable4Path).Equals("1");
						if (flag3)
						{
							this.rejectAll = value;
							this.OnPropertyChanged("RejectAll");
							this.dBConnection.SetRejectAllIVP(this.rejectAll);
						}
						else
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtRejecterIsDisabled"), DialogStyle.Info, DialogButtons.OK);
						}
					}
				}
				else
				{
					this.rejectAll = value;
					this.OnPropertyChanged("RejectAll");
					this.dBConnection.SetRejectAllIVP(this.rejectAll);
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0003EDE0 File Offset: 0x0003CFE0
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x0003EDF8 File Offset: 0x0003CFF8
		public bool MinusValid
		{
			get
			{
				return this.minusValid;
			}
			set
			{
				this.minusValid = value;
				this.OnPropertyChanged("MinusValid");
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0003EE10 File Offset: 0x0003D010
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x0003EE28 File Offset: 0x0003D028
		public bool ProductToConveyVisible
		{
			get
			{
				return this.productToConveyVisible;
			}
			set
			{
				this.productToConveyVisible = value;
				this.OnPropertyChanged("ProductToConveyVisible");
			}
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0003EE40 File Offset: 0x0003D040
		public IVPSettingsViewModel(DBConnection dBConnection, ParameterHandling parHandling, ProductManager productManager)
		{
			this.dBConnection = dBConnection;
			this.parHandling = parHandling;
			this.IVPTimer = new IVPTimer(dBConnection);
			this.Initialize();
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.initialize = false;
			bool flag = productManager != null;
			if (flag)
			{
				bool flag2 = productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
				if (flag2)
				{
					this.ProductToConveyVisible = false;
				}
				else
				{
					this.ProductToConveyVisible = true;
				}
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0003EEF4 File Offset: 0x0003D0F4
		public void SetStartTime()
		{
			string text = "";
			this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, ref text);
			bool flag = text.Length == 10;
			if (flag)
			{
				this.dateDay = Convert.ToInt32(text.Substring(0, 2));
				this.dateMonth = Convert.ToInt32(text.Substring(3, 2));
				this.dateYear = Convert.ToInt32(text.Substring(6, 4));
				this.IVPDateTime = new DateTime(this.dateYear, this.dateMonth, this.dateDay);
				bool flag2 = this.IVPDateTime.Year == 2999;
				if (flag2)
				{
					this.IVPDateTime = DateTime.Now;
				}
				this.IVPDate = this.IVPDateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
				this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.LastValidationDate, ref text);
				bool flag3 = text.Length == 10;
				if (flag3)
				{
					this.dateDayLast = Convert.ToInt32(text.Substring(0, 2));
					this.dateMonthLast = Convert.ToInt32(text.Substring(3, 2));
					this.dateYearLast = Convert.ToInt32(text.Substring(6, 4));
					bool flag4 = this.dateYearLast == 2999;
					if (flag4)
					{
						DateTime now = DateTime.Now;
						this.dateDayLast = now.Day;
						this.dateMonthLast = now.Month;
						this.dateYearLast = now.Year;
						this.dBConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.LastValidationDate, DateTime.Now.ToString("dd.MM.yyy"));
					}
					this.IVPDateLast = new DateTime(this.dateYearLast, this.dateMonthLast, this.dateDayLast).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0003F0BC File Offset: 0x0003D2BC
		public void Initialize()
		{
			string text = "";
			this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, ref text);
			bool flag = text.Length == 10;
			if (flag)
			{
				this.dateDay = Convert.ToInt32(text.Substring(0, 2));
				this.dateMonth = Convert.ToInt32(text.Substring(3, 2));
				this.dateYear = Convert.ToInt32(text.Substring(6, 4));
				this.IVPDateTime = new DateTime(this.dateYear, this.dateMonth, this.dateDay);
				this.IVPDate = this.IVPDateTime.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
			}
			this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationIsActive, ref text);
			this.OnOff = (text == "1");
			this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.LastValidationDate, ref text);
			bool flag2 = text.Length == 10;
			if (flag2)
			{
				this.dateDayLast = Convert.ToInt32(text.Substring(0, 2));
				this.dateMonthLast = Convert.ToInt32(text.Substring(3, 2));
				this.dateYearLast = Convert.ToInt32(text.Substring(6, 4));
				this.IVPDateLast = new DateTime(this.dateYearLast, this.dateMonthLast, this.dateDayLast).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
			}
			this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.IntervallOnOff, ref text);
			bool flag3 = text == "1";
			if (flag3)
			{
				this.isManually = false;
				this.isIntervall = true;
			}
			else
			{
				this.isManually = true;
				this.isIntervall = false;
			}
			this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.IntervallMonths, ref text);
			bool flag4 = text.Length > 0;
			if (flag4)
			{
				this.Intervall = Convert.ToInt32(text);
			}
			this.dBConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.NumberOfProductsToConvey, ref text);
			bool flag5 = text.Length > 0;
			if (flag5)
			{
				this.NumberSamples = Convert.ToInt32(text);
			}
			this.LowerIntervallCommand = new RelayCommand(new Action<object>(this.LowerIntervall));
			this.HigherIntervallCommand = new RelayCommand(new Action<object>(this.HigherIntervall));
			bool flag6 = false;
			this.dBConnection.GetRejectAllIVP(ref flag6);
			this.RejectAll = flag6;
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0003F30C File Offset: 0x0003D50C
		private void HigherIntervall(object obj)
		{
			bool flag = this.Intervall < 12;
			if (flag)
			{
				int num = this.Intervall;
				this.Intervall = num + 1;
			}
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0003F33C File Offset: 0x0003D53C
		private void LowerIntervall(object obj)
		{
			bool flag = this.Intervall > 0;
			if (flag)
			{
				int num = this.Intervall;
				this.Intervall = num - 1;
			}
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0003F36C File Offset: 0x0003D56C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0003F39C File Offset: 0x0003D59C
		private void RegisterLocalizedText()
		{
			this.StrOnOff = new BoundLocalizedText("txtOnOff");
			this.StrNumberTestsamples = new BoundLocalizedText("txtNumImagesValidation");
			this.StrDate = new BoundLocalizedText("txtNextDate");
			this.StrIntervallInMonths = new BoundLocalizedText("txtIntervallInMonths");
			this.StrIntervall = new BoundLocalizedText("txtPerInterval");
			this.StrManually = new BoundLocalizedText("txtManually");
			this.StrLastDate = new BoundLocalizedText("txtLastValidation");
			this.StrSettings = new BoundLocalizedText("txtSettings");
			this.StrRejectAll = new BoundLocalizedText("txtRejectAll");
			this.StrSettingsMenu = new BoundLocalizedText("txtSettings");
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0003F454 File Offset: 0x0003D654
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrOnOff);
			MainWindow.localizationManager.Deregister(this.StrNumberTestsamples);
			MainWindow.localizationManager.Deregister(this.StrDate);
			MainWindow.localizationManager.Deregister(this.StrIntervallInMonths);
			MainWindow.localizationManager.Deregister(this.StrIntervall);
			MainWindow.localizationManager.Deregister(this.StrManually);
			MainWindow.localizationManager.Deregister(this.StrLastDate);
			MainWindow.localizationManager.Deregister(this.StrSettings);
			MainWindow.localizationManager.Deregister(this.StrRejectAll);
			MainWindow.localizationManager.Deregister(this.StrSettingsMenu);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0003F50C File Offset: 0x0003D70C
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0003F518 File Offset: 0x0003D718
		public void getNumKeyboard()
		{
			this.ChangeDateDay();
			this.ChangeDateMonth();
			this.ChangeDateYear();
			DateTime now = DateTime.Now;
			try
			{
				string text = (this.dateDay > 9) ? "" : "0";
				string text2 = (this.dateMonth > 9) ? "" : "0";
				string text3 = string.Concat(new string[]
				{
					text,
					Convert.ToString(this.dateDay),
					".",
					text2,
					Convert.ToString(this.dateMonth),
					".",
					Convert.ToString(this.dateYear)
				});
				bool flag = DateTime.ParseExact(text3, "dd.MM.yyyy", CultureInfo.InvariantCulture).CompareTo(now) < 0;
				if (flag)
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtDateTooEarly"), DialogStyle.Info, DialogButtons.None, true);
				}
				else
				{
					this.IVPDate = text3;
				}
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtWrongInput"), DialogStyle.Info, DialogButtons.None, true);
			}
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0003F654 File Offset: 0x0003D854
		private void ChangeDateDay()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDay"), this.dateDay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateDay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0003F6A7 File Offset: 0x0003D8A7
		private void numKeyboard_InputDateDay(object sender, KeyboardInputEventArgs e)
		{
			this.dateDay = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateDay;
		}

		// Token: 0x06000A95 RID: 2709 RVA: 0x0003F6D4 File Offset: 0x0003D8D4
		private void ChangeDateMonth()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtMonth"), this.dateMonth.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateMonth;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x0003F727 File Offset: 0x0003D927
		private void numKeyboard_InputDateMonth(object sender, KeyboardInputEventArgs e)
		{
			this.dateMonth = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateMonth;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x0003F754 File Offset: 0x0003D954
		private void ChangeDateYear()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtYear"), this.dateYear.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputDateYear;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x0003F7A7 File Offset: 0x0003D9A7
		private void numKeyboard_InputDateYear(object sender, KeyboardInputEventArgs e)
		{
			this.dateYear = int.Parse(e.Text);
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputDateYear;
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0003F7D4 File Offset: 0x0003D9D4
		public void getNumKeyboardNumber()
		{
			string oldValue = this.NumberSamples.ToString();
			this.numKeyboard = new NumKeyboard("", oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_TextInputKeyboardNumber;
			this.numKeyboard.Show();
		}

		// Token: 0x06000A9A RID: 2714 RVA: 0x0003F828 File Offset: 0x0003DA28
		private void keyboard_TextInputKeyboardNumber(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				this.NumberSamples = int.Parse(e.Text);
				bool flag2 = this.NumberSamples > 100;
				if (flag2)
				{
					this.NumberSamples = 100;
				}
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_TextInputKeyboardNumber;
		}

		// Token: 0x04000704 RID: 1796
		private DBConnection dBConnection;

		// Token: 0x04000705 RID: 1797
		private NumKeyboard numKeyboard;

		// Token: 0x04000712 RID: 1810
		private bool initialize = true;

		// Token: 0x04000713 RID: 1811
		private int dateDay;

		// Token: 0x04000714 RID: 1812
		private int dateMonth;

		// Token: 0x04000715 RID: 1813
		private int dateYear;

		// Token: 0x04000716 RID: 1814
		private int dateDayLast;

		// Token: 0x04000717 RID: 1815
		private int dateMonthLast;

		// Token: 0x04000718 RID: 1816
		private int dateYearLast;

		// Token: 0x04000719 RID: 1817
		private string iVPDate;

		// Token: 0x0400071A RID: 1818
		private string iVPDateLast;

		// Token: 0x0400071C RID: 1820
		private DateTime IVPDateTime;

		// Token: 0x0400071D RID: 1821
		private bool onOff;

		// Token: 0x0400071E RID: 1822
		private int numberSamples;

		// Token: 0x0400071F RID: 1823
		private int intervall;

		// Token: 0x04000720 RID: 1824
		private bool isIntervall;

		// Token: 0x04000721 RID: 1825
		private bool isManually;

		// Token: 0x04000722 RID: 1826
		private bool rejectAll = false;

		// Token: 0x04000723 RID: 1827
		private bool minusValid = false;

		// Token: 0x04000724 RID: 1828
		private ParameterHandling parHandling;

		// Token: 0x04000725 RID: 1829
		private bool productToConveyVisible = true;
	}
}

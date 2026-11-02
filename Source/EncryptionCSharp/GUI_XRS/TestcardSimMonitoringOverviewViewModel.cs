using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000D8 RID: 216
	public class TestcardSimMonitoringOverviewViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06001503 RID: 5379 RVA: 0x0007D904 File Offset: 0x0007BB04
		// (remove) Token: 0x06001504 RID: 5380 RVA: 0x0007D93C File Offset: 0x0007BB3C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06001505 RID: 5381 RVA: 0x0007D971 File Offset: 0x0007BB71
		// (set) Token: 0x06001506 RID: 5382 RVA: 0x0007D979 File Offset: 0x0007BB79
		public BoundLocalizedText StrMonitoring { get; set; }

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06001507 RID: 5383 RVA: 0x0007D982 File Offset: 0x0007BB82
		// (set) Token: 0x06001508 RID: 5384 RVA: 0x0007D98A File Offset: 0x0007BB8A
		public ErrorHandling errorHandling { get; set; }

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06001509 RID: 5385 RVA: 0x0007D993 File Offset: 0x0007BB93
		// (set) Token: 0x0600150A RID: 5386 RVA: 0x0007D99B File Offset: 0x0007BB9B
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x0600150B RID: 5387 RVA: 0x0007D9A4 File Offset: 0x0007BBA4
		// (set) Token: 0x0600150C RID: 5388 RVA: 0x0007D9AC File Offset: 0x0007BBAC
		public RelayCommand ChangeTimeIntervallCmd { get; set; }

		// Token: 0x1700065A RID: 1626
		// (get) Token: 0x0600150D RID: 5389 RVA: 0x0007D9B5 File Offset: 0x0007BBB5
		// (set) Token: 0x0600150E RID: 5390 RVA: 0x0007D9BD File Offset: 0x0007BBBD
		public RelayCommand ShowOverviewParameterCommand { get; set; }

		// Token: 0x1700065B RID: 1627
		// (get) Token: 0x0600150F RID: 5391 RVA: 0x0007D9C6 File Offset: 0x0007BBC6
		// (set) Token: 0x06001510 RID: 5392 RVA: 0x0007D9CE File Offset: 0x0007BBCE
		public string CompSensMessageLevel { get; set; }

		// Token: 0x1700065C RID: 1628
		// (get) Token: 0x06001511 RID: 5393 RVA: 0x0007D9D7 File Offset: 0x0007BBD7
		// (set) Token: 0x06001512 RID: 5394 RVA: 0x0007D9DF File Offset: 0x0007BBDF
		public string CompSensTimeIntervall { get; set; }

		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x06001513 RID: 5395 RVA: 0x0007D9E8 File Offset: 0x0007BBE8
		// (set) Token: 0x06001514 RID: 5396 RVA: 0x0007D9F0 File Offset: 0x0007BBF0
		public BoundLocalizedText StrMessageLevel { get; set; }

		// Token: 0x1700065E RID: 1630
		// (get) Token: 0x06001515 RID: 5397 RVA: 0x0007D9F9 File Offset: 0x0007BBF9
		// (set) Token: 0x06001516 RID: 5398 RVA: 0x0007DA01 File Offset: 0x0007BC01
		public BoundLocalizedText StrTimeIntervall { get; set; }

		// Token: 0x1700065F RID: 1631
		// (get) Token: 0x06001517 RID: 5399 RVA: 0x0007DA0A File Offset: 0x0007BC0A
		// (set) Token: 0x06001518 RID: 5400 RVA: 0x0007DA12 File Offset: 0x0007BC12
		public BoundLocalizedText StrSimuAfterTimeIntervall { get; set; }

		// Token: 0x17000660 RID: 1632
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0007DA1C File Offset: 0x0007BC1C
		// (set) Token: 0x0600151A RID: 5402 RVA: 0x0007DA34 File Offset: 0x0007BC34
		public int TimeIntervall
		{
			get
			{
				return this.timeIntervall;
			}
			set
			{
				bool flag = this.timeIntervall != value;
				if (flag)
				{
					this.timeIntervall = value;
					this.OnPropertyChanged("TimeIntervall");
					this.parHandling.WriteValue(Definition.timeIntervallForTestcardSim, this.timeIntervall.ToString());
					this.testcardSimService.SetProperties();
				}
			}
		}

		// Token: 0x17000661 RID: 1633
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x0007DA8F File Offset: 0x0007BC8F
		// (set) Token: 0x0600151C RID: 5404 RVA: 0x0007DA97 File Offset: 0x0007BC97
		public BoundLocalizedText StrInfo { get; set; }

		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x0007DAA0 File Offset: 0x0007BCA0
		// (set) Token: 0x0600151E RID: 5406 RVA: 0x0007DAA8 File Offset: 0x0007BCA8
		public BoundLocalizedText StrWarning { get; set; }

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x0007DAB1 File Offset: 0x0007BCB1
		// (set) Token: 0x06001520 RID: 5408 RVA: 0x0007DAB9 File Offset: 0x0007BCB9
		public BoundLocalizedText StrError { get; set; }

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0007DAC2 File Offset: 0x0007BCC2
		// (set) Token: 0x06001522 RID: 5410 RVA: 0x0007DACA File Offset: 0x0007BCCA
		public List<string> ListMessLev { get; set; }

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06001523 RID: 5411 RVA: 0x0007DAD3 File Offset: 0x0007BCD3
		// (set) Token: 0x06001524 RID: 5412 RVA: 0x0007DADB File Offset: 0x0007BCDB
		public bool ActivateMonitoringParams { get; set; }

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x0007DAE4 File Offset: 0x0007BCE4
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x0007DAFC File Offset: 0x0007BCFC
		public bool SimulationAfterTimeIntervall
		{
			get
			{
				return this.simulationAfterTimeIntervall;
			}
			set
			{
				bool flag = this.simulationAfterTimeIntervall != value;
				if (flag)
				{
					this.simulationAfterTimeIntervall = value;
					bool flag2 = this.simulationAfterTimeIntervall;
					if (flag2)
					{
						bool flag3 = this.parHandling.ReadPar(Definition.simulationAfterAutolearn) == "0";
						if (flag3)
						{
							this.parHandling.WriteValue(Definition.simulationAfterAutolearn, "1");
						}
						this.parHandling.WriteValue(Definition.simulationAfterTimeIntervall, "1");
					}
					else
					{
						this.parHandling.WriteValue(Definition.simulationAfterTimeIntervall, "0");
					}
					this.testcardSimService.SetProperties();
					this.OnPropertyChanged("SimulationAfterTimeIntervall");
				}
			}
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0007DBAC File Offset: 0x0007BDAC
		public TestcardSimMonitoringOverviewViewModel(ParameterHandling parHandling, TestcardSimService testcardSimService, DBConnection dbConnection, ProductManager productManager, ErrorHandling errorHandling, AccessRightsManager accessManager)
		{
			this.parHandling = parHandling;
			this.testcardSimService = testcardSimService;
			this.dbConnection = dbConnection;
			this.productManager = productManager;
			this.errorHandling = errorHandling;
			this.accessManager = accessManager;
			this.btnCsvExport = new Button();
			this.RegisterLocalizedText();
			this.setOverview();
			this.TimeIntervall = Convert.ToInt32(parHandling.ReadPar(Definition.timeIntervallForTestcardSim));
			this.SimulationAfterTimeIntervall = (parHandling.ReadPar(Definition.simulationAfterTimeIntervall) == "1");
			this.CompSensMessageLevel = this.StrMessageLevel.LocalizedText;
			this.CompSensTimeIntervall = this.StrTimeIntervall.LocalizedText;
			this.ChangeTimeIntervallCmd = new RelayCommand(new Action<object>(this.ChangeTimeIntervall));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.initMonitoringMessageLevel();
			this.ActivateMonitoringParams = (accessManager.CurrentUser.Rights <= BuiltInPriviledges.Operator1);
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0007DCBC File Offset: 0x0007BEBC
		private void setOverview()
		{
			TestcardSimMonitoringOverviewViewModel.SensitivityMonitoringParams = new List<TestcardSimMonitoringParams>();
			TestcardSimMonitoringParams testcardSimMonitoringParams = new TestcardSimMonitoringParams();
			string text = "";
			string vaact = "";
			string text2 = "";
			string vacompl = "";
			string text3 = "";
			int num = 0;
			this.dbConnection.GetLengthForMonitoringTestcardSim(ref num, this.productManager.CurrentLoadedProduct.TableName);
			for (int i = 0; i < num; i++)
			{
				this.dbConnection.GetTestcardEntriesForMonitoring(this.productManager.CurrentLoadedProduct.TableName, i, ref text, ref vaact, ref text2, ref vacompl, ref text3);
				bool flag = text != "0";
				if (flag)
				{
					testcardSimMonitoringParams = new TestcardSimMonitoringParams();
					testcardSimMonitoringParams.DTTimeStamp = Convert.ToDateTime(text, new CultureInfo("de-DE"));
					testcardSimMonitoringParams.StrTimeStamp = text;
					testcardSimMonitoringParams.VAAct = vaact;
					testcardSimMonitoringParams.GlAct = text2;
					testcardSimMonitoringParams.VACompl = vacompl;
					testcardSimMonitoringParams.GlCompl = text3;
					testcardSimMonitoringParams.NoComplBackRed = this.setCellBackgroundToRed(vaact, text2, vacompl, text3);
					TestcardSimMonitoringOverviewViewModel.SensitivityMonitoringParams.Add(testcardSimMonitoringParams);
				}
			}
			TestcardSimMonitoringOverviewViewModel.SensitivityMonitoringParams.Sort((TestcardSimMonitoringParams x, TestcardSimMonitoringParams y) => y.DTTimeStamp.CompareTo(x.DTTimeStamp));
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x0007DE08 File Offset: 0x0007C008
		private void initMonitoringMessageLevel()
		{
			int num = 1;
			string localizedText = this.StrWarning.LocalizedText;
			this.dbConnection.GetTestcardSimMonitoringMessLev(ref num);
			bool flag = num == 0;
			if (flag)
			{
				localizedText = this.StrInfo.LocalizedText;
			}
			bool flag2 = num == 2;
			if (flag2)
			{
				localizedText = this.StrError.LocalizedText;
			}
			this.errorHandling.MessageLevelTestcardSimMonitoring = localizedText;
			this.ListMessLev = new List<string>();
			this.ListMessLev.Add(this.StrInfo.LocalizedText);
			this.ListMessLev.Add(this.StrWarning.LocalizedText);
			this.ListMessLev.Add(this.StrError.LocalizedText);
			this.errorHandling.StrInfoTestcardSimMonitoring = this.StrInfo.LocalizedText;
			this.errorHandling.StrWarningTestcardSimMonitoring = this.StrWarning.LocalizedText;
			this.errorHandling.StrErrorTestcardSimMonitoring = this.StrError.LocalizedText;
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0007DEFC File Offset: 0x0007C0FC
		private bool setCellBackgroundToRed(string vaact, string glact, string vacompl, string glcompl)
		{
			int num = this.testcardSimService.ConvertBallSizeToInt("VA", vaact, true);
			int num2 = this.testcardSimService.ConvertBallSizeToInt("Glass", glact, true);
			int num3 = this.testcardSimService.ConvertBallSizeToInt("VA", vacompl, true);
			int num4 = this.testcardSimService.ConvertBallSizeToInt("Glass", glcompl, true);
			return num > num3 || num2 > num4;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0007DF6C File Offset: 0x0007C16C
		public void ChangeTimeIntervall(object obj)
		{
			string oldValue = "";
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtTimeIntervallinMin"), oldValue, NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_ChangeTimeIntervall;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0007DFBC File Offset: 0x0007C1BC
		private void numKeyboard_ChangeTimeIntervall(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				int num = int.Parse(e.Text, new CultureInfo("de-DE"));
				bool flag = num > 0;
				if (flag)
				{
					this.TimeIntervall = num;
				}
				this.numKeyboard.TextInputKeyboard -= this.numKeyboard_ChangeTimeIntervall;
			}
			catch
			{
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0007E024 File Offset: 0x0007C224
		public void CreateCsvForICMSensitivities(ProductManager productManager)
		{
			string text = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff");
			string text2 = "";
			string text3 = "TestcardSim";
			int num = 0;
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			List<string> list = new List<string>();
			string text9 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\TestcardSim\\";
			string text10 = string.Concat(new string[]
			{
				text9,
				"\\TestcardSim_ICM_",
				productManager.CurrentLoadedProduct.ProductName,
				"_",
				text,
				".csv"
			});
			bool flag = !Directory.Exists(text9);
			if (flag)
			{
				Directory.CreateDirectory(text9);
			}
			this.csvwriter = File.CreateText(text10);
			try
			{
				bool flag2 = TestcardSimViewModel.ImgNamesForSensitivities != null;
				if (flag2)
				{
					int count = TestcardSimViewModel.ImgNamesForSensitivities.Count;
				}
				this.dbConnection.GetLengthForMonitoringTestcardSim(ref num, productManager.CurrentLoadedProduct.TableName);
				string value = "Timestamp; VA Actual; Glass Actual; VA Compliance; Glass Compliance; ";
				this.csvwriter.WriteLine(value);
				for (int i = 0; i < num; i++)
				{
					this.dbConnection.GetTestcardEntriesForMonitoring(productManager.CurrentLoadedProduct.TableName, i, ref text4, ref text5, ref text6, ref text7, ref text8);
					bool flag3 = text4 != "0";
					if (flag3)
					{
						string value2 = string.Concat(new string[]
						{
							text4,
							"; ",
							text5,
							"; ",
							text6,
							"; ",
							text7,
							"; ",
							text8,
							"; "
						});
						this.csvwriter.WriteLine(value2);
					}
				}
				this.csvwriter.Close();
				this.csvwriter.Dispose();
				this.dbConnection.GetUsbDriveLetter(ref text2);
				bool flag4 = Directory.Exists(text2);
				if (flag4)
				{
					bool flag5 = !Directory.Exists(text2 + text3);
					if (flag5)
					{
						Directory.CreateDirectory(text2 + "\\" + text3);
					}
					File.Copy(text10, string.Concat(new string[]
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
				}
				Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
			}
			catch (UnauthorizedAccessException)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
				{
					LocalizationManager.Localize("msgPathAccess"),
					" \"",
					Path.GetDirectoryName(text2 + "\\TestcardSim"),
					"\" ",
					LocalizationManager.Localize("msgForbidden")
				}), DialogStyle.Error);
			}
			catch
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error);
			}
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0007E394 File Offset: 0x0007C594
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0007E3C4 File Offset: 0x0007C5C4
		private void RegisterLocalizedText()
		{
			this.StrMonitoring = new BoundLocalizedText("txtMonitoring");
			this.StrMessageLevel = new BoundLocalizedText("txtMessageLevelMonitoring");
			this.StrTimeIntervall = new BoundLocalizedText("txtTimeIntervallinMin");
			this.StrInfo = new BoundLocalizedText("txtInfo");
			this.StrWarning = new BoundLocalizedText("txtWarning");
			this.StrError = new BoundLocalizedText("txtError");
			this.StrSimuAfterTimeIntervall = new BoundLocalizedText("txtMonitoring");
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0007E44C File Offset: 0x0007C64C
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrMonitoring);
			MainWindow.localizationManager.Deregister(this.StrMessageLevel);
			MainWindow.localizationManager.Deregister(this.StrTimeIntervall);
			MainWindow.localizationManager.Deregister(this.StrInfo);
			MainWindow.localizationManager.Deregister(this.StrWarning);
			MainWindow.localizationManager.Deregister(this.StrError);
			MainWindow.localizationManager.Deregister(this.StrSimuAfterTimeIntervall);
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0007E4D1 File Offset: 0x0007C6D1
		private void CleanUp(object obj)
		{
			TestcardSimMonitoringOverviewViewModel.SensitivityMonitoringParams = null;
			this.DeregisterLocalizedText();
		}

		// Token: 0x04000DFA RID: 3578
		private ParameterHandling parHandling;

		// Token: 0x04000DFB RID: 3579
		private TestcardSimService testcardSimService;

		// Token: 0x04000DFC RID: 3580
		private DBConnection dbConnection;

		// Token: 0x04000DFD RID: 3581
		private ProductManager productManager;

		// Token: 0x04000E03 RID: 3587
		public static List<TestcardSimMonitoringParams> SensitivityMonitoringParams;

		// Token: 0x04000E0A RID: 3594
		private NumKeyboard numKeyboard;

		// Token: 0x04000E0B RID: 3595
		private int timeIntervall;

		// Token: 0x04000E11 RID: 3601
		private AccessRightsManager accessManager;

		// Token: 0x04000E12 RID: 3602
		private bool simulationAfterTimeIntervall;

		// Token: 0x04000E13 RID: 3603
		public Button btnCsvExport;

		// Token: 0x04000E14 RID: 3604
		private StreamWriter csvwriter;
	}
}

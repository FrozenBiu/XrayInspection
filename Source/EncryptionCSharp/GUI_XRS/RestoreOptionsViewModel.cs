using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200017D RID: 381
	public class RestoreOptionsViewModel : INotifyPropertyChanged
	{
		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600229A RID: 8858 RVA: 0x000DF9AC File Offset: 0x000DDBAC
		// (set) Token: 0x0600229B RID: 8859 RVA: 0x000DF9B4 File Offset: 0x000DDBB4
		public Restore Restore { get; set; }

		// Token: 0x140000B8 RID: 184
		// (add) Token: 0x0600229C RID: 8860 RVA: 0x000DF9C0 File Offset: 0x000DDBC0
		// (remove) Token: 0x0600229D RID: 8861 RVA: 0x000DF9F8 File Offset: 0x000DDBF8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x000DFA2D File Offset: 0x000DDC2D
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x000DFA35 File Offset: 0x000DDC35
		public BoundLocalizedText StrRestoreDatabase { get; set; }

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x000DFA3E File Offset: 0x000DDC3E
		// (set) Token: 0x060022A1 RID: 8865 RVA: 0x000DFA46 File Offset: 0x000DDC46
		public BoundLocalizedText StrRestoreProductParameters { get; set; }

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x000DFA4F File Offset: 0x000DDC4F
		// (set) Token: 0x060022A3 RID: 8867 RVA: 0x000DFA57 File Offset: 0x000DDC57
		public BoundLocalizedText StrRestoreSystem { get; set; }

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000DFA60 File Offset: 0x000DDC60
		// (set) Token: 0x060022A5 RID: 8869 RVA: 0x000DFA68 File Offset: 0x000DDC68
		public BoundLocalizedText StrRestoreAnomalyDetectionImages { get; set; }

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x060022A6 RID: 8870 RVA: 0x000DFA71 File Offset: 0x000DDC71
		// (set) Token: 0x060022A7 RID: 8871 RVA: 0x000DFA79 File Offset: 0x000DDC79
		public BoundLocalizedText StrChoseBackup { get; set; }

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x060022A8 RID: 8872 RVA: 0x000DFA82 File Offset: 0x000DDC82
		// (set) Token: 0x060022A9 RID: 8873 RVA: 0x000DFA8A File Offset: 0x000DDC8A
		public RelayCommand ChooseBackupCommand { get; set; }

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x060022AA RID: 8874 RVA: 0x000DFA93 File Offset: 0x000DDC93
		// (set) Token: 0x060022AB RID: 8875 RVA: 0x000DFA9B File Offset: 0x000DDC9B
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060022AC RID: 8876 RVA: 0x000DFAA4 File Offset: 0x000DDCA4
		// (set) Token: 0x060022AD RID: 8877 RVA: 0x000DFAAC File Offset: 0x000DDCAC
		public bool RestoreDatabaseVisible { get; set; }

		// Token: 0x060022AE RID: 8878 RVA: 0x000DFAB8 File Offset: 0x000DDCB8
		public RestoreOptionsViewModel(DBConnection dbConnection, ParameterHandling parHandling, Restore restore, AccessRightsManager accessManager, TestcardSimService testcardSimService)
		{
			this.dbConnection = dbConnection;
			this.Restore = restore;
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.testcardSimService = testcardSimService;
			this.ChooseBackupCommand = new RelayCommand(new Action<object>(this.ChooseBackup));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.RegisterLocalizedText();
			dbConnection.GetUsbDriveLetter(ref this.usbLetter);
			this.usbLetter += "\\";
			this.RestoreDatabaseVisible = (accessManager.CurrentUser.Rights >= this.rightRestoreDatabaseVisible);
			bool flag = parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" && parHandling.ReadPar(Definition.imgprocarchitecture) == "1";
			if (flag)
			{
				this.ShowRestoreAnomalyDetectionImages = true;
			}
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000DFBD0 File Offset: 0x000DDDD0
		private void restore_RestoreCompleted()
		{
			Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtRestoreSuccess"), DialogStyle.Ok, DialogButtons.None, true);
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000DFBF0 File Offset: 0x000DDDF0
		private void RegisterLocalizedText()
		{
			this.StrRestoreProductParameters = new BoundLocalizedText("txtRestoreProductParameters");
			this.StrRestoreDatabase = new BoundLocalizedText("txtRestoreDatabase");
			this.StrRestoreSystem = new BoundLocalizedText("txtRestoreSystem");
			this.StrChoseBackup = new BoundLocalizedText("txtChoseBackup");
			this.StrRestoreAnomalyDetectionImages = new BoundLocalizedText("txtRestoreAIImages");
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000DFC54 File Offset: 0x000DDE54
		private void CleanUp(object obj)
		{
			this.testcardSimService.SetProperties();
			MainWindow.localizationManager.Deregister(this.StrRestoreDatabase);
			MainWindow.localizationManager.Deregister(this.StrRestoreSystem);
			MainWindow.localizationManager.Deregister(this.StrChoseBackup);
			MainWindow.localizationManager.Deregister(this.StrRestoreProductParameters);
			MainWindow.localizationManager.Deregister(this.StrRestoreAnomalyDetectionImages);
			this.StopUsbSurvey();
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000DFCCC File Offset: 0x000DDECC
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000DFCFC File Offset: 0x000DDEFC
		public void ChooseBackup(object dummy)
		{
			bool flag = !this.Restore.RestoreProducts && !this.Restore.RestoreSystem;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoOptionChosen"), DialogStyle.Warning);
			}
			else
			{
				CtrlRestoreList ctrlRestoreList = new CtrlRestoreList(this.dbConnection, this.parHandling, this.Restore, false, this.accessManager, this.Restore.RestoreDatabase);
				Subwindow.Show(ctrlRestoreList, SubwindowButtons.BackAndHome, ctrlRestoreList.Buttons);
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000DFD84 File Offset: 0x000DDF84
		public void StartUsbSurvey(FrameworkElement element)
		{
			this.usbSurveyRunning = true;
			this.usbElement = element;
			new Thread(new ThreadStart(this.UsbSurvey))
			{
				Name = "UsbSurveyRestore"
			}.Start();
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000DFDC5 File Offset: 0x000DDFC5
		public void StopUsbSurvey()
		{
			this.usbSurveyRunning = false;
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000DFDD0 File Offset: 0x000DDFD0
		private void UsbSurvey()
		{
			try
			{
				while (this.usbSurveyRunning)
				{
					this.SetUsbVisibility();
					Thread.Sleep(this.surveyPeriod);
				}
			}
			catch (Exception ex)
			{
				Exception ex3;
				Exception ex2 = ex3;
				Exception ex = ex2;
				bool flag = Application.Current != null;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
					{
						Dialog.Show("Error", ex.Message, DialogStyle.Error, DialogButtons.OK);
					}));
				}
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000DFE50 File Offset: 0x000DE050
		private void SetUsbVisibility()
		{
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				bool flag = this.usbLetter != "\\" && Directory.Exists(this.usbLetter);
				if (flag)
				{
					this.usbElement.Visibility = Visibility.Visible;
				}
				else
				{
					this.usbElement.Visibility = Visibility.Hidden;
				}
			}));
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x060022B8 RID: 8888 RVA: 0x000DFE74 File Offset: 0x000DE074
		// (set) Token: 0x060022B9 RID: 8889 RVA: 0x000DFE8C File Offset: 0x000DE08C
		public bool ShowRestoreAnomalyDetectionImages
		{
			get
			{
				return this.showRestoreAnomalyDetectionImages;
			}
			set
			{
				this.showRestoreAnomalyDetectionImages = value;
				this.OnPropertyChanged("ShowRestoreAnomalyDetectionImages");
			}
		}

		// Token: 0x04001677 RID: 5751
		private BuiltInPriviledges rightRestoreDatabaseVisible = BuiltInPriviledges.Service;

		// Token: 0x04001678 RID: 5752
		private DBConnection dbConnection;

		// Token: 0x04001679 RID: 5753
		private bool usbSurveyRunning;

		// Token: 0x0400167A RID: 5754
		private FrameworkElement usbElement;

		// Token: 0x0400167B RID: 5755
		private int surveyPeriod = 1000;

		// Token: 0x0400167C RID: 5756
		private string usbLetter = "";

		// Token: 0x0400167D RID: 5757
		private AccessRightsManager accessManager;

		// Token: 0x0400167E RID: 5758
		private ParameterHandling parHandling;

		// Token: 0x0400167F RID: 5759
		private bool showRestoreAnomalyDetectionImages = false;

		// Token: 0x0400168A RID: 5770
		private TestcardSimService testcardSimService;
	}
}

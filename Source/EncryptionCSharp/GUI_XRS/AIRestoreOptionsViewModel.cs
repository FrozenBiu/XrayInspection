using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200000F RID: 15
	public class AIRestoreOptionsViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060000BF RID: 191 RVA: 0x000058A0 File Offset: 0x00003AA0
		// (remove) Token: 0x060000C0 RID: 192 RVA: 0x000058D8 File Offset: 0x00003AD8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x0000590D File Offset: 0x00003B0D
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00005915 File Offset: 0x00003B15
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x0000591E File Offset: 0x00003B1E
		// (set) Token: 0x060000C4 RID: 196 RVA: 0x00005926 File Offset: 0x00003B26
		public RelayCommand ChooseBackupCommand { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x0000592F File Offset: 0x00003B2F
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00005937 File Offset: 0x00003B37
		public BoundLocalizedText StrRestoreXA { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005940 File Offset: 0x00003B40
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x00005948 File Offset: 0x00003B48
		public BoundLocalizedText StrRestoreXO { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x00005951 File Offset: 0x00003B51
		// (set) Token: 0x060000CA RID: 202 RVA: 0x00005959 File Offset: 0x00003B59
		public BoundLocalizedText StrChoseBackup { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005962 File Offset: 0x00003B62
		// (set) Token: 0x060000CC RID: 204 RVA: 0x0000596A File Offset: 0x00003B6A
		public bool RestoreXAVisible { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000CD RID: 205 RVA: 0x00005973 File Offset: 0x00003B73
		// (set) Token: 0x060000CE RID: 206 RVA: 0x0000597B File Offset: 0x00003B7B
		public bool RestoreXOVisible { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005984 File Offset: 0x00003B84
		// (set) Token: 0x060000D0 RID: 208 RVA: 0x0000598C File Offset: 0x00003B8C
		public bool RestoreXA
		{
			get
			{
				return this.restoreXA;
			}
			set
			{
				bool flag = value != this.restoreXA;
				if (flag)
				{
					this.restoreXA = value;
					this.OnPropertyChanged("RestoreXA");
				}
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000059BF File Offset: 0x00003BBF
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x000059C8 File Offset: 0x00003BC8
		public bool RestoreXO
		{
			get
			{
				return this.restoreXO;
			}
			set
			{
				bool flag = value != this.restoreXO;
				if (flag)
				{
					this.restoreXO = value;
					this.OnPropertyChanged("RestoreXO");
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000059FC File Offset: 0x00003BFC
		public AIRestoreOptionsViewModel(DBConnection dbConnection, ParameterHandling parHandling, ProductManager productManager)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.ChooseBackupCommand = new RelayCommand(new Action<object>(this.ChooseBackup));
			this.RegisterLocalizedText();
			dbConnection.GetUsbDriveLetter(ref this.usbLetter);
			this.usbLetter += "\\";
			bool restoreXAVisible = false;
			bool restoreXOVisible = false;
			dbConnection.GetAnomalyDetectionEnabled(ref restoreXAVisible);
			dbConnection.GetObjectDetectionEnabled(ref restoreXOVisible);
			this.RestoreXAVisible = restoreXAVisible;
			this.RestoreXOVisible = restoreXOVisible;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00005ABC File Offset: 0x00003CBC
		private void RegisterLocalizedText()
		{
			this.StrRestoreXA = new BoundLocalizedText("txtRestoreThinkXA");
			this.StrRestoreXO = new BoundLocalizedText("txtRestoreThinkXO");
			this.StrChoseBackup = new BoundLocalizedText("txtChoseBackup");
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00005AF4 File Offset: 0x00003CF4
		public void StartUsbSurvey(FrameworkElement element)
		{
			this.usbSurveyRunning = true;
			this.usbElement = element;
			new Thread(new ThreadStart(this.UsbSurvey))
			{
				Name = "UsbSurveyRestore"
			}.Start();
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00005B35 File Offset: 0x00003D35
		public void StopUsbSurvey()
		{
			this.usbSurveyRunning = false;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00005B40 File Offset: 0x00003D40
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

		// Token: 0x060000D8 RID: 216 RVA: 0x00005BC0 File Offset: 0x00003DC0
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

		// Token: 0x060000D9 RID: 217 RVA: 0x00005BE4 File Offset: 0x00003DE4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00005C11 File Offset: 0x00003E11
		private void CleanUp(object obj)
		{
			MainWindow.localizationManager.Deregister(this.StrRestoreXA);
			MainWindow.localizationManager.Deregister(this.StrRestoreXO);
			MainWindow.localizationManager.Deregister(this.StrChoseBackup);
			this.StopUsbSurvey();
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005C50 File Offset: 0x00003E50
		public void ChooseBackup(object dummy)
		{
			bool flag = !this.RestoreXA && !this.RestoreXO;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNoOptionChosen"), DialogStyle.Warning);
			}
			else
			{
				CtrlAIRestoreListBackups ctrlAIRestoreListBackups = new CtrlAIRestoreListBackups(this.dbConnection, this.parHandling, this.RestoreXA, this.RestoreXO, this.productManager);
				Subwindow.Show(ctrlAIRestoreListBackups, SubwindowButtons.BackAndHome, ctrlAIRestoreListBackups.Buttons);
			}
		}

		// Token: 0x0400010B RID: 267
		private bool usbSurveyRunning;

		// Token: 0x0400010C RID: 268
		private FrameworkElement usbElement;

		// Token: 0x0400010D RID: 269
		private int surveyPeriod = 1000;

		// Token: 0x0400010E RID: 270
		private string usbLetter = "";

		// Token: 0x0400010F RID: 271
		private DBConnection dbConnection;

		// Token: 0x04000110 RID: 272
		private ParameterHandling parHandling;

		// Token: 0x04000111 RID: 273
		private ProductManager productManager;

		// Token: 0x0400011A RID: 282
		private bool restoreXA;

		// Token: 0x0400011B RID: 283
		private bool restoreXO;
	}
}

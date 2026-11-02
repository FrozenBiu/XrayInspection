using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000009 RID: 9
	internal class AIMenuSelectionViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000046 RID: 70 RVA: 0x00002ECC File Offset: 0x000010CC
		// (remove) Token: 0x06000047 RID: 71 RVA: 0x00002F04 File Offset: 0x00001104
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002F39 File Offset: 0x00001139
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002F41 File Offset: 0x00001141
		public RelayCommand ShowThinkXACommand { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002F4A File Offset: 0x0000114A
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002F52 File Offset: 0x00001152
		public RelayCommand ShowThinkXOCommand { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002F5B File Offset: 0x0000115B
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002F63 File Offset: 0x00001163
		public RelayCommand RestoreModelCommand { get; set; }

		// Token: 0x0600004E RID: 78 RVA: 0x00002F6C File Offset: 0x0000116C
		public AIMenuSelectionViewModel(ParameterHandling parHandling, ProductManager productManager, DBConnection dbConnection, AccessRightsManager accessManager, ImageDataTransfer imageTransfer, StoreImageData imageArchiver, ErrorHandling errorHandling, TestcardSimService testcardSimService, AIService aiService, CtrlAIMenuSelection.SelectionMenuContext context)
		{
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.imageTransfer = imageTransfer;
			this.imageArchiver = imageArchiver;
			this.errorHandling = errorHandling;
			this.testcardSimService = testcardSimService;
			this.aiService = aiService;
			bool flag = context == CtrlAIMenuSelection.SelectionMenuContext.MainMenu;
			if (flag)
			{
				this.RestoreModelCommand = new RelayCommand(new Action<object>(this.RestoreModel));
				this.btnRestoreModel = new Button();
				this.ShowThinkXACommand = new RelayCommand(new Action<object>(this.ShowThinkXAFineTuningMenu));
				this.ShowThinkXOCommand = new RelayCommand(new Action<object>(this.ShowThinkXOMenu));
			}
			else
			{
				bool flag2 = context == CtrlAIMenuSelection.SelectionMenuContext.StatusMenu;
				if (flag2)
				{
					this.ShowThinkXACommand = new RelayCommand(new Action<object>(this.ShowThinkXAStatusMenu));
					this.ShowThinkXOCommand = new RelayCommand(new Action<object>(this.ShowThinkXOStatusMenu));
				}
			}
			this.Initialize();
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000307E File Offset: 0x0000127E
		private void Initialize()
		{
			this.aiService.UpdateAvailabilityModelFiles();
			this.RefreshColourBtnAIStatus();
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003094 File Offset: 0x00001294
		public void ShowThinkXAFineTuningMenu(object dummy)
		{
			bool flag = false;
			this.dbConnection.GetAnomalyDetectionEnabled(ref flag);
			bool flag2 = !flag;
			if (flag2)
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgSoftwareFeatureDisabled"), DialogStyle.Info, DialogButtons.OK);
			}
			else
			{
				CtrlAI ctrlAI = new CtrlAI(this.parHandling, this.imageArchiver, this.accessManager, this.imageTransfer, this.dbConnection, this.productManager, this.aiService, this.errorHandling, this.testcardSimService);
				EventHandler handler = null;
				handler = delegate(object sender, EventArgs args)
				{
					this.aiService.UpdateAvailabilityModelFiles();
					this.RefreshColourBtnAIStatus();
					ctrlAI.UpdateColourBtnSelectionMenu -= handler;
				};
				ctrlAI.UpdateColourBtnSelectionMenu += handler;
				Subwindow.Show(ctrlAI, SubwindowButtons.BackAndHome, ctrlAI.PanelButtons, ctrlAI.UiElementRightAligned);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000317C File Offset: 0x0000137C
		public void ShowThinkXOMenu(object dummy)
		{
			bool flag = false;
			this.dbConnection.GetObjectDetectionEnabled(ref flag);
			bool flag2 = this.parHandling.ReadPar(Definition.filterMainPath + ObjectCounter.enabledSubPath) == "1";
			bool flag3 = !flag;
			if (flag3)
			{
				Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgSoftwareFeatureDisabled"), DialogStyle.Info, DialogButtons.OK);
			}
			else
			{
				bool flag4 = flag2;
				if (flag4)
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgObjectCounterEnabled"), DialogStyle.Info, DialogButtons.OK);
				}
				else
				{
					bool flag5 = this.parHandling.ReadPar(Definition.objectDetectionTensorRTEngineAvailable) == "1" || this.parHandling.ReadPar(Definition.objectDetectionOnnxAvailable) == "1";
					if (flag5)
					{
						CtrlAIObject ctrlAIObject = new CtrlAIObject(this.parHandling, this.imageArchiver, this.imageTransfer, this.productManager, this.dbConnection);
						EventHandler handler = null;
						handler = delegate(object sender, EventArgs args)
						{
							this.RefreshColourBtnAIStatus();
							ctrlAIObject.UpdateColourBtnSelectionMenu -= handler;
						};
						ctrlAIObject.UpdateColourBtnSelectionMenu += handler;
						Subwindow.Show(ctrlAIObject, SubwindowButtons.BackAndHome, ctrlAIObject.PanelButtons, ctrlAIObject.UiElementRightAligned);
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgThinkXoNoModel"), DialogStyle.Info, DialogButtons.OK);
					}
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003300 File Offset: 0x00001500
		public void ShowThinkXAStatusMenu(object dummy)
		{
			CtrlAIStatus ctrlstatus = new CtrlAIStatus(this.parHandling, this.accessManager, this.dbConnection, this.productManager, this.aiService, this.testcardSimService, this.imageTransfer, this.imageArchiver);
			EventHandler handler = null;
			handler = delegate(object sender, EventArgs args)
			{
				this.RefreshColourBtnAIStatus();
				ctrlstatus.UpdateColourBtnSelectionMenu -= handler;
			};
			ctrlstatus.UpdateColourBtnSelectionMenu += handler;
			Subwindow.Show(ctrlstatus, SubwindowButtons.BackAndHome, ctrlstatus.PanelButtons, ctrlstatus.UiElementRightAligned);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000033A4 File Offset: 0x000015A4
		public void ShowThinkXOStatusMenu(object dummy)
		{
			CtrlAIObjectStatus ctrlAIObjectStatus = new CtrlAIObjectStatus(this.parHandling, this.accessManager, this.productManager);
			EventHandler handler = null;
			handler = delegate(object sender, EventArgs args)
			{
				this.RefreshColourBtnAIStatus();
				ctrlAIObjectStatus.UpdateColourBtnSelectionMenu -= handler;
			};
			ctrlAIObjectStatus.UpdateColourBtnSelectionMenu += handler;
			Subwindow.Show(ctrlAIObjectStatus, SubwindowButtons.BackAndHome, ctrlAIObjectStatus.PanelButtons, ctrlAIObjectStatus.UiElementRightAligned);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000342C File Offset: 0x0000162C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000345C File Offset: 0x0000165C
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003474 File Offset: 0x00001674
		public string ImageSourceThinkXA
		{
			get
			{
				return this.imageSourceThinkXA;
			}
			set
			{
				bool flag = this.imageSourceThinkXA != value;
				if (flag)
				{
					this.imageSourceThinkXA = value;
					this.OnPropertyChanged("ImageSourceThinkXA");
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000034A8 File Offset: 0x000016A8
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000034C0 File Offset: 0x000016C0
		public string ImageSourceThinkXO
		{
			get
			{
				return this.imageSourceThinkXO;
			}
			set
			{
				bool flag = this.imageSourceThinkXO != value;
				if (flag)
				{
					this.imageSourceThinkXO = value;
					this.OnPropertyChanged("ImageSourceThinkXO");
				}
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000034F4 File Offset: 0x000016F4
		private void RefreshColourBtnAIStatus()
		{
			bool flag = this.parHandling.ReadPar(Definition.anomalyDetectionInferenceEnabled) == "1";
			bool flag2 = this.parHandling.ReadPar(Definition.anomalyDetectionTensorRTEngineAvailable) == "1" || this.parHandling.ReadPar(Definition.anomalyDetectionOnnxAvailable) == "1";
			bool flag3 = flag2 && flag;
			if (flag3)
			{
				this.ImageSourceThinkXA = "../img/ai_brain_green.png";
			}
			else
			{
				bool flag4 = flag2 && !flag;
				if (flag4)
				{
					this.ImageSourceThinkXA = "../img/ai_brain_blue.png";
				}
				else
				{
					this.ImageSourceThinkXA = "../img/ai_brain.png";
				}
			}
			bool flag5 = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			bool flag6 = this.parHandling.ReadPar(Definition.objectDetectionTensorRTEngineAvailable) == "1" || this.parHandling.ReadPar(Definition.objectDetectionOnnxAvailable) == "1";
			bool flag7 = flag6 && flag5;
			if (flag7)
			{
				this.ImageSourceThinkXO = "../img/ai_brain_green.png";
			}
			else
			{
				bool flag8 = flag6 && !flag5;
				if (flag8)
				{
					this.ImageSourceThinkXO = "../img/ai_brain_blue.png";
				}
				else
				{
					this.ImageSourceThinkXO = "../img/ai_brain.png";
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003630 File Offset: 0x00001830
		public void RestoreModel(object obj)
		{
			CtrlAIRestoreOptions ctrlAIRestoreOptions = new CtrlAIRestoreOptions(this.dbConnection, this.parHandling, this.productManager);
			EventHandler handler = null;
			handler = delegate(object sender, EventArgs args)
			{
				this.aiService.UpdateAvailabilityModelFiles();
				this.RefreshColourBtnAIStatus();
				ctrlAIRestoreOptions.UpdateColourBtnSelectionMenu -= handler;
			};
			ctrlAIRestoreOptions.UpdateColourBtnSelectionMenu += handler;
			Subwindow.Show(ctrlAIRestoreOptions, SubwindowButtons.BackAndHome, ctrlAIRestoreOptions.Buttons);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000036AB File Offset: 0x000018AB
		public void CleanUp()
		{
			this.btnRestoreModel = null;
			this.RestoreModelCommand = null;
		}

		// Token: 0x040000B3 RID: 179
		private string imageSourceThinkXA = "../img/ai_brain.png";

		// Token: 0x040000B4 RID: 180
		private string imageSourceThinkXO = "../img/ai_brain.png";

		// Token: 0x040000B5 RID: 181
		private ParameterHandling parHandling;

		// Token: 0x040000B6 RID: 182
		private ProductManager productManager;

		// Token: 0x040000B7 RID: 183
		private DBConnection dbConnection;

		// Token: 0x040000B8 RID: 184
		private AccessRightsManager accessManager;

		// Token: 0x040000B9 RID: 185
		private ImageDataTransfer imageTransfer;

		// Token: 0x040000BA RID: 186
		private StoreImageData imageArchiver;

		// Token: 0x040000BB RID: 187
		private ErrorHandling errorHandling;

		// Token: 0x040000BC RID: 188
		private TestcardSimService testcardSimService;

		// Token: 0x040000BD RID: 189
		private AIService aiService;

		// Token: 0x040000C1 RID: 193
		public Button btnRestoreModel;
	}
}

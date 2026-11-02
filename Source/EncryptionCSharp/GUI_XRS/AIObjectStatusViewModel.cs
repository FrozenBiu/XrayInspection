using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200000C RID: 12
	internal class AIObjectStatusViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600007C RID: 124 RVA: 0x000044DC File Offset: 0x000026DC
		// (remove) Token: 0x0600007D RID: 125 RVA: 0x00004514 File Offset: 0x00002714
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00004549 File Offset: 0x00002749
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00004551 File Offset: 0x00002751
		public BoundLocalizedText StrInferenceEnabled { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000080 RID: 128 RVA: 0x0000455A File Offset: 0x0000275A
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00004562 File Offset: 0x00002762
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000082 RID: 130 RVA: 0x0000456C File Offset: 0x0000276C
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00004584 File Offset: 0x00002784
		public bool ControlsDisabled
		{
			get
			{
				return this.controlsDisabled;
			}
			set
			{
				bool flag = this.controlsDisabled != value;
				if (flag)
				{
					this.controlsDisabled = value;
					this.OnPropertyChanged("ControlsDisabled");
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000084 RID: 132 RVA: 0x000045B8 File Offset: 0x000027B8
		// (set) Token: 0x06000085 RID: 133 RVA: 0x000045D0 File Offset: 0x000027D0
		public bool ObjectDetectionInferenceOnOff
		{
			get
			{
				return this.objectDetectionInferenceOnOff;
			}
			set
			{
				bool flag = this.objectDetectionInferenceOnOff != value;
				if (flag)
				{
					bool flag2 = !value;
					if (flag2)
					{
						string message = LocalizationManager.Localize("txtAnomalyDetectionDisableInference");
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), message, DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
						bool flag4 = flag3;
						if (!flag4)
						{
							return;
						}
						this.parHandling.WriteValue(Definition.objectDetectionInferenceEnabled, "0");
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 3, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
					}
					else
					{
						bool flag5 = value && (this.parHandling.ReadPar(Definition.objectDetectionTensorRTEngineAvailable) == "1" || this.parHandling.ReadPar(Definition.objectDetectionOnnxAvailable) == "1");
						if (!flag5)
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtAnomalyDetectionNoModel"), DialogStyle.Info, DialogButtons.OK);
							return;
						}
						bool flag6 = this.parHandling.ReadPar(Definition.filterMainPath + ObjectCounter.enabledSubPath) == "1";
						bool flag7 = flag6;
						if (flag7)
						{
							Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("msgObjectCounterEnabled"), DialogStyle.Info, DialogButtons.OK);
							return;
						}
						this.parHandling.WriteValue(Definition.objectDetectionInferenceEnabled, "1");
						LogbookManager.AddEntry(SSTEntryCode.AIDetectionActivated, 3, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
					}
					this.objectDetectionInferenceOnOff = value;
					this.OnPropertyChanged("ObjectDetectionInferenceOnOff");
				}
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004788 File Offset: 0x00002988
		public AIObjectStatusViewModel(ParameterHandling parHandling, AccessRightsManager accessManager, ProductManager productManager)
		{
			this.parHandling = parHandling;
			this.accessManager = accessManager;
			this.productManager = productManager;
			this.Initialize();
			this.RegisterLocalizedText();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000047D8 File Offset: 0x000029D8
		private void Initialize()
		{
			bool flag = this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin;
			if (flag)
			{
				this.ControlsDisabled = false;
			}
			else
			{
				this.ControlsDisabled = true;
			}
			this.objectDetectionInferenceOnOff = (this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1");
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000483C File Offset: 0x00002A3C
		private void RegisterLocalizedText()
		{
			this.StrInferenceEnabled = new BoundLocalizedText("txtInferenceEnabled");
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004850 File Offset: 0x00002A50
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000487D File Offset: 0x00002A7D
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00004887 File Offset: 0x00002A87
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrInferenceEnabled);
		}

		// Token: 0x040000E5 RID: 229
		private ParameterHandling parHandling;

		// Token: 0x040000E6 RID: 230
		private AccessRightsManager accessManager;

		// Token: 0x040000E7 RID: 231
		private ProductManager productManager;

		// Token: 0x040000EB RID: 235
		private bool controlsDisabled;

		// Token: 0x040000EC RID: 236
		private bool objectDetectionInferenceOnOff;
	}
}

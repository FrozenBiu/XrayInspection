using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000AB RID: 171
	public class CanFilterViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000056 RID: 86
		// (add) Token: 0x060010DD RID: 4317 RVA: 0x00067B6C File Offset: 0x00065D6C
		// (remove) Token: 0x060010DE RID: 4318 RVA: 0x00067BA4 File Offset: 0x00065DA4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00067BDC File Offset: 0x00065DDC
		public CanFilter CanFilter
		{
			get
			{
				return this.canFilter;
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x060010E0 RID: 4320 RVA: 0x00067BF4 File Offset: 0x00065DF4
		// (set) Token: 0x060010E1 RID: 4321 RVA: 0x00067C0C File Offset: 0x00065E0C
		public XRSImage CurrentImage
		{
			get
			{
				return this.currentImage;
			}
			set
			{
				this.currentImage = value;
				this.OnPropertyChanged("CurrentImage");
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00067C24 File Offset: 0x00065E24
		// (set) Token: 0x060010E3 RID: 4323 RVA: 0x00067C3C File Offset: 0x00065E3C
		public int MaxCenterX
		{
			get
			{
				return this.maxCenterX;
			}
			set
			{
				this.maxCenterX = value;
				this.OnPropertyChanged("MaxCenterX");
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060010E4 RID: 4324 RVA: 0x00067C54 File Offset: 0x00065E54
		// (set) Token: 0x060010E5 RID: 4325 RVA: 0x00067C6C File Offset: 0x00065E6C
		public int MaxCenterY
		{
			get
			{
				return this.maxCenterY;
			}
			set
			{
				this.maxCenterY = value;
				this.OnPropertyChanged("MaxCenterY");
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x060010E6 RID: 4326 RVA: 0x00067C84 File Offset: 0x00065E84
		// (set) Token: 0x060010E7 RID: 4327 RVA: 0x00067C9C File Offset: 0x00065E9C
		public int ScaleOfY
		{
			get
			{
				return this.scaleOfY;
			}
			set
			{
				this.scaleOfY = value;
				this.OnPropertyChanged("ScaleOfY");
			}
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x060010E8 RID: 4328 RVA: 0x00067CB2 File Offset: 0x00065EB2
		// (set) Token: 0x060010E9 RID: 4329 RVA: 0x00067CBA File Offset: 0x00065EBA
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x060010EA RID: 4330 RVA: 0x00067CC3 File Offset: 0x00065EC3
		// (set) Token: 0x060010EB RID: 4331 RVA: 0x00067CCB File Offset: 0x00065ECB
		public RelayCommand ChangeCenterXCommand { get; set; }

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060010EC RID: 4332 RVA: 0x00067CD4 File Offset: 0x00065ED4
		// (set) Token: 0x060010ED RID: 4333 RVA: 0x00067CDC File Offset: 0x00065EDC
		public RelayCommand ChangeCenterYCommand { get; set; }

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x00067CE5 File Offset: 0x00065EE5
		// (set) Token: 0x060010EF RID: 4335 RVA: 0x00067CED File Offset: 0x00065EED
		public RelayCommand ChangeRadiusXCommand { get; set; }

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060010F0 RID: 4336 RVA: 0x00067CF6 File Offset: 0x00065EF6
		// (set) Token: 0x060010F1 RID: 4337 RVA: 0x00067CFE File Offset: 0x00065EFE
		public RelayCommand ChangeRadiusYCommand { get; set; }

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00067D07 File Offset: 0x00065F07
		// (set) Token: 0x060010F3 RID: 4339 RVA: 0x00067D0F File Offset: 0x00065F0F
		public RelayCommand ChangeNPixelCommand { get; set; }

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00067D18 File Offset: 0x00065F18
		// (set) Token: 0x060010F5 RID: 4341 RVA: 0x00067D20 File Offset: 0x00065F20
		public BoundLocalizedText StrThickness { get; set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00067D29 File Offset: 0x00065F29
		// (set) Token: 0x060010F7 RID: 4343 RVA: 0x00067D31 File Offset: 0x00065F31
		public BoundLocalizedText StrWidth { get; set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060010F8 RID: 4344 RVA: 0x00067D3A File Offset: 0x00065F3A
		// (set) Token: 0x060010F9 RID: 4345 RVA: 0x00067D42 File Offset: 0x00065F42
		public BoundLocalizedText StrHeight { get; set; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060010FA RID: 4346 RVA: 0x00067D4B File Offset: 0x00065F4B
		// (set) Token: 0x060010FB RID: 4347 RVA: 0x00067D53 File Offset: 0x00065F53
		public BoundLocalizedText StrPositionX { get; set; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x00067D5C File Offset: 0x00065F5C
		// (set) Token: 0x060010FD RID: 4349 RVA: 0x00067D64 File Offset: 0x00065F64
		public BoundLocalizedText StrPositionY { get; set; }

		// Token: 0x060010FE RID: 4350 RVA: 0x00067D70 File Offset: 0x00065F70
		public CanFilterViewModel(ParameterHandling parHandling, ImageDataTransfer imageTransfer, DBConnection dbConnection, ProductManager productManager, MachineState machineState, int imageGroup, StoreImageData imageArchiver)
		{
			this.imageGroup = imageGroup;
			this.parHandling = parHandling;
			this.imageTransfer = imageTransfer;
			this.machineState = machineState;
			this.productManager = productManager;
			this.imageArchiver = imageArchiver;
			this.ChangeCenterXCommand = new RelayCommand(new Action<object>(this.ChangeCenterX));
			this.ChangeCenterYCommand = new RelayCommand(new Action<object>(this.ChangeCenterY));
			this.ChangeRadiusXCommand = new RelayCommand(new Action<object>(this.ChangeRadiusX));
			this.ChangeRadiusYCommand = new RelayCommand(new Action<object>(this.ChangeRadiusY));
			this.ChangeNPixelCommand = new RelayCommand(new Action<object>(this.ChangeNPixel));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.canFilter = new CanFilter(parHandling, imageTransfer, dbConnection, productManager, imageGroup);
			parHandling.EdgeFilterMode = true;
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.timer = new Timer(new TimerCallback(this.LoadLastImage));
			this.timer.Change(100, -1);
			this.scaleOfY = -1;
			this.RegisterLocalizedText();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00067EA4 File Offset: 0x000660A4
		private void RegisterLocalizedText()
		{
			this.StrThickness = new BoundLocalizedText("txtThickness");
			this.StrWidth = new BoundLocalizedText("txtWidth");
			this.StrHeight = new BoundLocalizedText("txtHeight");
			this.StrPositionX = new BoundLocalizedText("txtPositionX");
			this.StrPositionY = new BoundLocalizedText("txtPositionY");
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00067F08 File Offset: 0x00066108
		private void LoadLastImage(object obj)
		{
			bool flag = this.timer != null;
			if (flag)
			{
				this.timer.Dispose();
			}
			this.currentImage = HistoryHelper.GetLatestImage((!this.productManager.CurrentLoadedProduct.IsRegionProduct) ? this.imageGroup : 0, this.imageArchiver);
			this.originalImage = ((this.currentImage != null) ? this.currentImage.DeepCopy() : null);
			bool flag2 = this.currentImage != null;
			if (flag2)
			{
				string text = (this.currentImage.Group != 0) ? this.currentImage.Group.ToString() : "";
				this.MaxCenterX = (int)this.currentImage.Width;
				this.MaxCenterY = (int)this.currentImage.Height;
				this.ScaleOfY = ((this.currentImage.ConveyingDirection == ConveyingDirection.LeftToRight) ? -1 : 1);
				this.imageTransfer.SendImage(this.currentImage);
			}
			else
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
				}));
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00068034 File Offset: 0x00066234
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00068064 File Offset: 0x00066264
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = this.machineState.State != MachineStates.Reseasoning && this.machineState.State != MachineStates.XrayOnCalibrationNotDone && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					bool flag2 = (!this.productManager.CurrentLoadedProduct.IsRegionProduct && this.imageGroup == (int)e.Image.Group) || (this.productManager.CurrentLoadedProduct.IsRegionProduct && e.Image.Group == 0);
					if (flag2)
					{
						XRSImage xrsimage = e.Image.DeepCopy();
						bool flag3 = this.originalImage != null;
						if (flag3)
						{
							xrsimage.Regions = this.originalImage.Regions;
						}
						this.productManager.PrepareRegionImage(xrsimage, this.imageGroup);
						this.CurrentImage = xrsimage;
					}
				}));
			}
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x000680E0 File Offset: 0x000662E0
		private void CleanUp(object obj)
		{
			bool flag = this.currentImage != null;
			if (flag)
			{
				string text = (this.currentImage.Group != 0) ? this.currentImage.Group.ToString() : "";
			}
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.parHandling.EdgeFilterMode = false;
			MainWindow.localizationManager.Deregister(this.StrThickness);
			MainWindow.localizationManager.Deregister(this.StrWidth);
			MainWindow.localizationManager.Deregister(this.StrHeight);
			MainWindow.localizationManager.Deregister(this.StrPositionX);
			MainWindow.localizationManager.Deregister(this.StrPositionY);
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x000681A4 File Offset: 0x000663A4
		public void ShowLearnWindow()
		{
			bool flag = this.currentImage != null && this.currentImage.Height > 0 && this.currentImage.Width > 0;
			if (flag)
			{
				CtrlLearnCanFilter ctrlLearnCanFilter = new CtrlLearnCanFilter(this.CurrentImage, this.parHandling, this.imageTransfer, this.imageGroup, this.canFilter);
				Subwindow.Show(ctrlLearnCanFilter, SubwindowButtons.BackAndHome, ctrlLearnCanFilter.Buttons);
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoValidImage"), DialogStyle.Error);
			}
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00068230 File Offset: 0x00066430
		public void SendImage()
		{
			bool flag = this.currentImage != null && this.currentImage.Height > 0 && this.currentImage.Width > 0;
			if (flag)
			{
				bool flag2 = this.machineState.State != MachineStates.XrayOnCalibrationNotDone && this.machineState.State != MachineStates.Reseasoning;
				if (flag2)
				{
					this.imageTransfer.SendImage(this.currentImage);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtWaitingForCalibration"), DialogStyle.Info, DialogButtons.OK);
				}
			}
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x000682C4 File Offset: 0x000664C4
		public void ChangeNPixel(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumEdgePixel"), this.canFilter.NPixel.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNPixel;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00068320 File Offset: 0x00066520
		private void numKeyboard_InputNPixel(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.canFilter.NPixel = int.Parse(e.Text);
				this.SendImage();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNPixel;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00068380 File Offset: 0x00066580
		public void ChangeRadiusX(object obj)
		{
			this.numKeyboard = new NumKeyboard("Radius X", this.canFilter.RadiusX.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputRadiusX;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x000683D8 File Offset: 0x000665D8
		private void numKeyboard_InputRadiusX(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.canFilter.RadiusX = int.Parse(e.Text);
				this.SendImage();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputRadiusX;
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00068438 File Offset: 0x00066638
		public void ChangeRadiusY(object obj)
		{
			this.numKeyboard = new NumKeyboard("Radius Y", this.canFilter.RadiusY.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputRadiusY;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00068490 File Offset: 0x00066690
		private void numKeyboard_InputRadiusY(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.canFilter.RadiusY = int.Parse(e.Text);
				this.SendImage();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputRadiusY;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x000684F0 File Offset: 0x000666F0
		public void ChangeCenterX(object obj)
		{
			this.numKeyboard = new NumKeyboard("Center X", this.canFilter.CenterX.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputCenterX;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00068548 File Offset: 0x00066748
		private void numKeyboard_InputCenterX(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.canFilter.CenterX = int.Parse(e.Text);
				this.SendImage();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputCenterX;
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x000685A8 File Offset: 0x000667A8
		public void ChangeCenterY(object obj)
		{
			this.numKeyboard = new NumKeyboard("Center Y", this.canFilter.CenterY.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputCenterY;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00068600 File Offset: 0x00066800
		private void numKeyboard_InputCenterY(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.canFilter.CenterY = int.Parse(e.Text);
				this.SendImage();
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputCenterY;
		}

		// Token: 0x04000B3E RID: 2878
		private XRSImage currentImage;

		// Token: 0x04000B3F RID: 2879
		private XRSImage originalImage;

		// Token: 0x04000B40 RID: 2880
		private CanFilter canFilter;

		// Token: 0x04000B41 RID: 2881
		private NumKeyboard numKeyboard;

		// Token: 0x04000B42 RID: 2882
		private Timer timer;

		// Token: 0x04000B43 RID: 2883
		private int imageGroup;

		// Token: 0x04000B44 RID: 2884
		private ParameterHandling parHandling;

		// Token: 0x04000B45 RID: 2885
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000B46 RID: 2886
		private MachineState machineState;

		// Token: 0x04000B47 RID: 2887
		private int maxCenterX;

		// Token: 0x04000B48 RID: 2888
		private int maxCenterY;

		// Token: 0x04000B49 RID: 2889
		private int scaleOfY;

		// Token: 0x04000B4A RID: 2890
		private ProductManager productManager;

		// Token: 0x04000B4B RID: 2891
		private StoreImageData imageArchiver;
	}
}

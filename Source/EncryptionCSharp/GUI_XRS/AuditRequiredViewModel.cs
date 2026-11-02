using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200018E RID: 398
	public class AuditRequiredViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x0600257F RID: 9599 RVA: 0x001010CF File Offset: 0x000FF2CF
		// (set) Token: 0x06002580 RID: 9600 RVA: 0x001010D7 File Offset: 0x000FF2D7
		public RelayCommand UnlockBinCommand { get; set; }

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06002581 RID: 9601 RVA: 0x001010E0 File Offset: 0x000FF2E0
		// (set) Token: 0x06002582 RID: 9602 RVA: 0x001010E8 File Offset: 0x000FF2E8
		public ObservableCollection<RejecterItem> UnlockBinItem { get; set; }

		// Token: 0x140000C9 RID: 201
		// (add) Token: 0x06002583 RID: 9603 RVA: 0x001010F4 File Offset: 0x000FF2F4
		// (remove) Token: 0x06002584 RID: 9604 RVA: 0x0010112C File Offset: 0x000FF32C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x140000CA RID: 202
		// (add) Token: 0x06002585 RID: 9605 RVA: 0x00101164 File Offset: 0x000FF364
		// (remove) Token: 0x06002586 RID: 9606 RVA: 0x0010119C File Offset: 0x000FF39C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Close;

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06002587 RID: 9607 RVA: 0x001011D1 File Offset: 0x000FF3D1
		// (set) Token: 0x06002588 RID: 9608 RVA: 0x001011D9 File Offset: 0x000FF3D9
		public AuditRequired AuditRequired { get; set; }

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06002589 RID: 9609 RVA: 0x001011E2 File Offset: 0x000FF3E2
		// (set) Token: 0x0600258A RID: 9610 RVA: 0x001011EA File Offset: 0x000FF3EA
		public AccessRightsManager AccessManager { get; set; }

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x001011F3 File Offset: 0x000FF3F3
		// (set) Token: 0x0600258C RID: 9612 RVA: 0x001011FB File Offset: 0x000FF3FB
		public RelayCommand TestOkCommand { get; set; }

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x0600258D RID: 9613 RVA: 0x00101204 File Offset: 0x000FF404
		// (set) Token: 0x0600258E RID: 9614 RVA: 0x0010120C File Offset: 0x000FF40C
		public RelayCommand TestFailedCommand { get; set; }

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x0600258F RID: 9615 RVA: 0x00101215 File Offset: 0x000FF415
		// (set) Token: 0x06002590 RID: 9616 RVA: 0x0010121D File Offset: 0x000FF41D
		public RelayCommand StartTestCommand { get; set; }

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06002591 RID: 9617 RVA: 0x00101226 File Offset: 0x000FF426
		// (set) Token: 0x06002592 RID: 9618 RVA: 0x0010122E File Offset: 0x000FF42E
		public RelayCommand AbortTestCommand { get; set; }

		// Token: 0x17000AA3 RID: 2723
		// (get) Token: 0x06002593 RID: 9619 RVA: 0x00101237 File Offset: 0x000FF437
		// (set) Token: 0x06002594 RID: 9620 RVA: 0x0010123F File Offset: 0x000FF43F
		public BoundLocalizedText StrRemainingTime { get; set; }

		// Token: 0x17000AA4 RID: 2724
		// (get) Token: 0x06002595 RID: 9621 RVA: 0x00101248 File Offset: 0x000FF448
		// (set) Token: 0x06002596 RID: 9622 RVA: 0x00101250 File Offset: 0x000FF450
		public BoundLocalizedText StrSeconds { get; set; }

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x06002597 RID: 9623 RVA: 0x00101259 File Offset: 0x000FF459
		// (set) Token: 0x06002598 RID: 9624 RVA: 0x00101261 File Offset: 0x000FF461
		public BoundLocalizedText StrDetectedContaminations { get; set; }

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x06002599 RID: 9625 RVA: 0x0010126A File Offset: 0x000FF46A
		// (set) Token: 0x0600259A RID: 9626 RVA: 0x00101272 File Offset: 0x000FF472
		public BoundLocalizedText StrTarget { get; set; }

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x0600259B RID: 9627 RVA: 0x0010127B File Offset: 0x000FF47B
		// (set) Token: 0x0600259C RID: 9628 RVA: 0x00101283 File Offset: 0x000FF483
		public BoundLocalizedText StrUserID { get; set; }

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x0600259D RID: 9629 RVA: 0x0010128C File Offset: 0x000FF48C
		// (set) Token: 0x0600259E RID: 9630 RVA: 0x00101294 File Offset: 0x000FF494
		public BoundLocalizedText StrRemainingProducts { get; set; }

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x0600259F RID: 9631 RVA: 0x0010129D File Offset: 0x000FF49D
		// (set) Token: 0x060025A0 RID: 9632 RVA: 0x001012A5 File Offset: 0x000FF4A5
		public AuditMode Mode { get; set; }

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x060025A1 RID: 9633 RVA: 0x001012B0 File Offset: 0x000FF4B0
		// (set) Token: 0x060025A2 RID: 9634 RVA: 0x001012C8 File Offset: 0x000FF4C8
		public double ImageBoxWidth
		{
			get
			{
				return this.imageBoxWidth;
			}
			set
			{
				this.imageBoxWidth = value;
				this.OnPropertyChanged("ImageBoxWidth");
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x060025A3 RID: 9635 RVA: 0x001012E0 File Offset: 0x000FF4E0
		// (set) Token: 0x060025A4 RID: 9636 RVA: 0x001012F8 File Offset: 0x000FF4F8
		public bool MainEnabled
		{
			get
			{
				return this.mainEnabled;
			}
			set
			{
				this.mainEnabled = value;
				this.OnPropertyChanged("MainEnabled");
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x00101310 File Offset: 0x000FF510
		// (set) Token: 0x060025A6 RID: 9638 RVA: 0x00101328 File Offset: 0x000FF528
		public bool RemainingTimeVisible
		{
			get
			{
				return this.remainingTimeVisible;
			}
			set
			{
				this.remainingTimeVisible = value;
				this.OnPropertyChanged("RemainingTimeVisible");
			}
		}

		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x00101340 File Offset: 0x000FF540
		// (set) Token: 0x060025A8 RID: 9640 RVA: 0x00101358 File Offset: 0x000FF558
		public bool ContNumberVisible
		{
			get
			{
				return this.contNumberVisible;
			}
			set
			{
				this.contNumberVisible = value;
				this.OnPropertyChanged("ContNumberVisible");
			}
		}

		// Token: 0x17000AAE RID: 2734
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x00101370 File Offset: 0x000FF570
		// (set) Token: 0x060025AA RID: 9642 RVA: 0x00101388 File Offset: 0x000FF588
		public bool UserLogAvailable
		{
			get
			{
				return this.userLogAvailable;
			}
			set
			{
				this.userLogAvailable = value;
				this.OnPropertyChanged("UserLogAvailable");
			}
		}

		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x0010139E File Offset: 0x000FF59E
		// (set) Token: 0x060025AC RID: 9644 RVA: 0x001013A6 File Offset: 0x000FF5A6
		public WindowState WState { get; set; }

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x060025AD RID: 9645 RVA: 0x001013B0 File Offset: 0x000FF5B0
		// (set) Token: 0x060025AE RID: 9646 RVA: 0x0003C95F File Offset: 0x0003AB5F
		public double WindowWidth
		{
			get
			{
				return Definition.windowWidth;
			}
			set
			{
				Definition.windowWidth = value;
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x001013C8 File Offset: 0x000FF5C8
		// (set) Token: 0x060025B0 RID: 9648 RVA: 0x0003C97F File Offset: 0x0003AB7F
		public double WindowHeight
		{
			get
			{
				return Definition.windowHeight;
			}
			set
			{
				Definition.windowHeight = value;
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x001013E0 File Offset: 0x000FF5E0
		// (set) Token: 0x060025B2 RID: 9650 RVA: 0x0003C99F File Offset: 0x0003AB9F
		public double WLeft
		{
			get
			{
				return Definition.windowLeft;
			}
			set
			{
				Definition.windowLeft = value;
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x001013F8 File Offset: 0x000FF5F8
		// (set) Token: 0x060025B4 RID: 9652 RVA: 0x0003C9BF File Offset: 0x0003ABBF
		public double WTop
		{
			get
			{
				return Definition.windowTop;
			}
			set
			{
				Definition.windowTop = value;
			}
		}

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x00101410 File Offset: 0x000FF610
		// (set) Token: 0x060025B6 RID: 9654 RVA: 0x00101428 File Offset: 0x000FF628
		public XRSImage CurrentImage
		{
			get
			{
				return this.currentImage;
			}
			set
			{
				bool flag = this.currentImage != value;
				if (flag)
				{
					this.currentImage = value;
					this.OnPropertyChanged("CurrentImage");
				}
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x060025B7 RID: 9655 RVA: 0x0010145C File Offset: 0x000FF65C
		// (set) Token: 0x060025B8 RID: 9656 RVA: 0x00101474 File Offset: 0x000FF674
		public Visibility CommitFailureVisible
		{
			get
			{
				return this.commitFailureVisible;
			}
			set
			{
				bool flag = this.commitFailureVisible != value;
				if (flag)
				{
					this.commitFailureVisible = value;
					this.OnPropertyChanged("CommitFailureVisible");
				}
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x060025B9 RID: 9657 RVA: 0x001014A8 File Offset: 0x000FF6A8
		// (set) Token: 0x060025BA RID: 9658 RVA: 0x001014C0 File Offset: 0x000FF6C0
		public Visibility CommitSuccessVisible
		{
			get
			{
				return this.commitSuccessVisible;
			}
			set
			{
				bool flag = this.commitSuccessVisible != value;
				if (flag)
				{
					this.commitSuccessVisible = value;
					this.OnPropertyChanged("CommitSuccessVisible");
				}
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x060025BB RID: 9659 RVA: 0x001014F4 File Offset: 0x000FF6F4
		// (set) Token: 0x060025BC RID: 9660 RVA: 0x0010150C File Offset: 0x000FF70C
		public Visibility StartTestVisible
		{
			get
			{
				return this.startTestVisible;
			}
			set
			{
				bool flag = this.startTestVisible != value;
				if (flag)
				{
					this.startTestVisible = value;
					this.OnPropertyChanged("StartTestVisible");
				}
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x060025BD RID: 9661 RVA: 0x00101540 File Offset: 0x000FF740
		// (set) Token: 0x060025BE RID: 9662 RVA: 0x00101558 File Offset: 0x000FF758
		public Visibility CancelTestVisible
		{
			get
			{
				return this.cancelTestVisible;
			}
			set
			{
				bool flag = this.cancelTestVisible != value;
				if (flag)
				{
					this.cancelTestVisible = value;
					this.OnPropertyChanged("CancelTestVisible");
				}
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x060025BF RID: 9663 RVA: 0x0010158C File Offset: 0x000FF78C
		public List<XRSImage> AuditImages
		{
			get
			{
				return this.auditImages;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x060025C0 RID: 9664 RVA: 0x001015A4 File Offset: 0x000FF7A4
		// (set) Token: 0x060025C1 RID: 9665 RVA: 0x001015BC File Offset: 0x000FF7BC
		public int NRemainingProducts
		{
			get
			{
				return this.nRemainingProducts;
			}
			set
			{
				bool flag = this.nRemainingProducts != value;
				if (flag)
				{
					this.nRemainingProducts = value;
					this.OnPropertyChanged("NRemainingProducts");
				}
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x060025C2 RID: 9666 RVA: 0x001015F0 File Offset: 0x000FF7F0
		// (set) Token: 0x060025C3 RID: 9667 RVA: 0x00101608 File Offset: 0x000FF808
		public GridLength NumbersColumnWidth
		{
			get
			{
				return this.numbersColumnWidth;
			}
			set
			{
				bool flag = this.numbersColumnWidth != value;
				if (flag)
				{
					this.numbersColumnWidth = value;
					this.OnPropertyChanged("NumbersColumnWidth");
				}
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x060025C4 RID: 9668 RVA: 0x0010163C File Offset: 0x000FF83C
		// (set) Token: 0x060025C5 RID: 9669 RVA: 0x00101654 File Offset: 0x000FF854
		public bool DoorLockAvailable
		{
			get
			{
				return this.doorLockAvailable;
			}
			set
			{
				this.doorLockAvailable = value;
				this.OnPropertyChanged("DoorLockAvailable");
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x060025C6 RID: 9670 RVA: 0x0010166C File Offset: 0x000FF86C
		// (set) Token: 0x060025C7 RID: 9671 RVA: 0x00101684 File Offset: 0x000FF884
		public bool ContextUnlockBinVisibility
		{
			get
			{
				return this.contextUnlockBinVisibility;
			}
			set
			{
				bool flag = this.contextUnlockBinVisibility != value;
				if (flag)
				{
					this.contextUnlockBinVisibility = value;
					this.OnPropertyChanged("ContextUnlockBinVisibility");
				}
			}
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x001016B8 File Offset: 0x000FF8B8
		private void UnlockBin(object obj)
		{
			int binTimeout = 0;
			this.AuditRequired.DbConnection.GetMSBinTimeout(true, ref binTimeout);
			string a = this.parHandling.ReadPar(Rejecter.rejecter2MainPath + "/Enabled/Value");
			bool flag = a == "0";
			if (flag)
			{
				Rejecter.UnlockBin(Rejecter.rejecter1MainPath, this.parHandling, binTimeout);
				this.ContextUnlockBinVisibility = false;
			}
			else
			{
				this.ContextUnlockBinVisibility = true;
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x060025C9 RID: 9673 RVA: 0x00101732 File Offset: 0x000FF932
		// (set) Token: 0x060025CA RID: 9674 RVA: 0x0010173A File Offset: 0x000FF93A
		public Restore Restore { get; private set; }

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x060025CB RID: 9675 RVA: 0x00101743 File Offset: 0x000FF943
		// (set) Token: 0x060025CC RID: 9676 RVA: 0x0010174B File Offset: 0x000FF94B
		public MachineType MachineType { get; set; }

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x060025CD RID: 9677 RVA: 0x00101754 File Offset: 0x000FF954
		// (set) Token: 0x060025CE RID: 9678 RVA: 0x0010176C File Offset: 0x000FF96C
		public bool IsPreAuditMenu
		{
			get
			{
				return this.isPreAuditMenu;
			}
			set
			{
				bool flag = this.isPreAuditMenu != value;
				if (flag)
				{
					this.isPreAuditMenu = value;
					this.OnPropertyChanged("IsPreAuditMenu");
				}
			}
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x001017A0 File Offset: 0x000FF9A0
		public AuditRequiredViewModel(AuditRequired auditRequired, AccessRightsManager accessManager, MachineState machineState, AuditMode mode, ImageDataTransfer imageTransfer, ParameterHandling parHandling, Restore restore)
		{
			this.AuditRequired = auditRequired;
			this.AccessManager = accessManager;
			this.imageTransfer = imageTransfer;
			this.parHandling = parHandling;
			this.Restore = restore;
			this.MainEnabled = true;
			this.ContNumberVisible = false;
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			this.timerClose = new Timer(2000.0);
			this.timerClose.Elapsed += this.timerClose_Elapsed;
			this.RegisterLocalizedText();
			this.StartTestCommand = new RelayCommand(new Action<object>(this.StartTest));
			this.AbortTestCommand = new RelayCommand(new Action<object>(this.AbortTest));
			this.TestOkCommand = new RelayCommand(new Action<object>(this.TestOk));
			this.TestFailedCommand = new RelayCommand(new Action<object>(this.TestFailed));
			this.AuditRequired.TimeToStartAuditElapsed += this.AuditRequired_TimeToStartAuditElapsed;
			this.machineState = machineState;
			this.oldState = MachineStates.Inspecting;
			machineState.State = MachineStates.PreAuditCheck;
			this.Mode = mode;
			bool flag = this.AuditRequired.TimeToStartAudit > 0;
			if (flag)
			{
				this.RemainingTimeVisible = true;
			}
			else
			{
				this.RemainingTimeVisible = false;
			}
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			this.AuditRequired.DbConnection.UpdateCurrentProductTable("LastAuditCheckDay", DateTime.Now.DayOfWeek.ToString());
			LogbookManager.AddEntry(SSTEntryCode.AuditNeeded);
			this.startTestVisible = Visibility.Visible;
			this.cancelTestVisible = Visibility.Collapsed;
			this.commitSuccessVisible = Visibility.Collapsed;
			this.commitFailureVisible = Visibility.Collapsed;
			this.nRemainingProducts = auditRequired.NProducts;
			this.numbersColumnWidth = new GridLength(1.0, GridUnitType.Star);
			this.AuditRequired.DbConnection.GetDoorLockAvailable(ref this.doorLockAvailable);
			this.UnlockBinCommand = new RelayCommand(new Action<object>(this.UnlockBin));
			this.rejecter = new Rejecter(parHandling, this.AuditRequired.DbConnection);
			this.UnlockBinItem = this.rejecter.UnlockBinItems;
			this.userLogAvailable = false;
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.AuditRequired.DbConnection.GetMachineType(ref machineType);
			this.MachineType = machineType;
			this.IsPreAuditMenu = true;
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00101A34 File Offset: 0x000FFC34
		private void RegisterLocalizedText()
		{
			this.StrRemainingTime = new BoundLocalizedText("txtRemainingTime");
			this.StrSeconds = new BoundLocalizedText("txtSeconds");
			this.StrDetectedContaminations = new BoundLocalizedText("txtDetectedContaminations");
			this.StrTarget = new BoundLocalizedText("txtTarget");
			this.StrUserID = new BoundLocalizedText("txtUserID");
			this.StrRemainingProducts = new BoundLocalizedText("txtRemainingProducts");
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00101AA8 File Offset: 0x000FFCA8
		public void CleanUp()
		{
			MainWindow.localizationManager.Deregister(this.StrRemainingTime);
			MainWindow.localizationManager.Deregister(this.StrSeconds);
			MainWindow.localizationManager.Deregister(this.StrDetectedContaminations);
			MainWindow.localizationManager.Deregister(this.StrTarget);
			MainWindow.localizationManager.Deregister(this.StrUserID);
			MainWindow.localizationManager.Deregister(this.StrRemainingProducts);
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
			this.parHandling.WriteValue(Definition.inAuditModePath, "0", false);
			this.AccessManager = null;
			this.currentImage = null;
			this.auditImages.Clear();
			this.auditImages = null;
			this.timerClose.Elapsed -= this.timerClose_Elapsed;
			this.timerClose.Dispose();
			this.rejecter.Cleanup();
			this.rejecter = null;
			this.UnlockBinItem.Clear();
			this.UnlockBinItem = null;
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00101BD0 File Offset: 0x000FFDD0
		private void AuditRequired_TimeToStartAuditElapsed()
		{
			bool flag = this.MainEnabled;
			if (flag)
			{
				this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
				this.AuditRequired.StrAuditNeeded.ResourceKey = "txtTestFailed";
				bool auditFailedError = this.AuditRequired.AuditFailedError;
				if (auditFailedError)
				{
					this.AuditRequired.LastAuditFailed = true;
				}
				else
				{
					LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
				}
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Timeout);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(false);
				this.timerClose.Start();
			}
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00101C7C File Offset: 0x000FFE7C
		private void timerClose_Elapsed(object sender, ElapsedEventArgs e)
		{
			bool flag = this.machineState.State == MachineStates.AuditCheck || this.machineState.State == MachineStates.PreAuditCheck;
			if (flag)
			{
				this.machineState.State = this.oldState;
			}
			this.timerClose.Stop();
			this.timerClose.Elapsed -= this.timerClose_Elapsed;
			this.AuditRequired.TimeToStartAuditElapsed -= this.AuditRequired_TimeToStartAuditElapsed;
			this.OnClose();
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00101D04 File Offset: 0x000FFF04
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00101D34 File Offset: 0x000FFF34
		protected virtual void OnClose()
		{
			Action close = this.Close;
			bool flag = close != null;
			if (flag)
			{
				close();
			}
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00101D5C File Offset: 0x000FFF5C
		private void StartTest(object obj)
		{
			bool flag = this.machineState.State == MachineStates.Inspecting || this.machineState.State == MachineStates.PreAuditCheck;
			if (flag)
			{
				bool flag2 = this.AccessManager.CurrentUser.ID != 0;
				if (flag2)
				{
					this.IsPreAuditMenu = false;
					this.AccessManager.StopTimerAutoLogout();
					bool flag3 = this.AuditRequired.StrAuditNeeded.ResourceKey == "txtAuditNeeded";
					if (flag3)
					{
						switch (this.Mode)
						{
						case AuditMode.Contamination:
							this.AuditRequired.StrAuditNeeded.ResourceKey = "txtConveyContaminantProduct";
							this.AuditRequired.InTestContaminationMode = true;
							break;
						case AuditMode.Weight:
							this.AuditRequired.StrAuditNeeded.ResourceKey = "txtConveyWeightProduct";
							this.AuditRequired.InTestWeightMode = true;
							break;
						case AuditMode.Count:
							this.AuditRequired.StrAuditNeeded.ResourceKey = "txtConveyCountProduct";
							this.AuditRequired.InTestCountMode = true;
							break;
						}
						this.machineState.State = MachineStates.AuditCheck;
						LogbookManager.AddEntry(SSTEntryCode.AuditStart);
						this.StartTestVisible = Visibility.Collapsed;
						this.CancelTestVisible = Visibility.Visible;
						this.CommitSuccessVisible = Visibility.Collapsed;
						this.CommitFailureVisible = Visibility.Collapsed;
						this.ImageBoxWidth = 350.0;
						this.parHandling.WriteValue(Definition.inAuditModePath, "1", false);
						this.UserLogAvailable = true;
					}
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgLogin"), DialogStyle.Warning, DialogButtons.OK);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
			}
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00101F28 File Offset: 0x00100128
		private void AbortTest(object obj)
		{
			this.AccessManager.StartTimerAutoLogout();
			bool flag = this.ImageBoxWidth != 0.0;
			if (flag)
			{
				this.TestFailed(null);
			}
			else
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgAbort"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					bool auditFailedError = this.AuditRequired.AuditFailedError;
					if (auditFailedError)
					{
						this.AuditRequired.LastAuditFailed = true;
					}
					else
					{
						LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
					}
					this.AuditRequired.StrAuditNeeded.ResourceKey = "txtTestFailed";
					this.timerClose.Start();
					this.MainEnabled = false;
					this.StartTestVisible = Visibility.Collapsed;
					this.CancelTestVisible = Visibility.Collapsed;
					this.CommitSuccessVisible = Visibility.Collapsed;
					this.CommitFailureVisible = Visibility.Collapsed;
					this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.NotPerformed);
					this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
					this.IncreaseNAuditChecks(false);
				}
			}
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00102030 File Offset: 0x00100230
		private void TestOk(object obj)
		{
			this.AccessManager.StartTimerAutoLogout();
			bool flag = this.auditImages.Count == this.AuditRequired.NProducts && this.currentAuditImageIndex == this.AuditRequired.NProducts - 1;
			if (flag)
			{
				LogbookManager.AddEntry(SSTEntryCode.AuditOK);
				this.AuditRequired.StrAuditNeeded.ResourceKey = "txtTestOk";
				this.timerClose.Start();
				this.MainEnabled = false;
				this.StartTestVisible = Visibility.Collapsed;
				this.CancelTestVisible = Visibility.Collapsed;
				this.CommitSuccessVisible = Visibility.Collapsed;
				this.CommitFailureVisible = Visibility.Collapsed;
				this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Passed);
				this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
				this.IncreaseNAuditChecks(true);
			}
			else
			{
				this.currentAuditImageIndex++;
				bool flag2 = this.auditImages.Count > this.currentAuditImageIndex && this.auditImages[this.currentAuditImageIndex] != null;
				if (flag2)
				{
					this.CurrentImage = this.auditImages[this.currentAuditImageIndex];
					bool flag3 = this.ImageConformsToTest(this.CurrentImage);
					if (flag3)
					{
						this.CancelTestVisible = Visibility.Collapsed;
						this.CommitSuccessVisible = Visibility.Visible;
						this.CommitFailureVisible = Visibility.Visible;
					}
					else
					{
						this.CancelTestVisible = Visibility.Visible;
						this.CommitSuccessVisible = Visibility.Collapsed;
						this.CommitFailureVisible = Visibility.Collapsed;
					}
				}
				else
				{
					this.NumbersColumnWidth = new GridLength(1.0, GridUnitType.Star);
					this.CurrentImage = null;
					this.StartTestVisible = Visibility.Collapsed;
					this.CancelTestVisible = Visibility.Visible;
					this.CommitSuccessVisible = Visibility.Collapsed;
					this.CommitFailureVisible = Visibility.Collapsed;
					this.ContNumberVisible = false;
				}
			}
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x001021F4 File Offset: 0x001003F4
		private void TestFailed(object obj)
		{
			this.AccessManager.StartTimerAutoLogout();
			bool auditFailedError = this.AuditRequired.AuditFailedError;
			if (auditFailedError)
			{
				this.AuditRequired.LastAuditFailed = true;
			}
			else
			{
				LogbookManager.AddEntry(SSTEntryCode.AuditFailedInfo);
			}
			this.AuditRequired.StrAuditNeeded.ResourceKey = "txtTestFailed";
			this.timerClose.Start();
			this.MainEnabled = false;
			this.StartTestVisible = Visibility.Collapsed;
			this.CancelTestVisible = Visibility.Collapsed;
			this.CommitSuccessVisible = Visibility.Collapsed;
			this.CommitFailureVisible = Visibility.Collapsed;
			this.AuditRequired.DbConnection.SetLastAuditCheckResult(AuditResult.Failed);
			this.AuditRequired.DbConnection.SetAuditCheckErrorReset(false);
			this.IncreaseNAuditChecks(false);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x001022B0 File Offset: 0x001004B0
		public bool ShowImage(XRSImage image)
		{
			bool result = false;
			bool flag = image.Type == ImageType.Audit;
			if (flag)
			{
				result = true;
				bool inTestContaminationMode = this.AuditRequired.InTestContaminationMode;
				if (inTestContaminationMode)
				{
					this.ContNumberVisible = true;
				}
			}
			return result;
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x001022F0 File Offset: 0x001004F0
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = this.ShowImage(e.Image);
				if (flag2)
				{
					this.NumbersColumnWidth = GridLength.Auto;
					bool flag3 = this.CurrentImage == null;
					if (flag3)
					{
						this.CurrentImage = e.Image;
						this.ImageBoxWidth = 350.0;
						this.StartTestVisible = Visibility.Collapsed;
						bool flag4 = this.ImageConformsToTest(e.Image);
						if (flag4)
						{
							this.CancelTestVisible = Visibility.Collapsed;
							this.CommitSuccessVisible = Visibility.Visible;
							this.CommitFailureVisible = Visibility.Visible;
						}
						else
						{
							this.CancelTestVisible = Visibility.Visible;
							this.CommitSuccessVisible = Visibility.Collapsed;
							this.CommitFailureVisible = Visibility.Collapsed;
							this.AuditRequired.StrAuditNeeded.ResourceKey = "txtTestFailed";
						}
					}
					bool flag5 = this.auditImages.Count < this.AuditRequired.NProducts;
					if (flag5)
					{
						this.auditImages.Add(e.Image);
						int nremainingProducts = this.NRemainingProducts;
						this.NRemainingProducts = nremainingProducts - 1;
						bool flag6 = this.nRemainingProducts == 0;
						if (flag6)
						{
							this.AuditRequired.StrAuditNeeded.ResourceKey = "";
							this.parHandling.WriteValue(Definition.inAuditModePath, "0", false);
						}
					}
				}
			}
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00102470 File Offset: 0x00100670
		private void IncreaseNAuditChecks(bool passed)
		{
			int num = 0;
			if (passed)
			{
				bool nauditChecksPassed = this.AuditRequired.DbConnection.GetNAuditChecksPassed(ref num);
				if (nauditChecksPassed)
				{
					this.AuditRequired.DbConnection.SetNAuditChecksPassed(++num);
				}
			}
			else
			{
				bool nauditChecksFailed = this.AuditRequired.DbConnection.GetNAuditChecksFailed(ref num);
				if (nauditChecksFailed)
				{
					this.AuditRequired.DbConnection.SetNAuditChecksFailed(++num);
				}
			}
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x001024E4 File Offset: 0x001006E4
		private bool ImageConformsToTest(XRSImage image)
		{
			return (this.Mode == AuditMode.Contamination && (int)image.ContaminationCount >= this.AuditRequired.NContaminations) || (this.Mode == AuditMode.Count && image.HasFalseCount()) || (this.Mode == AuditMode.Weight && image.HasFalseWeight());
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00102537 File Offset: 0x00100737
		public void CheckUserRightforUnlockBin(Button btnUnlockBin)
		{
			this.AccessManager.RegisterControl(btnUnlockBin, Rejecter.doVerifyTestRight, Rejecter.doVerifyTestRight);
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00102551 File Offset: 0x00100751
		public void DeregisterUnlockBinButton(Button btnUnlockBin)
		{
			this.AccessManager.DeregisterControl(btnUnlockBin);
		}

		// Token: 0x04001811 RID: 6161
		private const int imageBoxVisibleWidth = 350;

		// Token: 0x04001812 RID: 6162
		private Timer timerClose;

		// Token: 0x04001813 RID: 6163
		private MachineState machineState;

		// Token: 0x04001814 RID: 6164
		private MachineStates oldState;

		// Token: 0x04001815 RID: 6165
		private double imageBoxWidth;

		// Token: 0x04001816 RID: 6166
		private bool mainEnabled;

		// Token: 0x04001817 RID: 6167
		private bool remainingTimeVisible;

		// Token: 0x04001818 RID: 6168
		private bool contNumberVisible;

		// Token: 0x04001819 RID: 6169
		private List<XRSImage> auditImages = new List<XRSImage>();

		// Token: 0x0400181A RID: 6170
		private ImageDataTransfer imageTransfer;

		// Token: 0x0400181B RID: 6171
		private XRSImage currentImage;

		// Token: 0x0400181C RID: 6172
		private int currentAuditImageIndex = 0;

		// Token: 0x0400181D RID: 6173
		private Visibility commitFailureVisible;

		// Token: 0x0400181E RID: 6174
		private Visibility commitSuccessVisible;

		// Token: 0x0400181F RID: 6175
		private Visibility startTestVisible;

		// Token: 0x04001820 RID: 6176
		private Visibility cancelTestVisible;

		// Token: 0x04001821 RID: 6177
		private int nRemainingProducts;

		// Token: 0x04001822 RID: 6178
		private GridLength numbersColumnWidth;

		// Token: 0x04001823 RID: 6179
		private ParameterHandling parHandling;

		// Token: 0x04001824 RID: 6180
		private bool doorLockAvailable;

		// Token: 0x04001827 RID: 6183
		private Rejecter rejecter;

		// Token: 0x04001837 RID: 6199
		private bool userLogAvailable;

		// Token: 0x04001839 RID: 6201
		private bool contextUnlockBinVisibility = false;

		// Token: 0x0400183C RID: 6204
		private bool isPreAuditMenu;
	}
}

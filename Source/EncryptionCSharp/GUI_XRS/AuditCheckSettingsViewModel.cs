using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000191 RID: 401
	public class AuditCheckSettingsViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000CC RID: 204
		// (add) Token: 0x06002605 RID: 9733 RVA: 0x0010374C File Offset: 0x0010194C
		// (remove) Token: 0x06002606 RID: 9734 RVA: 0x00103784 File Offset: 0x00101984
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06002607 RID: 9735 RVA: 0x001037B9 File Offset: 0x001019B9
		// (set) Token: 0x06002608 RID: 9736 RVA: 0x001037C1 File Offset: 0x001019C1
		public AuditCheckSettings AuditSettings { get; set; }

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06002609 RID: 9737 RVA: 0x001037CC File Offset: 0x001019CC
		// (set) Token: 0x0600260A RID: 9738 RVA: 0x001037E4 File Offset: 0x001019E4
		public bool AuditCheckForContaminationsAccessible
		{
			get
			{
				return this.auditCheckForContaminationsAccessible;
			}
			set
			{
				this.auditCheckForContaminationsAccessible = value;
				this.OnPropertyChanged("AuditCheckForContaminationsAccessible");
				this.OnPropertyChanged("AuditCheckForContaminationsNotAccessible");
			}
		}

		// Token: 0x17000AD2 RID: 2770
		// (get) Token: 0x0600260B RID: 9739 RVA: 0x00103808 File Offset: 0x00101A08
		public bool AuditCheckForContaminationsNotAccessible
		{
			get
			{
				return !this.auditCheckForContaminationsAccessible;
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x0600260C RID: 9740 RVA: 0x00103823 File Offset: 0x00101A23
		// (set) Token: 0x0600260D RID: 9741 RVA: 0x0010382B File Offset: 0x00101A2B
		public RelayCommand ChangeStartTimeHourCommand { get; set; }

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x0600260E RID: 9742 RVA: 0x00103834 File Offset: 0x00101A34
		// (set) Token: 0x0600260F RID: 9743 RVA: 0x0010383C File Offset: 0x00101A3C
		public RelayCommand ChangeStartTimeMinuteCommand { get; set; }

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x06002610 RID: 9744 RVA: 0x00103845 File Offset: 0x00101A45
		// (set) Token: 0x06002611 RID: 9745 RVA: 0x0010384D File Offset: 0x00101A4D
		public RelayCommand ChangeIntervalTimeHourCommand { get; set; }

		// Token: 0x17000AD6 RID: 2774
		// (get) Token: 0x06002612 RID: 9746 RVA: 0x00103856 File Offset: 0x00101A56
		// (set) Token: 0x06002613 RID: 9747 RVA: 0x0010385E File Offset: 0x00101A5E
		public RelayCommand ChangeIntervalTimeMinuteCommand { get; set; }

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06002614 RID: 9748 RVA: 0x00103867 File Offset: 0x00101A67
		// (set) Token: 0x06002615 RID: 9749 RVA: 0x0010386F File Offset: 0x00101A6F
		public RelayCommand ChangeTimeToStartAuditCommand { get; set; }

		// Token: 0x17000AD8 RID: 2776
		// (get) Token: 0x06002616 RID: 9750 RVA: 0x00103878 File Offset: 0x00101A78
		// (set) Token: 0x06002617 RID: 9751 RVA: 0x00103880 File Offset: 0x00101A80
		public RelayCommand ChangeNContaminationsCommand { get; set; }

		// Token: 0x17000AD9 RID: 2777
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x00103889 File Offset: 0x00101A89
		// (set) Token: 0x06002619 RID: 9753 RVA: 0x00103891 File Offset: 0x00101A91
		public RelayCommand ChangeNProductsCommand { get; set; }

		// Token: 0x17000ADA RID: 2778
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x0010389A File Offset: 0x00101A9A
		// (set) Token: 0x0600261B RID: 9755 RVA: 0x001038A2 File Offset: 0x00101AA2
		public RelayCommand TriggerAuditCommand { get; set; }

		// Token: 0x17000ADB RID: 2779
		// (get) Token: 0x0600261C RID: 9756 RVA: 0x001038AB File Offset: 0x00101AAB
		// (set) Token: 0x0600261D RID: 9757 RVA: 0x001038B3 File Offset: 0x00101AB3
		public RelayCommand DeregisterLocalizedTextCommand { get; set; }

		// Token: 0x17000ADC RID: 2780
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x001038BC File Offset: 0x00101ABC
		// (set) Token: 0x0600261F RID: 9759 RVA: 0x001038C4 File Offset: 0x00101AC4
		public BoundLocalizedText StrMode { get; set; }

		// Token: 0x17000ADD RID: 2781
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x001038CD File Offset: 0x00101ACD
		// (set) Token: 0x06002621 RID: 9761 RVA: 0x001038D5 File Offset: 0x00101AD5
		public BoundLocalizedText StrDay { get; set; }

		// Token: 0x17000ADE RID: 2782
		// (get) Token: 0x06002622 RID: 9762 RVA: 0x001038DE File Offset: 0x00101ADE
		// (set) Token: 0x06002623 RID: 9763 RVA: 0x001038E6 File Offset: 0x00101AE6
		public BoundLocalizedText StrTime { get; set; }

		// Token: 0x17000ADF RID: 2783
		// (get) Token: 0x06002624 RID: 9764 RVA: 0x001038EF File Offset: 0x00101AEF
		// (set) Token: 0x06002625 RID: 9765 RVA: 0x001038F7 File Offset: 0x00101AF7
		public BoundLocalizedText StrInterval { get; set; }

		// Token: 0x17000AE0 RID: 2784
		// (get) Token: 0x06002626 RID: 9766 RVA: 0x00103900 File Offset: 0x00101B00
		// (set) Token: 0x06002627 RID: 9767 RVA: 0x00103908 File Offset: 0x00101B08
		public BoundLocalizedText StrAuditFailedError { get; set; }

		// Token: 0x17000AE1 RID: 2785
		// (get) Token: 0x06002628 RID: 9768 RVA: 0x00103911 File Offset: 0x00101B11
		// (set) Token: 0x06002629 RID: 9769 RVA: 0x00103919 File Offset: 0x00101B19
		public BoundLocalizedText StrTimeToStartAudit { get; set; }

		// Token: 0x17000AE2 RID: 2786
		// (get) Token: 0x0600262A RID: 9770 RVA: 0x00103922 File Offset: 0x00101B22
		// (set) Token: 0x0600262B RID: 9771 RVA: 0x0010392A File Offset: 0x00101B2A
		public BoundLocalizedText StrRejectAll { get; set; }

		// Token: 0x17000AE3 RID: 2787
		// (get) Token: 0x0600262C RID: 9772 RVA: 0x00103933 File Offset: 0x00101B33
		// (set) Token: 0x0600262D RID: 9773 RVA: 0x0010393B File Offset: 0x00101B3B
		public BoundLocalizedText StrAuditContaminationActive { get; set; }

		// Token: 0x17000AE4 RID: 2788
		// (get) Token: 0x0600262E RID: 9774 RVA: 0x00103944 File Offset: 0x00101B44
		// (set) Token: 0x0600262F RID: 9775 RVA: 0x0010394C File Offset: 0x00101B4C
		public BoundLocalizedText StrAuditWeightActive { get; set; }

		// Token: 0x17000AE5 RID: 2789
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x00103955 File Offset: 0x00101B55
		// (set) Token: 0x06002631 RID: 9777 RVA: 0x0010395D File Offset: 0x00101B5D
		public BoundLocalizedText StrAuditCountActive { get; set; }

		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x00103966 File Offset: 0x00101B66
		// (set) Token: 0x06002633 RID: 9779 RVA: 0x0010396E File Offset: 0x00101B6E
		public BoundLocalizedText StrNumberOfContaminations { get; set; }

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x00103977 File Offset: 0x00101B77
		// (set) Token: 0x06002635 RID: 9781 RVA: 0x0010397F File Offset: 0x00101B7F
		public BoundLocalizedText StrTimeToNextAudit { get; set; }

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x00103988 File Offset: 0x00101B88
		// (set) Token: 0x06002637 RID: 9783 RVA: 0x00103990 File Offset: 0x00101B90
		public BoundLocalizedText StrStartAudit { get; set; }

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x00103999 File Offset: 0x00101B99
		// (set) Token: 0x06002639 RID: 9785 RVA: 0x001039A1 File Offset: 0x00101BA1
		public BoundLocalizedText StrNumberOfProducts { get; set; }

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x001039AA File Offset: 0x00101BAA
		// (set) Token: 0x0600263B RID: 9787 RVA: 0x001039B2 File Offset: 0x00101BB2
		public BoundLocalizedText StrTimeOfDay { get; set; }

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x0600263C RID: 9788 RVA: 0x001039BB File Offset: 0x00101BBB
		// (set) Token: 0x0600263D RID: 9789 RVA: 0x001039C3 File Offset: 0x00101BC3
		public BoundLocalizedText StrProductSpecific { get; set; }

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x0600263E RID: 9790 RVA: 0x001039CC File Offset: 0x00101BCC
		// (set) Token: 0x0600263F RID: 9791 RVA: 0x001039D4 File Offset: 0x00101BD4
		public BoundLocalizedText StrCommonSettings { get; set; }

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06002640 RID: 9792 RVA: 0x001039DD File Offset: 0x00101BDD
		// (set) Token: 0x06002641 RID: 9793 RVA: 0x001039E5 File Offset: 0x00101BE5
		public BoundLocalizedText StrFastMode { get; set; }

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06002642 RID: 9794 RVA: 0x001039F0 File Offset: 0x00101BF0
		// (set) Token: 0x06002643 RID: 9795 RVA: 0x00103A08 File Offset: 0x00101C08
		public bool DocumentationEnabled
		{
			get
			{
				return this.documentationEnabled;
			}
			set
			{
				this.documentationEnabled = value;
				this.OnPropertyChanged("DocumentationEnabled");
				this.dBConnection.SetAuditCheckDocumentation(this.documentationEnabled);
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06002644 RID: 9796 RVA: 0x00103A30 File Offset: 0x00101C30
		// (set) Token: 0x06002645 RID: 9797 RVA: 0x00103A48 File Offset: 0x00101C48
		public bool RejectAll
		{
			get
			{
				return this.rejectAll;
			}
			set
			{
				bool flag = value && !this.first;
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
							this.dBConnection.SetRejectAllAuditCheck(this.rejectAll);
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
					this.dBConnection.SetRejectAllAuditCheck(this.rejectAll);
				}
			}
		}

		// Token: 0x17000AF0 RID: 2800
		// (get) Token: 0x06002646 RID: 9798 RVA: 0x00103B68 File Offset: 0x00101D68
		// (set) Token: 0x06002647 RID: 9799 RVA: 0x00103B80 File Offset: 0x00101D80
		public bool AuditCheckProductChange
		{
			get
			{
				return this.auditCheckProductChange;
			}
			set
			{
				this.auditCheckProductChange = value;
				this.OnPropertyChanged("AuditCheckProductChange");
				this.dBConnection.SetAuditCheckProductChange(this.auditCheckProductChange);
			}
		}

		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06002648 RID: 9800 RVA: 0x00103BA8 File Offset: 0x00101DA8
		// (set) Token: 0x06002649 RID: 9801 RVA: 0x00103BC0 File Offset: 0x00101DC0
		public bool IsCount
		{
			get
			{
				return this.isCount;
			}
			set
			{
				this.isCount = value;
				this.OnPropertyChanged("IsCount");
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x0600264A RID: 9802 RVA: 0x00103BD8 File Offset: 0x00101DD8
		// (set) Token: 0x0600264B RID: 9803 RVA: 0x00103BF0 File Offset: 0x00101DF0
		public bool IsWeight
		{
			get
			{
				return this.isWeight;
			}
			set
			{
				this.isWeight = value;
				this.OnPropertyChanged("IsWeight");
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x0600264C RID: 9804 RVA: 0x00103C08 File Offset: 0x00101E08
		// (set) Token: 0x0600264D RID: 9805 RVA: 0x00103C20 File Offset: 0x00101E20
		public bool IsNotBulk
		{
			get
			{
				return this.isNotBulk;
			}
			set
			{
				this.isNotBulk = value;
				this.OnPropertyChanged("IsNotBulk");
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x0600264E RID: 9806 RVA: 0x00103C36 File Offset: 0x00101E36
		// (set) Token: 0x0600264F RID: 9807 RVA: 0x00103C3E File Offset: 0x00101E3E
		public BoundLocalizedText StrDocumentation { get; set; }

		// Token: 0x17000AF5 RID: 2805
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x00103C47 File Offset: 0x00101E47
		// (set) Token: 0x06002651 RID: 9809 RVA: 0x00103C4F File Offset: 0x00101E4F
		public BoundLocalizedText StrAuditCheckProductChange { get; set; }

		// Token: 0x06002652 RID: 9810 RVA: 0x00103C58 File Offset: 0x00101E58
		public AuditCheckSettingsViewModel(AuditCheckSettings auditSettings, DBConnection dbConnection, AccessRightsManager accessManager, ProductManager productManager, ParameterHandling parHandling)
		{
			this.accessManager = accessManager;
			this.dBConnection = dbConnection;
			this.parHandling = parHandling;
			this.AuditSettings = auditSettings;
			this.RegisterLocalizedText();
			this.ChangeStartTimeHourCommand = new RelayCommand(new Action<object>(this.ChangeStartTimeHour));
			this.ChangeStartTimeMinuteCommand = new RelayCommand(new Action<object>(this.ChangeStartTimeMinute));
			this.ChangeIntervalTimeHourCommand = new RelayCommand(new Action<object>(this.ChangeIntervalTimeHour));
			this.ChangeIntervalTimeMinuteCommand = new RelayCommand(new Action<object>(this.ChangeIntervalTimeMinute));
			this.ChangeTimeToStartAuditCommand = new RelayCommand(new Action<object>(this.ChangeTimeToStartAudit));
			this.ChangeNContaminationsCommand = new RelayCommand(new Action<object>(this.ChangeNContaminations));
			this.ChangeNProductsCommand = new RelayCommand(new Action<object>(this.ChangeNProducts));
			this.TriggerAuditCommand = new RelayCommand(new Action<object>(this.TriggerAudit));
			this.DeregisterLocalizedTextCommand = new RelayCommand(new Action<object>(this.DeregisterLocalizedText));
			dbConnection.GetUKUserRightsHL(ref this.ukUserRightsHL);
			this.SetAuditCheckForContaminationsAccessible();
			this.AuditSettings.PropertyChanged += this.auditSettings_PropertyChanged;
			bool flag = false;
			dbConnection.GetAuditCheckDocumentation(ref flag);
			this.documentationEnabled = flag;
			dbConnection.GetRejectAllAuditCheck(ref flag);
			this.RejectAll = flag;
			dbConnection.GetAuditCheckProductChange(ref flag);
			this.AuditCheckProductChange = flag;
			MachineMode machineMode = MachineMode.Standard;
			dbConnection.GetMachineMode(ref machineMode);
			bool flag2 = machineMode != MachineMode.Bulk;
			if (flag2)
			{
				this.IsNotBulk = true;
			}
			else
			{
				this.IsNotBulk = false;
			}
			bool flag3 = productManager != null && this.IsNotBulk;
			if (flag3)
			{
				string a = "";
				string str = (productManager.CurrentLoadedProduct.Group > 0) ? productManager.CurrentLoadedProduct.Group.ToString() : "";
				bool flag4 = dbConnection.GetParameterValue(Definition.filterMainPath + str + Weigher.enableWeightSubPath, ref a) && a == "1";
				if (flag4)
				{
					this.IsWeight = true;
				}
				else
				{
					this.IsWeight = false;
				}
				bool flag5 = dbConnection.GetParameterValue(Definition.filterMainPath + str + ObjectCounter.enabledSubPath, ref a) && a == "1";
				if (flag5)
				{
					this.IsCount = true;
				}
				else
				{
					this.IsCount = false;
				}
			}
			this.first = false;
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x00103F00 File Offset: 0x00102100
		private void auditSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "ContaminationEnabled";
			if (flag)
			{
				this.SetAuditCheckForContaminationsAccessible();
			}
		}

		// Token: 0x06002654 RID: 9812 RVA: 0x00103F2B File Offset: 0x0010212B
		private void SetAuditCheckForContaminationsAccessible()
		{
			this.AuditCheckForContaminationsAccessible = (!this.ukUserRightsHL || this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin);
		}

		// Token: 0x06002655 RID: 9813 RVA: 0x00103F5C File Offset: 0x0010215C
		private void RegisterLocalizedText()
		{
			this.StrMode = new BoundLocalizedText("txtAuditMode");
			this.StrDay = new BoundLocalizedText("txtAuditDay");
			this.StrTime = new BoundLocalizedText("txtAuditTime");
			this.StrInterval = new BoundLocalizedText("txtAuditInterval");
			this.StrAuditFailedError = new BoundLocalizedText("txtAuditFailedError");
			this.StrTimeToStartAudit = new BoundLocalizedText("txtTimeToStartAudit");
			this.StrAuditContaminationActive = new BoundLocalizedText("txtAuditContaminationActive");
			this.StrAuditWeightActive = new BoundLocalizedText("txtAuditWeightActive");
			this.StrAuditCountActive = new BoundLocalizedText("txtAuditCountActive");
			this.StrNumberOfContaminations = new BoundLocalizedText("txtNumContaminations");
			this.StrTimeToNextAudit = new BoundLocalizedText("txtTimeToNextAudit");
			this.StrStartAudit = new BoundLocalizedText("txtStartAudit");
			this.StrNumberOfProducts = new BoundLocalizedText("txtNumberOfProducts");
			this.StrDocumentation = new BoundLocalizedText("txtDocumentation");
			this.StrRejectAll = new BoundLocalizedText("txtRejectAll");
			this.StrAuditCheckProductChange = new BoundLocalizedText("txtAuditCheckProductChange");
			this.StrTimeOfDay = new BoundLocalizedText("txtTimeOfDay");
			this.StrCommonSettings = new BoundLocalizedText("txtCommonSettings");
			this.StrProductSpecific = new BoundLocalizedText("txtProductSpecificSettings");
			this.StrFastMode = new BoundLocalizedText("txtFastModeContaminations");
		}

		// Token: 0x06002656 RID: 9814 RVA: 0x001040C0 File Offset: 0x001022C0
		private void DeregisterLocalizedText(object obj)
		{
			MainWindow.localizationManager.Deregister(this.StrMode);
			MainWindow.localizationManager.Deregister(this.StrDay);
			MainWindow.localizationManager.Deregister(this.StrTime);
			MainWindow.localizationManager.Deregister(this.StrInterval);
			MainWindow.localizationManager.Deregister(this.StrAuditFailedError);
			MainWindow.localizationManager.Deregister(this.StrTimeToStartAudit);
			MainWindow.localizationManager.Deregister(this.StrAuditContaminationActive);
			MainWindow.localizationManager.Deregister(this.StrAuditWeightActive);
			MainWindow.localizationManager.Deregister(this.StrAuditCountActive);
			MainWindow.localizationManager.Deregister(this.StrNumberOfContaminations);
			MainWindow.localizationManager.Deregister(this.StrTimeToNextAudit);
			MainWindow.localizationManager.Deregister(this.StrStartAudit);
			MainWindow.localizationManager.Deregister(this.StrNumberOfProducts);
			MainWindow.localizationManager.Deregister(this.StrDocumentation);
			MainWindow.localizationManager.Deregister(this.StrRejectAll);
			MainWindow.localizationManager.Deregister(this.StrAuditCheckProductChange);
			MainWindow.localizationManager.Deregister(this.StrCommonSettings);
			MainWindow.localizationManager.Deregister(this.StrProductSpecific);
			MainWindow.localizationManager.Deregister(this.StrTimeOfDay);
			MainWindow.localizationManager.Deregister(this.StrFastMode);
			this.StrMode = null;
			this.StrDay = null;
			this.StrTime = null;
			this.StrInterval = null;
			this.StrAuditFailedError = null;
			this.StrTimeToStartAudit = null;
			this.StrAuditContaminationActive = null;
			this.StrAuditWeightActive = null;
			this.StrAuditCountActive = null;
			this.StrNumberOfContaminations = null;
			this.StrTimeToNextAudit = null;
			this.StrStartAudit = null;
			this.StrNumberOfProducts = null;
			this.StrRejectAll = null;
			this.StrFastMode = null;
			this.ChangeStartTimeHourCommand = null;
			this.ChangeStartTimeMinuteCommand = null;
			this.ChangeIntervalTimeHourCommand = null;
			this.ChangeIntervalTimeMinuteCommand = null;
			this.ChangeTimeToStartAuditCommand = null;
			this.ChangeNContaminationsCommand = null;
			this.ChangeNProductsCommand = null;
			this.TriggerAuditCommand = null;
			this.DeregisterLocalizedTextCommand = null;
			this.AuditSettings.PropertyChanged -= this.auditSettings_PropertyChanged;
		}

		// Token: 0x06002657 RID: 9815 RVA: 0x001042FC File Offset: 0x001024FC
		private void ChangeStartTimeHour(object dummy)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtAuditTimeHour"), this.AuditSettings.StartTimeHour.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputStartTimeHour;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x00104358 File Offset: 0x00102558
		private void numKeyboard_InputStartTimeHour(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.AuditSettings.StartTimeHour = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputStartTimeHour;
		}

		// Token: 0x06002659 RID: 9817 RVA: 0x001043B0 File Offset: 0x001025B0
		private void ChangeStartTimeMinute(object dummy)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtAuditTimeMinute"), this.AuditSettings.StartTimeMinute.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputStartTimeMinute;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600265A RID: 9818 RVA: 0x0010440C File Offset: 0x0010260C
		private void numKeyboard_InputStartTimeMinute(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.AuditSettings.StartTimeMinute = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputStartTimeMinute;
		}

		// Token: 0x0600265B RID: 9819 RVA: 0x00104464 File Offset: 0x00102664
		private void ChangeIntervalTimeHour(object dummy)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtAuditIntervalHour"), this.AuditSettings.IntervalHour.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputIntervalTimeHour;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600265C RID: 9820 RVA: 0x001044C0 File Offset: 0x001026C0
		private void numKeyboard_InputIntervalTimeHour(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.AuditSettings.IntervalHour = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputIntervalTimeHour;
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x00104518 File Offset: 0x00102718
		private void ChangeIntervalTimeMinute(object dummy)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtAuditIntervalMinute"), this.AuditSettings.IntervalMinute.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputIntervalTimeMinute;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x00104574 File Offset: 0x00102774
		private void numKeyboard_InputIntervalTimeMinute(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.AuditSettings.IntervalMinute = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputIntervalTimeMinute;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x001045CC File Offset: 0x001027CC
		private void ChangeTimeToStartAudit(object dummy)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtAuditStartTime"), this.AuditSettings.AuditRequired.TimeToStartAudit.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputTimeToStartAudit;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x0010462C File Offset: 0x0010282C
		private void numKeyboard_InputTimeToStartAudit(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.AuditSettings.AuditRequired.TimeToStartAudit = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputTimeToStartAudit;
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x00104688 File Offset: 0x00102888
		private void ChangeNContaminations(object dummy)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumContaminations"), this.AuditSettings.AuditRequired.NContaminations.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNContaminations;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x001046E8 File Offset: 0x001028E8
		private void numKeyboard_InputNContaminations(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.AuditSettings.AuditRequired.NContaminations = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNContaminations;
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x00104744 File Offset: 0x00102944
		private void ChangeNProducts(object dummy)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtNumberOfProducts"), this.AuditSettings.AuditRequired.NProducts.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_InputNProducts;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x001047A4 File Offset: 0x001029A4
		private void numKeyboard_InputNProducts(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.AuditSettings.AuditRequired.NProducts = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_InputNProducts;
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x00104800 File Offset: 0x00102A00
		private void TriggerAudit(object dummy)
		{
			bool flag = !this.AuditSettings.ContaminationEnabled && !this.AuditSettings.CountEnabled && !this.AuditSettings.WeightEnabled;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoAuditActive"), DialogStyle.Error, DialogButtons.OK);
			}
			else
			{
				Thread thread = new Thread(new ThreadStart(this.AuditSettings.AuditRequired.TriggerAuditCheck));
				thread.Start();
			}
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x00104880 File Offset: 0x00102A80
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04001860 RID: 6240
		private NumKeyboard numKeyboard;

		// Token: 0x04001861 RID: 6241
		private bool ukUserRightsHL = false;

		// Token: 0x04001862 RID: 6242
		private AccessRightsManager accessManager;

		// Token: 0x04001863 RID: 6243
		private bool auditCheckForContaminationsAccessible;

		// Token: 0x04001881 RID: 6273
		private bool documentationEnabled;

		// Token: 0x04001882 RID: 6274
		private bool rejectAll = false;

		// Token: 0x04001883 RID: 6275
		private bool auditCheckProductChange = false;

		// Token: 0x04001884 RID: 6276
		private bool isCount = false;

		// Token: 0x04001885 RID: 6277
		private bool isWeight = false;

		// Token: 0x04001886 RID: 6278
		private bool isNotBulk = false;

		// Token: 0x04001887 RID: 6279
		private DBConnection dBConnection;

		// Token: 0x0400188A RID: 6282
		private ParameterHandling parHandling;

		// Token: 0x0400188B RID: 6283
		private bool first = true;
	}
}

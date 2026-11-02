using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x020000C2 RID: 194
	public class ComissioningViewModel : INotifyPropertyChanged
	{
		// Token: 0x14000061 RID: 97
		// (add) Token: 0x0600135A RID: 4954 RVA: 0x00071828 File Offset: 0x0006FA28
		// (remove) Token: 0x0600135B RID: 4955 RVA: 0x00071860 File Offset: 0x0006FA60
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x00071895 File Offset: 0x0006FA95
		// (set) Token: 0x0600135D RID: 4957 RVA: 0x0007189D File Offset: 0x0006FA9D
		public BoundLocalizedText StrMachineType { get; set; }

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x000718A6 File Offset: 0x0006FAA6
		// (set) Token: 0x0600135F RID: 4959 RVA: 0x000718AE File Offset: 0x0006FAAE
		public BoundLocalizedText StrMachineSubType { get; set; }

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x000718B7 File Offset: 0x0006FAB7
		// (set) Token: 0x06001361 RID: 4961 RVA: 0x000718BF File Offset: 0x0006FABF
		public BoundLocalizedText StrTunnel { get; set; }

		// Token: 0x170005F4 RID: 1524
		// (get) Token: 0x06001362 RID: 4962 RVA: 0x000718C8 File Offset: 0x0006FAC8
		// (set) Token: 0x06001363 RID: 4963 RVA: 0x000718D0 File Offset: 0x0006FAD0
		public BoundLocalizedText StrDetector { get; set; }

		// Token: 0x170005F5 RID: 1525
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x000718D9 File Offset: 0x0006FAD9
		// (set) Token: 0x06001365 RID: 4965 RVA: 0x000718E1 File Offset: 0x0006FAE1
		public BoundLocalizedText StrFlapsCount { get; set; }

		// Token: 0x170005F6 RID: 1526
		// (get) Token: 0x06001366 RID: 4966 RVA: 0x000718EA File Offset: 0x0006FAEA
		// (set) Token: 0x06001367 RID: 4967 RVA: 0x000718F2 File Offset: 0x0006FAF2
		public BoundLocalizedText StrIOsCount { get; set; }

		// Token: 0x170005F7 RID: 1527
		// (get) Token: 0x06001368 RID: 4968 RVA: 0x000718FB File Offset: 0x0006FAFB
		// (set) Token: 0x06001369 RID: 4969 RVA: 0x00071903 File Offset: 0x0006FB03
		public BoundLocalizedText StrSensors { get; set; }

		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x0600136A RID: 4970 RVA: 0x0007190C File Offset: 0x0006FB0C
		// (set) Token: 0x0600136B RID: 4971 RVA: 0x00071914 File Offset: 0x0006FB14
		public RelayCommand StartWritingParameterCommand { get; set; }

		// Token: 0x170005F9 RID: 1529
		// (get) Token: 0x0600136C RID: 4972 RVA: 0x0007191D File Offset: 0x0006FB1D
		// (set) Token: 0x0600136D RID: 4973 RVA: 0x00071925 File Offset: 0x0006FB25
		public RelayCommand ShowOverviewParameterCommand { get; set; }

		// Token: 0x170005FA RID: 1530
		// (get) Token: 0x0600136E RID: 4974 RVA: 0x0007192E File Offset: 0x0006FB2E
		// (set) Token: 0x0600136F RID: 4975 RVA: 0x00071936 File Offset: 0x0006FB36
		public RelayCommand ShowMultilaneAdjustmentsCommand { get; set; }

		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001370 RID: 4976 RVA: 0x0007193F File Offset: 0x0006FB3F
		// (set) Token: 0x06001371 RID: 4977 RVA: 0x00071947 File Offset: 0x0006FB47
		public RelayCommand ShowObjectBasedFlapsCommand { get; set; }

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00071950 File Offset: 0x0006FB50
		// (set) Token: 0x06001373 RID: 4979 RVA: 0x00071958 File Offset: 0x0006FB58
		public RelayCommand ShowFeaturesLicenseCommand { get; set; }

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001374 RID: 4980 RVA: 0x00071961 File Offset: 0x0006FB61
		// (set) Token: 0x06001375 RID: 4981 RVA: 0x00071969 File Offset: 0x0006FB69
		public RelayCommand ResetAuditCheckCommand { get; set; }

		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001376 RID: 4982 RVA: 0x00071972 File Offset: 0x0006FB72
		// (set) Token: 0x06001377 RID: 4983 RVA: 0x0007197A File Offset: 0x0006FB7A
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001378 RID: 4984 RVA: 0x00071983 File Offset: 0x0006FB83
		// (set) Token: 0x06001379 RID: 4985 RVA: 0x0007198B File Offset: 0x0006FB8B
		public List<string> MachineTypeList { get; set; }

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x0600137A RID: 4986 RVA: 0x00071994 File Offset: 0x0006FB94
		// (set) Token: 0x0600137B RID: 4987 RVA: 0x0007199C File Offset: 0x0006FB9C
		public List<string> SubMachineTypeList { get; set; }

		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600137C RID: 4988 RVA: 0x000719A5 File Offset: 0x0006FBA5
		// (set) Token: 0x0600137D RID: 4989 RVA: 0x000719AD File Offset: 0x0006FBAD
		public List<string> TunnelSizeList { get; set; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600137E RID: 4990 RVA: 0x000719B6 File Offset: 0x0006FBB6
		// (set) Token: 0x0600137F RID: 4991 RVA: 0x000719BE File Offset: 0x0006FBBE
		public List<string> DetectorList { get; set; }

		// Token: 0x17000603 RID: 1539
		// (get) Token: 0x06001380 RID: 4992 RVA: 0x000719C7 File Offset: 0x0006FBC7
		// (set) Token: 0x06001381 RID: 4993 RVA: 0x000719CF File Offset: 0x0006FBCF
		public List<string> SensorList { get; set; }

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06001382 RID: 4994 RVA: 0x000719D8 File Offset: 0x0006FBD8
		// (set) Token: 0x06001383 RID: 4995 RVA: 0x000719F0 File Offset: 0x0006FBF0
		public List<string> FlapsCountList
		{
			get
			{
				return this.flapsCountList;
			}
			set
			{
				this.flapsCountList = value;
				this.OnPropertyChanged("FlapsCountList");
			}
		}

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06001384 RID: 4996 RVA: 0x00071A06 File Offset: 0x0006FC06
		// (set) Token: 0x06001385 RID: 4997 RVA: 0x00071A0E File Offset: 0x0006FC0E
		public List<string> IOModuleCountList { get; set; }

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x00071A18 File Offset: 0x0006FC18
		// (set) Token: 0x06001387 RID: 4999 RVA: 0x00071A30 File Offset: 0x0006FC30
		public bool SVTEnabled
		{
			get
			{
				return this.svtEnabled;
			}
			set
			{
				bool flag = this.svtEnabled && !value;
				if (flag)
				{
					this.dbConnection.SetBulkObjectEnabled(false);
				}
				this.svtEnabled = value;
				if (value)
				{
					this.InitializeFlapsCountListSVT();
				}
				else
				{
					this.InitializeFlapsCountList();
				}
				this.CheckForObjectBasedFlapsToBeVisible();
				this.OnPropertyChanged("SVTEnabled");
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06001388 RID: 5000 RVA: 0x00071A90 File Offset: 0x0006FC90
		// (set) Token: 0x06001389 RID: 5001 RVA: 0x00071AA8 File Offset: 0x0006FCA8
		public string SelectedMachineType
		{
			get
			{
				return this.selectedMachineType;
			}
			set
			{
				bool flag = this.selectedMachineType != value;
				if (flag)
				{
					this.selectedMachineType = value;
					this.OnPropertyChanged("SelectedMachineType");
					this.InitializeSensorsList();
					this.InitializeTunnelSizeList();
					bool flag2 = value == "Raycon EX1";
					if (flag2)
					{
						this.IsSubTypeVisible = false;
						this.IsDetectorVisible = false;
						this.IsSensorsVisible = true;
						this.IsFlapsCountVisible = false;
						this.IsIOModuleCountVisible = false;
					}
					else
					{
						bool flag3 = value == "Raycon EX2A";
						if (flag3)
						{
							this.IsSubTypeVisible = false;
							this.IsDetectorVisible = false;
							this.IsSensorsVisible = true;
							this.IsFlapsCountVisible = false;
							this.IsIOModuleCountVisible = false;
						}
						else
						{
							bool flag4 = value == "Raycon D+ TX";
							if (flag4)
							{
								this.IsSubTypeVisible = false;
								this.IsDetectorVisible = false;
								this.IsIOModuleCountVisible = true;
								bool flag5 = this.SelectedSubMachineType == "Standard";
								if (flag5)
								{
									this.IsSensorsVisible = true;
									this.IsFlapsCountVisible = false;
								}
								else
								{
									bool flag6 = this.SelectedSubMachineType == "LW" || this.SelectedSubMachineType == "LW Hy";
									if (flag6)
									{
										this.IsSensorsVisible = true;
										this.IsFlapsCountVisible = false;
									}
									else
									{
										bool flag7 = this.SelectedSubMachineType == "Bulk" || this.SelectedSubMachineType == "Bulk Hy";
										if (flag7)
										{
											this.IsSensorsVisible = false;
											this.IsFlapsCountVisible = true;
										}
									}
								}
							}
							else
							{
								this.IsSubTypeVisible = true;
								this.IsDetectorVisible = true;
								this.IsIOModuleCountVisible = true;
								bool flag8 = this.SelectedSubMachineType == "Standard";
								if (flag8)
								{
									this.IsSensorsVisible = true;
									this.IsFlapsCountVisible = false;
								}
								else
								{
									bool flag9 = this.SelectedSubMachineType == "LW" || this.SelectedSubMachineType == "LW Hy";
									if (flag9)
									{
										this.IsSensorsVisible = true;
										this.IsFlapsCountVisible = false;
									}
									else
									{
										bool flag10 = this.SelectedSubMachineType == "Bulk" || this.SelectedSubMachineType == "Bulk Hy";
										if (flag10)
										{
											this.IsSensorsVisible = false;
											this.IsFlapsCountVisible = true;
										}
									}
								}
								this.InitializeDetectorList();
							}
						}
					}
					this.CheckForObjectBasedFlapsToBeVisible();
				}
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x0600138A RID: 5002 RVA: 0x00071D10 File Offset: 0x0006FF10
		// (set) Token: 0x0600138B RID: 5003 RVA: 0x00071D28 File Offset: 0x0006FF28
		public string SelectedSubMachineType
		{
			get
			{
				return this.selectedSubMachineType;
			}
			set
			{
				bool flag = this.SelectedMachineType != "Raycon EX1" && this.SelectedMachineType != "Raycon EX2A";
				if (flag)
				{
					this.IsDetectorVisible = true;
				}
				this.selectedSubMachineType = value;
				this.OnPropertyChanged("SelectedSubMachineType");
				this.InitializeTunnelSizeList();
				bool flag2 = value == "Standard";
				if (flag2)
				{
					this.IsSensorsVisible = true;
					this.IsFlapsCountVisible = false;
					this.IsIOModuleCountVisible = true;
				}
				else
				{
					bool flag3 = value == "LW" || value == "LW Hy";
					if (flag3)
					{
						this.IsSensorsVisible = true;
						this.IsFlapsCountVisible = false;
						this.IsIOModuleCountVisible = true;
					}
					else
					{
						bool flag4 = value == "Bulk" || value == "Bulk Hy";
						if (flag4)
						{
							this.IsSensorsVisible = false;
							this.IsFlapsCountVisible = true;
							this.IsIOModuleCountVisible = true;
						}
					}
				}
				this.CheckForObjectBasedFlapsToBeVisible();
			}
		}

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00071E28 File Offset: 0x00070028
		// (set) Token: 0x0600138D RID: 5005 RVA: 0x00071E40 File Offset: 0x00070040
		public string SelectedTunnelSize
		{
			get
			{
				return this.selectedTunnelSize;
			}
			set
			{
				bool flag = this.selectedTunnelSize != value;
				if (flag)
				{
					this.selectedTunnelSize = value;
					this.OnPropertyChanged("SelectedTunnelSize");
				}
			}
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x00071E74 File Offset: 0x00070074
		// (set) Token: 0x0600138F RID: 5007 RVA: 0x00071E8C File Offset: 0x0007008C
		public string SelectedDetector
		{
			get
			{
				return this.selectedDetector;
			}
			set
			{
				bool flag = this.selectedDetector != value;
				if (flag)
				{
					this.selectedDetector = value;
					this.OnPropertyChanged("SelectedDetector");
				}
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06001390 RID: 5008 RVA: 0x00071EC0 File Offset: 0x000700C0
		// (set) Token: 0x06001391 RID: 5009 RVA: 0x00071ED8 File Offset: 0x000700D8
		public string SelectedSensors
		{
			get
			{
				return this.selectedSensors;
			}
			set
			{
				bool flag = this.selectedSensors != value;
				if (flag)
				{
					this.selectedSensors = value;
					this.OnPropertyChanged("SelectedSensors");
				}
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001392 RID: 5010 RVA: 0x00071F0C File Offset: 0x0007010C
		// (set) Token: 0x06001393 RID: 5011 RVA: 0x00071F24 File Offset: 0x00070124
		public string SelectedFlapsCount
		{
			get
			{
				return this.selectedFlapsCount;
			}
			set
			{
				bool flag = this.selectedFlapsCount != value;
				if (flag)
				{
					this.selectedFlapsCount = value;
					this.OnPropertyChanged("SelectedFlapsCount");
				}
			}
		}

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06001394 RID: 5012 RVA: 0x00071F58 File Offset: 0x00070158
		// (set) Token: 0x06001395 RID: 5013 RVA: 0x00071F70 File Offset: 0x00070170
		public string SelectedIOsCount
		{
			get
			{
				return this.selectedIOsCount;
			}
			set
			{
				bool flag = this.selectedIOsCount != value;
				if (flag)
				{
					this.selectedIOsCount = value;
					this.OnPropertyChanged("SelectedIOsCount");
				}
			}
		}

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06001396 RID: 5014 RVA: 0x00071FA4 File Offset: 0x000701A4
		// (set) Token: 0x06001397 RID: 5015 RVA: 0x00071FBC File Offset: 0x000701BC
		public bool IsSubTypeVisible
		{
			get
			{
				return this.isSubTypeVisible;
			}
			set
			{
				this.isSubTypeVisible = value;
				this.OnPropertyChanged("IsSubTypeVisible");
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06001398 RID: 5016 RVA: 0x00071FD4 File Offset: 0x000701D4
		// (set) Token: 0x06001399 RID: 5017 RVA: 0x00071FEC File Offset: 0x000701EC
		public bool IsDetectorVisible
		{
			get
			{
				return this.isDetectorVisible;
			}
			set
			{
				this.isDetectorVisible = value;
				this.OnPropertyChanged("IsDetectorVisible");
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x0600139A RID: 5018 RVA: 0x00072004 File Offset: 0x00070204
		// (set) Token: 0x0600139B RID: 5019 RVA: 0x0007201C File Offset: 0x0007021C
		public bool IsSensorsVisible
		{
			get
			{
				return this.isSensorsVisible;
			}
			set
			{
				this.isSensorsVisible = value;
				this.OnPropertyChanged("IsSensorsVisible");
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x0600139C RID: 5020 RVA: 0x00072034 File Offset: 0x00070234
		// (set) Token: 0x0600139D RID: 5021 RVA: 0x0007204C File Offset: 0x0007024C
		public bool IsFlapsCountVisible
		{
			get
			{
				return this.isFlapsCountVisible;
			}
			set
			{
				this.isFlapsCountVisible = value;
				this.OnPropertyChanged("IsFlapsCountVisible");
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x0600139E RID: 5022 RVA: 0x00072064 File Offset: 0x00070264
		// (set) Token: 0x0600139F RID: 5023 RVA: 0x0007207C File Offset: 0x0007027C
		public bool IsIOModuleCountVisible
		{
			get
			{
				return this.isIOModuleCountVisible;
			}
			set
			{
				this.isIOModuleCountVisible = value;
				this.OnPropertyChanged("IsIOModuleCountVisible");
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x060013A0 RID: 5024 RVA: 0x00072094 File Offset: 0x00070294
		// (set) Token: 0x060013A1 RID: 5025 RVA: 0x000720AC File Offset: 0x000702AC
		public bool ObjectBasedFlapsVisible
		{
			get
			{
				return this.objectBasedFlapsVisible;
			}
			set
			{
				this.objectBasedFlapsVisible = value;
				this.OnPropertyChanged("ObjectBasedFlapsVisible");
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060013A2 RID: 5026 RVA: 0x000720C4 File Offset: 0x000702C4
		// (set) Token: 0x060013A3 RID: 5027 RVA: 0x000720DC File Offset: 0x000702DC
		public bool ObjectBasedFlapsEnabled
		{
			get
			{
				return this.objectBasedFlapsEnabled;
			}
			set
			{
				bool flag = this.objectBasedFlapsEnabled != value;
				if (flag)
				{
					this.parHandling.WriteValue(Definition.objectBasedFlapsEnabledPath, value ? "1" : "0");
					this.objectBasedFlapsEnabled = value;
					MachineMode machineMode = MachineMode.Standard;
					this.dbConnection.GetMachineMode(ref machineMode);
					bool flag2 = machineMode != MachineMode.Bulk && machineMode != MachineMode.BulkObject;
					if (flag2)
					{
						bool flag3 = !this.objectBasedFlapsEnabled;
						if (flag3)
						{
							this.parHandling.WriteValue(Definition.objectBasedFlapsCountPath, "1");
							this.writeToActProdAndDefault(Acquisition.acquisitionModePath, "0");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter1/Mode/Value", "0");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter2/Mode/Value", "0");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter3/Mode/Value", "0");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter4/Mode/Value", "0");
						}
						else
						{
							this.writeToActProdAndDefault(Acquisition.acquisitionModePath, "1");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter1/Mode/Value", "1");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter2/Mode/Value", "1");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter3/Mode/Value", "1");
							this.writeToActProdAndDefault("/hardware/rejecter/rejecter4/Mode/Value", "1");
						}
					}
				}
				this.OnPropertyChanged("ObjectBasedFlapsEnabled");
			}
		}

		// Token: 0x060013A4 RID: 5028 RVA: 0x0007222C File Offset: 0x0007042C
		public ComissioningViewModel(DBConnection dbConnection, ParameterHandling parHandling, DBDataLogs dBDataLogs, AuditCheckSettings auditSettings)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.dBDataLogs = dBDataLogs;
			this.auditSettings = auditSettings;
			this.Initialize();
			this.StartWritingParameterCommand = new RelayCommand(new Action<object>(this.WriteParameterFromDBToIPC));
			this.ShowOverviewParameterCommand = new RelayCommand(new Action<object>(this.ShowOverview));
			this.ShowMultilaneAdjustmentsCommand = new RelayCommand(new Action<object>(this.ShowMultilaneParams));
			this.ShowObjectBasedFlapsCommand = new RelayCommand(new Action<object>(this.ShowObjectBasedFlaps));
			this.ShowFeaturesLicenseCommand = new RelayCommand(new Action<object>(this.ShowFeaturesActivation));
			this.ResetAuditCheckCommand = new RelayCommand(new Action<object>(this.ResetAuditCheck));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00072378 File Offset: 0x00070578
		private void Initialize()
		{
			this.firstInit = true;
			this.InitializeSelection();
			this.InitializeLocalizedText();
			this.InitializeMachineTypeList();
			this.InitializeSubMachineTypeList();
			this.InitializeTunnelSizeList();
			this.InitializeDetectorList();
			bool flag = !this.SVTEnabled;
			if (flag)
			{
				this.InitializeFlapsCountList();
			}
			else
			{
				this.InitializeFlapsCountListSVT();
			}
			this.InitializeSensorsList();
			this.InitializeIOModulCountList();
			this.firstInit = false;
			this.btnStartWritingParameter = new Button();
			this.btnComissioningMultilane = new Button();
			this.btnComissioningFeaturesLicense = new Button();
			this.btnOverview = new Button();
			this.btnAuditCheck = new Button();
			bool flag2 = false;
			this.dbConnection.GetObjectBasedFlapsEnabled(ref flag2);
			this.ObjectBasedFlapsEnabled = flag2;
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x00072438 File Offset: 0x00070638
		private void InitializeLocalizedText()
		{
			this.StrMachineType = new BoundLocalizedText("txtMachineType");
			this.StrMachineSubType = new BoundLocalizedText("txtMachineSubType");
			this.StrTunnel = new BoundLocalizedText("txtTunnel");
			this.StrDetector = new BoundLocalizedText("parItemDetector");
			this.StrSensors = new BoundLocalizedText("txtSensors");
			this.StrFlapsCount = new BoundLocalizedText("txtFlapsCount");
			this.StrIOsCount = new BoundLocalizedText("txtIOsCount");
		}

		// Token: 0x060013A7 RID: 5031 RVA: 0x000724C0 File Offset: 0x000706C0
		private void InitializeMachineTypeList()
		{
			string a = "";
			this.dbConnection.GetXRaySourceModelNumber(ref a);
			this.MachineTypeList = new List<string>();
			bool flag = a == "IXS050SE060P359" || a == "X5884";
			if (flag)
			{
				this.MachineTypeList.Add("Raycon EX1");
			}
			else
			{
				bool flag2 = a == "IXS050SE100P214";
				if (flag2)
				{
					this.MachineTypeList.Add("Raycon D+ MX");
					this.MachineTypeList.Add("Raycon EX2A");
				}
				else
				{
					bool flag3 = a == "IXS100BP200P395" || a == "IXS100BP200P367" || a == "XRB100PN210HRX5885" || a == "IXS060BP200P630" || a == "IXS060BP200P760";
					if (flag3)
					{
						this.MachineTypeList.Add("Raycon D+ HX");
						this.MachineTypeList.Add("Raycon D+ TX");
					}
					else
					{
						this.MachineTypeList.Add("Raycon EX1");
						this.MachineTypeList.Add("Raycon EX2A");
						this.MachineTypeList.Add("Raycon D+ MX");
						this.MachineTypeList.Add("Raycon D+ HX");
						this.MachineTypeList.Add("Raycon D+ TX");
					}
				}
			}
			this.OnPropertyChanged("MachineTypeList");
		}

		// Token: 0x060013A8 RID: 5032 RVA: 0x0007262C File Offset: 0x0007082C
		private void InitializeSubMachineTypeList()
		{
			this.SubMachineTypeList = new List<string>();
			this.SubMachineTypeList.Add("Standard");
			this.SubMachineTypeList.Add("LW");
			this.SubMachineTypeList.Add("Bulk");
			this.SubMachineTypeList.Add("LW Hy");
			this.SubMachineTypeList.Add("Bulk Hy");
			this.OnPropertyChanged("SubMachineTypeList");
		}

		// Token: 0x060013A9 RID: 5033 RVA: 0x000726A8 File Offset: 0x000708A8
		private void InitializeTunnelSizeList()
		{
			this.TunnelSizeList = new List<string>();
			bool flag = this.SelectedMachineType == "Raycon EX1";
			if (flag)
			{
				this.TunnelSizeList.Add("200/120");
				bool flag2 = !this.firstInit;
				if (flag2)
				{
					this.SelectedTunnelSize = "200/120";
				}
			}
			else
			{
				bool flag3 = this.SelectedMachineType == "Raycon EX2A";
				if (flag3)
				{
					this.TunnelSizeList.Add("300/150");
					this.TunnelSizeList.Add("400/250");
					bool flag4 = !this.firstInit;
					if (flag4)
					{
						this.SelectedTunnelSize = "300/150";
					}
				}
				else
				{
					bool flag5 = this.SelectedSubMachineType == "Standard";
					if (flag5)
					{
						this.TunnelSizeList.Add("330/200");
						this.TunnelSizeList.Add("450/250");
						this.TunnelSizeList.Add("600/120");
						bool flag6 = !this.firstInit;
						if (flag6)
						{
							this.SelectedTunnelSize = "330/200";
						}
					}
					else
					{
						bool flag7 = this.SelectedSubMachineType == "LW" || this.SelectedSubMachineType == "Bulk" || this.SelectedSubMachineType == "LW Hy" || this.SelectedSubMachineType == "Bulk Hy";
						if (flag7)
						{
							this.TunnelSizeList.Add("330/50");
							this.TunnelSizeList.Add("630/50");
							bool flag8 = !this.firstInit;
							if (flag8)
							{
								this.SelectedTunnelSize = "330/50";
							}
						}
					}
				}
			}
			bool flag9 = this.SelectedMachineType != "Raycon EX2A";
			if (flag9)
			{
				this.TunnelSizeList.Add("Other");
			}
			this.OnPropertyChanged("TunnelSizeList");
		}

		// Token: 0x060013AA RID: 5034 RVA: 0x0007288C File Offset: 0x00070A8C
		private void InitializeDetectorList()
		{
			this.DetectorList = new List<string>();
			bool flag = this.SelectedMachineType == "Raycon D+ MX";
			if (flag)
			{
				this.DetectorList.Add("0.8 mm");
				this.DetectorList.Add("0.4 mm");
				bool flag2 = !this.firstInit;
				if (flag2)
				{
					this.SelectedDetector = "0.8 mm";
				}
			}
			else
			{
				bool flag3 = this.SelectedMachineType == "Raycon D+ HX";
				if (flag3)
				{
					this.DetectorList.Add("0.4 mm");
					this.DetectorList.Add("Dual Energy");
					bool flag4 = !this.firstInit;
					if (flag4)
					{
						this.SelectedDetector = "0.4 mm";
					}
				}
				else
				{
					this.DetectorList.Add("0.8 mm");
					this.DetectorList.Add("0.4 mm");
					this.DetectorList.Add("Dual Energy");
				}
			}
			this.OnPropertyChanged("DetectorList");
		}

		// Token: 0x060013AB RID: 5035 RVA: 0x00072990 File Offset: 0x00070B90
		private void InitializeFlapsCountList()
		{
			this.FlapsCountList = new List<string>
			{
				"1",
				"4"
			};
			bool flag = !this.firstInit;
			if (flag)
			{
				this.SelectedFlapsCount = this.FlapsCountList.Last<string>();
			}
		}

		// Token: 0x060013AC RID: 5036 RVA: 0x000729E4 File Offset: 0x00070BE4
		private void InitializeFlapsCountListSVT()
		{
			this.FlapsCountList = new List<string>
			{
				"10",
				"20"
			};
			bool flag = !this.firstInit;
			if (flag)
			{
				bool flag2 = this.SelectedTunnelSize.Equals("630/50");
				if (flag2)
				{
					this.SelectedFlapsCount = this.FlapsCountList.Last<string>();
				}
				else
				{
					this.SelectedFlapsCount = this.FlapsCountList.First<string>();
				}
			}
		}

		// Token: 0x060013AD RID: 5037 RVA: 0x00072A61 File Offset: 0x00070C61
		private void InitializeIOModulCountList()
		{
			this.IOModuleCountList = new List<string>();
			this.IOModuleCountList.Add("1");
			this.IOModuleCountList.Add("2");
		}

		// Token: 0x060013AE RID: 5038 RVA: 0x00072A94 File Offset: 0x00070C94
		private void InitializeSensorsList()
		{
			this.SensorList = new List<string>();
			bool flag = this.SelectedMachineType == "Raycon EX1" || this.SelectedMachineType == "Raycon EX2A";
			if (flag)
			{
				this.SensorList.Add("Standard");
				this.SensorList.Add("HighLevel");
				bool flag2 = !this.firstInit;
				if (flag2)
				{
					this.SelectedSensors = "Standard";
				}
			}
			else
			{
				this.SensorList.Add("Standard");
				this.SensorList.Add("HighLevel");
				this.SensorList.Add("Advanced");
			}
			this.OnPropertyChanged("SensorList");
		}

		// Token: 0x060013AF RID: 5039 RVA: 0x00072B58 File Offset: 0x00070D58
		private void InitializeSelection()
		{
			string a = "";
			string text = "";
			string text2 = "";
			string text3 = "";
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			string text8 = "";
			string text9 = "";
			string text10 = "";
			string text11 = "";
			string ioMachineType = "";
			string text12 = "";
			this.dbConnection.GetStatusInfoComissioning(ref a, ref text, ref text2, ref text3, ref text4, ref text5, ref text6, ref text7, ref text8, ref text9, ref text10, ref ioMachineType, ref text11, ref text12);
			bool flag = a != "";
			if (flag)
			{
				this.SelectedSensors = text9;
				this.SelectedDetector = text8;
				this.SelectedTunnelSize = text7;
				this.SelectedSubMachineType = text6;
				this.SelectedMachineType = text5;
				this.SelectedFlapsCount = text10;
				this.SelectedIOsCount = this.convertIOMachineTypeToIOsCount(ioMachineType);
				this.firstSimulation = false;
				bool flag2 = text11.Equals("0");
				if (flag2)
				{
					this.SVTEnabled = false;
				}
				else
				{
					this.SVTEnabled = true;
				}
			}
			else
			{
				this.SelectedMachineType = "Raycon D+ HX";
				this.SelectedSubMachineType = "Standard";
				this.SelectedTunnelSize = "330/200";
				this.SelectedDetector = "0.4 mm";
				this.SelectedSensors = "Standard";
				this.SelectedFlapsCount = "1";
				this.SelectedIOsCount = "1";
				this.firstSimulation = true;
				this.SVTEnabled = false;
			}
		}

		// Token: 0x060013B0 RID: 5040 RVA: 0x00072CD8 File Offset: 0x00070ED8
		private string convertIOMachineTypeToIOsCount(string ioMachineType)
		{
			bool flag = ioMachineType == "Param1" || ioMachineType == "Param3";
			string result;
			if (flag)
			{
				result = "1";
			}
			else
			{
				result = "2";
			}
			return result;
		}

		// Token: 0x060013B1 RID: 5041 RVA: 0x00072D18 File Offset: 0x00070F18
		private void WriteParameterFromDBToIPC(object obj)
		{
			bool enable = false;
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			this.activateAdvancedFilter(true);
			this.parHandling.WriteValue(Definition.buzzerAvailablePath, "1");
			this.parHandling.WriteValue(Definition.useAuditCheckLampPath, "1");
			this.parHandling.WriteValue(Definition.detectorMonitoringActive, "1");
			this.parHandling.WriteValue(Definition.detectorMonitoringThreshold, "15");
			this.parHandling.WriteValue(Definition.detectorMonitoringPixelCountBorder, "5");
			this.parHandling.WriteValue(Definition.xraySourcePortCom, "1");
			bool flag = this.SelectedMachineType == "Raycon EX1";
			if (flag)
			{
				bool flag2 = this.SelectedSensors == "HighLevel";
				if (flag2)
				{
					this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameHighlevel, true);
					this.dbConnection.SetHigherLevelFeatures(true);
				}
				else
				{
					bool flag3 = this.SelectedSensors == "Standard";
					if (flag3)
					{
						this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameStandard, true);
						this.dbConnection.SetHigherLevelFeatures(false);
					}
				}
				this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameEX1, true);
				this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
				this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
				this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
				this.selectedDetector = "0.8 mm";
				this.selectedSubMachineType = "Standard";
				this.selectedIOsCount = "1";
				this.parHandling.WriteValue(Definition.detectorMonitoringThreshold, "10");
				this.parHandling.WriteValue(Definition.detectorMonitoringPixelCountBorder, "3");
			}
			else
			{
				bool flag4 = this.SelectedMachineType == "Raycon EX2A";
				if (flag4)
				{
					bool flag5 = this.SelectedSensors == "HighLevel";
					if (flag5)
					{
						this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameHighlevel, true);
						this.dbConnection.SetHigherLevelFeatures(true);
					}
					else
					{
						bool flag6 = this.SelectedSensors == "Standard";
						if (flag6)
						{
							this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameStandard, true);
							this.dbConnection.SetHigherLevelFeatures(false);
						}
					}
					this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameEX2A, true);
					this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
					this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
					this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam4, true);
					this.selectedDetector = "0.4 mm";
					this.selectedSubMachineType = "Standard";
					this.selectedIOsCount = "1";
					this.parHandling.WriteValue(Definition.buzzerAvailablePath, "0");
					this.parHandling.WriteValue(Definition.xraySourcePortCom, "3");
				}
				else
				{
					bool flag7 = this.SelectedMachineType == "Raycon D+ MX";
					if (flag7)
					{
						bool flag8 = this.SelectedIOsCount == "2";
						if (flag8)
						{
							this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam2, true);
						}
						else
						{
							bool flag9 = this.SelectedIOsCount == "1";
							if (flag9)
							{
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
							}
						}
						bool flag10 = this.SelectedSubMachineType != "Bulk" && this.SelectedSubMachineType != "Bulk Hy";
						if (flag10)
						{
							bool flag11 = this.SelectedSensors == "HighLevel";
							if (flag11)
							{
								this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameHighlevel, true);
								this.dbConnection.SetHigherLevelFeatures(true);
							}
							else
							{
								bool flag12 = this.SelectedSensors == "Standard";
								if (flag12)
								{
									this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameStandard, true);
									this.dbConnection.SetHigherLevelFeatures(false);
								}
								else
								{
									bool flag13 = this.SelectedSensors == "Advanced";
									if (flag13)
									{
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameAdvanced, true);
										this.dbConnection.SetHigherLevelFeatures(false);
									}
								}
							}
						}
						bool flag14 = this.SelectedSubMachineType == "Standard";
						if (flag14)
						{
							bool flag15 = this.SelectedDetector == "0.8 mm";
							if (flag15)
							{
								this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameMX, true);
								this.parHandling.WriteValue(Definition.detectorMonitoringPixelCountBorder, "3");
							}
							else
							{
								bool flag16 = this.SelectedDetector == "0.4 mm";
								if (flag16)
								{
									this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameMX04, true);
								}
							}
							bool flag17 = this.SelectedTunnelSize == "600/120";
							if (flag17)
							{
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
							}
							else
							{
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
							}
							this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
						}
						else
						{
							bool flag18 = this.SelectedSubMachineType == "LW" || this.SelectedSubMachineType == "LW Hy";
							if (flag18)
							{
								bool flag19 = this.SelectedDetector == "0.8 mm";
								if (flag19)
								{
									this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameMX, true);
									this.parHandling.WriteValue(Definition.detectorMonitoringPixelCountBorder, "3");
								}
								else
								{
									bool flag20 = this.SelectedDetector == "0.4 mm";
									if (flag20)
									{
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameMX04, true);
									}
								}
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam2, true);
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
							}
							else
							{
								bool flag21 = this.SelectedSubMachineType == "Bulk" || this.SelectedSubMachineType == "Bulk Hy";
								if (flag21)
								{
									this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameMXBulk, true);
									list.Add("/machine/NAutolearnDefaultBulk");
									list2.Add("30");
									this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameBulk, true);
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, (this.SelectedFlapsCount == "1") ? DBComissioning.DBColumnNameParam2 : DBComissioning.DBColumnNameParam3, true);
									this.activateAdvancedFilter(false);
									this.selectedSensors = "Advanced";
									this.parHandling.WriteValue(Definition.detectorMonitoringActive, "0");
									this.dBDataLogs.AddRejectOnOffTimes();
									bool svtenabled = this.SVTEnabled;
									if (svtenabled)
									{
										int index = list.IndexOf("/hardware/rejecter/bulkflaps/Enabled");
										list2[index] = "0";
										index = list.IndexOf("/hardware/rejecter/bulkflaps/EnableValves");
										list2[index] = "0";
										list.Add("/hardware/svt/Enable");
										list2.Add("1");
										list.Add("/hardware/svt/ValvesEnabled");
										list2.Add("0");
										int num = 20;
										int.TryParse(this.SelectedFlapsCount, out num);
										this.dbConnection.SetNumValves(num);
										list.Add("/hardware/svt/testing/MaxValve");
										list2.Add(this.SelectedFlapsCount);
										this.dBDataLogs.AddSVTValves(num);
										this.dBDataLogs.AddSVTValvesOperatingCounter(num);
									}
								}
							}
						}
					}
					else
					{
						bool flag22 = this.SelectedMachineType == "Raycon D+ HX";
						if (flag22)
						{
							bool flag23 = this.SelectedIOsCount == "2";
							if (flag23)
							{
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam2, true);
							}
							else
							{
								bool flag24 = this.SelectedIOsCount == "1";
								if (flag24)
								{
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
								}
							}
							bool flag25 = this.SelectedSubMachineType != "Bulk" && this.SelectedSubMachineType != "Bulk Hy";
							if (flag25)
							{
								bool flag26 = this.SelectedSensors == "HighLevel";
								if (flag26)
								{
									this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameHighlevel, true);
									this.dbConnection.SetHigherLevelFeatures(true);
								}
								else
								{
									bool flag27 = this.SelectedSensors == "Standard";
									if (flag27)
									{
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameStandard, true);
										this.dbConnection.SetHigherLevelFeatures(false);
									}
									else
									{
										bool flag28 = this.SelectedSensors == "Advanced";
										if (flag28)
										{
											this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameAdvanced, true);
											this.dbConnection.SetHigherLevelFeatures(false);
										}
									}
								}
							}
							bool flag29 = this.SelectedSubMachineType == "Standard";
							if (flag29)
							{
								bool flag30 = this.SelectedDetector == "0.4 mm";
								if (flag30)
								{
									this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameHX, true);
									enable = true;
								}
								else
								{
									bool flag31 = this.SelectedDetector == "Dual Energy";
									if (flag31)
									{
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameHXDual, true);
										this.parHandling.WriteValue(Definition.detectorMonitoringActive, "0");
									}
								}
								bool flag32 = this.SelectedTunnelSize == "600/120";
								if (flag32)
								{
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
								}
								else
								{
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
								}
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
							}
							else
							{
								bool flag33 = this.SelectedSubMachineType == "LW" || this.SelectedSubMachineType == "LW Hy";
								if (flag33)
								{
									bool flag34 = this.SelectedDetector == "0.4 mm";
									if (flag34)
									{
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameHX, true);
										enable = true;
									}
									else
									{
										bool flag35 = this.SelectedDetector == "Dual Energy";
										if (flag35)
										{
											this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameHXDual, true);
											this.parHandling.WriteValue(Definition.detectorMonitoringActive, "0");
										}
									}
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam2, true);
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
								}
								else
								{
									bool flag36 = this.SelectedSubMachineType == "Bulk" || this.SelectedSubMachineType == "Bulk Hy";
									if (flag36)
									{
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameHXBulk, true);
										list.Add("/machine/NAutolearnDefaultBulk");
										list2.Add("30");
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameBulk, true);
										this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
										this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, (this.SelectedFlapsCount == "1") ? DBComissioning.DBColumnNameParam2 : DBComissioning.DBColumnNameParam3, true);
										this.activateAdvancedFilter(false);
										this.selectedSensors = "Advanced";
										this.parHandling.WriteValue(Definition.detectorMonitoringActive, "0");
										this.dBDataLogs.AddRejectOnOffTimes();
										bool svtenabled2 = this.SVTEnabled;
										if (svtenabled2)
										{
											int index2 = list.IndexOf("/hardware/rejecter/bulkflaps/Enabled");
											list2[index2] = "0";
											index2 = list.IndexOf("/hardware/rejecter/bulkflaps/EnableValves");
											list2[index2] = "0";
											list.Add("/hardware/svt/Enable");
											list2.Add("1");
											list.Add("/hardware/svt/ValvesEnabled");
											list2.Add("0");
											int num2 = 20;
											int.TryParse(this.SelectedFlapsCount, out num2);
											this.dbConnection.SetNumValves(num2);
											list.Add("/hardware/svt/testing/MaxValve");
											list2.Add(this.SelectedFlapsCount);
											this.dBDataLogs.AddSVTValves(num2);
											this.dBDataLogs.AddSVTValvesOperatingCounter(num2);
										}
									}
								}
							}
						}
						else
						{
							bool flag37 = this.SelectedMachineType == "Raycon D+ TX";
							if (flag37)
							{
								bool flag38 = this.SelectedIOsCount == "2";
								if (flag38)
								{
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam2, true);
								}
								else
								{
									bool flag39 = this.SelectedIOsCount == "1";
									if (flag39)
									{
										this.dbConnection.GetEntriesComissioningOthers(list, list2, 3, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
									}
								}
								bool flag40 = this.SelectedSubMachineType != "Bulk" && this.SelectedSubMachineType != "Bulk Hy";
								if (flag40)
								{
									bool flag41 = this.SelectedSensors == "HighLevel";
									if (flag41)
									{
										this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameHighlevel, true);
										this.dbConnection.SetHigherLevelFeatures(true);
									}
									else
									{
										bool flag42 = this.SelectedSensors == "Standard";
										if (flag42)
										{
											this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameStandard, true);
											this.dbConnection.SetHigherLevelFeatures(false);
										}
										else
										{
											bool flag43 = this.SelectedSensors == "Advanced";
											if (flag43)
											{
												this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterHardware, DBComissioning.DBColumnNameAdvanced, true);
												this.dbConnection.SetHigherLevelFeatures(false);
											}
										}
									}
								}
								this.dbConnection.GetEntriesComissioningMachineHardware(list, list2, DBComissioning.DBColumnNameParameterMachine, DBComissioning.DBColumnNameTX, true);
								enable = true;
								bool flag44 = this.SelectedTunnelSize == "600/120";
								if (flag44)
								{
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam3, true);
								}
								else
								{
									this.dbConnection.GetEntriesComissioningOthers(list, list2, 1, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
								}
								this.dbConnection.GetEntriesComissioningOthers(list, list2, 2, DBComissioning.DBColumnNameParameterOthers, DBComissioning.DBColumnNameParam1, true);
								this.parHandling.WriteValue(Definition.detectorMonitoringThreshold, "10");
							}
						}
					}
				}
			}
			this.WriteParmamsToIPC(list, list2);
			bool flag45 = this.SelectedMachineType == "Raycon D+ TX";
			if (flag45)
			{
				for (int i = 1; i <= Definition.nObjectFinder; i++)
				{
					this.writeToActProdAndDefault(Definition.filterMainPath + Definition.objectFindersSubPath + i.ToString() + "/Enabled/Value", "0");
				}
				for (int j = 1; j <= Definition.nComparer; j++)
				{
					this.writeToActProdAndDefault("/filter/comparer/comparer" + j.ToString() + "/Enabled/Value", "1");
				}
			}
			this.activateTestcardSim(enable);
			this.dbConnection.SetStatusInfoComissioning(this.SelectedMachineType, this.SelectedSubMachineType, this.SelectedTunnelSize, this.SelectedDetector, this.SelectedSensors, this.SelectedFlapsCount, this.SVTEnabled ? "1" : "0");
			this.WriteMachineInfos(this.SelectedMachineType, this.SelectedSubMachineType, this.SelectedTunnelSize);
			this.btnOverview.Visibility = Visibility.Visible;
			this.setLastXRayOnTimeIfPossible();
			this.setadditionalParamsBeltStopIfError();
			this.setComp19to30dependingOnImgProcPC();
			this.setAIParameters();
			this.setObjectBasedReject();
			this.dBDataLogs.setCommissioningDefaultLimits(this.SelectedMachineType, this.SelectedSubMachineType, this.SelectedDetector);
			string message = LocalizationManager.Localize("txtParameterLoadedSuccess");
			Dialog.Show(LocalizationManager.Localize("txtSuccess"), message, DialogStyle.Ok, DialogButtons.OK);
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00073E28 File Offset: 0x00072028
		private void setadditionalParamsBeltStopIfError()
		{
			bool flag = this.parHandling.ReadPar(Definition.beltStopOnErrorEnablePath) == "1";
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			this.dbConnection.GetErrorContact(Definition.ErrorContact.RejectVerify, ref flag2);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.BinFull, ref flag3);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.HighLevelSensors, ref flag4);
			this.dbConnection.GetErrorContact(Definition.ErrorContact.BinMissing, ref flag5);
			this.parHandling.WriteValue(Definition.noBeltStartOnErrorPath, flag ? "1" : "0");
			this.parHandling.WriteValue(Definition.beltStopOnVerifyErrorEnablePath, (flag && flag2) ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.beltStopOnBinFullErrorEnablePath, (flag && flag3) ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.BeltStopOnInOutErrorEnablePath, (flag && flag4) ? "1" : "0", false);
			this.parHandling.WriteValue(Definition.BeltStopOnBinMissingEnablePath, (flag && flag5) ? "1" : "0", false);
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00073F4C File Offset: 0x0007214C
		private void setLastXRayOnTimeIfPossible()
		{
			DateTime dateTime = DateTime.Now;
			string text = "";
			List<string> list = new List<string>();
			string text2 = "SwitchOnCycles_" + this.parHandling.ReadPar("/hardware/xraysource/SerialNumber/Value");
			text2 = text2.Replace("-", "");
			text2 += "S17716B00081";
			this.dBDataLogs.XRaySwitchOnGetEntries(text2, ref list);
			int count = list.Count;
			bool flag = count > 0;
			if (flag)
			{
				dateTime = Convert.ToDateTime(list[count - 1], new CultureInfo("de-DE"));
				text = string.Concat(new string[]
				{
					Convert.ToString(dateTime.Day),
					".",
					Convert.ToString(dateTime.Month),
					".",
					Convert.ToString(dateTime.Year)
				});
			}
			bool flag2 = text != "";
			if (flag2)
			{
				this.parHandling.WriteValue(MachineState.lastXRayOnPath, text);
			}
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0007405C File Offset: 0x0007225C
		private void WriteMachineInfos(string machinetype, string machinesubtype, string tunnelsize)
		{
			string text = "";
			string value = "";
			string value2 = "";
			bool flag = machinetype == "Raycon EX1";
			if (flag)
			{
				text = "";
			}
			else
			{
				bool flag2 = machinetype == "Raycon D+ MX";
				if (flag2)
				{
					text = "MX ";
					bool flag3 = machinesubtype != "Standard";
					if (flag3)
					{
						text += machinesubtype;
					}
				}
				else
				{
					bool flag4 = machinetype == "Raycon D+ HX";
					if (flag4)
					{
						text = "HX ";
						bool flag5 = machinesubtype != "Standard";
						if (flag5)
						{
							text += machinesubtype;
						}
					}
					else
					{
						bool flag6 = machinetype == "Raycon D+ TX";
						if (flag6)
						{
							text = "TX ";
						}
					}
				}
			}
			bool flag7 = tunnelsize != "Other";
			if (flag7)
			{
				string[] array = tunnelsize.Split(new char[]
				{
					'/'
				});
				bool flag8 = array.Length == 2;
				if (flag8)
				{
					value = array[1];
					value2 = array[0];
				}
			}
			else
			{
				value = "0";
				value2 = "0";
			}
			this.parHandling.WriteValue(Definition.machineSubTypePath, text);
			this.parHandling.WriteValue(Definition.inspectionHeightPath, value);
			this.parHandling.WriteValue(Definition.inspectionWidthPath, value2);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x000741A0 File Offset: 0x000723A0
		private void WriteParmamsToIPC(List<string> paths, List<string> values)
		{
			bool flag = paths.Count > 0 && values.Count > 0 && paths.Count == values.Count;
			if (flag)
			{
				for (int i = 0; i < paths.Count; i++)
				{
					string text = paths[i] + "/Value";
					this.parHandling.WriteValue(text, values[i]);
					bool flag2 = text.Contains("defaultProduct");
					if (flag2)
					{
						bool flag3 = paths[i].Contains("subTable");
						if (flag3)
						{
							text = paths[i].Substring(24);
						}
						else
						{
							text = text.Substring(25);
						}
						this.parHandling.WriteValue(text, values[i]);
					}
				}
			}
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x00074280 File Offset: 0x00072480
		private void ShowOverview(object obj)
		{
			CtrlComissioningOverview ctrlComissioningOverview = new CtrlComissioningOverview(this.dbConnection, this.parHandling);
			Subwindow.Show(ctrlComissioningOverview, SubwindowButtons.Back, ctrlComissioningOverview.PanelSubButtons);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x000742B0 File Offset: 0x000724B0
		private void ShowMultilaneParams(object obj)
		{
			CtrlComissioningMultilane ctrlComissioningMultilane = new CtrlComissioningMultilane(this.dbConnection, this.parHandling);
			Subwindow.Show(ctrlComissioningMultilane, SubwindowButtons.Back, ctrlComissioningMultilane.PanelSubButtons);
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x000742E0 File Offset: 0x000724E0
		private void ShowObjectBasedFlaps(object obj)
		{
			CtrlComissioningObjectBasedFlaps ctrlComissioningObjectBasedFlaps = new CtrlComissioningObjectBasedFlaps(this.dbConnection, this.parHandling);
			Subwindow.Show(ctrlComissioningObjectBasedFlaps, SubwindowButtons.Back, ctrlComissioningObjectBasedFlaps.PanelSubButtons);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00074310 File Offset: 0x00072510
		private void ShowFeaturesActivation(object obj)
		{
			CtrlComissioningFeatures ctrlComissioningFeatures = new CtrlComissioningFeatures(this.dbConnection, this.parHandling);
			Subwindow.Show(ctrlComissioningFeatures, SubwindowButtons.Back, ctrlComissioningFeatures.PanelSubButtons);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00074340 File Offset: 0x00072540
		public Visibility getOverviewVisibility()
		{
			bool flag = this.firstSimulation;
			Visibility result;
			if (flag)
			{
				result = Visibility.Collapsed;
			}
			else
			{
				result = Visibility.Visible;
			}
			return result;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00074364 File Offset: 0x00072564
		public Visibility getMultilaneVisibility()
		{
			bool flag = false;
			this.dbConnection.GetMultiLaneView(ref flag);
			string a = "";
			DetectorType detectorType = DetectorType.DT;
			string text = "/hardware/detector/dt/CommError/Value";
			string text2 = "/hardware/detector/sesotec/CommError/Value";
			Visibility result = Visibility.Visible;
			bool detectorType2 = this.dbConnection.GetDetectorType(ref detectorType);
			if (detectorType2)
			{
				string path = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? text : text2;
				a = this.parHandling.ReadPar(path);
			}
			bool flag2 = !flag || a == "1";
			if (flag2)
			{
				result = Visibility.Collapsed;
			}
			return result;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000743FC File Offset: 0x000725FC
		private void activateTestcardSim(bool enable)
		{
			bool flag = !(this.SelectedSubMachineType == "Bulk") && !(this.SelectedSubMachineType == "Bulk Hy");
			if (flag)
			{
				this.writeToActProdAndDefault(Definition.simulationAfterTimeIntervall, "0");
				if (enable)
				{
					this.writeToActProdAndDefault(Definition.testcardSimulationPath, "1");
					this.writeToActProdAndDefault(Definition.simulationAfterAutolearn, "1");
					this.parHandling.WriteValue(Autotrain.nAutolearnPath, "10");
				}
				else
				{
					this.writeToActProdAndDefault(Definition.testcardSimulationPath, "0");
					this.writeToActProdAndDefault(Definition.simulationAfterAutolearn, "0");
					this.parHandling.WriteValue(Autotrain.nAutolearnPath, "5");
				}
			}
			else
			{
				this.writeToActProdAndDefault(Definition.testcardSimulationPath, "0");
				this.writeToActProdAndDefault(Definition.simulationAfterAutolearn, "0");
			}
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x000744ED File Offset: 0x000726ED
		private void activateAdvancedFilter(bool enable)
		{
			this.parHandling.WriteValue(Definition.advancedFiltersPath, enable ? "1" : "0");
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00074510 File Offset: 0x00072710
		private void writeToActProdAndDefault(string path, string val)
		{
			this.parHandling.WriteValue(path, val);
			this.parHandling.WriteValue("/defaultProduct/mainTable" + path, val);
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x0007453C File Offset: 0x0007273C
		private void ResetAuditCheck(object obj)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtWarningResetAuditCheck"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.dbConnection.DeleteAuditCheckTable();
				this.DeleteDocuments();
				this.dbConnection.SetAuditCheckDocumentation(true);
				this.dbConnection.SetAuditCheckData("");
				this.dbConnection.SetAuditCheckProductChange(false);
				this.dbConnection.SetNAuditChecksFailed(0);
				this.dbConnection.SetNAuditChecksPassed(0);
				this.dbConnection.SetLastAuditCheckResult(AuditResult.Passed);
				this.dbConnection.SetLastDateDocumentation("01.01.2999");
				this.dbConnection.SetNextDateDocumentation("01.01.2999");
				this.dbConnection.SetRejectAllAuditCheck(false);
				this.dbConnection.UpdateCurrentProductTable(this.auditWeightEnabledPath, "0");
				this.dbConnection.UpdateCurrentProductTable(this.auditCountEnabledPath, "0");
				this.dbConnection.UpdateCurrentProductTable(this.auditContaminationEnabledPath, "0");
				this.dbConnection.UpdateCurrentProductTable(ComissioningViewModel.nProductsPath, "3");
				this.dbConnection.SetTimeToStartAudit("10");
				this.dbConnection.SetAuditFailedError("1");
				this.dbConnection.SetDocIsDaily(true);
				this.dbConnection.SetDocIsWeekly(false);
				this.dbConnection.SetDocIntervallDays(1);
				this.dbConnection.SetDocIsManually(false);
				this.dbConnection.SetAuditCheckEntries(0);
				this.dbConnection.UpdateCurrentProductTable("AuditCheckAlarmMode", "PerHour");
				this.dbConnection.UpdateCurrentProductTable("AuditCheckDay", "Monday");
				this.dbConnection.UpdateCurrentProductTable("AuditCheckIntervalHour", "0");
				this.dbConnection.UpdateCurrentProductTable("AuditCheckIntervalMinute", "0");
				this.dbConnection.UpdateCurrentProductTable("AuditCheckHour", "0");
				this.dbConnection.UpdateCurrentProductTable("AuditCheckMinute", "0");
				string s = "";
				this.dbConnection.GetTimeToStartAudit(ref s);
				try
				{
					this.auditSettings.AuditRequired.TimeToStartAudit = int.Parse(s);
				}
				catch
				{
				}
				this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath1, "None");
				this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath2, "None");
				this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath3, "None");
				this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath4, "None");
				this.dbConnection.UpdateCurrentProductTable(this.TestcardMaterialPath5, "None");
				this.auditSettings.GetCurrentValues();
			}
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0007480C File Offset: 0x00072A0C
		private void DeleteDocuments()
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\AuditcheckDocumentation";
			bool flag = Directory.Exists(path);
			if (flag)
			{
				string[] directories = Directory.GetDirectories(path);
				for (int i = 0; i < directories.Length; i++)
				{
					string[] files = Directory.GetFiles(directories[i]);
					for (int j = 0; j < files.Length; j++)
					{
						File.Delete(files[j]);
					}
				}
			}
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x00074890 File Offset: 0x00072A90
		private void setComp19to30dependingOnImgProcPC()
		{
			DetectorType detectorType = DetectorType.DT;
			this.dbConnection.GetDetectorType(ref detectorType);
			bool flag = this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1" && detectorType != DetectorType.DTNetTDI;
			string str = "";
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer19/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer19/Radius/Value", Convert.ToString(flag ? 11 : 2));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer19/MinSize/Value", Convert.ToString(flag ? 10 : 2));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer19/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer20/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer20/Radius/Value", Convert.ToString(flag ? 11 : 2));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer20/MinSize/Value", Convert.ToString(flag ? 50 : 3));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer20/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer21/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer21/Radius/Value", Convert.ToString(flag ? 11 : 3));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer21/MinSize/Value", Convert.ToString(flag ? 75 : 1));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer21/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer22/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer22/Radius/Value", Convert.ToString(flag ? 11 : 3));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer22/MinSize/Value", Convert.ToString(flag ? 95 : 2));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer22/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer23/Matches/Value", Convert.ToString(flag ? 7 : 5));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer23/Radius/Value", Convert.ToString(flag ? 11 : 2));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer23/MinSize/Value", Convert.ToString(flag ? 100 : 1));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer23/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer24/Matches/Value", Convert.ToString(flag ? 7 : 5));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer24/Radius/Value", Convert.ToString(flag ? 11 : 2));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer24/MinSize/Value", Convert.ToString(flag ? 105 : 2));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer24/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer25/Matches/Value", Convert.ToString(flag ? 7 : 5));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer25/Radius/Value", Convert.ToString(flag ? 11 : 3));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer25/MinSize/Value", Convert.ToString(flag ? 110 : 1));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer25/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer26/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer26/Radius/Value", Convert.ToString(flag ? 11 : 3));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer26/MinSize/Value", Convert.ToString(flag ? 125 : 4));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer26/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer27/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer27/Radius/Value", Convert.ToString(flag ? 11 : 3));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer27/MinSize/Value", Convert.ToString(flag ? 130 : 6));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer27/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer28/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer28/Radius/Value", Convert.ToString(flag ? 11 : 3));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer28/MinSize/Value", Convert.ToString(flag ? 135 : 8));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer28/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer29/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer29/Radius/Value", Convert.ToString(flag ? 11 : 4));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer29/MinSize/Value", Convert.ToString(flag ? 140 : 4));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer29/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer30/Matches/Value", Convert.ToString(flag ? 7 : 7));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer30/Radius/Value", Convert.ToString(flag ? 11 : 4));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer30/MinSize/Value", Convert.ToString(flag ? 200 : 6));
			this.writeToActProdAndDefault(Definition.filterMainPath + str + "/comparer/comparer30/MaxSize/Value", Convert.ToString(flag ? 50 : 0));
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x00074F9C File Offset: 0x0007319C
		private void setAIParameters()
		{
			bool flag = this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1";
			if (flag)
			{
				string text = "";
				this.dbConnection.GetMachineSubType(ref text);
				bool flag2 = text.Contains("Hy");
				bool flag3 = text.Contains("TX");
				string str = Definition.filterMainPath + Definition.anomalyDetectionMain + Definition.anomalyDetectionInference + Definition.aipostproFilterSubPath;
				this.writeToActProdAndDefault(str + "3/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 11 : 16));
				this.writeToActProdAndDefault(str + "3/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 15 : 20));
				this.writeToActProdAndDefault(str + "3/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 11 : 16));
				this.writeToActProdAndDefault(str + "3/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 15 : 20));
				this.writeToActProdAndDefault(str + "4/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 16 : 31));
				this.writeToActProdAndDefault(str + "4/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 20 : 35));
				this.writeToActProdAndDefault(str + "4/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 16 : 31));
				this.writeToActProdAndDefault(str + "4/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 20 : 35));
				this.writeToActProdAndDefault(str + "5/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 21 : 46));
				this.writeToActProdAndDefault(str + "5/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 25 : 50));
				this.writeToActProdAndDefault(str + "5/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 21 : 46));
				this.writeToActProdAndDefault(str + "5/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 25 : 50));
				this.writeToActProdAndDefault(str + "6/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 26 : 56));
				this.writeToActProdAndDefault(str + "6/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 30 : 60));
				this.writeToActProdAndDefault(str + "6/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 26 : 56));
				this.writeToActProdAndDefault(str + "6/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 30 : 60));
				this.writeToActProdAndDefault(str + "7/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 41 : 66));
				this.writeToActProdAndDefault(str + "7/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 45 : 70));
				this.writeToActProdAndDefault(str + "7/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 41 : 66));
				this.writeToActProdAndDefault(str + "7/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 45 : 70));
				this.writeToActProdAndDefault(str + "8/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 46 : 76));
				this.writeToActProdAndDefault(str + "8/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 50 : 80));
				this.writeToActProdAndDefault(str + "8/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 46 : 76));
				this.writeToActProdAndDefault(str + "8/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 50 : 80));
				this.writeToActProdAndDefault(str + "9/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 76 : 91));
				this.writeToActProdAndDefault(str + "9/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 80 : 95));
				this.writeToActProdAndDefault(str + "9/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 76 : 91));
				this.writeToActProdAndDefault(str + "9/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 80 : 95));
				this.writeToActProdAndDefault(str + "10/MinGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 91 : 116));
				this.writeToActProdAndDefault(str + "10/MaxGroupSizeInside/" + Definition.strValue, Convert.ToString(flag2 ? 95 : 120));
				this.writeToActProdAndDefault(str + "10/MinGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 91 : 116));
				this.writeToActProdAndDefault(str + "10/MaxGroupSizeContour/" + Definition.strValue, Convert.ToString(flag2 ? 95 : 120));
				this.writeToActProdAndDefault(Definition.anomalyDetectionTrainEncoders, Convert.ToString(flag3 ? 1 : 3));
				this.writeToActProdAndDefault(Definition.anomalyDetectionTrainEpochs, Convert.ToString(flag3 ? 100 : 35));
				this.writeToActProdAndDefault(CtrlAcquisitionSettingsViewModel.addedBackLinesPath, Convert.ToString(flag2 ? 40 : 20));
				this.writeToActProdAndDefault(CtrlAcquisitionSettingsViewModel.addedFrontLinesPath, Convert.ToString(flag2 ? 40 : 20));
				this.writeToActProdAndDefault(CtrlAcquisitionSettingsViewModel.addedLeftPixelPath, Convert.ToString(flag2 ? 40 : 20));
				this.writeToActProdAndDefault(CtrlAcquisitionSettingsViewModel.addedRightPixelPath, Convert.ToString(flag2 ? 40 : 20));
			}
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00075570 File Offset: 0x00073770
		private void setObjectBasedReject()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			list.Add(Definition.objectBasedFlapsEnableValvesPath);
			list.Add(Definition.objectBasedFlapsEjectTimePath);
			list.Add(Definition.objectBasedFlapsDelayTimePath);
			list2.Add("0");
			list2.Add("100");
			list2.Add("100");
			this.dbConnection.UpdateCurrentProductTable(list, list2);
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x000755E0 File Offset: 0x000737E0
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrMachineType);
			MainWindow.localizationManager.Deregister(this.StrMachineSubType);
			MainWindow.localizationManager.Deregister(this.StrTunnel);
			MainWindow.localizationManager.Deregister(this.StrDetector);
			MainWindow.localizationManager.Deregister(this.StrSensors);
			MainWindow.localizationManager.Deregister(this.StrFlapsCount);
			MainWindow.localizationManager.Deregister(this.StrIOsCount);
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00075668 File Offset: 0x00073868
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
			this.MachineTypeList.Clear();
			this.MachineTypeList = null;
			this.SubMachineTypeList.Clear();
			this.SubMachineTypeList = null;
			this.TunnelSizeList.Clear();
			this.TunnelSizeList = null;
			this.SensorList.Clear();
			this.SensorList = null;
			this.DetectorList.Clear();
			this.DetectorList = null;
			this.FlapsCountList.Clear();
			this.FlapsCountList = null;
			this.IOModuleCountList.Clear();
			this.IOModuleCountList = null;
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x0007570C File Offset: 0x0007390C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x0007573C File Offset: 0x0007393C
		private void UpdateDataLogsMachineInfos()
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			string str = "/Value";
			string text = "/machine/MachineType";
			list.Add(text);
			list2.Add(this.valuesToStringMachineInfos("MachineType", this.parHandling.ReadPar(text + str)));
			text = "/machine/MachineSubType";
			list.Add(text);
			list2.Add(this.parHandling.ReadPar(text + str));
			text = "/hardware/detector/Type";
			list.Add(text);
			list2.Add(this.valuesToStringMachineInfos("DetType", this.parHandling.ReadPar(text + str)));
			text = "/hardware/detector/sesotec/PixelSize";
			list.Add(text);
			list2.Add(this.parHandling.ReadPar(text + str));
			this.dBDataLogs.MachineInfosAddEntries(list, list2);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00075824 File Offset: 0x00073A24
		private string valuesToStringMachineInfos(string parameter, string value)
		{
			string result = "";
			bool flag = parameter == "MachineType";
			if (flag)
			{
				bool flag2 = value == "0";
				if (flag2)
				{
					result = "Raycon XRS";
				}
				else
				{
					bool flag3 = value == "1";
					if (flag3)
					{
						result = "Raycon D";
					}
					else
					{
						bool flag4 = value == "2";
						if (flag4)
						{
							result = "Raycon Entry";
						}
						else
						{
							bool flag5 = value == "3";
							if (flag5)
							{
								result = "Raycon D+";
							}
							else
							{
								result = value;
							}
						}
					}
				}
			}
			else
			{
				bool flag6 = parameter == "DetType";
				if (flag6)
				{
					bool flag7 = value == "0";
					if (flag7)
					{
						result = "DT";
					}
					else
					{
						bool flag8 = value == "1";
						if (flag8)
						{
							result = "Sesotec";
						}
						else
						{
							bool flag9 = value == "2";
							if (flag9)
							{
								result = "DT Dual Energy";
							}
							else
							{
								bool flag10 = value == "3";
								if (flag10)
								{
									result = "DT";
								}
								else
								{
									bool flag11 = value == "4";
									if (flag11)
									{
										result = "DTNetTDI";
									}
									else
									{
										result = value;
									}
								}
							}
						}
					}
				}
				else
				{
					bool flag12 = parameter == "DetSubType";
					if (flag12)
					{
						bool flag13 = value == "0";
						if (flag13)
						{
							result = "Standard";
						}
						else
						{
							bool flag14 = value == "1";
							if (flag14)
							{
								result = "HS";
							}
							else
							{
								result = value;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00075998 File Offset: 0x00073B98
		private void CheckForObjectBasedFlapsToBeVisible()
		{
			bool flag = false;
			this.dbConnection.GetBulkObjectEnabled(ref flag);
			bool flag2 = this.SelectedSubMachineType == "Bulk" || this.SelectedSubMachineType == "Bulk Hy";
			if (flag2)
			{
				this.ObjectBasedFlapsEnabled = false;
				this.ObjectBasedFlapsVisible = false;
			}
			else
			{
				this.ObjectBasedFlapsVisible = true;
			}
		}

		// Token: 0x04000CA0 RID: 3232
		private const string TypeRaycon_EX1 = "Raycon EX1";

		// Token: 0x04000CA1 RID: 3233
		private const string TypeRaycon_EX2A = "Raycon EX2A";

		// Token: 0x04000CA2 RID: 3234
		private const string TypeRaycon_DPlus_MX = "Raycon D+ MX";

		// Token: 0x04000CA3 RID: 3235
		private const string TypeRaycon_DPlus_HX = "Raycon D+ HX";

		// Token: 0x04000CA4 RID: 3236
		private const string TypeRaycon_DPlus_TX = "Raycon D+ TX";

		// Token: 0x04000CA5 RID: 3237
		private const string SubTypeStandard = "Standard";

		// Token: 0x04000CA6 RID: 3238
		private const string SubTypeLW = "LW";

		// Token: 0x04000CA7 RID: 3239
		private const string SubTypeBulk = "Bulk";

		// Token: 0x04000CA8 RID: 3240
		private const string SubTypeLWHy = "LW Hy";

		// Token: 0x04000CA9 RID: 3241
		private const string SubTypeBulkHy = "Bulk Hy";

		// Token: 0x04000CAA RID: 3242
		private const string TunnelSizeTwoHundredOneTwenty = "200/120";

		// Token: 0x04000CAB RID: 3243
		private const string TunnelSizeThreeThirtyTwoHundred = "330/200";

		// Token: 0x04000CAC RID: 3244
		private const string TunnelSizeForFiftyTwoFifty = "450/250";

		// Token: 0x04000CAD RID: 3245
		private const string TunnelSizeSixHundredOneTwenty = "600/120";

		// Token: 0x04000CAE RID: 3246
		private const string TunnelSizeThreeThirtyFifty = "330/50";

		// Token: 0x04000CAF RID: 3247
		private const string TunnelSizeSixThirtyFifty = "630/50";

		// Token: 0x04000CB0 RID: 3248
		private const string TunnelSizeThreeHundredOneHundredFiftey = "300/150";

		// Token: 0x04000CB1 RID: 3249
		private const string TunnelSizeFourHundredTwoHundredFiftey = "400/250";

		// Token: 0x04000CB2 RID: 3250
		private const string TunnelSizeOther = "Other";

		// Token: 0x04000CB3 RID: 3251
		private const string DetectorZeroPointFour = "0.4 mm";

		// Token: 0x04000CB4 RID: 3252
		private const string DetectorZeroPointEight = "0.8 mm";

		// Token: 0x04000CB5 RID: 3253
		private const string DetectorDualEnergy = "Dual Energy";

		// Token: 0x04000CB6 RID: 3254
		private const string FlapsCountOne = "1";

		// Token: 0x04000CB7 RID: 3255
		private const string FlapsCountFour = "4";

		// Token: 0x04000CB8 RID: 3256
		private const string FlapsCountTwenty = "20";

		// Token: 0x04000CB9 RID: 3257
		private const string FlapsCountTen = "10";

		// Token: 0x04000CBA RID: 3258
		private const string IOsCountOne = "1";

		// Token: 0x04000CBB RID: 3259
		private const string IOsCountTwo = "2";

		// Token: 0x04000CBC RID: 3260
		private const string SensorsStandard = "Standard";

		// Token: 0x04000CBD RID: 3261
		private const string SensorsHighlevel = "HighLevel";

		// Token: 0x04000CBE RID: 3262
		private const string SensorsAdvanced = "Advanced";

		// Token: 0x04000CBF RID: 3263
		private DBConnection dbConnection;

		// Token: 0x04000CC0 RID: 3264
		private ParameterHandling parHandling;

		// Token: 0x04000CC1 RID: 3265
		private DBDataLogs dBDataLogs;

		// Token: 0x04000CC2 RID: 3266
		public Button btnStartWritingParameter;

		// Token: 0x04000CC3 RID: 3267
		public Button btnComissioningMultilane;

		// Token: 0x04000CC4 RID: 3268
		public Button btnComissioningFeaturesLicense;

		// Token: 0x04000CC5 RID: 3269
		public Button btnOverview;

		// Token: 0x04000CC6 RID: 3270
		public Button btnAuditCheck;

		// Token: 0x04000CDB RID: 3291
		private List<string> flapsCountList = new List<string>();

		// Token: 0x04000CDD RID: 3293
		private bool svtEnabled = false;

		// Token: 0x04000CDE RID: 3294
		private bool firstInit;

		// Token: 0x04000CDF RID: 3295
		private bool firstSimulation;

		// Token: 0x04000CE0 RID: 3296
		private string selectedMachineType;

		// Token: 0x04000CE1 RID: 3297
		private string selectedSubMachineType;

		// Token: 0x04000CE2 RID: 3298
		private string selectedTunnelSize;

		// Token: 0x04000CE3 RID: 3299
		private string selectedDetector;

		// Token: 0x04000CE4 RID: 3300
		private string selectedSensors;

		// Token: 0x04000CE5 RID: 3301
		private string selectedFlapsCount;

		// Token: 0x04000CE6 RID: 3302
		private string selectedIOsCount;

		// Token: 0x04000CE7 RID: 3303
		private bool isSubTypeVisible;

		// Token: 0x04000CE8 RID: 3304
		private bool isDetectorVisible;

		// Token: 0x04000CE9 RID: 3305
		private bool isSensorsVisible;

		// Token: 0x04000CEA RID: 3306
		private bool isFlapsCountVisible;

		// Token: 0x04000CEB RID: 3307
		private bool isIOModuleCountVisible;

		// Token: 0x04000CEC RID: 3308
		private bool objectBasedFlapsVisible;

		// Token: 0x04000CED RID: 3309
		private bool objectBasedFlapsEnabled;

		// Token: 0x04000CEE RID: 3310
		private readonly string TestcardMaterialPath1 = "TestcardMaterial1";

		// Token: 0x04000CEF RID: 3311
		private readonly string TestcardMaterialPath2 = "TestcardMaterial2";

		// Token: 0x04000CF0 RID: 3312
		private readonly string TestcardMaterialPath3 = "TestcardMaterial3";

		// Token: 0x04000CF1 RID: 3313
		private readonly string TestcardMaterialPath4 = "TestcardMaterial4";

		// Token: 0x04000CF2 RID: 3314
		private readonly string TestcardMaterialPath5 = "TestcardMaterial5";

		// Token: 0x04000CF3 RID: 3315
		private readonly string auditContaminationEnabledPath = "AuditContaminationEnabled";

		// Token: 0x04000CF4 RID: 3316
		private readonly string auditWeightEnabledPath = "AuditWeightEnabled";

		// Token: 0x04000CF5 RID: 3317
		private readonly string auditCountEnabledPath = "AuditCountEnabled";

		// Token: 0x04000CF6 RID: 3318
		public static readonly string nProductsPath = "AuditCheckNProducts";

		// Token: 0x04000CF7 RID: 3319
		private AuditCheckSettings auditSettings;
	}
}

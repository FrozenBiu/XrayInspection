using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x0200014C RID: 332
	public class Rejecter : INotifyPropertyChanged
	{
		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06001F30 RID: 7984 RVA: 0x000CF5B3 File Offset: 0x000CD7B3
		// (set) Token: 0x06001F31 RID: 7985 RVA: 0x000CF5BB File Offset: 0x000CD7BB
		public ObservableCollection<RejecterItem> UnlockBinItems { get; set; }

		// Token: 0x140000A6 RID: 166
		// (add) Token: 0x06001F32 RID: 7986 RVA: 0x000CF5C4 File Offset: 0x000CD7C4
		// (remove) Token: 0x06001F33 RID: 7987 RVA: 0x000CF5FC File Offset: 0x000CD7FC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06001F34 RID: 7988 RVA: 0x000CF634 File Offset: 0x000CD834
		// (set) Token: 0x06001F35 RID: 7989 RVA: 0x000CF64C File Offset: 0x000CD84C
		public bool ObjectBasedFlapsVisible
		{
			get
			{
				return this.objectBasedFlapsVisible;
			}
			set
			{
				bool flag = this.objectBasedFlapsVisible != value;
				if (flag)
				{
					this.objectBasedFlapsVisible = value;
				}
				this.OnPropertyChanged("ObjectBasedFlapsVisible");
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06001F36 RID: 7990 RVA: 0x000CF680 File Offset: 0x000CD880
		// (set) Token: 0x06001F37 RID: 7991 RVA: 0x000CF698 File Offset: 0x000CD898
		public int Delay
		{
			get
			{
				return this.delay;
			}
			set
			{
				bool flag = this.delay != value;
				if (flag)
				{
					this.delay = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/Delay/Value", this.delay.ToString());
					this.OnPropertyChanged("Delay");
				}
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06001F38 RID: 7992 RVA: 0x000CF6F4 File Offset: 0x000CD8F4
		// (set) Token: 0x06001F39 RID: 7993 RVA: 0x000CF70C File Offset: 0x000CD90C
		public int EjectTime
		{
			get
			{
				return this.ejectTime;
			}
			set
			{
				bool flag = this.ejectTime != value;
				if (flag)
				{
					this.ejectTime = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/Length/Value", this.ejectTime.ToString());
					this.OnPropertyChanged("EjectTime");
				}
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06001F3A RID: 7994 RVA: 0x000CF768 File Offset: 0x000CD968
		// (set) Token: 0x06001F3B RID: 7995 RVA: 0x000CF780 File Offset: 0x000CD980
		public bool Enable
		{
			get
			{
				return this.enable;
			}
			set
			{
				bool flag = this.enable != value;
				if (flag)
				{
					this.enable = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/Enabled/Value", this.enable ? "1" : "0");
					this.OnPropertyChanged("Enable");
				}
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x000CF7E4 File Offset: 0x000CD9E4
		// (set) Token: 0x06001F3D RID: 7997 RVA: 0x000CF7FC File Offset: 0x000CD9FC
		public bool VerifyEnable
		{
			get
			{
				return this.verifyEnable;
			}
			set
			{
				bool flag = this.verifyEnable != value;
				if (flag)
				{
					this.verifyEnable = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/VerifyEnable/Value", this.verifyEnable ? "1" : "0");
					this.OnPropertyChanged("VerifyEnable");
				}
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x000CF860 File Offset: 0x000CDA60
		// (set) Token: 0x06001F3F RID: 7999 RVA: 0x000CF878 File Offset: 0x000CDA78
		public int Output
		{
			get
			{
				return this.output;
			}
			set
			{
				bool flag = this.output != value;
				if (flag)
				{
					this.output = value;
					this.OnPropertyChanged("Output");
				}
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06001F40 RID: 8000 RVA: 0x000CF8AC File Offset: 0x000CDAAC
		// (set) Token: 0x06001F41 RID: 8001 RVA: 0x000CF8C4 File Offset: 0x000CDAC4
		public RejectPriority Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				bool flag = this.priority != value;
				if (flag)
				{
					this.priority = value;
					ParameterHandling parameterHandling = this.parHandling;
					string path = this.rejecterMainPath + "/Priority/Value";
					int num = (int)this.priority;
					parameterHandling.WriteValue(path, num.ToString());
					this.OnPropertyChanged("Priority");
				}
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06001F42 RID: 8002 RVA: 0x000CF924 File Offset: 0x000CDB24
		// (set) Token: 0x06001F43 RID: 8003 RVA: 0x000CF93C File Offset: 0x000CDB3C
		public RejectMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				bool flag = this.mode != value;
				if (flag)
				{
					List<string> list = new List<string>();
					this.mode = value;
					ParameterHandling parameterHandling = this.parHandling;
					string path = this.rejecterMainPath + "/Mode/Value";
					int num = (int)this.mode;
					parameterHandling.WriteValue(path, num.ToString(), false);
					this.dbConnection.GetMainProductTableNames(list);
					foreach (string text in list)
					{
						DBConnection dbconnection = this.dbConnection;
						string tableName = text;
						string path2 = this.rejecterMainPath + "/Mode/Value";
						num = (int)this.mode;
						dbconnection.UpdateTable(tableName, path2, num.ToString());
					}
					this.OnPropertyChanged("Mode");
				}
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x000CFA1C File Offset: 0x000CDC1C
		// (set) Token: 0x06001F45 RID: 8005 RVA: 0x000CFA24 File Offset: 0x000CDC24
		public List<RejectMode> Modes { get; set; }

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x000CFA30 File Offset: 0x000CDC30
		// (set) Token: 0x06001F47 RID: 8007 RVA: 0x000CFA48 File Offset: 0x000CDC48
		public Lane Lane
		{
			get
			{
				return this.lane;
			}
			set
			{
				bool flag = this.lane != value;
				if (flag)
				{
					this.lane = value;
					ParameterHandling parameterHandling = this.parHandling;
					string path = this.rejecterMainPath + "/Lane/Value";
					int num = (int)this.lane;
					parameterHandling.WriteValue(path, num.ToString());
					this.OnPropertyChanged("Lane");
				}
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x000CFAA5 File Offset: 0x000CDCA5
		// (set) Token: 0x06001F49 RID: 8009 RVA: 0x000CFAAD File Offset: 0x000CDCAD
		public List<Lane> Lanes { get; set; }

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06001F4A RID: 8010 RVA: 0x000CFAB8 File Offset: 0x000CDCB8
		// (set) Token: 0x06001F4B RID: 8011 RVA: 0x000CFAD0 File Offset: 0x000CDCD0
		public int RejecterIndex
		{
			get
			{
				return this.rejecterIndex;
			}
			set
			{
				this.rejecterIndex = value;
				this.OnPropertyChanged("RejecterIndex");
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x000CFAE8 File Offset: 0x000CDCE8
		// (set) Token: 0x06001F4D RID: 8013 RVA: 0x000CFB00 File Offset: 0x000CDD00
		public bool PriorityVisible
		{
			get
			{
				return this.priorityVisible;
			}
			set
			{
				bool flag = this.priorityVisible != value;
				if (flag)
				{
					this.priorityVisible = value;
					this.OnPropertyChanged("PriorityVisible");
				}
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x000CFB34 File Offset: 0x000CDD34
		// (set) Token: 0x06001F4F RID: 8015 RVA: 0x000CFB4C File Offset: 0x000CDD4C
		public bool LaneVisible
		{
			get
			{
				return this.laneVisible;
			}
			set
			{
				bool flag = this.laneVisible != value;
				if (flag)
				{
					this.laneVisible = value;
					this.OnPropertyChanged("LaneVisible");
				}
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000CFB80 File Offset: 0x000CDD80
		// (set) Token: 0x06001F51 RID: 8017 RVA: 0x000CFB98 File Offset: 0x000CDD98
		public bool ContaminationEnabled
		{
			get
			{
				return this.contaminationEnabled;
			}
			set
			{
				bool flag = this.contaminationEnabled != value;
				if (flag)
				{
					this.contaminationEnabled = value;
					this.SetRejectType();
					this.OnPropertyChanged("ContaminationEnabled");
				}
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x000CFBD4 File Offset: 0x000CDDD4
		// (set) Token: 0x06001F53 RID: 8019 RVA: 0x000CFBEC File Offset: 0x000CDDEC
		public bool OverweightEnabled
		{
			get
			{
				return this.overweightEnabled;
			}
			set
			{
				bool flag = this.overweightEnabled != value;
				if (flag)
				{
					this.overweightEnabled = value;
					this.SetRejectType();
					this.OnPropertyChanged("OverweightEnabled");
				}
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06001F54 RID: 8020 RVA: 0x000CFC28 File Offset: 0x000CDE28
		// (set) Token: 0x06001F55 RID: 8021 RVA: 0x000CFC40 File Offset: 0x000CDE40
		public bool UnderweightEnabled
		{
			get
			{
				return this.underweightEnabled;
			}
			set
			{
				bool flag = this.underweightEnabled != value;
				if (flag)
				{
					this.underweightEnabled = value;
					this.SetRejectType();
					this.OnPropertyChanged("UnderweightEnabled");
				}
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06001F56 RID: 8022 RVA: 0x000CFC7C File Offset: 0x000CDE7C
		// (set) Token: 0x06001F57 RID: 8023 RVA: 0x000CFC94 File Offset: 0x000CDE94
		public bool OvercountEnabled
		{
			get
			{
				return this.overcountEnabled;
			}
			set
			{
				bool flag = this.overcountEnabled != value;
				if (flag)
				{
					this.overcountEnabled = value;
					this.SetRejectType();
					this.OnPropertyChanged("OvercountEnabled");
				}
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06001F58 RID: 8024 RVA: 0x000CFCD0 File Offset: 0x000CDED0
		// (set) Token: 0x06001F59 RID: 8025 RVA: 0x000CFCE8 File Offset: 0x000CDEE8
		public bool UndercountEnabled
		{
			get
			{
				return this.undercountEnabled;
			}
			set
			{
				bool flag = this.undercountEnabled != value;
				if (flag)
				{
					this.undercountEnabled = value;
					this.SetRejectType();
					this.OnPropertyChanged("UndercountEnabled");
				}
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06001F5A RID: 8026 RVA: 0x000CFD24 File Offset: 0x000CDF24
		// (set) Token: 0x06001F5B RID: 8027 RVA: 0x000CFD3C File Offset: 0x000CDF3C
		public bool OversizeEnabled
		{
			get
			{
				return this.oversizeEnabled;
			}
			set
			{
				bool flag = this.oversizeEnabled != value;
				if (flag)
				{
					this.oversizeEnabled = value;
					this.SetRejectType();
					this.OnPropertyChanged("OversizeEnabled");
				}
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06001F5C RID: 8028 RVA: 0x000CFD78 File Offset: 0x000CDF78
		// (set) Token: 0x06001F5D RID: 8029 RVA: 0x000CFD90 File Offset: 0x000CDF90
		public bool UndersizeEnabled
		{
			get
			{
				return this.undersizeEnabled;
			}
			set
			{
				bool flag = this.undersizeEnabled != value;
				if (flag)
				{
					this.undersizeEnabled = value;
					this.SetRejectType();
					this.OnPropertyChanged("UndersizeEnabled");
				}
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06001F5E RID: 8030 RVA: 0x000CFDCC File Offset: 0x000CDFCC
		// (set) Token: 0x06001F5F RID: 8031 RVA: 0x000CFDE4 File Offset: 0x000CDFE4
		public bool BeltStopOnContaminant
		{
			get
			{
				return this.beltStopOnContaminant;
			}
			set
			{
				bool flag = this.beltStopOnContaminant != value;
				if (flag)
				{
					this.beltStopOnContaminant = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/BeltStopOnContaminant/Value", this.beltStopOnContaminant ? "1" : "0");
					this.OnPropertyChanged("BeltStopOnContaminant");
				}
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06001F60 RID: 8032 RVA: 0x000CFE48 File Offset: 0x000CE048
		// (set) Token: 0x06001F61 RID: 8033 RVA: 0x000CFE60 File Offset: 0x000CE060
		public int BinFullTimeout
		{
			get
			{
				return this.binFullTimeout;
			}
			set
			{
				bool flag = this.binFullTimeout != value;
				if (flag)
				{
					int wagoProgramVersion = this.GetWagoProgramVersion();
					bool flag2 = wagoProgramVersion < Definition.minWagoVersionForUserProgramType;
					if (flag2)
					{
						value = ((value <= 65) ? value : 65);
						this.binFullTimeout = value;
						this.parHandling.WriteValue(this.rejecterMainPath + "/BinFullTimeout/Value", (this.binFullTimeout * 1000).ToString());
						this.OnPropertyChanged("BinFullTimeout");
					}
					else
					{
						value = ((value <= 65535) ? value : 65535);
						this.binFullTimeout = value;
						this.parHandling.WriteValue(this.rejecterMainPath + "/BinFullTimeout/Value", this.binFullTimeout.ToString());
						this.OnPropertyChanged("BinFullTimeout");
					}
				}
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06001F62 RID: 8034 RVA: 0x000CFF38 File Offset: 0x000CE138
		// (set) Token: 0x06001F63 RID: 8035 RVA: 0x000CFF50 File Offset: 0x000CE150
		public bool WeightVisible
		{
			get
			{
				return this.weightVisible;
			}
			set
			{
				bool flag = this.weightVisible != value;
				if (flag)
				{
					this.weightVisible = value;
					this.OnPropertyChanged("WeightVisible");
				}
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06001F64 RID: 8036 RVA: 0x000CFF84 File Offset: 0x000CE184
		// (set) Token: 0x06001F65 RID: 8037 RVA: 0x000CFF9C File Offset: 0x000CE19C
		public bool ObjectCountVisible
		{
			get
			{
				return this.objectCountVisible;
			}
			set
			{
				bool flag = this.objectCountVisible != value;
				if (flag)
				{
					this.objectCountVisible = value;
					this.OnPropertyChanged("ObjectCountVisible");
				}
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06001F66 RID: 8038 RVA: 0x000CFFD0 File Offset: 0x000CE1D0
		// (set) Token: 0x06001F67 RID: 8039 RVA: 0x000CFFE8 File Offset: 0x000CE1E8
		public bool Op1CompontentVisible
		{
			get
			{
				return this.op1CompontentVisible;
			}
			set
			{
				bool flag = this.op1CompontentVisible != value;
				if (flag)
				{
					this.op1CompontentVisible = value;
					this.OnPropertyChanged("Op1CompontentVisible");
				}
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06001F68 RID: 8040 RVA: 0x000D001C File Offset: 0x000CE21C
		// (set) Token: 0x06001F69 RID: 8041 RVA: 0x000D0034 File Offset: 0x000CE234
		public bool VerifyTestVisible
		{
			get
			{
				return this.verifyTestVisible;
			}
			set
			{
				bool flag = this.verifyTestVisible != value;
				if (flag)
				{
					this.verifyTestVisible = value;
					this.OnPropertyChanged("VerifyTestVisible");
				}
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06001F6A RID: 8042 RVA: 0x000D0068 File Offset: 0x000CE268
		// (set) Token: 0x06001F6B RID: 8043 RVA: 0x000D0080 File Offset: 0x000CE280
		public bool ImageSizeVisible
		{
			get
			{
				return this.imageSizeVisible;
			}
			set
			{
				bool flag = this.imageSizeVisible != value;
				if (flag)
				{
					this.imageSizeVisible = value;
					this.OnPropertyChanged("ImageSizeVisible");
				}
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06001F6C RID: 8044 RVA: 0x000D00B4 File Offset: 0x000CE2B4
		// (set) Token: 0x06001F6D RID: 8045 RVA: 0x000D00CC File Offset: 0x000CE2CC
		public int ImageGroup
		{
			get
			{
				return this.imageGroup;
			}
			set
			{
				bool flag = value >= 0 && value <= Definition.maxSubProducts && this.imageGroup != value;
				if (flag)
				{
					this.imageGroup = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/ImageGroup/Value", this.imageGroup.ToString());
					this.OnPropertyChanged("ImageGroup");
				}
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06001F6E RID: 8046 RVA: 0x000D0135 File Offset: 0x000CE335
		// (set) Token: 0x06001F6F RID: 8047 RVA: 0x000D013D File Offset: 0x000CE33D
		public List<int> ImageGroups { get; set; }

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06001F70 RID: 8048 RVA: 0x000D0148 File Offset: 0x000CE348
		// (set) Token: 0x06001F71 RID: 8049 RVA: 0x000D0160 File Offset: 0x000CE360
		public bool ImageGroupVisible
		{
			get
			{
				return this.imageGroupVisible;
			}
			set
			{
				bool flag = this.imageGroupVisible != value;
				if (flag)
				{
					this.imageGroupVisible = value;
					this.OnPropertyChanged("ImageGroupVisible");
				}
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06001F72 RID: 8050 RVA: 0x000D0194 File Offset: 0x000CE394
		// (set) Token: 0x06001F73 RID: 8051 RVA: 0x000D01AC File Offset: 0x000CE3AC
		public bool BeltStopOnContaminantVisible
		{
			get
			{
				return this.beltStopOnContaminantVisible;
			}
			set
			{
				bool flag = this.beltStopOnContaminantVisible != value;
				if (flag)
				{
					this.beltStopOnContaminantVisible = value;
					this.OnPropertyChanged("BeltStopOnContaminantVisible");
				}
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x000D01E0 File Offset: 0x000CE3E0
		// (set) Token: 0x06001F75 RID: 8053 RVA: 0x000D01F8 File Offset: 0x000CE3F8
		public bool BinFullTimeoutVisible
		{
			get
			{
				return this.binFullTimeoutVisible;
			}
			set
			{
				bool flag = this.binFullTimeoutVisible != value;
				if (flag)
				{
					this.binFullTimeoutVisible = value;
					this.OnPropertyChanged("BinFullTimeoutVisible");
				}
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06001F76 RID: 8054 RVA: 0x000D022C File Offset: 0x000CE42C
		// (set) Token: 0x06001F77 RID: 8055 RVA: 0x000D0244 File Offset: 0x000CE444
		public bool VerifyVisible
		{
			get
			{
				return this.verifyVisible;
			}
			set
			{
				bool flag = this.verifyVisible != value;
				if (flag)
				{
					this.verifyVisible = value;
					this.OnPropertyChanged("VerifyVisible");
				}
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06001F78 RID: 8056 RVA: 0x000D0278 File Offset: 0x000CE478
		// (set) Token: 0x06001F79 RID: 8057 RVA: 0x000D0290 File Offset: 0x000CE490
		public bool ModeVisible
		{
			get
			{
				return this.modeVisible;
			}
			set
			{
				bool flag = this.modeVisible != value;
				if (flag)
				{
					this.modeVisible = value;
					this.OnPropertyChanged("ModeVisible");
				}
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06001F7A RID: 8058 RVA: 0x000D02C4 File Offset: 0x000CE4C4
		// (set) Token: 0x06001F7B RID: 8059 RVA: 0x000D02DC File Offset: 0x000CE4DC
		public bool RightsOk
		{
			get
			{
				return this.rightsOk;
			}
			set
			{
				bool flag = this.rightsOk != value;
				if (flag)
				{
					this.rightsOk = value;
					this.OnPropertyChanged("RightsOk");
				}
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000D0310 File Offset: 0x000CE510
		// (set) Token: 0x06001F7D RID: 8061 RVA: 0x000D0328 File Offset: 0x000CE528
		public bool MSVisible
		{
			get
			{
				return this.msVisible;
			}
			set
			{
				bool flag = this.msVisible != value;
				if (flag)
				{
					this.msVisible = value;
					this.OnPropertyChanged("MSVisible");
				}
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06001F7E RID: 8062 RVA: 0x000D035C File Offset: 0x000CE55C
		// (set) Token: 0x06001F7F RID: 8063 RVA: 0x000D0374 File Offset: 0x000CE574
		public bool MSBinUnlockVisible
		{
			get
			{
				return this.msBinUnlockVisible;
			}
			set
			{
				bool flag = this.msBinUnlockVisible != value;
				if (flag)
				{
					this.msBinUnlockVisible = value;
					this.OnPropertyChanged("MSBinUnlockVisible");
				}
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06001F80 RID: 8064 RVA: 0x000D03A8 File Offset: 0x000CE5A8
		// (set) Token: 0x06001F81 RID: 8065 RVA: 0x000D03C0 File Offset: 0x000CE5C0
		public bool MSBinUnlockTimeoutVisible
		{
			get
			{
				return this.msBinUnlockTimeoutVisible;
			}
			set
			{
				bool flag = this.msBinUnlockTimeoutVisible != value;
				if (flag)
				{
					this.msBinUnlockTimeoutVisible = value;
					this.OnPropertyChanged("MSBinUnlockTimeoutVisible");
				}
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06001F82 RID: 8066 RVA: 0x000D03F4 File Offset: 0x000CE5F4
		// (set) Token: 0x06001F83 RID: 8067 RVA: 0x000D040C File Offset: 0x000CE60C
		public int BinUnlockTimeout
		{
			get
			{
				return this.binUnlockTimeout;
			}
			set
			{
				bool flag = this.binUnlockTimeout != value;
				if (flag)
				{
					this.binUnlockTimeout = value;
					string value2 = this.binUnlockTimeout.ToString();
					bool flag2 = this.iUserProgrammVersion < 26;
					if (flag2)
					{
						value2 = (this.binUnlockTimeout * 1000).ToString();
					}
					this.parHandling.WriteValue(this.rejecterMainPath + "/HLBinTimeout/Value", value2);
					this.OnPropertyChanged("BinUnlockTimeout");
				}
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06001F84 RID: 8068 RVA: 0x000D048C File Offset: 0x000CE68C
		// (set) Token: 0x06001F85 RID: 8069 RVA: 0x000D04A4 File Offset: 0x000CE6A4
		public bool HighLevelSensors
		{
			get
			{
				return this.highLevelSensors;
			}
			set
			{
				bool flag = this.highLevelSensors != value;
				if (flag)
				{
					this.highLevelSensors = value;
					this.OnPropertyChanged("HighLevelSensors");
				}
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06001F86 RID: 8070 RVA: 0x000D04D8 File Offset: 0x000CE6D8
		// (set) Token: 0x06001F87 RID: 8071 RVA: 0x000D04F0 File Offset: 0x000CE6F0
		public int DebounceTime
		{
			get
			{
				return this.debounceTime;
			}
			set
			{
				bool flag = this.debounceTime != value;
				if (flag)
				{
					this.debounceTime = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/HLDebounceTime/Value", this.debounceTime.ToString());
					this.OnPropertyChanged("DebounceTime");
				}
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06001F88 RID: 8072 RVA: 0x000D054C File Offset: 0x000CE74C
		// (set) Token: 0x06001F89 RID: 8073 RVA: 0x000D0564 File Offset: 0x000CE764
		public int CorrectionTime
		{
			get
			{
				return this.correctionTime;
			}
			set
			{
				bool flag = this.correctionTime != value;
				if (flag)
				{
					this.correctionTime = value;
					this.parHandling.WriteValue(this.rejecterMainPath + "/HLEFCorrectionTime/Value", this.correctionTime.ToString());
					this.OnPropertyChanged("CorrectionTime");
				}
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06001F8A RID: 8074 RVA: 0x000D05C0 File Offset: 0x000CE7C0
		// (set) Token: 0x06001F8B RID: 8075 RVA: 0x000D05D8 File Offset: 0x000CE7D8
		public bool DebounceTimeVisible
		{
			get
			{
				return this.debounceTimeVisible;
			}
			set
			{
				bool flag = this.debounceTimeVisible != value;
				if (flag)
				{
					this.debounceTimeVisible = value;
					this.OnPropertyChanged("DebounceTimeVisible");
				}
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x000D060C File Offset: 0x000CE80C
		// (set) Token: 0x06001F8D RID: 8077 RVA: 0x000D0624 File Offset: 0x000CE824
		public int RejectVerifySensorBlockedTimeout
		{
			get
			{
				return this.rejectVerifySensorBlockedTimeout;
			}
			set
			{
				bool flag = this.rejectVerifySensorBlockedTimeout != value;
				if (flag)
				{
					bool flag2 = value <= 65535;
					if (flag2)
					{
						this.rejectVerifySensorBlockedTimeout = value;
						this.parHandling.WriteValue(this.rejecterMainPath + "/LightBarrierBlockedTimeout/Value", this.rejectVerifySensorBlockedTimeout.ToString());
						this.OnPropertyChanged("RejectVerifySensorBlockedTimeout");
					}
				}
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06001F8E RID: 8078 RVA: 0x000D0690 File Offset: 0x000CE890
		// (set) Token: 0x06001F8F RID: 8079 RVA: 0x000D06A8 File Offset: 0x000CE8A8
		public bool RejectEnableAccessible
		{
			get
			{
				return this.rejectEnableAccessible;
			}
			set
			{
				bool flag = this.rejectEnableAccessible != value;
				if (flag)
				{
					this.rejectEnableAccessible = value;
					this.OnPropertyChanged("RejectEnableAccessible");
					this.OnPropertyChanged("RejectEnableNotAccessible");
				}
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x000D06E8 File Offset: 0x000CE8E8
		public bool RejectEnableNotAccessible
		{
			get
			{
				return !this.rejectEnableAccessible;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000D0703 File Offset: 0x000CE903
		// (set) Token: 0x06001F92 RID: 8082 RVA: 0x000D070B File Offset: 0x000CE90B
		public bool TimeoutRejectVerifySensorBlockedVisible { get; set; }

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06001F93 RID: 8083 RVA: 0x000D0714 File Offset: 0x000CE914
		// (set) Token: 0x06001F94 RID: 8084 RVA: 0x000D071C File Offset: 0x000CE91C
		public int TabHeight { get; set; }

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06001F95 RID: 8085 RVA: 0x000D0728 File Offset: 0x000CE928
		// (set) Token: 0x06001F96 RID: 8086 RVA: 0x000D0740 File Offset: 0x000CE940
		public bool AirPressureTestVisible
		{
			get
			{
				return this.airPressureTestVisible;
			}
			set
			{
				bool flag = this.airPressureTestVisible != value;
				if (flag)
				{
					this.airPressureTestVisible = value;
					this.OnPropertyChanged("AirPressureTestVisible");
				}
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06001F97 RID: 8087 RVA: 0x000D0773 File Offset: 0x000CE973
		// (set) Token: 0x06001F98 RID: 8088 RVA: 0x000D077B File Offset: 0x000CE97B
		public BoundLocalizedText StrEjectTime { get; set; }

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06001F99 RID: 8089 RVA: 0x000D0784 File Offset: 0x000CE984
		// (set) Token: 0x06001F9A RID: 8090 RVA: 0x000D078C File Offset: 0x000CE98C
		public BoundLocalizedText StrDelay { get; set; }

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06001F9B RID: 8091 RVA: 0x000D0795 File Offset: 0x000CE995
		// (set) Token: 0x06001F9C RID: 8092 RVA: 0x000D079D File Offset: 0x000CE99D
		public BoundLocalizedText StrRejecterEnabled { get; set; }

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06001F9D RID: 8093 RVA: 0x000D07A6 File Offset: 0x000CE9A6
		// (set) Token: 0x06001F9E RID: 8094 RVA: 0x000D07AE File Offset: 0x000CE9AE
		public BoundLocalizedText StrPriority { get; set; }

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06001F9F RID: 8095 RVA: 0x000D07B7 File Offset: 0x000CE9B7
		// (set) Token: 0x06001FA0 RID: 8096 RVA: 0x000D07BF File Offset: 0x000CE9BF
		public BoundLocalizedText StrContamination { get; set; }

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x000D07C8 File Offset: 0x000CE9C8
		// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x000D07D0 File Offset: 0x000CE9D0
		public BoundLocalizedText StrOverweight { get; set; }

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x000D07D9 File Offset: 0x000CE9D9
		// (set) Token: 0x06001FA4 RID: 8100 RVA: 0x000D07E1 File Offset: 0x000CE9E1
		public BoundLocalizedText StrUnderweight { get; set; }

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06001FA5 RID: 8101 RVA: 0x000D07EA File Offset: 0x000CE9EA
		// (set) Token: 0x06001FA6 RID: 8102 RVA: 0x000D07F2 File Offset: 0x000CE9F2
		public BoundLocalizedText StrAboveMaxObjectCount { get; set; }

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06001FA7 RID: 8103 RVA: 0x000D07FB File Offset: 0x000CE9FB
		// (set) Token: 0x06001FA8 RID: 8104 RVA: 0x000D0803 File Offset: 0x000CEA03
		public BoundLocalizedText StrBelowMinObjectCount { get; set; }

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06001FA9 RID: 8105 RVA: 0x000D080C File Offset: 0x000CEA0C
		// (set) Token: 0x06001FAA RID: 8106 RVA: 0x000D0814 File Offset: 0x000CEA14
		public BoundLocalizedText StrAboveMaxImageSize { get; set; }

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06001FAB RID: 8107 RVA: 0x000D081D File Offset: 0x000CEA1D
		// (set) Token: 0x06001FAC RID: 8108 RVA: 0x000D0825 File Offset: 0x000CEA25
		public BoundLocalizedText StrBelowMinImageSize { get; set; }

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x000D082E File Offset: 0x000CEA2E
		// (set) Token: 0x06001FAE RID: 8110 RVA: 0x000D0836 File Offset: 0x000CEA36
		public BoundLocalizedText StrLane { get; set; }

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06001FAF RID: 8111 RVA: 0x000D083F File Offset: 0x000CEA3F
		// (set) Token: 0x06001FB0 RID: 8112 RVA: 0x000D0847 File Offset: 0x000CEA47
		public BoundLocalizedText StrBeltStopOnContaminant { get; set; }

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06001FB1 RID: 8113 RVA: 0x000D0850 File Offset: 0x000CEA50
		// (set) Token: 0x06001FB2 RID: 8114 RVA: 0x000D0858 File Offset: 0x000CEA58
		public BoundLocalizedText StrBinFullTimeout { get; set; }

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x000D0861 File Offset: 0x000CEA61
		// (set) Token: 0x06001FB4 RID: 8116 RVA: 0x000D0869 File Offset: 0x000CEA69
		public BoundLocalizedText StrType { get; set; }

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x000D0872 File Offset: 0x000CEA72
		// (set) Token: 0x06001FB6 RID: 8118 RVA: 0x000D087A File Offset: 0x000CEA7A
		public BoundLocalizedText StrProductSubGroup { get; set; }

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x000D0883 File Offset: 0x000CEA83
		// (set) Token: 0x06001FB8 RID: 8120 RVA: 0x000D088B File Offset: 0x000CEA8B
		public BoundLocalizedText StrRejectVerifyEnabled { get; set; }

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x06001FB9 RID: 8121 RVA: 0x000D0894 File Offset: 0x000CEA94
		// (set) Token: 0x06001FBA RID: 8122 RVA: 0x000D089C File Offset: 0x000CEA9C
		public BoundLocalizedText StrRejectVerifyLearn { get; set; }

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x06001FBB RID: 8123 RVA: 0x000D08A5 File Offset: 0x000CEAA5
		// (set) Token: 0x06001FBC RID: 8124 RVA: 0x000D08AD File Offset: 0x000CEAAD
		public BoundLocalizedText StrRejectVerifyTest { get; set; }

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06001FBD RID: 8125 RVA: 0x000D08B6 File Offset: 0x000CEAB6
		// (set) Token: 0x06001FBE RID: 8126 RVA: 0x000D08BE File Offset: 0x000CEABE
		public BoundLocalizedText StrMode { get; set; }

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06001FBF RID: 8127 RVA: 0x000D08C7 File Offset: 0x000CEAC7
		// (set) Token: 0x06001FC0 RID: 8128 RVA: 0x000D08CF File Offset: 0x000CEACF
		public BoundLocalizedText StrTrainInOutDelay { get; set; }

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06001FC1 RID: 8129 RVA: 0x000D08D8 File Offset: 0x000CEAD8
		// (set) Token: 0x06001FC2 RID: 8130 RVA: 0x000D08E0 File Offset: 0x000CEAE0
		public BoundLocalizedText StrEncoderCorrectionTime { get; set; }

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06001FC3 RID: 8131 RVA: 0x000D08E9 File Offset: 0x000CEAE9
		// (set) Token: 0x06001FC4 RID: 8132 RVA: 0x000D08F1 File Offset: 0x000CEAF1
		public BoundLocalizedText StrUnlockBin { get; set; }

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06001FC5 RID: 8133 RVA: 0x000D08FA File Offset: 0x000CEAFA
		// (set) Token: 0x06001FC6 RID: 8134 RVA: 0x000D0902 File Offset: 0x000CEB02
		public BoundLocalizedText StrUnlockBinTimeout { get; set; }

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06001FC7 RID: 8135 RVA: 0x000D090B File Offset: 0x000CEB0B
		// (set) Token: 0x06001FC8 RID: 8136 RVA: 0x000D0913 File Offset: 0x000CEB13
		public BoundLocalizedText StrRejectVerifySensorBlockedTimeout { get; set; }

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x06001FC9 RID: 8137 RVA: 0x000D091C File Offset: 0x000CEB1C
		// (set) Token: 0x06001FCA RID: 8138 RVA: 0x000D0924 File Offset: 0x000CEB24
		public BoundLocalizedText StrDebounceTime { get; set; }

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x000D092D File Offset: 0x000CEB2D
		// (set) Token: 0x06001FCC RID: 8140 RVA: 0x000D0935 File Offset: 0x000CEB35
		public BoundLocalizedText StrTestAirPressureMonitoing { get; set; }

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x000D093E File Offset: 0x000CEB3E
		// (set) Token: 0x06001FCE RID: 8142 RVA: 0x000D0946 File Offset: 0x000CEB46
		public RelayCommand VerifyTestCommand { get; set; }

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x000D094F File Offset: 0x000CEB4F
		// (set) Token: 0x06001FD0 RID: 8144 RVA: 0x000D0957 File Offset: 0x000CEB57
		public RelayCommand VerifyLearnCommand { get; set; }

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x000D0960 File Offset: 0x000CEB60
		// (set) Token: 0x06001FD2 RID: 8146 RVA: 0x000D0968 File Offset: 0x000CEB68
		public RelayCommand MSLearnCommand { get; set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x000D0971 File Offset: 0x000CEB71
		// (set) Token: 0x06001FD4 RID: 8148 RVA: 0x000D0979 File Offset: 0x000CEB79
		public RelayCommand UnlockBinCommand { get; set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x000D0982 File Offset: 0x000CEB82
		// (set) Token: 0x06001FD6 RID: 8150 RVA: 0x000D098A File Offset: 0x000CEB8A
		public RelayCommand TestAirPressureMonitoringCommand { get; set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x000D0993 File Offset: 0x000CEB93
		// (set) Token: 0x06001FD8 RID: 8152 RVA: 0x000D099B File Offset: 0x000CEB9B
		public MachineState MachineState { get; set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06001FD9 RID: 8153 RVA: 0x000D09A4 File Offset: 0x000CEBA4
		// (set) Token: 0x06001FDA RID: 8154 RVA: 0x000D09AC File Offset: 0x000CEBAC
		public bool DemoMode { get; set; }

		// Token: 0x06001FDB RID: 8155 RVA: 0x000D09B8 File Offset: 0x000CEBB8
		public Rejecter(ParameterHandling parHandling, DBConnection dbConnection, string rejecterMainPath, int rejecterIndex, AccessRightsManager accessManager, MachineState machineState, MessageSystem messageSystem)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.rejecterMainPath = rejecterMainPath;
			this.rejecterIndex = rejecterIndex;
			this.accessManager = accessManager;
			this.MachineState = machineState;
			this.messageSystem = messageSystem;
			bool demoMode = false;
			dbConnection.GetDemoMode(ref demoMode);
			this.DemoMode = demoMode;
			string s = parHandling.ReadPar(Definition.ioVersionPath);
			int.TryParse(s, out this.iUserProgrammVersion);
			this.VerifyTestCommand = new RelayCommand(new Action<object>(this.VerifyTest));
			this.VerifyLearnCommand = new RelayCommand(new Action<object>(this.VerifyLearn));
			this.MSLearnCommand = new RelayCommand(new Action<object>(this.MSLearn));
			this.UnlockBinCommand = new RelayCommand(new Action<object>(this.UnlockBin));
			this.TestAirPressureMonitoringCommand = new RelayCommand(new Action<object>(this.TestAirPressureMonitoring));
			this.rightsOk = (accessManager.CurrentUser.Rights >= this.rejecterChangeRight);
			this.op1CompontentVisible = this.rightsOk;
			this.modeVisible = (accessManager.CurrentUser.Rights >= this.rejecterModeRight);
			this.GetValuesFromDatabase();
			this.GetValuesFromParamInterface();
			this.SetVerifyTestVisibility(accessManager);
			this.SetMSVisibility(accessManager);
			this.SetRejectEnableAccessible();
			this.SetVisibilityForRayconDAndAbove(accessManager);
			this.RegisterLocalizedText();
			this.TabHeight = 50;
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000D0B50 File Offset: 0x000CED50
		public Rejecter(ParameterHandling parHandling, DBConnection dbConnection)
		{
			this.parHandling = parHandling;
			this.dbConnection = dbConnection;
			this.cUnlockBin1 = new RejecterItem("txtUnlockBin1", new Action<object>(this.UnlockBin1Selected), parHandling);
			this.cUnlockBin2 = new RejecterItem("txtUnlockBin2", new Action<object>(this.UnlockBin2Selected), parHandling);
			MainWindow.localizationManager.Register(this.cUnlockBin1);
			MainWindow.localizationManager.Register(this.cUnlockBin2);
			this.CreateUnlockBinItems();
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000D0BF8 File Offset: 0x000CEDF8
		private void SetRejectEnableAccessible()
		{
			bool flag = false;
			this.dbConnection.GetUKUserRightsHL(ref flag);
			bool flag2 = !flag;
			if (flag2)
			{
				this.RejectEnableAccessible = (this.accessManager.CurrentUser.Rights >= this.rejecterChangeRight);
			}
			else
			{
				this.RejectEnableAccessible = (this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Admin);
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x000D0C64 File Offset: 0x000CEE64
		private void SetVerifyTestVisibility(AccessRightsManager accessManager)
		{
			this.verifyTestVisible = this.verifyVisible;
			bool flag = !this.rightsOk;
			if (flag)
			{
				this.verifyTestVisible = (accessManager.CurrentUser.Rights >= Rejecter.doVerifyTestRight);
			}
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x000D0CA8 File Offset: 0x000CEEA8
		private void SetMSVisibility(AccessRightsManager accessManager)
		{
			MachineMode machineMode = MachineMode.Standard;
			bool flag = false;
			bool flag2 = false;
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetHighLevelSensors(ref flag);
			this.dbConnection.GetDoorLockAvailable(ref flag2);
			this.dbConnection.GetMachineMode(ref machineMode);
			this.dbConnection.GetMachineType(ref machineType);
			this.msVisible = (machineMode == MachineMode.MAndS && accessManager.CurrentUser.Rights >= this.msRight);
			this.highLevelSensors = (machineMode == MachineMode.MAndS && flag && accessManager.CurrentUser.Rights >= this.msRight);
			this.msBinUnlockVisible = (machineMode == MachineMode.MAndS && flag2 && accessManager.CurrentUser.Rights >= Rejecter.doVerifyTestRight);
			this.msBinUnlockTimeoutVisible = (machineMode == MachineMode.MAndS && flag2 && accessManager.CurrentUser.Rights >= this.msRight);
			this.debounceTimeVisible = (this.highLevelSensors && this.parHandling.ReadPar(Definition.msVersion1Path) != "1");
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000D0DB8 File Offset: 0x000CEFB8
		private void GetValuesFromParamInterface()
		{
			int num = 7;
			string s = this.parHandling.ReadPar(Definition.ioVersionPath);
			int.TryParse(s, out num);
			string a = this.parHandling.ReadPar(Definition.ioSubversionPath);
			this.beltStopOnContaminantVisible = (this.rightsOk && (num == 0 || num >= 7));
			this.binFullTimeoutVisible = (this.rightsOk && a != "1" && (num == 0 || num >= 7));
			this.verifyVisible = (this.rightsOk && a != "1");
			this.TimeoutRejectVerifySensorBlockedVisible = (this.rightsOk && a != "1" && (num == 0 || num >= Definition.minWagoVersionForUserProgramType));
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000D0E8C File Offset: 0x000CF08C
		private void RegisterLocalizedText()
		{
			this.StrEjectTime = new BoundLocalizedText("txtEjectTime");
			this.StrDelay = new BoundLocalizedText("txtDelay");
			this.StrRejecterEnabled = new BoundLocalizedText("txtRejecterEnabled");
			this.StrPriority = new BoundLocalizedText("txtPriority");
			this.StrType = new BoundLocalizedText("txtType");
			this.StrContamination = new BoundLocalizedText("txtContamination");
			this.StrOverweight = new BoundLocalizedText("txtOverweight");
			this.StrUnderweight = new BoundLocalizedText("txtUnderweight");
			this.StrAboveMaxObjectCount = new BoundLocalizedText("txtAboveMaxObjectCount");
			this.StrBelowMinObjectCount = new BoundLocalizedText("txtBelowMinObjectCount");
			this.StrAboveMaxImageSize = new BoundLocalizedText("txtAboveMaxImageSize");
			this.StrBelowMinImageSize = new BoundLocalizedText("txtBelowMinImageSize");
			this.StrLane = new BoundLocalizedText("txtLane");
			this.StrBeltStopOnContaminant = new BoundLocalizedText("txtBeltStopOnContaminant");
			this.StrBinFullTimeout = new BoundLocalizedText("txtBinFullTimeout");
			this.StrProductSubGroup = new BoundLocalizedText("txtProductSubGroup");
			this.StrMode = new BoundLocalizedText("txtMode");
			this.StrRejectVerifyEnabled = new BoundLocalizedText("txtRejectVerifyEnabled");
			this.StrRejectVerifyLearn = new BoundLocalizedText("txtRejectVerifyLearn");
			this.StrRejectVerifyTest = new BoundLocalizedText("txtRejectVerifyTest");
			this.StrRejectVerifySensorBlockedTimeout = new BoundLocalizedText("txtRejectVerifySensorBlockedTimeout");
			this.StrTrainInOutDelay = new BoundLocalizedText("txtTrainInOutDelay");
			this.StrEncoderCorrectionTime = new BoundLocalizedText("txtEncoderCorrectionTime");
			this.StrUnlockBin = new BoundLocalizedText("txtUnlockBin");
			this.StrUnlockBinTimeout = new BoundLocalizedText("txtUnlockBinTimeout");
			this.StrDebounceTime = new BoundLocalizedText("txtDebounceTime");
			this.StrTestAirPressureMonitoing = new BoundLocalizedText("txtTestAirPressureMonitoing");
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x000D1068 File Offset: 0x000CF268
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrEjectTime);
			MainWindow.localizationManager.Deregister(this.StrDelay);
			MainWindow.localizationManager.Deregister(this.StrRejecterEnabled);
			MainWindow.localizationManager.Deregister(this.StrPriority);
			MainWindow.localizationManager.Deregister(this.StrType);
			MainWindow.localizationManager.Deregister(this.StrContamination);
			MainWindow.localizationManager.Deregister(this.StrOverweight);
			MainWindow.localizationManager.Deregister(this.StrUnderweight);
			MainWindow.localizationManager.Deregister(this.StrAboveMaxObjectCount);
			MainWindow.localizationManager.Deregister(this.StrBelowMinObjectCount);
			MainWindow.localizationManager.Deregister(this.StrAboveMaxImageSize);
			MainWindow.localizationManager.Deregister(this.StrBelowMinImageSize);
			MainWindow.localizationManager.Deregister(this.StrLane);
			MainWindow.localizationManager.Deregister(this.StrBeltStopOnContaminant);
			MainWindow.localizationManager.Deregister(this.StrBinFullTimeout);
			MainWindow.localizationManager.Deregister(this.StrProductSubGroup);
			MainWindow.localizationManager.Deregister(this.StrMode);
			MainWindow.localizationManager.Deregister(this.StrRejectVerifyEnabled);
			MainWindow.localizationManager.Deregister(this.StrRejectVerifyLearn);
			MainWindow.localizationManager.Deregister(this.StrRejectVerifyTest);
			MainWindow.localizationManager.Deregister(this.StrRejectVerifySensorBlockedTimeout);
			MainWindow.localizationManager.Deregister(this.StrTrainInOutDelay);
			MainWindow.localizationManager.Deregister(this.StrEncoderCorrectionTime);
			MainWindow.localizationManager.Deregister(this.StrUnlockBin);
			MainWindow.localizationManager.Deregister(this.StrUnlockBinTimeout);
			MainWindow.localizationManager.Deregister(this.StrDebounceTime);
			MainWindow.localizationManager.Deregister(this.StrTestAirPressureMonitoing);
			MainWindow.localizationManager.Deregister(this.cUnlockBin1);
			MainWindow.localizationManager.Deregister(this.cUnlockBin2);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000D1264 File Offset: 0x000CF464
		private void GetValuesFromDatabase()
		{
			string text = null;
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/Delay/Value", ref text);
			int.TryParse(text, out this.delay);
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/Length/Value", ref text);
			int.TryParse(text, out this.ejectTime);
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/Enabled/Value", ref text);
			this.enable = (text == "1");
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/VerifyEnable/Value", ref text);
			this.verifyEnable = (text == "1");
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/BeltStopOnContaminant/Value", ref text);
			this.beltStopOnContaminant = (text == "1");
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/BinFullTimeout/Value", ref text);
			int.TryParse(text, out this.binFullTimeout);
			bool flag = this.GetWagoProgramVersion() < Definition.minWagoVersionForUserProgramType;
			if (flag)
			{
				this.binFullTimeout /= 1000;
			}
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/LightBarrierBlockedTimeout/Value", ref text);
			int.TryParse(text, out this.rejectVerifySensorBlockedTimeout);
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/ImageGroup/Value", ref text);
			int.TryParse(text, out this.imageGroup);
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/HLDebounceTime/Value", ref text);
			int.TryParse(text, out this.debounceTime);
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/HLEFCorrectionTime/Value", ref text);
			int.TryParse(text, out this.correctionTime);
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/Lane/Value", ref text);
			try
			{
				this.lane = (Lane)Enum.Parse(typeof(Lane), text);
			}
			catch
			{
			}
			bool flag2 = this.rejecterIndex == 1;
			if (flag2)
			{
				this.dbConnection.GetMSBinTimeout(true, ref this.binUnlockTimeout);
				bool flag3 = this.iUserProgrammVersion < 26;
				if (flag3)
				{
					this.binUnlockTimeout /= 1000;
				}
			}
			else
			{
				bool flag4 = this.rejecterIndex == 2;
				if (flag4)
				{
					this.dbConnection.GetMSBinTimeout(false, ref this.binUnlockTimeout);
					bool flag5 = this.iUserProgrammVersion < 26;
					if (flag5)
					{
						this.binUnlockTimeout /= 1000;
					}
				}
			}
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/Priority/Value", ref text);
			string text2 = text;
			string a = text2;
			if (!(a == "0"))
			{
				if (!(a == "1"))
				{
					if (!(a == "2"))
					{
						if (!(a == "3"))
						{
							this.priority = RejectPriority.Highest;
						}
						else
						{
							this.priority = RejectPriority.Lowest;
						}
					}
					else
					{
						this.priority = RejectPriority.Low;
					}
				}
				else
				{
					this.priority = RejectPriority.High;
				}
			}
			else
			{
				this.priority = RejectPriority.Highest;
			}
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/Mode/Value", ref text);
			string text3 = text;
			string a2 = text3;
			if (!(a2 == "0"))
			{
				if (!(a2 == "1"))
				{
					this.mode = RejectMode.Center;
				}
				else
				{
					this.mode = RejectMode.LeadingEdge;
				}
			}
			else
			{
				this.mode = RejectMode.Center;
			}
			this.Modes = new List<RejectMode>();
			this.Modes.Add(RejectMode.Center);
			this.Modes.Add(RejectMode.LeadingEdge);
			this.InitializeRejectTypes();
			this.InitializeLanes();
			this.InitializeGroupsAndVisibilities();
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000D1654 File Offset: 0x000CF854
		private void InitializeGroupsAndVisibilities()
		{
			string text = "";
			List<string> list = new List<string>();
			int item = 0;
			string text2 = "";
			ProductType productType = ProductType.SingleProduct;
			string a = "";
			this.ImageGroups = new List<int>();
			this.dbConnection.GetCurrentProductTableName(ref text);
			this.dbConnection.GetProductType(text, ref productType);
			bool flag = productType > ProductType.SingleProduct;
			if (flag)
			{
				this.imageGroupVisible = this.rightsOk;
				this.ImageGroups.Add(0);
				this.dbConnection.GetSubProductTablenames(text, list);
				foreach (string text3 in list)
				{
					this.dbConnection.GetProductNumber(text3, ref text2);
					string text4 = text3.Remove(0, text3.IndexOf(Definition.subTableIdentifier) + 1);
					bool flag2 = int.TryParse(text4, out item);
					if (flag2)
					{
						this.ImageGroups.Add(item);
					}
					this.dbConnection.GetParameterValue(Definition.filterMainPath + text4 + Weigher.enableWeightSubPath, text3, ref a);
					this.weightVisible |= (a != "0");
					this.dbConnection.GetParameterValue(Definition.filterMainPath + text4 + ObjectCounter.enabledSubPath, text3, ref a);
					this.objectCountVisible |= (a != "0");
					this.dbConnection.GetParameterValue(Definition.filterMainPath + text4 + Definition.sizeFilterEnableSubPath, text3, ref a);
					this.imageSizeVisible |= (a != "0");
				}
			}
			else
			{
				this.dbConnection.GetParameterValue(Definition.filterMainPath + Weigher.enableWeightSubPath, ref a);
				this.weightVisible = (a != "0");
				this.dbConnection.GetParameterValue(Definition.filterMainPath + ObjectCounter.enabledSubPath, ref a);
				this.objectCountVisible = (a != "0");
				this.dbConnection.GetParameterValue(Definition.filterMainPath + Definition.sizeFilterEnableSubPath, ref a);
				this.imageSizeVisible = (a != "0");
				bool flag3 = !this.objectCountVisible && this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1";
				if (flag3)
				{
					this.objectCountVisible = (this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1" && this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1");
				}
			}
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000D1924 File Offset: 0x000CFB24
		private void InitializeRejectTypes()
		{
			string s = null;
			int num = 0;
			this.dbConnection.GetParameterValue(this.rejecterMainPath + "/Type/Value", ref s);
			int.TryParse(s, out num);
			this.contaminationEnabled = ((num & 1) == 1);
			this.overweightEnabled = ((num & 2) == 2);
			this.underweightEnabled = ((num & 4) == 4);
			this.overcountEnabled = ((num & 8) == 8);
			this.undercountEnabled = ((num & 16) == 16);
			this.oversizeEnabled = ((num & 32) == 32);
			this.undersizeEnabled = ((num & 64) == 64);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000D19B8 File Offset: 0x000CFBB8
		private void InitializeLanes()
		{
			int num = 0;
			string a = null;
			this.Lanes = new List<Lane>();
			bool flag = this.dbConnection.GetParameterValue(Acquisition.lane1EnablePath, ref a) && a != "0";
			if (flag)
			{
				num++;
				this.Lanes.Add(Lane.Lane1);
			}
			bool flag2 = this.dbConnection.GetParameterValue(Acquisition.lane2EnablePath, ref a) && a != "0";
			if (flag2)
			{
				num++;
				this.Lanes.Add(Lane.Lane2);
			}
			bool flag3 = this.dbConnection.GetParameterValue(Acquisition.lane3EnablePath, ref a) && a != "0";
			if (flag3)
			{
				num++;
				this.Lanes.Add(Lane.Lane3);
			}
			bool flag4 = this.dbConnection.GetParameterValue(Acquisition.lane4EnablePath, ref a) && a != "0";
			if (flag4)
			{
				num++;
				this.Lanes.Add(Lane.Lane4);
			}
			bool flag5 = num > 1;
			if (flag5)
			{
				this.laneVisible = this.rightsOk;
				this.Lanes.Insert(0, Lane.All);
			}
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000D1AE4 File Offset: 0x000CFCE4
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000D1B14 File Offset: 0x000CFD14
		public void SetDelay()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDelay"), this.Delay.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetDelay;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000D1B6C File Offset: 0x000CFD6C
		private void numKeyboard_SetDelay(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Delay = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetDelay;
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x000D1BC0 File Offset: 0x000CFDC0
		public void SetEjectTime()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtEjectTime"), this.EjectTime.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetEjectTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x000D1C18 File Offset: 0x000CFE18
		private void numKeyboard_SetEjectTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.EjectTime = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetEjectTime;
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000D1C6C File Offset: 0x000CFE6C
		public void SetBinFullTimeout()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtBinFullTimeout"), this.BinFullTimeout.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetBinFullTimeout;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000D1CC4 File Offset: 0x000CFEC4
		private void numKeyboard_SetBinFullTimeout(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.BinFullTimeout = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetBinFullTimeout;
		}

		// Token: 0x06001FEE RID: 8174 RVA: 0x000D1D18 File Offset: 0x000CFF18
		public void SetBinUnlockTimeout()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtUnlockBinTimeout"), this.BinUnlockTimeout.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetBinUnlockTimeout;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001FEF RID: 8175 RVA: 0x000D1D70 File Offset: 0x000CFF70
		private void numKeyboard_SetBinUnlockTimeout(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.BinUnlockTimeout = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetBinFullTimeout;
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x000D1DC4 File Offset: 0x000CFFC4
		private void SetRejectType()
		{
			int num = 0;
			bool flag = this.contaminationEnabled;
			if (flag)
			{
				num |= 1;
			}
			bool flag2 = this.overweightEnabled;
			if (flag2)
			{
				num |= 2;
			}
			bool flag3 = this.underweightEnabled;
			if (flag3)
			{
				num |= 4;
			}
			bool flag4 = this.overcountEnabled;
			if (flag4)
			{
				num |= 8;
			}
			bool flag5 = this.undercountEnabled;
			if (flag5)
			{
				num |= 16;
			}
			bool flag6 = this.oversizeEnabled;
			if (flag6)
			{
				num |= 32;
			}
			bool flag7 = this.undersizeEnabled;
			if (flag7)
			{
				num |= 64;
			}
			this.parHandling.WriteValue(this.rejecterMainPath + "/Type/Value", num.ToString());
		}

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000D1E64 File Offset: 0x000D0064
		private void VerifyTest(object obj)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetMachineType(ref machineType);
			bool flag = machineType == MachineType.RayconXRS;
			if (flag)
			{
				bool flag2 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtStartVerifyTest"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
				if (flag2)
				{
					bool flag3 = this.rejecterIndex == 1;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.msResetError1Path, "7", false);
					}
					else
					{
						bool flag4 = this.rejecterIndex == 2;
						if (flag4)
						{
							this.parHandling.WriteValue(Definition.msResetError2Path, "7", false);
						}
					}
					this.parHandling.WriteValue(this.rejecterMainPath + "/VerifyTest/Value", "1", false);
				}
			}
			else
			{
				bool flag5 = this.MachineState.State == MachineStates.Inspecting;
				if (flag5)
				{
					this.parHandling.WriteValue(this.rejecterMainPath + "/Length/Value", "15", false);
					this.parHandling.WriteValue(this.rejecterMainPath + "/Mode/Value", 0.ToString(), false);
					WdTrain wdTrain = new WdTrain(this.parHandling, this.rejecterMainPath, TrainMode.VerifyHL, 0, this.messageSystem, machineType);
					wdTrain.ShowDialog();
					ParameterHandling parameterHandling = this.parHandling;
					string path = this.rejecterMainPath + "/Mode/Value";
					int num = (int)this.mode;
					parameterHandling.WriteValue(path, num.ToString(), false);
					this.parHandling.WriteValue(this.rejecterMainPath + "/Length/Value", this.ejectTime.ToString(), false);
				}
			}
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x000D2004 File Offset: 0x000D0204
		private void VerifyLearn(object obj)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.parHandling.WriteValue(this.rejecterMainPath + "/VerifyTrain/Value", "1", false);
				WdTrain wdTrain = new WdTrain(this.parHandling, this.rejecterMainPath, TrainMode.Verify);
				wdTrain.ShowDialog();
				bool flag2 = this.parHandling.ReadPar(this.rejecterMainPath + "/VerifyTrain/Value") == "0";
				if (flag2)
				{
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtAfterAutotrain"), DialogStyle.Ok, DialogButtons.None, true);
				}
				else
				{
					this.parHandling.WriteValue(this.rejecterMainPath + "/VerifyTrain/Value", "0", false);
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtAbortAutotrain"), DialogStyle.Error, DialogButtons.None, true);
				}
			}
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000D2100 File Offset: 0x000D0300
		private void MSLearn(object obj)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.parHandling.WriteValue(this.rejecterMainPath + "/HLSCInOutDelayTrain/Value", "1", false);
				WdTrain wdTrain = new WdTrain(this.parHandling, this.rejecterMainPath, TrainMode.InOutDelay);
				wdTrain.ShowDialog();
				int num = 0;
				string text = this.parHandling.ReadPar(this.rejecterMainPath + "/HLSCInOutDelayTrained/Value");
				bool flag2 = text != "0" && int.TryParse(text, out num) && num > 0;
				if (flag2)
				{
					this.dbConnection.UpdateCurrentProductTable(this.rejecterMainPath + "/HLSCInOutDelayInit/Value", num.ToString());
					this.parHandling.WriteValue(this.rejecterMainPath + "/HLSCInOutDelayInit/Value", num.ToString(), false);
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtAfterAutotrain"), DialogStyle.Ok, DialogButtons.None, true);
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtAbortAutotrain"), DialogStyle.Error, DialogButtons.None, true);
				}
			}
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x000D2240 File Offset: 0x000D0440
		private void UnlockBin(object obj)
		{
			int binTimeout = 0;
			this.dbConnection.GetMSBinTimeout(this.rejecterIndex == 1, ref binTimeout);
			Rejecter.UnlockBin(this.rejecterMainPath, this.parHandling, binTimeout);
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000D227C File Offset: 0x000D047C
		private void UnlockBin1Selected(object obj)
		{
			int binTimeout = 0;
			this.dbConnection.GetMSBinTimeout(true, ref binTimeout);
			Rejecter.UnlockBin(Rejecter.rejecter1MainPath, this.parHandling, binTimeout);
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000D22B0 File Offset: 0x000D04B0
		private void UnlockBin2Selected(object obj)
		{
			int binTimeout = 0;
			this.dbConnection.GetMSBinTimeout(false, ref binTimeout);
			Rejecter.UnlockBin(Rejecter.rejecter2MainPath, this.parHandling, binTimeout);
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x000D22E4 File Offset: 0x000D04E4
		public void SetDebounceTime()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDebounceTime"), this.DebounceTime.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetDebounceTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x000D233C File Offset: 0x000D053C
		private void numKeyboard_SetDebounceTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.DebounceTime = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetDebounceTime;
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x000D2390 File Offset: 0x000D0590
		public void SetCorrectionTime()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtEncoderCorrectionTime"), this.CorrectionTime.ToString(), NumKeyboardType.TypeInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetCorrectionTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x000D23E8 File Offset: 0x000D05E8
		private void numKeyboard_SetCorrectionTime(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.CorrectionTime = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetCorrectionTime;
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x000D243C File Offset: 0x000D063C
		private int GetWagoProgramVersion()
		{
			int result = 0;
			bool flag = this.parHandling != null;
			if (flag)
			{
				string s = this.parHandling.ReadPar(Definition.ioVersionPath);
				int.TryParse(s, out result);
			}
			return result;
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x000D2480 File Offset: 0x000D0680
		public void SetRejectVerifySensorBlockedTimeout()
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtRejectVerifySensorBlockedTimeout"), this.RejectVerifySensorBlockedTimeout.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetRejectVerifySensorBlockedTimeout;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x000D24D8 File Offset: 0x000D06D8
		private void numKeyboard_SetRejectVerifySensorBlockedTimeout(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.RejectVerifySensorBlockedTimeout = int.Parse(e.Text);
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetRejectVerifySensorBlockedTimeout;
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x000D252C File Offset: 0x000D072C
		private void TestAirPressureMonitoring(object obj)
		{
			MachineType machineType = MachineType.RayconD;
			this.dbConnection.GetMachineType(ref machineType);
			bool isDPlus1Module = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module || this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
			string testAirPressurePath = Definition.GetTestAirPressurePath(machineType, isDPlus1Module);
			bool flag = this.MachineState.State == MachineStates.Inspecting;
			if (flag)
			{
				this.parHandling.WriteValue(testAirPressurePath, "1", false);
				WdTrain wdTrain = new WdTrain(this.parHandling, this.rejecterMainPath, TrainMode.Air, 0, this.messageSystem, machineType);
				wdTrain.ShowDialog();
				this.parHandling.WriteValue(testAirPressurePath, "0", false);
			}
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x000D25F4 File Offset: 0x000D07F4
		private void SetVisibilityForRayconDAndAbove(AccessRightsManager accessManager)
		{
			MachineType machineType = MachineType.MachineTypeUndefined;
			MachineMode machineMode = MachineMode.Standard;
			this.dbConnection.GetMachineType(ref machineType);
			bool flag = machineType > MachineType.RayconXRS;
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				this.dbConnection.GetHigherLevelFeatures(ref flag2);
				this.dbConnection.GetLineJamSensor(ref flag3);
				this.dbConnection.GetMachineMode(ref machineMode);
				string a = this.parHandling.ReadPar(Definition.ioSubversionPath);
				this.binFullTimeoutVisible &= (flag2 || (machineMode == MachineMode.Standard && a == "2"));
				this.verifyVisible &= (flag2 || (machineMode == MachineMode.Standard && a == "2"));
				this.verifyTestVisible = (this.verifyTestVisible && flag2);
				this.TimeoutRejectVerifySensorBlockedVisible = (this.rightsOk && (flag3 || (this.TimeoutRejectVerifySensorBlockedVisible && (flag2 || (machineMode == MachineMode.Standard && a == "2")))));
				this.airPressureTestVisible = (flag2 && accessManager.CurrentUser.Rights >= Rejecter.doVerifyTestRight);
			}
			else
			{
				this.airPressureTestVisible = false;
			}
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x000D2724 File Offset: 0x000D0924
		public static void UnlockBin(string rejecterMainPath, ParameterHandling parHandling, int binTimeout)
		{
			parHandling.WriteValue(rejecterMainPath + "/HLBinUnlock/Value", "1", false);
			WdTrain wdTrain = new WdTrain(parHandling, rejecterMainPath, TrainMode.UnlockBin, binTimeout);
			wdTrain.ShowDialog();
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x000D275B File Offset: 0x000D095B
		private void CreateUnlockBinItems()
		{
			this.UnlockBinItems = new ObservableCollection<RejecterItem>();
			this.UnlockBinItems.Add(this.cUnlockBin1);
			this.UnlockBinItems.Add(this.cUnlockBin2);
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x000D2790 File Offset: 0x000D0990
		public void Cleanup()
		{
			bool flag = this.UnlockBinItems != null;
			if (flag)
			{
				for (int i = 0; i < this.UnlockBinItems.Count; i++)
				{
					this.UnlockBinItems[i] = null;
				}
			}
			this.DeregisterLocalizedText();
			this.UnlockBinItems.Clear();
			this.UnlockBinItems = null;
			this.cUnlockBin1 = null;
			this.cUnlockBin2 = null;
		}

		// Token: 0x04001458 RID: 5208
		private BuiltInPriviledges rejecterChangeRight = BuiltInPriviledges.Operator1;

		// Token: 0x04001459 RID: 5209
		public static readonly BuiltInPriviledges doVerifyTestRight = BuiltInPriviledges.Operator2;

		// Token: 0x0400145A RID: 5210
		private BuiltInPriviledges rejecterModeRight = BuiltInPriviledges.Service;

		// Token: 0x0400145B RID: 5211
		private BuiltInPriviledges msRight = BuiltInPriviledges.Operator1;

		// Token: 0x0400145C RID: 5212
		public static readonly string rejecter1MainPath = "/hardware/rejecter/rejecter1";

		// Token: 0x0400145D RID: 5213
		public static readonly string rejecter2MainPath = "/hardware/rejecter/rejecter2";

		// Token: 0x0400145E RID: 5214
		public static readonly string rejecter3MainPath = "/hardware/rejecter/rejecter3";

		// Token: 0x0400145F RID: 5215
		public static readonly string rejecter4MainPath = "/hardware/rejecter/rejecter4";

		// Token: 0x04001460 RID: 5216
		private RejecterItem cUnlockBin1;

		// Token: 0x04001461 RID: 5217
		private RejecterItem cUnlockBin2;

		// Token: 0x04001462 RID: 5218
		private NumKeyboard numKeyboard;

		// Token: 0x04001463 RID: 5219
		private ParameterHandling parHandling;

		// Token: 0x04001464 RID: 5220
		private DBConnection dbConnection;

		// Token: 0x04001465 RID: 5221
		private string rejecterMainPath;

		// Token: 0x04001466 RID: 5222
		private int delay;

		// Token: 0x04001467 RID: 5223
		private int ejectTime;

		// Token: 0x04001468 RID: 5224
		private int output;

		// Token: 0x04001469 RID: 5225
		private RejectPriority priority;

		// Token: 0x0400146A RID: 5226
		private RejectMode mode;

		// Token: 0x0400146B RID: 5227
		private Lane lane;

		// Token: 0x0400146C RID: 5228
		private bool enable;

		// Token: 0x0400146D RID: 5229
		private bool verifyEnable;

		// Token: 0x0400146E RID: 5230
		private int rejecterIndex;

		// Token: 0x0400146F RID: 5231
		private bool priorityVisible;

		// Token: 0x04001470 RID: 5232
		private bool laneVisible;

		// Token: 0x04001471 RID: 5233
		private bool weightVisible;

		// Token: 0x04001472 RID: 5234
		private bool objectCountVisible;

		// Token: 0x04001473 RID: 5235
		private bool imageSizeVisible;

		// Token: 0x04001474 RID: 5236
		private bool imageGroupVisible;

		// Token: 0x04001475 RID: 5237
		private bool beltStopOnContaminantVisible;

		// Token: 0x04001476 RID: 5238
		private bool binFullTimeoutVisible;

		// Token: 0x04001477 RID: 5239
		private bool verifyVisible;

		// Token: 0x04001478 RID: 5240
		private bool modeVisible;

		// Token: 0x04001479 RID: 5241
		private bool contaminationEnabled;

		// Token: 0x0400147A RID: 5242
		private bool overweightEnabled;

		// Token: 0x0400147B RID: 5243
		private bool underweightEnabled;

		// Token: 0x0400147C RID: 5244
		private bool overcountEnabled;

		// Token: 0x0400147D RID: 5245
		private bool undercountEnabled;

		// Token: 0x0400147E RID: 5246
		private bool oversizeEnabled;

		// Token: 0x0400147F RID: 5247
		private bool undersizeEnabled;

		// Token: 0x04001480 RID: 5248
		private bool beltStopOnContaminant;

		// Token: 0x04001481 RID: 5249
		private int binFullTimeout;

		// Token: 0x04001482 RID: 5250
		private int rejectVerifySensorBlockedTimeout;

		// Token: 0x04001483 RID: 5251
		private int imageGroup;

		// Token: 0x04001484 RID: 5252
		private bool op1CompontentVisible;

		// Token: 0x04001485 RID: 5253
		private bool verifyTestVisible;

		// Token: 0x04001486 RID: 5254
		private bool rightsOk;

		// Token: 0x04001487 RID: 5255
		private bool msVisible;

		// Token: 0x04001488 RID: 5256
		private bool highLevelSensors;

		// Token: 0x04001489 RID: 5257
		private int binUnlockTimeout;

		// Token: 0x0400148A RID: 5258
		private bool msBinUnlockVisible;

		// Token: 0x0400148B RID: 5259
		private bool msBinUnlockTimeoutVisible;

		// Token: 0x0400148C RID: 5260
		private int debounceTime;

		// Token: 0x0400148D RID: 5261
		private int correctionTime;

		// Token: 0x0400148E RID: 5262
		private bool debounceTimeVisible;

		// Token: 0x0400148F RID: 5263
		private bool rejectEnableAccessible;

		// Token: 0x04001490 RID: 5264
		private AccessRightsManager accessManager;

		// Token: 0x04001491 RID: 5265
		private bool airPressureTestVisible;

		// Token: 0x04001492 RID: 5266
		private MessageSystem messageSystem;

		// Token: 0x04001493 RID: 5267
		private int iUserProgrammVersion;

		// Token: 0x04001496 RID: 5270
		private bool objectBasedFlapsVisible;
	}
}

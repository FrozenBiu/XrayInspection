using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Common;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000151 RID: 337
	public class TrainViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x06002025 RID: 8229 RVA: 0x000D34BC File Offset: 0x000D16BC
		// (set) Token: 0x06002026 RID: 8230 RVA: 0x000D34D4 File Offset: 0x000D16D4
		public string StyleResourceKey
		{
			get
			{
				return this.___StyleResourceKey;
			}
			private set
			{
				bool flag = this.___StyleResourceKey != value;
				if (flag)
				{
					this.___StyleResourceKey = value;
				}
			}
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06002027 RID: 8231 RVA: 0x000D34FC File Offset: 0x000D16FC
		// (set) Token: 0x06002028 RID: 8232 RVA: 0x000D3514 File Offset: 0x000D1714
		public string Title
		{
			get
			{
				return this.title;
			}
			set
			{
				this.title = value;
				this.OnPropertyChanged("Title");
			}
		}

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06002029 RID: 8233 RVA: 0x000D352C File Offset: 0x000D172C
		// (set) Token: 0x0600202A RID: 8234 RVA: 0x000D3544 File Offset: 0x000D1744
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
				this.OnPropertyChanged("Message");
			}
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x0600202B RID: 8235 RVA: 0x000D355C File Offset: 0x000D175C
		// (set) Token: 0x0600202C RID: 8236 RVA: 0x000D3574 File Offset: 0x000D1774
		public ImageSource ButtonImage
		{
			get
			{
				return this.buttonImage;
			}
			set
			{
				this.buttonImage = value;
				this.OnPropertyChanged("ButtonImage");
			}
		}

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x0600202D RID: 8237 RVA: 0x000D358C File Offset: 0x000D178C
		// (set) Token: 0x0600202E RID: 8238 RVA: 0x000D35A4 File Offset: 0x000D17A4
		public Style ButtonStyle
		{
			get
			{
				return this.buttonStyle;
			}
			set
			{
				this.buttonStyle = value;
				this.OnPropertyChanged("ButtonStyle");
			}
		}

		// Token: 0x140000A8 RID: 168
		// (add) Token: 0x0600202F RID: 8239 RVA: 0x000D35BC File Offset: 0x000D17BC
		// (remove) Token: 0x06002030 RID: 8240 RVA: 0x000D35F4 File Offset: 0x000D17F4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x140000A9 RID: 169
		// (add) Token: 0x06002031 RID: 8241 RVA: 0x000D362C File Offset: 0x000D182C
		// (remove) Token: 0x06002032 RID: 8242 RVA: 0x000D3664 File Offset: 0x000D1864
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action TrainFinshed;

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06002033 RID: 8243 RVA: 0x000D3699 File Offset: 0x000D1899
		// (set) Token: 0x06002034 RID: 8244 RVA: 0x000D36A1 File Offset: 0x000D18A1
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x06002035 RID: 8245 RVA: 0x000D36AA File Offset: 0x000D18AA
		// (set) Token: 0x06002036 RID: 8246 RVA: 0x000D36B2 File Offset: 0x000D18B2
		public bool TrainSuccessful { get; set; }

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06002037 RID: 8247 RVA: 0x000D36BB File Offset: 0x000D18BB
		// (set) Token: 0x06002038 RID: 8248 RVA: 0x000D36C3 File Offset: 0x000D18C3
		public WindowState WState { get; set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06002039 RID: 8249 RVA: 0x000D36CC File Offset: 0x000D18CC
		// (set) Token: 0x0600203A RID: 8250 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x0600203B RID: 8251 RVA: 0x000D36E4 File Offset: 0x000D18E4
		// (set) Token: 0x0600203C RID: 8252 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x0600203D RID: 8253 RVA: 0x000D36FC File Offset: 0x000D18FC
		// (set) Token: 0x0600203E RID: 8254 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x000D3714 File Offset: 0x000D1914
		// (set) Token: 0x06002040 RID: 8256 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x000D372B File Offset: 0x000D192B
		// (set) Token: 0x06002042 RID: 8258 RVA: 0x000D3733 File Offset: 0x000D1933
		public bool LowAirPressureTriggered { get; set; }

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x000D373C File Offset: 0x000D193C
		// (set) Token: 0x06002044 RID: 8260 RVA: 0x000D3744 File Offset: 0x000D1944
		public bool RejectVerifyTriggered { get; set; }

		// Token: 0x06002045 RID: 8261 RVA: 0x000D3750 File Offset: 0x000D1950
		private string CreateBinUnlockMessage()
		{
			string a = this.parHandling.ReadPar(Rejecter.rejecter2MainPath + "/Enabled/Value");
			bool flag = this.rejecterMainPath == Rejecter.rejecter1MainPath && a == "1";
			string text;
			if (flag)
			{
				text = LocalizationManager.Localize("txtBinUnlocked1");
			}
			else
			{
				bool flag2 = this.rejecterMainPath == Rejecter.rejecter2MainPath && a == "1";
				if (flag2)
				{
					text = LocalizationManager.Localize("txtBinUnlocked2");
				}
				else
				{
					text = LocalizationManager.Localize("txtBinUnlocked");
				}
			}
			return string.Concat(new string[]
			{
				text,
				": ",
				this.currentBinUnlockTime.ToString(),
				" ",
				LocalizationManager.Localize("txtSeconds")
			});
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000D3834 File Offset: 0x000D1A34
		public TrainViewModel(ParameterHandling parHandling, string rejecterMainPath, TrainMode trainMode, int binTimeout, MessageSystem messageSystem, Style okButtonStyle, Style cancelButtonStyle, MachineType machineType)
		{
			this.parHandling = parHandling;
			this.rejecterMainPath = rejecterMainPath;
			this.trainMode = trainMode;
			this.currentBinUnlockTime = binTimeout / 1000;
			this.messageSystem = messageSystem;
			this.okButtonStyle = okButtonStyle;
			this.cancelButtonStyle = cancelButtonStyle;
			this.machineType = machineType;
			int num = 0;
			string s = parHandling.ReadPar(Definition.ioVersionPath);
			int.TryParse(s, out num);
			bool flag = trainMode == TrainMode.UnlockBin && num > 25;
			if (flag)
			{
				this.currentBinUnlockTime = binTimeout;
			}
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			bool flag2 = trainMode == TrainMode.Verify || trainMode == TrainMode.VerifyHL;
			if (flag2)
			{
				string s2 = parHandling.ReadPar(rejecterMainPath + "/VerifyMissingObjects/Value");
				int.TryParse(s2, out this.nMissingOld);
				this.Title = "";
				this.Message = LocalizationManager.Localize("txtConveyContaminantProduct") + ".";
				this.StyleResourceKey = "DefaultStyle";
			}
			else
			{
				bool flag3 = trainMode == TrainMode.InOutDelay;
				if (flag3)
				{
					this.Title = "";
					this.Message = LocalizationManager.Localize("txtConveyProduct") + ".";
					this.StyleResourceKey = "DefaultStyle";
				}
				else
				{
					bool flag4 = trainMode == TrainMode.Air;
					if (flag4)
					{
						this.Title = "";
						this.Message = LocalizationManager.Localize("txtDisablingAirPressure") + ".";
						this.StyleResourceKey = "DefaultStyle";
					}
					else
					{
						this.Title = LocalizationManager.Localize("txtWarning");
						this.Message = LocalizationManager.Localize("txtBinIsOpen") + Environment.NewLine + Environment.NewLine + this.CreateBinUnlockMessage();
						this.StyleResourceKey = "WarningStyle";
					}
				}
			}
			this.buttonImage = ImageConverter.ToBitmap("/img/cancel.png");
			this.buttonStyle = cancelButtonStyle;
			this.timerUpdateTitle = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, new EventHandler(this.UpdateText), Application.Current.Dispatcher);
			this.timerUpdateTitle.Start();
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000D3A88 File Offset: 0x000D1C88
		private void CleanUp(object obj)
		{
			this.timerUpdateTitle.Stop();
			bool flag = this.trainMode == TrainMode.UnlockBin;
			if (flag)
			{
				this.LockBin();
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000D3AB8 File Offset: 0x000D1CB8
		private void UpdateText(object sender, EventArgs e)
		{
			bool flag = this.trainMode != TrainMode.UnlockBin;
			if (flag)
			{
				bool flag2 = this.Message.EndsWith("....");
				if (flag2)
				{
					this.Message = this.Message.Substring(0, this.Message.Length - 3);
				}
				else
				{
					this.Message += ".";
				}
				bool flag3 = (this.trainMode == TrainMode.Verify && this.parHandling.ReadPar(this.rejecterMainPath + "/VerifyTrain/Value") == "0") || (this.trainMode == TrainMode.InOutDelay && this.parHandling.ReadPar(this.rejecterMainPath + "/HLSCInOutDelayTrained/Value") != "0");
				if (flag3)
				{
					this.OnTrainFinshed();
				}
				bool flag4 = this.trainMode == TrainMode.VerifyHL;
				if (flag4)
				{
					bool flag5 = this.messageSystem != null;
					if (flag5)
					{
						List<Message> list = new List<Message>();
						list.AddRange(from message in this.messageSystem.Messages
						where message.LogbookEntry.EntryCode == SSTEntryCode.RejectVerify && ((message.LogbookEntry.Param1 == 1 && this.rejecterMainPath == Rejecter.rejecter1MainPath) || (message.LogbookEntry.Param1 == 2 && this.rejecterMainPath == Rejecter.rejecter2MainPath))
						select message);
						bool flag6 = list.Count > 0;
						if (flag6)
						{
							int num = 0;
							this.timerUpdateTitle.Stop();
							int.TryParse(list[0].LogbookEntry.Param2, out num);
							this.Message = string.Concat(new string[]
							{
								"Rejecter ",
								list[0].LogbookEntry.Param1.ToString(),
								" Verify: ",
								num.ToString(),
								" ",
								LocalizationManager.Localize("txtMissingProducts")
							});
							this.ButtonImage = ImageConverter.ToBitmap("/img/ok.png");
							this.ButtonStyle = this.okButtonStyle;
							this.RejectVerifyTriggered = true;
						}
					}
				}
				bool flag7 = this.trainMode == TrainMode.Air;
				if (flag7)
				{
					int num2 = 0;
					string s = this.parHandling.ReadPar(Definition.ioVersionPath);
					bool flag8 = int.TryParse(s, out num2);
					if (flag8)
					{
						bool flag9 = num2 < Definition.minNewWagoConfigVersion;
						if (flag9)
						{
							InOut inOut = ErrorSurvey.ioLowAirPressureOld;
						}
						else
						{
							bool flag10 = num2 < Definition.minSesotecIOProgramVersion;
							if (flag10)
							{
								InOut inOut = ErrorSurvey.ioLowAirPressureNew;
							}
							else
							{
								InOut inOut = ErrorSurvey.ioLowAirPressureSesotecIO;
							}
						}
						bool flag11;
						if (this.messageSystem != null)
						{
							flag11 = ((from message in this.messageSystem.Messages
							where message.LogbookEntry.EntryCode == SSTEntryCode.LowAirPressure
							select message).Count<Message>() > 0);
						}
						else
						{
							flag11 = false;
						}
						bool flag12 = flag11;
						if (flag12)
						{
							this.timerUpdateTitle.Stop();
							bool isDPlus1Module = this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1Module || this.parHandling.ReadPar(Definition.wagoMachineTypePath) == Definition.ioModuleIsRayconDPlus1ModuleHYRework;
							this.parHandling.WriteValue(Definition.GetTestAirPressurePath(this.machineType, isDPlus1Module), "0");
							this.Message = LocalizationManager.Localize("parItemLowAirPressure");
							this.ButtonImage = ImageConverter.ToBitmap("/img/ok.png");
							this.ButtonStyle = this.okButtonStyle;
							this.LowAirPressureTriggered = true;
						}
					}
				}
			}
			else
			{
				this.currentBinUnlockTime--;
				this.Message = LocalizationManager.Localize("txtBinIsOpen") + Environment.NewLine + Environment.NewLine + this.CreateBinUnlockMessage();
				bool flag13 = this.currentBinUnlockTime <= 1;
				if (flag13)
				{
					this.OnTrainFinshed();
				}
			}
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000D3E64 File Offset: 0x000D2064
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000D3E94 File Offset: 0x000D2094
		protected virtual void OnTrainFinshed()
		{
			Action trainFinshed = this.TrainFinshed;
			bool flag = trainFinshed != null;
			if (flag)
			{
				trainFinshed();
			}
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000D3EBA File Offset: 0x000D20BA
		public void LockBin()
		{
			this.parHandling.WriteValue(this.rejecterMainPath + "/HLBinUnlock/Value", "0", false);
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000D3EE0 File Offset: 0x000D20E0
		public void CommitLowAirPressure()
		{
			bool flag = this.messageSystem != null;
			if (flag)
			{
				this.messageSystem.RemoveRegisteredMessage(SSTEntryCode.LowAirPressure, 0);
			}
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000D3F0C File Offset: 0x000D210C
		public void CommitRejectVerify()
		{
			bool flag = this.messageSystem != null;
			if (flag)
			{
				this.messageSystem.RemoveRegisteredMessage(SSTEntryCode.RejectVerify, (this.rejecterMainPath == Rejecter.rejecter1MainPath) ? 1 : 2);
			}
		}

		// Token: 0x040014DD RID: 5341
		private DispatcherTimer timerUpdateTitle;

		// Token: 0x040014DE RID: 5342
		private ParameterHandling parHandling;

		// Token: 0x040014DF RID: 5343
		private Style messageStyle;

		// Token: 0x040014E0 RID: 5344
		private string title;

		// Token: 0x040014E1 RID: 5345
		private string message;

		// Token: 0x040014E2 RID: 5346
		private string rejecterMainPath;

		// Token: 0x040014E3 RID: 5347
		private TrainMode trainMode;

		// Token: 0x040014E4 RID: 5348
		private int currentBinUnlockTime;

		// Token: 0x040014E5 RID: 5349
		private int nMissingOld;

		// Token: 0x040014E6 RID: 5350
		private ImageSource buttonImage;

		// Token: 0x040014E7 RID: 5351
		private Style okButtonStyle;

		// Token: 0x040014E8 RID: 5352
		private Style cancelButtonStyle;

		// Token: 0x040014E9 RID: 5353
		private Style buttonStyle;

		// Token: 0x040014EA RID: 5354
		private MessageSystem messageSystem;

		// Token: 0x040014EB RID: 5355
		private MachineType machineType;

		// Token: 0x040014EC RID: 5356
		private string ___StyleResourceKey;
	}
}

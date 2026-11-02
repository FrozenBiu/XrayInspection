using System;
using System.ComponentModel;
using System.Diagnostics;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000125 RID: 293
	public class MachineParameterViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06001C28 RID: 7208 RVA: 0x000B56A8 File Offset: 0x000B38A8
		// (remove) Token: 0x06001C29 RID: 7209 RVA: 0x000B56E0 File Offset: 0x000B38E0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06001C2A RID: 7210 RVA: 0x000B5718 File Offset: 0x000B3918
		// (set) Token: 0x06001C2B RID: 7211 RVA: 0x000B5730 File Offset: 0x000B3930
		public bool ServiceMode
		{
			get
			{
				return this.serviceMode;
			}
			set
			{
				this.serviceMode = value;
				this.OnPropertyChanged("ServiceMode");
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06001C2C RID: 7212 RVA: 0x000B5748 File Offset: 0x000B3948
		// (set) Token: 0x06001C2D RID: 7213 RVA: 0x000B5760 File Offset: 0x000B3960
		public bool EditAllowed
		{
			get
			{
				return this.editAllowed;
			}
			set
			{
				this.editAllowed = value;
				this.OnPropertyChanged("EditAllowed");
			}
		}

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06001C2E RID: 7214 RVA: 0x000B5778 File Offset: 0x000B3978
		// (set) Token: 0x06001C2F RID: 7215 RVA: 0x000B5790 File Offset: 0x000B3990
		public string DeviceName
		{
			get
			{
				return this.deviceName;
			}
			set
			{
				this.deviceName = value;
				this.OnPropertyChanged("DeviceName");
			}
		}

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06001C30 RID: 7216 RVA: 0x000B57A8 File Offset: 0x000B39A8
		// (set) Token: 0x06001C31 RID: 7217 RVA: 0x000B57C0 File Offset: 0x000B39C0
		public string LineName
		{
			get
			{
				return this.lineName;
			}
			set
			{
				this.lineName = value;
				this.OnPropertyChanged("LineName");
			}
		}

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06001C32 RID: 7218 RVA: 0x000B57D8 File Offset: 0x000B39D8
		// (set) Token: 0x06001C33 RID: 7219 RVA: 0x000B57F0 File Offset: 0x000B39F0
		public string SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
			set
			{
				this.serialNumber = value;
				this.OnPropertyChanged("SerialNumber");
			}
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x000B5808 File Offset: 0x000B3A08
		// (set) Token: 0x06001C35 RID: 7221 RVA: 0x000B5820 File Offset: 0x000B3A20
		public string MachineType
		{
			get
			{
				return this.machineType;
			}
			set
			{
				this.machineType = value;
				this.OnPropertyChanged("MachineType");
			}
		}

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x000B5838 File Offset: 0x000B3A38
		// (set) Token: 0x06001C37 RID: 7223 RVA: 0x000B5850 File Offset: 0x000B3A50
		public ushort InspectionWidth
		{
			get
			{
				return this.inspectionWidth;
			}
			set
			{
				this.inspectionWidth = value;
				this.OnPropertyChanged("InspectionWidth");
			}
		}

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x000B5868 File Offset: 0x000B3A68
		// (set) Token: 0x06001C39 RID: 7225 RVA: 0x000B5880 File Offset: 0x000B3A80
		public ushort InspectionHeight
		{
			get
			{
				return this.inspectionHeight;
			}
			set
			{
				this.inspectionHeight = value;
				this.OnPropertyChanged("InspectionHeight");
			}
		}

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x000B5898 File Offset: 0x000B3A98
		// (set) Token: 0x06001C3B RID: 7227 RVA: 0x000B58B0 File Offset: 0x000B3AB0
		public string MachineSubType
		{
			get
			{
				return this.machineSubType;
			}
			set
			{
				this.machineSubType = value;
				this.OnPropertyChanged("MachineSubType");
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x000B58C8 File Offset: 0x000B3AC8
		// (set) Token: 0x06001C3D RID: 7229 RVA: 0x000B58E0 File Offset: 0x000B3AE0
		public string Location
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
				this.OnPropertyChanged("Location");
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x000B58F6 File Offset: 0x000B3AF6
		// (set) Token: 0x06001C3F RID: 7231 RVA: 0x000B58FE File Offset: 0x000B3AFE
		public BoundLocalizedText StrMachineName { get; set; }

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x000B5907 File Offset: 0x000B3B07
		// (set) Token: 0x06001C41 RID: 7233 RVA: 0x000B590F File Offset: 0x000B3B0F
		public BoundLocalizedText StrLineName { get; set; }

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x000B5918 File Offset: 0x000B3B18
		// (set) Token: 0x06001C43 RID: 7235 RVA: 0x000B5920 File Offset: 0x000B3B20
		public BoundLocalizedText StrSerialNumber { get; set; }

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06001C44 RID: 7236 RVA: 0x000B5929 File Offset: 0x000B3B29
		// (set) Token: 0x06001C45 RID: 7237 RVA: 0x000B5931 File Offset: 0x000B3B31
		public BoundLocalizedText StrMachineType { get; set; }

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x000B593A File Offset: 0x000B3B3A
		// (set) Token: 0x06001C47 RID: 7239 RVA: 0x000B5942 File Offset: 0x000B3B42
		public BoundLocalizedText StrMachineSubType { get; set; }

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x000B594B File Offset: 0x000B3B4B
		// (set) Token: 0x06001C49 RID: 7241 RVA: 0x000B5953 File Offset: 0x000B3B53
		public BoundLocalizedText StrLocation { get; set; }

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06001C4A RID: 7242 RVA: 0x000B595C File Offset: 0x000B3B5C
		// (set) Token: 0x06001C4B RID: 7243 RVA: 0x000B5964 File Offset: 0x000B3B64
		public RelayCommand SetMachineNameCommand { get; set; }

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x000B596D File Offset: 0x000B3B6D
		// (set) Token: 0x06001C4D RID: 7245 RVA: 0x000B5975 File Offset: 0x000B3B75
		public RelayCommand SetLineNameCommand { get; set; }

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x000B597E File Offset: 0x000B3B7E
		// (set) Token: 0x06001C4F RID: 7247 RVA: 0x000B5986 File Offset: 0x000B3B86
		public RelayCommand SetSerialNumberCommand { get; set; }

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x000B598F File Offset: 0x000B3B8F
		// (set) Token: 0x06001C51 RID: 7249 RVA: 0x000B5997 File Offset: 0x000B3B97
		public RelayCommand SetInspectionWidthCommand { get; set; }

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x000B59A0 File Offset: 0x000B3BA0
		// (set) Token: 0x06001C53 RID: 7251 RVA: 0x000B59A8 File Offset: 0x000B3BA8
		public RelayCommand SetInspectionHeightCommand { get; set; }

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06001C54 RID: 7252 RVA: 0x000B59B1 File Offset: 0x000B3BB1
		// (set) Token: 0x06001C55 RID: 7253 RVA: 0x000B59B9 File Offset: 0x000B3BB9
		public RelayCommand SetMachineSubTypeCommand { get; set; }

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06001C56 RID: 7254 RVA: 0x000B59C2 File Offset: 0x000B3BC2
		// (set) Token: 0x06001C57 RID: 7255 RVA: 0x000B59CA File Offset: 0x000B3BCA
		public RelayCommand SetLocationCommand { get; set; }

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x000B59D3 File Offset: 0x000B3BD3
		// (set) Token: 0x06001C59 RID: 7257 RVA: 0x000B59DB File Offset: 0x000B3BDB
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x06001C5A RID: 7258 RVA: 0x000B59E4 File Offset: 0x000B3BE4
		public MachineParameterViewModel(DBConnection dbConnection, AccessRightsManager accessManager, ParameterHandling parHandling)
		{
			this.dbConnection = dbConnection;
			this.accessManager = accessManager;
			this.parHandling = parHandling;
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.SetLineNameCommand = new RelayCommand(new Action<object>(this.SetLineName));
			this.SetMachineNameCommand = new RelayCommand(new Action<object>(this.SetMachineName));
			this.SetSerialNumberCommand = new RelayCommand(new Action<object>(this.SetSerialNumber));
			this.SetInspectionWidthCommand = new RelayCommand(new Action<object>(this.SetInspectionWidth));
			this.SetInspectionHeightCommand = new RelayCommand(new Action<object>(this.SetInspectionHeight));
			this.SetMachineSubTypeCommand = new RelayCommand(new Action<object>(this.SetMachineSubType));
			this.SetLocationCommand = new RelayCommand(new Action<object>(this.SetLocation));
			this.Initialize();
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x000B5AE0 File Offset: 0x000B3CE0
		private void Initialize()
		{
			try
			{
				MachineType machineType = Definitions.MachineType.MachineTypeUndefined;
				this.ServiceMode = (this.accessManager.CurrentUser.Rights >= BuiltInPriviledges.Service);
				this.EditAllowed = (this.accessManager.CurrentUser.Rights >= this.rightEditNames);
				this.dbConnection.GetDeviceName(ref this.deviceName);
				this.dbConnection.GetLineName(ref this.lineName);
				this.dbConnection.GetSerialNumber(ref this.serialNumber);
				this.dbConnection.GetMachineType(ref machineType);
				bool flag = this.dbConnection.GetDesign() == Definition.GUIDesign.Bizerba;
				if (flag)
				{
					bool flag2 = machineType == Definitions.MachineType.RayconXRS;
					if (flag2)
					{
						this.machineType = "XRE";
					}
					else
					{
						bool flag3 = machineType == Definitions.MachineType.RayconD;
						if (flag3)
						{
							this.machineType = "XRE-D";
						}
						else
						{
							bool flag4 = machineType == Definitions.MachineType.RayconEntry;
							if (flag4)
							{
								this.machineType = "XRE-D";
							}
							else
							{
								bool flag5 = machineType == Definitions.MachineType.RayconDPlus;
								if (flag5)
								{
									this.machineType = "XRE-D pro";
								}
								else
								{
									bool flag6 = machineType == Definitions.MachineType.RayconEX2A;
									if (flag6)
									{
										this.machineType = "Raycon EX2A";
									}
								}
							}
						}
					}
				}
				else
				{
					bool flag7 = machineType == Definitions.MachineType.RayconXRS;
					if (flag7)
					{
						this.machineType = "Raycon XRS";
					}
					else
					{
						bool flag8 = machineType == Definitions.MachineType.RayconD;
						if (flag8)
						{
							this.machineType = "Raycon D";
						}
						else
						{
							bool flag9 = machineType == Definitions.MachineType.RayconEntry;
							if (flag9)
							{
								this.machineType = "Raycon EX1";
							}
							else
							{
								bool flag10 = machineType == Definitions.MachineType.RayconDPlus;
								if (flag10)
								{
									this.machineType = "Raycon D+";
								}
								else
								{
									bool flag11 = machineType == Definitions.MachineType.RayconEX2A;
									if (flag11)
									{
										this.machineType = "Raycon EX2A";
									}
								}
							}
						}
					}
				}
				this.dbConnection.GetInspectionWidth(ref this.inspectionWidth);
				this.dbConnection.GetInspectionHeight(ref this.inspectionHeight);
				this.dbConnection.GetMachineSubType(ref this.machineSubType);
				this.dbConnection.GetLocation(ref this.location);
				this.InitializeLocalizedText();
			}
			catch
			{
			}
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x000B5CE4 File Offset: 0x000B3EE4
		private void InitializeLocalizedText()
		{
			this.StrMachineName = new BoundLocalizedText("txtMachineName");
			this.StrLineName = new BoundLocalizedText("txtLineName");
			this.StrSerialNumber = new BoundLocalizedText("txtSerialNumber");
			this.StrMachineType = new BoundLocalizedText("txtMachineType");
			this.StrMachineSubType = new BoundLocalizedText("txtMachineSubType");
			this.StrLocation = new BoundLocalizedText("txtLocation");
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000B5D58 File Offset: 0x000B3F58
		private void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrMachineName);
			MainWindow.localizationManager.Deregister(this.StrLineName);
			MainWindow.localizationManager.Deregister(this.StrSerialNumber);
			MainWindow.localizationManager.Deregister(this.StrMachineType);
			MainWindow.localizationManager.Deregister(this.StrMachineSubType);
			MainWindow.localizationManager.Deregister(this.StrLocation);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000B5DCC File Offset: 0x000B3FCC
		private void CleanUp(object obj)
		{
			this.DeregisterLocalizedText();
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000B5DD8 File Offset: 0x000B3FD8
		public void SetMachineName(object obj)
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("txtMachineName"), this.DeviceName, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_InputMachineName;
			this.keyboard.ShowDialog();
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000B5E28 File Offset: 0x000B4028
		private void keyboard_InputMachineName(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.DeviceName = e.Text;
				this.parHandling.WriteValue(Definition.machineNamePath, this.DeviceName);
			}
			catch
			{
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_InputMachineName;
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000B5E8C File Offset: 0x000B408C
		public void SetLineName(object obj)
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("txtLineName"), this.LineName, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_InputLineName;
			this.keyboard.ShowDialog();
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000B5EDC File Offset: 0x000B40DC
		private void keyboard_InputLineName(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.LineName = e.Text;
				this.parHandling.WriteValue(Definition.lineNamePath, this.LineName);
			}
			catch
			{
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_InputLineName;
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000B5F40 File Offset: 0x000B4140
		public void SetSerialNumber(object obj)
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("txtSerialNumber"), this.SerialNumber, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_InputSerialNumber;
			this.keyboard.ShowDialog();
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000B5F90 File Offset: 0x000B4190
		private void keyboard_InputSerialNumber(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.SerialNumber = e.Text;
				this.parHandling.WriteValue(Definition.machineSerialNumberPath, this.SerialNumber);
			}
			catch
			{
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_InputSerialNumber;
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000B5FF4 File Offset: 0x000B41F4
		public void SetInspectionWidth(object obj)
		{
			this.numKeyboard = new NumKeyboard("Inspection Width", this.InspectionWidth.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_InputInspectionWidth;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000B6048 File Offset: 0x000B4248
		private void keyboard_InputInspectionWidth(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.InspectionWidth = ushort.Parse(e.Text);
				this.parHandling.WriteValue(Definition.inspectionWidthPath, this.InspectionWidth.ToString());
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_InputInspectionWidth;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000B60B8 File Offset: 0x000B42B8
		public void SetInspectionHeight(object obj)
		{
			this.numKeyboard = new NumKeyboard("Inspection Height", this.InspectionHeight.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.keyboard_InputInspectionHeight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000B610C File Offset: 0x000B430C
		private void keyboard_InputInspectionHeight(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.InspectionHeight = ushort.Parse(e.Text);
				this.parHandling.WriteValue(Definition.inspectionHeightPath, this.InspectionHeight.ToString());
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.keyboard_InputInspectionHeight;
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000B617C File Offset: 0x000B437C
		public void SetMachineSubType(object obj)
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("txtMachineSubType"), this.MachineSubType, KeyboardType.Application);
			this.keyboard.TextInputKeyboard += this.keyboard_InputMachineSubType;
			this.keyboard.ShowDialog();
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000B61CC File Offset: 0x000B43CC
		private void keyboard_InputMachineSubType(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.MachineSubType = e.Text;
				this.parHandling.WriteValue(Definition.machineSubTypePath, this.MachineSubType);
			}
			catch
			{
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_InputMachineSubType;
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000B6230 File Offset: 0x000B4430
		public void SetLocation(object obj)
		{
			this.keyboard = new Keyboard(LocalizationManager.Localize("txtLocation"), this.Location, KeyboardType.Application);
			this.keyboard.HasEnterButton = true;
			this.keyboard.MaxTextLength = 100;
			this.keyboard.TextInputKeyboard += this.keyboard_InputLocation;
			this.keyboard.ShowDialog();
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000B629C File Offset: 0x000B449C
		private void keyboard_InputLocation(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				this.Location = e.Text;
				this.parHandling.WriteValue(Definition.machinelocationPath, this.Location);
			}
			catch
			{
			}
			this.keyboard.TextInputKeyboard -= this.keyboard_InputLocation;
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000B6300 File Offset: 0x000B4500
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x040012AA RID: 4778
		private BuiltInPriviledges rightEditNames = BuiltInPriviledges.Admin;

		// Token: 0x040012AB RID: 4779
		private DBConnection dbConnection;

		// Token: 0x040012AC RID: 4780
		private AccessRightsManager accessManager;

		// Token: 0x040012AD RID: 4781
		private ParameterHandling parHandling;

		// Token: 0x040012AE RID: 4782
		private Keyboard keyboard;

		// Token: 0x040012AF RID: 4783
		private NumKeyboard numKeyboard;

		// Token: 0x040012B0 RID: 4784
		private bool serviceMode;

		// Token: 0x040012B1 RID: 4785
		private string lineName;

		// Token: 0x040012B2 RID: 4786
		private string serialNumber;

		// Token: 0x040012B3 RID: 4787
		private string deviceName;

		// Token: 0x040012B4 RID: 4788
		private bool editAllowed;

		// Token: 0x040012B5 RID: 4789
		private string machineType;

		// Token: 0x040012B6 RID: 4790
		private ushort inspectionWidth;

		// Token: 0x040012B7 RID: 4791
		private ushort inspectionHeight;

		// Token: 0x040012B8 RID: 4792
		private string machineSubType;

		// Token: 0x040012B9 RID: 4793
		private string location;
	}
}

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000159 RID: 345
	public class MoveSourceViewModel : INotifyPropertyChanged
	{
		// Token: 0x140000AC RID: 172
		// (add) Token: 0x060020B5 RID: 8373 RVA: 0x000D55B8 File Offset: 0x000D37B8
		// (remove) Token: 0x060020B6 RID: 8374 RVA: 0x000D55F0 File Offset: 0x000D37F0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x140000AD RID: 173
		// (add) Token: 0x060020B7 RID: 8375 RVA: 0x000D5628 File Offset: 0x000D3828
		// (remove) Token: 0x060020B8 RID: 8376 RVA: 0x000D5660 File Offset: 0x000D3860
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool> SourceMoveEnded;

		// Token: 0x17000977 RID: 2423
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x000D5698 File Offset: 0x000D3898
		// (set) Token: 0x060020BA RID: 8378 RVA: 0x000D56B0 File Offset: 0x000D38B0
		public bool ReadonlyMode
		{
			get
			{
				return this.readonlyMode;
			}
			set
			{
				bool flag = this.readonlyMode != value;
				if (flag)
				{
					this.readonlyMode = value;
					this.OnPropertyChanged("ReadonlyMode");
					this.OnPropertyChanged("WritableHeight");
				}
			}
		}

		// Token: 0x17000978 RID: 2424
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x000D56F0 File Offset: 0x000D38F0
		public bool WritableHeight
		{
			get
			{
				return !this.readonlyMode;
			}
		}

		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x060020BC RID: 8380 RVA: 0x000D570C File Offset: 0x000D390C
		// (set) Token: 0x060020BD RID: 8381 RVA: 0x000D5724 File Offset: 0x000D3924
		public int SourcePosition
		{
			get
			{
				return this.sourcePosition;
			}
			set
			{
				bool flag = this.sourcePosition != value;
				if (flag)
				{
					this.sourcePosition = value;
					this.OnPropertyChanged("SourcePosition");
					this.OnPropertyChanged("ProductHeight");
					this.OnPropertyChanged("ProductHeightInScreenPx");
					this.OnPropertyChanged("XRayMargin");
					this.RecalculateTrianglePoints();
				}
			}
		}

		// Token: 0x1700097A RID: 2426
		// (get) Token: 0x060020BE RID: 8382 RVA: 0x000D5784 File Offset: 0x000D3984
		public double ProductHeight
		{
			get
			{
				return this.lengthMetric ? this.GetPositionInMM(this.sourcePosition) : this.GetPositionInInches(this.sourcePosition);
			}
		}

		// Token: 0x1700097B RID: 2427
		// (get) Token: 0x060020BF RID: 8383 RVA: 0x000D57B8 File Offset: 0x000D39B8
		public double ProductHeightInScreenPx
		{
			get
			{
				return this.lengthMetric ? this.ProductHeight : (this.ProductHeight / Definition.millimeterToInchFactor);
			}
		}

		// Token: 0x1700097C RID: 2428
		// (get) Token: 0x060020C0 RID: 8384 RVA: 0x000D57E8 File Offset: 0x000D39E8
		public double MinHeight
		{
			get
			{
				return this.minHeight;
			}
		}

		// Token: 0x1700097D RID: 2429
		// (get) Token: 0x060020C1 RID: 8385 RVA: 0x000D5800 File Offset: 0x000D3A00
		public double MaxHeight
		{
			get
			{
				return this.maxHeight;
			}
		}

		// Token: 0x1700097E RID: 2430
		// (get) Token: 0x060020C2 RID: 8386 RVA: 0x000D5818 File Offset: 0x000D3A18
		public Thickness XRayMargin
		{
			get
			{
				return new Thickness(100.0, 0.0, 0.0, this.ProductHeightInScreenPx + 100.0);
			}
		}

		// Token: 0x1700097F RID: 2431
		// (get) Token: 0x060020C3 RID: 8387 RVA: 0x000D585A File Offset: 0x000D3A5A
		// (set) Token: 0x060020C4 RID: 8388 RVA: 0x000D5862 File Offset: 0x000D3A62
		public bool SourceMoved { get; set; }

		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x060020C5 RID: 8389 RVA: 0x000D586C File Offset: 0x000D3A6C
		public ObservableCollection<Point> TrianglePoints
		{
			get
			{
				return this.trianglePoints;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x060020C6 RID: 8390 RVA: 0x000D5884 File Offset: 0x000D3A84
		// (set) Token: 0x060020C7 RID: 8391 RVA: 0x000D589C File Offset: 0x000D3A9C
		public bool LengthMetric
		{
			get
			{
				return this.lengthMetric;
			}
			set
			{
				this.lengthMetric = value;
				bool flag = this.lengthMetric;
				if (flag)
				{
					this.dbConnection.SetLengthMetric(true);
					this.LengthAbbr = "mm";
				}
				else
				{
					this.dbConnection.SetLengthMetric(false);
					this.LengthAbbr = "in";
				}
				this.SetMinAndMaxHeight();
				this.OnPropertyChanged("LengthMetric");
				this.OnPropertyChanged("ProductHeight");
				this.OnPropertyChanged("ProductHeightInScreenPx");
				this.RecalculateTrianglePoints();
			}
		}

		// Token: 0x17000982 RID: 2434
		// (get) Token: 0x060020C8 RID: 8392 RVA: 0x000D5928 File Offset: 0x000D3B28
		public bool LengthAmerican
		{
			get
			{
				return !this.lengthMetric;
			}
		}

		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x060020C9 RID: 8393 RVA: 0x000D5944 File Offset: 0x000D3B44
		// (set) Token: 0x060020CA RID: 8394 RVA: 0x000D595C File Offset: 0x000D3B5C
		public string LengthAbbr
		{
			get
			{
				return this.lengthAbbr;
			}
			set
			{
				this.lengthAbbr = value;
				this.OnPropertyChanged("LengthAbbr");
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x060020CB RID: 8395 RVA: 0x000D5974 File Offset: 0x000D3B74
		// (set) Token: 0x060020CC RID: 8396 RVA: 0x000D598C File Offset: 0x000D3B8C
		public bool LengthUnitEnabled
		{
			get
			{
				return this.lengthUnitEnabled;
			}
			set
			{
				this.lengthUnitEnabled = value;
				this.OnPropertyChanged("LengthUnitEnabled");
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x060020CD RID: 8397 RVA: 0x000D59A2 File Offset: 0x000D3BA2
		// (set) Token: 0x060020CE RID: 8398 RVA: 0x000D59AA File Offset: 0x000D3BAA
		public RelayCommand SetProductHeightCommand { get; set; }

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x060020CF RID: 8399 RVA: 0x000D59B3 File Offset: 0x000D3BB3
		// (set) Token: 0x060020D0 RID: 8400 RVA: 0x000D59BB File Offset: 0x000D3BBB
		public BoundLocalizedText StrProductHeight { get; set; }

		// Token: 0x060020D1 RID: 8401 RVA: 0x000D59C4 File Offset: 0x000D3BC4
		public MoveSourceViewModel(DBConnection dbConnection, ParameterHandling parHandling, MachineState machineState, AccessRightsManager accessManager) : this(dbConnection, parHandling, machineState, accessManager, false)
		{
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000D59D4 File Offset: 0x000D3BD4
		public MoveSourceViewModel(DBConnection dbConnection, ParameterHandling parHandling, MachineState machineState, AccessRightsManager accessManager, bool readonlyMode)
		{
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.readonlyMode = readonlyMode;
			this.machineState = machineState;
			this.accessManager = accessManager;
			bool flag = machineState != null;
			if (flag)
			{
				this.oldState = machineState.State;
				machineState.State = MachineStates.MovingSource;
			}
			this.Initialize();
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000D5A5C File Offset: 0x000D3C5C
		private void Initialize()
		{
			string s = "";
			int num = 0;
			DetectorType detectorType = DetectorType.DT;
			this.GetLengthUnit();
			this.LengthUnitEnabled = (this.accessManager != null && this.accessManager.CurrentUser.Rights >= this.rightChangeLengthUnit);
			this.SetMinAndMaxHeight();
			this.trianglePoints = new ObservableCollection<Point>();
			Sideshooter.SetXRayPositioningParameters(this.dbConnection, this.parHandling, this.readonlyMode);
			bool flag = !this.readonlyMode;
			if (flag)
			{
				this.dbConnection.GetParameterValue("SShXRayPosition", ref s);
			}
			else
			{
				this.parHandling.WriteValue(Definition.sshXrayActualPositionPath, "1", false);
				Thread.Sleep(500);
				s = this.parHandling.ReadPar(Definition.sshXrayActualPositionPath);
			}
			try
			{
				this.SourcePosition = int.Parse(s);
			}
			catch
			{
			}
			this.StrProductHeight = new BoundLocalizedText("txtProductHeight");
			this.SetProductHeightCommand = new RelayCommand(new Action<object>(this.SetProductHeight));
			this.parHandling.WriteValue(Definition.sshActualPositionPath, "1", false);
			bool flag2 = this.parHandling.ReadPar(Conveyor.detectorDTPixelSizePath) == Conveyor.detectorDT04Identifier;
			if (flag2)
			{
				this.pixelSize = 0.4;
			}
			else
			{
				this.pixelSize = 0.8;
			}
			int num2 = 0;
			this.dbConnection.GetSshPixelTolerance(ref num2);
			this.dbConnection.GetDetectorType(ref detectorType);
			string s2 = (detectorType == DetectorType.DT || detectorType == DetectorType.DTNetSE || detectorType == DetectorType.DTNetDE || detectorType == DetectorType.DTNetTDI) ? this.parHandling.ReadPar(Definition.detectorDTNPixelPath) : this.parHandling.ReadPar(Definition.detectorSesotecNPixelPath);
			int.TryParse(s2, out num);
			bool flag3 = num > 0;
			if (flag3)
			{
				this.nPixelWithTolerance = num - num2;
			}
			this.RecalculateTrianglePoints();
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000D5C50 File Offset: 0x000D3E50
		private void SetMinAndMaxHeight()
		{
			int num = 0;
			int num2 = 0;
			this.dbConnection.GetSshMinHeight(ref num);
			this.dbConnection.GetSshMaxHeight(ref num2);
			this.minHeight = (this.lengthMetric ? ((double)num) : Math.Round((double)num * Definition.millimeterToInchFactor, 2, MidpointRounding.AwayFromZero));
			this.maxHeight = (this.lengthMetric ? ((double)num2) : Math.Round((double)num2 * Definition.millimeterToInchFactor, 2, MidpointRounding.AwayFromZero));
			while (Math.Round((this.minHeight - Math.Truncate(this.minHeight)) * 100.0, 2) % 4.0 != 0.0)
			{
				this.minHeight = Math.Round(this.minHeight - 0.01, 2);
			}
			while (Math.Round((this.maxHeight - Math.Truncate(this.maxHeight)) * 100.0, 2) % 4.0 != 0.0)
			{
				double num3 = (this.maxHeight - Math.Truncate(this.maxHeight)) * 100.0;
				this.maxHeight = Math.Round(this.maxHeight - 0.01, 2);
			}
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000D5D9C File Offset: 0x000D3F9C
		private double GetPositionInInches(int positionInPixel)
		{
			double num = (positionInPixel > 0) ? (((double)positionInPixel + this.maxHeight * (1.0 / (this.pixelSize * Definition.millimeterToInchFactor)) - ((double)this.nPixelWithTolerance - 1.0)) * (this.pixelSize * Definition.millimeterToInchFactor)) : 0.0;
			num = (double)((int)Math.Ceiling(num * 100.0));
			double num2 = num / 100.0;
			bool flag = num2 < this.minHeight;
			if (flag)
			{
				num2 = this.minHeight;
			}
			return num2;
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000D5E48 File Offset: 0x000D4048
		private double GetPositionInMM(int positionInPixel)
		{
			return (positionInPixel > 0) ? Math.Ceiling(((double)positionInPixel + this.maxHeight * (1.0 / this.pixelSize) - ((double)this.nPixelWithTolerance - 1.0)) * this.pixelSize) : 0.0;
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000D5EA4 File Offset: 0x000D40A4
		private int GetPositionInPixel(double positionInMMOrInches)
		{
			bool flag = this.lengthMetric;
			int result;
			if (flag)
			{
				result = (int)(positionInMMOrInches * (1.0 / this.pixelSize) + (double)this.nPixelWithTolerance - 1.0 - this.maxHeight * (1.0 / this.pixelSize));
			}
			else
			{
				result = (int)(positionInMMOrInches * (1.0 / (this.pixelSize * Definition.millimeterToInchFactor)) + (double)this.nPixelWithTolerance - 1.0 - this.maxHeight * (1.0 / (this.pixelSize * Definition.millimeterToInchFactor)));
			}
			return result;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000D5F50 File Offset: 0x000D4150
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000D5F80 File Offset: 0x000D4180
		private void SetProductHeight(object obj)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtProductHeight"), this.ProductHeight.ToString(), this.lengthMetric ? NumKeyboardType.TypeUInt : NumKeyboardType.TypeDouble);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetProductHeight;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000D5FE4 File Offset: 0x000D41E4
		private void numKeyboard_SetProductHeight(object sender, KeyboardInputEventArgs e)
		{
			try
			{
				double num = double.Parse(e.Text, new CultureInfo("de-DE"));
				bool flag = num < this.minHeight;
				if (flag)
				{
					string str = this.lengthMetric ? (this.minHeight.ToString() + " mm") : (this.minHeight.ToString("F2") + " in");
					Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtMinProductHeight") + ": " + str, DialogStyle.Info, DialogButtons.OK);
				}
				else
				{
					bool flag2 = num > this.maxHeight;
					if (flag2)
					{
						string str2 = this.lengthMetric ? (this.maxHeight.ToString() + " mm") : (this.maxHeight.ToString("F2") + " in");
						Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtMaxProductHeight") + ": " + str2, DialogStyle.Info, DialogButtons.OK);
					}
					else
					{
						this.SourceMoved = true;
						this.MoveSource(this.GetPositionInPixel(num), true);
					}
				}
			}
			catch
			{
			}
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetProductHeight;
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000D6154 File Offset: 0x000D4354
		public void MoveSource(int positionInPixel, bool updateTable)
		{
			this.parHandling.WriteValue(Definition.sshXrayMotorSetpointPath, positionInPixel.ToString(), false);
			if (updateTable)
			{
				this.dbConnection.UpdateCurrentProductTable("SShXRayPosition", positionInPixel.ToString());
			}
			this.StartMoveSource();
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000D61A0 File Offset: 0x000D43A0
		public void CleanUp()
		{
			MainWindow.localizationManager.Deregister(this.StrProductHeight);
			Sideshooter.ResetXRayPositioningParameters(this.dbConnection, this.parHandling, this.readonlyMode);
			bool flag = this.machineState != null && this.machineState.State == MachineStates.MovingSource && (this.readonlyMode || !this.SourceMoved);
			if (flag)
			{
				this.machineState.State = this.oldState;
			}
			bool flag2 = !this.readonlyMode && this.SourceMoved;
			if (flag2)
			{
				this.parHandling.WriteValue("SShXRayPosition", this.sourcePosition.ToString());
			}
			bool flag3 = this.halfTransparent != null;
			if (flag3)
			{
				this.halfTransparent.Close();
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000D6268 File Offset: 0x000D4468
		private void StartMoveSource()
		{
			this.halfTransparent = new WdHalfTransparent(new TimeSpan(0L));
			this.halfTransparent.Show();
			new Thread(new ThreadStart(this.UpdateSourcePosition))
			{
				Name = "UpdateSourcePosition"
			}.Start();
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000D62BC File Offset: 0x000D44BC
		private void UpdateSourcePosition()
		{
			int actualPosition = 0;
			int setpoint = 0;
			int minPositionInPixel = 0;
			bool success = false;
			int num = 90;
			this.dbConnection.GetSshMaxMoveTimeXRayMotor(ref num);
			this.dbConnection.SetSshSourcePosNeeded(true);
			minPositionInPixel = this.GetPositionInPixel(this.minHeight);
			Stopwatch stopwatch = Stopwatch.StartNew();
			while (this.parHandling.ReadPar(Definition.sshXrayMotorEnablePath) == "1")
			{
				string s = this.parHandling.ReadPar(Definition.sshActualPositionPath);
				int.TryParse(s, out actualPosition);
				bool flag = Application.Current != null;
				if (flag)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
					{
						bool flag6 = actualPosition >= minPositionInPixel;
						if (flag6)
						{
							this.SourcePosition = actualPosition;
						}
					}));
				}
				Thread.Sleep(300);
				bool flag2 = stopwatch.ElapsedMilliseconds > (long)(num * 1000);
				if (flag2)
				{
					break;
				}
			}
			stopwatch.Stop();
			string s2 = this.parHandling.ReadPar(Definition.sshXrayMotorSetpointPath);
			int.TryParse(s2, out setpoint);
			bool flag3 = Math.Abs(setpoint - actualPosition) <= 5;
			if (flag3)
			{
				bool flag4 = Application.Current != null;
				if (flag4)
				{
					Application.Current.Dispatcher.Invoke(DispatcherPriority.Render, new Action(delegate()
					{
						success = true;
						this.SourcePosition = setpoint;
					}));
				}
			}
			bool flag5 = Application.Current != null;
			if (flag5)
			{
				Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Render, new Action(delegate()
				{
					bool flag6 = this.halfTransparent != null;
					if (flag6)
					{
						this.halfTransparent.Close();
					}
					this.halfTransparent = null;
				}));
			}
			this.SaveIgnorePixels();
			bool success2 = success;
			if (success2)
			{
				this.dbConnection.SetSshSourcePosNeeded(false);
			}
			this.OnSourceMoveEnded(success);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000D649C File Offset: 0x000D469C
		private void SaveIgnorePixels()
		{
			int num = 0;
			string s = this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath);
			int.TryParse(s, out num);
			this.dbConnection.SetIgnoredLeftPixel(num);
			s = this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath);
			int.TryParse(s, out num);
			this.dbConnection.SetIgnoredRightPixel(num);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000D6500 File Offset: 0x000D4700
		private void RecalculateTrianglePoints()
		{
			bool flag = this.trianglePoints != null;
			if (flag)
			{
				int num = (int)this.ProductHeightInScreenPx;
				this.trianglePoints.Clear();
				this.trianglePoints.Add(new Point(0.0, (double)(150 - num)));
				this.trianglePoints.Add(new Point(-250.0, (double)(150 - num)));
				this.trianglePoints.Add(new Point(-250.0, 260.0));
				this.OnPropertyChanged("TrianglePoints");
			}
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000D65A8 File Offset: 0x000D47A8
		protected virtual void OnSourceMoveEnded(bool success)
		{
			Action<bool> sourceMoveEnded = this.SourceMoveEnded;
			bool flag = sourceMoveEnded != null;
			if (flag)
			{
				sourceMoveEnded(success);
			}
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000D65D0 File Offset: 0x000D47D0
		private void GetLengthUnit()
		{
			bool flag = true;
			this.dbConnection.GetLengthMetric(ref flag);
			this.LengthMetric = flag;
		}

		// Token: 0x04001538 RID: 5432
		private BuiltInPriviledges rightChangeLengthUnit = BuiltInPriviledges.Admin;

		// Token: 0x04001539 RID: 5433
		private const int maxDelta = 5;

		// Token: 0x0400153A RID: 5434
		private DBConnection dbConnection;

		// Token: 0x0400153B RID: 5435
		private ParameterHandling parHandling;

		// Token: 0x0400153C RID: 5436
		private int sourcePosition;

		// Token: 0x0400153D RID: 5437
		private NumKeyboard numKeyboard;

		// Token: 0x0400153E RID: 5438
		private double minHeight;

		// Token: 0x0400153F RID: 5439
		private double maxHeight;

		// Token: 0x04001540 RID: 5440
		private WdHalfTransparent halfTransparent;

		// Token: 0x04001541 RID: 5441
		private double pixelSize = 0.8;

		// Token: 0x04001542 RID: 5442
		private int nPixelWithTolerance = 512;

		// Token: 0x04001543 RID: 5443
		private ObservableCollection<Point> trianglePoints;

		// Token: 0x04001544 RID: 5444
		private bool readonlyMode;

		// Token: 0x04001545 RID: 5445
		private MachineStates oldState;

		// Token: 0x04001546 RID: 5446
		private MachineState machineState;

		// Token: 0x04001547 RID: 5447
		private AccessRightsManager accessManager;

		// Token: 0x04001548 RID: 5448
		private bool lengthUnitEnabled;

		// Token: 0x04001549 RID: 5449
		private bool lengthMetric;

		// Token: 0x0400154A RID: 5450
		private string lengthAbbr;
	}
}

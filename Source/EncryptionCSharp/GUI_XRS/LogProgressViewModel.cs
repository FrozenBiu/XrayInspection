using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Common;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000123 RID: 291
	public class LogProgressViewModel : INotifyPropertyChanged
	{
		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06001C0C RID: 7180 RVA: 0x000B51FB File Offset: 0x000B33FB
		// (set) Token: 0x06001C0D RID: 7181 RVA: 0x000B5203 File Offset: 0x000B3403
		public ObservableCollection<MCULogEntry> LogEntries { get; set; }

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06001C0E RID: 7182 RVA: 0x000B520C File Offset: 0x000B340C
		// (set) Token: 0x06001C0F RID: 7183 RVA: 0x000B5214 File Offset: 0x000B3414
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06001C10 RID: 7184 RVA: 0x000B5220 File Offset: 0x000B3420
		// (set) Token: 0x06001C11 RID: 7185 RVA: 0x000B5238 File Offset: 0x000B3438
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

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06001C12 RID: 7186 RVA: 0x000B524E File Offset: 0x000B344E
		// (set) Token: 0x06001C13 RID: 7187 RVA: 0x000B5256 File Offset: 0x000B3456
		public WindowState WState { get; set; }

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06001C14 RID: 7188 RVA: 0x000B5260 File Offset: 0x000B3460
		// (set) Token: 0x06001C15 RID: 7189 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06001C16 RID: 7190 RVA: 0x000B5278 File Offset: 0x000B3478
		// (set) Token: 0x06001C17 RID: 7191 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06001C18 RID: 7192 RVA: 0x000B5290 File Offset: 0x000B3490
		// (set) Token: 0x06001C19 RID: 7193 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x000B52A8 File Offset: 0x000B34A8
		// (set) Token: 0x06001C1B RID: 7195 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06001C1C RID: 7196 RVA: 0x000B52C0 File Offset: 0x000B34C0
		// (remove) Token: 0x06001C1D RID: 7197 RVA: 0x000B52F8 File Offset: 0x000B34F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06001C1E RID: 7198 RVA: 0x000B5330 File Offset: 0x000B3530
		public LogProgressViewModel(ParameterHandling parHandling, MeanPixelLogger meanPixelLogger, bool offsetMode)
		{
			this.parHandling = parHandling;
			this.meanPixelLogger = meanPixelLogger;
			this.offsetMode = offsetMode;
			this.SetImageProcessingParameters();
			meanPixelLogger.OffsetValuesSaved += this.meanPixelLogger_OffsetValuesSaved;
			meanPixelLogger.GainValuesSaved += this.meanPixelLogger_GainValuesSaved;
			this.LogEntries = new ObservableCollection<MCULogEntry>();
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			this.Title = (offsetMode ? "Logging offset..." : "Logging gain...");
			this.timerUpdateTitle = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Background, new EventHandler(this.UpdateTitle), Application.Current.Dispatcher);
			this.timerUpdateTitle.Start();
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x000B5424 File Offset: 0x000B3624
		private void CleanUp(object obj)
		{
			this.timerUpdateTitle.Stop();
			this.meanPixelLogger.OffsetValuesSaved -= this.meanPixelLogger_OffsetValuesSaved;
			this.meanPixelLogger.GainValuesSaved -= this.meanPixelLogger_GainValuesSaved;
			this.parHandling.WriteValue(Definition.recordLinesMeanValuePath, "0");
			bool flag = !this.offsetMode;
			if (flag)
			{
				this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "1", false);
			}
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x000B54AC File Offset: 0x000B36AC
		private void meanPixelLogger_GainValuesSaved(GainCalibrationInfo info)
		{
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.LogEntries.Add(new MCULogEntry(info.Temperature, info.Scanrate, info.VoltageSetpoint, info.CurrentSetpoint, true));
			}));
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x000B54EC File Offset: 0x000B36EC
		private void meanPixelLogger_OffsetValuesSaved(int temperature)
		{
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
			{
				this.LogEntries.Add(new MCULogEntry(temperature));
			}));
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x000B552C File Offset: 0x000B372C
		private void UpdateTitle(object sender, EventArgs e)
		{
			bool flag = this.Title.EndsWith("....");
			if (flag)
			{
				this.Title = this.Title.Substring(0, this.Title.Length - 3);
			}
			else
			{
				this.Title += ".";
			}
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x000B5588 File Offset: 0x000B3788
		private void SetImageProcessingParameters()
		{
			this.parHandling.WriteValue(XRayOnProcessing.dtBaselineEnablePath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.dtGainEnablePath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.dtOffsetEnablePath, "0", false);
			this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, "0", false);
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x000B560C File Offset: 0x000B380C
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x0400129F RID: 4767
		private string title;

		// Token: 0x040012A0 RID: 4768
		private MeanPixelLogger meanPixelLogger;

		// Token: 0x040012A1 RID: 4769
		private DispatcherTimer timerUpdateTitle;

		// Token: 0x040012A2 RID: 4770
		private ParameterHandling parHandling;

		// Token: 0x040012A6 RID: 4774
		private bool offsetMode;
	}
}

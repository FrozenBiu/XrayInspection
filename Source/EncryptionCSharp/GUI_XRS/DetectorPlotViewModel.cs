using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Graph;
using SNRPlotLibrary;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000117 RID: 279
	public class DetectorPlotViewModel : INotifyPropertyChanged, IImageArrived, IPlotDetailsArrived
	{
		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06001AA1 RID: 6817 RVA: 0x0009FB1C File Offset: 0x0009DD1C
		// (remove) Token: 0x06001AA2 RID: 6818 RVA: 0x0009FB54 File Offset: 0x0009DD54
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06001AA3 RID: 6819 RVA: 0x0009FB8C File Offset: 0x0009DD8C
		// (set) Token: 0x06001AA4 RID: 6820 RVA: 0x0009FBA4 File Offset: 0x0009DDA4
		public double Voltage
		{
			get
			{
				return this.voltage;
			}
			set
			{
				this.voltage = value;
				this.OnPropertyChanged("Voltage");
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06001AA5 RID: 6821 RVA: 0x0009FBBC File Offset: 0x0009DDBC
		// (set) Token: 0x06001AA6 RID: 6822 RVA: 0x0009FBD4 File Offset: 0x0009DDD4
		public double Current
		{
			get
			{
				return this.current;
			}
			set
			{
				this.current = value;
				this.OnPropertyChanged("Current");
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06001AA7 RID: 6823 RVA: 0x0009FBEC File Offset: 0x0009DDEC
		// (set) Token: 0x06001AA8 RID: 6824 RVA: 0x0009FC04 File Offset: 0x0009DE04
		public double DetectorTemperature
		{
			get
			{
				return this.detectorTemperature;
			}
			set
			{
				this.detectorTemperature = value;
				this.OnPropertyChanged("DetectorTemperature");
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06001AA9 RID: 6825 RVA: 0x0009FC1C File Offset: 0x0009DE1C
		// (set) Token: 0x06001AAA RID: 6826 RVA: 0x0009FC34 File Offset: 0x0009DE34
		public bool BaselineEnable
		{
			get
			{
				return this.baselineEnable;
			}
			set
			{
				this.baselineEnable = value;
				this.OnPropertyChanged("BaselineEnable");
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06001AAB RID: 6827 RVA: 0x0009FC4C File Offset: 0x0009DE4C
		// (set) Token: 0x06001AAC RID: 6828 RVA: 0x0009FC64 File Offset: 0x0009DE64
		public bool OffsetEnable
		{
			get
			{
				return this.offsetEnable;
			}
			set
			{
				this.offsetEnable = value;
				this.OnPropertyChanged("OffsetEnable");
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06001AAD RID: 6829 RVA: 0x0009FC7C File Offset: 0x0009DE7C
		// (set) Token: 0x06001AAE RID: 6830 RVA: 0x0009FC94 File Offset: 0x0009DE94
		public bool GainEnable
		{
			get
			{
				return this.gainEnable;
			}
			set
			{
				this.gainEnable = value;
				this.OnPropertyChanged("GainEnable");
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x0009FCAC File Offset: 0x0009DEAC
		// (set) Token: 0x06001AB0 RID: 6832 RVA: 0x0009FCC4 File Offset: 0x0009DEC4
		public bool XRayEnable
		{
			get
			{
				return this.xRayEnable;
			}
			set
			{
				this.xRayEnable = value;
				this.OnPropertyChanged("XRayEnable");
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x0009FCDC File Offset: 0x0009DEDC
		// (set) Token: 0x06001AB2 RID: 6834 RVA: 0x0009FCF4 File Offset: 0x0009DEF4
		public int IgnoredLeftPixel
		{
			get
			{
				return this.ignoredLeftPixel;
			}
			set
			{
				this.ignoredLeftPixel = value;
				this.OnPropertyChanged("IgnoredLeftPixel");
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x0009FD0C File Offset: 0x0009DF0C
		// (set) Token: 0x06001AB4 RID: 6836 RVA: 0x0009FD24 File Offset: 0x0009DF24
		public int IgnoredRightPixel
		{
			get
			{
				return this.ignoredRightPixel;
			}
			set
			{
				this.ignoredRightPixel = value;
				this.OnPropertyChanged("IgnoredRightPixel");
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06001AB5 RID: 6837 RVA: 0x0009FD3C File Offset: 0x0009DF3C
		// (set) Token: 0x06001AB6 RID: 6838 RVA: 0x0009FD54 File Offset: 0x0009DF54
		public int IgnoredLeftPixelDB
		{
			get
			{
				return this.ignoredLeftPixelDB;
			}
			set
			{
				this.ignoredLeftPixelDB = value;
				this.OnPropertyChanged("IgnoredLeftPixelDB");
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06001AB7 RID: 6839 RVA: 0x0009FD6C File Offset: 0x0009DF6C
		// (set) Token: 0x06001AB8 RID: 6840 RVA: 0x0009FD84 File Offset: 0x0009DF84
		public int IgnoredRightPixelDB
		{
			get
			{
				return this.ignoredRightPixelDB;
			}
			set
			{
				this.ignoredRightPixelDB = value;
				this.OnPropertyChanged("IgnoredRightPixelDB");
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06001AB9 RID: 6841 RVA: 0x0009FD9C File Offset: 0x0009DF9C
		// (set) Token: 0x06001ABA RID: 6842 RVA: 0x0009FDB4 File Offset: 0x0009DFB4
		public int Scanrate
		{
			get
			{
				return this.scanrate;
			}
			set
			{
				this.scanrate = value;
				this.OnPropertyChanged("Scanrate");
			}
		}

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06001ABB RID: 6843 RVA: 0x0009FDCC File Offset: 0x0009DFCC
		// (set) Token: 0x06001ABC RID: 6844 RVA: 0x0009FDE4 File Offset: 0x0009DFE4
		public int HighGainDualEnergy
		{
			get
			{
				return this.highGainDualEnergy;
			}
			set
			{
				this.highGainDualEnergy = value;
				this.OnPropertyChanged("HighGainDualEnergy");
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06001ABD RID: 6845 RVA: 0x0009FDFC File Offset: 0x0009DFFC
		// (set) Token: 0x06001ABE RID: 6846 RVA: 0x0009FE14 File Offset: 0x0009E014
		public int LowGainDualEnergy
		{
			get
			{
				return this.lowGainDualEnergy;
			}
			set
			{
				this.lowGainDualEnergy = value;
				this.OnPropertyChanged("LowGainDualEnergy");
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x0009FE2C File Offset: 0x0009E02C
		// (set) Token: 0x06001AC0 RID: 6848 RVA: 0x0009FE44 File Offset: 0x0009E044
		public int GainSingleEnergy
		{
			get
			{
				return this.gainSingleEnergy;
			}
			set
			{
				this.gainSingleEnergy = value;
				this.OnPropertyChanged("LowGainDualEnergy");
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x0009FE5A File Offset: 0x0009E05A
		// (set) Token: 0x06001AC2 RID: 6850 RVA: 0x0009FE62 File Offset: 0x0009E062
		public bool ParameterAccessible { get; set; }

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0009FE6C File Offset: 0x0009E06C
		// (set) Token: 0x06001AC4 RID: 6852 RVA: 0x0009FE84 File Offset: 0x0009E084
		public bool BaselineVisible
		{
			get
			{
				return this.baselineVisible;
			}
			set
			{
				this.baselineVisible = value;
				this.OnPropertyChanged("BaselineVisible");
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06001AC5 RID: 6853 RVA: 0x0009FE9C File Offset: 0x0009E09C
		// (set) Token: 0x06001AC6 RID: 6854 RVA: 0x0009FEB4 File Offset: 0x0009E0B4
		public bool DualEnergyAvailable
		{
			get
			{
				return this.dualEnergyAvailable;
			}
			set
			{
				this.dualEnergyAvailable = value;
				this.OnPropertyChanged("DualEnergyAvailable");
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06001AC7 RID: 6855 RVA: 0x0009FECC File Offset: 0x0009E0CC
		// (set) Token: 0x06001AC8 RID: 6856 RVA: 0x0009FEE4 File Offset: 0x0009E0E4
		public bool LowHighGainAvailable
		{
			get
			{
				return this.lowHighGainAvailable;
			}
			set
			{
				this.lowHighGainAvailable = value;
				this.OnPropertyChanged("LowHighGainAvailable");
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06001AC9 RID: 6857 RVA: 0x0009FEFC File Offset: 0x0009E0FC
		// (set) Token: 0x06001ACA RID: 6858 RVA: 0x0009FF14 File Offset: 0x0009E114
		public string SerialNumberMachine
		{
			get
			{
				return this.serialNumberMachine;
			}
			set
			{
				this.serialNumberMachine = value;
				this.OnPropertyChanged("SerialNumberMachine");
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06001ACB RID: 6859 RVA: 0x0009FF2C File Offset: 0x0009E12C
		// (set) Token: 0x06001ACC RID: 6860 RVA: 0x0009FF44 File Offset: 0x0009E144
		public string SerialNumberXRaySource
		{
			get
			{
				return this.serialNumberXRaySource;
			}
			set
			{
				this.serialNumberXRaySource = value;
				this.OnPropertyChanged("SerialNumberXRaySource");
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06001ACD RID: 6861 RVA: 0x0009FF5C File Offset: 0x0009E15C
		// (set) Token: 0x06001ACE RID: 6862 RVA: 0x0009FF74 File Offset: 0x0009E174
		public double XRayOnTimeInternal
		{
			get
			{
				return this.xRayOnTimeInternal;
			}
			set
			{
				this.xRayOnTimeInternal = value;
				this.OnPropertyChanged("XRayOnTimeInternal");
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06001ACF RID: 6863 RVA: 0x0009FF8C File Offset: 0x0009E18C
		// (set) Token: 0x06001AD0 RID: 6864 RVA: 0x0009FFA4 File Offset: 0x0009E1A4
		public string SerialNumberDetector
		{
			get
			{
				return this.serialNumberDetector;
			}
			set
			{
				this.serialNumberDetector = value;
				this.OnPropertyChanged("SerialNumberDetector");
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06001AD1 RID: 6865 RVA: 0x0009FFBC File Offset: 0x0009E1BC
		// (set) Token: 0x06001AD2 RID: 6866 RVA: 0x0009FFD4 File Offset: 0x0009E1D4
		public bool SerialNumberXRayVisible
		{
			get
			{
				return this.serialNumberXRayVisible;
			}
			set
			{
				this.serialNumberXRayVisible = value;
				this.OnPropertyChanged("SerialNumberXRayVisible");
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06001AD3 RID: 6867 RVA: 0x0009FFEC File Offset: 0x0009E1EC
		// (set) Token: 0x06001AD4 RID: 6868 RVA: 0x000A0004 File Offset: 0x0009E204
		public double MachineParametersFontSize
		{
			get
			{
				return this.machineParametersFontSize;
			}
			set
			{
				this.machineParametersFontSize = value;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06001AD5 RID: 6869 RVA: 0x000A0010 File Offset: 0x0009E210
		// (set) Token: 0x06001AD6 RID: 6870 RVA: 0x000A0028 File Offset: 0x0009E228
		public int NPixel
		{
			get
			{
				return this.nPixel;
			}
			set
			{
				this.nPixel = value;
				this.OnPropertyChanged("NPixel");
			}
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06001AD7 RID: 6871 RVA: 0x000A0040 File Offset: 0x0009E240
		// (set) Token: 0x06001AD8 RID: 6872 RVA: 0x000A0058 File Offset: 0x0009E258
		public int MaxY
		{
			get
			{
				return this.maxY;
			}
			set
			{
				this.maxY = value;
				this.OnPropertyChanged("MaxY");
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x06001AD9 RID: 6873 RVA: 0x000A0070 File Offset: 0x0009E270
		// (set) Token: 0x06001ADA RID: 6874 RVA: 0x000A0088 File Offset: 0x0009E288
		public int MinY
		{
			get
			{
				return this.minY;
			}
			set
			{
				this.minY = value;
				this.OnPropertyChanged("MinY");
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x06001ADB RID: 6875 RVA: 0x000A00A0 File Offset: 0x0009E2A0
		// (set) Token: 0x06001ADC RID: 6876 RVA: 0x000A00B8 File Offset: 0x0009E2B8
		public int TicksY
		{
			get
			{
				return this.ticksY;
			}
			set
			{
				this.ticksY = value;
				this.OnPropertyChanged("TicksY");
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06001ADD RID: 6877 RVA: 0x000A00D0 File Offset: 0x0009E2D0
		// (set) Token: 0x06001ADE RID: 6878 RVA: 0x000A00E8 File Offset: 0x0009E2E8
		public int TicksX
		{
			get
			{
				return this.ticksX;
			}
			set
			{
				this.ticksX = value;
				this.OnPropertyChanged("TicksX");
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x000A0100 File Offset: 0x0009E300
		// (set) Token: 0x06001AE0 RID: 6880 RVA: 0x000A0118 File Offset: 0x0009E318
		public int MajorTicksX
		{
			get
			{
				return this.majorTicksX;
			}
			set
			{
				this.majorTicksX = value;
				this.OnPropertyChanged("MajorTicksX");
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x06001AE1 RID: 6881 RVA: 0x000A0130 File Offset: 0x0009E330
		// (set) Token: 0x06001AE2 RID: 6882 RVA: 0x000A0148 File Offset: 0x0009E348
		public List<Point> PlotPoints
		{
			get
			{
				return this.plotPoints;
			}
			set
			{
				this.plotPoints = value;
				bool flag = this.PlotNoTdiPoints != null;
				if (flag)
				{
					this.PlotNoTdiPoints.Clear();
				}
				bool flag2 = this.ReferencePlotNoTdiPoints != null;
				if (flag2)
				{
					this.ReferencePlotNoTdiPoints.Clear();
				}
				this.OnPropertyChanged("PlotPoints");
			}
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x000A019C File Offset: 0x0009E39C
		// (set) Token: 0x06001AE4 RID: 6884 RVA: 0x000A01B4 File Offset: 0x0009E3B4
		public List<Point> ReferencePlotPoints
		{
			get
			{
				return this.referencePlotPoints;
			}
			set
			{
				this.referencePlotPoints = value;
				this.OnPropertyChanged("ReferencePlotPoints");
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06001AE5 RID: 6885 RVA: 0x000A01CC File Offset: 0x0009E3CC
		// (set) Token: 0x06001AE6 RID: 6886 RVA: 0x000A01E4 File Offset: 0x0009E3E4
		public List<Point> PlotNoTdiPoints
		{
			get
			{
				return this.plotNoTdiPoints;
			}
			set
			{
				this.plotNoTdiPoints = value;
				bool flag = this.PlotPoints != null;
				if (flag)
				{
					this.PlotPoints.Clear();
				}
				bool flag2 = this.ReferencePlotPoints != null;
				if (flag2)
				{
					this.ReferencePlotPoints.Clear();
				}
				this.OnPropertyChanged("PlotNoTdiPoints");
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06001AE7 RID: 6887 RVA: 0x000A0238 File Offset: 0x0009E438
		// (set) Token: 0x06001AE8 RID: 6888 RVA: 0x000A0250 File Offset: 0x0009E450
		public List<Point> ReferencePlotNoTdiPoints
		{
			get
			{
				return this.referencePlotNoTdiPoints;
			}
			set
			{
				this.referencePlotNoTdiPoints = value;
				this.OnPropertyChanged("ReferencePlotNoTdiPoints");
			}
		}

		// Token: 0x17000808 RID: 2056
		// (get) Token: 0x06001AE9 RID: 6889 RVA: 0x000A0268 File Offset: 0x0009E468
		// (set) Token: 0x06001AEA RID: 6890 RVA: 0x000A0280 File Offset: 0x0009E480
		public List<Point> IgnoreLeftPoints
		{
			get
			{
				return this.ignoreLeftPoints;
			}
			set
			{
				this.ignoreLeftPoints = value;
				this.OnPropertyChanged("IgnoreLeftPoints");
			}
		}

		// Token: 0x17000809 RID: 2057
		// (get) Token: 0x06001AEB RID: 6891 RVA: 0x000A0298 File Offset: 0x0009E498
		// (set) Token: 0x06001AEC RID: 6892 RVA: 0x000A02B0 File Offset: 0x0009E4B0
		public List<Point> IgnoreRightPoints
		{
			get
			{
				return this.ignoreRightPoints;
			}
			set
			{
				this.ignoreRightPoints = value;
				this.OnPropertyChanged("IgnoreRightPoints");
			}
		}

		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x000A02C8 File Offset: 0x0009E4C8
		// (set) Token: 0x06001AEE RID: 6894 RVA: 0x000A02E0 File Offset: 0x0009E4E0
		public double ZoomFactorX
		{
			get
			{
				return this.zoomFactorX;
			}
			set
			{
				this.zoomFactorX = value;
				this.OnPropertyChanged("ZoomFactorX");
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x06001AEF RID: 6895 RVA: 0x000A02F8 File Offset: 0x0009E4F8
		// (set) Token: 0x06001AF0 RID: 6896 RVA: 0x000A0310 File Offset: 0x0009E510
		public List<Point> PlotHEPoints
		{
			get
			{
				return this.plotHEPoints;
			}
			set
			{
				this.plotHEPoints = value;
				this.OnPropertyChanged("PlotHEPoints");
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06001AF1 RID: 6897 RVA: 0x000A0328 File Offset: 0x0009E528
		// (set) Token: 0x06001AF2 RID: 6898 RVA: 0x000A0340 File Offset: 0x0009E540
		public List<Point> ReferencePlotHEPoints
		{
			get
			{
				return this.referencePlotHEPoints;
			}
			set
			{
				this.referencePlotHEPoints = value;
				this.OnPropertyChanged("ReferencePlotHEPoints");
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x000A0358 File Offset: 0x0009E558
		// (set) Token: 0x06001AF4 RID: 6900 RVA: 0x000A0370 File Offset: 0x0009E570
		public bool IsDualEnergyDetector
		{
			get
			{
				return this.isDualEnergyDetector;
			}
			set
			{
				this.isDualEnergyDetector = value;
				this.OnPropertyChanged("IsDualEnergyDetector");
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x000A0388 File Offset: 0x0009E588
		// (set) Token: 0x06001AF6 RID: 6902 RVA: 0x000A03A0 File Offset: 0x0009E5A0
		public EnergyLevelMode EnergyMode
		{
			get
			{
				return this.energyMode;
			}
			set
			{
				this.energyMode = value;
				this.OnPropertyChanged("EnergyMode");
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x06001AF7 RID: 6903 RVA: 0x000A03B8 File Offset: 0x0009E5B8
		// (set) Token: 0x06001AF8 RID: 6904 RVA: 0x000A03D0 File Offset: 0x0009E5D0
		public string DetectorTypeString
		{
			get
			{
				return this.detectorTypeString;
			}
			set
			{
				this.detectorTypeString = value;
				this.OnPropertyChanged("DetectorTypeString");
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x06001AF9 RID: 6905 RVA: 0x000A03E8 File Offset: 0x0009E5E8
		// (set) Token: 0x06001AFA RID: 6906 RVA: 0x000A0400 File Offset: 0x0009E600
		public string PixelSize
		{
			get
			{
				return this.pixelSize;
			}
			set
			{
				this.pixelSize = value;
				this.OnPropertyChanged("PixelSize");
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x06001AFB RID: 6907 RVA: 0x000A0418 File Offset: 0x0009E618
		// (set) Token: 0x06001AFC RID: 6908 RVA: 0x000A0430 File Offset: 0x0009E630
		public bool IsTdiDetector
		{
			get
			{
				return this.isTdiDetector;
			}
			set
			{
				this.isTdiDetector = value;
				this.OnPropertyChanged("IsTdiDetector");
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x06001AFD RID: 6909 RVA: 0x000A0448 File Offset: 0x0009E648
		// (set) Token: 0x06001AFE RID: 6910 RVA: 0x000A0460 File Offset: 0x0009E660
		public bool IsTdiScanMode
		{
			get
			{
				return this.isTdiScanMode;
			}
			set
			{
				if (value)
				{
					this.isTdiScanMode = true;
					this.isNoTdiScanMode = false;
					bool flag = this.isTdiScanMode;
					if (flag)
					{
						this.IsNoTdiScanMode = false;
						this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 1.ToString(), false);
						this.DrawReferencePlot();
					}
				}
				this.OnPropertyChanged("IsTdiScanMode");
				this.OnPropertyChanged("IsNoTdiScanMode");
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x000A04D4 File Offset: 0x0009E6D4
		// (set) Token: 0x06001B00 RID: 6912 RVA: 0x000A04EC File Offset: 0x0009E6EC
		public bool IsNoTdiScanMode
		{
			get
			{
				return this.isNoTdiScanMode;
			}
			set
			{
				if (value)
				{
					this.isNoTdiScanMode = true;
					this.isTdiScanMode = false;
					bool flag = this.isNoTdiScanMode;
					if (flag)
					{
						this.IsTdiScanMode = false;
						this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 0.ToString(), false);
						this.DrawReferencePlot();
					}
				}
				this.OnPropertyChanged("IsTdiScanMode");
				this.OnPropertyChanged("IsNoTdiScanMode");
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06001B01 RID: 6913 RVA: 0x000A0560 File Offset: 0x0009E760
		public Details PlotDetails
		{
			get
			{
				Details details = new Details();
				string value = this.XRayEnable ? "1" : "0";
				details.XrayEnable = Convert.ToString(value);
				value = (this.OffsetEnable ? "1" : "0");
				details.OffsetEnable = Convert.ToString(value);
				value = (this.GainEnable ? "1" : "0");
				details.GainEnable = Convert.ToString(value);
				details.Scanrate = Convert.ToString(this.Scanrate);
				details.Voltage = Convert.ToString(this.Voltage);
				details.Current = Convert.ToString(this.Current);
				details.SNDetector = Convert.ToString(this.SerialNumberDetector);
				details.SNXRaySource = Convert.ToString(this.serialNumberXRaySource);
				bool flag = this.LowHighGainAvailable;
				if (flag)
				{
					details.HighGain = Convert.ToString(this.HighGainDualEnergy);
					details.LowGain = Convert.ToString(this.LowGainDualEnergy);
					details.LowHighGainAvailable = true;
				}
				else
				{
					details.Gain = Convert.ToString(this.GainSingleEnergy);
				}
				details.IsDualDetector = this.DualEnergyAvailable;
				double num = 0.0;
				bool flag2 = double.TryParse(this.parHandling.ReadPar((this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.dtMCUTemperaturePath : Definition.detectorSesotecTemperaturePath), NumberStyles.Any, new CultureInfo("en-US"), out num);
				if (flag2)
				{
					details.Temperature = (num / 10.0).ToString().Replace(',', '.');
				}
				details.SNMachine = Convert.ToString(this.SerialNumberMachine);
				details.XRayOnTimeInternal = this.XRayOnTimeInternal.ToString();
				details.DetectorTypeString = this.DetectorTypeString;
				details.PixelSize = this.PixelSize;
				bool flag3 = this.detectorType == DetectorType.DTNetTDI;
				if (flag3)
				{
					details.IsTdiDetector = true;
					details.IsTdiScanMode = this.IsTdiScanMode;
					details.IsNoTdiScanMode = this.IsNoTdiScanMode;
				}
				return details;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06001B02 RID: 6914 RVA: 0x000A0797 File Offset: 0x0009E997
		// (set) Token: 0x06001B03 RID: 6915 RVA: 0x000A079F File Offset: 0x0009E99F
		public RelayCommand RescaleCommand { get; set; }

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06001B04 RID: 6916 RVA: 0x000A07A8 File Offset: 0x0009E9A8
		// (set) Token: 0x06001B05 RID: 6917 RVA: 0x000A07B0 File Offset: 0x0009E9B0
		public RelayCommand SaveReferencePlotCommand { get; set; }

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06001B06 RID: 6918 RVA: 0x000A07B9 File Offset: 0x0009E9B9
		// (set) Token: 0x06001B07 RID: 6919 RVA: 0x000A07C1 File Offset: 0x0009E9C1
		public RelayCommand ZoomInXCommand { get; set; }

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06001B08 RID: 6920 RVA: 0x000A07CA File Offset: 0x0009E9CA
		// (set) Token: 0x06001B09 RID: 6921 RVA: 0x000A07D2 File Offset: 0x0009E9D2
		public RelayCommand ZoomOutXCommand { get; set; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x06001B0A RID: 6922 RVA: 0x000A07DB File Offset: 0x0009E9DB
		// (set) Token: 0x06001B0B RID: 6923 RVA: 0x000A07E3 File Offset: 0x0009E9E3
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06001B0C RID: 6924 RVA: 0x000A07EC File Offset: 0x0009E9EC
		// (set) Token: 0x06001B0D RID: 6925 RVA: 0x000A07F4 File Offset: 0x0009E9F4
		public RelayCommand ShowSnrCommand { get; set; }

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06001B0E RID: 6926 RVA: 0x000A0800 File Offset: 0x0009EA00
		// (remove) Token: 0x06001B0F RID: 6927 RVA: 0x000A0838 File Offset: 0x0009EA38
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<RcvImgEventArgs> ImageArrived;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06001B10 RID: 6928 RVA: 0x000A0870 File Offset: 0x0009EA70
		// (remove) Token: 0x06001B11 RID: 6929 RVA: 0x000A08A8 File Offset: 0x0009EAA8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<Details> PlotDetailsArrived;

		// Token: 0x06001B12 RID: 6930 RVA: 0x000A08E0 File Offset: 0x0009EAE0
		public DetectorPlotViewModel(ParameterHandling parHandling, AccessRightsManager accessManager, DBConnection dBConnection, MachineState machineState, ImageDataTransfer imageTransfer, XrayState xrayState)
		{
			this.parHandling = parHandling;
			this.dBConnection = dBConnection;
			this.machineState = machineState;
			this.imageTransfer = imageTransfer;
			this.xrayState = xrayState;
			this.RescaleCommand = new RelayCommand(new Action<object>(this.Rescale));
			this.SaveReferencePlotCommand = new RelayCommand(new Action<object>(this.SaveReferencePlot));
			this.ZoomInXCommand = new RelayCommand(new Action<object>(this.ZoomInX));
			this.ZoomOutXCommand = new RelayCommand(new Action<object>(this.ZoomOutX));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			this.ShowSnrCommand = new RelayCommand(new Action<object>(this.ShowSnr));
			Progress.ShowStaticProgress(LocalizationManager.Localize("txtProcessing"), false);
			this.InitializePlotParameter();
			machineState.StateChangedToInspecting += this.machineState_StateChangedToInspecting;
			this.timerUpdateParams = new Timer(new TimerCallback(this.timer_UpdateParameters), null, 0, 2000);
			this.ParameterAccessible = (accessManager.CurrentUser.Rights == BuiltInPriviledges.Service);
			dBConnection.GetSerialNumber(ref this.serialNumberMachine);
			MachineType machineType = MachineType.MachineTypeUndefined;
			dBConnection.GetMachineType(ref machineType);
			this.machineParametersFontSize = ((machineType != MachineType.RayconEntry) ? 12.0 : 10.0);
			this.DrawReferencePlot();
			Progress.CloseStaticProgress(false);
			ErrorSurvey.CheckArcsIsActive = false;
			imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
			xrayState.CalibrationDone += this.xrayState_CalibrationDone;
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000A0ADC File Offset: 0x0009ECDC
		private void machineState_StateChangedToInspecting(MachineStates obj)
		{
			this.xrayTurnedOnInPlotMenu = true;
			this.InitializePlotParameter();
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x000A0AF0 File Offset: 0x0009ECF0
		private void timer_UpdateParameters(object dummy)
		{
			int num = 0;
			bool flag = int.TryParse(this.parHandling.ReadPar(this.scanratePath), out num);
			if (flag)
			{
				this.Scanrate = num;
			}
			double num2 = 0.0;
			bool flag2 = double.TryParse(this.parHandling.ReadPar(XRayOnProcessing.xrayVoltagePath), NumberStyles.Any, new CultureInfo("en-US"), out num2);
			if (flag2)
			{
				this.Voltage = num2;
			}
			double value = 0.0;
			bool flag3 = double.TryParse(this.parHandling.ReadPar(XRayOnProcessing.xrayCurrentPath), NumberStyles.Any, new CultureInfo("en-US"), out value);
			if (flag3)
			{
				this.Current = value;
			}
			double num3 = 0.0;
			bool flag4 = double.TryParse(this.parHandling.ReadPar((this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.dtMCUTemperaturePath : Definition.detectorSesotecTemperaturePath), NumberStyles.Any, new CultureInfo("en-US"), out num3);
			if (flag4)
			{
				this.DetectorTemperature = num3 / 10.0;
			}
			this.GainEnable = (this.parHandling.ReadPar(XRayOnProcessing.manualGainCalibrationPath) == "1");
			this.OffsetEnable = (this.parHandling.ReadPar(XRayOnProcessing.manualOffsetCalibrationPath) == "1");
			this.BaselineEnable = (this.parHandling.ReadPar(XRayOnProcessing.dtBaselineEnablePath) == "1");
			int num4 = 0;
			this.dBConnection.GetIgnoredLeftPixel(ref num4);
			this.IgnoredLeftPixelDB = num4;
			num4 = 0;
			this.dBConnection.GetIgnoredRightPixel(ref num4);
			this.IgnoredRightPixelDB = num4;
			num4 = 0;
			bool flag5 = int.TryParse(this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath), out num4);
			if (flag5)
			{
				this.IgnoredLeftPixel = num4;
			}
			num4 = 0;
			bool flag6 = int.TryParse(this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath), out num4);
			if (flag6)
			{
				this.IgnoredRightPixel = num4;
			}
			int num5 = 0;
			bool flag7 = int.TryParse(this.parHandling.ReadPar(ParameterHandling.highGainPixelPath), out num5);
			if (flag7)
			{
				this.HighGainDualEnergy = num5;
			}
			int num6 = 0;
			bool flag8 = int.TryParse(this.parHandling.ReadPar(ParameterHandling.lowGainPixelPath), out num6);
			if (flag8)
			{
				this.LowGainDualEnergy = num6;
			}
			int num7 = 0;
			bool flag9 = int.TryParse(this.parHandling.ReadPar(ParameterHandling.gainPixelPath), out num7);
			if (flag9)
			{
				this.GainSingleEnergy = num7;
			}
			this.XRayEnable = (this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1");
			string path = (this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTSerialPath : Definition.detectorSesotecSerialPath;
			this.SerialNumberDetector = this.parHandling.ReadPar(path);
			this.SerialNumberXRayVisible = (this.parHandling.ReadPar(Definition.xRaySourceTypePath) == "1");
			bool flag10 = this.serialNumberXRayVisible;
			if (flag10)
			{
				this.SerialNumberXRaySource = this.parHandling.ReadPar(Definition.xraySerialPath);
				double xrayOnTimeInternal = 0.0;
				bool flag11 = double.TryParse(this.parHandling.ReadPar(Definition.xRayOnTimeInternalPath + "Value"), NumberStyles.Any, new CultureInfo("en-US"), out xrayOnTimeInternal);
				if (flag11)
				{
					this.XRayOnTimeInternal = xrayOnTimeInternal;
				}
			}
			Details details = new Details();
			bool flag12 = this.PlotDetailsArrived != null;
			if (flag12)
			{
				this.PlotDetailsArrived(this, this.PlotDetails);
			}
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000A0E9C File Offset: 0x0009F09C
		private void InitializePlotParameter()
		{
			this.machineState.State = MachineStates.ViewingPlot;
			DetectorSubType detectorSubType = DetectorSubType.None;
			string value = "0";
			this.MajorTicksX = 64;
			this.dBConnection.GetDetectorType(ref this.detectorType);
			this.DetectorTypeString = this.detectorType.ToString();
			bool flag = this.detectorType == DetectorType.Sesotec && this.parHandling.ReadPar(Definition.detectorSubtypePath) == "1";
			if (flag)
			{
				this.DetectorTypeString += " HS";
			}
			bool flag2 = this.detectorType == DetectorType.Sesotec;
			if (flag2)
			{
				this.PixelSize = this.parHandling.ReadPar(Definition.detectorSesotecPixelSizePath);
			}
			else
			{
				bool flag3 = this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI;
				if (flag3)
				{
					this.PixelSize = this.parHandling.ReadPar(Definition.detectorDtPixelSizePath);
				}
			}
			bool flag4 = this.detectorType == DetectorType.DTNetTDI;
			if (flag4)
			{
				this.IsTdiDetector = true;
				this.IsTdiScanMode = true;
				this.parHandling.WriteValueAndCheckxTimes("/hardware/detector/dt/StartStopScanning/Value", "0", false, this.writeParameterRepeat);
			}
			this.scanratePath = ((this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTScanratePath : Definition.detectorSesotecScanratePath);
			bool flag5 = this.oldScanrate == 0;
			if (flag5)
			{
				int.TryParse(this.parHandling.ReadPar(this.scanratePath), out this.oldScanrate);
			}
			this.oldContinuousMode = this.parHandling.ReadPar(ParameterHandling.continuousModePath);
			this.oldContinuousModeFiltering = this.parHandling.ReadPar(Acquisition.continuousModeFilteringPath);
			this.oldSendOnlyIfContaminant = this.parHandling.ReadPar(Acquisition.sendOnlyIfContaminantPath);
			this.oldSendImage = this.parHandling.ReadPar(Acquisition.sendImagePath);
			this.oldBulkFlapsValvesActive = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnableValvesPath);
			this.oldSVTValvesActive = this.parHandling.ReadPar(SVT.svtValvesEnablePath);
			this.oldLiveCalibrationMode = this.parHandling.ReadPar(Definition.isLiveCalibrationActivePath);
			this.parHandling.WriteValue(Definition.isLiveCalibrationActivePath, "0", false);
			bool flag6 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
			if (flag6)
			{
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, "0");
			}
			else
			{
				bool flag7 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag7)
				{
					this.parHandling.WriteValue(SVT.svtValvesEnablePath, "0");
				}
			}
			this.parHandling.WriteValue(Acquisition.binningPath, "1", false);
			this.parHandling.WriteValue(Acquisition.binPixelPath, "1", false);
			this.parHandling.WriteValue(Acquisition.lane1EnablePath, "1", false);
			this.parHandling.WriteValue(Acquisition.lane2EnablePath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane3EnablePath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane4EnablePath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane1MinPixelPath, "0", false);
			this.parHandling.WriteValue(Acquisition.lane1MaxPixelPath, 10239.ToString(), false);
			this.parHandling.WriteValue(Acquisition.continuousModeFilteringPath, "0", false);
			this.parHandling.WriteValueAndCheckxTimes(XRayOnProcessing.dtGainEnablePath, "0", false, this.writeParameterRepeat);
			this.parHandling.WriteValue(XRayOnProcessing.manualGainCalibrationPath, "0", false);
			this.GainEnable = false;
			bool flag8 = this.detectorType == DetectorType.DTNetTDI;
			if (flag8)
			{
				this.parHandling.WriteValueAndCheckxTimes(XRayOnProcessing.dtOffsetEnablePath, "1", false, this.writeParameterRepeat);
			}
			else
			{
				this.parHandling.WriteValue(XRayOnProcessing.dtOffsetEnablePath, "0", false);
			}
			string a = this.parHandling.ReadPar("/hardware/detector/sesotec/SubType/Value");
			bool flag9 = a == "1";
			if (flag9)
			{
				detectorSubType = DetectorSubType.SesotecHS;
			}
			bool flag10 = this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI || (this.detectorType == DetectorType.Sesotec && detectorSubType == DetectorSubType.SesotecHS);
			if (flag10)
			{
				value = "1";
			}
			this.parHandling.WriteValue(XRayOnProcessing.manualOffsetCalibrationPath, value, false);
			this.OffsetEnable = false;
			this.Scanrate = ((this.detectorType != DetectorType.DTNetDE) ? 375 : 500);
			this.parHandling.WriteValueAndCheckxTimes(this.scanratePath, this.Scanrate.ToString(), false, this.writeParameterRepeat);
			this.IgnoredLeftPixel = 0;
			this.parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, this.IgnoredLeftPixel.ToString(), false);
			this.IgnoredRightPixel = 0;
			this.parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, this.IgnoredRightPixel.ToString(), false);
			this.parHandling.WriteValue(XRayOnProcessing.dtBaselineEnablePath, "0", false);
			this.BaselineEnable = false;
			this.BaselineVisible = (this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI);
			this.DualEnergyAvailable = (this.detectorType == DetectorType.DTNetDE);
			bool flag11 = this.DualEnergyAvailable;
			if (flag11)
			{
				this.LowHighGainAvailable = true;
			}
			this.Current = ((this.detectorType != DetectorType.DTNetDE) ? 1.0 : 0.5);
			bool flag12 = this.detectorType == DetectorType.DTNetTDI;
			if (flag12)
			{
				this.Current = 2.0;
			}
			ParameterHandling parameterHandling = this.parHandling;
			string path = "/hardware/xraysource/CurrentSetpoint/Value";
			double num = this.Current;
			parameterHandling.WriteValue(path, num.ToString(new CultureInfo("en-US")), false);
			this.Voltage = (double)((this.detectorType != DetectorType.DTNetDE) ? 40 : 70);
			this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", ((int)this.Voltage).ToString(), false);
			this.parHandling.WriteValue(Acquisition.sendOnlyIfContaminantPath, "0", false);
			this.parHandling.WriteValue(Acquisition.sendImagePath, "1", false);
			this.parHandling.WriteValue("/filter/acquisition/MaxLines/Value", "200", false);
			this.parHandling.WriteValue("/filter/acquisition/MinLines/Value", "100", false);
			this.parHandling.WriteValue(ParameterHandling.continuousModePath, "1", false);
			bool flag13 = this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1";
			if (flag13)
			{
				this.XRayEnable = true;
			}
			else
			{
				this.XRayEnable = false;
			}
			this.SerialNumberXRayVisible = (this.parHandling.ReadPar(Definition.xRaySourceTypePath) == "1");
			this.parHandling.WriteValue(Acquisition.acquisitionModePath, "0", false);
			int.TryParse(this.parHandling.ReadPar((this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTNPixelPath : Definition.detectorSesotecNPixelPath), out this.nPixel);
			this.SetPlotScaleX();
			string a2 = this.parHandling.ReadPar(Definition.machineModePath);
			bool flag14 = a2 == 1.ToString();
			if (flag14)
			{
				this.parHandling.WriteValue("/filter/xrayoffimage/Enable/Value", "0");
			}
			bool flag15 = this.detectorType == DetectorType.DTNetTDI;
			if (flag15)
			{
				this.LowHighGainAvailable = true;
				this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 1.ToString(), false);
				this.parHandling.WriteValueAndCheckxTimes("/hardware/detector/dt/StartStopScanning/Value", "1", false, this.writeParameterRepeat);
			}
			this.parHandling.WriteValue(Rejecter.rejecter1MainPath + "/Enabled/Value", "0", false);
			this.parHandling.WriteValue(Rejecter.rejecter2MainPath + "/Enabled/Value", "0", false);
			this.parHandling.WriteValue(Rejecter.rejecter3MainPath + "/Enabled/Value", "0", false);
			this.parHandling.WriteValue(Rejecter.rejecter4MainPath + "/Enabled/Value", "0", false);
			XRayOnProcessing.CalibrateInPlot = true;
			XRayOnProcessing.ConveyingScanrate = this.oldScanrate;
			bool flag16 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag16)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "0");
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x000A1760 File Offset: 0x0009F960
		private void DeinitializePlotParameter()
		{
			string s = null;
			string s2 = null;
			string value = null;
			this.dBConnection.GetParameterValue(Acquisition.binningPath, ref value);
			this.parHandling.WriteValue(Acquisition.binningPath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.binPixelPath, ref value);
			this.parHandling.WriteValue(Acquisition.binPixelPath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane1EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane1EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane2EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane2EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane3EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane3EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane4EnablePath, ref value);
			this.parHandling.WriteValue(Acquisition.lane4EnablePath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane1MinPixelPath, ref value);
			this.parHandling.WriteValue(Acquisition.lane1MinPixelPath, value, false);
			this.dBConnection.GetParameterValue(Acquisition.lane1MaxPixelPath, ref value);
			this.parHandling.WriteValue(Acquisition.lane1MaxPixelPath, value, false);
			this.parHandling.WriteValue(Acquisition.sendOnlyIfContaminantPath, this.oldSendOnlyIfContaminant, false);
			this.parHandling.WriteValue(Acquisition.sendImagePath, this.oldSendImage, false);
			this.parHandling.WriteValue(ParameterHandling.continuousModePath, this.oldContinuousMode, false);
			this.parHandling.WriteValue(Acquisition.continuousModeFilteringPath, this.oldContinuousModeFiltering, false);
			bool flag = this.detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				this.parHandling.WriteValueAndCheckxTimes("/hardware/detector/dt/StartStopScanning/Value", "0", false, this.writeParameterRepeat);
				this.parHandling.WriteValueAndCheckxTimes(XRayOnProcessing.dtOffsetEnablePath, "1", false, this.writeParameterRepeat);
				this.parHandling.WriteValueAndCheckxTimes(XRayOnProcessing.dtGainEnablePath, "1", false, this.writeParameterRepeat);
			}
			bool flag2 = this.oldScanrate != 0;
			if (flag2)
			{
				this.parHandling.WriteValueAndCheckxTimes(this.scanratePath, this.oldScanrate.ToString(), this.xrayTurnedOnInPlotMenu, this.writeParameterRepeat);
			}
			bool flag3 = this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath) == "0";
			if (flag3)
			{
				int num = 0;
				this.dBConnection.GetIgnoredLeftPixel(ref num);
				this.parHandling.WriteValue(ParameterHandling.ignoreLeftPixelPath, num.ToString(), false);
			}
			bool flag4 = this.parHandling.ReadPar(ParameterHandling.ignoreRightPixelPath) == "0";
			if (flag4)
			{
				int num2 = 0;
				this.dBConnection.GetIgnoredRightPixel(ref num2);
				this.parHandling.WriteValue(ParameterHandling.ignoreRightPixelPath, num2.ToString(), false);
			}
			bool flag5 = this.parHandling.ReadPar(FlapsOverIOs.bulkflapsEnabledPath) == "1";
			if (flag5)
			{
				this.parHandling.WriteValue(FlapsOverIOs.bulkflapsEnableValvesPath, this.oldBulkFlapsValvesActive, true);
			}
			else
			{
				bool flag6 = this.parHandling.ReadPar(SVT.svtEnablePath) == "1";
				if (flag6)
				{
					this.parHandling.WriteValue(SVT.svtValvesEnablePath, this.oldSVTValvesActive, true);
				}
			}
			string value2 = null;
			this.dBConnection.GetParameterValue("/filter/acquisition/MaxLines/Value", ref value2);
			this.parHandling.WriteValue("/filter/acquisition/MaxLines/Value", value2, false);
			string value3 = null;
			this.dBConnection.GetParameterValue("/filter/acquisition/MinLines/Value", ref value3);
			this.parHandling.WriteValue("/filter/acquisition/MinLines/Value", value3, false);
			this.dBConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref s);
			this.dBConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref s2);
			double num3;
			bool flag7 = double.TryParse(s, NumberStyles.Any, new CultureInfo("en-US"), out num3) && num3 > 0.0;
			if (flag7)
			{
				this.parHandling.WriteValue("/hardware/xraysource/CurrentSetpoint/Value", num3.ToString().Replace(',', '.'), false);
			}
			double num4;
			bool flag8 = double.TryParse(s2, NumberStyles.Any, new CultureInfo("en-US"), out num4) && num4 > 0.0;
			if (flag8)
			{
				this.parHandling.WriteValue("/hardware/xraysource/VoltageSetpoint/Value", num4.ToString().Replace(',', '.'), false);
			}
			string value4 = null;
			this.dBConnection.GetParameterValue(Acquisition.acquisitionModePath, ref value4);
			this.parHandling.WriteValue(Acquisition.acquisitionModePath, value4, false);
			bool flag9 = this.detectorType == DetectorType.DTNetTDI;
			if (flag9)
			{
				ConveyingDirection conveyingDirection = ConveyingDirection.LeftToRight;
				this.dBConnection.GetConveyingDirection(ref conveyingDirection);
				bool flag10 = conveyingDirection == ConveyingDirection.LeftToRight;
				if (flag10)
				{
					this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 1.ToString(), false);
				}
				else
				{
					this.parHandling.WriteValue("/hardware/detector/dt/TdiCorrectionMode/Value", 2.ToString(), false);
				}
				this.parHandling.WriteValueAndCheckxTimes("/hardware/detector/dt/StartStopScanning/Value", "1", false, this.writeParameterRepeat);
				Thread.Sleep(1000);
			}
			XRayOnProcessing.CalibrateInPlot = false;
			bool flag11 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1" && this.parHandling.ReadPar(XRayOnProcessing.xrayEnablePath) == "1";
			if (flag11)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "1");
			}
			this.parHandling.WriteValue(Definition.isLiveCalibrationActivePath, this.oldLiveCalibrationMode, false);
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x000A1D00 File Offset: 0x0009FF00
		public void SavePlot(Grid chartGrid, Rect bounds)
		{
			string str = "";
			string text = DetectorPlotViewModel.plotFolderName;
			string text2 = null;
			bool flag = this.dBConnection == null;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNoDatabase"), DialogStyle.Error);
			}
			else
			{
				bool flag2 = !Directory.Exists(text);
				if (flag2)
				{
					Directory.CreateDirectory(text);
				}
				this.dBConnection.GetUsbDriveLetter(ref str);
				this.dBConnection.GetSerialNumber(ref text2);
				string text3 = "Plot_";
				bool flag3 = this.detectorType == DetectorType.DTNetTDI;
				if (flag3)
				{
					bool flag4 = this.IsTdiScanMode;
					if (flag4)
					{
						text3 += "TDI_";
					}
					bool flag5 = this.IsNoTdiScanMode;
					if (flag5)
					{
						text3 += "NoTDI_";
					}
				}
				text3 = text3 + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ff") + ((text2 != null && text2.Length > 0) ? ("_" + text2) : "") + ".png";
				string text4 = str + "\\" + text3;
				text = text + "\\" + text3;
				FileInfo fileInfo = new FileInfo(text4);
				try
				{
					RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bounds.Width, (int)bounds.Height, 96.0, 96.0, PixelFormats.Pbgra32);
					DrawingVisual drawingVisual = new DrawingVisual();
					using (DrawingContext drawingContext = drawingVisual.RenderOpen())
					{
						VisualBrush brush = new VisualBrush(chartGrid);
						drawingContext.DrawRectangle(brush, null, new Rect(default(Point), bounds.Size));
					}
					renderTargetBitmap.Render(drawingVisual);
					List<string> list = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Plot").ToList<string>();
					list.Sort(new NamesComparer());
					for (int i = list.Count; i >= 10; i--)
					{
						File.Delete(list[0]);
					}
					FileStream fileStream = new FileStream(text, FileMode.Create);
					new PngBitmapEncoder
					{
						Frames = 
						{
							BitmapFrame.Create(renderTargetBitmap)
						}
					}.Save(fileStream);
					fileStream.Close();
					bool flag6 = Directory.Exists(str + "\\");
					if (flag6)
					{
						fileStream = new FileStream(fileInfo.FullName, FileMode.Create);
						new PngBitmapEncoder
						{
							Frames = 
							{
								BitmapFrame.Create(renderTargetBitmap)
							}
						}.Save(fileStream);
						fileStream.Close();
					}
					Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
				}
				catch (UnauthorizedAccessException)
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
					{
						LocalizationManager.Localize("msgPathAccess"),
						" \"",
						Path.GetDirectoryName(text4),
						"\" ",
						LocalizationManager.Localize("msgForbidden")
					}), DialogStyle.Error);
				}
				catch
				{
					Dialog.Show(LocalizationManager.Localize("txtError"), string.Concat(new string[]
					{
						LocalizationManager.Localize("msgPathAccess"),
						" \"",
						Path.GetDirectoryName(text4),
						"\" ",
						LocalizationManager.Localize("msgForbidden")
					}), DialogStyle.Error);
				}
			}
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x000A20B0 File Offset: 0x000A02B0
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x000A20E0 File Offset: 0x000A02E0
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = this.detectorType == DetectorType.DTNetTDI;
				if (flag2)
				{
					this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "0", false);
				}
				List<Point> pointsTemp = new List<Point>();
				List<Point> pointsHETemp = new List<Point>();
				List<Point> list = new List<Point>();
				int num = 0;
				string s = this.parHandling.ReadPar(ParameterHandling.ignoreLeftPixelPath);
				int.TryParse(s, out num);
				bool flag3 = e.Image.Width > 0 && e.Image.Height > 0 && Application.Current != null;
				if (flag3)
				{
					bool flag4 = e.Image.EnergyLevels == 1;
					if (flag4)
					{
						for (int i = 0; i < (int)e.Image.Width; i++)
						{
							pointsTemp.Add(new Point((double)(i + this.ignoredLeftPixel), (double)e.Image.Data[i]));
						}
					}
					bool flag5 = e.Image.EnergyLevels == 2;
					if (flag5)
					{
						for (int j = 0; j < (int)(e.Image.Width / 2); j++)
						{
							pointsTemp.Add(new Point((double)(j + this.ignoredLeftPixel), (double)e.Image.Data[j]));
							pointsHETemp.Add(new Point((double)(j + this.ignoredLeftPixel), (double)e.Image.Data[j + (int)(e.Image.Width / 2)]));
						}
					}
				}
				List<int> list2 = new List<int>();
				bool flag6 = !this.isNoTdiScanMode;
				if (flag6)
				{
					List<int> list3 = list2;
					int item;
					if (this.plotPoints == null || this.plotPoints.Count <= 0 || this.energyMode == EnergyLevelMode.HighEnergy)
					{
						item = 0;
					}
					else
					{
						item = (int)this.plotPoints.Max((Point point) => point.Y);
					}
					list3.Add(item);
					List<int> list4 = list2;
					int item2;
					if (this.referencePlotPoints.Count <= 0 || this.energyMode == EnergyLevelMode.HighEnergy)
					{
						item2 = 0;
					}
					else
					{
						item2 = (int)this.referencePlotPoints.Max((Point point) => point.Y);
					}
					list4.Add(item2);
				}
				List<int> list5 = list2;
				int item3;
				if (this.plotHEPoints.Count <= 0 || this.energyMode == EnergyLevelMode.LowEnergy)
				{
					item3 = 0;
				}
				else
				{
					item3 = (int)this.plotHEPoints.Max((Point point) => point.Y);
				}
				list5.Add(item3);
				List<int> list6 = list2;
				int item4;
				if (this.referencePlotHEPoints.Count <= 0 || this.energyMode == EnergyLevelMode.LowEnergy)
				{
					item4 = 0;
				}
				else
				{
					item4 = (int)this.referencePlotHEPoints.Max((Point point) => point.Y);
				}
				list6.Add(item4);
				bool flag7 = this.isNoTdiScanMode;
				if (flag7)
				{
					List<int> list7 = list2;
					int item5;
					if (this.plotNoTdiPoints == null || this.plotNoTdiPoints.Count <= 0)
					{
						item5 = 0;
					}
					else
					{
						item5 = (int)this.plotNoTdiPoints.Max((Point point) => point.Y);
					}
					list7.Add(item5);
					List<int> list8 = list2;
					int item6;
					if (this.referencePlotNoTdiPoints.Count <= 0)
					{
						item6 = 0;
					}
					else
					{
						item6 = (int)this.referencePlotNoTdiPoints.Max((Point point) => point.Y);
					}
					list8.Add(item6);
				}
				this.CalculateNewMaxY(list2.Max(), true);
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					bool flag11 = !this.isNoTdiScanMode;
					if (flag11)
					{
						this.PlotPoints = ((this.energyMode != EnergyLevelMode.HighEnergy) ? pointsTemp : new List<Point>());
					}
					else
					{
						this.PlotNoTdiPoints = pointsTemp;
					}
					bool flag12 = e.Image.EnergyLevels > 1;
					if (flag12)
					{
						this.PlotHEPoints = ((this.energyMode != EnergyLevelMode.LowEnergy) ? pointsHETemp : new List<Point>());
					}
					bool flag13 = !this.isNoTdiScanMode;
					if (flag13)
					{
						this.IgnoreLeftPoints = this.GetIgnorePoints(true);
						this.IgnoreRightPoints = this.GetIgnorePoints(false);
					}
					else
					{
						List<Point> list9 = new List<Point>();
						this.IgnoreLeftPoints = list9;
						this.IgnoreRightPoints = list9;
					}
					bool flag14 = this.saveReferencePlot;
					if (flag14)
					{
						this.SaveReferencePlot(e.Image);
						this.saveReferencePlot = false;
					}
				}));
				bool flag8 = (this.nPixel == 0 && e.Image.Width > 0) || (int)e.Image.Width > this.nPixel * 5 || (int)(e.Image.Width * 5) < this.nPixel;
				if (flag8)
				{
					int.TryParse(this.parHandling.ReadPar((this.detectorType == DetectorType.DT || this.detectorType == DetectorType.DTNetSE || this.detectorType == DetectorType.DTNetDE || this.detectorType == DetectorType.DTNetTDI) ? Definition.detectorDTNPixelPath : Definition.detectorSesotecNPixelPath), out this.nPixel);
					this.OnPropertyChanged("NPixel");
					this.SetPlotScaleX();
				}
				bool flag9 = this.ImageArrived != null;
				if (flag9)
				{
					this.ImageArrived(sender, new RcvImgEventArgs(e.Image));
				}
				bool flag10 = this.detectorType == DetectorType.DTNetTDI;
				if (flag10)
				{
					this.parHandling.WriteValue("/hardware/detector/dt/StartStopScanning/Value", "1", false);
				}
			}
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x000A2698 File Offset: 0x000A0898
		private void CalculateNewMaxY(int currentMax, bool respectCurrentMax = true)
		{
			bool flag = currentMax > this.maxY || currentMax < (int)((double)this.maxY * 0.7) || !respectCurrentMax;
			if (flag)
			{
				int num = 1;
				int newMax = (int)((double)currentMax * 1.3);
				int newTicksY = 1;
				bool flag2 = newMax >= 10000;
				if (flag2)
				{
					num = 1000;
				}
				else
				{
					bool flag3 = newMax >= 1000;
					if (flag3)
					{
						num = 100;
					}
					else
					{
						bool flag4 = newMax >= 100;
						if (flag4)
						{
							num = 10;
						}
					}
				}
				newMax = newMax / num * num;
				newTicksY = newMax / 10;
				bool flag5 = newTicksY > 100;
				if (flag5)
				{
					newTicksY = newTicksY / 100 * 100;
				}
				else
				{
					bool flag6 = newTicksY > 10;
					if (flag6)
					{
						newTicksY = newTicksY / 10 * 10;
					}
				}
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					this.MinY = 0;
					this.MaxY = newMax;
					this.TicksY = newTicksY;
				}));
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x000A27CC File Offset: 0x000A09CC
		private void Rescale(object obj)
		{
			bool flag = this.plotPoints != null;
			if (flag)
			{
				List<int> list = new List<int>();
				bool flag2 = !this.isNoTdiScanMode;
				if (flag2)
				{
					List<int> list2 = list;
					int item;
					if (this.plotPoints.Count <= 0 || this.energyMode == EnergyLevelMode.HighEnergy)
					{
						item = 0;
					}
					else
					{
						item = (int)this.plotPoints.Max((Point point) => point.Y);
					}
					list2.Add(item);
					List<int> list3 = list;
					int item2;
					if (this.referencePlotPoints.Count <= 0 || this.energyMode == EnergyLevelMode.HighEnergy)
					{
						item2 = 0;
					}
					else
					{
						item2 = (int)this.referencePlotPoints.Max((Point point) => point.Y);
					}
					list3.Add(item2);
				}
				List<int> list4 = list;
				int item3;
				if (this.plotHEPoints.Count <= 0 || this.energyMode == EnergyLevelMode.LowEnergy)
				{
					item3 = 0;
				}
				else
				{
					item3 = (int)this.plotHEPoints.Max((Point point) => point.Y);
				}
				list4.Add(item3);
				List<int> list5 = list;
				int item4;
				if (this.referencePlotHEPoints.Count <= 0 || this.energyMode == EnergyLevelMode.LowEnergy)
				{
					item4 = 0;
				}
				else
				{
					item4 = (int)this.referencePlotHEPoints.Max((Point point) => point.Y);
				}
				list5.Add(item4);
				bool flag3 = this.isNoTdiScanMode;
				if (flag3)
				{
					bool flag4 = this.plotNoTdiPoints != null;
					if (flag4)
					{
						List<int> list6 = list;
						int item5;
						if (this.plotNoTdiPoints.Count <= 0)
						{
							item5 = 0;
						}
						else
						{
							item5 = (int)this.plotNoTdiPoints.Max((Point point) => point.Y);
						}
						list6.Add(item5);
					}
					List<int> list7 = list;
					int item6;
					if (this.referencePlotNoTdiPoints.Count <= 0)
					{
						item6 = 0;
					}
					else
					{
						item6 = (int)this.referencePlotNoTdiPoints.Max((Point point) => point.Y);
					}
					list7.Add(item6);
				}
				this.CalculateNewMaxY(list.Max(), false);
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x000A29E4 File Offset: 0x000A0BE4
		private List<Point> GetIgnorePoints(bool left)
		{
			List<Point> list = new List<Point>();
			int num = 0;
			try
			{
				if (left)
				{
					this.dBConnection.GetIgnoredLeftPixel(ref num);
				}
				else
				{
					this.dBConnection.GetIgnoredRightPixel(ref num);
				}
				int num2 = this.maxY / 20;
				bool flag = num2 == 0;
				if (flag)
				{
					return list;
				}
				for (int i = 0; i < this.maxY - this.minY; i += num2)
				{
					list.Add(new Point((double)(left ? num : (this.NPixel - num)), (double)i));
				}
			}
			catch
			{
			}
			return list;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x000A2A98 File Offset: 0x000A0C98
		private void SaveReferencePlot(object obj)
		{
			bool flag = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtOverwriteExisting"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
			if (flag)
			{
				this.saveReferencePlot = true;
			}
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x000A2AD4 File Offset: 0x000A0CD4
		private void SaveReferencePlot(XRSImage image)
		{
			bool flag = !this.isNoTdiScanMode;
			if (flag)
			{
				bool flag2 = this.PlotPoints != null && this.PlotPoints.Count > 0;
				if (flag2)
				{
					List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
					for (int i = 0; i < (int)image.Width; i++)
					{
						list.Add(new KeyValuePair<int, int>(i + this.ignoredLeftPixel, (int)image.Data[i]));
					}
					bool flag3 = this.dBConnection.AddPairsToReferencePlot(list);
					if (flag3)
					{
						Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
						this.DrawReferencePlot();
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error, DialogButtons.OK);
					}
				}
			}
			else
			{
				bool flag4 = this.PlotNoTdiPoints != null && this.PlotNoTdiPoints.Count > 0;
				if (flag4)
				{
					List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
					for (int j = 0; j < (int)image.Width; j++)
					{
						list.Add(new KeyValuePair<int, int>(j + this.ignoredLeftPixel, (int)image.Data[j]));
					}
					bool flag5 = this.dBConnection.AddPairsToReferencePlotNoTdi(list);
					if (flag5)
					{
						Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSaveSuccessful"), DialogStyle.Ok, DialogButtons.None, true);
						this.DrawReferencePlot();
					}
					else
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error, DialogButtons.OK);
					}
				}
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000A2C68 File Offset: 0x000A0E68
		private void DrawReferencePlot()
		{
			List<Point> list = new List<Point>();
			List<Point> list2 = new List<Point>();
			bool flag = !this.isNoTdiScanMode;
			List<KeyValuePair<int, int>> list3;
			if (flag)
			{
				list3 = this.dBConnection.GetEntriesFromReferencePlot();
			}
			else
			{
				list3 = this.dBConnection.GetEntriesFromReferencePlotNoTdi();
			}
			bool flag2 = !this.IsDualEnergyDetector;
			if (flag2)
			{
				for (int i = 0; i < list3.Count; i++)
				{
					list.Add(new Point((double)list3[i].Key, (double)list3[i].Value));
				}
				bool flag3 = !this.isNoTdiScanMode;
				if (flag3)
				{
					this.ReferencePlotPoints = list;
				}
				else
				{
					this.ReferencePlotNoTdiPoints = list;
				}
			}
			else
			{
				int j;
				for (j = 0; j < list3.Count / 2; j++)
				{
					list.Add(new Point((double)list3[j].Key, (double)list3[j].Value));
				}
				while (j < list3.Count)
				{
					list2.Add(new Point((double)(list3[j].Key - list.Count), (double)list3[j].Value));
					j++;
				}
				this.ReferencePlotPoints = ((this.energyMode != EnergyLevelMode.HighEnergy) ? list : new List<Point>());
				this.ReferencePlotHEPoints = ((this.energyMode != EnergyLevelMode.LowEnergy) ? list2 : new List<Point>());
			}
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x000A2DF8 File Offset: 0x000A0FF8
		private void ZoomInX(object obj)
		{
			this.ZoomFactorX *= 2.0;
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x000A2E12 File Offset: 0x000A1012
		private void ZoomOutX(object obj)
		{
			this.ZoomFactorX /= 2.0;
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x000A2E2C File Offset: 0x000A102C
		public void EnergyModeChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = e.AddedItems.Count > 0 && e.AddedItems[0] is EnergyLevelMode;
			if (flag)
			{
				EnergyLevelMode energyLevelMode = (EnergyLevelMode)e.AddedItems[0];
				this.EnergyMode = energyLevelMode;
				this.DrawReferencePlot();
			}
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000A2E88 File Offset: 0x000A1088
		private void ShowSnr(object obj)
		{
			CtrlSnr ctrlSnr = new CtrlSnr(this, this, this.parHandling, this.dBConnection, this.PlotDetails);
			Subwindow.Show(ctrlSnr, SubwindowButtons.Back, ctrlSnr.PanelButtons, ctrlSnr.UiElementRightAligned);
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x000A2EC4 File Offset: 0x000A10C4
		private void SetPlotScaleX()
		{
			this.isDualEnergyDetector = (this.detectorType == DetectorType.DTNetDE);
			bool flag = this.isDualEnergyDetector;
			if (flag)
			{
				this.nPixel /= 2;
			}
			this.TicksX = ((this.nPixel <= 512) ? 4 : 8);
			this.MajorTicksX = 64;
			bool flag2 = this.nPixel > 2048;
			if (flag2)
			{
				this.TicksX = 16;
				this.MajorTicksX = 160;
			}
			bool flag3 = this.nPixel > 4096;
			if (flag3)
			{
				this.TicksX = 32;
				this.MajorTicksX = 320;
			}
			bool flag4 = this.nPixel > 8192;
			if (flag4)
			{
				this.TicksX = 64;
				this.MajorTicksX = 640;
			}
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x000A2F94 File Offset: 0x000A1194
		private void xrayState_CalibrationDone()
		{
			bool flag = this.detectorType == DetectorType.DTNetTDI;
			if (flag)
			{
				for (int i = 0; i < 3; i++)
				{
					this.parHandling.WriteValue(Definition.detectorDTIntegrationTimePath, "2666", false);
					Thread.Sleep(500);
					this.OnPropertyChanged("NPixel");
					this.SetPlotScaleX();
				}
			}
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x000A2FF8 File Offset: 0x000A11F8
		public void CleanUp(object obj)
		{
			this.machineState.StateChangedToInspecting -= this.machineState_StateChangedToInspecting;
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.xrayState.CalibrationDone -= this.xrayState_CalibrationDone;
			string a = this.parHandling.ReadPar(Definition.machineModePath);
			bool flag = a == 1.ToString();
			if (flag)
			{
				this.parHandling.WriteValue("/filter/xrayoffimage/Enable/Value", "1");
			}
			this.timerUpdateParams.Change(-1, -1);
			this.DeinitializePlotParameter();
		}

		// Token: 0x040011BD RID: 4541
		private int ignoredRightPixel;

		// Token: 0x040011BE RID: 4542
		private int ignoredLeftPixel;

		// Token: 0x040011BF RID: 4543
		private int ignoredRightPixelDB;

		// Token: 0x040011C0 RID: 4544
		private int ignoredLeftPixelDB;

		// Token: 0x040011C1 RID: 4545
		private bool gainEnable;

		// Token: 0x040011C2 RID: 4546
		private bool offsetEnable;

		// Token: 0x040011C3 RID: 4547
		private bool baselineEnable;

		// Token: 0x040011C4 RID: 4548
		private double current;

		// Token: 0x040011C5 RID: 4549
		private double voltage;

		// Token: 0x040011C6 RID: 4550
		private double detectorTemperature;

		// Token: 0x040011C7 RID: 4551
		private int scanrate;

		// Token: 0x040011C8 RID: 4552
		private bool xRayEnable;

		// Token: 0x040011C9 RID: 4553
		private bool baselineVisible;

		// Token: 0x040011CA RID: 4554
		private bool dualEnergyAvailable;

		// Token: 0x040011CB RID: 4555
		private bool lowHighGainAvailable;

		// Token: 0x040011CC RID: 4556
		private bool xrayTurnedOnInPlotMenu;

		// Token: 0x040011CD RID: 4557
		private const string maxLinesPath = "/filter/acquisition/MaxLines/Value";

		// Token: 0x040011CE RID: 4558
		private const string minLinesPath = "/filter/acquisition/MinLines/Value";

		// Token: 0x040011CF RID: 4559
		private const int maxPlotFiles = 10;

		// Token: 0x040011D0 RID: 4560
		public static readonly string plotFolderName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Plot";

		// Token: 0x040011D1 RID: 4561
		private DBConnection dBConnection;

		// Token: 0x040011D2 RID: 4562
		private ParameterHandling parHandling;

		// Token: 0x040011D3 RID: 4563
		private int oldScanrate;

		// Token: 0x040011D4 RID: 4564
		private string oldContinuousModeFiltering;

		// Token: 0x040011D5 RID: 4565
		private string oldContinuousMode;

		// Token: 0x040011D6 RID: 4566
		private string oldSendOnlyIfContaminant;

		// Token: 0x040011D7 RID: 4567
		private string oldSendImage;

		// Token: 0x040011D8 RID: 4568
		private string oldBulkFlapsValvesActive;

		// Token: 0x040011D9 RID: 4569
		private string oldSVTValvesActive;

		// Token: 0x040011DA RID: 4570
		private Timer timerUpdateParams;

		// Token: 0x040011DB RID: 4571
		private MachineState machineState;

		// Token: 0x040011DC RID: 4572
		private string scanratePath = "";

		// Token: 0x040011DD RID: 4573
		private DetectorType detectorType = DetectorType.DT;

		// Token: 0x040011DE RID: 4574
		private string serialNumberMachine;

		// Token: 0x040011DF RID: 4575
		private string serialNumberXRaySource;

		// Token: 0x040011E0 RID: 4576
		private double xRayOnTimeInternal;

		// Token: 0x040011E1 RID: 4577
		private string serialNumberDetector;

		// Token: 0x040011E2 RID: 4578
		private bool serialNumberXRayVisible;

		// Token: 0x040011E3 RID: 4579
		private double machineParametersFontSize;

		// Token: 0x040011E4 RID: 4580
		private ImageDataTransfer imageTransfer;

		// Token: 0x040011E5 RID: 4581
		private List<Point> plotPoints;

		// Token: 0x040011E6 RID: 4582
		private List<Point> referencePlotPoints = new List<Point>();

		// Token: 0x040011E7 RID: 4583
		private List<Point> plotNoTdiPoints;

		// Token: 0x040011E8 RID: 4584
		private List<Point> referencePlotNoTdiPoints = new List<Point>();

		// Token: 0x040011E9 RID: 4585
		private List<Point> ignoreLeftPoints;

		// Token: 0x040011EA RID: 4586
		private List<Point> ignoreRightPoints;

		// Token: 0x040011EB RID: 4587
		private int nPixel;

		// Token: 0x040011EC RID: 4588
		private int maxY;

		// Token: 0x040011ED RID: 4589
		private int minY;

		// Token: 0x040011EE RID: 4590
		private int ticksY;

		// Token: 0x040011EF RID: 4591
		private int ticksX;

		// Token: 0x040011F0 RID: 4592
		private int majorTicksX;

		// Token: 0x040011F1 RID: 4593
		private double zoomFactorX = 1.0;

		// Token: 0x040011F2 RID: 4594
		private List<Point> plotHEPoints = new List<Point>();

		// Token: 0x040011F3 RID: 4595
		private List<Point> referencePlotHEPoints = new List<Point>();

		// Token: 0x040011F4 RID: 4596
		private bool isDualEnergyDetector;

		// Token: 0x040011F5 RID: 4597
		private EnergyLevelMode energyMode = EnergyLevelMode.AllEnergies;

		// Token: 0x040011F6 RID: 4598
		private bool saveReferencePlot;

		// Token: 0x040011F7 RID: 4599
		private int highGainDualEnergy;

		// Token: 0x040011F8 RID: 4600
		private int lowGainDualEnergy;

		// Token: 0x040011F9 RID: 4601
		private int gainSingleEnergy;

		// Token: 0x040011FA RID: 4602
		private string detectorTypeString;

		// Token: 0x040011FB RID: 4603
		private string pixelSize;

		// Token: 0x040011FC RID: 4604
		private bool isTdiDetector;

		// Token: 0x040011FD RID: 4605
		private bool isTdiScanMode;

		// Token: 0x040011FE RID: 4606
		private bool isNoTdiScanMode;

		// Token: 0x040011FF RID: 4607
		private string oldLiveCalibrationMode;

		// Token: 0x04001200 RID: 4608
		private XrayState xrayState;

		// Token: 0x04001201 RID: 4609
		private int writeParameterRepeat = 5;
	}
}

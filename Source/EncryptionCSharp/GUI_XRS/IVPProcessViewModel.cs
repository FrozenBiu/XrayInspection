using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Common;
using DatabaseConnection;
using Definitions;
using GUI_XRS.Properties;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000078 RID: 120
	public class IVPProcessViewModel : INotifyPropertyChanged
	{
		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06000B13 RID: 2835 RVA: 0x00040E60 File Offset: 0x0003F060
		// (remove) Token: 0x06000B14 RID: 2836 RVA: 0x00040E98 File Offset: 0x0003F098
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00040ECD File Offset: 0x0003F0CD
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x00040ED5 File Offset: 0x0003F0D5
		public BoundLocalizedText StrData { get; set; }

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00040EDE File Offset: 0x0003F0DE
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x00040EE6 File Offset: 0x0003F0E6
		public BoundLocalizedText StrRequest { get; set; }

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x00040EEF File Offset: 0x0003F0EF
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x00040EF7 File Offset: 0x0003F0F7
		public BoundLocalizedText StrResult { get; set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000B1B RID: 2843 RVA: 0x00040F00 File Offset: 0x0003F100
		// (set) Token: 0x06000B1C RID: 2844 RVA: 0x00040F08 File Offset: 0x0003F108
		public BoundLocalizedText StrValidation { get; set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00040F11 File Offset: 0x0003F111
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x00040F19 File Offset: 0x0003F119
		public BoundLocalizedText StrProduct { get; set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000B1F RID: 2847 RVA: 0x00040F22 File Offset: 0x0003F122
		// (set) Token: 0x06000B20 RID: 2848 RVA: 0x00040F2A File Offset: 0x0003F12A
		public BoundLocalizedText StrTemperature { get; set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000B21 RID: 2849 RVA: 0x00040F33 File Offset: 0x0003F133
		// (set) Token: 0x06000B22 RID: 2850 RVA: 0x00040F3B File Offset: 0x0003F13B
		public BoundLocalizedText StrLength { get; set; }

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x00040F44 File Offset: 0x0003F144
		// (set) Token: 0x06000B24 RID: 2852 RVA: 0x00040F4C File Offset: 0x0003F14C
		public BoundLocalizedText StrWidth { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x00040F55 File Offset: 0x0003F155
		// (set) Token: 0x06000B26 RID: 2854 RVA: 0x00040F5D File Offset: 0x0003F15D
		public BoundLocalizedText StrHeight { get; set; }

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00040F66 File Offset: 0x0003F166
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x00040F6E File Offset: 0x0003F16E
		public BoundLocalizedText StrRate { get; set; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00040F77 File Offset: 0x0003F177
		// (set) Token: 0x06000B2A RID: 2858 RVA: 0x00040F7F File Offset: 0x0003F17F
		public BoundLocalizedText StrName { get; set; }

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00040F88 File Offset: 0x0003F188
		// (set) Token: 0x06000B2C RID: 2860 RVA: 0x00040F90 File Offset: 0x0003F190
		public BoundLocalizedText StrStreet { get; set; }

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x00040F99 File Offset: 0x0003F199
		// (set) Token: 0x06000B2E RID: 2862 RVA: 0x00040FA1 File Offset: 0x0003F1A1
		public BoundLocalizedText StrCity { get; set; }

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x00040FAA File Offset: 0x0003F1AA
		// (set) Token: 0x06000B30 RID: 2864 RVA: 0x00040FB2 File Offset: 0x0003F1B2
		public BoundLocalizedText StrState { get; set; }

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x00040FBB File Offset: 0x0003F1BB
		// (set) Token: 0x06000B32 RID: 2866 RVA: 0x00040FC3 File Offset: 0x0003F1C3
		public BoundLocalizedText StrPLZ { get; set; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00040FCC File Offset: 0x0003F1CC
		// (set) Token: 0x06000B34 RID: 2868 RVA: 0x00040FD4 File Offset: 0x0003F1D4
		public BoundLocalizedText StrAdditionalAddress { get; set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x00040FDD File Offset: 0x0003F1DD
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00040FE5 File Offset: 0x0003F1E5
		public BoundLocalizedText StrContactPerson { get; set; }

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00040FEE File Offset: 0x0003F1EE
		// (set) Token: 0x06000B38 RID: 2872 RVA: 0x00040FF6 File Offset: 0x0003F1F6
		public BoundLocalizedText StrEmail { get; set; }

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000B39 RID: 2873 RVA: 0x00040FFF File Offset: 0x0003F1FF
		// (set) Token: 0x06000B3A RID: 2874 RVA: 0x00041007 File Offset: 0x0003F207
		public BoundLocalizedText StrTelephone { get; set; }

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000B3B RID: 2875 RVA: 0x00041010 File Offset: 0x0003F210
		// (set) Token: 0x06000B3C RID: 2876 RVA: 0x00041018 File Offset: 0x0003F218
		public BoundLocalizedText StrOffice { get; set; }

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000B3D RID: 2877 RVA: 0x00041021 File Offset: 0x0003F221
		// (set) Token: 0x06000B3E RID: 2878 RVA: 0x00041029 File Offset: 0x0003F229
		public BoundLocalizedText StrServiceTechnician { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000B3F RID: 2879 RVA: 0x00041032 File Offset: 0x0003F232
		// (set) Token: 0x06000B40 RID: 2880 RVA: 0x0004103A File Offset: 0x0003F23A
		public BoundLocalizedText StrCustomerInformation { get; set; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000B41 RID: 2881 RVA: 0x00041043 File Offset: 0x0003F243
		// (set) Token: 0x06000B42 RID: 2882 RVA: 0x0004104B File Offset: 0x0003F24B
		public BoundLocalizedText StrManufacturerInformation { get; set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000B43 RID: 2883 RVA: 0x00041054 File Offset: 0x0003F254
		// (set) Token: 0x06000B44 RID: 2884 RVA: 0x0004105C File Offset: 0x0003F25C
		public BoundLocalizedText StrProductInformation { get; set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x00041065 File Offset: 0x0003F265
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x0004106D File Offset: 0x0003F26D
		public BoundLocalizedText StrOrderNumber { get; set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x00041076 File Offset: 0x0003F276
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x0004107E File Offset: 0x0003F27E
		public BoundLocalizedText StrConvey { get; set; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00041087 File Offset: 0x0003F287
		// (set) Token: 0x06000B4A RID: 2890 RVA: 0x0004108F File Offset: 0x0003F28F
		public BoundLocalizedText StrStartValidation { get; set; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00041098 File Offset: 0x0003F298
		// (set) Token: 0x06000B4C RID: 2892 RVA: 0x000410A0 File Offset: 0x0003F2A0
		public BoundLocalizedText StrPassed { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x000410A9 File Offset: 0x0003F2A9
		// (set) Token: 0x06000B4E RID: 2894 RVA: 0x000410B1 File Offset: 0x0003F2B1
		public BoundLocalizedText StrFailed { get; set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x000410BA File Offset: 0x0003F2BA
		// (set) Token: 0x06000B50 RID: 2896 RVA: 0x000410C2 File Offset: 0x0003F2C2
		public BoundLocalizedText StrCertificate { get; set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000B51 RID: 2897 RVA: 0x000410CB File Offset: 0x0003F2CB
		// (set) Token: 0x06000B52 RID: 2898 RVA: 0x000410D3 File Offset: 0x0003F2D3
		public BoundLocalizedText StrReport { get; set; }

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000B53 RID: 2899 RVA: 0x000410DC File Offset: 0x0003F2DC
		// (set) Token: 0x06000B54 RID: 2900 RVA: 0x000410E4 File Offset: 0x0003F2E4
		public BoundLocalizedText StrIfQuestions { get; set; }

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x000410ED File Offset: 0x0003F2ED
		// (set) Token: 0x06000B56 RID: 2902 RVA: 0x000410F5 File Offset: 0x0003F2F5
		public BoundLocalizedText StrIfQuestions2 { get; set; }

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x000410FE File Offset: 0x0003F2FE
		// (set) Token: 0x06000B58 RID: 2904 RVA: 0x00041106 File Offset: 0x0003F306
		public BoundLocalizedText StrTestballs { get; set; }

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000B59 RID: 2905 RVA: 0x0004110F File Offset: 0x0003F30F
		// (set) Token: 0x06000B5A RID: 2906 RVA: 0x00041117 File Offset: 0x0003F317
		public BoundLocalizedText StrProducts { get; set; }

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x00041120 File Offset: 0x0003F320
		// (set) Token: 0x06000B5C RID: 2908 RVA: 0x00041128 File Offset: 0x0003F328
		public BoundLocalizedText StrPusher { get; set; }

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x00041131 File Offset: 0x0003F331
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x00041139 File Offset: 0x0003F339
		public BoundLocalizedText StrLeadingEdge { get; set; }

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x00041142 File Offset: 0x0003F342
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0004114A File Offset: 0x0003F34A
		public BoundLocalizedText StrNumberContaminations { get; set; }

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x00041153 File Offset: 0x0003F353
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x0004115B File Offset: 0x0003F35B
		public BoundLocalizedText StrLane { get; set; }

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00041164 File Offset: 0x0003F364
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x0004116C File Offset: 0x0003F36C
		public BoundLocalizedText StrSubproduct { get; set; }

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00041175 File Offset: 0x0003F375
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0004117D File Offset: 0x0003F37D
		public BoundLocalizedText StrTestcard { get; set; }

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x00041186 File Offset: 0x0003F386
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x0004118E File Offset: 0x0003F38E
		public BoundLocalizedText StrMenuName { get; set; }

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00041198 File Offset: 0x0003F398
		// (set) Token: 0x06000B6A RID: 2922 RVA: 0x000411B0 File Offset: 0x0003F3B0
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

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06000B6B RID: 2923 RVA: 0x000411E4 File Offset: 0x0003F3E4
		// (remove) Token: 0x06000B6C RID: 2924 RVA: 0x0004121C File Offset: 0x0003F41C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action Close;

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06000B6D RID: 2925 RVA: 0x00041251 File Offset: 0x0003F451
		// (set) Token: 0x06000B6E RID: 2926 RVA: 0x00041259 File Offset: 0x0003F459
		public AccessRightsManager AccessManager { get; set; }

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06000B6F RID: 2927 RVA: 0x00041264 File Offset: 0x0003F464
		// (set) Token: 0x06000B70 RID: 2928 RVA: 0x0004127C File Offset: 0x0003F47C
		public string StringNumberContaminations
		{
			get
			{
				return this.stringNumberContaminations;
			}
			set
			{
				bool flag = value != this.stringNumberContaminations;
				if (flag)
				{
					this.stringNumberContaminations = value;
					this.OnPropertyChanged("StringNumbercontaminations");
				}
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06000B71 RID: 2929 RVA: 0x000412AF File Offset: 0x0003F4AF
		// (set) Token: 0x06000B72 RID: 2930 RVA: 0x000412B7 File Offset: 0x0003F4B7
		public User UserLoggedIn { get; set; }

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x000412C0 File Offset: 0x0003F4C0
		// (set) Token: 0x06000B74 RID: 2932 RVA: 0x000412D8 File Offset: 0x0003F4D8
		public int RemainingProducts
		{
			get
			{
				return this.remainingProducts;
			}
			set
			{
				bool flag = this.remainingProducts != value;
				if (flag)
				{
					this.remainingProducts = value;
					this.OnPropertyChanged("RemainingProducts");
				}
			}
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0004130C File Offset: 0x0003F50C
		// (set) Token: 0x06000B76 RID: 2934 RVA: 0x00041324 File Offset: 0x0003F524
		public int RemainingTestBalls
		{
			get
			{
				return this.remainingTestBalls;
			}
			set
			{
				bool flag = this.remainingTestBalls != value;
				if (flag)
				{
					this.remainingTestBalls = value;
					this.OnPropertyChanged("RemainingTestBalls");
				}
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000B77 RID: 2935 RVA: 0x00041358 File Offset: 0x0003F558
		// (set) Token: 0x06000B78 RID: 2936 RVA: 0x00041370 File Offset: 0x0003F570
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

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x000413A3 File Offset: 0x0003F5A3
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x000413AB File Offset: 0x0003F5AB
		public ObservableCollection<RejecterItem> UnlockBinItem { get; set; }

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x000413B4 File Offset: 0x0003F5B4
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x000413BC File Offset: 0x0003F5BC
		public RelayCommand UnlockBinCommand { get; set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x000413C8 File Offset: 0x0003F5C8
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x000413E0 File Offset: 0x0003F5E0
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

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x000413F8 File Offset: 0x0003F5F8
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x00041410 File Offset: 0x0003F610
		public bool Widescreen
		{
			get
			{
				return this.widescreen;
			}
			set
			{
				this.widescreen = value;
				this.OnPropertyChanged("Widescreen");
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x00041428 File Offset: 0x0003F628
		// (set) Token: 0x06000B82 RID: 2946 RVA: 0x00041440 File Offset: 0x0003F640
		public bool button1Enabled
		{
			get
			{
				return this.enabled1;
			}
			set
			{
				this.enabled1 = value;
				this.OnPropertyChanged("button1Enabled");
			}
		}

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x00041458 File Offset: 0x0003F658
		// (set) Token: 0x06000B84 RID: 2948 RVA: 0x00041470 File Offset: 0x0003F670
		public bool button2Enabled
		{
			get
			{
				return this.enabled2;
			}
			set
			{
				this.enabled2 = value;
				this.OnPropertyChanged("button2Enabled");
				bool flag = this.enabled2;
				if (flag)
				{
					this.TestsamplesList = this.initTestsampleList();
					LogbookManager.AddEntry(SSTEntryCode.IVPStarted);
				}
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000414B8 File Offset: 0x0003F6B8
		private ObservableCollection<InfosTestsamples> initTestsampleList()
		{
			ObservableCollection<InfosTestsamples> observableCollection = new ObservableCollection<InfosTestsamples>();
			List<string> list = new List<string>();
			bool flag = this.AccessManager.CurrentUser.ID < 0 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag)
			{
				MachineType machineType = MachineType.MachineTypeUndefined;
				this.dbConnection.GetMachineType(ref machineType);
				string text = "";
				this.dbConnection.GetMachineSubType(ref text);
				bool flag2 = machineType == MachineType.RayconEntry || (text.Contains("MX") && machineType == MachineType.RayconDPlus);
				if (flag2)
				{
					this.dbConnection.GetIVPTestcardRowReferenceProduct(1, ref list);
					bool flag3 = list[0] != null && list[0] != "n.a." && list[0] != "" && list[0] != "None";
					if (flag3)
					{
						observableCollection.Add(new InfosTestsamples
						{
							Description = this.convertStringToTestcardMaterial(list[0]),
							Value = list[2],
							TestcardSize = this.convertStringToTestcardType(list[1]),
							NumberFoundTestballs = 0,
							NumberTestballsToBeFound = Convert.ToInt32(list[3]),
							INNUmber = list[4],
							NumberProductsRejected = 0,
							Density = list[5]
						});
					}
				}
				else
				{
					this.dbConnection.GetIVPTestcardRowReferenceProduct(0, ref list);
					bool flag4 = list[0] != null && list[0] != "n.a." && list[0] != "" && list[0] != "None";
					if (flag4)
					{
						observableCollection.Add(new InfosTestsamples
						{
							Description = this.convertStringToTestcardMaterial(list[0]),
							Value = list[2],
							TestcardSize = this.convertStringToTestcardType(list[1]),
							NumberFoundTestballs = 0,
							NumberTestballsToBeFound = Convert.ToInt32(list[3]),
							INNUmber = list[4],
							NumberProductsRejected = 0,
							Density = list[5]
						});
					}
				}
				list = new List<string>();
				bool flag5 = machineType != MachineType.RayconEX2A;
				if (flag5)
				{
					this.dbConnection.GetIVPTestcardRowReferenceProduct(2, ref list);
					bool flag6 = list[0] != null && list[0] != "n.a." && list[0] != "" && list[0] != "None";
					if (flag6)
					{
						observableCollection.Add(new InfosTestsamples
						{
							Description = this.convertStringToTestcardMaterial(list[0]),
							Value = list[2],
							TestcardSize = this.convertStringToTestcardType(list[1]),
							NumberFoundTestballs = 0,
							NumberTestballsToBeFound = Convert.ToInt32(list[3]),
							INNUmber = list[4],
							NumberProductsRejected = 0,
							Density = list[5]
						});
					}
				}
				list = new List<string>();
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					this.dbConnection.GetIVPTestcardRow(i, ref list);
					bool flag7 = list[0] != null && list[0] != "n.a." && list[0] != "" && list[0] != "None";
					if (flag7)
					{
						observableCollection.Add(new InfosTestsamples
						{
							Description = this.convertStringToTestcardMaterial(list[0]),
							Value = list[2],
							TestcardSize = this.convertStringToTestcardType(list[1]),
							NumberFoundTestballs = 0,
							NumberTestballsToBeFound = Convert.ToInt32(list[3]),
							INNUmber = list[4],
							NumberProductsRejected = 0,
							Density = list[5]
						});
					}
					list = new List<string>();
				}
			}
			return observableCollection;
		}

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00041920 File Offset: 0x0003FB20
		// (set) Token: 0x06000B87 RID: 2951 RVA: 0x00041938 File Offset: 0x0003FB38
		public bool button3Enabled
		{
			get
			{
				return this.enabled3;
			}
			set
			{
				this.enabled3 = value;
				this.OnPropertyChanged("button3Enabled");
				bool flag = this.enabled3;
				if (flag)
				{
					bool flag2 = this.parHandling.ReadPar(Definition.contaminationsCounterEnablePath) == "1";
					if (flag2)
					{
						this.countCont = true;
					}
					else
					{
						this.countCont = false;
						this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "1");
					}
					bool flag3 = false;
					bool flag4 = this.rejectAll;
					if (flag4)
					{
						bool flag5 = this.parHandling.ReadPar(Definition.rejectEnable1Path).Equals("1");
						if (flag5)
						{
							this.parHandling.WriteValue(Definition.rejectAll1Path, "1");
							flag3 = true;
						}
						bool flag6 = this.parHandling.ReadPar(Definition.rejectEnable2Path).Equals("1");
						if (flag6)
						{
							this.parHandling.WriteValue(Definition.rejectAll2Path, "1");
							flag3 = true;
						}
						bool flag7 = this.parHandling.ReadPar(Definition.rejectEnable3Path).Equals("1");
						if (flag7)
						{
							this.parHandling.WriteValue(Definition.rejectAll3Path, "1");
							flag3 = true;
						}
						bool flag8 = this.parHandling.ReadPar(Definition.rejectEnable4Path).Equals("1");
						if (flag8)
						{
							this.parHandling.WriteValue(Definition.rejectAll4Path, "1");
							flag3 = true;
						}
					}
					bool flag9 = !flag3 && this.rejectAll;
					if (flag9)
					{
						bool flag10 = Application.Current != null;
						if (flag10)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								bool flag11 = Dialog.Show(LocalizationManager.Localize("txtInfo"), string.Concat(new string[]
								{
									LocalizationManager.Localize("txtRejecterIsDisabled"),
									". ",
									LocalizationManager.Localize("txtRejectAll"),
									" ",
									LocalizationManager.Localize("txtNotPossible")
								}), DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
								if (flag11)
								{
									this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
									this.StartTest();
								}
							}));
						}
					}
					else
					{
						this.imageTransfer.FrameArrived += this.imageTransfer_FrameArrived;
						this.StartTest();
					}
					this.numberTestsamples = this.TestsamplesList.Count<InfosTestsamples>();
					this.RemainingProducts = this.numberProducts;
					this.RemainingTestBalls = this.numberTestsamples;
					this.ButtonNextEnabled = false;
					this.TestsampleToConvey = string.Concat(new string[]
					{
						this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description),
						", ",
						this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value,
						" mm,",
						this.ConvertTestcard(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].TestcardSize),
						" (",
						this.StrTestcard.LocalizedText,
						" ",
						(1 + this.testsamplesList.Count<InfosTestsamples>() - this.remainingTestBalls).ToString(),
						"/",
						this.testsamplesList.Count<InfosTestsamples>().ToString(),
						")"
					});
					this.ProductsConveyed = string.Concat(new string[]
					{
						this.StrProducts.LocalizedText,
						": ",
						(this.numberProducts - this.RemainingProducts).ToString(),
						"/",
						this.numberProducts.ToString()
					});
				}
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000B88 RID: 2952 RVA: 0x00041C90 File Offset: 0x0003FE90
		// (set) Token: 0x06000B89 RID: 2953 RVA: 0x00041CA8 File Offset: 0x0003FEA8
		public bool button4Enabled
		{
			get
			{
				return this.enabled4;
			}
			set
			{
				this.enabled4 = value;
				bool flag = this.enabled4;
				if (flag)
				{
					bool result = this.getResult();
					bool flag2 = result;
					string[] name;
					if (flag2)
					{
						name = this.drawCertificate(true);
						bool flag3 = Application.Current != null;
						if (flag3)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtValidationSuccess"), DialogStyle.Ok, DialogButtons.OK);
							}));
						}
						bool flag4 = Application.Current != null;
						if (flag4)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								bool flag7 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtDoBackup") + "?", DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
								if (flag7)
								{
									this.doFullBackup(IVPProcessViewModel.backupFolder + name[0], name[0], true);
								}
								else
								{
									this.doFullBackup(IVPProcessViewModel.backupFolder + name[0], name[0], false);
								}
							}));
						}
					}
					else
					{
						name = this.drawCertificate(false);
						bool flag5 = Application.Current != null;
						if (flag5)
						{
							Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtValidationFailure"), DialogStyle.Warning, DialogButtons.OK);
							}));
						}
						this.doFullBackup(IVPProcessViewModel.backupFolder + name[0], name[0], false);
					}
					this.OrderNumber = "000";
					this.reset();
					this.ButtonNextEnabled = true;
					bool flag6 = this.certificateReady != null;
					if (flag6)
					{
						this.certificateReady(name[1]);
					}
				}
				this.OnPropertyChanged("button4Enabled");
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00041E26 File Offset: 0x00040026
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00041E2E File Offset: 0x0004002E
		public ImageSource imageForCertificate { get; set; }

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06000B8C RID: 2956 RVA: 0x00041E38 File Offset: 0x00040038
		// (remove) Token: 0x06000B8D RID: 2957 RVA: 0x00041E70 File Offset: 0x00040070
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> certificateReady;

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00041EA5 File Offset: 0x000400A5
		// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00041EAD File Offset: 0x000400AD
		public string success { get; set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000B90 RID: 2960 RVA: 0x00041EB8 File Offset: 0x000400B8
		// (set) Token: 0x06000B91 RID: 2961 RVA: 0x00041ED0 File Offset: 0x000400D0
		public bool Check
		{
			get
			{
				return this.check;
			}
			set
			{
				this.check = value;
				this.OnPropertyChanged("Check");
			}
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x00041EE8 File Offset: 0x000400E8
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x00041F00 File Offset: 0x00040100
		public bool ButtonNextEnabled
		{
			get
			{
				return this.enabledNext;
			}
			set
			{
				this.enabledNext = value;
				this.OnPropertyChanged("buttonNextEnabled");
			}
		}

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x00041F18 File Offset: 0x00040118
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x00041F30 File Offset: 0x00040130
		public string OrderNumber
		{
			get
			{
				return this.orderNumber;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.orderNumber = value;
					this.OnPropertyChanged("OrderNumber");
					bool flag2 = this.orderNumber.Length > 2;
					if (flag2)
					{
						this.ButtonNextEnabled = true;
					}
					else
					{
						this.ButtonNextEnabled = false;
					}
					this.OnPropertyChanged("ButtonNextEnabled");
				}
			}
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x00041F90 File Offset: 0x00040190
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x00041FA8 File Offset: 0x000401A8
		public string TestsampleToConvey
		{
			get
			{
				return this.testsampleToConvey;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.testsampleToConvey = value;
					this.OnPropertyChanged("TestsampleToConvey");
				}
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x00041FD4 File Offset: 0x000401D4
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x00041FEC File Offset: 0x000401EC
		public string ProductsConveyed
		{
			get
			{
				return this.productsConveyed;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.productsConveyed = value;
					this.OnPropertyChanged("ProductsConveyed");
				}
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x00042018 File Offset: 0x00040218
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x00042030 File Offset: 0x00040230
		public string TestsamplesConveyed
		{
			get
			{
				return this.testsamplesConveyed;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.testsamplesConveyed = value;
					this.OnPropertyChanged("TestsamplesConveyed");
				}
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0004205C File Offset: 0x0004025C
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x00042074 File Offset: 0x00040274
		public ObservableCollection<Infos> ProductList
		{
			get
			{
				return this.productList;
			}
			set
			{
				this.productList = value;
				this.OnPropertyChanged("ProductList");
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0004208C File Offset: 0x0004028C
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x000420A4 File Offset: 0x000402A4
		public ObservableCollection<InfosTestsamples> TestsamplesList
		{
			get
			{
				return this.testsamplesList;
			}
			set
			{
				this.testsamplesList = value;
				this.OnPropertyChanged("TestsamplesList");
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x000420BC File Offset: 0x000402BC
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x000420D4 File Offset: 0x000402D4
		public ObservableCollection<Infos> CustomerList
		{
			get
			{
				return this.customerList;
			}
			set
			{
				this.customerList = value;
				this.OnPropertyChanged("CustomerList");
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x000420EC File Offset: 0x000402EC
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x00042104 File Offset: 0x00040304
		public ObservableCollection<Infos> ManufacturerList
		{
			get
			{
				return this.manufacturerList;
			}
			set
			{
				this.manufacturerList = value;
				this.OnPropertyChanged("ManufacturerList");
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0004211A File Offset: 0x0004031A
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x00042122 File Offset: 0x00040322
		public RelayCommand CleanUpCommand { get; set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x0004212B File Offset: 0x0004032B
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x00042133 File Offset: 0x00040333
		public ParameterHandling parHandling { get; set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000BA8 RID: 2984 RVA: 0x0004213C File Offset: 0x0004033C
		// (set) Token: 0x06000BA9 RID: 2985 RVA: 0x00042144 File Offset: 0x00040344
		public MachineState machineState { get; set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0004214D File Offset: 0x0004034D
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x00042155 File Offset: 0x00040355
		public Backup backup { get; set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x00042160 File Offset: 0x00040360
		// (set) Token: 0x06000BAD RID: 2989 RVA: 0x00042178 File Offset: 0x00040378
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

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000BAE RID: 2990 RVA: 0x00042190 File Offset: 0x00040390
		// (set) Token: 0x06000BAF RID: 2991 RVA: 0x000421A8 File Offset: 0x000403A8
		public bool CustomerListVisible
		{
			get
			{
				return this.customerListVisible;
			}
			set
			{
				bool flag = this.customerListVisible != value;
				if (flag)
				{
					this.customerListVisible = value;
					this.OnPropertyChanged("CustomerListVisible");
				}
			}
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x000421DC File Offset: 0x000403DC
		public IVPProcessViewModel(XRSImageBox imageBox, AccessRightsManager accessManager, MachineState machineState, ParameterHandling parHandl, ImageDataTransfer imageTransfer, DBConnection dbCon, Backup backup, int productType, string number, MessageSystem messageSystem, ProductManager productManager)
		{
			this.Widescreen = (Definition.windowWidth >= 1300.0);
			this.dbConnection = dbCon;
			this.parHandling = parHandl;
			this.machineState = machineState;
			this.imageTransfer = imageTransfer;
			this.backup = backup;
			this.imageBox = imageBox;
			this.productManager = productManager;
			this.AccessManager = accessManager;
			this.dbConnection.GetDoorLockAvailable(ref this.doorLockAvailable);
			this.rejecter = new Rejecter(this.parHandling, this.dbConnection);
			this.UnlockBinItem = this.rejecter.UnlockBinItems;
			bool flag = productType == 1;
			if (flag)
			{
				this.isLane = true;
				this.isMultiProduct = false;
				this.nameSubproduct = number;
			}
			else
			{
				bool flag2 = productType == 2;
				if (flag2)
				{
					this.isLane = false;
					this.isMultiProduct = true;
					this.nameSubproduct = number;
				}
				else
				{
					this.isLane = false;
					this.isMultiProduct = false;
				}
			}
			this.InitValues();
			this.RegisterLocalizedText();
			this.InitLists();
			string text = this.ProductList[0].Value;
			bool flag3 = this.isLane || this.isMultiProduct;
			if (flag3)
			{
				text = text + ", Subpr. " + this.nameSubproduct;
			}
			this.ProductList[0].Value = text;
			this.UnlockBinCommand = new RelayCommand(new Action<object>(this.UnlockBin));
			this.CleanUpCommand = new RelayCommand(new Action<object>(this.CleanUp));
			bool flag4 = machineState.State == MachineStates.Inspecting;
			if (flag4)
			{
				this.timerCheck = new DispatcherTimer(new TimeSpan(0, 0, 2), DispatcherPriority.Normal, new EventHandler(this.checkMachineState), Application.Current.Dispatcher);
				machineState.State = MachineStates.AuditCheck;
				this.timerCheck.Start();
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Warning, DialogButtons.OK);
				this.OnClose();
			}
			this.OrderNumber = "000";
			this.button1Enabled = true;
			this.messageSystem = messageSystem;
			this.dbConnection.GetRejectAllIVP(ref this.rejectAll);
			bool flag5 = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag5)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "0");
			}
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x000424B0 File Offset: 0x000406B0
		private void checkMachineState(object sender, EventArgs e)
		{
			bool flag = this.machineState.State != MachineStates.AuditCheck;
			if (flag)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Error, DialogButtons.OK);
				this.OnClose();
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x00042504 File Offset: 0x00040704
		private void InitValues()
		{
			string value = "";
			bool flag = this.AccessManager.CurrentUser.ID < 0 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag)
			{
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.NumberOfProductsToConvey, ref value);
			}
			else
			{
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.NumberOfProductsToConvey, ref value);
			}
			this.numberProducts = Convert.ToInt32(value);
			this.numberContaminations = 1;
			this.button1Enabled = true;
			this.button2Enabled = false;
			this.button3Enabled = false;
			this.button4Enabled = false;
			this.ButtonNextEnabled = false;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x000425B4 File Offset: 0x000407B4
		private void InitLists()
		{
			string value = "";
			ObservableCollection<Infos> observableCollection = new ObservableCollection<Infos>();
			bool flag = this.AccessManager.CurrentUser.ID < 0 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag)
			{
				this.dbConnection.GetCurrentAppName(ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrProduct.LocalizedText + ":",
					Value = value,
					DBPath = DBIntegratedValidationProcessReferenceProduct.ProductName
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductLength, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrLength.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductWidth, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrWidth.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductHeight, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrHeight.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductTemperature, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrTemperature.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductPackRate, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrRate.LocalizedText + ":",
					Value = value
				});
				this.ProductList = observableCollection;
			}
			else
			{
				this.dbConnection.GetCurrentAppName(ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrProduct.LocalizedText + ":",
					Value = value,
					DBPath = DBIntegratedValidationProcess.ProductName
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductLength, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrLength.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductWidth, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrWidth.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductHeight, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrHeight.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductTemperature, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrTemperature.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductPackRate, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrRate.LocalizedText + ":",
					Value = value
				});
				this.ProductList = observableCollection;
			}
			bool flag2 = this.AccessManager.CurrentUser.ID == -3 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag2)
			{
				this.CustomerListVisible = false;
				observableCollection = new ObservableCollection<Infos>();
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerOffice, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrOffice.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerStreet, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrStreet.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerLocation, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrCity.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerState, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrState.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerPostalCode, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrPLZ.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerAdditionalAdress, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrAdditionalAddress.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerServiceTechnician, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrServiceTechnician.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerEMail, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrEmail.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ManufacturerPhone, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrTelephone.LocalizedText + ":",
					Value = value
				});
				this.ManufacturerList = observableCollection;
			}
			else
			{
				this.CustomerListVisible = true;
				observableCollection = new ObservableCollection<Infos>();
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerName, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrName.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerStreet, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrStreet.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerLocation, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrCity.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerPostalCode, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrPLZ.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerState, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrState.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerAdditionalAdress, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrAdditionalAddress.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerContactPerson, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrContactPerson.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerEMail, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrEmail.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.CustomerPhone, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrTelephone.LocalizedText + ":",
					Value = value
				});
				this.CustomerList = observableCollection;
				observableCollection = new ObservableCollection<Infos>();
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerOffice, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrOffice.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerStreet, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrStreet.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerLocation, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrCity.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerState, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrState.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerPostalCode, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrPLZ.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerAdditionalAdress, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrAdditionalAddress.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerServiceTechnician, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrServiceTechnician.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerEMail, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrEmail.LocalizedText + ":",
					Value = value
				});
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ManufacturerPhone, ref value);
				observableCollection.Add(new Infos(this.dbConnection)
				{
					Description = this.StrTelephone.LocalizedText + ":",
					Value = value
				});
				this.ManufacturerList = observableCollection;
			}
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x000431C8 File Offset: 0x000413C8
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x000431F8 File Offset: 0x000413F8
		public void setResult(bool result)
		{
			this.Check = false;
			bool flag = !result;
			if (flag)
			{
				bool flag2 = Application.Current != null;
				if (flag2)
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
					{
						bool flag4 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtContinueValidation"), DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
						if (flag4)
						{
							this.AbortTest();
						}
						else
						{
							bool flag5 = this.AccessManager.CurrentUser.ID == -3 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
							if (flag5)
							{
								this.countRejectedProducts++;
								bool flag6 = this.RemainingProducts == 0;
								if (flag6)
								{
									this.Update(this.CurrentImage);
								}
								else
								{
									this.imageQueue.Remove(this.CurrentImage);
									bool flag7 = this.imageQueue.Count > 0;
									if (flag7)
									{
										this.workWithImage();
									}
								}
							}
							else
							{
								bool flag8 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtConfirmRejection"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
								if (flag8)
								{
									this.countRejectedProducts++;
									bool flag9 = this.RemainingProducts == 0;
									if (flag9)
									{
										this.Update(this.CurrentImage);
									}
									else
									{
										this.imageQueue.Remove(this.CurrentImage);
										bool flag10 = this.imageQueue.Count > 0;
										if (flag10)
										{
											this.workWithImage();
										}
									}
								}
								else
								{
									bool flag11 = this.RemainingProducts == 0;
									if (flag11)
									{
										this.Update(this.CurrentImage);
									}
									else
									{
										this.imageQueue.Remove(this.CurrentImage);
										bool flag12 = this.imageQueue.Count > 0;
										if (flag12)
										{
											this.workWithImage();
										}
									}
								}
							}
						}
					}), new object[0]);
				}
			}
			else
			{
				this.countFoundSamples++;
				bool flag3 = Application.Current != null;
				if (flag3)
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
					{
						bool flag4 = this.AccessManager.CurrentUser.ID == -3 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
						if (flag4)
						{
							this.countRejectedProducts++;
							bool flag5 = this.RemainingProducts == 0;
							if (flag5)
							{
								this.Update(this.CurrentImage);
							}
							else
							{
								this.imageQueue.Remove(this.CurrentImage);
								bool flag6 = this.imageQueue.Count > 0;
								if (flag6)
								{
									this.workWithImage();
								}
							}
						}
						else
						{
							bool flag7 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtConfirmRejection"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
							if (flag7)
							{
								this.countRejectedProducts++;
								bool flag8 = this.RemainingProducts == 0;
								if (flag8)
								{
									this.Update(this.CurrentImage);
								}
								else
								{
									this.imageQueue.Remove(this.CurrentImage);
									bool flag9 = this.imageQueue.Count > 0;
									if (flag9)
									{
										this.workWithImage();
									}
								}
							}
							else
							{
								bool flag10 = this.RemainingProducts == 0;
								if (flag10)
								{
									this.Update(this.CurrentImage);
								}
								else
								{
									this.imageQueue.Remove(this.CurrentImage);
									bool flag11 = this.imageQueue.Count > 0;
									if (flag11)
									{
										this.workWithImage();
									}
								}
							}
						}
					}), new object[0]);
				}
			}
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0004328C File Offset: 0x0004148C
		private void RegisterLocalizedText()
		{
			this.StrValidation = new BoundLocalizedText("txtValidation");
			this.StrData = new BoundLocalizedText("txtData");
			this.StrRequest = new BoundLocalizedText("txtRequest");
			this.StrResult = new BoundLocalizedText("txtResult");
			this.StrProduct = new BoundLocalizedText("txtApplication");
			this.StrTemperature = new BoundLocalizedText("txtTemperature");
			this.StrLength = new BoundLocalizedText("txtLengthProduct");
			this.StrWidth = new BoundLocalizedText("txtWidth");
			this.StrHeight = new BoundLocalizedText("txtHeight");
			this.StrRate = new BoundLocalizedText("txtRate");
			this.StrName = new BoundLocalizedText("txtName");
			this.StrStreet = new BoundLocalizedText("txtStreet");
			this.StrCity = new BoundLocalizedText("txtLocation");
			this.StrPLZ = new BoundLocalizedText("txtPLZ");
			this.StrAdditionalAddress = new BoundLocalizedText("txtAdditionalAddress");
			this.StrContactPerson = new BoundLocalizedText("txtContactPerson");
			this.StrEmail = new BoundLocalizedText("txtEmail");
			this.StrTelephone = new BoundLocalizedText("txtTelephone");
			this.StrServiceTechnician = new BoundLocalizedText("txtServiceTechnician");
			this.StrOffice = new BoundLocalizedText("txtOffice");
			this.StrManufacturerInformation = new BoundLocalizedText("txtInformationManufacturer");
			this.StrCustomerInformation = new BoundLocalizedText("txtInformationCustomer");
			this.StrProductInformation = new BoundLocalizedText("txtInformationProduct");
			this.StrOrderNumber = new BoundLocalizedText("txtOrderNumber");
			this.StrConvey = new BoundLocalizedText("txtConveyContaminantProduct");
			this.StrStartValidation = new BoundLocalizedText("txtStartValidation");
			this.StrState = new BoundLocalizedText("txtState");
			this.StrTestballs = new BoundLocalizedText("txtTestballs");
			this.StrProducts = new BoundLocalizedText("txtApplication");
			this.StrNumberContaminations = new BoundLocalizedText("txtNumberContaminations");
			this.StrTestcard = new BoundLocalizedText("txtTestcard");
			this.StrLane = new BoundLocalizedText("txtLane");
			this.StrMenuName = new BoundLocalizedText("menuItemIVPStart");
			this.StrPassed = new BoundLocalizedText("txtPassed");
			this.StrFailed = new BoundLocalizedText("txtFailed");
			this.StrCertificate = new BoundLocalizedText("txtCertificate");
			this.StrReport = new BoundLocalizedText("txtReport");
			this.StrIfQuestions = new BoundLocalizedText("txtIfQuestions");
			this.StrIfQuestions2 = new BoundLocalizedText("txtIfQuestions2");
			this.StrPusher = new BoundLocalizedText("txtRejectModeCenter");
			this.StrLeadingEdge = new BoundLocalizedText("txtRejectModeLeadingEdge");
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00043554 File Offset: 0x00041754
		public void DeregisterLocalizedText()
		{
			MainWindow.localizationManager.Deregister(this.StrValidation);
			MainWindow.localizationManager.Deregister(this.StrData);
			MainWindow.localizationManager.Deregister(this.StrRequest);
			MainWindow.localizationManager.Deregister(this.StrResult);
			MainWindow.localizationManager.Deregister(this.StrProduct);
			MainWindow.localizationManager.Deregister(this.StrTemperature);
			MainWindow.localizationManager.Deregister(this.StrLength);
			MainWindow.localizationManager.Deregister(this.StrWidth);
			MainWindow.localizationManager.Deregister(this.StrHeight);
			MainWindow.localizationManager.Deregister(this.StrRate);
			MainWindow.localizationManager.Deregister(this.StrName);
			MainWindow.localizationManager.Deregister(this.StrStreet);
			MainWindow.localizationManager.Deregister(this.StrCity);
			MainWindow.localizationManager.Deregister(this.StrPLZ);
			MainWindow.localizationManager.Deregister(this.StrAdditionalAddress);
			MainWindow.localizationManager.Deregister(this.StrContactPerson);
			MainWindow.localizationManager.Deregister(this.StrEmail);
			MainWindow.localizationManager.Deregister(this.StrTelephone);
			MainWindow.localizationManager.Deregister(this.StrServiceTechnician);
			MainWindow.localizationManager.Deregister(this.StrOffice);
			MainWindow.localizationManager.Deregister(this.StrCustomerInformation);
			MainWindow.localizationManager.Deregister(this.StrManufacturerInformation);
			MainWindow.localizationManager.Deregister(this.StrProductInformation);
			MainWindow.localizationManager.Deregister(this.StrOrderNumber);
			MainWindow.localizationManager.Deregister(this.StrConvey);
			MainWindow.localizationManager.Deregister(this.StrStartValidation);
			MainWindow.localizationManager.Deregister(this.StrState);
			MainWindow.localizationManager.Deregister(this.StrPassed);
			MainWindow.localizationManager.Deregister(this.StrFailed);
			MainWindow.localizationManager.Deregister(this.StrCertificate);
			MainWindow.localizationManager.Deregister(this.StrReport);
			MainWindow.localizationManager.Deregister(this.StrIfQuestions);
			MainWindow.localizationManager.Deregister(this.StrIfQuestions2);
			MainWindow.localizationManager.Deregister(this.StrPusher);
			MainWindow.localizationManager.Deregister(this.StrLeadingEdge);
			MainWindow.localizationManager.Deregister(this.StrProducts);
			MainWindow.localizationManager.Deregister(this.StrTestballs);
			MainWindow.localizationManager.Deregister(this.StrNumberContaminations);
			MainWindow.localizationManager.Deregister(this.StrTestcard);
			MainWindow.localizationManager.Deregister(this.StrLane);
			MainWindow.localizationManager.Deregister(this.StrMenuName);
		}

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0004381C File Offset: 0x00041A1C
		public void ShowKeyboard()
		{
			string oldValue = this.OrderNumber;
			this.keyboard = new Keyboard(this.StrOrderNumber.LocalizedText, oldValue, KeyboardType.Default);
			this.keyboard.TextInputKeyboard += this.keyboard_result;
			this.keyboard.Show();
		}

		// Token: 0x06000BB9 RID: 3001 RVA: 0x00043870 File Offset: 0x00041A70
		private void keyboard_result(object sender, KeyboardInputEventArgs e)
		{
			bool flag = e.Text != null;
			if (flag)
			{
				string text = e.Text;
				bool flag2 = text.Length > 30;
				if (flag2)
				{
					text = text.Substring(0, 30);
				}
				this.OrderNumber = e.Text;
				this.OnPropertyChanged("OrderNumber");
				this.keyboard.TextInputKeyboard -= this.keyboard_result;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x000438E0 File Offset: 0x00041AE0
		protected virtual void OnClose()
		{
			Action close = this.Close;
			bool flag = close != null;
			if (flag)
			{
				close();
			}
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x00043908 File Offset: 0x00041B08
		public void backToStart()
		{
			this.reset();
			this.button1Enabled = true;
			this.button2Enabled = false;
			this.button3Enabled = false;
			this.button4Enabled = false;
			foreach (InfosTestsamples infosTestsamples in this.TestsamplesList)
			{
				infosTestsamples.NumberFoundTestballs = 0;
				infosTestsamples.NumberProductsRejected = 0;
			}
			this.imageBox.Reset();
			this.Check = false;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x000439A0 File Offset: 0x00041BA0
		private void AbortTest()
		{
			this.backToStart();
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000439AC File Offset: 0x00041BAC
		public bool ShowImage(XRSImage image)
		{
			bool result = false;
			bool flag = image.Type == ImageType.IVP;
			if (flag)
			{
				bool flag2 = this.isLane || this.isMultiProduct;
				if (flag2)
				{
					bool flag3 = image.SubProductName == this.nameSubproduct;
					if (flag3)
					{
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			else
			{
				bool flag4 = image.Type != ImageType.IVP;
				if (flag4)
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), "NOT IVP IMAGE", DialogStyle.Warning, DialogButtons.OK);
				}
			}
			return result;
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00043A38 File Offset: 0x00041C38
		private void StartTest()
		{
			bool flag = this.machineState.State == MachineStates.AuditCheck;
			if (flag)
			{
				bool flag2 = this.AccessManager.CurrentUser.ID != 0;
				if (flag2)
				{
					this.AccessManager.StopTimerAutoLogout();
					this.parHandling.WriteValue(Definition.inIVPModePath, "1", false);
					this.numberOfImages = 0;
					this.UserLogAvailable = true;
					this.countFoundSamples = 0;
					this.TestsampleToConvey = string.Concat(new string[]
					{
						this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description),
						", ",
						this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value,
						" mm, ",
						this.ConvertTestcard(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].TestcardSize),
						" (",
						this.StrTestcard.LocalizedText,
						" ",
						(1 + this.testsamplesList.Count<InfosTestsamples>() - this.remainingTestBalls).ToString(),
						"/",
						this.testsamplesList.Count<InfosTestsamples>().ToString(),
						")"
					});
					this.numberContaminations = this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberTestballsToBeFound;
					this.StringNumberContaminations = this.StrNumberContaminations.LocalizedText + ": " + this.numberContaminations.ToString();
				}
				else
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgLogin"), DialogStyle.Warning, DialogButtons.OK);
				}
			}
			else
			{
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtTurnXrayOn") + ".", DialogStyle.Warning, DialogButtons.OK);
				this.AbortTest();
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00043C44 File Offset: 0x00041E44
		private void imageTransfer_FrameArrived(object sender, ReceiveEventArgs e)
		{
			bool flag = e.Image.Type != ImageType.Histogram && e.Image.Type != ImageType.MaterialCurve && e.Image.Type != ImageType.TestcardSimulation;
			if (flag)
			{
				bool flag2 = this.ShowImage(e.Image);
				if (flag2)
				{
					this.numberOfImages++;
					bool flag3 = this.numberOfImages >= this.numberTestsamples * this.numberProducts;
					if (flag3)
					{
						this.parHandling.WriteValue(Definition.inIVPModePath, "0", false);
						this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
					}
					this.imageQueue.Add(e.Image);
					bool flag4 = this.imageQueue.Count == 1;
					if (flag4)
					{
						this.workWithImage();
					}
				}
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00043D28 File Offset: 0x00041F28
		public void workWithImage()
		{
			bool flag = this.imageQueue != null && this.imageQueue.Count<XRSImage>() > 0;
			if (flag)
			{
				XRSImage image = this.imageQueue.First<XRSImage>();
				this.CurrentImage = image;
				this.imageBox.ViewXRSImage(image);
				int number = this.ImageConformsToTest(image);
				bool flag2 = number == 0;
				if (flag2)
				{
					bool flag3 = Application.Current != null;
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
						{
							bool flag7 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNumberContaminationsRight"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
							if (flag7)
							{
								this.countRejectedProducts++;
							}
							this.countFoundSamples++;
							this.setResultAfterDialog(number, image);
						}), new object[0]);
					}
				}
				else
				{
					bool flag4 = number == 1;
					if (flag4)
					{
						bool flag5 = Application.Current != null;
						if (flag5)
						{
							Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
							{
								Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtNumberContaminationsMore"), DialogStyle.Warning, DialogButtons.OK);
								this.Check = true;
								int num = this.RemainingProducts;
								this.RemainingProducts = num - 1;
								this.ProductsConveyed = string.Concat(new string[]
								{
									this.StrProducts.LocalizedText,
									": ",
									(this.numberProducts - this.RemainingProducts).ToString(),
									"/",
									this.numberProducts.ToString()
								});
							}), new object[0]);
						}
					}
					else
					{
						bool flag6 = Application.Current != null;
						if (flag6)
						{
							Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
							{
								bool flag7 = Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtNumberContaminationsLess"), DialogStyle.Error, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
								if (flag7)
								{
									this.AbortTest();
								}
								else
								{
									bool flag8 = this.AccessManager.CurrentUser.ID == -3 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
									if (flag8)
									{
										this.countRejectedProducts++;
										this.setResultAfterDialog(number, image);
									}
									else
									{
										bool flag9 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtConfirmRejection"), DialogStyle.Info, DialogButtons.OKCancel) == Dialog.DialogResults.Ok;
										if (flag9)
										{
											this.countRejectedProducts++;
											this.setResultAfterDialog(number, image);
										}
										else
										{
											this.setResultAfterDialog(number, image);
										}
									}
								}
							}), new object[0]);
						}
					}
				}
			}
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00043E64 File Offset: 0x00042064
		public void setResultAfterDialog(int number, XRSImage image)
		{
			int num = this.RemainingProducts;
			this.RemainingProducts = num - 1;
			this.ProductsConveyed = string.Concat(new string[]
			{
				this.StrProducts.LocalizedText,
				": ",
				(this.numberProducts - this.RemainingProducts).ToString(),
				"/",
				this.numberProducts.ToString()
			});
			bool flag = number != 1;
			if (flag)
			{
				bool flag2 = this.RemainingProducts == 0;
				if (flag2)
				{
					this.Update(image);
				}
				else
				{
					this.imageQueue.Remove(image);
					bool flag3 = this.imageQueue.Count > 0;
					if (flag3)
					{
						this.workWithImage();
					}
				}
			}
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00043F2C File Offset: 0x0004212C
		public void Update(XRSImage image)
		{
			this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberFoundTestballs = this.countFoundSamples;
			this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberProductsRejected = this.countRejectedProducts;
			LogbookManager.AddEntry(SSTEntryCode.IVPTestballFinished, 0, this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description), this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value, this.countFoundSamples.ToString(), this.countRejectedProducts.ToString(), this.numberProducts.ToString());
			this.countFoundSamples = 0;
			this.countRejectedProducts = 0;
			int num = this.RemainingTestBalls;
			this.RemainingTestBalls = num - 1;
			bool flag = this.RemainingTestBalls == 0;
			if (flag)
			{
				this.button3Enabled = false;
				this.button4Enabled = true;
			}
			else
			{
				this.RemainingProducts = this.numberProducts;
				this.ProductsConveyed = string.Concat(new string[]
				{
					this.StrProducts.LocalizedText,
					": ",
					(this.numberProducts - this.RemainingProducts).ToString(),
					"/",
					this.numberProducts.ToString()
				});
				this.TestsampleToConvey = string.Concat(new string[]
				{
					this.ConvertMaterial(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Description),
					", ",
					this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].Value,
					" mm, ",
					this.ConvertTestcard(this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].TestcardSize),
					" (",
					this.StrTestcard.LocalizedText,
					" ",
					(1 + this.testsamplesList.Count<InfosTestsamples>() - this.remainingTestBalls).ToString(),
					"/",
					this.testsamplesList.Count<InfosTestsamples>().ToString(),
					")"
				});
				this.numberContaminations = this.TestsamplesList[this.numberTestsamples - this.RemainingTestBalls].NumberTestballsToBeFound;
				this.StringNumberContaminations = this.StrNumberContaminations.LocalizedText + ": " + this.numberContaminations.ToString();
				bool flag2 = Application.Current != null;
				if (flag2)
				{
					Application.Current.Dispatcher.BeginInvoke(new Action(delegate()
					{
						bool flag3 = Dialog.Show(LocalizationManager.Localize("txtInfo"), LocalizationManager.Localize("txtNextTestsample") + ": " + this.TestsampleToConvey, DialogStyle.Info, DialogButtons.OK) == Dialog.DialogResults.Ok;
						if (flag3)
						{
							this.imageQueue.Remove(image);
							bool flag4 = this.imageQueue.Count > 0;
							if (flag4)
							{
								this.workWithImage();
							}
						}
					}), new object[0]);
				}
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0004421C File Offset: 0x0004241C
		private int ImageConformsToTest(XRSImage image)
		{
			bool flag = (int)image.ContaminationCount == this.numberContaminations;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = (int)image.ContaminationCount >= this.numberContaminations;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
			}
			return result;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00044262 File Offset: 0x00042462
		public void CheckUserRightforUnlockBin(Button btnUnlockBin)
		{
			this.AccessManager.RegisterControl(btnUnlockBin, Rejecter.doVerifyTestRight, Rejecter.doVerifyTestRight);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0004427C File Offset: 0x0004247C
		public void DeregisterUnlockBinButton(Button btnUnlockBin)
		{
			this.AccessManager.DeregisterControl(btnUnlockBin);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0004428C File Offset: 0x0004248C
		private void UnlockBin(object obj)
		{
			int binTimeout = 0;
			this.dbConnection.GetMSBinTimeout(true, ref binTimeout);
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

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00044304 File Offset: 0x00042504
		public void reset()
		{
			this.imageTransfer.FrameArrived -= this.imageTransfer_FrameArrived;
			this.parHandling.WriteValue(Definition.inIVPModePath, "0", false);
			bool flag = !this.countCont;
			if (flag)
			{
				this.parHandling.WriteValue(Definition.contaminationsCounterEnablePath, "0");
			}
			this.parHandling.WriteValue(Definition.rejectAll1Path, "0");
			this.parHandling.WriteValue(Definition.rejectAll2Path, "0");
			this.parHandling.WriteValue(Definition.rejectAll3Path, "0");
			this.parHandling.WriteValue(Definition.rejectAll4Path, "0");
			this.CurrentImage = null;
			bool flag2 = this.button3Enabled || this.button2Enabled;
			if (flag2)
			{
				LogbookManager.AddEntry(SSTEntryCode.IVPCancelled);
			}
			this.RemainingProducts = this.numberProducts;
			this.RemainingTestBalls = this.numberTestsamples;
			this.countFoundSamples = 0;
			this.countRejectedProducts = 0;
			this.AccessManager.StartTimerAutoLogout();
			bool flag3 = this.imageQueue != null;
			if (flag3)
			{
				this.imageQueue.Clear();
			}
			this.OrderNumber = "000";
			bool flag4 = this.machineState.State != MachineStates.AuditCheck;
			if (flag4)
			{
				this.OnClose();
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00044460 File Offset: 0x00042660
		public bool getResult()
		{
			bool result = true;
			foreach (InfosTestsamples infosTestsamples in this.TestsamplesList)
			{
				bool flag = this.numberProducts != infosTestsamples.NumberFoundTestballs || this.numberProducts != infosTestsamples.NumberProductsRejected;
				if (flag)
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000444DC File Offset: 0x000426DC
		private string ConvertMaterial(Material value)
		{
			string result = LocalizationManager.Localize("txtNA");
			switch (value)
			{
			case Material.VA:
				result = LocalizationManager.Localize("txtVA");
				break;
			case Material.Glass:
				result = LocalizationManager.Localize("txtGlass");
				break;
			case Material.VAWire:
				result = LocalizationManager.Localize("txtVAWire");
				break;
			case Material.Ceramics:
				result = LocalizationManager.Localize("txtKeramik");
				break;
			case Material.EPDM:
				result = LocalizationManager.Localize("txtEPDM");
				break;
			case Material.PTFE:
				result = LocalizationManager.Localize("txtPTFE");
				break;
			case Material.Aluminium:
				result = LocalizationManager.Localize("txtAluminium");
				break;
			case Material.Bone:
				result = LocalizationManager.Localize("txtBone");
				break;
			case Material.None:
				result = LocalizationManager.Localize("txtNA");
				break;
			case Material.Other:
				result = LocalizationManager.Localize("txtOther");
				break;
			case Material.FE:
				result = LocalizationManager.Localize("txtFE");
				break;
			case Material.NonFE:
				result = LocalizationManager.Localize("txtNonFE");
				break;
			}
			return result;
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000445DC File Offset: 0x000427DC
		private string ConvertTestcard(TestCardSize value)
		{
			string result = "";
			switch (value)
			{
			case TestCardSize.SingleBall:
				result = LocalizationManager.Localize("txtSingleBall");
				break;
			case TestCardSize.Testcard5:
				result = LocalizationManager.Localize("txtTestCard5");
				break;
			case TestCardSize.Testcard100:
				result = LocalizationManager.Localize("txtTestCard100");
				break;
			case TestCardSize.Other:
				result = LocalizationManager.Localize("txtOther");
				break;
			case TestCardSize.Testcard5Same:
				result = LocalizationManager.Localize("txtTestCard5Same");
				break;
			}
			return result;
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00044658 File Offset: 0x00042858
		public string[] drawCertificate(bool passed)
		{
			bool flag = this.AccessManager.CurrentUser.ID == -3 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			PdfDocument pdfDocument = new PdfDocument();
			PdfPage pdfPage = pdfDocument.AddPage();
			XSize xsize = PageSizeConverter.ToSize(PageSize.A4);
			pdfPage.Height = xsize.Height;
			pdfPage.Width = xsize.Width;
			string text = "";
			this.dbConnection.GetLanguage(ref text);
			bool flag2 = text.Equals("de-DE") && !flag;
			XImage image;
			if (flag2)
			{
				Bitmap sesotec_certificate_raycon_de_ = Resources.sesotec_certificate_raycon_de_2023;
				MemoryStream stream = new MemoryStream();
				sesotec_certificate_raycon_de_.Save(stream, ImageFormat.Png);
				image = XImage.FromStream(stream);
			}
			else
			{
				Bitmap sesotec_certificate_raycon_en_ = Resources.sesotec_certificate_raycon_en_2023;
				MemoryStream stream2 = new MemoryStream();
				sesotec_certificate_raycon_en_.Save(stream2, ImageFormat.Png);
				image = XImage.FromStream(stream2);
			}
			XGraphics xgraphics = XGraphics.FromPdfPage(pdfPage);
			xgraphics.DrawImage(image, 0.0, 0.0);
			XFont font = new XFont("Calibri", 40.0);
			bool flag3 = this.AccessManager.CurrentUser.ID == -1 || this.AccessManager.CurrentUser.ID == -2 || this.AccessManager.CurrentUser.ID == -3;
			if (flag3)
			{
				if (passed)
				{
					bool flag4 = flag;
					if (flag4)
					{
						xgraphics.DrawString("Certificate", font, XBrushes.DarkGray, 60.0, 180.0);
					}
					else
					{
						xgraphics.DrawString(this.StrCertificate.LocalizedText, font, XBrushes.DarkGray, 60.0, 180.0);
					}
					this.updateNextValidationDate(this.AccessManager.CurrentUser.ID == -3);
					this.dbConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.LastValidationDate, DateTime.Today.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
				}
				else
				{
					bool flag5 = flag;
					if (flag5)
					{
						xgraphics.DrawString("Report", font, XBrushes.DarkGray, 60.0, 180.0);
					}
					else
					{
						xgraphics.DrawString(this.StrReport.LocalizedText, font, XBrushes.DarkGray, 60.0, 180.0);
					}
				}
				bool flag6 = this.AccessManager.CurrentUser.ID == -2;
				if (flag6)
				{
					switch (this.dbConnection.GetDesign())
					{
					case Definition.GUIDesign.Bizerba:
					{
						Bitmap bizerba_powered_by_sesotec = Resources.bizerba_powered_by_sesotec;
						MemoryStream stream3 = new MemoryStream();
						bizerba_powered_by_sesotec.Save(stream3, ImageFormat.Png);
						xgraphics.DrawImage(XImage.FromStream(stream3), 57.0, 57.0, 300.0, 75.0);
						break;
					}
					}
				}
				else
				{
					bool flag7 = this.AccessManager.CurrentUser.ID == -3;
					if (flag7)
					{
						Definition.GUIDesign design = this.dbConnection.GetDesign();
						bool flag8 = design == Definition.GUIDesign.Bizerba;
						if (flag8)
						{
							Bitmap bizerba_powered_by_sesotec2 = Resources.bizerba_powered_by_sesotec;
							MemoryStream stream4 = new MemoryStream();
							bizerba_powered_by_sesotec2.Save(stream4, ImageFormat.Png);
							xgraphics.DrawImage(XImage.FromStream(stream4), 57.0, 57.0, 300.0, 75.0);
						}
						else
						{
							Bitmap suS_logo = Resources.SuS_logo;
							MemoryStream stream5 = new MemoryStream();
							suS_logo.Save(stream5, ImageFormat.Png);
							xgraphics.DrawImage(XImage.FromStream(stream5), 57.0, 57.0, 300.0, 72.0);
						}
					}
					else
					{
						Bitmap suS_logo2 = Resources.SuS_logo;
						MemoryStream stream6 = new MemoryStream();
						suS_logo2.Save(stream6, ImageFormat.Png);
						xgraphics.DrawImage(XImage.FromStream(stream6), 57.0, 57.0, 300.0, 72.0);
					}
				}
				XTextFormatter xtextFormatter = new XTextFormatter(xgraphics);
				bool flag9 = flag;
				if (flag9)
				{
					xtextFormatter.DrawString("For any questions or to schedule your next validation please contact Sesotec at " + this.ManufacturerList.Last<Infos>().Value + " or at " + this.ManufacturerList[this.ManufacturerList.Count<Infos>() - 2].Value, new XFont("Calibri", 8.0), XBrushes.DarkGray, new XRect(57.0, 740.0, 520.0, 40.0));
				}
				else
				{
					xtextFormatter.DrawString(string.Concat(new string[]
					{
						this.StrIfQuestions.LocalizedText,
						" ",
						this.ManufacturerList.Last<Infos>().Value,
						" ",
						this.StrIfQuestions2.LocalizedText,
						" ",
						this.ManufacturerList[this.ManufacturerList.Count<Infos>() - 2].Value
					}), new XFont("Calibri", 8.0), XBrushes.DarkGray, new XRect(57.0, 740.0, 520.0, 40.0));
				}
			}
			else
			{
				xgraphics.DrawString(this.StrReport.LocalizedText, font, XBrushes.DarkGray, 60.0, 180.0);
			}
			bool flag10 = this.Widescreen;
			XFont font2;
			if (flag10)
			{
				font2 = new XFont("Calibri", 10.0);
			}
			else
			{
				font2 = new XFont("Calibri", 9.0);
			}
			int num = 140;
			int num2 = 405;
			int num3 = 16;
			int num4 = 476;
			for (int i = 0; i < this.ProductList.Count<Infos>() - 1; i++)
			{
				xgraphics.DrawString(this.ProductList[i].Value, font2, XBrushes.Black, (double)num, (double)num4);
				num4 += num3;
			}
			string text2 = "";
			this.dbConnection.GetParameterValue("ConveyorSpeed", ref text2);
			xgraphics.DrawString(text2 + " m/s", font2, XBrushes.Black, (double)num, (double)num4);
			num4 += num3;
			bool flag11 = this.AccessManager.CurrentUser.ID < 0 && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
			if (flag11)
			{
				this.dbConnection.GetIVPInfoEntryReferenceProduct(DBIntegratedValidationProcessReferenceProduct.ProductPackRate, ref text2);
			}
			else
			{
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ProductPackRate, ref text2);
			}
			xgraphics.DrawString(text2, font2, XBrushes.Black, (double)num, (double)num4);
			num4 = 653;
			foreach (InfosTestsamples infosTestsamples in this.TestsamplesList)
			{
				bool flag12 = flag;
				string text3;
				if (flag12)
				{
					bool flag13 = infosTestsamples.Description == Material.VA;
					if (flag13)
					{
						text3 = "Stainless Steel," + infosTestsamples.Value + " mm";
					}
					else
					{
						text3 = "Glass," + infosTestsamples.Value + " mm";
					}
				}
				else
				{
					text3 = this.ConvertMaterial(infosTestsamples.Description) + "," + infosTestsamples.Value + " mm";
				}
				double num5 = xgraphics.MeasureString(text3, font2).Width / 105.0;
				bool flag14 = num5 > 1.0;
				if (flag14)
				{
					int length = (int)((double)text3.Length / num5);
					text3 = text3.Substring(0, length);
				}
				xgraphics.DrawString(text3, font2, XBrushes.Black, 58.0, (double)num4);
				xgraphics.DrawString(infosTestsamples.INNUmber, font2, XBrushes.Black, 165.0, (double)num4);
				xgraphics.DrawString(infosTestsamples.Density, font2, XBrushes.Black, 233.0, (double)num4);
				xgraphics.DrawString(this.ConvertTestcard(infosTestsamples.TestcardSize), font2, XBrushes.Black, 290.0, (double)num4);
				xgraphics.DrawString(Math.Round(100.0 * (double)infosTestsamples.NumberFoundTestballs / (double)this.numberProducts, 0).ToString() + " %", font2, XBrushes.Black, 359.0, (double)num4);
				bool flag15 = this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
				if (flag15)
				{
					xgraphics.DrawString("-", font2, XBrushes.Black, 430.0, (double)num4);
				}
				else
				{
					xgraphics.DrawString(infosTestsamples.NumberProductsRejected.ToString() + "/" + this.numberProducts.ToString(), font2, XBrushes.Black, 430.0, (double)num4);
				}
				bool flag16 = infosTestsamples.NumberFoundTestballs == this.numberProducts && infosTestsamples.NumberProductsRejected == this.numberProducts;
				if (flag16)
				{
					bool flag17 = flag;
					if (flag17)
					{
						xgraphics.DrawString("Passed", font2, XBrushes.Black, 494.0, (double)num4);
					}
					else
					{
						xgraphics.DrawString(this.StrPassed.LocalizedText, font2, XBrushes.Black, 494.0, (double)num4);
					}
				}
				else
				{
					bool flag18 = flag;
					if (flag18)
					{
						xgraphics.DrawString("Failed", font2, XBrushes.Black, 494.0, (double)num4);
					}
					else
					{
						xgraphics.DrawString(this.StrFailed.LocalizedText, font2, XBrushes.Black, 494.0, (double)num4);
					}
				}
				num4 += num3;
			}
			num4 = 245;
			xgraphics.DrawString(DateTime.Now.ToString("dd.MM.yyyy HH:mm"), font2, XBrushes.Black, (double)num2, (double)num4);
			num4 += num3;
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, ref text2);
			xgraphics.DrawString(text2, font2, XBrushes.Black, (double)num2, (double)num4);
			num4 += num3;
			bool flag19 = this.CustomerList != null;
			if (flag19)
			{
				for (int j = 0; j <= 5; j++)
				{
					bool flag20 = j == 2;
					if (flag20)
					{
						xgraphics.DrawString(this.CustomerList[j].Value + ", " + this.CustomerList[j + 1].Value, font2, XBrushes.Black, (double)num, (double)num4);
						j += 2;
					}
					else
					{
						xgraphics.DrawString(this.CustomerList[j].Value, font2, XBrushes.Black, (double)num, (double)num4);
					}
					num4 += num3;
					bool flag21 = num4 == 293;
					if (flag21)
					{
						num4--;
					}
				}
			}
			else
			{
				for (int k = 0; k <= 3; k++)
				{
					xgraphics.DrawString("-", font2, XBrushes.Black, (double)num, (double)num4);
					num4 += num3;
					bool flag22 = num4 == 293;
					if (flag22)
					{
						num4--;
					}
				}
			}
			num4 = 245 + num3 + num3;
			bool flag23 = this.AccessManager.CurrentUser.ID < 0;
			if (flag23)
			{
				xgraphics.DrawString(this.ManufacturerList[6].Value, font2, XBrushes.Black, (double)num2, (double)num4);
			}
			else
			{
				xgraphics.DrawString("-", font2, XBrushes.Black, (double)num2, (double)num4);
			}
			num4 = num4 + num3 - 1;
			bool flag24 = this.CustomerList != null;
			if (flag24)
			{
				for (int l = 6; l < this.CustomerList.Count<Infos>(); l++)
				{
					xgraphics.DrawString(this.CustomerList[l].Value, font2, XBrushes.Black, (double)num2, (double)num4);
					num4 += num3;
				}
			}
			else
			{
				for (int m = 0; m < 3; m++)
				{
					xgraphics.DrawString("-", font2, XBrushes.Black, (double)num2, (double)num4);
					num4 += num3;
				}
			}
			string s = "";
			string s2 = "";
			ushort num6 = 0;
			ushort num7 = 0;
			string text4 = "";
			string text5 = "";
			string text6 = "";
			string text7 = "";
			MachineType machineType = MachineType.MachineTypeUndefined;
			this.dbConnection.GetDeviceName(ref s);
			this.dbConnection.GetLineName(ref s2);
			this.dbConnection.GetInspectionWidth(ref num6);
			this.dbConnection.GetInspectionHeight(ref num7);
			this.dbConnection.GetSerialNumber(ref text4);
			this.dbConnection.GetMachineType(ref machineType);
			this.dbConnection.GetParameterValue("/hardware/xraysource/VoltageSetpoint/Value", ref text5);
			this.dbConnection.GetParameterValue("/hardware/xraysource/CurrentSetpoint/Value", ref text6);
			this.dbConnection.GetParameterValue("/hardware/rejecter/rejecter1/Mode/Value", ref text7);
			bool flag25 = machineType == MachineType.RayconXRS;
			string s3;
			if (flag25)
			{
				s3 = "Raycon XRS";
			}
			else
			{
				bool flag26 = machineType == MachineType.RayconD;
				if (flag26)
				{
					s3 = "Raycon D";
				}
				else
				{
					bool flag27 = machineType == MachineType.RayconEntry;
					if (flag27)
					{
						s3 = "Raycon EX1";
					}
					else
					{
						bool flag28 = machineType == MachineType.RayconDPlus;
						if (flag28)
						{
							s3 = "Raycon D+";
						}
						else
						{
							bool flag29 = machineType == MachineType.RayconEX2A;
							if (flag29)
							{
								s3 = "Raycon EX2A";
							}
							else
							{
								s3 = "UnknownType";
							}
						}
					}
				}
			}
			bool flag30 = flag;
			if (flag30)
			{
				bool flag31 = text7.Equals("0");
				if (flag31)
				{
					text7 = "Pusher / nozzle";
				}
				else
				{
					text7 = "Flap";
				}
			}
			else
			{
				bool flag32 = text7.Equals("0");
				if (flag32)
				{
					text7 = this.StrPusher.LocalizedText;
				}
				else
				{
					text7 = this.StrLeadingEdge.LocalizedText;
				}
			}
			num4 = 377;
			xgraphics.DrawString(s3, font2, XBrushes.Black, (double)num, (double)num4);
			xgraphics.DrawString(num6.ToString() + "/" + num7.ToString(), font2, XBrushes.Black, (double)num2, (double)num4);
			num4 += num3;
			xgraphics.DrawString(text4, font2, XBrushes.Black, (double)num, (double)num4);
			xgraphics.DrawString(text7, font2, XBrushes.Black, (double)num2, (double)num4);
			num4 += num3;
			xgraphics.DrawString(s, font2, XBrushes.Black, (double)num, (double)num4);
			xgraphics.DrawString(text5.ToString() + " kV", font2, XBrushes.Black, (double)num2, (double)num4);
			num4 += num3;
			xgraphics.DrawString(s2, font2, XBrushes.Black, (double)num, (double)num4);
			xgraphics.DrawString(text6.ToString() + " mA", font2, XBrushes.Black, (double)num2, (double)num4);
			string text8 = "";
			bool flag33 = (this.AccessManager.CurrentUser.ID == -1 || this.AccessManager.CurrentUser.ID == -2 || this.AccessManager.CurrentUser.ID == -3) && !this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1") && passed;
			if (flag33)
			{
				text8 = "Cert_" + DateTime.Now.ToString("yyyyMMdd_HHmmss").Replace(".", "").Replace(":", "").Replace(" ", "_").Replace("/", "") + "_" + text4;
			}
			else
			{
				bool flag34 = (this.AccessManager.CurrentUser.ID == -1 || this.AccessManager.CurrentUser.ID == -2) && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1") && passed;
				if (flag34)
				{
					text8 = "RP_Cert_" + DateTime.Now.ToString("yyyyMMdd_HHmmss").Replace(".", "").Replace(":", "").Replace(" ", "_").Replace("/", "") + "_" + text4;
				}
				else
				{
					bool flag35 = this.AccessManager.CurrentUser.ID == -3 && passed && this.productManager.CurrentLoadedProduct.ProductNumber.Equals("-1");
					if (flag35)
					{
						text8 = "EA_Cert_" + DateTime.Now.ToString("yyyyMMdd_HHmmss").Replace(".", "").Replace(":", "").Replace(" ", "_").Replace("/", "") + "_" + text4;
					}
					else
					{
						text8 = "Rep_" + DateTime.Now.ToString("yyyyMMdd_HHmmss").Replace(".", "").Replace(":", "").Replace(" ", "_").Replace("/", "") + "_" + text4;
					}
				}
			}
			xgraphics.DrawString(text8, font2, XBrushes.Black, (double)num, 245.0);
			xgraphics.DrawString(this.OrderNumber, font2, XBrushes.Black, (double)num, (double)(245 + num3));
			string text9 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/IVPCertificates/" + text8;
			try
			{
				bool flag36 = !Directory.Exists(text9);
				if (flag36)
				{
					Directory.CreateDirectory(text9);
				}
			}
			catch (Exception)
			{
			}
			pdfDocument.Save(text9 + "/" + text8 + ".pdf");
			pdfDocument.Close();
			pdfDocument.Dispose();
			if (passed)
			{
				LogbookManager.AddEntry(SSTEntryCode.IVPOK, 0, text8);
			}
			else
			{
				LogbookManager.AddEntry(SSTEntryCode.IVPFailed, 0, text8);
			}
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/IVPCertificates/";
			string[] directories = Directory.GetDirectories(path);
			List<string> list = new List<string>(directories);
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			foreach (string text10 in list)
			{
				bool flag37 = text10.Contains("EA");
				if (!flag37)
				{
					bool flag38 = text10.Contains("Cert_");
					if (flag38)
					{
						list2.Add(text10);
					}
					else
					{
						list3.Add(text10);
					}
				}
			}
			bool flag39 = list2.Count<string>() >= 9 || list3.Count<string>() >= 9;
			if (flag39)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate()
				{
					Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtSaveDocuments"), DialogStyle.Warning, DialogButtons.OK);
				}));
				bool flag40 = list2.Count<string>() > 10 || list3.Count<string>() > 10;
				if (flag40)
				{
					this.deleteDirectory(path);
				}
			}
			return new string[]
			{
				text8,
				text9 + "/" + text8 + ".pdf"
			};
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00045AF8 File Offset: 0x00043CF8
		private void deleteDirectory(string path)
		{
			string[] directories = Directory.GetDirectories(path);
			List<string> list = new List<string>(directories);
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			foreach (string text in list)
			{
				bool flag = text.Contains("EA");
				if (!flag)
				{
					bool flag2 = text.Contains("Cert_");
					if (flag2)
					{
						list2.Add(text);
					}
					else
					{
						list3.Add(text);
					}
				}
			}
			list3.Sort(new DirectoriesComparer());
			while (list3.Count > 10)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(list3.Last<string>());
				FileInfo[] files = directoryInfo.GetFiles();
				try
				{
					foreach (FileInfo fileInfo in files.ToList<FileInfo>())
					{
						fileInfo.Delete();
					}
					directoryInfo.Delete();
					list3.RemoveAt(list3.Count - 1);
				}
				catch
				{
					break;
				}
			}
			list2.Sort(new DirectoriesComparer());
			while (list2.Count > 10)
			{
				DirectoryInfo directoryInfo2 = new DirectoryInfo(list2.Last<string>());
				FileInfo[] files2 = directoryInfo2.GetFiles();
				try
				{
					foreach (FileInfo fileInfo2 in files2.ToList<FileInfo>())
					{
						fileInfo2.Delete();
					}
					directoryInfo2.Delete();
					list2.RemoveAt(list2.Count - 1);
				}
				catch
				{
					break;
				}
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00045CF8 File Offset: 0x00043EF8
		public BitmapImage ConvertToBitmap(System.Drawing.Image image)
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Seek(0L, SeekOrigin.Begin);
			image.Save(memoryStream, ImageFormat.Bmp);
			BitmapImage bitmapImage = new BitmapImage();
			bitmapImage.BeginInit();
			bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
			bitmapImage.StreamSource = memoryStream;
			bitmapImage.EndInit();
			bitmapImage.Freeze();
			return bitmapImage;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00045D54 File Offset: 0x00043F54
		private Material convertStringToTestcardMaterial(string input)
		{
			Material result = Material.None;
			bool flag = input == Convert.ToString(Material.VA);
			if (flag)
			{
				result = Material.VA;
			}
			else
			{
				bool flag2 = input == Convert.ToString(Material.Glass);
				if (flag2)
				{
					result = Material.Glass;
				}
				else
				{
					bool flag3 = input == Convert.ToString(Material.VAWire);
					if (flag3)
					{
						result = Material.VAWire;
					}
					else
					{
						bool flag4 = input == Convert.ToString(Material.Ceramics);
						if (flag4)
						{
							result = Material.Ceramics;
						}
						else
						{
							bool flag5 = input == Convert.ToString(Material.EPDM);
							if (flag5)
							{
								result = Material.EPDM;
							}
							else
							{
								bool flag6 = input == Convert.ToString(Material.PTFE);
								if (flag6)
								{
									result = Material.PTFE;
								}
								else
								{
									bool flag7 = input == Convert.ToString(Material.Aluminium);
									if (flag7)
									{
										result = Material.Aluminium;
									}
									else
									{
										bool flag8 = input == Convert.ToString(Material.Bone);
										if (flag8)
										{
											result = Material.Bone;
										}
										else
										{
											bool flag9 = input == Convert.ToString(Material.Other);
											if (flag9)
											{
												result = Material.Other;
											}
											else
											{
												bool flag10 = input == Convert.ToString(Material.NonFE);
												if (flag10)
												{
													result = Material.NonFE;
												}
												else
												{
													bool flag11 = input == Convert.ToString(Material.FE);
													if (flag11)
													{
														result = Material.FE;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00045EA4 File Offset: 0x000440A4
		public string updateNextValidationDate(bool isEndAssembly)
		{
			string text = "";
			this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.IntervallOnOff, ref text);
			bool flag = text.Equals("1") && !isEndAssembly;
			string result;
			if (flag)
			{
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.IntervallMonths, ref text);
				int months = Convert.ToInt32(text);
				string value = DateTime.Today.AddMonths(months).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
				this.dbConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, value);
				result = DateTime.Today.ToString();
			}
			else
			{
				this.dbConnection.GetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, ref text);
				int day = Convert.ToInt32(text.Substring(0, 2));
				int month = Convert.ToInt32(text.Substring(3, 2));
				int num = Convert.ToInt32(text.Substring(6, 4));
				DateTime d = new DateTime(num, month, day);
				int num2 = (int)(d - DateTime.Today).TotalDays;
				bool flag2 = num2 <= 0 || num == 2999 || isEndAssembly;
				if (flag2)
				{
					string text2 = DateTime.Today.AddMonths(12).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
					this.dbConnection.SetIVPInfoEntry(DBIntegratedValidationProcess.ValidationDate, text2);
					result = text2;
				}
				else
				{
					result = text;
				}
			}
			return result;
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00046014 File Offset: 0x00044214
		public string getDensity(Material material)
		{
			switch (material)
			{
			case Material.VA:
				return "8,0g/cm^3";
			case Material.Glass:
				return "2,5g/cm^3";
			case Material.VAWire:
				return "8,0g/cm^3";
			case Material.Ceramics:
				return "-";
			case Material.EPDM:
				return "1,2g/cm^3";
			case Material.PTFE:
				return "2,1g/cm^3";
			case Material.Aluminium:
				return "2,7g/cm^3";
			case Material.Bone:
				return "1.6g/cm^3";
			case Material.None:
				return "-";
			case Material.FE:
				return "7,8g/cm^3";
			case Material.NonFE:
				return "8,4g/cm^3";
			}
			return "-";
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x000460CC File Offset: 0x000442CC
		private void doFullBackup(string backupFolderName, string certificateName, bool withBackup)
		{
			this.backup.SaveProducts = true;
			this.backup.SaveSystem = false;
			string text = "";
			this.dbConnection.GetUsbDriveLetter(ref text);
			text += "\\";
			bool flag = Directory.Exists(text) && text != "\\";
			if (flag)
			{
				string text2 = text + backupFolderName;
				bool flag2 = !Directory.Exists(text2);
				if (flag2)
				{
					Directory.CreateDirectory(text2);
				}
				try
				{
					File.Copy(string.Concat(new string[]
					{
						Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
						"/IVPCertificates/",
						certificateName,
						"/",
						certificateName,
						".pdf"
					}), text2 + "/" + certificateName + ".pdf");
				}
				catch
				{
				}
				if (withBackup)
				{
					bool flag3 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
					if (flag3)
					{
						string text3 = "";
						this.dbConnection.GetCurrentProductTableNameAI(ref text3);
						string[] selectedModels = new string[]
						{
							text3
						};
						this.backup.DoBackupIVP(certificateName, text2, selectedModels);
					}
					else
					{
						this.backup.DoBackupIVP(certificateName, text2, null);
					}
				}
			}
			else if (withBackup)
			{
				bool flag4 = this.parHandling.ReadPar(Definition.anomalyDetectionEnabled) == "1" || this.parHandling.ReadPar(Definition.imgprocarchitecture) == "1" || this.parHandling.ReadPar(Definition.objectDetectionEnabled) == "1";
				if (flag4)
				{
					string text4 = "";
					this.dbConnection.GetCurrentProductTableNameAI(ref text4);
					string[] selectedModels2 = new string[]
					{
						text4
					};
					this.backup.DoBackupIVP(certificateName, selectedModels2);
				}
				else
				{
					this.backup.DoBackupIVP(certificateName, null);
				}
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00046318 File Offset: 0x00044518
		private TestCardSize convertStringToTestcardType(string input)
		{
			TestCardSize result = TestCardSize.Other;
			bool flag = input == Convert.ToString(TestCardSize.SingleBall);
			if (flag)
			{
				result = TestCardSize.SingleBall;
			}
			else
			{
				bool flag2 = input == Convert.ToString(TestCardSize.Testcard5);
				if (flag2)
				{
					result = TestCardSize.Testcard5;
				}
				else
				{
					bool flag3 = input == Convert.ToString(TestCardSize.Testcard100);
					if (flag3)
					{
						result = TestCardSize.Testcard100;
					}
					else
					{
						bool flag4 = input == Convert.ToString(TestCardSize.Testcard5Same);
						if (flag4)
						{
							result = TestCardSize.Testcard5Same;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00046394 File Offset: 0x00044594
		public void CleanUp(object obj)
		{
			bool flag = this.parHandling.ReadPar(Definition.detectorMonitoringActive) == "1";
			if (flag)
			{
				this.parHandling.WriteValue(Definition.detectorMonitoringBackgroundEnable, "1");
			}
			this.parHandling.WriteValue(Definition.inIVPModePath, "0", false);
			bool flag2 = this.timerCheck != null;
			if (flag2)
			{
				this.timerCheck.Stop();
			}
			this.DeregisterLocalizedText();
			this.reset();
			bool flag3 = this.TestsamplesList != null;
			if (flag3)
			{
				this.TestsamplesList.Clear();
				this.TestsamplesList = null;
			}
			bool flag4 = this.imageQueue != null;
			if (flag4)
			{
				this.imageQueue.Clear();
				this.imageQueue = null;
			}
			bool flag5 = this.UnlockBinItem != null;
			if (flag5)
			{
				this.UnlockBinItem.Clear();
				this.UnlockBinItem = null;
			}
			this.ProductList.Clear();
			this.ManufacturerList.Clear();
			this.CustomerList = null;
			this.ManufacturerList = null;
			bool flag6 = this.CustomerList != null;
			if (flag6)
			{
				this.CustomerList.Clear();
				this.ProductList = null;
			}
			bool flag7 = this.machineState.State == MachineStates.AuditCheck;
			if (flag7)
			{
				this.machineState.State = MachineStates.Inspecting;
			}
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x000464EF File Offset: 0x000446EF
		internal void End()
		{
			this.OnClose();
		}

		// Token: 0x04000789 RID: 1929
		public Keyboard keyboard;

		// Token: 0x0400078A RID: 1930
		private XRSImage currentImage;

		// Token: 0x0400078B RID: 1931
		private DBConnection dbConnection;

		// Token: 0x0400078C RID: 1932
		private bool countCont;

		// Token: 0x0400078F RID: 1935
		public AuditMode Mode = AuditMode.Contamination;

		// Token: 0x04000790 RID: 1936
		private ImageDataTransfer imageTransfer;

		// Token: 0x04000791 RID: 1937
		private int remainingProducts;

		// Token: 0x04000792 RID: 1938
		private int numberContaminations;

		// Token: 0x04000793 RID: 1939
		private string stringNumberContaminations;

		// Token: 0x04000794 RID: 1940
		private int numberProducts;

		// Token: 0x04000795 RID: 1941
		private int numberTestsamples;

		// Token: 0x04000797 RID: 1943
		private bool userLogAvailable;

		// Token: 0x04000798 RID: 1944
		public int remainingTestBalls;

		// Token: 0x04000799 RID: 1945
		private bool contextUnlockBinVisibility = false;

		// Token: 0x0400079B RID: 1947
		private Rejecter rejecter;

		// Token: 0x0400079D RID: 1949
		private int countFoundSamples = 0;

		// Token: 0x0400079E RID: 1950
		private int countRejectedProducts = 0;

		// Token: 0x0400079F RID: 1951
		private bool widescreen;

		// Token: 0x040007A0 RID: 1952
		private List<XRSImage> imageQueue = new List<XRSImage>();

		// Token: 0x040007A1 RID: 1953
		private bool enabled1;

		// Token: 0x040007A2 RID: 1954
		private bool enabled2;

		// Token: 0x040007A3 RID: 1955
		private bool enabled3;

		// Token: 0x040007A4 RID: 1956
		private bool enabled4;

		// Token: 0x040007A5 RID: 1957
		public static readonly string backupFolder = "IVPBackupsRaycon\\";

		// Token: 0x040007A9 RID: 1961
		private bool check;

		// Token: 0x040007AA RID: 1962
		private bool enabledNext;

		// Token: 0x040007AB RID: 1963
		private string orderNumber;

		// Token: 0x040007AC RID: 1964
		private string testsampleToConvey;

		// Token: 0x040007AD RID: 1965
		private string productsConveyed;

		// Token: 0x040007AE RID: 1966
		private string testsamplesConveyed;

		// Token: 0x040007AF RID: 1967
		public ObservableCollection<Infos> productList;

		// Token: 0x040007B0 RID: 1968
		public ObservableCollection<InfosTestsamples> testsamplesList;

		// Token: 0x040007B1 RID: 1969
		public ObservableCollection<Infos> customerList;

		// Token: 0x040007B2 RID: 1970
		public ObservableCollection<Infos> manufacturerList;

		// Token: 0x040007B6 RID: 1974
		private DispatcherTimer timerCheck;

		// Token: 0x040007B8 RID: 1976
		private string nameSubproduct = "";

		// Token: 0x040007B9 RID: 1977
		private bool isLane = false;

		// Token: 0x040007BA RID: 1978
		private bool isMultiProduct = false;

		// Token: 0x040007BB RID: 1979
		private int numberOfImages = 0;

		// Token: 0x040007BC RID: 1980
		private MessageSystem messageSystem;

		// Token: 0x040007BD RID: 1981
		private bool doorLockAvailable;

		// Token: 0x040007BE RID: 1982
		private bool rejectAll = false;

		// Token: 0x040007BF RID: 1983
		private ProductManager productManager;

		// Token: 0x040007C0 RID: 1984
		private XRSImageBox imageBox;

		// Token: 0x040007C1 RID: 1985
		private bool customerListVisible = true;
	}
}

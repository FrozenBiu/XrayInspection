using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200024D RID: 589
	public partial class Progress : Window
	{
		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06002CB2 RID: 11442 RVA: 0x00126005 File Offset: 0x00124205
		// (set) Token: 0x06002CB3 RID: 11443 RVA: 0x0012600D File Offset: 0x0012420D
		public WindowState WState { get; set; }

		// Token: 0x17000C07 RID: 3079
		// (get) Token: 0x06002CB4 RID: 11444 RVA: 0x00126018 File Offset: 0x00124218
		// (set) Token: 0x06002CB5 RID: 11445 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x17000C08 RID: 3080
		// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x00126030 File Offset: 0x00124230
		// (set) Token: 0x06002CB7 RID: 11447 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x17000C09 RID: 3081
		// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x00126048 File Offset: 0x00124248
		// (set) Token: 0x06002CB9 RID: 11449 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x17000C0A RID: 3082
		// (get) Token: 0x06002CBA RID: 11450 RVA: 0x00126060 File Offset: 0x00124260
		// (set) Token: 0x06002CBB RID: 11451 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x17000C0B RID: 3083
		// (get) Token: 0x06002CBC RID: 11452 RVA: 0x00126077 File Offset: 0x00124277
		// (set) Token: 0x06002CBD RID: 11453 RVA: 0x0012607E File Offset: 0x0012427E
		public static double HeigthToGetButtonsDuringProgress { get; set; }

		// Token: 0x06002CBE RID: 11454 RVA: 0x00126088 File Offset: 0x00124288
		public Progress()
		{
			this.InitializeComponent();
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			base.DataContext = this;
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x001260D8 File Offset: 0x001242D8
		public Progress(string description, bool buttonsAreVisible) : this()
		{
			Progress.HeigthToGetButtonsDuringProgress = Definition.windowHeight;
			bool flag = buttonsAreVisible && Subwindow.ActualContentHeight > Progress.heightForButtonsInSubwindow;
			if (flag)
			{
				Progress.HeigthToGetButtonsDuringProgress = Definition.windowHeight - Progress.heightForButtonsInSubwindow;
			}
			this.txtHeader.Text = description;
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x00075F4E File Offset: 0x0007414E
		private void FormFadeOut_Completed(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x0012612C File Offset: 0x0012432C
		public void ChangeText(string newText)
		{
			this.txtHeader.Text = newText;
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x0012613C File Offset: 0x0012433C
		private void progress_Loaded(object sender, RoutedEventArgs e)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			compositionTarget.RenderMode = RenderMode.SoftwareOnly;
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x0003CA4C File Offset: 0x0003AC4C
		private void progress_Closing(object sender, CancelEventArgs e)
		{
			WorkingArea.ClearResources(this);
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x00126168 File Offset: 0x00124368
		public static void ShowProgressInvoked(Progress prog)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					prog.Show();
				}));
			}
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x001261B0 File Offset: 0x001243B0
		public static void UpdateProgressInvoked(Progress prog, int percentage)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					prog.progBar.Value = (double)percentage;
				}));
			}
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x00126200 File Offset: 0x00124400
		public static void CloseProgressInvoked(Progress prog)
		{
			bool flag = Application.Current != null;
			if (flag)
			{
				Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
				{
					prog.Close();
				}));
			}
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x00126248 File Offset: 0x00124448
		public static void ShowStaticProgress(string text, bool withTestcardSim)
		{
			int maxTime = 30000;
			if (withTestcardSim)
			{
				maxTime = 90000;
			}
			Progress.ShowStaticProgress(text, 500, maxTime, 20);
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x00126278 File Offset: 0x00124478
		public static void ShowStaticProgress(string text, int sleepTime, int maxTime, int percentagePerLoop)
		{
			bool flag = sleepTime > 0;
			if (flag)
			{
				Progress.staticProgressRunning = true;
				Progress.staticProgress = new Progress(text, false);
				Progress.staticProgress.Show();
				Thread thread = new Thread(delegate()
				{
					int loops = 0;
					while (Progress.staticProgressRunning)
					{
						percentagePerLoop += 20;
						bool flag2 = percentagePerLoop > 100;
						if (flag2)
						{
							percentagePerLoop = 0;
						}
						Progress.UpdateProgressInvoked(Progress.staticProgress, percentagePerLoop);
						Thread.Sleep(sleepTime);
						int loops2 = loops;
						loops = loops2 + 1;
						bool flag3 = loops > maxTime / sleepTime;
						if (flag3)
						{
							break;
						}
					}
					Progress.CloseProgressInvoked(Progress.staticProgress);
					bool flag4 = Application.Current != null;
					if (flag4)
					{
						Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate()
						{
							bool flag5 = loops <= maxTime / sleepTime;
							if (flag5)
							{
								bool flag6 = Progress.showSuccessMessage;
								if (flag6)
								{
									Dialog.Show(LocalizationManager.Localize("txtSuccess"), LocalizationManager.Localize("txtSuccess"), DialogStyle.Ok, DialogButtons.None, true);
								}
							}
							else
							{
								Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtError"), DialogStyle.Error, DialogButtons.None, true);
							}
						}));
					}
					Progress.staticProgressRunning = false;
				});
				thread.Start();
			}
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x001262E6 File Offset: 0x001244E6
		public static void CloseStaticProgress(bool showSuccessMessage)
		{
			Progress.showSuccessMessage = showSuccessMessage;
			Progress.staticProgressRunning = false;
		}

		// Token: 0x04001BF9 RID: 7161
		private static Progress staticProgress;

		// Token: 0x04001BFA RID: 7162
		private static bool staticProgressRunning;

		// Token: 0x04001BFB RID: 7163
		private static bool showSuccessMessage;

		// Token: 0x04001BFE RID: 7166
		private static double heightForButtonsInSubwindow = 114.0;
	}
}

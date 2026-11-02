using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000152 RID: 338
	public class WdTrain : Window, IComponentConnector
	{
		// Token: 0x0600204F RID: 8271 RVA: 0x000D3FAF File Offset: 0x000D21AF
		public WdTrain(ParameterHandling parHandling, string rejecterMainPath, TrainMode trainMode) : this(parHandling, rejecterMainPath, trainMode, 0)
		{
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000D3FBD File Offset: 0x000D21BD
		public WdTrain(ParameterHandling parHandling, string rejecterMainPath, TrainMode trainMode, int binTimeout) : this(parHandling, rejecterMainPath, trainMode, binTimeout, null, MachineType.RayconD)
		{
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000D3FD0 File Offset: 0x000D21D0
		public WdTrain(ParameterHandling parHandling, string rejecterMainPath, TrainMode trainMode, int binTimeout, MessageSystem messageSystem, MachineType machineType)
		{
			this.InitializeComponent();
			this.parHandling = parHandling;
			this.trainMode = trainMode;
			this.binTimeout = binTimeout;
			this.rejcecterMainPath = rejecterMainPath;
			this.viewModel = new TrainViewModel(parHandling, rejecterMainPath, trainMode, binTimeout, messageSystem, base.FindResource("okButtonStyle") as Style, base.FindResource("cancelButtonStyle") as Style, machineType);
			this.viewModel.TrainFinshed += this.viewModel_TrainFinshed;
			base.DataContext = this.viewModel;
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000D4062 File Offset: 0x000D2262
		private void viewModel_TrainFinshed()
		{
			this.FormFadeOut.Begin();
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000D4071 File Offset: 0x000D2271
		private void FormFadeOut_Completed(object sender, EventArgs e)
		{
			this.viewModel.TrainFinshed -= this.viewModel_TrainFinshed;
			base.Close();
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000D4094 File Offset: 0x000D2294
		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			bool lowAirPressureTriggered = this.viewModel.LowAirPressureTriggered;
			if (lowAirPressureTriggered)
			{
				this.viewModel.CommitLowAirPressure();
			}
			bool rejectVerifyTriggered = this.viewModel.RejectVerifyTriggered;
			if (rejectVerifyTriggered)
			{
				this.viewModel.CommitRejectVerify();
			}
			bool flag = this.trainMode == TrainMode.UnlockBin;
			if (flag)
			{
				this.parHandling.WriteValue(this.rejcecterMainPath + "/HLBinTimeout/Value", "1");
				Thread.Sleep(500);
				this.parHandling.WriteValue(this.rejcecterMainPath + "/HLBinTimeout/Value", this.binTimeout.ToString());
			}
			this.FormFadeOut.Begin();
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000D4148 File Offset: 0x000D2348
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/rejecter/wdtrain.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x000D4180 File Offset: 0x000D2380
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.overlay = (Grid)target;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.FormFadeIn = (Storyboard)target;
				break;
			case 4:
				this.MaximizeWidthAnimation = (DoubleAnimation)target;
				break;
			case 5:
				this.MaximizeHeightAnimation = (DoubleAnimation)target;
				break;
			case 6:
				this.FadeInAnimation = (DoubleAnimation)target;
				break;
			case 7:
				this.FadeInContentAnimation = (DoubleAnimation)target;
				break;
			case 8:
				this.FadeInBackgroundAnimation = (DoubleAnimation)target;
				break;
			case 9:
				this.FormFadeOut = (Storyboard)target;
				this.FormFadeOut.Completed += this.FormFadeOut_Completed;
				break;
			case 10:
				this.MinimizeWidthAnimation = (DoubleAnimation)target;
				break;
			case 11:
				this.MinimizeAnimation = (DoubleAnimation)target;
				break;
			case 12:
				this.FadeOutAnimation = (DoubleAnimation)target;
				break;
			case 13:
				this.FadeOutContentAnimation = (DoubleAnimation)target;
				break;
			case 14:
				this.FadeOutBackgroundAnimation = (DoubleAnimation)target;
				break;
			case 15:
				this.mainContent = (DockPanel)target;
				break;
			case 16:
				this.Header = (Border)target;
				break;
			case 17:
				this.Footer = (Border)target;
				break;
			case 18:
				this.btnCancel = (Button)target;
				this.btnCancel.Click += this.btnCancel_Click;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x040014F4 RID: 5364
		private TrainViewModel viewModel;

		// Token: 0x040014F5 RID: 5365
		private ParameterHandling parHandling;

		// Token: 0x040014F6 RID: 5366
		private TrainMode trainMode;

		// Token: 0x040014F7 RID: 5367
		private int binTimeout;

		// Token: 0x040014F8 RID: 5368
		private string rejcecterMainPath;

		// Token: 0x040014F9 RID: 5369
		internal Grid overlay;

		// Token: 0x040014FA RID: 5370
		internal Grid mainGrid;

		// Token: 0x040014FB RID: 5371
		internal Storyboard FormFadeIn;

		// Token: 0x040014FC RID: 5372
		internal DoubleAnimation MaximizeWidthAnimation;

		// Token: 0x040014FD RID: 5373
		internal DoubleAnimation MaximizeHeightAnimation;

		// Token: 0x040014FE RID: 5374
		internal DoubleAnimation FadeInAnimation;

		// Token: 0x040014FF RID: 5375
		internal DoubleAnimation FadeInContentAnimation;

		// Token: 0x04001500 RID: 5376
		internal DoubleAnimation FadeInBackgroundAnimation;

		// Token: 0x04001501 RID: 5377
		internal Storyboard FormFadeOut;

		// Token: 0x04001502 RID: 5378
		internal DoubleAnimation MinimizeWidthAnimation;

		// Token: 0x04001503 RID: 5379
		internal DoubleAnimation MinimizeAnimation;

		// Token: 0x04001504 RID: 5380
		internal DoubleAnimation FadeOutAnimation;

		// Token: 0x04001505 RID: 5381
		internal DoubleAnimation FadeOutContentAnimation;

		// Token: 0x04001506 RID: 5382
		internal DoubleAnimation FadeOutBackgroundAnimation;

		// Token: 0x04001507 RID: 5383
		internal DockPanel mainContent;

		// Token: 0x04001508 RID: 5384
		internal Border Header;

		// Token: 0x04001509 RID: 5385
		internal Border Footer;

		// Token: 0x0400150A RID: 5386
		internal Button btnCancel;

		// Token: 0x0400150B RID: 5387
		private bool _contentLoaded;
	}
}

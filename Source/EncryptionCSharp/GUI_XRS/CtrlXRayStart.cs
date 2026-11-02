using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GUI_XRS
{
	// Token: 0x020000C4 RID: 196
	public class CtrlXRayStart : UserControl, IComponentConnector
	{
		// Token: 0x060013D4 RID: 5076 RVA: 0x00075B44 File Offset: 0x00073D44
		public CtrlXRayStart()
		{
			this.InitializeComponent();
			base.Loaded += this.CtrlXRayStart_Loaded;
			base.Unloaded += this.CtrlXRayStart_Unloaded;
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00075B94 File Offset: 0x00073D94
		private void CtrlXRayStart_Loaded(object sender, RoutedEventArgs e)
		{
			this.timerAnimateBackground = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Background, new EventHandler(this.AnimateBackground), base.Dispatcher);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00075BC0 File Offset: 0x00073DC0
		private void CtrlXRayStart_Unloaded(object sender, RoutedEventArgs e)
		{
			this.timerAnimateBackground.Stop();
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00075BD0 File Offset: 0x00073DD0
		private void AnimateBackground(object sender, EventArgs e)
		{
			bool flag = base.Visibility == Visibility.Visible;
			if (flag)
			{
				string name = "btnXRayBorderUp";
				Ellipse ellipse = (Ellipse)this.btnStartXRay.Template.FindName(name, this.btnStartXRay);
				bool flag2 = ellipse != null;
				if (flag2)
				{
					LinearGradientBrush linearGradientBrush = (LinearGradientBrush)ellipse.Fill;
					LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush();
					byte b = byte.MaxValue;
					byte b2 = 0;
					for (int i = 0; i < linearGradientBrush.GradientStops.Count; i++)
					{
						bool flag3 = linearGradientBrush.GradientStops[i].Color.R > b2;
						if (flag3)
						{
							b2 = linearGradientBrush.GradientStops[i].Color.R;
						}
						bool flag4 = linearGradientBrush.GradientStops[i].Color.R < b;
						if (flag4)
						{
							b = linearGradientBrush.GradientStops[i].Color.R;
						}
					}
					this.downPhase = (this.downPhase ? ((int)b2 - this.step >= 200) : ((int)b2 + this.step > 255));
					bool flag5 = linearGradientBrush.GradientStops.Count > 0;
					if (flag5)
					{
						bool flag6 = this.downPhase;
						if (flag6)
						{
							for (int j = 0; j < linearGradientBrush.GradientStops.Count; j++)
							{
								GradientStop gradientStop = linearGradientBrush.GradientStops[j];
								gradientStop = new GradientStop(Color.FromRgb((byte)((int)gradientStop.Color.R - this.step), gradientStop.Color.G, gradientStop.Color.B), gradientStop.Offset);
								linearGradientBrush2.GradientStops.Add(gradientStop);
							}
						}
						else
						{
							for (int k = 0; k < linearGradientBrush.GradientStops.Count; k++)
							{
								GradientStop gradientStop2 = linearGradientBrush.GradientStops[k];
								gradientStop2 = new GradientStop(Color.FromRgb((byte)((int)gradientStop2.Color.R + this.step), gradientStop2.Color.G, gradientStop2.Color.B), gradientStop2.Offset);
								linearGradientBrush2.GradientStops.Add(gradientStop2);
							}
						}
					}
					ellipse.Fill = linearGradientBrush2;
				}
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00075E80 File Offset: 0x00074080
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/mainwindow/ctrlxraystart.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00075EB8 File Offset: 0x000740B8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				this._contentLoaded = true;
			}
			else
			{
				this.btnStartXRay = (Button)target;
			}
		}

		// Token: 0x04000CFA RID: 3322
		private DispatcherTimer timerAnimateBackground;

		// Token: 0x04000CFB RID: 3323
		private bool downPhase = true;

		// Token: 0x04000CFC RID: 3324
		private int step = 5;

		// Token: 0x04000CFD RID: 3325
		internal Button btnStartXRay;

		// Token: 0x04000CFE RID: 3326
		private bool _contentLoaded;
	}
}

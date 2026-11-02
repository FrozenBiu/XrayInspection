using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000162 RID: 354
	public class ButtonUSB : Button
	{
		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x0600211F RID: 8479 RVA: 0x000D7AA2 File Offset: 0x000D5CA2
		// (set) Token: 0x06002120 RID: 8480 RVA: 0x000D7AAA File Offset: 0x000D5CAA
		public bool IsActive { get; set; }

		// Token: 0x06002121 RID: 8481 RVA: 0x000D7AB4 File Offset: 0x000D5CB4
		public ButtonUSB(string usbLetter, string usbPluggedInPath, string usbPluggedOffPath, RoutedEventHandler clickHandler)
		{
			this.usbLetter = usbLetter;
			this.usbPluggedInPath = usbPluggedInPath;
			this.usbPluggedOffPath = usbPluggedOffPath;
			this.clickHandler = clickHandler;
			this.usbImagePluggedIn = ImageConverter.ToImage(usbPluggedInPath);
			this.usbImagePluggedOff = ImageConverter.ToImage(usbPluggedOffPath);
			base.Content = ImageConverter.ToImage("/img/retrain.png");
			base.Style = (base.FindResource("defaultButtonStyle") as Style);
			base.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			base.Click += clickHandler;
			base.Unloaded += new RoutedEventHandler(this.ButtonUSB_Unloaded);
			this.SurveyUSB(null, new EventArgs());
			this.timerSurveyUSB = new DispatcherTimer(new TimeSpan(0, 0, this.surveyInterval / 1000), DispatcherPriority.Background, new EventHandler(this.SurveyUSB), base.Dispatcher);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000D7BBC File Offset: 0x000D5DBC
		public ButtonUSB(string usbLetter, string usbPluggedInPath, string usbPluggedOffPath, RoutedEventHandler clickHandler, bool isActive)
		{
			this.usbLetter = usbLetter;
			this.usbPluggedInPath = usbPluggedInPath;
			this.usbPluggedOffPath = usbPluggedOffPath;
			this.clickHandler = clickHandler;
			this.IsActive = isActive;
			this.usbImagePluggedIn = ImageConverter.ToImage(usbPluggedInPath);
			this.usbImagePluggedOff = ImageConverter.ToImage(usbPluggedOffPath);
			base.Content = this.usbImagePluggedOff;
			base.IsEnabled = false;
			base.Style = (base.FindResource("defaultButtonStyle") as Style);
			base.Margin = new Thickness(0.0, 0.0, 10.0, 0.0);
			base.Click += clickHandler;
			base.Unloaded += new RoutedEventHandler(this.ButtonUSB_Unloaded);
			this.SurveyUSBwithActivation(null, new EventArgs());
			this.timerSurveyUSB = new DispatcherTimer(new TimeSpan(0, 0, this.surveyInterval / 1000), DispatcherPriority.Background, new EventHandler(this.SurveyUSBwithActivation), base.Dispatcher);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000D7CD0 File Offset: 0x000D5ED0
		private void SurveyUSB(object dummy, EventArgs e)
		{
			bool flag = this.usbLetter != "\\" && Directory.Exists(this.usbLetter);
			if (flag)
			{
				bool flag2 = base.Content != this.usbImagePluggedIn;
				if (flag2)
				{
					base.Content = this.usbImagePluggedIn;
					base.IsEnabled = true;
				}
			}
			else
			{
				bool flag3 = base.Content != this.usbImagePluggedOff;
				if (flag3)
				{
					base.Content = this.usbImagePluggedOff;
					base.IsEnabled = false;
				}
			}
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000D7D60 File Offset: 0x000D5F60
		private void ButtonUSB_Unloaded(object sender, EventArgs e)
		{
			base.Click -= this.clickHandler;
			base.Unloaded -= new RoutedEventHandler(this.ButtonUSB_Unloaded);
			this.timerSurveyUSB.Stop();
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000D7D90 File Offset: 0x000D5F90
		private void SurveyUSBwithActivation(object dummy, EventArgs e)
		{
			bool isActive = this.IsActive;
			if (isActive)
			{
				bool flag = this.usbLetter != "\\" && Directory.Exists(this.usbLetter);
				if (flag)
				{
					bool flag2 = base.Content != this.usbImagePluggedIn;
					if (flag2)
					{
						base.Content = this.usbImagePluggedIn;
						base.IsEnabled = true;
					}
				}
				else
				{
					bool flag3 = base.Content != this.usbImagePluggedOff;
					if (flag3)
					{
						base.Content = this.usbImagePluggedOff;
						base.IsEnabled = false;
					}
				}
			}
			else
			{
				base.Content = this.usbImagePluggedOff;
			}
		}

		// Token: 0x0400156F RID: 5487
		private int surveyInterval = 1000;

		// Token: 0x04001570 RID: 5488
		private string usbLetter;

		// Token: 0x04001571 RID: 5489
		private string usbPluggedInPath;

		// Token: 0x04001572 RID: 5490
		private string usbPluggedOffPath;

		// Token: 0x04001573 RID: 5491
		private Image usbImagePluggedIn;

		// Token: 0x04001574 RID: 5492
		private Image usbImagePluggedOff;

		// Token: 0x04001575 RID: 5493
		private DispatcherTimer timerSurveyUSB;

		// Token: 0x04001576 RID: 5494
		private RoutedEventHandler clickHandler;
	}
}

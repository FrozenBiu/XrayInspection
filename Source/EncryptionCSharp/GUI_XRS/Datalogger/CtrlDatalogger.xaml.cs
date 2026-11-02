using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using DatabaseConnection;
using XrsImage;

namespace GUI_XRS.Datalogger
{
	// Token: 0x0200026B RID: 619
	public partial class CtrlDatalogger : UserControl
	{
		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06002FA1 RID: 12193 RVA: 0x0013B2E8 File Offset: 0x001394E8
		// (set) Token: 0x06002FA2 RID: 12194 RVA: 0x0013B2F0 File Offset: 0x001394F0
		public Logger Datalogger { get; set; }

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06002FA3 RID: 12195 RVA: 0x0013B2F9 File Offset: 0x001394F9
		// (set) Token: 0x06002FA4 RID: 12196 RVA: 0x0013B301 File Offset: 0x00139501
		public List<Button> Buttons { get; set; }

		// Token: 0x06002FA5 RID: 12197 RVA: 0x0013B30C File Offset: 0x0013950C
		public CtrlDatalogger(ParameterHandling parHandling, Logger logger, DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.dataloggerViewModel = new DataloggerViewModel(logger, dbConnection);
			this.Datalogger = logger;
			this.parHandling = parHandling;
			this.parameterControl.Initialize(parHandling, true, logger, true, null, null);
			this.parameterControl.StartCreation();
			this.InitializeButtons();
			base.DataContext = this.dataloggerViewModel;
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x0013B378 File Offset: 0x00139578
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			Button button = new Button();
			button.Content = ImageConverter.ToImage("/img/save.png");
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Click += this.SaveLogToUSB;
			this.Buttons.Add(button);
			Button button2 = new Button();
			bool isRunning = this.Datalogger.IsRunning;
			if (isRunning)
			{
				button2.Content = "Stop";
			}
			else
			{
				button2.Content = "Start";
			}
			button2.Click += this.Start;
			this.Buttons.Add(button2);
		}

		// Token: 0x06002FA7 RID: 12199 RVA: 0x0013B434 File Offset: 0x00139634
		public void Start(object sender, RoutedEventArgs e)
		{
			bool isRunning = this.Datalogger.IsRunning;
			if (isRunning)
			{
				this.Datalogger.StopLog();
				this.Buttons[1].Content = "Start";
			}
			else
			{
				this.Datalogger.StartLog();
				this.Buttons[1].Content = "Stop";
			}
		}

		// Token: 0x06002FA8 RID: 12200 RVA: 0x0013B49D File Offset: 0x0013969D
		public void Stop(object sender, RoutedEventArgs e)
		{
			this.Datalogger.StopLog();
		}

		// Token: 0x06002FA9 RID: 12201 RVA: 0x0013B4AC File Offset: 0x001396AC
		private void SaveLogToUSB(object sender, RoutedEventArgs e)
		{
			this.dataloggerViewModel.SaveLogsToUsb();
		}

		// Token: 0x06002FAA RID: 12202 RVA: 0x0001092C File Offset: 0x0000EB2C
		public void listBoxItem_DoubleClick(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x04001DEC RID: 7660
		private DataloggerViewModel dataloggerViewModel;

		// Token: 0x04001DED RID: 7661
		private ParameterHandling parHandling;
	}
}

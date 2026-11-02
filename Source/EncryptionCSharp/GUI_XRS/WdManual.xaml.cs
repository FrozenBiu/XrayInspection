using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x0200022A RID: 554
	public partial class WdManual : Window
	{
		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x00113DD0 File Offset: 0x00111FD0
		// (set) Token: 0x06002999 RID: 10649 RVA: 0x00113DD8 File Offset: 0x00111FD8
		public WindowState WState { get; set; }

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x00113DE4 File Offset: 0x00111FE4
		// (set) Token: 0x0600299B RID: 10651 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x0600299C RID: 10652 RVA: 0x00113DFC File Offset: 0x00111FFC
		// (set) Token: 0x0600299D RID: 10653 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x00113E14 File Offset: 0x00112014
		// (set) Token: 0x0600299F RID: 10655 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x00113E2C File Offset: 0x0011202C
		// (set) Token: 0x060029A1 RID: 10657 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x060029A2 RID: 10658 RVA: 0x00113E44 File Offset: 0x00112044
		public WdManual(Branding selectedBranding)
		{
			this.InitializeComponent();
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			base.DataContext = this;
			this.OpenManual(selectedBranding);
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x00113E9C File Offset: 0x0011209C
		public WdManual(string document)
		{
			this.InitializeComponent();
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			base.DataContext = this;
			this.browser.Navigate(new Uri(document));
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x00113F00 File Offset: 0x00112100
		private void OpenManual(Branding selectedBranding)
		{
			string str = (selectedBranding.Design == Definition.GUIDesign.SuS) ? WdManual.manualPath : WdManual.manualBizerbaPath;
			string text = str + "_" + LocalizationManager.CurrentCulture.ToString() + Definition.pdfFileEnding;
			string uriString = str + "_en-GB" + Definition.pdfFileEnding;
			bool flag = File.Exists(text);
			if (flag)
			{
				this.browser.Navigate(new Uri(text));
			}
			else
			{
				this.browser.Navigate(new Uri(uriString));
			}
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x00113F7F File Offset: 0x0011217F
		private void btnBack_Click(object sender, EventArgs e)
		{
			this.browser.Navigate(new Uri("about:blank"));
			base.Close();
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x00113F9F File Offset: 0x0011219F
		protected override void OnClosing(CancelEventArgs e)
		{
			this.browser.Dispose();
			base.OnClosing(e);
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x00113FB8 File Offset: 0x001121B8
		public static bool ManualAvailable(Branding selectedBranding)
		{
			string str = (selectedBranding.Design == Definition.GUIDesign.SuS) ? WdManual.manualPath : WdManual.manualBizerbaPath;
			string path = str + "_" + LocalizationManager.CurrentCulture.ToString() + Definition.pdfFileEnding;
			string path2 = str + "_en-GB" + Definition.pdfFileEnding;
			return File.Exists(path) || File.Exists(path2);
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x0003CA4C File Offset: 0x0003AC4C
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			WorkingArea.ClearResources(this);
		}

		// Token: 0x040019CB RID: 6603
		private static string manualPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Manuals\\XRSManual";

		// Token: 0x040019CC RID: 6604
		private static string manualBizerbaPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Manuals\\XRSManual";
	}
}

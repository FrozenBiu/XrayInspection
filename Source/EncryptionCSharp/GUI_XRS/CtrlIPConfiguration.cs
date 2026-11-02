using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x020000BA RID: 186
	public class CtrlIPConfiguration : UserControl, IComponentConnector
	{
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001262 RID: 4706 RVA: 0x0006E749 File Offset: 0x0006C949
		// (set) Token: 0x06001263 RID: 4707 RVA: 0x0006E751 File Offset: 0x0006C951
		public List<Button> Buttons { get; set; }

		// Token: 0x06001264 RID: 4708 RVA: 0x0006E75C File Offset: 0x0006C95C
		public CtrlIPConfiguration(bool wired, MachineState machineState)
		{
			this.InitializeComponent();
			this.InitializeButtons();
			this.viewModel = new IPConfigurationViewModel(wired, machineState);
			bool flag = this.viewModel.NIC != null;
			if (flag)
			{
				this.viewModel.NIC.PropertyChanged += this.NIC_PropertyChanged;
			}
			else
			{
				Dialog.Show("Error", "No network adapter found", DialogStyle.Error, DialogButtons.OK);
			}
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0006E7E0 File Offset: 0x0006C9E0
		private void InitializeButtons()
		{
			this.Buttons = new List<Button>();
			this.btnCommit = new Button();
			this.btnCommit.Content = ImageConverter.ToImage("/img/ok.png");
			this.btnCommit.Click += new RoutedEventHandler(this.CommitChanges);
			this.btnCommit.Style = (base.FindResource("okButtonStyle") as Style);
			this.btnCommit.Visibility = Visibility.Collapsed;
			this.Buttons.Add(this.btnCommit);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x0006E870 File Offset: 0x0006CA70
		private void wdSub_BackButtonClicked()
		{
			bool flag = this.wdSub != null;
			if (flag)
			{
				bool flag2 = this.viewModel != null && this.viewModel.NIC != null && this.viewModel.NIC.ConfigurationChanged;
				if (flag2)
				{
					bool flag3 = Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("txtDiscardChanges") + "?", DialogStyle.Warning, DialogButtons.OKCancel) == Dialog.DialogResults.Cancel;
					if (flag3)
					{
						this.wdSub.IgnoreNextBack = true;
					}
					else
					{
						this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
					}
				}
				else
				{
					this.wdSub.BackButtonClicked -= this.wdSub_BackButtonClicked;
				}
			}
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x0006E930 File Offset: 0x0006CB30
		private void CtrlIPConfiguration_Loaded(object sender, RoutedEventArgs e)
		{
			this.wdSub = (Window.GetWindow(this) as Subwindow);
			bool flag = this.wdSub != null;
			if (flag)
			{
				bool flag2 = this.viewModel.NIC != null;
				if (flag2)
				{
					this.wdSub.BackButtonClicked += this.wdSub_BackButtonClicked;
				}
				else
				{
					this.wdSub.Close();
				}
			}
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x0006E998 File Offset: 0x0006CB98
		private void CommitChanges(object sender, EventArgs e)
		{
			bool flag = this.viewModel.CommitChanges();
			if (flag)
			{
				this.btnCommit.Visibility = Visibility.Collapsed;
			}
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x0006E9C4 File Offset: 0x0006CBC4
		private void NIC_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "ConfigurationChanged" && this.btnCommit != null;
			if (flag)
			{
				this.btnCommit.Visibility = Visibility.Visible;
			}
			else
			{
				bool flag2 = e.PropertyName == "PreferredDNS" && this.viewModel.NIC.PreferredDNS == null;
				if (flag2)
				{
					this.ipPreferredDNS.ClearText();
				}
				else
				{
					bool flag3 = e.PropertyName == "AlternateDNS" && this.viewModel.NIC.AlternateDNS == null;
					if (flag3)
					{
						this.ipAlternateDNS.ClearText();
					}
				}
			}
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x0006EA78 File Offset: 0x0006CC78
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ipconfiguration/ctrlipconfiguration.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x0006EAB0 File Offset: 0x0006CCB0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0006EACC File Offset: 0x0006CCCC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				((CtrlIPConfiguration)target).Loaded += this.CtrlIPConfiguration_Loaded;
				break;
			case 2:
				this.btnObtainIPAutomatically = (RadioButton)target;
				break;
			case 3:
				this.btnDefineIPManually = (RadioButton)target;
				break;
			case 4:
				this.btnObtainDNSAutomatically = (RadioButtonExtended)target;
				break;
			case 5:
				this.btnDefineDNSManually = (RadioButtonExtended)target;
				break;
			case 6:
				this.ipPreferredDNS = (IPAddressBox)target;
				break;
			case 7:
				this.ipAlternateDNS = (IPAddressBox)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000C23 RID: 3107
		private IPConfigurationViewModel viewModel;

		// Token: 0x04000C24 RID: 3108
		private Subwindow wdSub;

		// Token: 0x04000C25 RID: 3109
		private Button btnCommit;

		// Token: 0x04000C27 RID: 3111
		internal RadioButton btnObtainIPAutomatically;

		// Token: 0x04000C28 RID: 3112
		internal RadioButton btnDefineIPManually;

		// Token: 0x04000C29 RID: 3113
		internal RadioButtonExtended btnObtainDNSAutomatically;

		// Token: 0x04000C2A RID: 3114
		internal RadioButtonExtended btnDefineDNSManually;

		// Token: 0x04000C2B RID: 3115
		internal IPAddressBox ipPreferredDNS;

		// Token: 0x04000C2C RID: 3116
		internal IPAddressBox ipAlternateDNS;

		// Token: 0x04000C2D RID: 3117
		private bool _contentLoaded;
	}
}

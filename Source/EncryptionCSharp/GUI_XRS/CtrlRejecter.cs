using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using DatabaseConnection;

namespace GUI_XRS
{
	// Token: 0x0200014B RID: 331
	public class CtrlRejecter : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x06001F21 RID: 7969 RVA: 0x000CF0A2 File Offset: 0x000CD2A2
		public CtrlRejecter(ParameterHandling parHandling, DBConnection dbConnection, AccessRightsManager accessManager, MachineState machineState, MessageSystem messageSystem)
		{
			this.InitializeComponent();
			this.viewModel = new RejecterViewModel(parHandling, dbConnection, accessManager, machineState, messageSystem);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x000CF0D4 File Offset: 0x000CD2D4
		private void txtEjectTime_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Rejecter;
			if (flag)
			{
				Rejecter rejecter = (Rejecter)((TextBox)sender).DataContext;
				rejecter.SetEjectTime();
			}
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x000CF120 File Offset: 0x000CD320
		private void txtDelay_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Rejecter;
			if (flag)
			{
				Rejecter rejecter = (Rejecter)((TextBox)sender).DataContext;
				rejecter.SetDelay();
			}
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x000CF16C File Offset: 0x000CD36C
		private void txtBinFullTimeout_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Rejecter;
			if (flag)
			{
				Rejecter rejecter = (Rejecter)((TextBox)sender).DataContext;
				rejecter.SetBinFullTimeout();
			}
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000CF1B8 File Offset: 0x000CD3B8
		private void txtBinUnlockTimeout_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Rejecter;
			if (flag)
			{
				Rejecter rejecter = (Rejecter)((TextBox)sender).DataContext;
				rejecter.SetBinUnlockTimeout();
			}
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000CF204 File Offset: 0x000CD404
		private void txtDebounceTime_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Rejecter;
			if (flag)
			{
				Rejecter rejecter = (Rejecter)((TextBox)sender).DataContext;
				rejecter.SetDebounceTime();
			}
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x000CF250 File Offset: 0x000CD450
		private void txtCorrectionTime_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Rejecter;
			if (flag)
			{
				Rejecter rejecter = (Rejecter)((TextBox)sender).DataContext;
				rejecter.SetCorrectionTime();
			}
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000CF29C File Offset: 0x000CD49C
		private void txtRejectVerifySensorBlockedTimeout_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Rejecter;
			if (flag)
			{
				Rejecter rejecter = (Rejecter)((TextBox)sender).DataContext;
				rejecter.SetRejectVerifySensorBlockedTimeout();
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000CF2E8 File Offset: 0x000CD4E8
		private void txtObjectBasedFlapsEjectTime_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtEjectTime"), this.viewModel.ObjectBasedFlapsEjectTime.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetObjectBasedFlapsEjectTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000CF344 File Offset: 0x000CD544
		private void numKeyboard_SetObjectBasedFlapsEjectTime(object sender, KeyboardInputEventArgs e)
		{
			int objectBasedFlapsEjectTime = 0;
			int.TryParse(e.Text, out objectBasedFlapsEjectTime);
			this.viewModel.ObjectBasedFlapsEjectTime = objectBasedFlapsEjectTime;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetObjectBasedFlapsEjectTime;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x000CF388 File Offset: 0x000CD588
		private void txtObjectBasedFlapsDelayTime_MouseUp(object sender, MouseButtonEventArgs e)
		{
			this.numKeyboard = new NumKeyboard(LocalizationManager.Localize("txtDelay"), this.viewModel.ObjectBasedFlapsDelayTime.ToString(), NumKeyboardType.TypeUInt);
			this.numKeyboard.TextInputKeyboard += this.numKeyboard_SetObjectBasedFlapsDelayTime;
			this.numKeyboard.ShowDialog();
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x000CF3E4 File Offset: 0x000CD5E4
		private void numKeyboard_SetObjectBasedFlapsDelayTime(object sender, KeyboardInputEventArgs e)
		{
			int objectBasedFlapsDelayTime = 0;
			int.TryParse(e.Text, out objectBasedFlapsDelayTime);
			this.viewModel.ObjectBasedFlapsDelayTime = objectBasedFlapsDelayTime;
			this.numKeyboard.TextInputKeyboard -= this.numKeyboard_SetObjectBasedFlapsDelayTime;
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000CF428 File Offset: 0x000CD628
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/rejecter/ctrlrejecter.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x000CF460 File Offset: 0x000CD660
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				if (connectionId != 2)
				{
					this._contentLoaded = true;
				}
				else
				{
					((TextBox)target).MouseUp += this.txtObjectBasedFlapsDelayTime_MouseUp;
				}
			}
			else
			{
				((TextBox)target).MouseUp += this.txtObjectBasedFlapsEjectTime_MouseUp;
			}
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x000CF4BC File Offset: 0x000CD6BC
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 3:
				((TextBox)target).MouseUp += this.txtEjectTime_MouseUp;
				break;
			case 4:
				((TextBox)target).MouseUp += this.txtDelay_MouseUp;
				break;
			case 5:
				((TextBox)target).MouseUp += this.txtBinFullTimeout_MouseUp;
				break;
			case 6:
				((TextBox)target).MouseUp += this.txtRejectVerifySensorBlockedTimeout_MouseUp;
				break;
			case 7:
				((TextBox)target).MouseUp += this.txtDebounceTime_MouseUp;
				break;
			case 8:
				((TextBox)target).MouseUp += this.txtCorrectionTime_MouseUp;
				break;
			case 9:
				((TextBox)target).MouseUp += this.txtBinUnlockTimeout_MouseUp;
				break;
			}
		}

		// Token: 0x04001455 RID: 5205
		private RejecterViewModel viewModel;

		// Token: 0x04001456 RID: 5206
		private NumKeyboard numKeyboard;

		// Token: 0x04001457 RID: 5207
		private bool _contentLoaded;
	}
}

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
	// Token: 0x02000148 RID: 328
	public class CtrlBuzzer : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x06001F1B RID: 7963 RVA: 0x000CEF4C File Offset: 0x000CD14C
		public CtrlBuzzer(ParameterHandling parHandling, DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.viewModel = new BuzzerViewModel(parHandling, dbConnection);
			base.DataContext = this.viewModel;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000CEF78 File Offset: 0x000CD178
		private void txtLength_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Buzzer;
			if (flag)
			{
				Buzzer buzzer = (Buzzer)((TextBox)sender).DataContext;
				buzzer.SetLength();
			}
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x000CEFC4 File Offset: 0x000CD1C4
		private void txtDelay_MouseUp(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is Buzzer;
			if (flag)
			{
				Buzzer buzzer = (Buzzer)((TextBox)sender).DataContext;
				buzzer.SetDelay();
			}
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x000CF010 File Offset: 0x000CD210
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/rejecter/ctrlbuzzer.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x000CF046 File Offset: 0x000CD246
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			this._contentLoaded = true;
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x000CF050 File Offset: 0x000CD250
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 1)
			{
				if (connectionId == 2)
				{
					((TextBox)target).MouseUp += this.txtDelay_MouseUp;
				}
			}
			else
			{
				((TextBox)target).MouseUp += this.txtLength_MouseUp;
			}
		}

		// Token: 0x0400144B RID: 5195
		private BuzzerViewModel viewModel;

		// Token: 0x0400144C RID: 5196
		private bool _contentLoaded;
	}
}

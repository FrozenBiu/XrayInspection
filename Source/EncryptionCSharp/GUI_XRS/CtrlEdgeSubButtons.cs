using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x0200010B RID: 267
	public class CtrlEdgeSubButtons : UserControl, IComponentConnector
	{
		// Token: 0x1400007F RID: 127
		// (add) Token: 0x06001948 RID: 6472 RVA: 0x0009584C File Offset: 0x00093A4C
		// (remove) Token: 0x06001949 RID: 6473 RVA: 0x00095884 File Offset: 0x00093A84
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> EdgePropertyChanged;

		// Token: 0x17000791 RID: 1937
		// (get) Token: 0x0600194A RID: 6474 RVA: 0x000958BC File Offset: 0x00093ABC
		public EdgeFilterMode EdgeMode
		{
			get
			{
				return (this.viewModel != null) ? this.viewModel.SelectedEdgeMode : EdgeFilterMode.Remove;
			}
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x000958E4 File Offset: 0x00093AE4
		public CtrlEdgeSubButtons(ParameterHandling parHandling, AccessRightsManager accessManager, int imageGroup)
		{
			this.InitializeComponent();
			this.viewModel = new EdgeSubButtonsViewModel(parHandling, accessManager, imageGroup);
			this.viewModel.PropertyChanged += this.viewModel_PropertyChanged;
			base.DataContext = this.viewModel;
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00095933 File Offset: 0x00093B33
		private void viewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnEdgePropertyChanged(e.PropertyName);
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x00095944 File Offset: 0x00093B44
		protected virtual void OnEdgePropertyChanged(string propertyName)
		{
			Action<string> edgePropertyChanged = this.EdgePropertyChanged;
			bool flag = edgePropertyChanged != null;
			if (flag)
			{
				edgePropertyChanged(propertyName);
			}
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00095969 File Offset: 0x00093B69
		private void CtrlEdgeSubButtons_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.PropertyChanged -= this.viewModel_PropertyChanged;
			this.viewModel.CleanUp();
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00095990 File Offset: 0x00093B90
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/filter/ctrledgesubbuttons.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x000959C8 File Offset: 0x00093BC8
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
				((CtrlEdgeSubButtons)target).Unloaded += this.CtrlEdgeSubButtons_Unloaded;
			}
		}

		// Token: 0x0400108B RID: 4235
		private EdgeSubButtonsViewModel viewModel;

		// Token: 0x0400108D RID: 4237
		private bool _contentLoaded;
	}
}

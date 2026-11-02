using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x020000D1 RID: 209
	public class CtrlSWButtonsToggle : UserControl, IComponentConnector
	{
		// Token: 0x0600146E RID: 5230 RVA: 0x000784E0 File Offset: 0x000766E0
		public CtrlSWButtonsToggle()
		{
			this.InitializeComponent();
			base.Unloaded += this.CtrlSWButtonsToggle_Unloaded;
			HomeManager.Instance.PropertyChanged += this.HomeManager_PropertyChanged;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00078530 File Offset: 0x00076730
		private void btn_Checked(object sender, RoutedEventArgs e)
		{
			bool flag = this.popup == null;
			if (flag)
			{
				this.popup = new Popup();
				this.popup.AllowsTransparency = true;
				this.popup.Placement = PlacementMode.Bottom;
				this.popup.PlacementTarget = this.btn;
				CtrlSWButtons child = new CtrlSWButtons();
				this.popup.Child = child;
			}
			this.popup.IsOpen = true;
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x000785A4 File Offset: 0x000767A4
		private void btn_Unchecked(object sender, RoutedEventArgs e)
		{
			this.HidePopup();
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x000785B0 File Offset: 0x000767B0
		public void HidePopup()
		{
			bool flag = this.popup != null;
			if (flag)
			{
				this.popup.IsOpen = false;
			}
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x000785D8 File Offset: 0x000767D8
		private void HomeManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = HomeManager.Instance.NumSubWindows == 0 && this.popup != null;
			if (flag)
			{
				this.popup.IsOpen = false;
				this.btn.IsChecked = new bool?(false);
			}
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x00078623 File Offset: 0x00076823
		private void CtrlSWButtonsToggle_Unloaded(object sender, RoutedEventArgs e)
		{
			HomeManager.Instance.PropertyChanged -= this.HomeManager_PropertyChanged;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x00078640 File Offset: 0x00076840
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/subwindow/ctrlswbuttonstoggle.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x00078678 File Offset: 0x00076878
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
				this.btn = (ToggleButton)target;
				this.btn.Checked += this.btn_Checked;
				this.btn.Unchecked += this.btn_Unchecked;
			}
		}

		// Token: 0x04000D82 RID: 3458
		private Popup popup = null;

		// Token: 0x04000D83 RID: 3459
		internal ToggleButton btn;

		// Token: 0x04000D84 RID: 3460
		private bool _contentLoaded;
	}
}

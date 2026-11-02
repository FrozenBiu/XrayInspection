using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using DatabaseConnection;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000253 RID: 595
	public class Subwindow : Window, IComponentConnector
	{
		// Token: 0x14000104 RID: 260
		// (add) Token: 0x06002D78 RID: 11640 RVA: 0x00129864 File Offset: 0x00127A64
		// (remove) Token: 0x06002D79 RID: 11641 RVA: 0x0012989C File Offset: 0x00127A9C
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action BackButtonClicked;

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x001298D4 File Offset: 0x00127AD4
		// (set) Token: 0x06002D7B RID: 11643 RVA: 0x001298EC File Offset: 0x00127AEC
		public List<Button> PanelButtons
		{
			get
			{
				return new List<Button>();
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					foreach (Button button in value)
					{
						button.Margin = new Thickness(5.0);
						this.buttonPanel.Children.Add(button);
					}
				}
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x06002D7C RID: 11644 RVA: 0x0012996C File Offset: 0x00127B6C
		// (set) Token: 0x06002D7D RID: 11645 RVA: 0x00129984 File Offset: 0x00127B84
		public UIElement ControlUIElement
		{
			get
			{
				return new UIElement();
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.buttonPanel.Children.Add(value);
				}
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x06002D7E RID: 11646 RVA: 0x001299AC File Offset: 0x00127BAC
		// (set) Token: 0x06002D7F RID: 11647 RVA: 0x001299C4 File Offset: 0x00127BC4
		public UIElement ControlUIElementRightAligned
		{
			get
			{
				return new UIElement();
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this.panelRightAligned.Children.Add(value);
				}
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x001299EC File Offset: 0x00127BEC
		// (set) Token: 0x06002D81 RID: 11649 RVA: 0x001299F3 File Offset: 0x00127BF3
		public static double ActualContentHeight { get; set; }

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06002D82 RID: 11650 RVA: 0x001299FB File Offset: 0x00127BFB
		// (set) Token: 0x06002D83 RID: 11651 RVA: 0x00129A03 File Offset: 0x00127C03
		public bool IgnoreNextBack { get; set; }

		// Token: 0x06002D84 RID: 11652 RVA: 0x00129A0C File Offset: 0x00127C0C
		public Subwindow(UserControl usrControl, SubwindowButtons buttonOption, bool bringOthersToFrontAfterClose, bool warningStyle, Window owner, ParameterHandling parHandling, ProductManager productManager, DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.viewModel = new SubwindowViewModel(buttonOption, warningStyle, parHandling, productManager, dbConnection);
			bool flag = dbConnection != null;
			if (flag)
			{
				dbConnection.GetDemoMode(ref this.demoMode);
			}
			this.btnBack.DataContext = this.viewModel;
			this.btnHome.DataContext = this.viewModel;
			this.brdMainGrid.DataContext = this.viewModel;
			this.contentGrid.Children.Add(usrControl);
			this.userControl = usrControl;
			bool flag2 = Thread.CurrentThread.Name == null;
			if (flag2)
			{
				string name = this.ToString() + "-" + usrControl.ToString();
				Thread.CurrentThread.Name = name;
			}
			this.bringOthersToFrontAfterClose = bringOthersToFrontAfterClose;
			this.SetOwner(owner);
			Machine.Instance.PropertyChanged += this.Machine_PropertyChanged;
			MainWindowViewModel.accessRightsManager.UserAutoLogout += this.accessRightsManager_UserAutoLogout;
			base.DataContext = this.viewModel;
		}

		// Token: 0x06002D85 RID: 11653 RVA: 0x00129B2A File Offset: 0x00127D2A
		private void accessRightsManager_UserAutoLogout()
		{
			HomeManager.Instance.CloseAll();
		}

		// Token: 0x06002D86 RID: 11654 RVA: 0x00129B38 File Offset: 0x00127D38
		public static void Show(UserControl usrControl)
		{
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome, new List<Button>());
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x00129B48 File Offset: 0x00127D48
		public static void Show(UserControl usrControl, List<Button> panelButtons)
		{
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome, panelButtons);
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x00129B54 File Offset: 0x00127D54
		public static void Show(UserControl usrControl, UIElement controlUIElement)
		{
			Subwindow.Show(usrControl, SubwindowButtons.BackAndHome, controlUIElement);
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x00129B60 File Offset: 0x00127D60
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption)
		{
			Subwindow.Show(usrControl, buttonOption, new List<Button>());
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x00129B70 File Offset: 0x00127D70
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, List<Button> panelButtons)
		{
			Subwindow.Show(usrControl, buttonOption, panelButtons, true);
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x00129B7D File Offset: 0x00127D7D
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, UIElement controlUIElement)
		{
			Subwindow.Show(usrControl, buttonOption, controlUIElement, true);
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x00129B8A File Offset: 0x00127D8A
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, UIElement controlUIElementRightAligned, int dummy)
		{
			Subwindow.subwindow = new Subwindow(usrControl, buttonOption, true, false, null, null, null, null);
			Subwindow.subwindow.ControlUIElementRightAligned = controlUIElementRightAligned;
			HomeManager.Instance.Register(Subwindow.subwindow);
			Subwindow.subwindow.Show();
		}

		// Token: 0x06002D8D RID: 11661 RVA: 0x00129BC8 File Offset: 0x00127DC8
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, UIElement controlUIElement, UIElement controlUIElementRightAligned)
		{
			Subwindow.subwindow = new Subwindow(usrControl, buttonOption, true, false, null, null, null, null);
			Subwindow.subwindow.ControlUIElement = controlUIElement;
			Subwindow.subwindow.ControlUIElementRightAligned = controlUIElementRightAligned;
			HomeManager.Instance.Register(Subwindow.subwindow);
			Subwindow.subwindow.Show();
		}

		// Token: 0x06002D8E RID: 11662 RVA: 0x00129C1B File Offset: 0x00127E1B
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, List<Button> panelButtons, bool bringOthersToFrontAfterClose)
		{
			Subwindow.Show(usrControl, buttonOption, panelButtons, true, false, null);
		}

		// Token: 0x06002D8F RID: 11663 RVA: 0x00129C2A File Offset: 0x00127E2A
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, List<Button> panelButtons, bool bringOthersToFrontAfterClose, bool warningMode, Window owner)
		{
			Subwindow.subwindow = new Subwindow(usrControl, buttonOption, bringOthersToFrontAfterClose, warningMode, owner, null, null, null);
			Subwindow.subwindow.PanelButtons = panelButtons;
			HomeManager.Instance.Register(Subwindow.subwindow);
			Subwindow.subwindow.Show();
		}

		// Token: 0x06002D90 RID: 11664 RVA: 0x00129C68 File Offset: 0x00127E68
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, UIElement controlUIElement, bool bringOthersToFrontAfterClose)
		{
			Subwindow.subwindow = new Subwindow(usrControl, buttonOption, bringOthersToFrontAfterClose, false, null, null, null, null);
			Subwindow.subwindow.ControlUIElement = controlUIElement;
			HomeManager.Instance.Register(Subwindow.subwindow);
			Subwindow.subwindow.Show();
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x00129CA4 File Offset: 0x00127EA4
		public static Subwindow.AdjustmentResults ShowAutotrainAdjustments(UserControl usrControl, SubwindowButtons buttonOption, UIElement controlUIElement, bool bringOthersToFrontAfterClose, ParameterHandling parHandling, ProductManager productManager, DBConnection dbConnection)
		{
			Subwindow.subwindowAuto = new Subwindow(usrControl, buttonOption, bringOthersToFrontAfterClose, false, null, parHandling, productManager, dbConnection);
			HomeManager.Instance.Register(Subwindow.subwindowAuto);
			Subwindow.subwindowAuto.ShowDialog();
			return Subwindow.subwindowAuto.result;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x00129CF0 File Offset: 0x00127EF0
		private void back_Click(object sender, RoutedEventArgs e)
		{
			this.OnBackButtonClicked();
			bool flag = !this.IgnoreNextBack;
			if (flag)
			{
				bool flag2 = this.bringOthersToFrontAfterClose;
				if (flag2)
				{
					HomeManager.Instance.BringWindowsToFront();
				}
				this.Close();
			}
			else
			{
				this.IgnoreNextBack = false;
			}
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x00129D3C File Offset: 0x00127F3C
		public void confirm_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Subwindow.subwindowAuto != null;
			if (flag)
			{
				Subwindow.subwindowAuto.result = Subwindow.AdjustmentResults.Ok;
			}
			this.OnBackButtonClicked();
			bool flag2 = !this.IgnoreNextBack;
			if (flag2)
			{
				bool flag3 = this.bringOthersToFrontAfterClose;
				if (flag3)
				{
					HomeManager.Instance.BringWindowsToFront();
				}
				this.Close();
			}
			else
			{
				this.IgnoreNextBack = false;
			}
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x00129DA0 File Offset: 0x00127FA0
		private void cancel_Click(object sender, RoutedEventArgs e)
		{
			bool flag = Subwindow.subwindowAuto != null;
			if (flag)
			{
				Subwindow.subwindowAuto.result = Subwindow.AdjustmentResults.Cancel;
			}
			this.OnBackButtonClicked();
			bool flag2 = !this.IgnoreNextBack;
			if (flag2)
			{
				HomeManager.Instance.CloseAll();
			}
			else
			{
				this.IgnoreNextBack = false;
			}
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x00129DEE File Offset: 0x00127FEE
		private void Window_Closing(object sender, CancelEventArgs e)
		{
			WorkingArea.ClearResources(this);
			MainWindowViewModel.accessRightsManager.UserAutoLogout -= this.accessRightsManager_UserAutoLogout;
			HomeManager.Instance.Deregister(this);
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x00129E1C File Offset: 0x0012801C
		private void btnHome_Click(object sender, RoutedEventArgs e)
		{
			this.OnBackButtonClicked();
			bool flag = !this.IgnoreNextBack;
			if (flag)
			{
				bool flag2 = !this.IgnoreNextBack;
				if (flag2)
				{
					HomeManager.Instance.CloseAll();
				}
				else
				{
					this.IgnoreNextBack = false;
				}
			}
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000F8D1B File Offset: 0x000F6F1B
		private void subWindow_Click(object sender, RoutedEventArgs e)
		{
			MainWindowViewModel.accessRightsManager.SignalActivity();
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000F8D1B File Offset: 0x000F6F1B
		private void subWindow_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MainWindowViewModel.accessRightsManager.SignalActivity();
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x00129E62 File Offset: 0x00128062
		private void FormFadeOut_Completed(object sender, EventArgs e)
		{
			this.Close();
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x00129E6C File Offset: 0x0012806C
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			compositionTarget.RenderMode = RenderMode.SoftwareOnly;
			this.subWindowContent.DataContext = this;
			Subwindow.ActualContentHeight = this.subWindowContent.ActualHeight;
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x00129EB4 File Offset: 0x001280B4
		public new void Close()
		{
			bool flag = this.userControl != null;
			if (flag)
			{
				bool flag2 = this.userControl.GetType().Equals(typeof(CtrlConveyor));
				if (flag2)
				{
					((ConveyorViewModel)this.userControl.DataContext).CleanUp(null);
				}
			}
			base.Close();
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x00129F10 File Offset: 0x00128110
		protected virtual void OnBackButtonClicked()
		{
			Action backButtonClicked = this.BackButtonClicked;
			bool flag = backButtonClicked != null;
			if (flag)
			{
				backButtonClicked();
			}
		}

		// Token: 0x06002D9D RID: 11677 RVA: 0x00129F34 File Offset: 0x00128134
		public void RemovePanelButtons()
		{
			this.buttonPanel.Children.Clear();
		}

		// Token: 0x06002D9E RID: 11678 RVA: 0x00129F48 File Offset: 0x00128148
		private void subWindow_Unloaded(object sender, RoutedEventArgs e)
		{
			bool flag = this.viewModel != null;
			if (flag)
			{
				this.viewModel.CleanUp();
			}
			this.userControl = null;
			Machine.Instance.PropertyChanged -= this.Machine_PropertyChanged;
			this.viewModel.DeregisterLocalizedText();
			this.buttonPanel.Children.Clear();
			this.panelRightAligned.Children.Clear();
			this.contentGrid.Children.Clear();
		}

		// Token: 0x06002D9F RID: 11679 RVA: 0x00129FCC File Offset: 0x001281CC
		private void SetOwner(Window owner)
		{
			base.Owner = ((owner != null || Machine.Instance.MachineType == MachineType.RayconXRS || Machine.Instance.MachineType == MachineType.RayconD || this.demoMode) ? owner : Application.Current.MainWindow);
		}

		// Token: 0x06002DA0 RID: 11680 RVA: 0x0012A008 File Offset: 0x00128208
		private void Machine_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			bool flag = e.PropertyName == "MachineType";
			if (flag)
			{
				this.SetOwner(base.Owner);
			}
		}

		// Token: 0x06002DA1 RID: 11681 RVA: 0x00092B8B File Offset: 0x00090D8B
		private void subWindow_ContentRendered(object sender, EventArgs e)
		{
			base.Activate();
		}

		// Token: 0x06002DA2 RID: 11682 RVA: 0x0012A03C File Offset: 0x0012823C
		public static void Show(UserControl usrControl, SubwindowButtons buttonOption, bool isHistory)
		{
			if (isHistory)
			{
				MainWindowViewModel.accessRightsManager.OnResetRetrain();
			}
			Subwindow.Show(usrControl, buttonOption);
		}

		// Token: 0x06002DA3 RID: 11683 RVA: 0x0012A064 File Offset: 0x00128264
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/subwindow/subwindow.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x0012A09C File Offset: 0x0012829C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.subWindow = (Subwindow)target;
				this.subWindow.Closing += this.Window_Closing;
				this.subWindow.Loaded += this.Window_Loaded;
				this.subWindow.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.subWindow_Click));
				this.subWindow.MouseUp += this.subWindow_MouseUp;
				this.subWindow.Unloaded += this.subWindow_Unloaded;
				this.subWindow.ContentRendered += this.subWindow_ContentRendered;
				break;
			case 2:
				this.overlay = (Grid)target;
				break;
			case 3:
				this.subWindowContent = (Grid)target;
				break;
			case 4:
				this.subWindowContentRow = (RowDefinition)target;
				break;
			case 5:
				this.mainGrid = (Grid)target;
				break;
			case 6:
				this.brdMainGrid = (Border)target;
				break;
			case 7:
				this.contentGrid = (Grid)target;
				break;
			case 8:
				this.mainContent = (Border)target;
				break;
			case 9:
				this.buttonGrid = (Grid)target;
				break;
			case 10:
				this.btnCancel = (Button)target;
				this.btnCancel.Click += this.cancel_Click;
				break;
			case 11:
				this.btnConfirm = (Button)target;
				this.btnConfirm.Click += this.confirm_Click;
				break;
			case 12:
				this.btnHome = (Button)target;
				this.btnHome.Click += this.btnHome_Click;
				break;
			case 13:
				this.btnBack = (Button)target;
				this.btnBack.Click += this.back_Click;
				break;
			case 14:
				this.buttonPanel = (StackPanel)target;
				break;
			case 15:
				this.panelRightAligned = (StackPanel)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04001C66 RID: 7270
		private bool bringOthersToFrontAfterClose;

		// Token: 0x04001C67 RID: 7271
		public bool AutotrainAdjustmentClick = false;

		// Token: 0x04001C68 RID: 7272
		private Subwindow.AdjustmentResults result;

		// Token: 0x04001C69 RID: 7273
		private SubwindowViewModel viewModel;

		// Token: 0x04001C6A RID: 7274
		private static Subwindow subwindowAuto;

		// Token: 0x04001C6B RID: 7275
		private static Subwindow subwindow;

		// Token: 0x04001C6C RID: 7276
		private UserControl userControl;

		// Token: 0x04001C70 RID: 7280
		private bool demoMode;

		// Token: 0x04001C71 RID: 7281
		internal Subwindow subWindow;

		// Token: 0x04001C72 RID: 7282
		internal Grid overlay;

		// Token: 0x04001C73 RID: 7283
		internal Grid subWindowContent;

		// Token: 0x04001C74 RID: 7284
		internal RowDefinition subWindowContentRow;

		// Token: 0x04001C75 RID: 7285
		internal Grid mainGrid;

		// Token: 0x04001C76 RID: 7286
		internal Border brdMainGrid;

		// Token: 0x04001C77 RID: 7287
		internal Grid contentGrid;

		// Token: 0x04001C78 RID: 7288
		internal Border mainContent;

		// Token: 0x04001C79 RID: 7289
		internal Grid buttonGrid;

		// Token: 0x04001C7A RID: 7290
		internal Button btnCancel;

		// Token: 0x04001C7B RID: 7291
		internal Button btnConfirm;

		// Token: 0x04001C7C RID: 7292
		internal Button btnHome;

		// Token: 0x04001C7D RID: 7293
		internal Button btnBack;

		// Token: 0x04001C7E RID: 7294
		internal StackPanel buttonPanel;

		// Token: 0x04001C7F RID: 7295
		internal StackPanel panelRightAligned;

		// Token: 0x04001C80 RID: 7296
		private bool _contentLoaded;

		// Token: 0x02000392 RID: 914
		public enum AdjustmentResults
		{
			// Token: 0x04002227 RID: 8743
			Ok,
			// Token: 0x04002228 RID: 8744
			Cancel
		}
	}
}

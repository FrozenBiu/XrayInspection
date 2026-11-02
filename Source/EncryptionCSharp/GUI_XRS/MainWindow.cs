using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace GUI_XRS
{
	// Token: 0x020000FF RID: 255
	public class MainWindow : Window, IComponentConnector
	{
		// Token: 0x0600186E RID: 6254 RVA: 0x00092768 File Offset: 0x00090968
		public MainWindow()
		{
			this.InitializeComponent();
			this.viewModel = new MainWindowViewModel(this);
			CursorsSettings.SetCursor(this);
			base.DataContext = this.viewModel;
			this.viewModel.SetControlRights(this.btnLogbook, this.btnLearn, this.btnProduct, this.btnSettings, this.btnStartLiveModeBulk, this.btnStartObjectModeBulk, this.btnBulkSortingOff, null, null, null, null, this.btnUnlockBin, this.SliderBulk);
			this.timerInspectingAnimation = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100), DispatcherPriority.Background, new EventHandler(this.InspectingAnimation), base.Dispatcher);
			this.timerInspectingAnimation.Start();
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x00092828 File Offset: 0x00090A28
		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			hwndSource.AddHook(new HwndSourceHook(this.WndProc));
			HwndSource hwndSource2 = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
			bool flag = hwndSource2 != null;
			if (flag)
			{
				IntPtr handle = hwndSource2.Handle;
				hwndSource.AddHook(new HwndSourceHook(this.HwndHandler));
				UsbNotification.RegisterUsbDeviceNotification(handle);
			}
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x0009289C File Offset: 0x00090A9C
		private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
		{
			bool flag = msg == 537;
			if (flag)
			{
				int num = (int)wparam;
				int num2 = num;
				if (num2 != 32768)
				{
					if (num2 == 32772)
					{
						Debug.WriteLine("USB Device Removed....");
						UsbNotification.OnUsbDeviceDetached();
					}
				}
				else
				{
					Debug.WriteLine("USB Device Added!");
					UsbNotification.OnUsbDeviceAttached();
				}
			}
			handled = false;
			return IntPtr.Zero;
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x00092908 File Offset: 0x00090B08
		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			bool flag = msg == 537;
			if (flag)
			{
				bool flag2 = (int)wParam == 32768;
				if (flag2)
				{
					bool flag3 = this.viewModel != null;
					if (flag3)
					{
						this.viewModel.TryLoginToGo();
					}
				}
				else
				{
					bool flag4 = (int)wParam == 32772;
					if (flag4)
					{
						bool flag5 = this.viewModel != null;
						if (flag5)
						{
							this.viewModel.LogoutToGoUser();
						}
					}
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x0009298C File Offset: 0x00090B8C
		private void InspectingAnimation(object sender, EventArgs e)
		{
			bool flag = this.inspectingIconAnimationUpMode;
			if (flag)
			{
				bool flag2 = this.InspectingIcon.Height < 34.0;
				if (flag2)
				{
					this.InspectingIcon.Height += 1.0;
				}
				else
				{
					this.inspectingIconAnimationUpMode = false;
				}
			}
			else
			{
				bool flag3 = this.InspectingIcon.Height > 20.0;
				if (flag3)
				{
					this.InspectingIcon.Height -= 1.0;
				}
				else
				{
					this.inspectingIconAnimationUpMode = true;
				}
			}
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x00092A28 File Offset: 0x00090C28
		private void GUI_loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
				HwndTarget compositionTarget = hwndSource.CompositionTarget;
				compositionTarget.RenderMode = RenderMode.SoftwareOnly;
				AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;
			}
			catch (Exception ex)
			{
				Dialog.Show(LocalizationManager.Localize("txtError"), ex.Message, DialogStyle.Error);
			}
			bool flag = this.btnMessageSystem.ContextMenu != null;
			if (flag)
			{
				this.btnMessageSystem.ContextMenu.PlacementTarget = this;
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x00092ABC File Offset: 0x00090CBC
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Exception ex = e.ExceptionObject as Exception;
			string text = string.Empty;
			while (ex != null)
			{
				text = string.Concat(new string[]
				{
					text,
					ex.GetType().Name,
					": ",
					ex.Message,
					Environment.NewLine
				});
				ex = ex.InnerException;
			}
			Dialog.Show(LocalizationManager.Localize("txtError"), text, DialogStyle.Error);
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x00092B38 File Offset: 0x00090D38
		private void mainWindow_Click(object sender, RoutedEventArgs e)
		{
			this.viewModel.SignalActivity();
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x00092B48 File Offset: 0x00090D48
		private void GetVisuals(Visual parent, List<Visual> collection)
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				Visual visual = (Visual)VisualTreeHelper.GetChild(parent, i);
				collection.Add(visual);
				this.GetVisuals(visual, collection);
			}
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x00092B8B File Offset: 0x00090D8B
		private void mainWindow_ContentRendered(object sender, EventArgs e)
		{
			base.Activate();
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00092B98 File Offset: 0x00090D98
		private void buttonScreenshot_Click(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.viewModel.takeScreenshot();
			if (flag)
			{
				DoubleAnimation animation = new DoubleAnimation(0.0, 0.8, TimeSpan.FromMilliseconds(300.0));
				DoubleAnimation animation2 = new DoubleAnimation(0.8, 0.0, TimeSpan.FromMilliseconds(300.0));
				this.buttonScreenshot.BeginAnimation(UIElement.OpacityProperty, animation);
				this.buttonScreenshot.BeginAnimation(UIElement.OpacityProperty, animation2);
			}
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00092C34 File Offset: 0x00090E34
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/mainwindow/mainwindow.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x00092C6C File Offset: 0x00090E6C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00092C88 File Offset: 0x00090E88
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.mainWindow = (MainWindow)target;
				this.mainWindow.Loaded += this.GUI_loaded;
				this.mainWindow.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.mainWindow_Click));
				this.mainWindow.ContentRendered += this.mainWindow_ContentRendered;
				break;
			case 2:
				this.buttonScreenshot = (Button)target;
				this.buttonScreenshot.MouseDoubleClick += this.buttonScreenshot_Click;
				break;
			case 3:
				this.btnChangeUser = (Button)target;
				break;
			case 4:
				this.tbUser = (TextBlock)target;
				break;
			case 5:
				this.XrayOnIcon = (Image)target;
				break;
			case 6:
				this.InspectingIcon = (Image)target;
				break;
			case 7:
				this.DataLoggingIcon = (Image)target;
				break;
			case 8:
				this.btnMessageSystem = (Button)target;
				break;
			case 9:
				this.btnResetMessages = (Button)target;
				break;
			case 10:
				this.defaultXRay = (Button)target;
				break;
			case 11:
				this.btnStartXRay = (Button)target;
				break;
			case 12:
				this.btnLogbook = (Button)target;
				break;
			case 13:
				this.btnLearn = (Button)target;
				break;
			case 14:
				this.btnProduct = (Button)target;
				break;
			case 15:
				this.btnSettings = (Button)target;
				break;
			case 16:
				this.btnAIStatus = (Button)target;
				break;
			case 17:
				this.btnUnlockBin = (Button)target;
				break;
			case 18:
				this.btnHighSpeedImage = (Button)target;
				break;
			case 19:
				this.btnAutoRetrainOnOff = (Button)target;
				break;
			case 20:
				this.btnAutoRetrainUndo = (Button)target;
				break;
			case 21:
				this.btnStartLiveModeBulk = (Button)target;
				break;
			case 22:
				this.btnStartObjectModeBulk = (Button)target;
				break;
			case 23:
				this.btnBulkSortingOff = (Button)target;
				break;
			case 24:
				this.SliderBulk = (Border)target;
				break;
			case 25:
				this.BulkSensitivity = (TextBox)target;
				break;
			case 26:
				this.mainWindowContent = (Border)target;
				break;
			case 27:
				this.tabCtrl = (TabControl)target;
				break;
			case 28:
				this.btnHighSpeedImage2 = (Button)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04000FDC RID: 4060
		private const int DBT_DEVICEARRIVAL = 32768;

		// Token: 0x04000FDD RID: 4061
		private const int DBT_DEVICEREMOVECOMPLETE = 32772;

		// Token: 0x04000FDE RID: 4062
		private const int WM_DEVICECHANGE = 537;

		// Token: 0x04000FDF RID: 4063
		public static LocalizationManager localizationManager = new LocalizationManager();

		// Token: 0x04000FE0 RID: 4064
		private MainWindowViewModel viewModel;

		// Token: 0x04000FE1 RID: 4065
		private DispatcherTimer timerInspectingAnimation;

		// Token: 0x04000FE2 RID: 4066
		private bool inspectingIconAnimationUpMode = true;

		// Token: 0x04000FE3 RID: 4067
		internal MainWindow mainWindow;

		// Token: 0x04000FE4 RID: 4068
		internal Button buttonScreenshot;

		// Token: 0x04000FE5 RID: 4069
		internal Button btnChangeUser;

		// Token: 0x04000FE6 RID: 4070
		internal TextBlock tbUser;

		// Token: 0x04000FE7 RID: 4071
		internal Image XrayOnIcon;

		// Token: 0x04000FE8 RID: 4072
		internal Image InspectingIcon;

		// Token: 0x04000FE9 RID: 4073
		internal Image DataLoggingIcon;

		// Token: 0x04000FEA RID: 4074
		internal Button btnMessageSystem;

		// Token: 0x04000FEB RID: 4075
		internal Button btnResetMessages;

		// Token: 0x04000FEC RID: 4076
		internal Button defaultXRay;

		// Token: 0x04000FED RID: 4077
		internal Button btnStartXRay;

		// Token: 0x04000FEE RID: 4078
		internal Button btnLogbook;

		// Token: 0x04000FEF RID: 4079
		internal Button btnLearn;

		// Token: 0x04000FF0 RID: 4080
		internal Button btnProduct;

		// Token: 0x04000FF1 RID: 4081
		internal Button btnSettings;

		// Token: 0x04000FF2 RID: 4082
		internal Button btnAIStatus;

		// Token: 0x04000FF3 RID: 4083
		internal Button btnUnlockBin;

		// Token: 0x04000FF4 RID: 4084
		internal Button btnHighSpeedImage;

		// Token: 0x04000FF5 RID: 4085
		internal Button btnAutoRetrainOnOff;

		// Token: 0x04000FF6 RID: 4086
		internal Button btnAutoRetrainUndo;

		// Token: 0x04000FF7 RID: 4087
		internal Button btnStartLiveModeBulk;

		// Token: 0x04000FF8 RID: 4088
		internal Button btnStartObjectModeBulk;

		// Token: 0x04000FF9 RID: 4089
		internal Button btnBulkSortingOff;

		// Token: 0x04000FFA RID: 4090
		internal Border SliderBulk;

		// Token: 0x04000FFB RID: 4091
		internal TextBox BulkSensitivity;

		// Token: 0x04000FFC RID: 4092
		internal Border mainWindowContent;

		// Token: 0x04000FFD RID: 4093
		internal TabControl tabCtrl;

		// Token: 0x04000FFE RID: 4094
		internal Button btnHighSpeedImage2;

		// Token: 0x04000FFF RID: 4095
		private bool _contentLoaded;
	}
}

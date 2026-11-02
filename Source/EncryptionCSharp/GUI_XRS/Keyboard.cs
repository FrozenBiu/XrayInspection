using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x02000243 RID: 579
	public class Keyboard : Window, IComponentConnector
	{
		// Token: 0x140000F7 RID: 247
		// (add) Token: 0x06002BFE RID: 11262 RVA: 0x00122108 File Offset: 0x00120308
		// (remove) Token: 0x06002BFF RID: 11263 RVA: 0x00122140 File Offset: 0x00120340
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<KeyboardInputEventArgs> TextInputKeyboard;

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06002C00 RID: 11264 RVA: 0x00122178 File Offset: 0x00120378
		// (set) Token: 0x06002C01 RID: 11265 RVA: 0x00122195 File Offset: 0x00120395
		public bool ShouldClose
		{
			get
			{
				return this.viewModel.ShouldClose;
			}
			set
			{
				this.viewModel.ShouldClose = value;
			}
		}

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x001221A8 File Offset: 0x001203A8
		// (set) Token: 0x06002C03 RID: 11267 RVA: 0x001221C5 File Offset: 0x001203C5
		public bool HasEnterButton
		{
			get
			{
				return this.viewModel.HasEnterButton;
			}
			set
			{
				this.viewModel.HasEnterButton = value;
			}
		}

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06002C04 RID: 11268 RVA: 0x001221D8 File Offset: 0x001203D8
		// (set) Token: 0x06002C05 RID: 11269 RVA: 0x001221F5 File Offset: 0x001203F5
		public int MaxTextLength
		{
			get
			{
				return this.viewModel.MaxTextLength;
			}
			set
			{
				this.viewModel.MaxTextLength = value;
			}
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x00122208 File Offset: 0x00120408
		public Keyboard(string name, string oldValue, KeyboardType type)
		{
			this.InitializeComponent();
			this.viewModel = new KeyboardViewModel(new Action(base.Close));
			this.viewModel.Type = type;
			this.viewModel.PasswordBox = this.passwordBox;
			this.viewModel.OldValue = ((oldValue != null) ? oldValue : "");
			this.viewModel.Description = ((name != null) ? name : "");
			this.viewModel.Committed += this.viewModel_Committed;
			bool flag = type != KeyboardType.Password;
			if (flag)
			{
				this.viewModel.InputText = this.viewModel.OldValue;
				this.viewModel.InputBoxSelectionLength = this.viewModel.OldValue.Length;
				this.inputBox.Focus();
			}
			bool flag2 = type == KeyboardType.Cancelable;
			if (flag2)
			{
				this.viewModel.CancelVisibility = true;
				bool flag3 = !this.viewModel.Widescreen;
				if (flag3)
				{
					this.cancelPanel.Margin = new Thickness(75.0, 10.0, 20.0, 0.0);
				}
				else
				{
					this.cancelPanel2.Margin = new Thickness(0.0, 10.0, 0.0, 0.0);
				}
			}
			else
			{
				this.viewModel.CancelVisibility = false;
			}
			bool flag4 = type == KeyboardType.Email;
			if (flag4)
			{
				this.viewModel.EmailVisibility = true;
			}
			else
			{
				this.viewModel.EmailVisibility = false;
			}
			bool flag5 = type == KeyboardType.Network;
			if (flag5)
			{
				this.viewModel.NetworkVisibility = true;
			}
			else
			{
				this.viewModel.NetworkVisibility = false;
			}
			this.HasEnterButton = false;
			base.DataContext = this.viewModel;
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00122400 File Offset: 0x00120600
		public Keyboard(string name, string[] passwords, bool testPassword) : this(name, "", KeyboardType.Password)
		{
			this.viewModel.Passwords = passwords;
			this.viewModel.TestPassword = testPassword;
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x0012242C File Offset: 0x0012062C
		protected virtual void OnTextInputKeyboard(string text)
		{
			EventHandler<KeyboardInputEventArgs> textInputKeyboard = this.TextInputKeyboard;
			bool flag = textInputKeyboard != null;
			if (flag)
			{
				textInputKeyboard(this, new KeyboardInputEventArgs(text));
			}
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000F8D1B File Offset: 0x000F6F1B
		private void Window_Click(object sender, RoutedEventArgs e)
		{
			MainWindowViewModel.accessRightsManager.SignalActivity();
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x0012245C File Offset: 0x0012065C
		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MainWindowViewModel.accessRightsManager.SignalActivity();
			bool flag = this.viewModel.Type != KeyboardType.Password;
			if (flag)
			{
				this.inputBox.Focus();
			}
			else
			{
				this.passwordBox.Focus();
			}
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x001224A4 File Offset: 0x001206A4
		private void txtInput_Pasting(object sender, DataObjectPastingEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			object data = e.DataObject.GetData(typeof(string));
			string text = data as string;
			bool flag = textBox != null && text != null;
			if (flag)
			{
				bool flag2 = textBox.Text.Length < KeyboardViewModel.maxTextLength;
				if (flag2)
				{
					int num = KeyboardViewModel.maxTextLength - textBox.Text.Length;
					bool flag3 = text.Length > num;
					if (flag3)
					{
						e.CancelCommand();
					}
				}
				else
				{
					e.CancelCommand();
				}
			}
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x00122535 File Offset: 0x00120735
		public void SetKeyboardUppercase(bool upper)
		{
			this.viewModel.ToggleCase(null);
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x00122545 File Offset: 0x00120745
		private void viewModel_Committed(string text)
		{
			this.OnTextInputKeyboard(text);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x00122550 File Offset: 0x00120750
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			compositionTarget.RenderMode = RenderMode.SoftwareOnly;
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x0012257C File Offset: 0x0012077C
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/keyboard/keyboard.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x001225B4 File Offset: 0x001207B4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x001225D0 File Offset: 0x001207D0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.windowKeyboard = (Keyboard)target;
				this.windowKeyboard.Loaded += this.Window_Loaded;
				this.windowKeyboard.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.Window_Click));
				this.windowKeyboard.MouseUp += this.Window_MouseUp;
				break;
			case 2:
				this.mainGrid = (Grid)target;
				break;
			case 3:
				this.mainPanel = (DockPanel)target;
				break;
			case 4:
				this.displayPanel = (Grid)target;
				break;
			case 5:
				this.inputBox = (SelectionBindingTextBox)target;
				this.inputBox.AddHandler(DataObject.PastingEvent, new DataObjectPastingEventHandler(this.txtInput_Pasting));
				break;
			case 6:
				this.passwordBox = (PasswordBox)target;
				break;
			case 7:
				this.cancelPanel = (StackPanel)target;
				break;
			case 8:
				this.cancelPanel2 = (StackPanel)target;
				break;
			default:
				this._contentLoaded = true;
				break;
			}
		}

		// Token: 0x04001B8B RID: 7051
		private KeyboardViewModel viewModel;

		// Token: 0x04001B8C RID: 7052
		internal Keyboard windowKeyboard;

		// Token: 0x04001B8D RID: 7053
		internal Grid mainGrid;

		// Token: 0x04001B8E RID: 7054
		internal DockPanel mainPanel;

		// Token: 0x04001B8F RID: 7055
		internal Grid displayPanel;

		// Token: 0x04001B90 RID: 7056
		internal SelectionBindingTextBox inputBox;

		// Token: 0x04001B91 RID: 7057
		internal PasswordBox passwordBox;

		// Token: 0x04001B92 RID: 7058
		internal StackPanel cancelPanel;

		// Token: 0x04001B93 RID: 7059
		internal StackPanel cancelPanel2;

		// Token: 0x04001B94 RID: 7060
		private bool _contentLoaded;
	}
}

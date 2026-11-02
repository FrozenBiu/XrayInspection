using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000247 RID: 583
	public class NumKeyboard : Window, IComponentConnector, IStyleConnector
	{
		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06002C32 RID: 11314 RVA: 0x001233E5 File Offset: 0x001215E5
		// (set) Token: 0x06002C33 RID: 11315 RVA: 0x001233ED File Offset: 0x001215ED
		public bool ShouldClose { get; set; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06002C34 RID: 11316 RVA: 0x001233F6 File Offset: 0x001215F6
		// (set) Token: 0x06002C35 RID: 11317 RVA: 0x001233FE File Offset: 0x001215FE
		public Dock WindowPopupType { get; set; }

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06002C36 RID: 11318 RVA: 0x00123407 File Offset: 0x00121607
		// (set) Token: 0x06002C37 RID: 11319 RVA: 0x0012340F File Offset: 0x0012160F
		public WindowState WState { get; set; }

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06002C38 RID: 11320 RVA: 0x00123418 File Offset: 0x00121618
		// (set) Token: 0x06002C39 RID: 11321 RVA: 0x0003C95F File Offset: 0x0003AB5F
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

		// Token: 0x17000BF9 RID: 3065
		// (get) Token: 0x06002C3A RID: 11322 RVA: 0x00123430 File Offset: 0x00121630
		// (set) Token: 0x06002C3B RID: 11323 RVA: 0x0003C97F File Offset: 0x0003AB7F
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

		// Token: 0x17000BFA RID: 3066
		// (get) Token: 0x06002C3C RID: 11324 RVA: 0x00123448 File Offset: 0x00121648
		// (set) Token: 0x06002C3D RID: 11325 RVA: 0x0003C99F File Offset: 0x0003AB9F
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

		// Token: 0x17000BFB RID: 3067
		// (get) Token: 0x06002C3E RID: 11326 RVA: 0x00123460 File Offset: 0x00121660
		// (set) Token: 0x06002C3F RID: 11327 RVA: 0x0003C9BF File Offset: 0x0003ABBF
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

		// Token: 0x06002C40 RID: 11328 RVA: 0x00123477 File Offset: 0x00121677
		public NumKeyboard(string name, string oldValue, NumKeyboardType type)
		{
			this.InitWindow(name, oldValue, type);
			this.dockPanel.LastChildFill = true;
			MainWindowViewModel.accessRightsManager.UserAutoLogout += this.accessRightsManager_UserAutoLogout;
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x001234B0 File Offset: 0x001216B0
		public NumKeyboard(string name, string oldValue, NumKeyboardType type, Dock dock)
		{
			this.InitWindow(name, oldValue, type);
			this.WindowPopupType = dock;
			this.dockPanel.LastChildFill = false;
			this.mainPanel.Height = 420.0;
			this.mainPanel.Width = 420.0;
			this.InitWindow(name, oldValue, type);
			base.Background = Brushes.Transparent;
			MainWindowViewModel.accessRightsManager.UserAutoLogout += this.accessRightsManager_UserAutoLogout;
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x00075F4E File Offset: 0x0007414E
		private void accessRightsManager_UserAutoLogout()
		{
			base.Close();
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x0012353C File Offset: 0x0012173C
		private void InitWindow(string name, string oldValue, NumKeyboardType type)
		{
			this.InitializeComponent();
			this.type = type;
			this.WState = ((Definition.windowHeight > 0.0 && Definition.windowWidth > 0.0) ? WindowState.Normal : WindowState.Maximized);
			this.ShouldClose = true;
			this.oldBox.Text = oldValue;
			bool flag = type != NumKeyboardType.TypeIPAddress && type != NumKeyboardType.TypeSubnetMask && type != NumKeyboardType.TypeDNSServer;
			if (flag)
			{
				this.inputBox.Text = oldValue;
				this.inputBox.Focus();
				this.inputBox.SelectAll();
			}
			else
			{
				this.inputBox.Visibility = Visibility.Collapsed;
				this.oldBox.Visibility = Visibility.Collapsed;
				this.ipAddressBoxNew.Visibility = Visibility.Visible;
				this.ipAddressBoxOld.Visibility = Visibility.Visible;
				try
				{
					bool flag2 = oldValue != null;
					if (flag2)
					{
						this.ipAddressBoxOld.IPAddress = IPAddress.Parse(oldValue);
						this.ipAddressBoxNew.IPAddress = IPAddress.Parse(oldValue);
					}
					this.ipAddressBoxNew.Focus();
					bool flag3 = type == NumKeyboardType.TypeSubnetMask;
					if (flag3)
					{
						this.ipAddressBoxOld.IsSubnetMask = true;
						this.ipAddressBoxNew.IsSubnetMask = true;
					}
				}
				catch
				{
				}
			}
			this.tbDescription.Text = name;
			this.mainPanel.DataContext = this;
			bool flag4 = type == NumKeyboardType.TypeBool;
			if (flag4)
			{
				this.num2.IsEnabled = false;
				this.num2.Opacity = Definition.disabledButtonOpacity;
				this.num3.IsEnabled = false;
				this.num3.Opacity = Definition.disabledButtonOpacity;
				this.num4.IsEnabled = false;
				this.num4.Opacity = Definition.disabledButtonOpacity;
				this.num5.IsEnabled = false;
				this.num5.Opacity = Definition.disabledButtonOpacity;
				this.num6.IsEnabled = false;
				this.num6.Opacity = Definition.disabledButtonOpacity;
				this.num7.IsEnabled = false;
				this.num7.Opacity = Definition.disabledButtonOpacity;
				this.num8.IsEnabled = false;
				this.num8.Opacity = Definition.disabledButtonOpacity;
				this.num9.IsEnabled = false;
				this.num9.Opacity = Definition.disabledButtonOpacity;
				this.commaOrPlusMinus.IsEnabled = false;
				this.commaOrPlusMinus.Opacity = Definition.disabledButtonOpacity;
			}
			bool flag5 = type == NumKeyboardType.TypeUInt || type == NumKeyboardType.TypeIPAddress || type == NumKeyboardType.TypeSubnetMask || type == NumKeyboardType.TypeDNSServer;
			if (flag5)
			{
				this.commaOrPlusMinus.IsEnabled = false;
				this.commaOrPlusMinus.Opacity = Definition.disabledButtonOpacity;
			}
			bool flag6 = type == NumKeyboardType.TypeInt;
			if (flag6)
			{
				this.commaOrPlusMinus.Content = ImageConverter.ToImage("..\\img\\plusMinus.png");
				((Image)this.commaOrPlusMinus.Content).Stretch = Stretch.Uniform;
				((Image)this.commaOrPlusMinus.Content).Height = 30.0;
				this.commaOrPlusMinus.Click += this.commaOrPlusMinus_Click;
			}
			base.DataContext = this;
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x00123870 File Offset: 0x00121A70
		private void commaOrPlusMinus_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.inputBox.Text.Length > 0;
			if (flag)
			{
				bool flag2 = this.inputBox.Text[0] == '-';
				if (flag2)
				{
					this.inputBox.Text = this.inputBox.Text.Remove(0, 1);
				}
				else
				{
					this.inputBox.Text = "-" + this.inputBox.Text;
				}
				this.inputBox.SelectionStart = this.inputBox.Text.Length;
			}
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x00123910 File Offset: 0x00121B10
		private void CharBtn_Click(object sender, RoutedEventArgs e)
		{
			string text = ((Button)sender).Content.ToString();
			char[] array = text.ToCharArray();
			this.AddToDisplay(array[0]);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x00123940 File Offset: 0x00121B40
		private void OnWindowKeyDown(object sender, TextCompositionEventArgs e)
		{
			string text = e.Text;
			string pattern = "[\\d\\,\\b]";
			Regex regex = new Regex(pattern);
			bool flag = regex.IsMatch(text);
			if (flag)
			{
				char c = text.ToCharArray()[0];
				this.AddToDisplay(c);
			}
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void TextBoxGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
		}

		// Token: 0x140000FB RID: 251
		// (add) Token: 0x06002C48 RID: 11336 RVA: 0x00123984 File Offset: 0x00121B84
		// (remove) Token: 0x06002C49 RID: 11337 RVA: 0x001239BC File Offset: 0x00121BBC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler<KeyboardInputEventArgs> TextInputKeyboard;

		// Token: 0x06002C4A RID: 11338 RVA: 0x001239F4 File Offset: 0x00121BF4
		protected virtual void OnKeyboardInputEventArgs(string text)
		{
			EventHandler<KeyboardInputEventArgs> textInputKeyboard = this.TextInputKeyboard;
			bool flag = textInputKeyboard != null;
			if (flag)
			{
				textInputKeyboard(this, new KeyboardInputEventArgs(text));
			}
		}

		// Token: 0x06002C4B RID: 11339 RVA: 0x00123A24 File Offset: 0x00121C24
		private void SpecialBtn_Click(object sender, RoutedEventArgs e)
		{
			string text = ((Control)sender).Name.ToString();
			string a = text;
			if (a == "delete")
			{
				bool flag = this.type != NumKeyboardType.TypeIPAddress && this.type != NumKeyboardType.TypeSubnetMask && this.type != NumKeyboardType.TypeDNSServer;
				if (flag)
				{
					EditingCommands.Backspace.Execute(0, this.inputBox);
				}
				else
				{
					this.ipAddressBoxNew.DeleteNumber();
				}
			}
			e.Handled = true;
		}

		// Token: 0x06002C4C RID: 11340 RVA: 0x00123AA8 File Offset: 0x00121CA8
		private void AddToDisplay(char c)
		{
			bool flag = c == '\b';
			if (flag)
			{
				bool flag2 = !string.IsNullOrEmpty(this.inputBox.Text);
				if (flag2)
				{
					this.inputBox.Text = this.inputBox.Text.Substring(0, this.inputBox.Text.Length - 1);
				}
			}
			else
			{
				bool flag3 = this.type == NumKeyboardType.TypeBool;
				if (flag3)
				{
					this.inputBox.Text = c.ToString();
				}
				else
				{
					bool flag4 = this.inputBox.Text.Contains(',') && c == ',';
					if (!flag4)
					{
						bool flag5 = this.inputBox.SelectionLength != 0;
						if (flag5)
						{
							EditingCommands.Backspace.Execute(0, this.inputBox);
						}
						int selectionStart = this.inputBox.SelectionStart;
						bool flag6 = this.type == NumKeyboardType.TypeUInt;
						if (flag6)
						{
							string text = this.inputBox.Text;
							long num = Convert.ToInt64(text.Insert(selectionStart, c.ToString()));
							int maxValue = int.MaxValue;
							bool flag7 = num <= (long)maxValue;
							if (flag7)
							{
								this.inputBox.Text = this.inputBox.Text.Insert(selectionStart, c.ToString());
								this.inputBox.SelectionStart = selectionStart + 1;
							}
						}
						else
						{
							bool flag8 = this.type == NumKeyboardType.TypeIPAddress || this.type == NumKeyboardType.TypeSubnetMask || this.type == NumKeyboardType.TypeDNSServer;
							if (flag8)
							{
								int number = (int)(c - '0');
								this.ipAddressBoxNew.InsertNumber(number);
							}
							else
							{
								bool flag9 = this.inputBox.Text.IndexOf('-') != -1 && selectionStart <= this.inputBox.Text.IndexOf('-');
								if (!flag9)
								{
									this.inputBox.Text = this.inputBox.Text.Insert(selectionStart, c.ToString());
									this.inputBox.SelectionStart = selectionStart + 1;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002C4D RID: 11341 RVA: 0x00123CC8 File Offset: 0x00121EC8
		private void ok_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.type != NumKeyboardType.TypeIPAddress && this.type != NumKeyboardType.TypeSubnetMask && this.type != NumKeyboardType.TypeDNSServer;
			if (flag)
			{
				this.OnKeyboardInputEventArgs(this.inputBox.Text);
			}
			else
			{
				bool flag2 = this.ipAddressBoxNew.IPAddress != null;
				if (flag2)
				{
					this.OnKeyboardInputEventArgs(this.ipAddressBoxNew.IPAddress.ToString());
				}
				else
				{
					bool flag3 = this.type != NumKeyboardType.TypeDNSServer;
					if (flag3)
					{
						Dialog.Show(LocalizationManager.Localize("txtError"), LocalizationManager.Localize("txtSettingsInvalid") + ".", DialogStyle.Error, DialogButtons.OK);
						return;
					}
					this.OnKeyboardInputEventArgs("");
				}
			}
			bool shouldClose = this.ShouldClose;
			if (shouldClose)
			{
				base.Close();
			}
			else
			{
				this.ShouldClose = true;
			}
		}

		// Token: 0x06002C4E RID: 11342 RVA: 0x00075F4E File Offset: 0x0007414E
		private void cancel_Click(object sender, RoutedEventArgs e)
		{
			base.Close();
		}

		// Token: 0x06002C4F RID: 11343 RVA: 0x00123DA0 File Offset: 0x00121FA0
		private void NumBtn_Click(object sender, RoutedEventArgs e)
		{
			bool flag = this.type == NumKeyboardType.TypeInt && ((Button)sender).Name == "commaOrPlusMinus";
			if (!flag)
			{
				string text = ((Button)sender).Content.ToString();
				char[] array = text.ToCharArray();
				this.AddToDisplay(array[0]);
			}
		}

		// Token: 0x06002C50 RID: 11344 RVA: 0x00123DF8 File Offset: 0x00121FF8
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;
			HwndTarget compositionTarget = hwndSource.CompositionTarget;
			compositionTarget.RenderMode = RenderMode.SoftwareOnly;
		}

		// Token: 0x06002C51 RID: 11345 RVA: 0x00075F4E File Offset: 0x0007414E
		private void FormFadeOut_Completed(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06002C52 RID: 11346 RVA: 0x00123E24 File Offset: 0x00122024
		public static NumKeyboardType ConvertType(string type)
		{
			bool flag = type == "Bool";
			NumKeyboardType result;
			if (flag)
			{
				result = NumKeyboardType.TypeBool;
			}
			else
			{
				bool flag2 = type == "UInt";
				if (flag2)
				{
					result = NumKeyboardType.TypeUInt;
				}
				else
				{
					bool flag3 = type == "Double";
					if (flag3)
					{
						result = NumKeyboardType.TypeDouble;
					}
					else
					{
						bool flag4 = type == "Int";
						if (flag4)
						{
							result = NumKeyboardType.TypeInt;
						}
						else
						{
							result = NumKeyboardType.Default;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06002C53 RID: 11347 RVA: 0x000F8D1B File Offset: 0x000F6F1B
		private void Window_Click(object sender, RoutedEventArgs e)
		{
			MainWindowViewModel.accessRightsManager.SignalActivity();
		}

		// Token: 0x06002C54 RID: 11348 RVA: 0x000F8D1B File Offset: 0x000F6F1B
		private void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MainWindowViewModel.accessRightsManager.SignalActivity();
		}

		// Token: 0x06002C55 RID: 11349 RVA: 0x00123E85 File Offset: 0x00122085
		private void windowNumKeyboard_Unloaded(object sender, RoutedEventArgs e)
		{
			MainWindowViewModel.accessRightsManager.UserAutoLogout -= this.accessRightsManager_UserAutoLogout;
		}

		// Token: 0x06002C56 RID: 11350 RVA: 0x0001092C File Offset: 0x0000EB2C
		private void inputBox_SelectionChanged(object sender, RoutedEventArgs e)
		{
		}

		// Token: 0x06002C57 RID: 11351 RVA: 0x00123EA0 File Offset: 0x001220A0
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/keyboard/numkeyboard.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x06002C58 RID: 11352 RVA: 0x00123ED8 File Offset: 0x001220D8
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			return Delegate.CreateDelegate(delegateType, this, handler);
		}

		// Token: 0x06002C59 RID: 11353 RVA: 0x00123EF4 File Offset: 0x001220F4
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.windowNumKeyboard = (NumKeyboard)target;
				this.windowNumKeyboard.TextInput += this.OnWindowKeyDown;
				this.windowNumKeyboard.Loaded += this.Window_Loaded;
				this.windowNumKeyboard.Unloaded += this.windowNumKeyboard_Unloaded;
				this.windowNumKeyboard.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(this.Window_Click));
				this.windowNumKeyboard.MouseUp += this.Window_MouseUp;
				return;
			case 4:
				this.mainGrid = (Grid)target;
				return;
			case 5:
				this.dockPanel = (DockPanel)target;
				return;
			case 6:
				this.mainPanel = (Grid)target;
				return;
			case 7:
				this.displayPanel = (Grid)target;
				return;
			case 8:
				this.tbDescription = (TextBlock)target;
				return;
			case 9:
				this.oldBox = (TextBox)target;
				this.oldBox.PreviewGotKeyboardFocus += this.TextBoxGotKeyboardFocus;
				return;
			case 10:
				this.inputBox = (TextBox)target;
				this.inputBox.PreviewGotKeyboardFocus += this.TextBoxGotKeyboardFocus;
				this.inputBox.SelectionChanged += this.inputBox_SelectionChanged;
				return;
			case 11:
				this.ipAddressBoxOld = (IPAddressBox)target;
				return;
			case 12:
				this.ipAddressBoxNew = (IPAddressBox)target;
				return;
			case 13:
				this.keyboardPanel = (StackPanel)target;
				return;
			case 14:
				this.num1 = (Button)target;
				return;
			case 15:
				this.num2 = (Button)target;
				return;
			case 16:
				this.num3 = (Button)target;
				return;
			case 17:
				this.num4 = (Button)target;
				return;
			case 18:
				this.num5 = (Button)target;
				return;
			case 19:
				this.num6 = (Button)target;
				return;
			case 20:
				this.num7 = (Button)target;
				return;
			case 21:
				this.num8 = (Button)target;
				return;
			case 22:
				this.num9 = (Button)target;
				return;
			case 23:
				this.ok = (Button)target;
				this.ok.Click += this.ok_Click;
				return;
			case 24:
				this.commaOrPlusMinus = (Button)target;
				return;
			case 25:
				this.num0 = (Button)target;
				return;
			case 26:
				this.delete = (RepeatButton)target;
				this.delete.Click += this.SpecialBtn_Click;
				return;
			case 27:
				this.cancel = (Button)target;
				this.cancel.Click += this.cancel_Click;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x06002C5A RID: 11354 RVA: 0x00124230 File Offset: 0x00122430
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId != 2)
			{
				if (connectionId == 3)
				{
					EventSetter eventSetter = new EventSetter();
					eventSetter.Event = ButtonBase.ClickEvent;
					eventSetter.Handler = new RoutedEventHandler(this.NumBtn_Click);
					((Style)target).Setters.Add(eventSetter);
				}
			}
			else
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = ButtonBase.ClickEvent;
				eventSetter.Handler = new RoutedEventHandler(this.NumBtn_Click);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04001BA8 RID: 7080
		private NumKeyboardType type;

		// Token: 0x04001BAD RID: 7085
		internal NumKeyboard windowNumKeyboard;

		// Token: 0x04001BAE RID: 7086
		internal Grid mainGrid;

		// Token: 0x04001BAF RID: 7087
		internal DockPanel dockPanel;

		// Token: 0x04001BB0 RID: 7088
		internal Grid mainPanel;

		// Token: 0x04001BB1 RID: 7089
		internal Grid displayPanel;

		// Token: 0x04001BB2 RID: 7090
		internal TextBlock tbDescription;

		// Token: 0x04001BB3 RID: 7091
		internal TextBox oldBox;

		// Token: 0x04001BB4 RID: 7092
		internal TextBox inputBox;

		// Token: 0x04001BB5 RID: 7093
		internal IPAddressBox ipAddressBoxOld;

		// Token: 0x04001BB6 RID: 7094
		internal IPAddressBox ipAddressBoxNew;

		// Token: 0x04001BB7 RID: 7095
		internal StackPanel keyboardPanel;

		// Token: 0x04001BB8 RID: 7096
		internal Button num1;

		// Token: 0x04001BB9 RID: 7097
		internal Button num2;

		// Token: 0x04001BBA RID: 7098
		internal Button num3;

		// Token: 0x04001BBB RID: 7099
		internal Button num4;

		// Token: 0x04001BBC RID: 7100
		internal Button num5;

		// Token: 0x04001BBD RID: 7101
		internal Button num6;

		// Token: 0x04001BBE RID: 7102
		internal Button num7;

		// Token: 0x04001BBF RID: 7103
		internal Button num8;

		// Token: 0x04001BC0 RID: 7104
		internal Button num9;

		// Token: 0x04001BC1 RID: 7105
		internal Button ok;

		// Token: 0x04001BC2 RID: 7106
		internal Button commaOrPlusMinus;

		// Token: 0x04001BC3 RID: 7107
		internal Button num0;

		// Token: 0x04001BC4 RID: 7108
		internal RepeatButton delete;

		// Token: 0x04001BC5 RID: 7109
		internal Button cancel;

		// Token: 0x04001BC6 RID: 7110
		private bool _contentLoaded;
	}
}

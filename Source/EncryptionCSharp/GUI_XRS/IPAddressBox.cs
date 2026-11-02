using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x020000FA RID: 250
	public class IPAddressBox : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x1700072D RID: 1837
		// (get) Token: 0x060017F4 RID: 6132 RVA: 0x00090858 File Offset: 0x0008EA58
		// (set) Token: 0x060017F5 RID: 6133 RVA: 0x0009087A File Offset: 0x0008EA7A
		public IPAddress IPAddress
		{
			get
			{
				return (IPAddress)base.GetValue(IPAddressBox.IPAddressProperty);
			}
			set
			{
				base.SetValue(IPAddressBox.IPAddressProperty, value);
			}
		}

		// Token: 0x1700072E RID: 1838
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x0009088C File Offset: 0x0008EA8C
		// (set) Token: 0x060017F7 RID: 6135 RVA: 0x000908AE File Offset: 0x0008EAAE
		public bool IsSubnetMask
		{
			get
			{
				return (bool)base.GetValue(IPAddressBox.IsSubnetMaskProperty);
			}
			set
			{
				base.SetValue(IPAddressBox.IsSubnetMaskProperty, value);
			}
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x0009092F File Offset: 0x0008EB2F
		public IPAddressBox()
		{
			this.InitializeComponent();
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00090948 File Offset: 0x0008EB48
		private static void IPChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			bool flag = e.NewValue is IPAddress && d is IPAddressBox;
			if (flag)
			{
				IPAddress ipaddress = (IPAddress)e.NewValue;
				string text = ipaddress.ToString();
				IPAddressBox ipaddressBox = (IPAddressBox)d;
				string[] array = text.Split(new char[]
				{
					'.'
				});
				bool flag2 = array.Length == 4;
				if (flag2)
				{
					ipaddressBox.txtIP1.Text = array[0];
					ipaddressBox.txtIP2.Text = array[1];
					ipaddressBox.txtIP3.Text = array[2];
					ipaddressBox.txtIP4.Text = array[3];
				}
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x000909F4 File Offset: 0x0008EBF4
		private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			bool flag = sender is TextBox;
			if (flag)
			{
				TextBox textBox = (TextBox)sender;
				bool flag2 = e.Key == Key.Space;
				if (flag2)
				{
					e.Handled = true;
				}
				else
				{
					bool flag3 = e.Key == Key.Left || e.Key == Key.Right;
					if (flag3)
					{
						e.Handled = this.Navigate(textBox, e.Key);
					}
					else
					{
						bool flag4 = e.Key == Key.Back && textBox.SelectionLength == 0 && textBox.CaretIndex == 0;
						if (flag4)
						{
							this.FocusPreviousBox(textBox);
						}
					}
				}
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00090A8C File Offset: 0x0008EC8C
		private void TextBox_KeyDown(object sender, KeyEventArgs e)
		{
			bool flag = sender is TextBox;
			if (flag)
			{
				TextBox textBox = (TextBox)sender;
				int keyValue = this.GetKeyValue(e.Key);
				bool flag2 = KeyChecker.IsKeyADigit(e.Key) && keyValue >= 0;
				if (flag2)
				{
					int newIPSubNumber = this.GetNewIPSubNumber(textBox, keyValue);
					bool flag3 = newIPSubNumber == -1;
					if (flag3)
					{
						e.Handled = true;
					}
					bool flag4 = textBox.Text.Length == 2 && textBox.CaretIndex == 2 && newIPSubNumber >= 100;
					if (flag4)
					{
						this.nextBoxFocusNeeded = true;
					}
				}
				else
				{
					e.Handled = true;
				}
			}
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00090B38 File Offset: 0x0008ED38
		private void AppendIfNotSelected(StringBuilder strNumber, int index, TextBox txtBox)
		{
			bool flag = !this.IsIndexSelected(index, txtBox);
			if (flag)
			{
				strNumber.Append(txtBox.Text[index]);
			}
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00090B68 File Offset: 0x0008ED68
		private bool IsIndexSelected(int index, TextBox txtBox)
		{
			return index >= txtBox.SelectionStart && index < txtBox.SelectionLength + txtBox.SelectionStart;
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00090B98 File Offset: 0x0008ED98
		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			bool flag = sender is TextBox;
			if (flag)
			{
				IPAddress ipaddress = null;
				char[] trimChars = new char[]
				{
					'0'
				};
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append((this.txtIP1.Text == "0") ? this.txtIP1.Text : this.txtIP1.Text.TrimStart(trimChars));
				stringBuilder.Append('.');
				stringBuilder.Append((this.txtIP2.Text == "0") ? this.txtIP2.Text : this.txtIP2.Text.TrimStart(trimChars));
				stringBuilder.Append('.');
				stringBuilder.Append((this.txtIP3.Text == "0") ? this.txtIP3.Text : this.txtIP3.Text.TrimStart(trimChars));
				stringBuilder.Append('.');
				stringBuilder.Append((this.txtIP4.Text == "0") ? this.txtIP4.Text : this.txtIP4.Text.TrimStart(trimChars));
				bool flag2 = IPAddress.TryParse(stringBuilder.ToString(), out ipaddress);
				if (flag2)
				{
					this.IPAddress = ipaddress;
				}
				else
				{
					this.IPAddress = null;
				}
				bool flag3 = this.nextBoxFocusNeeded;
				if (flag3)
				{
					this.nextBoxFocusNeeded = false;
					this.FocusNextBox((TextBox)sender);
				}
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00090D20 File Offset: 0x0008EF20
		private bool Navigate(TextBox txtBox, Key key)
		{
			bool result = false;
			bool flag = key == Key.Left && txtBox.CaretIndex == 0;
			if (flag)
			{
				result = this.FocusPreviousBox(txtBox);
			}
			else
			{
				bool flag2 = key == Key.Right && txtBox.CaretIndex == txtBox.Text.Length;
				if (flag2)
				{
					result = this.FocusNextBox(txtBox);
				}
			}
			return result;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x00090D80 File Offset: 0x0008EF80
		private int GetKeyValue(Key key)
		{
			int result = -1;
			bool flag = key >= Key.D0 && key <= Key.D9;
			if (flag)
			{
				result = key - Key.D0;
			}
			else
			{
				bool flag2 = key >= Key.NumPad0 && key <= Key.NumPad9;
				if (flag2)
				{
					result = key - Key.NumPad0;
				}
			}
			return result;
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00090DD4 File Offset: 0x0008EFD4
		private bool FocusNextBox(TextBox txtBox)
		{
			bool result = false;
			bool flag = txtBox == this.txtIP1;
			if (flag)
			{
				this.txtIP2.CaretIndex = 0;
				this.txtIP2.Focus();
				result = true;
			}
			bool flag2 = txtBox == this.txtIP2;
			if (flag2)
			{
				this.txtIP3.CaretIndex = 0;
				this.txtIP3.Focus();
				result = true;
			}
			bool flag3 = txtBox == this.txtIP3;
			if (flag3)
			{
				this.txtIP4.CaretIndex = 0;
				this.txtIP4.Focus();
				result = true;
			}
			return result;
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x00090E6C File Offset: 0x0008F06C
		private bool FocusPreviousBox(TextBox txtBox)
		{
			bool result = false;
			bool flag = txtBox == this.txtIP2;
			if (flag)
			{
				this.txtIP1.CaretIndex = this.txtIP1.Text.Length;
				this.txtIP1.Focus();
				result = true;
			}
			bool flag2 = txtBox == this.txtIP3;
			if (flag2)
			{
				this.txtIP2.CaretIndex = this.txtIP2.Text.Length;
				this.txtIP2.Focus();
				result = true;
			}
			bool flag3 = txtBox == this.txtIP4;
			if (flag3)
			{
				this.txtIP3.CaretIndex = this.txtIP3.Text.Length;
				this.txtIP3.Focus();
				result = true;
			}
			return result;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00090F30 File Offset: 0x0008F130
		public void InsertNumber(int number)
		{
			TextBox focusedBox = this.GetFocusedBox();
			bool flag = focusedBox != null;
			if (flag)
			{
				bool flag2 = number >= 0 && number < 10;
				if (flag2)
				{
					int newIPSubNumber = this.GetNewIPSubNumber(focusedBox, number);
					bool flag3 = newIPSubNumber != -1;
					if (flag3)
					{
						bool flag4 = focusedBox.Text.Length == 2 && focusedBox.CaretIndex == 2 && newIPSubNumber >= 100;
						if (flag4)
						{
							this.nextBoxFocusNeeded = true;
						}
						int caretIndex = focusedBox.CaretIndex;
						focusedBox.Text = newIPSubNumber.ToString();
						bool flag5 = caretIndex < 2 || (caretIndex == 2 && focusedBox == this.txtIP4);
						if (flag5)
						{
							focusedBox.CaretIndex = caretIndex + 1;
						}
					}
				}
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00090FF0 File Offset: 0x0008F1F0
		private int GetNewIPSubNumber(TextBox txtBox, int enteredNumber)
		{
			int num = -1;
			int i = 0;
			StringBuilder stringBuilder = new StringBuilder();
			while (i < txtBox.CaretIndex)
			{
				this.AppendIfNotSelected(stringBuilder, i, txtBox);
				i++;
			}
			stringBuilder.Append(enteredNumber);
			while (i < txtBox.Text.Length)
			{
				this.AppendIfNotSelected(stringBuilder, i, txtBox);
				i++;
			}
			int.TryParse(stringBuilder.ToString(), out num);
			bool flag = num > 255 || stringBuilder.Length > 3 || (txtBox == this.txtIP1 && !this.IsSubnetMask && num > 223);
			if (flag)
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x000910A4 File Offset: 0x0008F2A4
		public void DeleteNumber()
		{
			TextBox focusedBox = this.GetFocusedBox();
			bool flag = focusedBox != null;
			if (flag)
			{
				bool flag2 = focusedBox.SelectionLength == 0 && focusedBox.CaretIndex == 0;
				if (flag2)
				{
					this.FocusPreviousBox(focusedBox);
				}
				EditingCommands.Backspace.Execute(0, focusedBox);
			}
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000910F8 File Offset: 0x0008F2F8
		private TextBox GetFocusedBox()
		{
			TextBox result = null;
			bool isFocused = this.txtIP1.IsFocused;
			if (isFocused)
			{
				result = this.txtIP1;
			}
			else
			{
				bool isFocused2 = this.txtIP2.IsFocused;
				if (isFocused2)
				{
					result = this.txtIP2;
				}
				else
				{
					bool isFocused3 = this.txtIP3.IsFocused;
					if (isFocused3)
					{
						result = this.txtIP3;
					}
					else
					{
						bool isFocused4 = this.txtIP4.IsFocused;
						if (isFocused4)
						{
							result = this.txtIP4;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00091170 File Offset: 0x0008F370
		private void FocusCorrectBox()
		{
			bool flag = this.txtIP4.Text.Length > 0;
			if (flag)
			{
				int caretIndex = (this.txtIP4.Text != null) ? this.txtIP4.Text.Length : 0;
				this.txtIP4.Focus();
				this.txtIP4.CaretIndex = caretIndex;
			}
			else
			{
				this.txtIP1.Focus();
			}
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x000911E1 File Offset: 0x0008F3E1
		private void ipCtrl_Loaded(object sender, RoutedEventArgs e)
		{
			this.FocusCorrectBox();
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000911EC File Offset: 0x0008F3EC
		public void ClearText()
		{
			this.txtIP1.Text = "";
			this.txtIP2.Text = "";
			this.txtIP3.Text = "";
			this.txtIP4.Text = "";
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x00091240 File Offset: 0x0008F440
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ipconfiguration/ipaddressbox.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x00091278 File Offset: 0x0008F478
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IComponentConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 1:
				this.ipCtrl = (IPAddressBox)target;
				this.ipCtrl.Loaded += this.ipCtrl_Loaded;
				return;
			case 3:
				this.txtIP1 = (TextBox)target;
				return;
			case 4:
				this.txtIP2 = (TextBox)target;
				return;
			case 5:
				this.txtIP3 = (TextBox)target;
				return;
			case 6:
				this.txtIP4 = (TextBox)target;
				return;
			}
			this._contentLoaded = true;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x00091314 File Offset: 0x0008F514
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			if (connectionId == 2)
			{
				EventSetter eventSetter = new EventSetter();
				eventSetter.Event = UIElement.PreviewKeyDownEvent;
				eventSetter.Handler = new KeyEventHandler(this.TextBox_PreviewKeyDown);
				((Style)target).Setters.Add(eventSetter);
				eventSetter = new EventSetter();
				eventSetter.Event = UIElement.KeyDownEvent;
				eventSetter.Handler = new KeyEventHandler(this.TextBox_KeyDown);
				((Style)target).Setters.Add(eventSetter);
				eventSetter = new EventSetter();
				eventSetter.Event = TextBoxBase.TextChangedEvent;
				eventSetter.Handler = new TextChangedEventHandler(this.TextBox_TextChanged);
				((Style)target).Setters.Add(eventSetter);
			}
		}

		// Token: 0x04000FAB RID: 4011
		private bool nextBoxFocusNeeded = false;

		// Token: 0x04000FAC RID: 4012
		public static readonly DependencyProperty IPAddressProperty = DependencyProperty.Register("IPAddress", typeof(IPAddress), typeof(IPAddressBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(IPAddressBox.IPChanged)));

		// Token: 0x04000FAD RID: 4013
		public static readonly DependencyProperty IsSubnetMaskProperty = DependencyProperty.Register("IsSubnetMask", typeof(bool), typeof(IPAddressBox));

		// Token: 0x04000FAE RID: 4014
		internal IPAddressBox ipCtrl;

		// Token: 0x04000FAF RID: 4015
		internal TextBox txtIP1;

		// Token: 0x04000FB0 RID: 4016
		internal TextBox txtIP2;

		// Token: 0x04000FB1 RID: 4017
		internal TextBox txtIP3;

		// Token: 0x04000FB2 RID: 4018
		internal TextBox txtIP4;

		// Token: 0x04000FB3 RID: 4019
		private bool _contentLoaded;
	}
}

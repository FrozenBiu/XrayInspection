using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI_XRS
{
	// Token: 0x020000BF RID: 191
	public class SelectionBindingTextBox : TextBox
	{
		// Token: 0x06001345 RID: 4933 RVA: 0x0007102F File Offset: 0x0006F22F
		public SelectionBindingTextBox()
		{
			base.SelectionChanged += this.OnSelectionChanged;
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x0007104C File Offset: 0x0006F24C
		// (set) Token: 0x06001347 RID: 4935 RVA: 0x0007106E File Offset: 0x0006F26E
		public int BindableSelectionStart
		{
			get
			{
				return (int)base.GetValue(SelectionBindingTextBox.BindableSelectionStartProperty);
			}
			set
			{
				base.SetValue(SelectionBindingTextBox.BindableSelectionStartProperty, value);
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x00071084 File Offset: 0x0006F284
		// (set) Token: 0x06001349 RID: 4937 RVA: 0x000710A6 File Offset: 0x0006F2A6
		public int BindableSelectionLength
		{
			get
			{
				return (int)base.GetValue(SelectionBindingTextBox.BindableSelectionLengthProperty);
			}
			set
			{
				base.SetValue(SelectionBindingTextBox.BindableSelectionLengthProperty, value);
			}
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x000710BC File Offset: 0x0006F2BC
		private static void OnBindableSelectionStartChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			SelectionBindingTextBox selectionBindingTextBox = dependencyObject as SelectionBindingTextBox;
			bool flag = !selectionBindingTextBox.changeFromUI;
			if (flag)
			{
				int selectionStart = (int)args.NewValue;
				selectionBindingTextBox.SelectionStart = selectionStart;
			}
			else
			{
				selectionBindingTextBox.changeFromUI = false;
			}
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x00071100 File Offset: 0x0006F300
		private static void OnBindableSelectionLengthChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
		{
			SelectionBindingTextBox selectionBindingTextBox = dependencyObject as SelectionBindingTextBox;
			bool flag = !selectionBindingTextBox.changeFromUI;
			if (flag)
			{
				int selectionLength = (int)args.NewValue;
				selectionBindingTextBox.SelectionLength = selectionLength;
			}
			else
			{
				selectionBindingTextBox.changeFromUI = false;
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x00071144 File Offset: 0x0006F344
		private void OnSelectionChanged(object sender, RoutedEventArgs e)
		{
			bool flag = this.BindableSelectionStart != base.SelectionStart;
			if (flag)
			{
				this.changeFromUI = true;
				this.BindableSelectionStart = base.SelectionStart;
			}
			bool flag2 = this.BindableSelectionLength != base.SelectionLength;
			if (flag2)
			{
				this.changeFromUI = true;
				this.BindableSelectionLength = base.SelectionLength;
			}
		}

		// Token: 0x04000C96 RID: 3222
		public static readonly DependencyProperty BindableSelectionStartProperty = DependencyProperty.Register("BindableSelectionStart", typeof(int), typeof(SelectionBindingTextBox), new PropertyMetadata(new PropertyChangedCallback(SelectionBindingTextBox.OnBindableSelectionStartChanged)));

		// Token: 0x04000C97 RID: 3223
		public static readonly DependencyProperty BindableSelectionLengthProperty = DependencyProperty.Register("BindableSelectionLength", typeof(int), typeof(SelectionBindingTextBox), new PropertyMetadata(new PropertyChangedCallback(SelectionBindingTextBox.OnBindableSelectionLengthChanged)));

		// Token: 0x04000C98 RID: 3224
		private bool changeFromUI;
	}
}

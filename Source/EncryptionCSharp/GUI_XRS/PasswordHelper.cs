using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI_XRS
{
	// Token: 0x02000228 RID: 552
	public static class PasswordHelper
	{
		// Token: 0x0600295E RID: 10590 RVA: 0x00112359 File Offset: 0x00110559
		public static void SetAttach(DependencyObject dp, bool value)
		{
			dp.SetValue(PasswordHelper.AttachProperty, value);
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x00112370 File Offset: 0x00110570
		public static bool GetAttach(DependencyObject dp)
		{
			return (bool)dp.GetValue(PasswordHelper.AttachProperty);
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x00112394 File Offset: 0x00110594
		public static string GetPassword(DependencyObject dp)
		{
			return (string)dp.GetValue(PasswordHelper.PasswordProperty);
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x001123B6 File Offset: 0x001105B6
		public static void SetPassword(DependencyObject dp, string value)
		{
			dp.SetValue(PasswordHelper.PasswordProperty, value);
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x001123C8 File Offset: 0x001105C8
		private static bool GetIsUpdating(DependencyObject dp)
		{
			return (bool)dp.GetValue(PasswordHelper.IsUpdatingProperty);
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x001123EA File Offset: 0x001105EA
		private static void SetIsUpdating(DependencyObject dp, bool value)
		{
			dp.SetValue(PasswordHelper.IsUpdatingProperty, value);
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x00112400 File Offset: 0x00110600
		private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			passwordBox.PasswordChanged -= PasswordHelper.PasswordChanged;
			bool flag = !PasswordHelper.GetIsUpdating(passwordBox);
			if (flag)
			{
				passwordBox.Password = (string)e.NewValue;
			}
			passwordBox.PasswordChanged += PasswordHelper.PasswordChanged;
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x00112460 File Offset: 0x00110660
		private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			bool flag = passwordBox == null;
			if (!flag)
			{
				bool flag2 = (bool)e.OldValue;
				if (flag2)
				{
					passwordBox.PasswordChanged -= PasswordHelper.PasswordChanged;
				}
				bool flag3 = (bool)e.NewValue;
				if (flag3)
				{
					passwordBox.PasswordChanged += PasswordHelper.PasswordChanged;
				}
			}
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x001124CC File Offset: 0x001106CC
		private static void PasswordChanged(object sender, RoutedEventArgs e)
		{
			PasswordBox passwordBox = sender as PasswordBox;
			PasswordHelper.SetIsUpdating(passwordBox, true);
			PasswordHelper.SetPassword(passwordBox, passwordBox.Password);
			PasswordHelper.SetIsUpdating(passwordBox, false);
		}

		// Token: 0x040019B7 RID: 6583
		public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordHelper), new FrameworkPropertyMetadata(string.Empty, new PropertyChangedCallback(PasswordHelper.OnPasswordPropertyChanged)));

		// Token: 0x040019B8 RID: 6584
		public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordHelper), new PropertyMetadata(false, new PropertyChangedCallback(PasswordHelper.Attach)));

		// Token: 0x040019B9 RID: 6585
		private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordHelper));
	}
}

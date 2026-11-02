using System;
using System.Windows;
using System.Windows.Controls;

namespace GUI_XRS
{
	// Token: 0x02000145 RID: 325
	public class RadioButtonExtended : RadioButton
	{
		// Token: 0x06001EF1 RID: 7921 RVA: 0x000CE74D File Offset: 0x000CC94D
		public RadioButtonExtended()
		{
			base.Checked += this.RadioButtonExtended_Checked;
			base.Unchecked += this.RadioButtonExtended_Unchecked;
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x000CE780 File Offset: 0x000CC980
		private void RadioButtonExtended_Unchecked(object sender, RoutedEventArgs e)
		{
			bool flag = !RadioButtonExtended.m_bIsChanging;
			if (flag)
			{
				this.IsCheckedReal = new bool?(false);
			}
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x000CE7A8 File Offset: 0x000CC9A8
		private void RadioButtonExtended_Checked(object sender, RoutedEventArgs e)
		{
			bool flag = !RadioButtonExtended.m_bIsChanging;
			if (flag)
			{
				this.IsCheckedReal = new bool?(true);
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06001EF4 RID: 7924 RVA: 0x000CE7D0 File Offset: 0x000CC9D0
		// (set) Token: 0x06001EF5 RID: 7925 RVA: 0x000CE7F2 File Offset: 0x000CC9F2
		public bool? IsCheckedReal
		{
			get
			{
				return (bool?)base.GetValue(RadioButtonExtended.IsCheckedRealProperty);
			}
			set
			{
				base.SetValue(RadioButtonExtended.IsCheckedRealProperty, value);
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000CE807 File Offset: 0x000CCA07
		public static void IsCheckedRealChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			RadioButtonExtended.m_bIsChanging = true;
			((RadioButtonExtended)d).IsChecked = new bool?((bool)e.NewValue);
			RadioButtonExtended.m_bIsChanging = false;
		}

		// Token: 0x04001437 RID: 5175
		private static bool m_bIsChanging = false;

		// Token: 0x04001438 RID: 5176
		public static readonly DependencyProperty IsCheckedRealProperty = DependencyProperty.Register("IsCheckedReal", typeof(bool?), typeof(RadioButtonExtended), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, new PropertyChangedCallback(RadioButtonExtended.IsCheckedRealChanged)));
	}
}

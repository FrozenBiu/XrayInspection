using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Windows.Controls;

namespace GUI_XRS
{
	// Token: 0x02000140 RID: 320
	public static class DataGridAttachedProps
	{
		// Token: 0x06001EAD RID: 7853 RVA: 0x000C946C File Offset: 0x000C766C
		public static ControlTemplate GetSelectAllButtonTemplate(Microsoft.Windows.Controls.DataGrid obj)
		{
			return (ControlTemplate)obj.GetValue(DataGridAttachedProps.SelectAllButtonTemplateProperty);
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000C948E File Offset: 0x000C768E
		public static void SetSelectAllButtonTemplate(Microsoft.Windows.Controls.DataGrid obj, ControlTemplate value)
		{
			obj.SetValue(DataGridAttachedProps.SelectAllButtonTemplateProperty, value);
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000C94A0 File Offset: 0x000C76A0
		private static void OnSelectAllButtonTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Microsoft.Windows.Controls.DataGrid dataGrid = d as Microsoft.Windows.Controls.DataGrid;
			bool flag = dataGrid == null;
			if (!flag)
			{
				EventHandler handler = null;
				handler = delegate(object <p0>, EventArgs <p1>)
				{
					DependencyObject dependencyObject = dataGrid;
					while (dependencyObject != null && VisualTreeHelper.GetChildrenCount(dependencyObject) != 0 && (!(dependencyObject is Button) || ((Button)dependencyObject).Command != Microsoft.Windows.Controls.DataGrid.SelectAllCommand))
					{
						dependencyObject = VisualTreeHelper.GetChild(dependencyObject, 0);
					}
					Button button = dependencyObject as Button;
					bool flag2 = button != null;
					if (flag2)
					{
						ControlTemplate selectAllButtonTemplate = DataGridAttachedProps.GetSelectAllButtonTemplate(dataGrid);
						button.Template = selectAllButtonTemplate;
						dataGrid.LayoutUpdated -= handler;
					}
				};
				dataGrid.LayoutUpdated += handler;
			}
		}

		// Token: 0x040013FA RID: 5114
		public static readonly DependencyProperty SelectAllButtonTemplateProperty = DependencyProperty.RegisterAttached("SelectAllButtonTemplate", typeof(ControlTemplate), typeof(DataGridAttachedProps), new UIPropertyMetadata(null, new PropertyChangedCallback(DataGridAttachedProps.OnSelectAllButtonTemplateChanged)));
	}
}

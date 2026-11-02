using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using DatabaseConnection;
using Definitions;
using XrsImage;

namespace GUI_XRS
{
	// Token: 0x02000014 RID: 20
	public class CtrlAIObject : UserControl, IComponentConnector, IStyleConnector
	{
		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060001A0 RID: 416 RVA: 0x00009E84 File Offset: 0x00008084
		// (remove) Token: 0x060001A1 RID: 417 RVA: 0x00009EBC File Offset: 0x000080BC
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event EventHandler UpdateColourBtnSelectionMenu;

		// Token: 0x060001A2 RID: 418 RVA: 0x00009EF4 File Offset: 0x000080F4
		public CtrlAIObject(ParameterHandling parHandling, StoreImageData imageArchiver, ImageDataTransfer imageTransfer, ProductManager productManager, DBConnection dbConnection)
		{
			this.InitializeComponent();
			this.viewModel = new AIObjectViewModel(parHandling, imageArchiver, imageTransfer, productManager, dbConnection);
			base.DataContext = this.viewModel;
			this.dbConnection = dbConnection;
			this.parHandling = parHandling;
			this.productManager = productManager;
			this.InitializeButtons();
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00009F50 File Offset: 0x00008150
		private void InitializeButtons()
		{
			this.PanelButtons = new DockPanel();
			this.PanelButtons.HorizontalAlignment = HorizontalAlignment.Stretch;
			this.PanelButtons.VerticalAlignment = VerticalAlignment.Stretch;
			Button button = new Button();
			button.Content = "";
			button.Style = (base.FindResource("defaultButtonStyle") as Style);
			button.Visibility = Visibility.Hidden;
			button.Width = 40.0;
			this.PanelButtons.Children.Add(button);
			this.viewModel.btnTestSettings.Content = ImageConverter.ToImage("../img/play_bright_small.png");
			this.viewModel.btnTestSettings.Style = (base.FindResource("defaultButtonStyle") as Style);
			this.viewModel.btnTestSettings.Command = this.viewModel.TestSettingsCommand;
			this.viewModel.btnTestSettings.Margin = new Thickness(0.0, 2.0, 10.0, 2.0);
			this.viewModel.btnTestSettings.IsEnabled = true;
			this.PanelButtons.Children.Add(this.viewModel.btnTestSettings);
			DockPanel.SetDock(this.viewModel.btnTestSettings, Dock.Left);
			this.UiElementRightAligned = new Label
			{
				Content = "THiNK XO",
				FontSize = 30.0,
				FontWeight = FontWeights.Bold,
				FontFamily = new FontFamily("Verdana"),
				Foreground = Brushes.DarkGray,
				VerticalAlignment = VerticalAlignment.Center,
				HorizontalAlignment = HorizontalAlignment.Right,
				Margin = new Thickness(0.0, 2.0, 20.0, 2.0)
			};
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000A13A File Offset: 0x0000833A
		private void CtrlAIObject_Unloaded(object sender, RoutedEventArgs e)
		{
			this.viewModel.CleanUp();
			this.UpdateInferenceEnabled();
			EventHandler updateColourBtnSelectionMenu = this.UpdateColourBtnSelectionMenu;
			if (updateColourBtnSelectionMenu != null)
			{
				updateColourBtnSelectionMenu(this, EventArgs.Empty);
			}
			this.UpdateClassAvailability();
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000A170 File Offset: 0x00008370
		private void UpdateClassAvailability()
		{
			bool connectionEstablished = this.parHandling.ConnectionEstablished;
			if (connectionEstablished)
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				string tableName = null;
				this.dbConnection.GetCurrentProductTableNameAI(ref tableName);
				bool flag = TableOperations.TableExists(this.dbConnection.Connection, tableName);
				if (flag)
				{
					string pattern = "^" + Definition.objectDetectionClassesSubPath + "\\d+/ClassAvailable.*$";
					this.dbConnection.GetProductPaths(tableName, list2);
					for (int i = 0; i < list2.Count; i++)
					{
						bool flag2 = Regex.IsMatch(list2[i], pattern);
						if (flag2)
						{
							list.Add(this.parHandling.ReadPar(list2[i]));
						}
						else
						{
							list2.Remove(list2[i]);
							i--;
						}
					}
					bool flag3 = !this.dbConnection.UpdateTable(tableName, list2, list);
					if (flag3)
					{
						Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate()
						{
							Dialog.Show("Error", "AI product table couldn't be updated");
						}));
					}
				}
			}
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000A2A0 File Offset: 0x000084A0
		private void UpdateInferenceEnabled()
		{
			for (int i = 0; i < Definition.nAIObjectClasses; i++)
			{
				string path = string.Format("{0}{1}{2}", Definition.objectDetectionClassesSubPath, i + 1, Definition.objectDetectionClassEnabled);
				bool flag = this.parHandling.ReadPar(path) == "1";
				bool flag2 = flag;
				if (flag2)
				{
					return;
				}
			}
			bool flag3 = this.parHandling.ReadPar(Definition.objectDetectionInferenceEnabled) == "1";
			if (flag3)
			{
				this.parHandling.WriteValue(Definition.objectDetectionInferenceEnabled, "0");
				LogbookManager.AddEntry(SSTEntryCode.AIDetectionDeactivated, 3, this.productManager.CurrentLoadedProduct.ProductNumber, this.productManager.CurrentLoadedProduct.ProductName);
				Dialog.Show(LocalizationManager.Localize("txtWarning"), LocalizationManager.Localize("msgThinkXODisableInference"), DialogStyle.Warning, DialogButtons.OK);
				return;
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000A384 File Offset: 0x00008584
		private void ChangeGroupIdCommand(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeGroupId();
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000A3D0 File Offset: 0x000085D0
		private void ChangeMinCountCommand(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeMinCount();
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000A41C File Offset: 0x0000861C
		private void ChangeMaxCountCommand(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeMaxCount();
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000A468 File Offset: 0x00008668
		private void ChangeConfThresholdCommand(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeConfThreshold();
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000A4B4 File Offset: 0x000086B4
		private void ChangeFilterSizeCommand(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeFilterSize();
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000A500 File Offset: 0x00008700
		private void ChangeMinGroupSizeCommand(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeMinGroupSize();
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000A54C File Offset: 0x0000874C
		private void ChangeConfThresholdTTA1Command(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeConfThresholdTTA1();
			}
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000A598 File Offset: 0x00008798
		private void ChangeConfThresholdTTA2Command(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeConfThresholdTTA2();
			}
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000A5E4 File Offset: 0x000087E4
		private void ChangeConfThresholdTTA3Command(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeConfThresholdTTA3();
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000A630 File Offset: 0x00008830
		private void ChangeLowThresholdMinMax(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeLowThresholdMinMax();
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000A67C File Offset: 0x0000887C
		private void ChangeHighThresholdMinMax(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangeHighThresholdMinMax();
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000A6C8 File Offset: 0x000088C8
		private void ChangePercentileThreshold(object sender, MouseButtonEventArgs e)
		{
			bool flag = sender is TextBox && ((TextBox)sender).DataContext is AIObjectClass;
			if (flag)
			{
				AIObjectClass aiobjectClass = (AIObjectClass)((TextBox)sender).DataContext;
				aiobjectClass.ChangePercentileThreshold();
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000A714 File Offset: 0x00008914
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/ai/ctrlaiobject.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000A74C File Offset: 0x0000894C
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
				((CtrlAIObject)target).Unloaded += this.CtrlAIObject_Unloaded;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000A788 File Offset: 0x00008988
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IStyleConnector.Connect(int connectionId, object target)
		{
			switch (connectionId)
			{
			case 2:
				((TextBox)target).MouseUp += this.ChangeGroupIdCommand;
				break;
			case 3:
				((TextBox)target).MouseUp += this.ChangeMinCountCommand;
				break;
			case 4:
				((TextBox)target).MouseUp += this.ChangeMaxCountCommand;
				break;
			case 5:
				((TextBox)target).MouseUp += this.ChangeConfThresholdCommand;
				break;
			case 6:
				((TextBox)target).MouseUp += this.ChangeFilterSizeCommand;
				break;
			case 7:
				((TextBox)target).MouseUp += this.ChangeMinGroupSizeCommand;
				break;
			case 8:
				((TextBox)target).MouseUp += this.ChangeLowThresholdMinMax;
				break;
			case 9:
				((TextBox)target).MouseUp += this.ChangeHighThresholdMinMax;
				break;
			case 10:
				((TextBox)target).MouseUp += this.ChangePercentileThreshold;
				break;
			case 11:
				((TextBox)target).MouseUp += this.ChangeConfThresholdTTA1Command;
				break;
			case 12:
				((TextBox)target).MouseUp += this.ChangeConfThresholdTTA2Command;
				break;
			case 13:
				((TextBox)target).MouseUp += this.ChangeConfThresholdTTA3Command;
				break;
			}
		}

		// Token: 0x0400018B RID: 395
		private AIObjectViewModel viewModel;

		// Token: 0x0400018C RID: 396
		private DBConnection dbConnection;

		// Token: 0x0400018D RID: 397
		private ParameterHandling parHandling;

		// Token: 0x0400018E RID: 398
		private ProductManager productManager;

		// Token: 0x0400018F RID: 399
		public DockPanel PanelButtons;

		// Token: 0x04000190 RID: 400
		public UIElement UiElementRightAligned;

		// Token: 0x04000192 RID: 402
		private bool _contentLoaded;
	}
}

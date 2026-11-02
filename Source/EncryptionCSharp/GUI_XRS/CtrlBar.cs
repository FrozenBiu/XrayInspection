using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace GUI_XRS
{
	// Token: 0x02000157 RID: 343
	public class CtrlBar : UserControl, IComponentConnector
	{
		// Token: 0x17000974 RID: 2420
		// (get) Token: 0x0600209F RID: 8351 RVA: 0x000D4C94 File Offset: 0x000D2E94
		// (set) Token: 0x060020A0 RID: 8352 RVA: 0x000D4CC4 File Offset: 0x000D2EC4
		public double CurrentValue
		{
			get
			{
				return (this.viewModel != null) ? this.viewModel.CurrentValue : 0.0;
			}
			set
			{
				bool flag = this.viewModel != null;
				if (flag)
				{
					this.viewModel.CurrentValue = value;
				}
			}
		}

		// Token: 0x17000975 RID: 2421
		// (set) Token: 0x060020A1 RID: 8353 RVA: 0x000D4CEC File Offset: 0x000D2EEC
		public double FirstUsedValue
		{
			set
			{
				bool flag = this.viewModel != null;
				if (flag)
				{
					this.viewModel.FirstUsedValue = value;
				}
			}
		}

		// Token: 0x17000976 RID: 2422
		// (set) Token: 0x060020A2 RID: 8354 RVA: 0x000D4D18 File Offset: 0x000D2F18
		public double LastUsedValue
		{
			set
			{
				bool flag = this.viewModel != null;
				if (flag)
				{
					this.viewModel.LastUsedValue = value;
					this.viewModel.MaxValue = value;
				}
			}
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000D4D50 File Offset: 0x000D2F50
		public CtrlBar()
		{
			this.InitializeComponent();
			this.viewModel = new BarViewModel(0.0, 240.0, 50.0, 240.0, 2.0, 10.0, 1.0);
			base.DataContext = this.viewModel;
			this.InitializeCanvas(0, 50);
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000D4DD4 File Offset: 0x000D2FD4
		private void InitializeCanvas(int offset, int numberSteps)
		{
			this.cnvNumbers.Children.Clear();
			for (int i = offset; i < this.viewModel.BarItems.Count; i += numberSteps)
			{
				TextBlock textBlock = new TextBlock();
				textBlock.Text = this.viewModel.BarItems[this.viewModel.BarItems.Count - 1 - i].Value.ToString("F" + this.floatingPointPrecision.ToString());
				textBlock.FontSize = 10.0;
				bool flag = (double)i == this.viewModel.FirstUsedValue || (double)i == this.viewModel.LastUsedValue;
				if (flag)
				{
					textBlock.FontWeight = FontWeights.Bold;
				}
				else
				{
					textBlock.FontWeight = FontWeights.Normal;
				}
				this.cnvNumbers.Children.Add(textBlock);
				Canvas.SetBottom(textBlock, (double)(i - 9));
				Canvas.SetRight(textBlock, 3.0);
			}
			bool flag2 = this.viewModel.FirstUsedValue % (double)numberSteps != 0.0;
			if (flag2)
			{
				int num = -1;
				for (int j = this.viewModel.BarItems.Count - 1; j >= 0; j--)
				{
					bool flag3 = this.viewModel.BarItems[j].Value == this.viewModel.FirstUsedValue;
					if (flag3)
					{
						num = j;
						break;
					}
				}
				bool flag4 = num != -1;
				if (flag4)
				{
					TextBlock textBlock2 = new TextBlock();
					textBlock2.Text = this.viewModel.FirstUsedValue.ToString("F" + this.floatingPointPrecision.ToString());
					textBlock2.FontSize = 10.0;
					textBlock2.FontWeight = FontWeights.Bold;
					this.cnvNumbers.Children.Add(textBlock2);
					Canvas.SetBottom(textBlock2, (double)(this.viewModel.BarItems.Count - num - 9));
					Canvas.SetRight(textBlock2, 3.0);
				}
			}
			bool flag5 = this.viewModel.LastUsedValue % (double)numberSteps != 0.0;
			if (flag5)
			{
				TextBlock textBlock3 = new TextBlock();
				textBlock3.Text = this.viewModel.LastUsedValue.ToString("F" + this.floatingPointPrecision.ToString());
				textBlock3.FontSize = 10.0;
				textBlock3.FontWeight = FontWeights.Bold;
				this.cnvNumbers.Children.Add(textBlock3);
				Canvas.SetBottom(textBlock3, (double)(this.viewModel.BarItems.Count - 9));
				Canvas.SetRight(textBlock3, 3.0);
			}
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x000D50DC File Offset: 0x000D32DC
		public void ReinitializeScale(double minUsedValue, double maxUsedValue, int floatingPointPrecision, double steps, int offset, int numberSteps)
		{
			this.floatingPointPrecision = floatingPointPrecision;
			this.viewModel = new BarViewModel(0.0, maxUsedValue, minUsedValue, maxUsedValue, 2.0, 10.0, steps);
			base.DataContext = this.viewModel;
			this.InitializeCanvas(offset, numberSteps);
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x000D5134 File Offset: 0x000D3334
		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			bool contentLoaded = this._contentLoaded;
			if (!contentLoaded)
			{
				this._contentLoaded = true;
				Uri resourceLocator = new Uri("/GUI_XRS;component/sideshooter/ctrlbar.xaml", UriKind.Relative);
				Application.LoadComponent(this, resourceLocator);
			}
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000D516C File Offset: 0x000D336C
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
				this.cnvNumbers = (Canvas)target;
			}
		}

		// Token: 0x0400152F RID: 5423
		private BarViewModel viewModel;

		// Token: 0x04001530 RID: 5424
		private int floatingPointPrecision = 0;

		// Token: 0x04001531 RID: 5425
		internal Canvas cnvNumbers;

		// Token: 0x04001532 RID: 5426
		private bool _contentLoaded;
	}
}

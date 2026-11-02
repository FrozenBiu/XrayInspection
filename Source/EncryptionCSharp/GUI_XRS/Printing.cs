using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Definitions;

namespace GUI_XRS
{
	// Token: 0x02000186 RID: 390
	public static class Printing
	{
		// Token: 0x060024DF RID: 9439 RVA: 0x000FD4F4 File Offset: 0x000FB6F4
		public static bool Print(UIElement element, string caption)
		{
			return Printing.Print(element, caption, null);
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x000FD510 File Offset: 0x000FB710
		public static bool Print(UIElement element, string caption, string printerName)
		{
			InlineUIContainer inline = new InlineUIContainer(element);
			Paragraph block = new Paragraph(inline);
			FlowDocument flowDocument = new FlowDocument(block);
			flowDocument.PagePadding = new Thickness(100.0);
			PrintDialog printDialog = new PrintDialog();
			bool flag = printerName != null;
			bool result;
			if (flag)
			{
				LocalPrintServer localPrintServer = new LocalPrintServer();
				bool flag2 = printerName == Definition.standardPrinterKey;
				if (flag2)
				{
					printDialog.PrintQueue = localPrintServer.DefaultPrintQueue;
				}
				else
				{
					PrintQueueCollection printQueues = localPrintServer.GetPrintQueues();
					List<PrintQueue> list = (from name in printQueues
					where name.FullName == printerName
					select name).ToList<PrintQueue>();
					bool flag3 = list.Count > 0;
					if (!flag3)
					{
						return false;
					}
					printDialog.PrintQueue = list[0];
				}
				printDialog.PrintDocument(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, caption);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}

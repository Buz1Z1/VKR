using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Excel=Microsoft.Office.Interop.Excel;
namespace restaur
{
    class Export_excel
    {
        public void ExportDatagrid(DataGridView dg, string filePath)
        {
            try
            {
                dg.EndEdit();
                Excel.Application excelApp = new Excel.Application();
                Excel.Workbook wb = excelApp.Workbooks.Add();
                Excel.Worksheet ws = wb.Sheets[1];
                excelApp.Visible = false;
                excelApp.DisplayAlerts = false;
                ws.Cells[1, 2] = "Заявка на заказ товара от " + DateTime.Now.ToShortDateString();
                ws.Cells[2, 2] = "Наименование";
                ws.Cells[2, 3] = "Количество для заказа";
                for (int i = 0; i <= dg.RowCount - 2; i++)
                {
                    ws.Cells[i + 3, 1] = i + 1;
                    for (int j = 0; j <= dg.ColumnCount - 2; j++)
                    {
                        ws.Cells[i + 3, j + 2] = dg.Rows[i].Cells[j].Value;

                    }
                }
                ws.Columns.AutoFit();
                string fileName = DateTime.Now.ToString("dd_MM HH_mm") + ".xlsx";
                string fullPath = System.IO.Path.Combine(filePath, fileName);
                wb.SaveAs(fullPath);
                wb.Close();
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ws);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

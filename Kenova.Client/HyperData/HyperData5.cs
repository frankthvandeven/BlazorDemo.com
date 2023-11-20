using Microsoft.JSInterop;
using PicoXLSX;
using System;
using System.IO;
using System.Threading.Tasks;


//
// EXPORT functionality.
//

namespace Kenova.Client.Components
{
    public partial class HyperData<ItemType>
    {

        public Task ExportToExcelAsync()
        {
            return this.ExportToExcelAsync("myWorkbook.xlsx", "Sheet1");
        }

        public async Task ExportToExcelAsync(string fileName = "myWorkbook.xlsx", string sheetName = "Sheet1")
        {
            //const string fileType = "application/octet-stream"; // "application/vnd.ms-excel"; // "application/octet-stream";

            // https://github.com/rabanti-github/PicoXLSX

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            if (string.IsNullOrEmpty(sheetName))
                throw new ArgumentNullException("sheetName");

            Workbook workbook = new Workbook(sheetName);

            foreach (var col in this.Columns)
            {
                workbook.CurrentWorksheet.AddNextCell(col.Header, Style.BasicStyles.Bold);
            }

            //workbook.CurrentWorksheet.GetCell().DataType == Cell.CellType.NUMBER
            //workbook.CurrentWorksheet.Columns[1].Width


            workbook.CurrentWorksheet.GoToNextRow();

            for (int row = 0; row < this.DisplayItems.Count; row++)
            {
                ItemType item = this.DisplayItems[row];

                for (int col = 0; col < this.Columns.Count; col++)
                {
                    var hypercol = this.Columns[col];
                    object value = hypercol.Value(item);
                    workbook.CurrentWorksheet.AddNextCell(value);
                }

                workbook.CurrentWorksheet.GoToNextRow();

                if (row == 1)
                {

                }


            }

            MemoryStream ms = new MemoryStream();
            workbook.SaveAsStream(ms, true);
            //var stringData = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();

            //await KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNSaveAsFile", fileName, fileType, stringData);
            await KenovaClientConfig.JSRuntime.InvokeVoidAsync("KNSaveAsFile2", fileName, ms.ToArray());


        }

        private void resolveHeaderAlignment()
        {


        }

    }
}
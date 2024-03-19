using EquipmentsAccounting.database;
using EquipmentsAccounting.models;
using System;
using System.IO;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Data;
using OfficeOpenXml;
using System.Net;

namespace EquipmentsAccounting.Excel
{
    internal class ExcelHelper
    {

        public System.Data.DataTable ExcelDataToDataTable(string filePath, string sheetName, bool hasHeader = true)
        {
            var dt = new System.Data.DataTable();
            var fi = new FileInfo(filePath);
            // Check if the file exists
            if (!fi.Exists)
                throw new Exception("File " + filePath + " Does Not Exists");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var xlPackage = new ExcelPackage(fi);
            // Получить первый лист книги
            var worksheet = xlPackage.Workbook.Worksheets[sheetName];

            dt = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].ToDataTable(c =>
            {
                c.FirstRowIsColumnNames = true;
            });

            return dt;
        }

        public void CreateCustomReport(System.Data.DataTable dataTable, DataGrid dataGrid, bool disponsalSum, int sum, bool date, bool reason, System.Data.DataTable additionallyDataTable)
        {
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Add();
            Worksheet worksheet = workbook.Sheets[1];

            //Заполнение заголовков
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dataGrid.Columns[i].Header;
            }

            //Заполнение данных
            int rowCount = 1;
            foreach (DataRow row in dataTable.Rows)
            {
                int colCount = 1;
                foreach (DataColumn column in dataTable.Columns)
                {
                    worksheet.Cells[rowCount+1, colCount].Value = row[column.ColumnName].ToString();
                    colCount++;
                }
                rowCount++;
            }

            if (date || reason)
            {
                //Заполнение заголовков
                for (int i = 1; i < additionallyDataTable.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, dataTable.Columns.Count + i] = additionallyDataTable.Columns[i-1].ColumnName;
                }

                rowCount = 1;
                //Заполнение данных
                foreach (DataRow row in additionallyDataTable.Rows)
                {
                    int colCount = 1;
                    foreach (DataColumn column in additionallyDataTable.Columns)
                    {
                        if (column.ColumnName == "Дата списания" || column.ColumnName == "Дата начала обслуживания")
                        {
                            worksheet.Cells[rowCount + 1, dataTable.Columns.Count + colCount].Value = row[column.ColumnName].ToString().Substring(0, 10);
                        }
                        else
                        {
                            worksheet.Cells[rowCount + 1, dataTable.Columns.Count + colCount].Value = row[column.ColumnName].ToString();
                        }
                        colCount++;
                    }
                    rowCount++;
                }

                /*if (disponsalSum)
                {
                    worksheet.Cells[dataTable.Rows.Count + 2, dataTable.Columns.Count + additionallyDataTable.Columns.Count] = sum;
                    worksheet.Cells[dataTable.Rows.Count + 2, 1] = "Сумма";
                }*/
            }
            /*else
            {
                if (disponsalSum)
                {
                    worksheet.Cells[dataTable.Rows.Count + 2, dataTable.Columns.Count] = sum;
                    worksheet.Cells[dataTable.Rows.Count + 2, 1] = "Сумма";
                }
            }*/

            if (disponsalSum)
            {
                worksheet.Cells[dataTable.Rows.Count + 2, dataTable.Columns.Count] = sum;
                worksheet.Cells[dataTable.Rows.Count + 2, 1] = "Сумма";
            }

            // Сохранение файла
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Катосмный документ.xlsx");

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();
        }

        public void createIssueAct(List<IssueAct> actList, int request, int release)
        {
            Application excelApp = new Application();

            Workbook workbook = excelApp.Workbooks.Open("E:\\!АСУТП Проект!\\PROJECT\\excel шаблоны\\ввод в эксплуатацию.xlsx");
            //Workbook workbook = excelApp.Workbooks.Open("D:\\!АСУТП Проект!\\PROJECT\\excel шаблоны\\ввод в эксплуатацию.xlsx");
            Worksheet worksheet = workbook.Worksheets[1];

            Database database = new Database();
            IssueAct act = database.getAct(50, 1, 1);

            //Выделение и вырезание
            //------------------------------------------------------------------------------------------

            //Выделение и вырезание области с подписями----------
            Range copyRangeBottom = worksheet.Range["B21:N22"];
            copyRangeBottom.Cut();

            //Вставка вырезанной области
            Range pasteCellBottom = worksheet.Range["B" + (21 + actList.Count + 2)]; // В по + (3 + 1)
            pasteCellBottom.Insert(XlInsertShiftDirection.xlShiftDown);
            //cutRange(worksheet, "B21:N22", "B23");


            //Выделение и вырез области Итого--------------
            Range copyRangeSignature = worksheet.Range["B18:N18"];
            copyRangeSignature.Cut();

            int startRow = 18 + actList.Count;
            int count = 0;
            //Вставка вырезанной области
            while (count < actList.Count)
            {
                worksheet.Rows[startRow + count].RowHeight = 10.50; //Регулировка высоты строки
                count++;
            }
            string pasteRangeSignatureCell = "B21" + (18 + (actList.Count - 1));
            worksheet.Rows[21 + (actList.Count - 1)].RowHeight = 25.50;
            Range pasteRangeSignature = worksheet.Range[pasteRangeSignatureCell]; // А и В по + (3-1)
            pasteRangeSignature.Insert(XlInsertShiftDirection.xlShiftDown);



            //Установка границ для bottom "Итого"
            string borderRowBottom = "B" + (18 + (actList.Count - 1));
            string borederColumnBottom = "N" + (18 + (actList.Count - 1));

            Range borderRangeBottom = worksheet.Range[borderRowBottom + ":" + borederColumnBottom];
            /*borderRangeBottom.Borders.LineStyle = XlLineStyle.xlLineStyleNone;
            borderRangeBottom.Borders.Weight = XlBorderWeight.xlThin;*/

            borderRangeBottom.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous; // верхняя внешняя
            borderRangeBottom.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous; // правая внешняя
            borderRangeBottom.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous; // левая внешняя
            borderRangeBottom.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous; // нижняя внешная

            //------------------------------------------------------------------------------------------

            //Заполнение вставенной области
            //------------------------------------------------------------------------------------------

            // Ячейка "Итого:"
            worksheet.Range[worksheet.Cells[18 + (actList.Count - 1), 4], worksheet.Cells[18 + (actList.Count - 1), 5]].Merge();
            Range cell1 = worksheet.Cells[18 + (actList.Count - 1), 4];
            cell1.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            cell1.VerticalAlignment = XlVAlign.xlVAlignBottom;
            worksheet.Cells[18 + (actList.Count - 1), 4] = "Итого:";

            // Итого-Запрошено
            Range cell2 = worksheet.Cells[18 + (actList.Count - 1), 10];
            cell2.HorizontalAlignment = XlHAlign.xlHAlignRight;
            cell2.VerticalAlignment = XlVAlign.xlVAlignTop;
            cell2.NumberFormat = "#,##0.000";
            worksheet.Cells[18 + (actList.Count - 1), 10] = actList.Count;//actList[i].requests;

            // Итого-Отпущено
            Range cell3 = worksheet.Cells[18 + (actList.Count - 1), 11];
            cell3.HorizontalAlignment = XlHAlign.xlHAlignRight;
            cell3.VerticalAlignment = XlVAlign.xlVAlignTop;
            cell3.NumberFormat = "#,##0.000";
            worksheet.Cells[18 + (actList.Count - 1), 11] = actList.Count;//actList[i].released;

            // Итого-Сумма
            //Заполнение ячейки ниже
            

            //------------------------------------------------------------------------------------------

            // Ячейка "Отпустил"
            Range cell5 = worksheet.Cells[21 + (actList.Count + 1), 2];
            cell5.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            cell5.VerticalAlignment = XlVAlign.xlVAlignTop;
            //worksheet.Cells[21 + 2, 2] = "Отпустил";

            // Ячейка "Начальник отдела"
            worksheet.Range[worksheet.Cells[21 + (actList.Count + 1), 3], worksheet.Cells[21 + actList.Count, 4]].Merge();
            Range cell6 = worksheet.Cells[21 + (actList.Count - 1), 3];
            cell6.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            cell6.VerticalAlignment = XlVAlign.xlVAlignBottom;
            //worksheet.Cells[21 + 2, 3] = "Начальник отдела";

            // ФИО начальника отдела
            worksheet.Range[worksheet.Cells[21 + (actList.Count - 1), 7], worksheet.Cells[21 +  (actList.Count - 1), 8]].Merge();
            Range cell7 = worksheet.Cells[21 +  (actList.Count - 1), 7];
            cell7.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            cell7.VerticalAlignment = XlVAlign.xlVAlignBottom;
            worksheet.Cells[21 + (actList.Count - 1), 7] = actList[0].manager_full;

            // Получил
            worksheet.Range[worksheet.Cells[21  + (actList.Count + 1), 7], worksheet.Cells[21 + actList.Count, 8]].Merge();
            Range cell8 = worksheet.Cells[21  + (actList.Count + 1), 7];
            cell8.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            cell8.VerticalAlignment = XlVAlign.xlVAlignTop;
            //worksheet.Cells[21+2, 10] = "Получил";

            worksheet.Cells[21 + (actList.Count - 1), 10] = actList[0].employeePosition;

            worksheet.Cells[21 + (actList.Count - 1), 13] = actList[0].employeeName_full;

            // Заполнение строк таблицы
            //------------------------------------------------------------------------------------------

            int row = 17;

            // Дата составления акта
            DateTime currentDate = DateTime.Today;
            worksheet.Cells[8, 2] = currentDate.ToString("dd.MM.yyyy");

            int sum = 0;

            for (int i = 0; i < actList.Count; i++)
            {
                worksheet.Rows[row + i].RowHeight = 21.75;

                // Субсчёт
                worksheet.Cells[row + i, 2] = actList[i].subschet;
                Range cell11 = worksheet.Cells[row + i, 2];
                cell11.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                cell11.VerticalAlignment = XlVAlign.xlVAlignTop;

                // Наименование оборудования
                worksheet.Cells[row + i, 4] = actList[i].eqName;
                worksheet.Range[worksheet.Cells[row + i, 4], worksheet.Cells[row + i, 6]].Merge();
                Range cell22 = worksheet.Cells[row + i, 4];
                cell22.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                cell22.VerticalAlignment = XlVAlign.xlVAlignTop;

                // Номенклатурный номер
                worksheet.Cells[row + i, 7] = actList[i].serialNum;
                Range cell33 = worksheet.Cells[row + i, 7];
                cell33.HorizontalAlignment = XlHAlign.xlHAlignRight;
                cell33.VerticalAlignment = XlVAlign.xlVAlignTop;

                // Код
                worksheet.Cells[row + i, 8] = "796";
                Range cell44 = worksheet.Cells[row + i, 8];
                cell44.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                cell44.VerticalAlignment = XlVAlign.xlVAlignTop;

                // Ед. измерения
                worksheet.Cells[row + i, 9] = "шт";
                Range cell55 = worksheet.Cells[row + i, 9];
                cell55.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                cell55.VerticalAlignment = XlVAlign.xlVAlignTop;

                // Запрошено
                Range cell66 = worksheet.Cells[row + i, 10];
                cell66.HorizontalAlignment = XlHAlign.xlHAlignRight;
                cell66.VerticalAlignment = XlVAlign.xlVAlignTop;
                worksheet.Cells[row + i, 10] = actList[i].requests;
                cell66.NumberFormat = "#,##0.000";

                // Отпущено
                Range cell77 = worksheet.Cells[row + i, 11];
                cell77.HorizontalAlignment = XlHAlign.xlHAlignRight;
                cell77.VerticalAlignment = XlVAlign.xlVAlignTop;
                worksheet.Cells[row + i, 11] = actList[i].released;
                cell77.NumberFormat = "#,##0.000";

                // Цена 
                Range cell88 = worksheet.Cells[row + i, 12];
                cell88.HorizontalAlignment = XlHAlign.xlHAlignRight;
                cell88.VerticalAlignment = XlVAlign.xlVAlignTop;
                worksheet.Cells[row + i, 12] = actList[i].price;
                cell88.NumberFormat = "#,##0.00";

                // Сумма
                Range cell99 = worksheet.Cells[row + i, 13];
                cell99.HorizontalAlignment = XlHAlign.xlHAlignRight;
                cell99.VerticalAlignment = XlVAlign.xlVAlignTop;
                cell88.NumberFormat = "#,##0.00";
                worksheet.Cells[row + i, 13] = actList[i].sum;

                // Инвентарный номер
                worksheet.Cells[row + i, 14] = actList[i].nomenNum;
                Range cell0 = worksheet.Cells[row + i, 14];
                cell0.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                cell0.VerticalAlignment = XlVAlign.xlVAlignTop;

                sum += actList[i].sum;
            }
            // Заполнение ячейки Итого-сумма
            worksheet.Cells[18 + (actList.Count - 1), 13] = sum;//actList[i].sum;
            Range cell4 = worksheet.Cells[18 + (actList.Count - 1), 13];
            cell4.HorizontalAlignment = XlHAlign.xlHAlignRight;
            cell4.VerticalAlignment = XlVAlign.xlVAlignTop;
            cell4.NumberFormat = "#,##0.00";

            //Установка границ для таблицы
            string borderRowTable = "B" + 17;
            string borederColumnTable = "N" + (17 +  (actList.Count - 1));

            Range borderRangeTable = worksheet.Range[borderRowTable + ":" + borederColumnTable];
            borderRangeTable.Borders.LineStyle = XlLineStyle.xlContinuous;
            borderRangeTable.Borders.Weight = XlBorderWeight.xlThin;


            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Ввод в эксплуатацию.xlsx");

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();
        }

        public void CreateWirteOffAct(List<IssueAct> list)
        {
            // Открытие документа
            Microsoft.Office.Interop.Excel.Application excelApp = new Application();

            Workbook workbook = excelApp.Workbooks.Open("E:\\!АСУТП Проект!\\PROJECT\\excel шаблоны\\сдача оборудования.xlsx");
            //Workbook workbook = excelApp.Workbooks.Open("D:\\!АСУТП Проект!\\PROJECT\\excel шаблоны\\сдача оборудования.xlsx");
            Worksheet worksheet = workbook.Worksheets[1];

            //Выделение и вырезание
            //------------------------------------------------------------------------------------------

            // Дата составления акта
            DateTime currentDate = DateTime.Today;
            worksheet.Cells[10, 2] = currentDate.ToString("dd.MM.yyyy");

            if (list.Count > 1)
            {
                // Область с подписями-------------------------------------------
                // Воизбежании ошибки перекрывания области, производиться вырезание и вставка со смещением в сторону для очистки области без потери bottom
                Range firstCopyRangeBottom = worksheet.Range["D19:AA24"];
                firstCopyRangeBottom.Cut();

                Range firstPasteCellBottom = worksheet.Range["AF19"]; // D16 + list.count
                firstPasteCellBottom.Insert(XlInsertShiftDirection.xlShiftDown);

                // Повторение процесса с указанием нужных ячеек
                Range copyRangeBottom = worksheet.Range["AF19:BC24"];
                copyRangeBottom.Cut();

                string insertBottomCell = "D" + (19 + list.Count - 1);
                Range pasteCellBottom = worksheet.Range[insertBottomCell]; // D19 + list.count - 1
                pasteCellBottom.Insert(XlInsertShiftDirection.xlShiftDown);

                int count = 0;
                // Регулировка высоты строк 
                while (count < list.Count)
                {
                    worksheet.Rows[19 + list.Count - 1].RowHeight = 11.25; //Регулировка высоты строки
                    count++;
                }

                // Область-Итого------------------------------------------------
                Range copyRangeSignature = worksheet.Range["B16:AI16"];
                copyRangeSignature.Cut();

                string insertSignatureCell = "B" + (16 + list.Count);
                Range pasteRangeSignature = worksheet.Range[insertSignatureCell]; // B16 + list.count - 1
                pasteRangeSignature.Insert(XlInsertShiftDirection.xlShiftDown);
            }

            //Заполнение ячеек bottom 
            //------------------------------------------------------------------------------------------

            // Область с подписями------------------------------------------

            //Ячейка ФИО начальника отдела
            worksheet.Cells[19 + (list.Count - 1), 21] = list[0].manager_inic;
            // Ячейка "Должность сотрудника"
            worksheet.Cells[19 + (list.Count + 3), 5] = list[0].employeePosition;
            // Ячейка "ФИО сотрудника"
            worksheet.Cells[19 + (list.Count + 3), 21] = list[0].employeeName_inic;

            // Область-Итого------------------------------------------------

            // Ячейка "Итого шт"
            worksheet.Cells[16 + list.Count - 1, 16] = list.Count;
            // Ячейка Итого сумма заполняется после вставки данных в таблицу


            // Заполнение таблицы
            //------------------------------------------------------------------------------------------

            int sum = 0;
            int row = 15;
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    worksheet.Rows[row + i].RowHeight = 21.75;

                    //Копирование строки
                    Range copyCell = worksheet.Range["B15:AI15"];
                    copyCell.Copy();

                    //Вставка строки
                    Range pasteRange = worksheet.Range["B" + (15 + i)];
                    pasteRange.PasteSpecial(XlPasteType.xlPasteAll);
                }

                //Наименование оборудования
                worksheet.Cells[row + i, 2] = list[i].eqName;
                // Номенклатурный номер
                worksheet.Cells[row + i, 7] = list[i].serialNum; // ???
                Range nomenNumCell = worksheet.Cells[row + i, 7];
                nomenNumCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                nomenNumCell.VerticalAlignment = XlVAlign.xlVAlignTop;
                // Код
                worksheet.Cells[row + i, 8] = "796";
                worksheet.Range[worksheet.Cells[row + i, 8], worksheet.Cells[row + i, 9]].Merge();
                Range codeCell = worksheet.Cells[row + i, 8];
                codeCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                codeCell.VerticalAlignment = XlVAlign.xlVAlignTop;
                // Шт по документу
                worksheet.Cells[row + i, 14] = "1";
                Range docCountCell = worksheet.Cells[row + i, 14];
                docCountCell.NumberFormat = "#,##0.000";
                docCountCell.HorizontalAlignment = XlHAlign.xlHAlignRight;
                docCountCell.VerticalAlignment = XlVAlign.xlVAlignTop;
                // Принято шт
                worksheet.Cells[row + i, 16] = "1";
                Range getCell = worksheet.Cells[row + i, 16];
                getCell.NumberFormat = "#,##0.000";
                getCell.HorizontalAlignment = XlHAlign.xlHAlignRight;
                getCell.VerticalAlignment = XlVAlign.xlVAlignTop;
                // Цена
                worksheet.Cells[row + i, 19] = list[i].price;
                Range priceCell = worksheet.Cells[row + i, 19];
                priceCell.NumberFormat = "#,##0.00";
                priceCell.HorizontalAlignment = XlHAlign.xlHAlignRight;
                priceCell.VerticalAlignment = XlVAlign.xlVAlignTop;
                // Сумма
                worksheet.Cells[row + i, 22] = list[i].sum;
                Range sumCell = worksheet.Cells[row + i, 22];
                sumCell.NumberFormat = "#,##0.00";
                sumCell.HorizontalAlignment = XlHAlign.xlHAlignRight;
                sumCell.VerticalAlignment = XlVAlign.xlVAlignTop;

                sum += list[i].sum;
            }

            // Общая сумма
            worksheet.Cells[16 + list.Count - 1, 22] = sum;
            Range finalSumCell = worksheet.Cells[16 + list.Count - 1, 22];
            finalSumCell.NumberFormat = "#,##0.00";
            finalSumCell.HorizontalAlignment = XlHAlign.xlHAlignRight;
            finalSumCell.VerticalAlignment = XlVAlign.xlVAlignTop;

            // Сохранение и закрытие документа
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Сдача оборудования_Тест.xlsx");

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();
        }

        public void CreateHandOverAct(List<IssueAct> list, string fromEmployeePositin, string fromEmployeeName)
        {
            // Открытие документа
            Application excelApp = new Application();

            Workbook workbook = excelApp.Workbooks.Open("E:\\!АСУТП Проект!\\PROJECT\\excel шаблоны\\передача.xlsx");
            //Workbook workbook = excelApp.Workbooks.Open("D:\\!АСУТП Проект!\\PROJECT\\excel шаблоны\\передача.xlsx");
            Worksheet worksheet = workbook.Worksheets[1];

            //Выделение и вырезание
            //------------------------------------------------------------------------------------------

            // Дата составления акта
            DateTime currentDate = DateTime.Today;
            worksheet.Cells[8, 2] = currentDate.ToString("dd.MM.yyyy");

            if (list.Count > 1)
            {
                // Область с подписями-------------------------------------------
                // Воизбежании ошибки перекрывания области, производиться вырезание и вставка со смещением в сторону для очистки области без потери bottom
                Range firstCopyRangeBottom = worksheet.Range["B20:N21"];
                firstCopyRangeBottom.Cut();

                Range firstPasteCellBottom = worksheet.Range["O20"]; // D16 + list.count
                firstPasteCellBottom.Insert(XlInsertShiftDirection.xlShiftDown);

                // Повторение процесса с указанием нужных ячеек
                Range copyRangeBottom = worksheet.Range["O20:AA21"];
                copyRangeBottom.Cut();

                string insertBottomCell = "B" + (20 + list.Count - 1);
                Range pasteCellBottom = worksheet.Range[insertBottomCell]; // D19 + list.count - 1
                pasteCellBottom.Insert(XlInsertShiftDirection.xlShiftDown);

                /*int count = 0;
                // Регулировка высоты строк 
                while (count < list.Count)
                {
                    worksheet.Rows[19 + list.Count - 1].RowHeight = 11.25; //Регулировка высоты строки
                    count++;
                }*/

                //Заполнение ячеек bottom 
                //---------------------------------------------------------------
                
            }
            //Ячейка Отпустил-должность
            worksheet.Cells[20 + (list.Count - 1), 3] = fromEmployeePositin;//list[0].employeePosition;
            // Ячейка Отпустил-ФИО
            worksheet.Cells[20 + (list.Count - 1), 7] = fromEmployeeName;//list[0].employeeName_inic;
            // Ячейка Принял-должность
            worksheet.Cells[20 + (list.Count - 1), 10] = list[0].employeePosition;//getEmployeePositin;
            // Ячейка Принял-ФИО
            worksheet.Cells[20 + (list.Count - 1), 13] = list[0].employeeName_inic;//getEmployeeName;

            //Заполнение строк таблица
            //---------------------------------------------------------------

            int row = 17;
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    //worksheet.Rows[row + i].RowHeight = 21.75;

                    //Копирование строки
                    Range copyCell = worksheet.Range["B17:N17"];
                    copyCell.Copy();

                    //Вставка строки
                    Range pasteRange = worksheet.Range["B" + (row + i)];
                    pasteRange.PasteSpecial(XlPasteType.xlPasteAll);
                }
                // Субсчёт
                worksheet.Cells[row + i, 2] = list[i].subschet;
                // Наименование оборудования
                worksheet.Cells[row + i, 4] = list[i].eqName;
                // Номенклатурный номер
                worksheet.Cells[row + i, 7] = list[i].serialNum;
                Range nomenNumCell = worksheet.Cells[row + i, 7];
                nomenNumCell.HorizontalAlignment = XlHAlign.xlHAlignCenter;
                nomenNumCell.VerticalAlignment = XlVAlign.xlVAlignTop;
                // Код
                worksheet.Cells[row + i, 8] = "796";
                // Затребовано шт
                worksheet.Cells[row + i, 10] = "1";
                // Отпущено шт
                worksheet.Cells[row + i, 11] = "1";
                // Цена
                worksheet.Cells[row + i, 12] = list[i].price;
                // Сумма
                worksheet.Cells[row + i, 13] = list[i].sum;
                // Порядковый номер по складской карте
                worksheet.Cells[row + i, 14] = list[i].nomenNum;

            }

            // Сохранение и закрытие документа------------------------------
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Передача_Тест.xlsx");

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();
        }

        public void CreateStockReport()
        {
            // Открытие
            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Open("E:\\!АСУТП Проект!\\PROJECT\\excel шаблоны\\Отчёты\\ТМЦ на складах.xlsx");
            Worksheet worksheet = workbook.Worksheets[1];

            Database db = new Database();
            List<Departament> departments = db.getDepartaments();
            int endCount = 0;
            int endSum = 0;
            int endPrice = 0;


            DateTime currentDate = DateTime.Today;
            worksheet.Cells[2, 1] = String.Format("Остатки ТМЦ (на складах) на {0} г.", currentDate.ToString("dd MMMM yyyy"));

            int row = 8;
            for (int i = 0; i < departments.Count; i++)
            {
                // Копирование и вставка
                Range copyRange = worksheet.Range["A8:F8"];
                copyRange.Copy();
                Range pasteRange = worksheet.Range["A" + row];
                pasteRange.PasteSpecial(XlPasteType.xlPasteAll);

                // Редактирование заголовков
                worksheet.Range["A" + row + ":B" + row].Merge(System.Type.Missing);
                worksheet.get_Range("A" + row, "B" + row).Cells.HorizontalAlignment = XlHAlign.xlHAlignLeft;
                worksheet.Range["D" + row + ":E" + row].Merge(System.Type.Missing);
                worksheet.get_Range("D" + row, "E" + row).Cells.HorizontalAlignment = XlHAlign.xlHAlignRight;

                int id = departments[i].id;

                List<StockEquipmentsInfo> equipments = db.GetStockEquipmentsInfo(id);


                int count = 0;
                int sum = 0;
                int price = 0;
                for (int j = 0; j < equipments.Count; j++)
                {
                    count += equipments[j].Count;
                    sum += equipments[j].Sum;
                    price += equipments[j].Price;
                }
                endCount += count;
                endSum += sum;
                endPrice += price;

                worksheet.Cells[row, 1] = departments[i].name;
                worksheet.Cells[row, 3] = count;
                worksheet.Cells[row, 4] = sum;
                worksheet.Cells[row, 6] = price;

                row++;

                for (int k = 0; k < equipments.Count; k++)
                {
                    worksheet.Cells[row, 1] = equipments[k].Characteristics;
                    worksheet.Cells[row, 3] = equipments[k].Count;
                    worksheet.Cells[row, 4] = equipments[k].Sum;
                    worksheet.Cells[row, 6] = equipments[k].Price;

                    row++;
                }
            }

            // Копирование и вставка
            Range copyRangeEnd = worksheet.Range["A8:F8"];
            copyRangeEnd.Copy();
            Range pasteRangeEnd = worksheet.Range["A" + row];
            pasteRangeEnd.PasteSpecial(XlPasteType.xlPasteAll);

            // Редактирование заголовков
            worksheet.Range["A" + row + ":B" + row].Merge(System.Type.Missing);
            worksheet.get_Range("A" + row, "B" + row).Cells.HorizontalAlignment = XlHAlign.xlHAlignLeft;
            worksheet.Range["D" + row + ":E" + row].Merge(System.Type.Missing);
            worksheet.get_Range("D" + row, "E" + row).Cells.HorizontalAlignment = XlHAlign.xlHAlignRight;

            worksheet.Cells[row, 1] = "Итого";
            worksheet.Cells[row, 3] = endCount;
            worksheet.Cells[row, 4] = endSum;
            worksheet.Cells[row, 6] = endPrice;

            // Усановка жирного шрифта
            worksheet.get_Range("A" + row, "F" + row).Font.Bold = true;



            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "Склад_тест.xlsx");

            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();
        }
    }
}

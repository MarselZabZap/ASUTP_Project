using EquipmentsAccounting.database;
using EquipmentsAccounting.Excel;
using EquipmentsAccounting.models;
using EquipmentsAccounting.views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace EquipmentsAccounting.windows
{
    /// <summary>
    /// Логика взаимодействия для IssueEquipmentWindow.xaml
    /// </summary>
    public partial class IssueEquipmentWindow : Window
    {

        Database database;
        DataTable equipmentsDataTable;
        DataTable selectedEquipmentsDataTable;
        EmployeesPage employeesPage;

        public IssueEquipmentWindow(EmployeesPage employeesPage)
        {
            this.employeesPage = employeesPage;
            InitializeComponent();


            database = new Database();
            equipmentsDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                    AND ""Статус"" = 'На складе' AND ""Серийный номер"" != ''
                                ORDER BY id", Singleton.MANAGER.Dep_id));

            EquipmentsDataGrid.DataContext = equipmentsDataTable.DefaultView;


            selectedEquipmentsDataTable = new DataTable();
            DataSet dataSet = new DataSet();

            selectedEquipmentsDataTable.Columns.Add(CreateColumn("id", "id"));
            //selectedEquipmentsDataTable.Columns.Add(CteateColumn("eq_id", "eq_id"));
            selectedEquipmentsDataTable.Columns.Add(CreateColumn("Тип", "Тип"));
            selectedEquipmentsDataTable.Columns.Add(CreateColumn("Характеристики", "Характеристики"));
            selectedEquipmentsDataTable.Columns.Add(CreateColumn("Серийный номер", "Серийный номер"));
            selectedEquipmentsDataTable.Columns.Add(CreateColumn("Статус", "Статус"));

            dataSet.Tables.Add(selectedEquipmentsDataTable);
        }

        private void DataFilter(object sender, KeyEventArgs e)
        {
            string[] filterList = new string[] { "Тип", "Характеристики", "Серийный номер" };

            if ((bool)TypeRadioButton.IsChecked)
            {
                EquipmentsDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", Singleton.MANAGER.Dep_id, filterList[0], FilterTextBox.Text)).DefaultView;
            }
            else if ((bool)CharsRadioButton.IsChecked)
            {
                EquipmentsDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", Singleton.MANAGER.Dep_id, filterList[1], FilterTextBox.Text)).DefaultView;
            }
            else
            {
                EquipmentsDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", Singleton.MANAGER.Dep_id, filterList[2], FilterTextBox.Text)).DefaultView;
            }
        }

        private void EquipmentsDataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            //Создание строки   
            DataRow dataRow = selectedEquipmentsDataTable.NewRow();


            //Заполнение таблицы строками
            var id = EquipmentsDataGrid.Columns[0].GetCellContent(EquipmentsDataGrid.Items[EquipmentsDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "id", id.Text);

            var type = EquipmentsDataGrid.Columns[1].GetCellContent(EquipmentsDataGrid.Items[EquipmentsDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Тип", type.Text);

            var cahrs = EquipmentsDataGrid.Columns[2].GetCellContent(EquipmentsDataGrid.Items[EquipmentsDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Характеристики", cahrs.Text);

            var serialNum = EquipmentsDataGrid.Columns[3].GetCellContent(EquipmentsDataGrid.Items[EquipmentsDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Серийный номер", serialNum.Text);

            var status = EquipmentsDataGrid.Columns[4].GetCellContent(EquipmentsDataGrid.Items[EquipmentsDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Статус", status.Text);

            SelectedEquipmentsDataGrid.DataContext = selectedEquipmentsDataTable.DefaultView;

            //Удаление строки
            DataRow rowForRemove = equipmentsDataTable.Rows[EquipmentsDataGrid.SelectedIndex];
            equipmentsDataTable.Rows.Remove(rowForRemove);
        }

        private void SelectedEquipmentsDataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            // Код для удаления строк и нижней таблицы
            DataRow newRow = equipmentsDataTable.NewRow();

            //Заполнение таблицы строками
            var id = SelectedEquipmentsDataGrid.Columns[0].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["id"] = id.Text;

            var type = SelectedEquipmentsDataGrid.Columns[1].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Тип"] = type.Text;

            var cahrs = SelectedEquipmentsDataGrid.Columns[2].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Характеристики"] = cahrs.Text;

            var serialNum = SelectedEquipmentsDataGrid.Columns[3].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Серийный номер"] = serialNum.Text;

            var status = SelectedEquipmentsDataGrid.Columns[4].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Статус"] = status.Text;

            equipmentsDataTable.Rows.Add(newRow);
            EquipmentsDataGrid.DataContext = equipmentsDataTable.DefaultView;

            //Удаление строки
            DataRow rowForRemove = selectedEquipmentsDataTable.Rows[SelectedEquipmentsDataGrid.SelectedIndex];
            selectedEquipmentsDataTable.Rows.Remove(rowForRemove);
        }

        //Создание столбца
        private DataColumn CreateColumn(string columnName, string caption)
        {
            DataColumn dataColumn = new DataColumn
            {
                DataType = System.Type.GetType("System.String"), //Задать тип столбца
                ColumnName = columnName, //Имя стольца
                AutoIncrement = false, //автоинкремент
                Caption = caption, //Подпись столбца
                ReadOnly = false, //Параметор "только для чтения"
                Unique = false //Уникальность
            };

            return dataColumn;

        }

        //Cоздание и добавление строки
        private void CreateRow(DataTable dataTable, DataRow row, string columnName, string value)
        {
            try
            {
                row[columnName] = value;
                dataTable.Rows.Add(row);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void Issue(object sender, RoutedEventArgs e)
        {
            string id = null;
            DataRow rowForRemove = null;
            List<IssueAct> actList = new List<IssueAct>();

            if (selectedEquipmentsDataTable.Rows.Count > 0)
            {
                /*for (int i = -1; i < selectedEquipmentsDataTable.Rows.Count; i++)
                {
                    eqId = (SelectedEquipmentsDataGrid.Columns[1].GetCellContent(SelectedEquipmentsDataGrid.Items[0]) as TextBlock).Text;
                    database.Query(String.Format(@"CALL issue_eq({0}, {1});", Singleton.EMPLOYEE.Id, Int16.Parse(eqId)));

                    // Удаление строки в таблице
                    rowForRemove = selectedEquipmentsDataTable.Rows[0];
                    selectedEquipmentsDataTable.Rows.Remove(rowForRemove);

                    actList.Add(database.getAct(Int16.Parse(eqId), 1, 1));
                }*/
                int i = 0;
                while (i < selectedEquipmentsDataTable.Rows.Count)
                {
                    id = (SelectedEquipmentsDataGrid.Columns[0].GetCellContent(SelectedEquipmentsDataGrid.Items[0]) as TextBlock).Text;
                    database.Query(String.Format(@"CALL issue_eq({0}, {1});", Singleton.EMPLOYEE.Id, Int16.Parse(id)));

                    // Удаление строки в таблице
                    rowForRemove = selectedEquipmentsDataTable.Rows[0];
                    selectedEquipmentsDataTable.Rows.Remove(rowForRemove);

                    actList.Add(database.getAct(Int16.Parse(id), 1, 1));
                }

                employeesPage.EmployeeInfoDGValue = database.Query(String.Format( @"SELECT id, concat(firstname, ' ', lastname) AS ""Сотрудник"", eq_count AS ""Кол-во""
                                                                                    FROM employee_info_for_desktop
                                                                                    WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));

                ExcelHelper excelHelper = new ExcelHelper();
                excelHelper.createIssueAct(actList, 1, 1);

                Notification.Visibility = Visibility.Visible;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(TimerTick);
                timer.Interval = new TimeSpan(0, 0, 3);
                timer.Start();

                /*eqId = (SelectedEquipmentsDataGrid.Columns[0].GetCellContent(SelectedEquipmentsDataGrid.Items[0]) as TextBlock).Text;
                database.Query(String.Format(@"CALL issue_eq({0}, {1});", Singleton.EMPLOYEE.Id, Int16.Parse(eqId)));

                rowForRemove = selectedEquipmentsDataTable.Rows[0];
                selectedEquipmentsDataTable.Rows.Remove(rowForRemove);*/
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Notification.Visibility = Visibility.Collapsed;
        }
    }
}

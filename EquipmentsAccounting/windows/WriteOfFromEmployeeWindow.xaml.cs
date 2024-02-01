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
    /// Логика взаимодействия для WriteOfEquipmentsWindow.xaml
    /// </summary>
    public partial class WriteOfFromEmployeeWindow : Window
    {

        readonly Database database;
        readonly DataTable equipmentsOfSelectedEmployee;
        readonly DataTable selectedEquipmentsDataTable;
        EmployeesPage employeesPage;

    public WriteOfFromEmployeeWindow(EmployeesPage employeesPage)
        {
            this.employeesPage = employeesPage; 
            InitializeComponent();

            database = new Database();

            equipmentsOfSelectedEmployee = database.Query(String.Format(
                @"SELECT * FROM equipmentsOfEmployee({0})", Singleton.EMPLOYEE.Id));
            EquipmentsOfSelectedEmployeeDataGrid.DataContext = equipmentsOfSelectedEmployee.DefaultView;


            selectedEquipmentsDataTable = new DataTable();
            DataSet dataSet = new DataSet();

            selectedEquipmentsDataTable.Columns.Add(CteateColumn("id", "id"));
            selectedEquipmentsDataTable.Columns.Add(CteateColumn("eq_id", "eq_id"));
            selectedEquipmentsDataTable.Columns.Add(CteateColumn("Тип", "Тип"));
            selectedEquipmentsDataTable.Columns.Add(CteateColumn("Характеристики", "Характеристики"));
            selectedEquipmentsDataTable.Columns.Add(CteateColumn("Серийный номер", "Серийный номер"));
            //selectedEquipmentsDataTable.Columns.Add(CteateColumn("Статус", "Статус"));

            dataSet.Tables.Add(selectedEquipmentsDataTable);
        }

        private void EquipmentsOfSelectedEmployeeDataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            //Создание строки   
            DataRow dataRow = selectedEquipmentsDataTable.NewRow();


            //Заполнение таблицы строками
            var id = EquipmentsOfSelectedEmployeeDataGrid.Columns[0].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "id", id.Text);

            var eq_id = EquipmentsOfSelectedEmployeeDataGrid.Columns[1].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "eq_id", eq_id.Text);

            var type = EquipmentsOfSelectedEmployeeDataGrid.Columns[2].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Тип", type.Text);

            var cahrs = EquipmentsOfSelectedEmployeeDataGrid.Columns[3].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Характеристики", cahrs.Text);

            var serialNum = EquipmentsOfSelectedEmployeeDataGrid.Columns[4].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Серийный номер", serialNum.Text);

            /*var status = EquipmentsOfSelectedEmployeeDataGrid.Columns[5].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Статус", status.Text);*/

            SelectedEquipmentsDataGrid.DataContext = selectedEquipmentsDataTable.DefaultView;


            DataRow rowForRemove = equipmentsOfSelectedEmployee.Rows[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex];
            equipmentsOfSelectedEmployee.Rows.Remove(rowForRemove);
        }

        private void SelectedEquipmentsCellFocused(object sender, MouseButtonEventArgs e)
        {
            //Создание строки   
            DataRow newRow = equipmentsOfSelectedEmployee.NewRow();


            //Заполнение таблицы строками
            var id = SelectedEquipmentsDataGrid.Columns[0].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["id"] = id.Text;

            var eq_id = SelectedEquipmentsDataGrid.Columns[1].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["eq_id"] = eq_id.Text;

            var type = SelectedEquipmentsDataGrid.Columns[2].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Тип"] = type.Text;

            var cahrs = SelectedEquipmentsDataGrid.Columns[3].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Характеристики"] = cahrs.Text;

            var serialNum = SelectedEquipmentsDataGrid.Columns[4].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Серийный номер"] = serialNum.Text;

            /*var status = SelectedEquipmentsDataGrid.Columns[4].GetCellContent(SelectedEquipmentsDataGrid.Items[SelectedEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            newRow["Статус"] = status.Text;*/

            equipmentsOfSelectedEmployee.Rows.Add(newRow);
            SelectedEquipmentsDataGrid.DataContext = selectedEquipmentsDataTable.DefaultView;


            DataRow rowForRemove = selectedEquipmentsDataTable.Rows[SelectedEquipmentsDataGrid.SelectedIndex];
            selectedEquipmentsDataTable.Rows.Remove(rowForRemove);
        }

        private void DataFilter(object sender, KeyEventArgs e)
        {
            string[] filterList = new string[] { "Тип", "Характеристики", "Серийный номер" };

            if ((bool)TypeRadioButton.IsChecked)
            {
                EquipmentsOfSelectedEmployeeDataGrid.DataContext = database.Query(String.Format(
                    @"SELECT * FROM equipmentsOfEmployee({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')", Singleton.EMPLOYEE.Id, filterList[0], FilterTextBox.Text))
                    .DefaultView;
            }
            else if ((bool)CharsRadioButton.IsChecked)
            {
                EquipmentsOfSelectedEmployeeDataGrid.DataContext = database.Query(String.Format(
                    @"SELECT * FROM equipmentsOfEmployee({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')", Singleton.EMPLOYEE.Id, filterList[1], FilterTextBox.Text))
                    .DefaultView;
            }
            else
            {
                EquipmentsOfSelectedEmployeeDataGrid.DataContext = database.Query(String.Format(
                    @"SELECT * FROM equipmentsOfEmployee({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')", Singleton.EMPLOYEE.Id, filterList[2], FilterTextBox.Text))
                    .DefaultView;
            }
        }

        private DataColumn CteateColumn(string columnName, string caption)
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
                //Ничего
            }
        }

        private void WriteOf(object sender, RoutedEventArgs e)
        {
            string id = null;
            string eqId = null;
            DataRow row = null;
            List<IssueAct> actList = new List<IssueAct>();
            if (selectedEquipmentsDataTable.Rows.Count > 0)
            {
                int i = 0;
                while (i < selectedEquipmentsDataTable.Rows.Count)
                {
                    id = (SelectedEquipmentsDataGrid.Columns[0].GetCellContent(SelectedEquipmentsDataGrid.Items[0]) as TextBlock).Text;
                    eqId = (SelectedEquipmentsDataGrid.Columns[1].GetCellContent(SelectedEquipmentsDataGrid.Items[0]) as TextBlock).Text;
                    database.Query(String.Format(@"CALL writeOffFromEmployee({0});", Int16.Parse(id)));

                    row = selectedEquipmentsDataTable.Rows[0];
                    selectedEquipmentsDataTable.Rows.Remove(row);

                    actList.Add(database.getAct(Int16.Parse(eqId), 1, 1));
                }

                employeesPage.EmployeeInfoDGValue = database.Query(String.Format(@"SELECT id, concat(firstname, ' ', lastname) AS ""Сотрудник"", eq_count AS ""Кол-во""
                                                                                   FROM employee_info_for_desktop
                                                                                   WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));

                ExcelHelper excelHelper = new ExcelHelper();
                excelHelper.CreateWirteOffAct(actList);

                Notification.Visibility = Visibility.Visible;

                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(TimerTick);
                timer.Interval = new TimeSpan(0, 0, 3);
                timer.Start();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Notification.Visibility = Visibility.Collapsed;
        }
    }
}

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
    /// Логика взаимодействия для HandOverWindow.xaml
    /// </summary>
    public partial class HandOverWindow : Window
    {

        Database database;
        DataTable fromEmployeesDataTable;
        DataTable toEmployeesDataTable;
        DataTable equipmentsDataTable;
        List<int> equipmentsExplIdList = new List<int>();
        List<int> equipmentsIdList = new List<int>();
        TextBlock toEmployeeId;
        TextBlock toEmployeePosition;
        TextBlock fromEmployeeName;
        TextBlock fromEmployeeId;
        TextBlock fromEmployeePosition;
        EmployeesPage employeesPage;

        public HandOverWindow(EmployeesPage employeesPage)
        {
            this.employeesPage = employeesPage;
            InitializeComponent();

            database = new Database();
            fromEmployeesDataTable = database.Query(String.Format(
                @"SELECT id, concat(lastname, ' ', SUBSTRING(firstname FROM 1 FOR 1), '. ', SUBSTRING(patronymic FROM 1 FOR 1), '.') AS ""Сотрудник"", position AS ""Должность"", eq_count AS ""Кол-во""
                  FROM employee_info_for_desktop
                  WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));

            /*toEmployeesDataTable = database.Query(String.Format(
                @"SELECT id, concat(lastname, ' ', SUBSTRING(firstname FROM 1 FOR 1), '. ', SUBSTRING(patronymic FROM 1 FOR 1), '.'), name AS ""Должность"", eq_count AS ""Кол-во""
                  FROM employee_info_for_desktop
                  WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));*/

            FromEmployeesDataGrid.DataContext = fromEmployeesDataTable.DefaultView;
            //ToEmployeeDataGrid.DataContext = toEmployeesDataTable.DefaultView;
        }

        private void HandOverButtonClick(object sender, RoutedEventArgs e)
        {
            List<IssueAct> actList = new List<IssueAct>();
            for (int i = 0; i < equipmentsExplIdList.Count; i++)
            {
                database.Query(String.Format(@"CALL writeOffFromEmployee({0})", equipmentsExplIdList[i]));
                database.Query(String.Format(@"CALL issue_eq({0}, {1})", toEmployeeId.Text, equipmentsIdList[i]));

                actList.Add(database.getAct(equipmentsIdList[i], 1, 1));
            }

            employeesPage.EmployeeInfoDGValue = database.Query(String.Format(@"SELECT id, concat(firstname, ' ', lastname) AS ""Сотрудник"", eq_count AS ""Кол-во""
                                                                               FROM employee_info_for_desktop
                                                                               WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));

            ExcelHelper excel = new ExcelHelper();
            excel.CreateHandOverAct(actList, fromEmployeePosition.Text.ToString(), fromEmployeeName.Text.ToString());

            Notification.Visibility = Visibility.Visible;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += new EventHandler(TimerTick);
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Start();

            EquipmentsNameLabel.Content = "";
            EmployeeNameLabel.Content = "";
            equipmentsExplIdList.Clear();
            equipmentsIdList.Clear();

            HandOverButton.IsEnabled = false;

            fromEmployeesDataTable = database.Query(String.Format(@"SELECT id, concat(lastname, ' ', SUBSTRING(firstname FROM 1 FOR 1), '. ', SUBSTRING(patronymic FROM 1 FOR 1), '.') AS ""Сотрудник"", position AS ""Должность"", eq_count AS ""Кол-во""
                                                                    FROM employee_info_for_desktop
                                                                    WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));

            toEmployeesDataTable = database.Query(String.Format(@"SELECT id, concat(lastname, ' ', SUBSTRING(firstname FROM 1 FOR 1), '. ', SUBSTRING(patronymic FROM 1 FOR 1), '.'), position AS ""Должность"", eq_count AS ""Кол-во""
                                                                  FROM employee_info_for_desktop
                                                                  WHERE dep_id = {0} AND id <> {1}", Singleton.MANAGER.Dep_id, fromEmployeeId.Text.ToString()));

            FromEmployeesDataGrid.DataContext = fromEmployeesDataTable.DefaultView;
            ToEmployeeDataGrid.DataContext = toEmployeesDataTable.DefaultView;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Notification.Visibility = Visibility.Collapsed;
        }


        // Таблица сотрудников, от которых идёт передача
        private void EmployeesFromCellFocused(object sender, MouseButtonEventArgs e)
        {
            EquipmentsFromFilterTextBox.IsEnabled = true;
            TypeRadioButton.IsEnabled = true;
            CharsRadioButton.IsEnabled = true;
            SerialNumRadioButton.IsEnabled = true;

            fromEmployeeId = FromEmployeesDataGrid.Columns[0].GetCellContent(FromEmployeesDataGrid.Items[FromEmployeesDataGrid.SelectedIndex]) as TextBlock;
            fromEmployeeName = FromEmployeesDataGrid.Columns[1].GetCellContent(FromEmployeesDataGrid.Items[FromEmployeesDataGrid.SelectedIndex]) as TextBlock;
            fromEmployeePosition = FromEmployeesDataGrid.Columns[2].GetCellContent(FromEmployeesDataGrid.Items[FromEmployeesDataGrid.SelectedIndex]) as TextBlock;

            Singleton.EMPLOYEE = new Employee(Int16.Parse(fromEmployeeId.Text), fromEmployeeName.Text);

            equipmentsDataTable = database.Query(String.Format(
                @"SELECT id, eq_id AS ""ОборудованиеID"", type_name AS ""Тип"", characteristics AS ""Характеристики"", serial_num AS ""Серийный номер"", to_char(date_issue, 'dd.MM.yyyy') as ""Дата выдачи""
                  FROM equipments eq
                  WHERE em_id = {0} and passed is null", Singleton.EMPLOYEE.Id));

            FromEmployeesEquipmentsDataGrid.DataContext = equipmentsDataTable.DefaultView;


            toEmployeesDataTable = database.Query(String.Format(
                @"SELECT id, concat(lastname, ' ', SUBSTRING(firstname FROM 1 FOR 1), '. ', SUBSTRING(patronymic FROM 1 FOR 1), '.'), position AS ""Должность"", eq_count AS ""Кол-во""
                  FROM employee_info_for_desktop
                  WHERE dep_id = {0} AND id <> {1}", Singleton.MANAGER.Dep_id, fromEmployeeId.Text.ToString()));

            ToEmployeeDataGrid.DataContext = toEmployeesDataTable.DefaultView;
        }

        // Таблица оборудования, выбранного сотрудника для передачи
        private void EmployeesFromEquipmentsCellFocused(object sender, MouseButtonEventArgs e)
        {
            var euipmentsExplId = FromEmployeesEquipmentsDataGrid.Columns[0].GetCellContent(FromEmployeesEquipmentsDataGrid.Items[FromEmployeesEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            var euipmentsId = FromEmployeesEquipmentsDataGrid.Columns[1].GetCellContent(FromEmployeesEquipmentsDataGrid.Items[FromEmployeesEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            var equipmentsName = FromEmployeesEquipmentsDataGrid.Columns[2].GetCellContent(FromEmployeesEquipmentsDataGrid.Items[FromEmployeesEquipmentsDataGrid.SelectedIndex]) as TextBlock;
            var equipmentsChars = FromEmployeesEquipmentsDataGrid.Columns[3].GetCellContent(FromEmployeesEquipmentsDataGrid.Items[FromEmployeesEquipmentsDataGrid.SelectedIndex]) as TextBlock;

            equipmentsExplIdList.Add(Int32.Parse(euipmentsExplId.Text));
            equipmentsIdList.Add(Int32.Parse(euipmentsId.Text));
            EquipmentsNameLabel.Content = EquipmentsNameLabel.Content + equipmentsName.Text + " " + equipmentsChars.Text + "\n";

            DataRow rowForRemove = equipmentsDataTable.Rows[FromEmployeesEquipmentsDataGrid.SelectedIndex];
            equipmentsDataTable.Rows.Remove(rowForRemove);

            if (EmployeeNameLabel.Content.ToString() != "[Наименование сотрудника]")
            {
                HandOverButton.IsEnabled = true;
            }
        }

        // Таблица сотрудников, которым будет передано оборудование
        private void EmployeeToCellFocused(object sender, MouseButtonEventArgs e)
        {
            toEmployeeId = ToEmployeeDataGrid.Columns[0].GetCellContent(ToEmployeeDataGrid.Items[ToEmployeeDataGrid.SelectedIndex]) as TextBlock;
            toEmployeePosition = ToEmployeeDataGrid.Columns[2].GetCellContent(ToEmployeeDataGrid.Items[ToEmployeeDataGrid.SelectedIndex]) as TextBlock;
            var toEmployeeName = ToEmployeeDataGrid.Columns[1].GetCellContent(ToEmployeeDataGrid.Items[ToEmployeeDataGrid.SelectedIndex]) as TextBlock;

            EmployeeNameLabel.Content = toEmployeeName.Text;

            if (EquipmentsNameLabel.Content.ToString() != "")
            {
                HandOverButton.IsEnabled = true;
            }
        }

        private void EquipmentsDataGridFiltartion(object sender, KeyEventArgs e)
        {

            if (EquipmentsFromFilterTextBox.Text == null)
            {
                equipmentsDataTable = database.Query(String.Format(
                @"SELECT * FROM equipmentsOfEmployee({0})", Singleton.EMPLOYEE.Id));
                FromEmployeesEquipmentsDataGrid.DataContext = equipmentsDataTable.DefaultView;

            }
            else
            {
                string[] filterList = new string[] { "Тип", "Характеристики", "Серийный номер" };

                if ((bool)TypeRadioButton.IsChecked)
                {
                    FromEmployeesEquipmentsDataGrid.DataContext = database.Query(String.Format(
                        @"SELECT * FROM equipmentsOfEmployee({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')",
                        Singleton.EMPLOYEE.Id, filterList[0], EquipmentsFromFilterTextBox.Text))
                        .DefaultView;
                }
                else if ((bool)CharsRadioButton.IsChecked)
                {
                    FromEmployeesEquipmentsDataGrid.DataContext = database.Query(String.Format(
                        @"SELECT * FROM equipmentsOfEmployee({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')",
                        Singleton.EMPLOYEE.Id, filterList[1], EquipmentsFromFilterTextBox.Text))
                        .DefaultView;
                }
                else
                {
                    FromEmployeesEquipmentsDataGrid.DataContext = database.Query(String.Format(
                        @"SELECT * FROM equipmentsOfEmployee({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')",
                        Singleton.EMPLOYEE.Id, filterList[2], EquipmentsFromFilterTextBox.Text))
                        .DefaultView;
                }
            }
        }

        private void EmployeeDataGridFiltartion(DataGrid dataGrid, DataTable dataTable, TextBox textBox)
        {
            if (textBox == null)
            {
                dataTable = database.Query(String.Format(
                @"SELECT id, concat(lastname, ' ', SUBSTRING(firstname FROM 1 FOR 1), '. ', SUBSTRING(patronymic FROM 1 FOR 1), '.') AS ""Сотрудник"", position AS ""Должность"", eq_count AS ""Кол-во""
                  FROM employee_info_for_desktop
                  WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));

                dataGrid.DataContext = dataTable.DefaultView;

            }
            else
            {
                dataTable = database.Query(String.Format(
                @"SELECT id, concat(lastname, ' ', SUNSTRING(firstname FROM 1 FOR 1), '. ', SUNSTRING(patronymic FROM 1 FOR 1), '.') AS ""Сотрудник"", position AS ""Должность"", eq_count AS ""Кол-во""
                  FROM employee_info_for_desktop
                  WHERE dep_id = {0} AND firstname LIKE '%{1}%' OR dep_id = {0} AND lastname LIKE '%{1}%'", Singleton.MANAGER.Dep_id, textBox.Text));

                dataGrid.DataContext = dataTable.DefaultView;
            }
        }

        private void ToEmployeeDataGridFiltartion(object sender, KeyEventArgs e)
        {
            EmployeeDataGridFiltartion(ToEmployeeDataGrid, toEmployeesDataTable, EmployeeToFiltartion);
        }

        private void FromEmployeeDataGridFiltartion(object sender, KeyEventArgs e)
        {
            EmployeeDataGridFiltartion(FromEmployeesDataGrid, fromEmployeesDataTable, EmployeeFromFilterTextBox);
        }

        private void EquipmentsClearMouseDown(object sender, MouseButtonEventArgs e)
        {
            EquipmentsNameLabel.Content = "";

            equipmentsExplIdList.Clear();
            equipmentsIdList.Clear();

            HandOverButton.IsEnabled = false;

            equipmentsDataTable = database.Query(String.Format(
                @"SELECT id, eq_id AS ""ОборудованиеID"", type_name AS ""Тип"", characteristics AS ""Характеристики"", serial_num AS ""Серийный номер"", to_char(date_issue, 'dd.MM.yyyy') as ""Дата выдачи""
                  FROM equipments eq
                  WHERE em_id = {0} and passed is null", Singleton.EMPLOYEE.Id));

            FromEmployeesEquipmentsDataGrid.DataContext = equipmentsDataTable.DefaultView;
        }

        private void EmployeeClearMouseDown(object sender, MouseButtonEventArgs e)
        {
            EmployeeNameLabel.Content = "[Наименование сотрудника]";

            HandOverButton.IsEnabled = false;
        }
    }
}

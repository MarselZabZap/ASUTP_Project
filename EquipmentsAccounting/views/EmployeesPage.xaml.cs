using EquipmentsAccounting.database;
using EquipmentsAccounting.models;
using EquipmentsAccounting.windows;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentsAccounting.views
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        readonly Database database;
        DataTable dataTable;



        public EmployeesPage()
        {
            InitializeComponent();

            database = new Database();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void DataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            // Пересенная хранящая ID устройства при выборе
            var id = EmployeesInfoDataGrid.Columns[0].GetCellContent(EmployeesInfoDataGrid.Items[EmployeesInfoDataGrid.SelectedIndex]) as TextBlock;
            var name = EmployeesInfoDataGrid.Columns[1].GetCellContent(EmployeesInfoDataGrid.Items[EmployeesInfoDataGrid.SelectedIndex]) as TextBlock;

            Singleton.EMPLOYEE = new Employee(Int16.Parse(id.Text), name.Text);

            EquipmentsInfoDataGrid.DataContext = database.Query(String.Format(
                @"SELECT id, eq_id AS ""ОборудованиеID"", type_name AS ""Тип"", characteristics AS ""Характеристики"", serial_num AS ""Серийный номер"", to_char(date_issue, 'dd.MM.yyyy') as ""Дата выдачи""
                  FROM equipments eq
                WHERE em_id = {0} and passed is null", Singleton.EMPLOYEE.Id))
                .DefaultView;

            //Обновление таблицы сотрудников
            //LoadData();
        }

        private void LoadData()
        {
            dataTable = database.Query(String.Format(
                @"SELECT id, concat(firstname, ' ', lastname) AS ""Сотрудник"", eq_count AS ""Кол-во""
                  FROM employee_info_for_desktop
                  WHERE dep_id = {0}", Singleton.MANAGER.Dep_id));
            EmployeesInfoDataGrid.DataContext = dataTable.DefaultView;
        }

        private void OpenIssueWindow(object sender, RoutedEventArgs e)
        {

            if (Singleton.EMPLOYEE != null)
            {
                IssueEquipmentWindow window = new IssueEquipmentWindow(this);
                window.ShowDialog();
            }
        }

        private void OpenWriteOfWindow(object sender, RoutedEventArgs e)
        {
            if (Singleton.EMPLOYEE != null)
            {
                WriteOfFromEmployeeWindow window = new WriteOfFromEmployeeWindow(this);
                window.ShowDialog();
            }
        }

        private void OpenHandOverWindow(object sender, RoutedEventArgs e)
        {
            HandOverWindow window = new HandOverWindow(this);
            window.ShowDialog();
        }

        private void Filtration(object sender, KeyEventArgs e)
        {
            if (FilterTextBox.Text.Equals(""))
            {
                LoadData();
            }
            else
            {
                dataTable = database.Query(String.Format(
                @"SELECT id, concat(firstname, ' ', lastname) AS ""Сотрудник"", eq_count AS ""Кол-во""
                  FROM employee_info_for_desktop
                  WHERE dep_id = {0} AND lower(firstname) LIKE lower('%{1}%') OR lower(lastname) LIKE lower('%{1}%')", Singleton.MANAGER.Dep_id, FilterTextBox.Text));
                EmployeesInfoDataGrid.DataContext = dataTable.DefaultView;
            }
        }

        public DataTable EmployeeInfoDGValue
        {
            get { return dataTable; }
            set
            { 
                dataTable = value;
                EmployeesInfoDataGrid.DataContext = dataTable.DefaultView;

                EquipmentsInfoDataGrid.DataContext = database.Query(String.Format(@"SELECT id, eq_id AS ""ОборудованиеID"", type_name AS ""Тип"", characteristics AS ""Характеристики"", serial_num AS ""Серийный номер"", to_char(date_issue, 'dd.MM.yyyy') as ""Дата выдачи""
                                                                                    FROM equipments eq WHERE em_id = {0} and passed is null", Singleton.EMPLOYEE.Id)).DefaultView;
            }
        }
    }
}

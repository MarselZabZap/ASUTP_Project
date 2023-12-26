using EquipmentsAccounting.database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EquipmentsAccounting.windows
{
    /// <summary>
    /// Логика взаимодействия для WriteOfEquipmentsWindow.xaml
    /// </summary>
    public partial class WriteOfEquipmentsWindow : Window
    {

        Database database;
        DataTable equipmentsOfSelectedEmployee;
        DataTable selectedEquipmentsDataTable;

        public WriteOfEquipmentsWindow()
        {
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

            dataSet.Tables.Add(selectedEquipmentsDataTable);
        }

        private void DataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            //Создание строки   
            DataRow dataRow = selectedEquipmentsDataTable.NewRow();


            //Заполнение таблицы строками
            var id = EquipmentsOfSelectedEmployeeDataGrid.Columns[0].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "id", id.Text);

            var eq_id = EquipmentsOfSelectedEmployeeDataGrid.Columns[1].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "eq_id", id.Text);

            var type = EquipmentsOfSelectedEmployeeDataGrid.Columns[2].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Тип", type.Text);

            var cahrs = EquipmentsOfSelectedEmployeeDataGrid.Columns[3].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Характеристики", cahrs.Text);

            var serialNum = EquipmentsOfSelectedEmployeeDataGrid.Columns[4].GetCellContent(EquipmentsOfSelectedEmployeeDataGrid.Items[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(selectedEquipmentsDataTable, dataRow, "Серийный номер", serialNum.Text);

            SelectedEquipmentsDataGrid.DataContext = selectedEquipmentsDataTable.DefaultView;


            DataRow rowForRemove = equipmentsOfSelectedEmployee.Rows[EquipmentsOfSelectedEmployeeDataGrid.SelectedIndex];
            equipmentsOfSelectedEmployee.Rows.Remove(rowForRemove);
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
            DataColumn dataColumn = new DataColumn();

            dataColumn.DataType = System.Type.GetType("System.String"); //Задать тип столбца
            dataColumn.ColumnName = columnName; //Имя стольца
            dataColumn.AutoIncrement = false; //автоинкремент
            dataColumn.Caption = caption; //Подпись столбца
            dataColumn.ReadOnly = false; //Параметор "только для чтения"
            dataColumn.Unique = false; //Уникальность

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
            DataRow row = null;

            if (selectedEquipmentsDataTable.Rows.Count > 0)
            {
                for (int i = -1; i < selectedEquipmentsDataTable.Rows.Count; i++)
                {
                    id = (SelectedEquipmentsDataGrid.Columns[0].GetCellContent(SelectedEquipmentsDataGrid.Items[0]) as TextBlock).Text;
                    database.Query(String.Format(@"CALL write_of_eq({0});", Int16.Parse(id)));

                    row = selectedEquipmentsDataTable.Rows[0];
                    selectedEquipmentsDataTable.Rows.Remove(row);
                }
            }
        }
    }
}

using EquipmentsAccounting.database;
using EquipmentsAccounting.models;
using EquipmentsAccounting.views;
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
    /// Логика взаимодействия для StockTramsferWindow.xaml
    /// </summary>
    public partial class StockTramsferWindow : Window
    {
        StockPage stockPage;
        Database database;
        List<Departament> departaments = new List<Departament>();
        int senderId, recevierId = -1;
        DataTable senderEquipmentsDataTable, recevierEquipmentsDataTabel;

        public StockTramsferWindow(StockPage stockPage)
        {
            InitializeComponent();
            stockPage = this.stockPage;

            database = new Database();

            InitSenderChoiceBox();

            recevierEquipmentsDataTabel = new DataTable();
            DataSet dataSet = new DataSet();

            recevierEquipmentsDataTabel.Columns.Add(CreateColumn("id", "id"));
            recevierEquipmentsDataTabel.Columns.Add(CreateColumn("Тип", "Тип"));
            recevierEquipmentsDataTabel.Columns.Add(CreateColumn("Характеристики", "Характеристики"));
            recevierEquipmentsDataTabel.Columns.Add(CreateColumn("Серийный номер", "Серийный номер"));
            recevierEquipmentsDataTabel.Columns.Add(CreateColumn("Статус", "Статус"));

            dataSet.Tables.Add(recevierEquipmentsDataTabel);
        }

        private void InitSenderChoiceBox()
        {
            departaments = database.getDepartaments();
            for (int i = 0; i < departaments.Count; i++)
            {
                SenderComboBox.Items.Add(departaments[i].name);
            }
        }

        private void Filtration(object sender, KeyEventArgs e)
        {
            if (senderId != -1)
            {
                if (FilterTextBox.Text == null)
                {
                    SendersEquipmentDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                AND ""Статус"" = 'На складе'
                                ORDER BY id", Singleton.MANAGER.Dep_id)).DefaultView;

                }
                else
                {
                    string[] filterList = new string[] { "Тип", "Характеристики", "Серийный номер" };

                    if ((bool)TypeRadioButton.IsChecked)
                    {
                        SendersEquipmentDataGrid.DataContext = database.Query(MainQuery(filterList[0], FilterTextBox.Text)).DefaultView;
                    }
                    else if ((bool)CharsRadioButton.IsChecked)
                    {
                        SendersEquipmentDataGrid.DataContext = database.Query(MainQuery(filterList[1], FilterTextBox.Text)).DefaultView;
                    }
                    else
                    {
                        SendersEquipmentDataGrid.DataContext = database.Query(MainQuery(filterList[2], FilterTextBox.Text)).DefaultView;
                    }
                }
            }
        }

        private void SendersEquipmentDataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            //Создание строки   
            DataRow dataRow = recevierEquipmentsDataTabel.NewRow();


            //Заполнение таблицы строками
            var id = SendersEquipmentDataGrid.Columns[0].GetCellContent(SendersEquipmentDataGrid.Items[SendersEquipmentDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(recevierEquipmentsDataTabel, dataRow, "id", id.Text);

            var type = SendersEquipmentDataGrid.Columns[1].GetCellContent(SendersEquipmentDataGrid.Items[SendersEquipmentDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(recevierEquipmentsDataTabel, dataRow, "Тип", type.Text);

            var cahrs = SendersEquipmentDataGrid.Columns[2].GetCellContent(SendersEquipmentDataGrid.Items[SendersEquipmentDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(recevierEquipmentsDataTabel, dataRow, "Характеристики", cahrs.Text);

            var serialNum = SendersEquipmentDataGrid.Columns[3].GetCellContent(SendersEquipmentDataGrid.Items[SendersEquipmentDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(recevierEquipmentsDataTabel, dataRow, "Серийный номер", serialNum.Text);

            var status = SendersEquipmentDataGrid.Columns[4].GetCellContent(SendersEquipmentDataGrid.Items[SendersEquipmentDataGrid.SelectedIndex]) as TextBlock;
            CreateRow(recevierEquipmentsDataTabel, dataRow, "Статус", status.Text);

            RecevierEquipmentDataGrid.DataContext = recevierEquipmentsDataTabel.DefaultView;

            //Удаление строки
            DataRow rowForRemove = senderEquipmentsDataTable.Rows[SendersEquipmentDataGrid.SelectedIndex];
            senderEquipmentsDataTable.Rows.Remove(rowForRemove);
        }

        private void RecevierEquipmentDataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            // Код для удаления строк и нижней таблицы
            DataRow newRow = senderEquipmentsDataTable.NewRow();

            //Заполнение таблицы строками
            var id = RecevierEquipmentDataGrid.Columns[0].GetCellContent(RecevierEquipmentDataGrid.Items[RecevierEquipmentDataGrid.SelectedIndex]) as TextBlock;
            newRow["id"] = id.Text;

            var type = RecevierEquipmentDataGrid.Columns[1].GetCellContent(RecevierEquipmentDataGrid.Items[RecevierEquipmentDataGrid.SelectedIndex]) as TextBlock;
            newRow["Тип"] = type.Text;

            var cahrs = RecevierEquipmentDataGrid.Columns[2].GetCellContent(RecevierEquipmentDataGrid.Items[RecevierEquipmentDataGrid.SelectedIndex]) as TextBlock;
            newRow["Характеристики"] = cahrs.Text;

            var serialNum = RecevierEquipmentDataGrid.Columns[3].GetCellContent(RecevierEquipmentDataGrid.Items[RecevierEquipmentDataGrid.SelectedIndex]) as TextBlock;
            newRow["Серийный номер"] = serialNum.Text;

            var status = RecevierEquipmentDataGrid.Columns[4].GetCellContent(RecevierEquipmentDataGrid.Items[RecevierEquipmentDataGrid.SelectedIndex]) as TextBlock;
            newRow["Статус"] = status.Text;

            senderEquipmentsDataTable.Rows.Add(newRow);
            SendersEquipmentDataGrid.DataContext = senderEquipmentsDataTable.DefaultView;

            //Удаление строки
            DataRow rowForRemove = recevierEquipmentsDataTabel.Rows[RecevierEquipmentDataGrid.SelectedIndex];
            recevierEquipmentsDataTabel.Rows.Remove(rowForRemove);
        }

        private void SenderIsChange(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < departaments.Count; i++)
            {
                if (departaments[i].name.Equals(SenderComboBox.SelectedValue))
                {
                    String selctedName = departaments[i].name;
                    senderId = departaments[i].id;
                    senderEquipmentsDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                AND ""Статус"" = 'На складе'
                                ORDER BY id", senderId));
                    SendersEquipmentDataGrid.DataContext = senderEquipmentsDataTable.DefaultView;

                    recevierEquipmentsDataTabel.Rows.Clear();

                    RecevierComboBox.Items.Clear();
                    for (int j = 0; j < departaments.Count; j++)
                    {
                        if (!departaments[j].name.Equals(selctedName))
                        {
                            RecevierComboBox.Items.Add(departaments[j].name);
                        }
                    }

                    RecevierComboBox.IsEnabled = true;

                    break;
                }
            }
        }

        private void RecevierIsChange(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ClearRecevierTableClick(object sender, MouseButtonEventArgs e)
        {

        }

        private string MainQuery(string filterType, string filterText)
        {
            return String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе' AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", senderId, filterType, filterText);
        }
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
    }
}

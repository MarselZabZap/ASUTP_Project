using EquipmentsAccounting.database;
using EquipmentsAccounting.views;
using EquipmentsAccounting.windows;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentsAccounting.view
{
    /// <summary>
    /// Логика взаимодействия для DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {

        readonly Database database;
        DataTable equipmentsInfoDT;

        public DataPage()
        {
            InitializeComponent();


            database = new Database();

        }

        //Первичная загрузка данных в таблицу
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UploadData();
        }

        private void UploadData()
        {
            equipmentsInfoDT = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0})", Singleton.MANAGER.Dep_id)); // Загрузка таблицы из БД
            EquipmentsInfoDataGrid.DataContext = equipmentsInfoDT.DefaultView;
        }

        //Фильрация таблицы
        private void Filtration(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(FilterTextBlock.Text);

            string[] filterList = new string[] { "Тип", "Характеристики", "Серийный номер" };

            if (FilterTextBox.Text == null)
            {
                EquipmentsInfoDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                ORDER BY id", Singleton.MANAGER.Dep_id)).DefaultView;

            }
            else
            {
                if ((bool)TypeRadioButton.IsChecked)
                {
                    EquipmentsInfoDataGrid.DataContext = database.Query(String.Format(
                        @"SELECT * FROM loc_eq_acc_info({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')", Singleton.MANAGER.Dep_id, filterList[0], FilterTextBox.Text))
                        .DefaultView;
                }
                else if ((bool)CharsRadioButton.IsChecked)
                {
                    EquipmentsInfoDataGrid.DataContext = database.Query(String.Format(
                        @"SELECT * FROM loc_eq_acc_info({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')", Singleton.MANAGER.Dep_id, filterList[1], FilterTextBox.Text))
                        .DefaultView;
                }
                else
                {
                    EquipmentsInfoDataGrid.DataContext = database.Query(String.Format(
                        @"SELECT * FROM loc_eq_acc_info({0}) WHERE lower(""{1}"") LIKE lower('%{2}%')", Singleton.MANAGER.Dep_id, filterList[2], FilterTextBox.Text))
                        .DefaultView;
                }
            }
        }


        //Выгразука персональных данных оборудования в таблицу снизу
        private void DataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            //Тип устройства
            var type = EquipmentsInfoDataGrid.Columns[1].GetCellContent(EquipmentsInfoDataGrid.Items[EquipmentsInfoDataGrid.SelectedIndex]) as TextBlock;
            // Переменная хранящая ID устройства при выборе
            var id = EquipmentsInfoDataGrid.Columns[0].GetCellContent(EquipmentsInfoDataGrid.Items[EquipmentsInfoDataGrid.SelectedIndex]) as TextBlock;

            Singleton.EQ_ID = Int16.Parse(id.Text);
            Singleton.EQUIPMENS_INFO = database.GetEquipmentsInfoById(Singleton.EQ_ID);

            //FilterTextBox.Text = Singleton.EQ_ID.ToString();

            if (type.Text.Equals("Компьютер"))
            {
                //Загрузка таблиц
                EquipmentsCharsDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM get_computers_components({0})", Singleton.EQ_ID)).DefaultView;
                EquipmentsFixDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_service_info({0})", Singleton.EQ_ID)).DefaultView;
                EquipmentsMoveDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_move_info({0})", Singleton.EQ_ID)).DefaultView;

                EditButton.IsEnabled = true;

            }
            else
            {
                EquipmentsCharsDataGrid.DataContext = null;
                EquipmentsFixDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_service_info({0})", Singleton.EQ_ID)).DefaultView;
                EquipmentsMoveDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_move_info({0})", Singleton.EQ_ID)).DefaultView;

                EditButton.IsEnabled = false;
            }

            EquipmentMaintenanceButton.IsEnabled = true;

            WriteOffButton.IsEnabled = true;
        }

        private void EditEquipmentButtonClick(object sender, RoutedEventArgs e)
        {
            if (Singleton.EQ_ID != 0)
            {
                EditingEquipment window = new EditingEquipment(this);
                window.ShowDialog();
            }
        }

        private void AddEquipmentButtonClick(object sender, RoutedEventArgs e)
        {
             AddEquipmentWindow window = new AddEquipmentWindow(this);
             window.ShowDialog();
        }
        private void EquipmentMaintenanceButtonClick(object sender, RoutedEventArgs e)
        {
            if (Singleton.EQ_ID != 0)
            {
                EquipmentMaintenanceWindow window = new EquipmentMaintenanceWindow(this);
                window.ShowDialog();
            }
        }

        private void WriteOffButtonClick(object sender, RoutedEventArgs e)
        {
            if (Singleton.EQ_ID != 0)
            {
                WriteOffEquipmentWindow window = new WriteOffEquipmentWindow(this);
                window.ShowDialog();
            }
        }

        public DataTable EquipmentsInfoDGValue
        {
            get { return equipmentsInfoDT; }
            set { equipmentsInfoDT = value; EquipmentsInfoDataGrid.DataContext = equipmentsInfoDT.DefaultView; }
        }

        private void DataGridCharsFocused(object sender, MouseButtonEventArgs e)
        {

        }

        private void DataGridFixFocused(object sender, MouseButtonEventArgs e)
        {

        }

        private void DataGridMoveFocused(object sender, MouseButtonEventArgs e)
        {

        }

        private void ReportSettingIconMouseDown(object sender, MouseButtonEventArgs e)
        {
            ReportSettingWindow reportSettingWindow = new ReportSettingWindow();
            reportSettingWindow.ShowDialog();
        }
    }
}

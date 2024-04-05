using EquipmentsAccounting.database;
using EquipmentsAccounting.Excel;
using EquipmentsAccounting.models;
using EquipmentsAccounting.windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EquipmentsAccounting.views
{
    /// <summary>
    /// Логика взаимодействия для StockPage.xaml
    /// </summary>
    public partial class StockPage : Page
    {

        Database database;
        DataTable equipmentDataTable;
        ExcelHelper excelHelper;
        List<Departament> departaments = new List<Departament>();
        int depId;

        public StockPage()
        {
            InitializeComponent();

            database = new Database();
            excelHelper = new ExcelHelper();
            InitSenderChoiceBox();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            depId = Singleton.MANAGER.Dep_id;
            equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                AND ""Статус"" = 'На складе'
                                ORDER BY id", depId));
            StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
        }

        private void EquipmentsDataGridFiltartion(object sender, KeyEventArgs e)
        {
            if (FilterTextBox.Text == null)
            {
                equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                AND ""Статус"" = 'На складе'
                                ORDER BY id", depId));
                StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;

            }
            else
            {
                string[] filterList = new string[] { "Тип", "Характеристики", "Серийный номер" };

                if ((bool)TypeRadioButton.IsChecked)
                {
                    equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", depId, filterList[0], FilterTextBox.Text));
                    StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
                }
                else if ((bool)CharsRadioButton.IsChecked)
                {
                    equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", depId, filterList[1], FilterTextBox.Text));
                    StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
                }
                else
                {
                    equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", depId, filterList[2], FilterTextBox.Text));
                    StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
                }
            }
        }

        private void OpenTransferWindowButtonClick(object sender, RoutedEventArgs e)
        {
            StockTramsferWindow window = new StockTramsferWindow(this);
            window.ShowDialog();
        }

        private void CreateStockReportMouseClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                excelHelper.CreateStockReport();

                string messageBoxText = "Документ создан";
                string messageBoxTitle = "Выполнено";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.None;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, messageBoxTitle, button, icon, MessageBoxResult.Yes);
            }
            catch (Exception ex)
            {
                string messageBoxText = ex.Message;
                string messageBoxTitle = "Ошибка";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, messageBoxTitle, button, icon, MessageBoxResult.Yes);
            }
        }
        private void InitSenderChoiceBox()
        {
            departaments = database.getDepartaments();

            for (int i = 0; i < departaments.Count; i++)
            {
                StocksComboBox.Items.Add(departaments[i].name);

            }
        }

        private void StocksComboBoxIsChange(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < departaments.Count; i++)
            {
                if (departaments[i].name.Equals(StocksComboBox.SelectedValue))
                {
                    depId = departaments[i].id;
                    break;
                }
            }
            equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                AND ""Статус"" = 'На складе'
                                ORDER BY id", depId));
            StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
        }
    }
}

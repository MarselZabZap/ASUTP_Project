using EquipmentsAccounting.database;
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

        public StockPage()
        {
            InitializeComponent();

            database = new Database();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null)
                                AND ""Статус"" = 'На складе'
                                ORDER BY id", Singleton.MANAGER.Dep_id));
            StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
        }

        private void EquipmentsDataGridFiltartion(object sender, KeyEventArgs e)
        {
            if (FilterTextBox.Text == null)
            {
                equipmentDataTable = database.Query(String.Format(
                @"SELECT * FROM equipmentsOfEmployee({0})", Singleton.EMPLOYEE.Id));
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
                                ORDER BY id", Singleton.MANAGER.Dep_id, filterList[0], FilterTextBox.Text));
                    StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
                }
                else if ((bool)CharsRadioButton.IsChecked)
                {
                    equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", Singleton.MANAGER.Dep_id, filterList[1], FilterTextBox.Text));
                    StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
                }
                else
                {
                    equipmentDataTable = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0}) eq
                                WHERE NOT EXISTS (SELECT 1 FROM eq_expl expl WHERE expl.eq_id = eq.id AND expl.passed is null) 
                                    AND ""Статус"" = 'На складе'
                                    AND lower(""{1}"") LIKE lower('%{2}%')
                                ORDER BY id", Singleton.MANAGER.Dep_id, filterList[2], FilterTextBox.Text));
                    StockInfoDataGrid.DataContext = equipmentDataTable.DefaultView;
                }
            }
        }
    }
}

using EquipmentsAccounting.view;
using EquipmentsAccounting.views;
using System.Windows;

namespace EquipmentsAccounting.window
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            RootWindow.Title = "Учёт оборудования (" + Singleton.MANAGER.Dep_name + ')';

            EquipmentsInfoFrame.Navigate(new DataPage());
            StockFrame.Navigate(new StockPage());
            EmployeesInfoFrame.Navigate(new EmployeesPage());
            EquipmentsMaintenanceFrame.Navigate(new EquipmentsMaintenancePage());
        }
    }
}

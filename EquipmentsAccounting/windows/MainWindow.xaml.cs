using EquipmentsAccounting.view;
using EquipmentsAccounting.views;
using System;
using System.Diagnostics;
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

        private void Open1C(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Process proc = new Process();
                proc.StartInfo.FileName = ""; // Путь к exe-шнику
                proc.Start();
            }
            catch (Exception ex)
            {
                string messageBoxText = ex.ToString();
                string messageBoxTitle = "Ошибка пути";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, messageBoxTitle, button, icon, MessageBoxResult.Yes);
            }
        }
    }
}

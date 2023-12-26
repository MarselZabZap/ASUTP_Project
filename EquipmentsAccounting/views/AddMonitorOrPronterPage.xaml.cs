using EquipmentsAccounting.database;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace EquipmentsAccounting.views
{
    /// <summary>
    /// Логика взаимодействия для AddMonitorOrPronterPage.xaml
    /// </summary>
    public partial class AddMonitorOrPronterPage : Page
    {
        Database database;
        public AddMonitorOrPronterPage()
        {
            InitializeComponent();

            database = new Database();
        }

        private void AddComputerButtonClick(object sender, RoutedEventArgs e)
        {
            if (SerialNumberTextBox.Text.Equals(""))
            {
                string messageBoxText = "Все поля должны быть заполненными!";
                string messageBoxTitle = "Пустые поля";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, messageBoxTitle, button, icon, MessageBoxResult.Yes);
            } else
            {
                database.AddOtherEquipments(SerialNumberTextBox.Text, Singleton.GEN_EQUIPMENT.Id, Singleton.MANAGER.Dep_id);

                Notification.Visibility = Visibility.Visible;

                // Создание таймера
                DispatcherTimer timer = new DispatcherTimer();
                // Объявление метода
                timer.Tick += new EventHandler(timerTick);
                timer.Interval = new TimeSpan(0, 0, 3);
                timer.Start();
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            Notification.Visibility = Visibility.Collapsed;
        }
    }
}

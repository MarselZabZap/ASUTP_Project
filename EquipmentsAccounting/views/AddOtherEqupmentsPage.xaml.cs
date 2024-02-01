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
    /// Логика взаимодействия для AddOtherEqupmentsPage.xaml
    /// </summary>
    public partial class AddOtherEqupmentsPage : Page
    {
        readonly Database database;
        public AddOtherEqupmentsPage()
        {
            InitializeComponent();

            database = new Database();
        }

        private void AddComputerButtonClick(object sender, RoutedEventArgs e)
        {
            if (CountTextBox.Text.Equals(""))
            {
                string messageBoxText = "Укажите количество";
                string messageBoxTitle = "Пустые поля";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, messageBoxTitle, button, icon, MessageBoxResult.Yes);
            } else
            {
                database.AddOtherEquipments(Int32.Parse(CountTextBox.Text), Singleton.GEN_EQUIPMENT.Id, Singleton.MANAGER.Dep_id);

                Notification.Visibility = Visibility.Visible;

                // Создание таймера
                DispatcherTimer timer = new DispatcherTimer();
                // Объявление метода
                timer.Tick += new EventHandler(TimerTick);
                timer.Interval = new TimeSpan(0, 0, 3);
                timer.Start();
            }
        }

        // Метод используемый по истечению времени таймера
        private void TimerTick(object sender, EventArgs e)
        {
            Notification.Visibility = Visibility.Collapsed;
        }
    }
}

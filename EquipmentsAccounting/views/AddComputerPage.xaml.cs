using EquipmentsAccounting.database;
using EquipmentsAccounting.view;
using EquipmentsAccounting.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EquipmentsAccounting.views
{
    /// <summary>
    /// Логика взаимодействия для AddComputerPage.xaml
    /// </summary>
    public partial class AddComputerPage : Page
    {
        readonly Database database;
        private DataPage dataPage;

        public AddComputerPage(DataPage dataPage)
        {
            this.dataPage = dataPage;
            InitializeComponent();

            database = new Database();
        }

        private void AddComputerButtonClick(object sender, RoutedEventArgs e)
        {
            if (SerialNumberTextBox.Text.Equals("") || CpuTextBox.Text.Equals("") || MotherboardTextBox.Text.Equals("") ||
                VideocardTextBox.Text.Equals("") || RamTextBox.Text.Equals("") || HddTextBox.Text.Equals("") || SSDTextBox.Text.Equals("") || OsTextBox.Text.Equals("") ||
                IpTextBox.Text.Equals(""))
            {
                string messageBoxText = "Все поля должны быть заполненными!";
                string messageBoxTitle = "Пустые поля";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, messageBoxTitle, button, icon, MessageBoxResult.Yes);
            }
            else
            {
                database.AddNewComputer(Singleton.GEN_EQUIPMENT.Id, SerialNumberTextBox.Text, Singleton.MANAGER.Dep_id, CpuTextBox.Text, MotherboardTextBox.Text,
                    VideocardTextBox.Text, Int32.Parse(RamTextBox.Text), Int32.Parse(HddTextBox.Text), Int32.Parse(SSDTextBox.Text), OsTextBox.Text, IpTextBox.Text);

                SerialNumberTextBox.Text = "";
                CpuTextBox.Text = "";
                MotherboardTextBox.Text = "";
                VideocardTextBox.Text = "";
                RamTextBox.Text = "";
                HddTextBox.Text = "";
                SSDTextBox.Text = "";
                OsTextBox.Text = "";
                IpTextBox.Text = "";

                Notification.Visibility  = Visibility.Visible;

                dataPage.EquipmentsInfoDGValue = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0})", Singleton.MANAGER.Dep_id));

                // Вспылвающее окно "Успешно"
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(TimerTick);
                timer.Interval = new TimeSpan(0,0,3);
                timer.Start();
            }

        }

        // Метод, работающий до истечения таймера
        private void TimerTick(object sender, EventArgs e)
        {
            Notification.Visibility = Visibility.Collapsed;
        }
    }
}

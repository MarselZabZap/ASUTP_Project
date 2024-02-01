using EquipmentsAccounting.database;
using EquipmentsAccounting.models;
using EquipmentsAccounting.view;
using EquipmentsAccounting.views;
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
using System.Windows.Shapes;

namespace EquipmentsAccounting.windows
{
    /// <summary>
    /// Логика взаимодействия для AddEquipmentWindow.xaml
    /// </summary>
    public partial class AddEquipmentWindow : Window
    {

        readonly Database database;
        List<GenEquipmentsAcc> genEquipments;
        DataPage dataPage;
        //int id;

        public AddEquipmentWindow(DataPage dataPage)
        {
            this.dataPage = dataPage;   
            InitializeComponent();

            database = new Database();

            genEquipments = database.GetGenEquipmentsAccList();
            for (int i = 0; i < genEquipments.Count; i++)
            {
                EquipmentsNameComboBox.Items.Add(genEquipments[i].Name);
            }
        }

        private void SetType(object sender, SelectionChangedEventArgs e)
        {
            string type = "[тип не найден!]";
            for (int i = 0; i < genEquipments.Count; i++)
            {
                if (genEquipments[i].Name.Equals(EquipmentsNameComboBox.SelectedValue))
                {
                    type = genEquipments[i].TypeName;
                    Singleton.GEN_EQUIPMENT = genEquipments[i];
                    break;
                }
            }

            EquipmentsTypeLabel.Content = type;
            if (type.Equals("Компьютер"))
            {
                AddComputerFrame.Navigate(new AddComputerPage(dataPage)); 
            }
            else if (type.Equals("Монитор") || type.Equals("Принтер"))
            {
                AddComputerFrame.Navigate(new AddMonitorOrPronterPage());
            }
            else
            {
                AddComputerFrame.Navigate (new AddOtherEqupmentsPage());
            }
        }

        private void ClearFrame(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            AddComputerFrame.NavigationService.RemoveBackEntry();
        }

        /*private void AddComputerButtonClick(object sender, RoutedEventArgs e)
        {
            if (EquipmentsNameComboBox.SelectedValue == null || SerialNumberTextBox.Text.Equals("") || CpuTextBox.Text.Equals("") || MotherboardTextBox.Text.Equals("") ||
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
                database.AddNewComputer(genEquipments[id].Id, SerialNumberTextBox.Text, Singleton.MANAGER.Dep_id, CpuTextBox.Text, MotherboardTextBox.Text, VideocardTextBox.Text,
                    Int32.Parse(RamTextBox.Text), Int32.Parse(HddTextBox.Text), Int32.Parse(SSDTextBox.Text), OsTextBox.Text, IpTextBox.Text);
                Close();
            }
        }

        private void CancelAddButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }*/
    }
}

using EquipmentsAccounting.database;
using EquipmentsAccounting.models;
using EquipmentsAccounting.view;
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
    /// Логика взаимодействия для EditingEquipment.xaml
    /// </summary>
    public partial class EditingEquipment : Window
    {
        Database database;
        EquipmentsInfo equipmentsInfo;
        ComputerComponents computerComponents;
        private DataPage dataPage;

        public EditingEquipment(DataPage dataPage)
        {
            InitializeComponent();
            this.dataPage = dataPage;

            database = new Database();
            equipmentsInfo = database.GetEquipmentsInfoById(Singleton.EQ_ID);
            computerComponents = database.GetComputerComponentsById(Singleton.EQ_ID);

            LoadData();
        }

        private void LoadData()
        {
            //Шапка
            EquipmentsName.Content = equipmentsInfo.Type + " " + equipmentsInfo.Characteristics;
            SerialNumberTextBox.Text = equipmentsInfo.SerialNum;

            //Тело
            CpuTextBox.Text = computerComponents.CPU;
            MotherboardTextBox.Text = computerComponents.Motherboard;
            VideocardTextBox.Text = computerComponents.Videocard;
            RamTextBox.Text = computerComponents.RAM.ToString();
            HddTextBox.Text = computerComponents.HDD.ToString();
            SSDTextBox.Text = computerComponents.SSD.ToString();

            List<Provider> providers = database.GetProviders();
            for (int i = 0; i < providers.Count; i++)
            {
                ProviderComboBox.Items.Add(providers[i].Name);
            }
            ProviderComboBox.SelectedValue = equipmentsInfo.Provider;

            DatePurchaseTextBox.Text = equipmentsInfo.DatePurchase;
            WarrantyLabel.Content = equipmentsInfo.Warranty;
            PriceTextBox.Text = equipmentsInfo.Price.ToString();
            OsTextBox.Text = computerComponents.OperationSystem;
            IpTextBox.Text = computerComponents.IpAdderss;
        }

        private void SaveChangesButtonClick(object sender, RoutedEventArgs e)
        {
            database.UpdateComputerData(equipmentsInfo.LocId, equipmentsInfo.GenId, SerialNumberTextBox.Text, ProviderComboBox.Text, DateTime.Parse(DatePurchaseTextBox.Text),
                Int32.Parse(PriceTextBox.Text), CpuTextBox.Text, MotherboardTextBox.Text, VideocardTextBox.Text, Int32.Parse(RamTextBox.Text),
                Int32.Parse(HddTextBox.Text), Int32.Parse(SSDTextBox.Text), OsTextBox.Text);

            if (!IpTextBox.Text.Equals(computerComponents.IpAdderss))
            {
                database.UpdateIpAddress(equipmentsInfo.GenId, IpTextBox.Text);
            }

            dataPage.EquipmentsInfoDGValue = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0})", Singleton.MANAGER.Dep_id));

            Close();
        }

        private void CancelChangesButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

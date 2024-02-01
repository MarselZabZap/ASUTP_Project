using EquipmentsAccounting.database;
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
using Color = System.Drawing;

namespace EquipmentsAccounting.windows
{
    /// <summary>
    /// Логика взаимодействия для EquipmentMaintenanceWindow.xaml
    /// </summary>
    public partial class EquipmentMaintenanceWindow : Window
    {
        readonly Database database;
        DataPage dataPage;

        public EquipmentMaintenanceWindow(DataPage dataPage)
        {
            this.dataPage = dataPage;
            InitializeComponent();

            database = new Database();

            CommentsTextBox.Text = "Суть обслуживания";
            CommentsTextBox.Foreground = Brushes.Gray;
            EquipmentsName.Text = Singleton.EQUIPMENS_INFO.Type + ' ' + Singleton.EQUIPMENS_INFO.Characteristics;
        }

        private void MouseDetected(object sender, MouseEventArgs e)
        {
            CommentsTextBox.Text = text;
            CommentsTextBox.Foreground = Brushes.Black;
            CommentsTextBox.Focusable = true;
        }

        private new void MouseLeave(object sender, MouseEventArgs e)
        {
            
            if (text == "")
            {
                CommentsTextBox.Text = "Суть обслуживания";
                CommentsTextBox.Foreground = Brushes.Gray;
                CommentsTextBox.Focusable = false;
            }
        }

        string text = "";

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            text = CommentsTextBox.Text;
        }

        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {
            if (text.Equals(""))
            {
                CommentsTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                database.SetOnMaintenance(Singleton.EQUIPMENS_INFO.LocId, CommentsTextBox.Text);
                dataPage.EquipmentsInfoDGValue = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0})", Singleton.MANAGER.Dep_id));
                Singleton.EQ_ID = 0;
                Singleton.EQUIPMENS_INFO = null;
                this.Close();
            }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

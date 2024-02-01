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

namespace EquipmentsAccounting.windows
{
    /// <summary>
    /// Логика взаимодействия для WriteOffEquipmentWindow.xaml
    /// </summary>
    public partial class WriteOffEquipmentWindow : Window
    {

        Database database;
        DataPage dataPage;

        public WriteOffEquipmentWindow(DataPage dataPage)
        {
            this.dataPage = dataPage;
            InitializeComponent();

            database = new Database();

            EquipmentsName.Text = Singleton.EQUIPMENS_INFO.Type + ' ' + Singleton.EQUIPMENS_INFO.Characteristics;
            
            CommentsTextBox.Text = "Причина списания";
            CommentsTextBox.Foreground = Brushes.Gray;

            List<string> causesList = new List<string>() 
            {
                "Не подлежит ремонту", "Другое"
            };
            for (int i = 0; i < causesList.Count(); i++)
            {
                CausesComboBox.Items.Add(causesList[i]);
            }
        }

        private void WriteOffButtonClick(object sender, RoutedEventArgs e)
        {
            if (CausesComboBox.SelectedValue.Equals("Другое") && text == "")
            {
                CausesComboBox.BorderBrush = Brushes.Black;
                CommentsTextBox.BorderBrush = Brushes.Red;
            }
            else 
            {
                CausesComboBox.BorderBrush = Brushes.Black;
                CommentsTextBox.BorderBrush = Brushes.Black;

                if (CausesComboBox.SelectedValue.Equals("Не подлежит ремонту"))
                {
                    database.Query(String.Format("CALL writeOffEquipment({0}, '{1}')", Singleton.EQUIPMENS_INFO.LocId, CausesComboBox.SelectedValue.ToString()));
                }
                else
                {
                    database.Query(String.Format("CALL writeOffEquipment({0}, '{1}')", Singleton.EQUIPMENS_INFO.LocId, CommentsTextBox.Text));
                }

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

        private void CausesChange(object sender, SelectionChangedEventArgs e)
        {
            if (CausesComboBox.SelectedValue.Equals("Другое"))
            {
                CommentsTextBox.IsEnabled = true;
            }
            else
            {
                CommentsTextBox.IsEnabled = false;
            }
            WriteOffButton.IsEnabled = true;
        }

        private void MouseDetected(object sender, MouseEventArgs e)
        {
            CommentsTextBox.Text = text;
            CommentsTextBox.Foreground = Brushes.Black;
            CommentsTextBox.Focusable = true;
        }

        string text = "";

        private void TextBoxKeyUp(object sender, KeyEventArgs e)
        {
            text = CommentsTextBox.Text;
        }

        private new void MouseLeave(object sender, MouseEventArgs e)
        {
            if (text == "")
            {
                CommentsTextBox.Text = "Причина списания";
                CommentsTextBox.Foreground = Brushes.Gray;
                CommentsTextBox.Focusable = false;
            }
        }
    }
}

using EquipmentsAccounting.views;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для CalendarRangeWindow.xaml
    /// </summary>
    public partial class CalendarRangeWindow : Window
    {

        private EquipmentsMaintenancePage page;
        List<DateTime> dateList;

        public CalendarRangeWindow(EquipmentsMaintenancePage page)
        {
            this.page = page;
            InitializeComponent();
        }

        private void DateChange(object sender, SelectionChangedEventArgs e)
        {
            dateList = Calendar.SelectedDates.ToList();

            if (dateList.Count > 1)
            {
                Singleton.START_DATE = dateList[0].ToString("yyyy-MM-dd");
                Singleton.END_DATE = dateList.Last().ToString("yyyy-MM-dd"); // Формат 'yyyy-MM-dd' - для работы с БД

                DateLabel.Content = String.Format("{0} - {1}", dateList[0].ToString("dd.MM.yyyy"), dateList.Last().ToString("dd.MM.yyyy")); // Формат 'dd.MM.yyyy' - для отображения
            }
            else
            {
                Singleton.START_DATE = dateList[0].ToString("yyyy-MM-dd");
                Singleton.END_DATE = null;
                DateLabel.Content = String.Format("{0} - _____._____.__________", dateList[0].ToString("dd.MM.yyyy"));
            }

            ApplyButton.IsEnabled = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            Singleton.START_DATE = null;
            Singleton.END_DATE = null;

            this.Close();
        }

        private void ApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (Singleton.END_DATE == null)
            {
                page.DateLabelText = String.Format("{0} - _____._____.__________", dateList[0].ToString("dd.MM.yyyy"));
            }
            else
            {
                page.DateLabelText = String.Format("{0} - {1}", dateList[0].ToString("dd.MM.yyyy"), dateList.Last().ToString("dd.MM.yyyy")); // DateLabelText - конструктор для изменения DateLabel на странице EquipmentsMaintenance
            }

            this.Close();
        }
    }
}

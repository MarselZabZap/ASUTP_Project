using EquipmentsAccounting.database;
using EquipmentsAccounting.windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentsAccounting.views
{
    /// <summary>
    /// Логика взаимодействия для EquipmentsMaintenancePage.xaml
    /// </summary>
    public partial class EquipmentsMaintenancePage : Page
    {
        readonly Database database;

        TextBlock eqName;

        string[] filterSettings;

        public EquipmentsMaintenancePage()
        {
            InitializeComponent();

            database = new Database();
        }

        private void Filtration(object sender, KeyEventArgs e)
        {
            DataTableFilter();
        }

        // Фильтрация вынесена в отдельный метод для его повторного использования при изменении параметра "Диапазон дат"
        private void DataTableFilter()
        {
            filterSettings = new string[] { "Наименование", "Обслуживание" };

            switch (WithoutEndDateCheckBox.IsChecked)
            {
                //Все записи
                case false:
                    if (FilterTextBox.Text.Equals("") && Singleton.START_DATE == null && Singleton.END_DATE == null)
                    {
                        EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(@"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE dep_id = {0}", Singleton.MANAGER.Dep_id)).DefaultView;
                    }
                    else
                    {
                        if (Singleton.START_DATE == null && Singleton.END_DATE == null) // Фильтрация БЕЗ указания диапазона дат
                        {
                            if ((bool)NameRadioButton.IsChecked)
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(@"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service  WHERE lower(""{0}"") LIKE lower('%{1}%') AND dep_id = {2}",
                                    filterSettings[0], FilterTextBox.Text, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                            else
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(@"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service  WHERE lower(""{0}"") LIKE lower('%{1}%') AND dep_id = {2}",
                                    filterSettings[1], FilterTextBox.Text, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                        }
                        else if (Singleton.END_DATE == null) // Фильтрация С указанием даты
                        {
                            if (DateTypeFilterToggleButton.IsChecked == false)
                            {
                                if ((bool)NameRadioButton.IsChecked)
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service  WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}' AND dep_id = {3}", filterSettings[0], FilterTextBox.Text, Singleton.START_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                                else
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}' AND dep_id = {3}", filterSettings[1], FilterTextBox.Text, Singleton.START_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                            }
                            else
                            {
                                if ((bool)NameRadioButton.IsChecked)
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата завершения""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}' AND dep_id = {3}", filterSettings[0], FilterTextBox.Text, Singleton.START_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                                else
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата завершения""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}' AND dep_id = {3}", filterSettings[1], FilterTextBox.Text, Singleton.START_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                            }
                        }
                        else // Фильтрация С указанием диапазона дат
                        {
                            if (DateTypeFilterToggleButton.IsChecked == false)
                            {
                                if ((bool)NameRadioButton.IsChecked)
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}'
                                AND (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) <= '{3}' AND dep_id = {4}", filterSettings[0], FilterTextBox.Text, Singleton.START_DATE, Singleton.END_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                                else
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}'
                                AND (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) <= '{3}' AND dep_id = {4}", filterSettings[1], FilterTextBox.Text, Singleton.START_DATE, Singleton.END_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                            }
                            else
                            {
                                if ((bool)NameRadioButton.IsChecked)
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата завершения""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}'
                                AND (SELECT TO_DATE(to_char(""Дата завершения""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) <= '{3}' AND dep_id = {4}", filterSettings[0], FilterTextBox.Text, Singleton.START_DATE, Singleton.END_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                                else
                                {
                                    EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                        @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}'
                                AND (SELECT TO_DATE(to_char(""Дата завершения""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) <= '{3}' AND dep_id = {4}", filterSettings[1], FilterTextBox.Text, Singleton.START_DATE, Singleton.END_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                                }
                            }
                        }
                    }
                    break;
                   
                // Только незавершенные записи
                case true:
                    if (FilterTextBox.Text.Equals("") && Singleton.START_DATE == null && Singleton.END_DATE == null)
                    {
                        EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(@"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE ""Дата завершения"" is null AND dep_id = {0}", Singleton.MANAGER.Dep_id)).DefaultView;
                    }
                    else
                    {
                        if (Singleton.START_DATE == null && Singleton.END_DATE == null) // Фильтрация БЕЗ указания диапазона дат
                        {
                            if ((bool)NameRadioButton.IsChecked)
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(@"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND ""Дата завершения"" is null AND dep_id = {2}",
                                    filterSettings[0], FilterTextBox.Text, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                            else
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(@"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND ""Дата завершения"" is null AND dep_id = {2}",
                                    filterSettings[1], FilterTextBox.Text, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                        }
                        else if (Singleton.END_DATE == null) // Фильтрация С указанием даты
                        {
                            if ((bool)NameRadioButton.IsChecked)
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                    @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}' AND ""Дата завершения"" is null AND dep_id = {3}", filterSettings[0], FilterTextBox.Text, Singleton.START_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                            else
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                    @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}' AND ""Дата завершения"" is null AND dep_id = {3}", filterSettings[1], FilterTextBox.Text, Singleton.START_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                        }
                        else // Фильтрация С указанием диапазона дат
                        {
                            if ((bool)NameRadioButton.IsChecked)
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                    @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}'
                                AND (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) <= '{3}' AND ""Дата завершения"" is null AND dep_id = {4}", filterSettings[0], FilterTextBox.Text, Singleton.START_DATE, Singleton.END_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                            else
                            {
                                EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(
                                    @"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE lower(""{0}"") LIKE lower('%{1}%') AND
                            (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) >= '{2}'
                                AND (SELECT TO_DATE(to_char(""Дата приёма""::date, 'YYYY-MM-DD'), 'YYYY-MM-DD')) <= '{3}' AND ""Дата завершения"" is null AND dep_id = {4}", filterSettings[1], FilterTextBox.Text, Singleton.START_DATE, Singleton.END_DATE, Singleton.MANAGER.Dep_id)).DefaultView;
                            }
                        }
                    }
                    break;
            }
        }

        private void DataGridCellFocused(object sender, MouseButtonEventArgs e)
        {
            var id = EquipmentsMaintenanceDataGrid.Columns[0].GetCellContent(EquipmentsMaintenanceDataGrid.Items[EquipmentsMaintenanceDataGrid.SelectedIndex]) as TextBlock;
            Singleton.EQ_ID = Int16.Parse(id.Text);

            eqName = EquipmentsMaintenanceDataGrid.Columns[1].GetCellContent(EquipmentsMaintenanceDataGrid.Items[EquipmentsMaintenanceDataGrid.SelectedIndex]) as TextBlock;

            var isEnd = EquipmentsMaintenanceDataGrid.Columns[3].GetCellContent(EquipmentsMaintenanceDataGrid.Items[EquipmentsMaintenanceDataGrid.SelectedIndex]) as TextBlock;

            if (isEnd.Text == "")
            {
                RemoveMaintenanceButton.IsEnabled = true;
                if (eqName.Text.Contains("Компьютер"))
                {
                    RemoveMaintenanceButtonWithChanges.IsEnabled = true;
                }
            }
            else
            {
                RemoveMaintenanceButton.IsEnabled = false;
                RemoveMaintenanceButtonWithChanges.IsEnabled = false;
            }
        }

        // Снятие с обслуживания без технических изменений устройства
        private void RemoveMaintenanceButtonClick(object sender, RoutedEventArgs e)
        {
            database.TakeOffMaintenance(Singleton.EQ_ID);
            DataTableFilter();
        }

        // Снятие с обслужиания с техническими изменениями устройства
        private void RemoveMaintenanceWithChangesButtonClick(object sender, RoutedEventArgs e)
        {
            database.TakeOffMaintenance(Singleton.EQ_ID);
            
            if (eqName.Text.Contains("Компьютер"))
            { 
                EditingEquipment window = new EditingEquipment(null);
                window.ShowDialog();
                DataTableFilter();
            }
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            EquipmentsMaintenanceDataGrid.DataContext = database.Query(String.Format(@"SELECT id, ""Наименование"", ""Дата приёма"", ""Дата завершения"", ""Обслуживание"" FROM equipments_service WHERE dep_id = {0}", Singleton.MANAGER.Dep_id)).DefaultView;
        }

        private void OpenCalendarButtonClick(object sender, MouseButtonEventArgs e)
        {
            CalendarRangeWindow window = new CalendarRangeWindow(this);
            window.ShowDialog();
        }

        // Конструкто для изменения DateLable на этой странице из CalendarRangeWindow
        public string DateLabelText
        {
            get { return DateLabel.Content.ToString(); }
            set { DateLabel.Content = value; DataTableFilter(); }
        }

        private void DateClearMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (!DateLabel.Content.Equals("______._____.__________ - _____._____.__________"))
            {
                DateLabel.Content = "______._____.__________ - _____._____.__________";
                Singleton.START_DATE = null;
                Singleton.END_DATE = null;
                DataTableFilter();
            }
        }

        private void DateTypeFilterChange(object sender, RoutedEventArgs e)
        {
            if (DateTypeFilterToggleButton.IsChecked == false) 
            {
                DateTypeFilterToggleButton.Content = "Приём";
            }
            else
            {
                DateTypeFilterToggleButton.Content = "Завершение";
            }
        }

        private void WithoutEndDateCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (WithoutEndDateCheckBox.IsChecked == true)
            {
                DateTypeFilterToggleButton.IsChecked = false;
                DateTypeFilterToggleButton.IsEnabled = false;
            }
            else
            {
                DateTypeFilterToggleButton.IsEnabled = true;
            }
            DataTableFilter();
        }
    }
}

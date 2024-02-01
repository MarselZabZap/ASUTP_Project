using EquipmentsAccounting.database;
using EquipmentsAccounting.Excel;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для ReportSettingWindow.xaml
    /// </summary>
    public partial class ReportSettingWindow : Window
    {

        Database database;
        DataTable datatabel;
        DataTable additionalyDatatable;
        ExcelHelper excelHelper;

        public ReportSettingWindow()
        {
            database = new Database();
            datatabel = new DataTable();
            excelHelper = new ExcelHelper();

            InitializeComponent();
        }

        private void MainChecked(object sender, RoutedEventArgs e)
        {
            AdditionallySettingsWrapPanel.IsEnabled = true;
            DateCheckBox.IsEnabled = false;
            ReasonCheckBox.IsEnabled = false;
            
            if (AllEquipmentsRadioButton.IsChecked == true && EquipmentsSumCheckBox.IsChecked == true)
            {
                NoDisponsalSumCheckBox.IsEnabled = true;
            }
            if (AllEquipmentsRadioButton.IsChecked == true || StockEquipmentsRadioButton.IsChecked == true)
            {
                DateCheckBox.IsEnabled = false;
                ReasonCheckBox.IsEnabled = false;

                DateCheckBox.IsChecked = false;
                ReasonCheckBox.IsChecked = false;
            }
            if (StockEquipmentsRadioButton.IsChecked == true || MaintenanceEquipmentsRadioButton.IsChecked == true || WriteOffEquipmentsRadioButton.IsChecked == true)
            {
                NoDisponsalSumCheckBox.IsChecked = true;
                NoDisponsalSumCheckBox.IsEnabled = false;
            }
            if (StockEquipmentsRadioButton.IsChecked == true)
            {
                NoDisponsalSumCheckBox.IsChecked = false;
                NoDisponsalSumCheckBox.IsEnabled = false;
            }
            if (MaintenanceEquipmentsRadioButton.IsChecked == true || WriteOffEquipmentsRadioButton.IsChecked == true)
            {
                if (EquipmentsCountRadioButton.IsChecked == true)
                {
                    DateCheckBox.IsEnabled = false;
                    ReasonCheckBox.IsEnabled = false;
                }
                else
                {
                    DateCheckBox.IsEnabled = true;
                    ReasonCheckBox.IsEnabled = true;
                }

                NoDisponsalSumCheckBox.IsChecked = false;
                NoDisponsalSumCheckBox.IsEnabled = false;
            }
        }
        private void AdditionallyChecked(object sender, RoutedEventArgs e)
        {
            if (MaintenanceEquipmentsRadioButton.IsChecked == true || WriteOffEquipmentsRadioButton.IsChecked == true)
            {
                if (EquipmentsCountRadioButton.IsChecked == true)
                {
                    DateCheckBox.IsEnabled = false;
                    ReasonCheckBox.IsEnabled = false;
                    DateCheckBox.IsChecked = false;
                    ReasonCheckBox.IsChecked = false;
                }
                else
                {
                    DateCheckBox.IsEnabled = true;
                    ReasonCheckBox.IsEnabled = true;
                }
            }
        }

        private void EquipmentsSumCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            if (AllEquipmentsRadioButton.IsChecked == true)
            {
                NoDisponsalSumCheckBox.IsEnabled = true;
            }
        }

        private void EquipmentsSumCheckBoxUnchecked(object sender, RoutedEventArgs e)
        {
            NoDisponsalSumCheckBox.IsEnabled = false;
        }

        private void DoneButtonClick(object sender, RoutedEventArgs e)
        {
            List<int> priceList = new List<int>();
            List<int> countList = new List<int>();
            int sum = 0;

            bool date = (bool)DateCheckBox.IsChecked;
            bool reason = (bool)ReasonCheckBox.IsChecked;

            // ВСЁ оборудование
            if (AllEquipmentsRadioButton.IsChecked == true)
            {
                // Опора на серийный номер
                if (EquipmentsSerialNumRadioButton.IsChecked == true)
                {
                    // Оборудованое с и без серийного номера
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                                                     "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0};", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                                                     "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0};", Singleton.MANAGER.Dep_id));
                        }
                    }
                    // Только оборудование с серийным номером
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));

                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        if (NoDisponsalSumCheckBox.IsChecked == true)
                        {

                            foreach (DataRow row in datatabel.Rows)
                            {
                                if (row["Статус"].ToString() != "Списан")
                                {
                                    priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                                }
                            }
                        }
                        else
                        {

                            foreach (DataRow row in datatabel.Rows)
                            {
                                priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                            }
                        }

                        foreach (int i in priceList)
                        {
                            sum += i;
                        }
                    }
                }
                // Опора на количество
                else if (EquipmentsCountRadioButton.IsChecked == true)
                {
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));

                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                    }
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\" WHERE l.dep_id = {0} AND \"Серийный номер\" <> '-'", Singleton.MANAGER.Dep_id));

                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a JOIN loc_eq_acc l ON a.loc_id = l.id" +
                                                       "GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\" WHERE l.dep_id = {0} AND \"Серийный номер\" <> '-'", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        if (NoDisponsalSumCheckBox.IsChecked == true)
                        {

                            foreach (DataRow row in datatabel.Rows)
                            {
                                if (row["Статус"].ToString() != "Списан")
                                {
                                    priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                                    countList.Add(Int32.Parse(row["Количество"].ToString()));
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow row in datatabel.Rows)
                            {
                                priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                                countList.Add(Int32.Parse(row["Количество"].ToString()));
                            }
                        }

                        for (int i = 0; i < priceList.Count; i++)
                        {
                            sum += priceList[i] * countList[i];
                        }
                    }
                }
            }
            // Оборудование на СКЛАДЕ
            else if (StockEquipmentsRadioButton.IsChecked == true)
            {
                // Опора на серийный номер
                if (EquipmentsSerialNumRadioButton.IsChecked == true)
                {
                    // Оборудованое с и без серийного номера
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе';", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе';", Singleton.MANAGER.Dep_id));
                        }
                    }
                    // Только оборудование с серийным номером
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\",\"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе' AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе' AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        foreach (DataRow row in datatabel.Rows)
                        {
                            priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                        }

                        for (int i = 0; i < priceList.Count; i++)
                        {
                            sum += priceList[i];
                        }
                    }
                }
                // Опора на количество
                else if (EquipmentsCountRadioButton.IsChecked == true)
                {
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                    }
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе' AND \"Серийный номер\" <> '-' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На складе' AND \"Серийный номер\" <> '-' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        foreach (DataRow row in datatabel.Rows)
                        {
                            priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                            countList.Add(Int32.Parse(row["Количество"].ToString()));
                        }

                        for (int i = 0; i < priceList.Count; i++)
                        {
                            sum += priceList[i] * countList[i];
                        }
                    }
                }
            }
            // Оборудование на ОБСЛУЖИВАНИИ
            else if (MaintenanceEquipmentsRadioButton.IsChecked == true)
            {
                // Опора на серийный номер
                if (EquipmentsSerialNumRadioButton.IsChecked == true)
                {
                    // Оборудованое с и без серийного номера
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании';", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании';", Singleton.MANAGER.Dep_id));
                        }
                    }
                    // Только оборудование с серийным номером
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании' AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании' AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        foreach (DataRow row in datatabel.Rows)
                        {
                            priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                        }

                        for (int i = 0; i < priceList.Count; i++)
                        {
                            sum += priceList[i];
                        }
                    }

                    if (date && reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_acceptance AS \"Дата начала обслуживания\", comments AS \"Причина\" FROM loc_eq_service WHERE date_end is null;");
                    }
                    else if (date && !reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_acceptance AS \"Дата начала обслуживания\" FROM loc_eq_service WHERE date_end is null;");
                    }
                    else if (!date && reason)
                    {
                        additionalyDatatable = database.Query("SELECT comments AS \"Причина\" FROM loc_eq_service WHERE date_end is null;");
                    }
                }
                // Опора на количество
                else if (EquipmentsCountRadioButton.IsChecked == true)
                {
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании' GROUP BY \"Тип\", \"Характеристики\",  \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании' GROUP BY \"Тип\", \"Характеристики\",  \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                    }
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\",  \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании' AND \"Серийный номер\" <> '-' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'На техобслуживании' AND \"Серийный номер\" <> '-' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        foreach (DataRow row in datatabel.Rows)
                        {
                            priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                            countList.Add(Int32.Parse(row["Количество"].ToString()));
                        }

                        for (int i = 0; i < priceList.Count; i++)
                        {
                            sum += priceList[i] * countList[i];
                        }
                    }
                }
            }
            // СПИСАННОЕ оборудование
            else if (WriteOffEquipmentsRadioButton.IsChecked == true)
            {
                // Опора на серийный номер
                if (EquipmentsSerialNumRadioButton.IsChecked == true)
                {
                    // Оборудованое с и без серийного номера
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\",  \"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан';", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан';", Singleton.MANAGER.Dep_id));
                        }
                    }
                    // Только оборудование с серийным номером
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\",  \"Статус\", \"Стоимость\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан' AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", \"Серийный номер\", \"Статус\" FROM all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан' AND \"Серийный номер\" <> '-';", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        foreach (DataRow row in datatabel.Rows)
                        {
                            priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                        }

                        for (int i = 0; i < priceList.Count; i++)
                        {
                            sum += priceList[i];
                        }
                    }

                    if (date && reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_of_disposal AS \"Дата списания\", comments AS \"Причина\" FROM loc_eq_acc WHERE sts_id = 3;");
                    }
                    else if (date && !reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_of_disposal AS \"Дата списания\" FROM loc_eq_acc WHERE sts_id = 3;");
                    }
                    else if (!date && reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_of_disposal AS comments AS \"Причина\" FROM loc_eq_acc WHERE sts_id = 3;");
                    }
                }
                // Опора на количество
                else if (EquipmentsCountRadioButton.IsChecked == true)
                {
                    if (OnlyWithSerialNumsCheckBox.IsChecked == false)
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\",  \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан' GROUP BY \"Тип\", \"Характеристики\",  \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан' GROUP BY \"Тип\", \"Характеристики\",  \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                    }
                    else
                    {
                        if (EquipmentsPriceCheckBox.IsChecked == true)
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\", \"Стоимость\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан' AND \"Серийный номер\" <> '-' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                        else
                        {
                            datatabel = database.Query(String.Format("SELECT concat(\"Тип\", ' ', \"Характеристики\") AS \"Наименование\", COUNT(*) AS \"Количество\", \"Статус\" from all_eq_info a " +
                                                       "JOIN loc_eq_acc l ON a.loc_id = l.id WHERE l.dep_id = {0} AND \"Статус\" = 'Списан' AND \"Серийный номер\" <> '-' GROUP BY \"Тип\", \"Характеристики\", \"Статус\", \"Стоимость\"\r\nORDER BY \"Статус\"", Singleton.MANAGER.Dep_id));
                        }
                    }

                    if (EquipmentsSumCheckBox.IsChecked == true)
                    {
                        foreach (DataRow row in datatabel.Rows)
                        {
                            priceList.Add(Int32.Parse(row["Стоимость"].ToString()));
                            countList.Add(Int32.Parse(row["Количество"].ToString()));
                        }

                        for (int i = 0; i < priceList.Count; i++)
                        {
                            sum += priceList[i] * countList[i];
                        }
                    }

                    if (date && reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_of_disposal AS \"Дата списания\", comments AS \"Причина\" FROM loc_eq_acc WHERE sts_id = 3;");
                    }
                    else if (date && !reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_of_disposal AS \"Дата списания\" FROM loc_eq_acc WHERE sts_id = 3;");
                    }
                    else if (!date && reason)
                    {
                        additionalyDatatable = database.Query("SELECT date_of_disposal AS comments AS \"Причина\" FROM loc_eq_acc WHERE sts_id = 3;");
                    }
                }
            }
            ReportDataGrid.DataContext = datatabel.DefaultView;

            //Формирование кастомного документа
            excelHelper.CreateCustomReport(datatabel, ReportDataGrid, (bool)EquipmentsSumCheckBox.IsChecked, sum, date, reason, additionalyDatatable);
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            //ReportDataGrid.DataContext = database.Query(String.Format(@"SELECT * FROM loc_eq_acc_info({0})", Singleton.MANAGER.Dep_id)).DefaultView;
        }
    }
}

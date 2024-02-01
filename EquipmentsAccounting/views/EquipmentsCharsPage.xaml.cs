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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static EquipmentsAccounting.view.DataPage;

namespace EquipmentsAccounting.views
{
    /// <summary>
    /// Логика взаимодействия для EquipmentsCharsPage.xaml
    /// </summary>
    public partial class EquipmentsCharsPage : Page
    {

        Database database;

        public EquipmentsCharsPage()
        {
            InitializeComponent();

            database = new Database();
        }

    }
}

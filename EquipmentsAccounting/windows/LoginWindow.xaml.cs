using EquipmentsAccounting.database;
using EquipmentsAccounting.model;
using EquipmentsAccounting.window;
using System.Windows;
using System.Windows.Media;

namespace EquipmentsAccounting
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Database database;

        public LoginWindow()
        {
            InitializeComponent();

            database = new Database();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager manager = database.GetManager(LoginTextBox.Text);

            if (LoginTextBox.Text.Equals("") || PasswordTextBox.Password.Equals(""))
            {
                LoginTextBox.BorderBrush = Brushes.Red;
                PasswordTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                LoginTextBox.BorderBrush = Brushes.Black;
                PasswordTextBox.BorderBrush = Brushes.Black;
                ErrorLoginLabel.Visibility = Visibility.Hidden;
                if (manager == null)
                {
                    ErrorLoginLabel.Visibility = Visibility.Visible;
                    ErrorLoginLabel.Content = "Такого пользователя нет";
                }
                else
                {
                    ErrorLoginLabel.Visibility = Visibility.Hidden;

                    if (!manager.Password.Equals(PasswordTextBox.Password))
                    {
                        ErrorPasswordLabel.Visibility = Visibility.Visible;
                        ErrorPasswordLabel.Content = "Неверный пароль";
                    }
                    else
                    {
                        Singleton.MANAGER = manager;

                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Close();
                    }
                }
            }
        }
    }
}

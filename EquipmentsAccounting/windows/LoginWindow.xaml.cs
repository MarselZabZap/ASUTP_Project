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
            if (LoginTextBox.Text.Equals("") || PasswordTextBox.Password.Equals(""))
            {

                LoginBorder.BorderBrush = Brushes.Red;
                PasswordBorder.BorderBrush = Brushes.Red;
            }
            else
            {
                Manager manager = database.GetManager(LoginTextBox.Text);

                LoginBorder.BorderBrush = Brushes.Black;
                PasswordBorder.BorderBrush = Brushes.Black;
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

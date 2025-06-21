using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class LoginPage : Page
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void GoReg(object sender, MouseButtonEventArgs e)
    {
        NavigationService.Navigate(new RegPage());
    }

    private void Login(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PasswordPB.Password) || string.IsNullOrWhiteSpace(LoginTB.Text))
        {
            MessageBox.Show("Пожалуйста введите логин и пароль!");
            return;
        }

        PhysContext.User =
            PhysContext.Instance.Users.FirstOrDefault(i =>
                i.Password == PasswordPB.Password && i.Email == LoginTB.Text);
        if (PhysContext.User == null)
        {
            MessageBox.Show("Введенные логин и пароль не верны!");
            return;
        }
        
        File.WriteAllText("certificate.txt", PhysContext.User.Id.ToString());
        NavigationService.Navigate(new SolvingPage());
    }
}
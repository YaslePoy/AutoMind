using System.IO;
using System.Net.Mail;
using System.Windows;
using System.Windows.Controls;
using PhysLab.DB;

namespace PhysLab.Pages;

public partial class RegPage : Page
{
    public RegPage()
    {
        InitializeComponent();
    }

    private void Reg(object sender, RoutedEventArgs e)
    {
        if (!HelperExtentions.AreValid(LoginTB.Text, PasswordPB.Password, NameTB.Text))
        {
            MessageBox.Show("Заполните все поля!");
            return;
        }

        if (!MailAddress.TryCreate(LoginTB.Text, out _))
        {
            MessageBox.Show("Email имеет некорректный формат!");
            return;
        }

        if (PhysContext.Instance.Users.Any(i => i.Email == LoginTB.Text))
        {
            MessageBox.Show("Этот email уже занят!");
            return;
        }

        if (PasswordPB.Password != RePasswordPB.Password)
        {
            MessageBox.Show("Пароли не совпадают!");
            return;
        }

        var user = new User { Name = NameTB.Text, Password = PasswordPB.Password, Email = LoginTB.Text };
        PhysContext.Instance.Users.Add(user);
        PhysContext.Instance.SaveChanges();
        PhysContext.User = user;
        File.WriteAllText("certificate.txt", PhysContext.User.Id.ToString());

        NavigationService!.Navigate(new SolvingPage());
    }
}
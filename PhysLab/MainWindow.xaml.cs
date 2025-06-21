using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PhysLab.DB;
using PhysLab.Pages;

namespace PhysLab;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        if (File.Exists("certificate.txt"))
        {
            var id = int.Parse(File.ReadAllText("certificate.txt"));
            PhysContext.User = PhysContext.Instance.Users.FirstOrDefault(x => x.Id == id);
            BaseFrame.Navigate(new SolvingPage());
            return;
        }
        BaseFrame.Navigate(new LoginPage());
    }
}
using System.Windows;
using StockBrokerProject.ViewModels;

namespace StockBrokerProject
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}

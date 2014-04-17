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
using ChitsManager.DataAccess;

namespace ChitsManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //DataController c = new DataController();
            //c.GetAuctionsbyChitId(1);

            DataConversion.ConvertAuctionData();
        }

        private void dgAuctions_Loaded(object sender, RoutedEventArgs e)
        {
            dgAuctions.Columns[0].Visibility = Visibility.Hidden;
            dgAuctions.Columns[1].Visibility = Visibility.Hidden;
            dgAuctions.Columns[2].IsReadOnly = true;
        }
    }
}

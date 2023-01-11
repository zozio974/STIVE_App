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
using System.Windows.Shapes;

namespace AppCUBES.command
{
    /// <summary>
    /// Logique d'interaction pour newcommand.xaml
    /// </summary>
    public partial class newcommand : Window
    {
        public newcommand()
        {
            InitializeComponent();
        }

        private void addline_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteline_Click(object sender, RoutedEventArgs e)
        {

        }

        private void valcommand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshnewcommand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void retnewcommmand_Click(object sender, RoutedEventArgs e)
        {
            dispcommand win = new dispcommand();
            win.Show();
            this.Close();
            
        }
    }
}

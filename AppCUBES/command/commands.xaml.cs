using AppCUBES.command;
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

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour command.xaml
    /// </summary>
    public partial class commands : Window
    {
        public commands()
        {
            InitializeComponent();
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            win1 win = new win1();
            win.Show();
            this.Close();
        }

        private void displaycommand_Click(object sender, RoutedEventArgs e)
        {
            dispcommand win = new dispcommand();
            win.Show();
            this.Close();
        }
    }
}

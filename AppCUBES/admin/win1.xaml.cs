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
using System.Windows.Threading;

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour win1.xaml
    /// </summary>
    public partial class win1 : Window
    {
        public win1()
        {
            InitializeComponent();
        }

        private void gest_Click(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }

        private void disconnect_Click(object sender, RoutedEventArgs e)
        {
            Connect.idjobuser = 0;
            Connect.iduser = 0;
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }

        private void inventory_Click(object sender, RoutedEventArgs e)
        {
            inventory inv = new inventory();
            inv.Show();
            this.Close();
        }

        private void command_Click(object sender, RoutedEventArgs e)
        {
            dispcommand win = new dispcommand();
            win.Show();
            this.Close();
        }
    }


}

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
    public partial class commandwin : Window
    {
        public commandwin()
        {
            InitializeComponent();
        }

        private void menu_Click(object sender, RoutedEventArgs e)
        {
            Connect.idjobuser = 0;
            Connect.iduser = 0;
            MainWindow win = new MainWindow();
            win.Show();
            this.Close();
        }
    }
}

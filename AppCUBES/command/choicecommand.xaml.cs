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
    /// Logique d'interaction pour choicecommand.xaml
    /// </summary>
    public partial class choicecommand : Window
    {
        public choicecommand()
        {
            InitializeComponent();
            if (Connect.idjobuser == 1)
            {
                retchoicecommand.Content = "Menu";
            }
            else
            {
                retchoicecommand.Content = "Déconnecter";
            }
        }

        private void commandsup_Click(object sender, RoutedEventArgs e)
        {
            dispcommand win = new dispcommand();
            win.Show();
            this.Close();
        }

        private void commandclient_Click(object sender, RoutedEventArgs e)
        {
            dispcommandclient win = new dispcommandclient();
            win.Show();
            this.Close();
        }

        private void retchoicecommand_Click(object sender, RoutedEventArgs e)
        {

            if (Connect.idjobuser == 1)
            {
                win1 win2 = new win1();
                win2.Show();
                this.Close();
                return;
            }
            Connect.idjobuser = 0;
            Connect.iduser = 0;
            MainWindow win1 = new MainWindow();
            win1.Show();
        }
    }
}

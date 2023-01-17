using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json.Linq;

namespace AppCUBES.admin
{
    /// <summary>
    /// Logique d'interaction pour option.xaml
    /// </summary>
    public partial class option : Window
    {
        public option()
        {
            InitializeComponent();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "CommandAuto/displayautovar";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                if (json == "1") {
                    rescomauto.Text = "Activé";
                }
                else
                {
                    rescomauto.Text = "Désactivé";

                }
            }

        }

        private void retouroption_Click(object sender, RoutedEventArgs e)
        {
            win1 win = new win1();
            win.Show();
            this.Close();
        }

        private void commandauto_Click(object sender, RoutedEventArgs e)
        {
            if (rescomauto.Text == "Activé")
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "https://localhost:7279/";
                    client.BaseAddress = new Uri(Url);
                    string parameters = "CommandAuto/commandeauto_off";
                    HttpResponseMessage response = client.PutAsync(parameters, null).Result;
                }
                rescomauto.Text = "Désactivé";
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "CommandAuto/commandeauto_on";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
            }
            rescomauto.Text = "Activé";
            resquantitychange.Text = "";
        }

        private void changestockminadd_Click(object sender, RoutedEventArgs e)
        {
            if (isinteger(amountchangemin.Text) == false || amountchangemin.Text == "")
            {
                resquantitychange.Text = "Le champs est mal entré";
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"CommandAuto/quantiteaddtostock?y={amountchangemin.Text}";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
            }
            resquantitychange.Text = $"La nouveau stock automatique ajouté est de {amountchangemin.Text}";
        }
        private bool isinteger(string str)
        {
            int res = 0;
            return int.TryParse(str, out res);
        }
    }
}

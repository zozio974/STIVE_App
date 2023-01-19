using Newtonsoft.Json.Linq;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static AppCUBES.command.commandinfo;

namespace AppCUBES.command
{

    /// <summary>
    /// Logique d'interaction pour newcommand.xaml
    /// </summary>
    public partial class newcommandclient : Window
    {
        List<int> list = new List<int>();
        List<string> listname = new List<string>();
        List<int> listidline = new List<int>();

        public newcommandclient()
        {
            InitializeComponent();
        }

        private void addline_Click(object sender, RoutedEventArgs e)
        {
            commandaddclient win = new commandaddclient();
            win.addlinecommand.Click += refreshnewcommand_Click;

            win.ShowDialog();

        }
        public string createparameter(List<int> list)
        {
            string a = "";
            foreach (int item in list)
            {
                a += $"listOfIds={item}&";
            }
            if (a.Length > 0)
            {
                var output = a.Remove(a.Length - 1);
                return output;

            }
            return a;
        }
        private void deleteline_Click(object sender, RoutedEventArgs e)
        {
            if (gridcomminfo.SelectedItem == null)
            {
                resinfonewcom.Text = "Veuillez selectionner un élément de la liste";
                return;
            }

            int a = listidline[gridcomminfo.SelectedIndex];
            
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Delete/delete_linecommand?ID={a}";
                HttpResponseMessage response = client.DeleteAsync(parameters).Result;
                resinfonewcom.Text = string.Empty;
            }
            refreshnewcommand_Click(sender, e);
        }

        private void valcommand_Click(object sender, RoutedEventArgs e)
        {
            string json = "";
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getiduser?login={VarCommand.login}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                json = response.Content.ReadAsStringAsync().Result;
            }               
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Commands/addcommandclient?refcom={VarCommand.refcom}&iduser={json}";
                HttpResponseMessage response = client.PostAsync(parameters,null).Result;
                resinfonewcom.Text = string.Empty;
            }
            dispcommandclient win = new dispcommandclient();
            win.Show();
            this.Close();

        }

        private void refreshnewcommand_Click(object sender, RoutedEventArgs e)
        {
            List<CommandInfo> comms = new List<CommandInfo>();
            list.Clear();
            listname.Clear();
            listidline.Clear();


            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Commands/displaylinecommand?refcom={VarCommand.refcom}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);

                for (int i = 0; i < detail.Count; i++)
                {
                    list.Add(Convert.ToInt32(detail[i]["id_article"]));
                    listidline.Add(Convert.ToInt32(detail[i]["id_LineCommande"]));
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnamearticle?{createparameter(list)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listname.Add((detail[i]).ToString());
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Commands/displaylinecommand?refcom={VarCommand.refcom}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);

                for (int i = 0; i < detail.Count; i++)
                {
                    comms.Add(new CommandInfo(detail[i]["ref_Command"].ToString(), listname[i], detail[i]["quantity"].ToString(), detail[i]["price"].ToString()));
                }
                gridcomminfo.ItemsSource = comms;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Commands/displaytotalcommand?refe={VarCommand.refcom}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                rescomtotal.Text = $"Le total est de {json} euros";
            }
        }

        private void retnewcommmand_Click(object sender, RoutedEventArgs e)
        {
            dispcommand win = new dispcommand();
            win.Show();
            this.Close();
        }
    }
}

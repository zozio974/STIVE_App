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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static AppCUBES.command.dispcommand;

namespace AppCUBES.command
{
    
    /// <summary>
    /// Logique d'interaction pour commandinfo.xaml
    /// </summary>
    public partial class commandinfo : Window
    {
        List<int> list = new List<int>();
        List<string> listname = new List<string>();
        
        public commandinfo()
        {
            InitializeComponent();
            List<CommandInfo> comms = new List<CommandInfo>();
            list.Clear();
            listname.Clear();


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
                gridcomminfo.ItemsSource= comms;
            }
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
        private void retcominfo_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        public class CommandInfo
        {
            public string Reference { get; set; }
            public string NomArticle { get; set; }
            public string Quantite { get; set;}
            public string PrixTotal { get; set;}
            public CommandInfo(string reference,string nomArticle,string quantite,string prixtotal)
            {
                Reference = reference;
                NomArticle = nomArticle;
                Quantite = quantite;
                PrixTotal = prixtotal;
            }
        }

        private void detarticle_Click(object sender, RoutedEventArgs e)
        {
            if (gridcomminfo.SelectedValue == null)
            {
                return;
            }
            VarCommand.idarticlecom = list[gridcomminfo.SelectedIndex];
            commandinfoart win = new commandinfoart();
            win.Show();
        }
    }
}

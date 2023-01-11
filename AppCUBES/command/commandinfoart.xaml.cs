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

namespace AppCUBES.command
{
    /// <summary>
    /// Logique d'interaction pour commandinfoart.xaml
    /// </summary>
    public partial class commandinfoart : Window
    {
        public string createparameter(List<int> list)
        {
            string a = "";
            foreach (int item in list)
            {
                a += $"listOfIds={item}&";
            }
            var output = a.Remove(a.Length - 1);
            return output;
        }
        public commandinfoart()
        {
            InitializeComponent();
            JArray detail = new JArray();
            List<int> listidsup = new List<int>();
            List<int> listidfam = new List<int>();
            List<string> listnamesup=new List<string>();
            List<string> listnamefam=new List<string>();

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/displayarticlebyid?ID={VarCommand.idarticlecom}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = $"[{ response.Content.ReadAsStringAsync().Result}]";
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidsup.Add(Convert.ToInt32(detail[i]["idProvider"]));
                    listidfam.Add(Convert.ToInt32(detail[i]["idFamily"]));
                }
            }
            List<Article> arts = new List<Article>();


            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnamesup?{createparameter(listidsup)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail1 = JArray.Parse(json);
                for (int i = 0; i < detail1.Count; i++)
                {
                    listnamesup.Add((detail1[i]).ToString());

                }
            }

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnamefam?{createparameter(listidfam)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail2 = JArray.Parse(json);
                for (int i = 0; i < detail2.Count; i++)
                {
                    listnamefam.Add((detail2[i]).ToString());
                }
            }

            for (int i = 0; i < detail.Count; i++)
            {
                arts.Add(new Article(detail[i]["nameArticle"].ToString(), listnamesup[i],
                                  detail[i]["dateFill"].ToString(), listnamefam[i],
                                  detail[i]["priceSup"].ToString(), detail[i]["price"].ToString(), detail[i]["volume"].ToString(),
                                  detail[i]["degree"].ToString(), detail[i]["grape"].ToString(), detail[i]["ladder"].ToString()));

            }

            gridartdet.ItemsSource = arts;
        }

        private void retcommand_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour articleput.xaml
    /// </summary>
    public partial class articleput : Window
    {
        public articleput()
        {
            InitializeComponent();
            List<string> list = new List<string>();
            List<string> list1 = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displaysup";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count(); i++)
                {
                    list.Add(detail[i]["name"].ToString());
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displayfamily";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count(); i++)
                {
                    list1.Add(detail[i]["nameFamily"].ToString());
                }
            }

            articlesup.ItemsSource = list;
            articlefamily.ItemsSource = list1;
        }

        private void articleput_Click_1(object sender, RoutedEventArgs e)
        {
            string ressup = "";
            string resfam = "";
            
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getidsupplier?name={articlesup.SelectedValue}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                ressup = response.Content.ReadAsStringAsync().Result;

            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getidfamily?name={articlefamily.SelectedValue}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                resfam = response.Content.ReadAsStringAsync().Result;
            }
            article articlo = new article();
            using (HttpClient client = new HttpClient()) 
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"PutTable/putarticle?ID={invputarticle.Text}&name={articlename.Text}&idsup={ressup}&datefill={articledatefill.Text}&idfamily={resfam}&pricesup={articlepricesup.Text}&price={articleprice.Text}&volume={articlevol.Text}&degree={articledegree.Text}&grape={articlesep.Text}&ladder={articleladder.Text}";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
            }

            article art = new article();
            
            this.Close();
            art.articlerefresh_Click(sender, e);
        }

        private void articleaddprec_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

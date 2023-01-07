using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Xml.Linq;

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour articleadd.xaml
    /// </summary>
    public partial class articleadd : Window
    {
        public articleadd()
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
                for (int i=0;i< detail.Count(); i++)
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
        private bool isinteger(string str)
        {
            int res = 0;
            return int.TryParse(str,out res);
        }

        private void validatearticleadd_Click(object sender, RoutedEventArgs e)
        {
            string ressup  = "";
            string resfam= "";
            if (articlename.Text == "" || articlesup.SelectedValue == null || articledatefill.Text == "" || articlefamily.SelectedValue == null
                ||articlepricesup.Text ==""||articleprice.Text ==""||articledegree.Text ==""||articlesep.Text ==""|| articleladder.Text == "")
            {
                resarticleadd.Text = "Un ou plusieurs champs est vide";
                return;
            }
            if (isinteger(articledatefill.Text) == false|| isinteger(articledegree.Text) == false|| isinteger(articlepricesup.Text) == false || isinteger(articleprice.Text) == false)
            {
                resarticleadd.Text = "Un des champs est mal entré";
                return;
            }
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
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Add/add_article?name={articlename.Text}&idprovider={ressup}&datefill={articledatefill.Text}&idfamily={resfam}&pricesup={articlepricesup.Text}&price={articleprice.Text}&volume={articlevol.Text}&degree={articledegree.Text}&grape={articlesep.Text}&ladder={articleladder.Text}";
                HttpResponseMessage response = client.PostAsync(parameters, null).Result;
            }
            article art = new article();
            
            this.Close();
            art.refresharticle_Click(sender, e);
        }

        private void articleaddprec_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
    
}

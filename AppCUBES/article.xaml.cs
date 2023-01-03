using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Logique d'interaction pour article.xaml
    /// </summary>
    public partial class article : Window
    {
        List<int> list = new List<int>();
        
        
        public article()
        {
            InitializeComponent();
            display_article();
        }
        public void display_article()
        {
            List<Article> arts = new List<Article>();
            using HttpClient client = new HttpClient();
            using HttpClient client1 = new HttpClient();
            using HttpClient client2 = new HttpClient();

            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            client1.BaseAddress = new Uri(Url);
            client2.BaseAddress = new Uri(Url);

            string parameters = "Display/displayarticle";
            string parameters2 = "Display/displaysup";
            string parameters3 = "Display/displayfamily";

            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            HttpResponseMessage response2 = client1.GetAsync(parameters2).Result;
            string json2 = response2.Content.ReadAsStringAsync().Result;
            HttpResponseMessage response3 = client2.GetAsync(parameters3).Result;
            string json3 = response3.Content.ReadAsStringAsync().Result;
            JArray detail = JArray.Parse(json);
            JArray detail2 = JArray.Parse(json2);
            JArray detail3 = JArray.Parse(json3);

            for (int i = 0; i < detail.Count; i++)
            {
                int a = Convert.ToInt32(detail[i]["idProvider"]);
                int b = Convert.ToInt32(detail[i]["idFamily"]);
                arts.Add(new Article(detail[i]["nameArticle"].ToString(), detail2[a-1]["name"].ToString(),
                    detail[i]["dateFill"].ToString(),detail3[b-1]["nameFamily"].ToString(),
                    detail[i]["priceSup"].ToString(), detail[i]["price"].ToString(), detail[i]["volume"].ToString(),
                    detail[i]["degree"].ToString(), detail[i]["grape"].ToString(), detail[i]["ladder"].ToString()));
                list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
            }
            articledata.ItemsSource = arts;

        }

        private void custdata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void articleadd_Click(object sender, RoutedEventArgs e)
        {
            articleadd custo = new articleadd();
            custo.Show();
            
        }

        public void articlerefresh_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_article();

        }

        private void articledelete_Click(object sender, RoutedEventArgs e)
        {
            if (articledata.SelectedItem == null)
            {
                articleconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[articledata.SelectedIndex];
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Delete/delete_article?ID={a}";
            HttpResponseMessage response = client.DeleteAsync(parameters).Result;
            articleconditionselect.Text = string.Empty;
            articlerefresh_Click(sender, e);
        }

        private void articleput_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void precedent_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void articleadd_Click_1(object sender, RoutedEventArgs e)
        {
            articleadd custo = new articleadd();
            custo.Show();
        }

        private void articlerefresh_Click_1(object sender, RoutedEventArgs e)
        {
            list.Clear();
            display_article();
        }

        private void articleput_Click_1(object sender, RoutedEventArgs e)
        {
            if (articledata.SelectedItem == null)
            {
                articleconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[articledata.SelectedIndex];
            var articlo = new articleput();
            List<Article> articles = new List<Article>();
            using HttpClient client = new HttpClient();
            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            string parameters = $"Display/displayarticlebyid?ID={a}";
            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = $"[{response.Content.ReadAsStringAsync().Result}]";
            JArray detail = JArray.Parse(json);
            articlo.articlename.Text = detail[0]["nameArticle"].ToString();
            int b = Convert.ToInt32(detail[0]["idProvider"]);
            articlo.articlesup.SelectedIndex= b-1;
            articlo.articledatefill.Text = detail[0]["dateFill"].ToString();
            int c  = Convert.ToInt32(detail[0]["idFamily"]);
            articlo.articlefamily.SelectedIndex = c-1;
            articlo.articlepricesup.Text = detail[0]["priceSup"].ToString();
            articlo.articleprice.Text = detail[0]["price"].ToString();
            articlo.articlevol.Text = detail[0]["volume"].ToString();
            articlo.articledegree.Text = detail[0]["degree"].ToString();
            articlo.articlesep.Text = detail[0]["grape"].ToString();
            articlo.articleladder.Text = detail[0]["ladder"].ToString();
            articlo.invputarticle.Text = a.ToString();
            articlo.Show();
            articleconditionselect.Text = string.Empty;
        }

        private void articleprec_Click(object sender, RoutedEventArgs e)
        {
            win2 win2 = new win2();
            win2.Show();
            this.Close();
        }
    }

    public class Article
    {
        public string NameArticle { get; set; }
        public string NameProvider { get; set; }
        public string DateFill { get; set; }
        public string NameFamily { get; set; }
        public string PriceSup { get; set; }
        public string Price { get; set; }
        public string Volume { get; set; }
        public string Degree { get; set; }
        public string Grape { get; set; }
        
        public string Ladder { get; set; }

        

        public Article(string nameArticle, string idProvider, string dateFill, string idFamily, string pricesup, string price, string volume, string degree, string grape, string ladder)
        {
            NameArticle = nameArticle;
            NameProvider = idProvider;
            DateFill = dateFill;
            NameFamily = idFamily;
            PriceSup = pricesup;
            Price = price;
            Volume = volume;
            Degree = degree;
            Grape = grape;
            Ladder = ladder;
        }
    }
}

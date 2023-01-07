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
using System.Xml.Linq;

namespace AppCUBES
{
    /// <summary>
    /// Logique d'interaction pour article.xaml
    /// </summary>
    public partial class article : Window
    {
        List<int> list = new List<int>();
        List<int> listidsup = new List<int>();
        List<int> listidfam = new List<int>();
        List<string> listnamesup = new List<string>();
        List<string> listnamefam = new List<string>();

        JArray detail = new JArray();

        int a = 0;

        public article()
        {
            

            InitializeComponent();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displayarticle";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidsup.Add(Convert.ToInt32(detail[i]["idProvider"]));
                    listidfam.Add(Convert.ToInt32(detail[i]["idFamily"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));

                }
            }
            display_article();
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
        public void display_article()
            
        {

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

            articledata.ItemsSource = arts;

        }

        public void display_articlebyname()
        {
            List<int> listidstock = new List<int>();

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/displayarticlebyname?name={nameart.Text}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    listidsup.Add(Convert.ToInt32(detail[i]["idProvider"]));
                    listidfam.Add(Convert.ToInt32(detail[i]["idFamily"]));
                }

            }
            display_article();
        }

        private void addarticle_Click(object sender, RoutedEventArgs e)
        {
            articleadd custo = new articleadd();
            custo.validatearticleadd.Click += refresharticle_Click;
            custo.Show();
            
        }

        public void refresharticle_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();
            listidfam.Clear();
            listidsup.Clear();
            listnamefam.Clear();
            listnamesup.Clear();
            if (a == 1)
            {
                display_articlebyname();
                return;
            }
            
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displayarticle";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidsup.Add(Convert.ToInt32(detail[i]["idProvider"]));
                    listidfam.Add(Convert.ToInt32(detail[i]["idFamily"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));

                }
            }
            display_article();

        }

        private void deletearticle_Click(object sender, RoutedEventArgs e)
        {
            if (articledata.SelectedItem == null)
            {
                articleconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }

            int a = list[articledata.SelectedIndex];
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters2 = $"Check/checkarticlestockexist?a={a}";
                HttpResponseMessage response = client.GetAsync(parameters2).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                if (json == "true")
                {
                    articleconditionselect.Text = "Impossible de supprimer l'article il a un stock";
                    return;
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Delete/delete_article?ID={a}";
                HttpResponseMessage response = client.DeleteAsync(parameters).Result;
                articleconditionselect.Text = string.Empty;
            }

            refresharticle_Click(sender, e);
        }

        

        

        private void putarticle_Click(object sender, RoutedEventArgs e)
        {
            if (articledata.SelectedItem == null)
            {
                articleconditionselect.Text = "Veuillez selectionner un élément du tableau";
                return;
            }
            int a = list[articledata.SelectedIndex];
            var articlo = new articleput();
            List<Article> articles = new List<Article>();
            int b = 0;
            int c =0;
            using (HttpClient client = new HttpClient()) 
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/displayarticlebyid?ID={a}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = $"[{response.Content.ReadAsStringAsync().Result}]";
                JArray detail = JArray.Parse(json);
                articlo.articlename.Text = detail[0]["nameArticle"].ToString();
                b = Convert.ToInt32(detail[0]["idProvider"]);
                articlo.articledatefill.Text = detail[0]["dateFill"].ToString();
                c = Convert.ToInt32(detail[0]["idFamily"]);
                
                articlo.articlepricesup.Text = detail[0]["priceSup"].ToString();
                articlo.articleprice.Text = detail[0]["price"].ToString();
                articlo.articlevol.Text = detail[0]["volume"].ToString();
                articlo.articledegree.Text = detail[0]["degree"].ToString();
                articlo.articlesep.Text = detail[0]["grape"].ToString();
                articlo.articleladder.Text = detail[0]["ladder"].ToString();
                articlo.invputarticle.Text = a.ToString();

            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getnamesupplier?ID={b}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                articlo.articlesup.SelectedValue= json;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getnamefamily?ID={c}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                articlo.articlefamily.SelectedValue = json;
            }

            articlo.Show();
            articlo.validatearticleput.Click += refresharticle_Click;
            articleconditionselect.Text = string.Empty;
        }

        private void precedentarticle_Click(object sender, RoutedEventArgs e)
        {
            if (Connect.idjobuser == 1)
            {
                win2 win2 = new win2();
                win2.Show();
                this.Close();
                return;
            }
            gestwin win1 = new gestwin();
            win1.Show();
            this.Close();


        }

        private void artbyname_Click(object sender, RoutedEventArgs e)
        {
            string json = "";
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Check/articlebynameexist?name={nameart.Text}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                json = response.Content.ReadAsStringAsync().Result;

            }

            if (nameart.Text == "" || json == "false")
            {
                articleconditionselect.Text = "L'article n'existe pas";
                return;
            }
            a = 1;
            refresharticle_Click(sender, e);
        }

        private void deselectart_Click(object sender, RoutedEventArgs e)
        {
            a = 0;
            refresharticle_Click(sender, e);
        }
    }

    public class Article
    {
        public string Nom { get; set; }
        public string Fournisseur { get; set; }
        public string Millesime { get; set; }
        public string Famille { get; set; }
        public string PrixFournisseur { get; set; }
        public string Prix { get; set; }
        public string Volume { get; set; }
        public string Degree { get; set; }
        public string Sepage { get; set; }        
        public string Recompense { get; set; }
        

        public Article(string nameArticle, string idProvider, string dateFill, string idFamily, string pricesup, string price, string volume, string degree, string grape, string ladder)
        {
            Nom = nameArticle;
            Fournisseur = idProvider;
            Millesime = dateFill;
            Famille = idFamily;
            PrixFournisseur = pricesup;
            Prix = price;
            Volume = volume;
            Degree = degree;
            Sepage = grape;
            Recompense = ladder;
        }
    }
}

using Newtonsoft.Json.Linq;
using System;
using System.Collections;
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
    /// Interaction logic for inventaire.xaml
    /// </summary>
    public partial class inventory : Window
    {
        List<int> list = new List<int>();

        public inventory()
        {
            list.Clear();
            InitializeComponent();
            display_inventaire();
        }

        public void display_inventaire() 
        {
            list.Clear();
            List<Inventaire> arts = new List<Inventaire>();

            using HttpClient client = new HttpClient();
            using HttpClient client1 = new HttpClient();
            using HttpClient client2 = new HttpClient();

            string Url = "https://localhost:7279/";
            client.BaseAddress = new Uri(Url);
            client1.BaseAddress = new Uri(Url);
            client2.BaseAddress = new Uri(Url);

            string parameters = "Display/displayarticle";


            HttpResponseMessage response = client.GetAsync(parameters).Result;
            string json = response.Content.ReadAsStringAsync().Result;

            JArray detail = JArray.Parse(json);


            for (int i = 0; i < detail.Count; i++)
            {
                string a = detail[i]["idProvider"].ToString();
                string b = detail[i]["idFamily"].ToString();
                string parameters2 = $"Display/getnamesupplier?id={a}";
                string parameters3 = $"Display/getnamefamily?id={b}";
                HttpResponseMessage response2 = client1.GetAsync(parameters2).Result;
                string json2 = response2.Content.ReadAsStringAsync().Result;
                HttpResponseMessage response3 = client2.GetAsync(parameters3).Result;
                string json3 = response3.Content.ReadAsStringAsync().Result;
                arts.Add(new Inventaire(detail[i]["nameArticle"].ToString(), json2,
                    detail[i]["dateFill"].ToString(), json3,
                    detail[i]["priceSup"].ToString(), detail[i]["price"].ToString(), detail[i]["volume"].ToString(),
                    detail[i]["degree"].ToString(), detail[i]["grape"].ToString(), detail[i]["ladder"].ToString()));
                list.Add(Convert.ToInt32(detail[i]["iD_Article"]));

            }

            gridinventory.ItemsSource = arts;
        }

        private void Moins_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    }
    public class Inventaire
    {

        public string NameArticle { get; set; }
        public string Stock { get; set; }
        public string StockActuel { get; set; }
        public string StockMin { get; set; }
        public string NameProvider { get; set; }
        public string DateFill { get; set; }
        public string NameFamily { get; set; }
        public string PriceSup { get; set; }
        public string Price { get; set; }
        public string Volume { get; set; }
        public string Degree { get; set; }
        public string Cepage { get; set; }
        public string Recompense { get; set; }






    public Inventaire(string NomArticle, string idProvider, string dateFill, string idFamily, string pricesup, string price, string volume, string degree, string cepage, string recompense, string stock,string stockActuel, string stockmin)
        {
            NameArticle = NomArticle;
            NameProvider = idProvider;
            DateFill = dateFill;
            NameFamily = idFamily;
            PriceSup = pricesup;
            Price = price;
            Volume = volume;
            Degree = degree;
            Cepage = cepage;
            Recompense = recompense;
            Stock = stock;
        

        }
    }


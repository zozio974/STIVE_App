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
using System.Windows.Media.Animation;
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
        JArray detail= new JArray();
        JArray detail1 = new JArray();
        List<int> listidsup = new List<int>();
        List<int> listidfam = new List<int>();
        List<string> listnamesup = new List<string>();
        List<string> listnamefam = new List<string>();
        public inventory()
        {
            list.Clear();
            InitializeComponent();
            display_inventaire();
        }

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
        public void display_inventaire() 
        {
            List<Inventaire> inv = new List<Inventaire>();
            List<int> listidstock = new List<int>();
            
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displaystock";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidstock.Add(Convert.ToInt32(detail[i]["idArticle"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Stock"]));
                }


            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistarticleinstock?{createparameter(listidstock)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail1 = JArray.Parse(json);
                for (int i = 0; i < detail1.Count; i++)
                {
                    listidsup.Add(Convert.ToInt32(detail1[i]["idProvider"])); 
                    listidfam.Add(Convert.ToInt32(detail1[i]["idFamily"]));

                }

            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnamesup?{createparameter(listidsup)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail2 = JArray.Parse(json);
                for (int i = 0; i < detail2.Count; i++)
                {
                    listnamesup.Add((detail2[i]).ToString());

                }
            }

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistnamefam?{createparameter(listidfam)}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail3 = JArray.Parse(json);
                for (int i = 0; i < detail3.Count; i++)
                {
                    listnamefam.Add((detail3[i]).ToString());
                }
            }
            for (int i = 0; i < detail.Count; i++)
            {
                inv.Add(new Inventaire(detail1[i]["nameArticle"].ToString(), detail[i]["stockActual"].ToString(), detail[i]["stockProv"].ToString(), detail[i]["stockMin"].ToString(), listnamesup[i], detail1[i]["dateFill"].ToString(), listnamefam[i], detail1[i]["priceSup"].ToString(), detail1[i]["price"].ToString(),detail1[i]["volume"].ToString(),detail1[i]["degree"].ToString(),detail1[i]["grape"].ToString(),detail1[i]["ladder"].ToString()));
                

            }
                



            gridinventory.ItemsSource = inv;

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

        public string Nom { get; set; }
        public string StockActuel { get; set; }
        public string StockProvisoire{ get; set; }
        public string StockMinimum { get; set; }
        public string Fournisseur { get; set; }
        public string Millesime { get; set; }
        public string Famille { get; set; }
        public string PrixFournisseur { get; set; }
        public string Prix { get; set; }
        public string Volume { get; set; }
        public string Degree { get; set; }
        public string Cepage { get; set; }
        public string Recompense { get; set; }






    public Inventaire(string NomArticle, string stockactuel, string stockprovisoire, string stockmin, string idProvider, string dateFill, string idFamily, string pricesup, string price, string volume, string degree, string cepage, string recompense)
        {
            Nom = NomArticle;
            StockActuel = stockactuel;
            StockProvisoire = stockprovisoire;
            StockMinimum = stockmin;
            Fournisseur = idProvider;
            Millesime = dateFill;
            Famille = idFamily;
            PrixFournisseur = pricesup;
            Prix = price;
            Volume = volume;
            Degree = degree;
            Cepage = cepage;
            Recompense = recompense;
        

        }
    }


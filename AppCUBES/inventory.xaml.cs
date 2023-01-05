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
        List<int> liststmin = new List<int>();
        List<int> listst = new List<int>();


        JArray detail = new JArray();
        JArray detail1 = new JArray();
        List<int> listidsup = new List<int>();
        List<int> listidfam = new List<int>();
        List<string> listnamesup = new List<string>();
        List<string> listnamefam = new List<string>();
        int sel = 0 ;
        public inventory()
        {
            list.Clear();
            InitializeComponent();
            
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
                    liststmin.Add(Convert.ToInt32(detail[i]["stockMin"]));
                    listst.Add(Convert.ToInt32(detail[i]["stockActual"]));
                }


            }
            display_inventaire(listidstock);
            List<string> listsearchsup = new List<string>();
            List<string> listsearchfam = new List<string>();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displaysup";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail5 = JArray.Parse(json);
                for (int i = 0; i < detail5.Count(); i++)
                {
                    listsearchsup.Add(detail5[i]["name"].ToString());
                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = "Display/displayfamily";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                JArray detail6 = JArray.Parse(json);
                for (int i = 0; i < detail6.Count(); i++)
                {
                    listsearchfam.Add(detail6[i]["nameFamily"].ToString());
                }
            }
            trichoicesup.ItemsSource = listsearchsup;
            trichoicefam.ItemsSource = listsearchfam;


        }
        private bool isinteger(string str)
        {
            int res = 0;
            return int.TryParse(str, out res);
        }
        public string createparameter(List<int> listo)
        {
            string a = "";
            foreach (int item in listo)
            {
                a += $"listOfIds={item}&";
            }
            var output = a.Remove(a.Length - 1);
            return output;
        }

        public void display_inventaire(List<int> listo) 
        {
            List<Inventaire> inv = new List<Inventaire>();
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getlistarticleinstock?{createparameter(listo)}";
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
        public int getIdsup(string namesup)
        {
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/?getidsupplier?name={namesup}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                return Convert.ToInt32(json);

            }

        }
        public void display_inventairebysup()
        {
            List<int> listidstock = new List<int>();

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                int va = getIdsup(trichoicesup.SelectedValue.ToString());
                string parameters = "Display/displayarticlebySupplier?ID={va}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidstock.Add(Convert.ToInt32(detail[i]["idArticle"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Stock"]));
                    liststmin.Add(Convert.ToInt32(detail[i]["stockMin"]));
                    listst.Add(Convert.ToInt32(detail[i]["stockActual"]));
                }


            }
            display_inventaire(listidstock);
        }



        private void Moins_Click(object sender, RoutedEventArgs e)
        {
            int a = list[gridinventory.SelectedIndex];

            if (gridinventory.SelectedItem == null)
            {
                resinventory.Text = "Veuillez selectionner un élément du tableau";

                return;
            }
            if (invquantsous.Text == "")
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "https://localhost:7279/";
                    client.BaseAddress = new Uri(Url);
                    string parameters = $"PutStock/dropstockunitid?idstock={a}";
                    HttpResponseMessage response = client.PutAsync(parameters, null).Result;
                    sel = gridinventory.SelectedIndex;
                    inventoryrefresh_Click(sender, e);

                    return;

                }
            }
            if (isinteger(invquantsous.Text) == false)
            {
                resinventory.Text = "Le champs est mal entré";

                return;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"PutStock/dropstockmulid?idstock={a}&i={invquantsous.Text}";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
                sel = gridinventory.SelectedIndex;
                inventoryrefresh_Click(sender, e);

                return;

            }

        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            int a = list[gridinventory.SelectedIndex];

            if (gridinventory.SelectedItem == null)
            {
                resinventory.Text = "Veuillez selectionner un élément du tableau";
                
                return;
            }
            
            if (invquantplus.Text == "")
            {
                using (HttpClient client = new HttpClient())
                {
                    string Url = "https://localhost:7279/";
                    client.BaseAddress = new Uri(Url);
                    string parameters = $"PutStock/addstockunitid?idstock={a}";
                    HttpResponseMessage response = client.PutAsync(parameters, null).Result;
                    sel = gridinventory.SelectedIndex;
                    inventoryrefresh_Click(sender, e);

                    return;

                }
            }
            if (isinteger(invquantplus.Text) == false)
            {
                resinventory.Text = "Le champs est mal entré";
                
                return;
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"PutStock/addstockmulid?idstock={a}&i={invquantplus.Text}";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
                sel = gridinventory.SelectedIndex;
                inventoryrefresh_Click(sender, e);

                return;

            }

        }

        private void inventoryrefresh_Click(object sender, RoutedEventArgs e)
        {
            list.Clear();

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
                    liststmin.Add(Convert.ToInt32(detail[i]["stockMin"]));
                    listst.Add(Convert.ToInt32(detail[i]["stockActual"]));
                }


            }
            display_inventaire(list);
            gridinventory.SelectedIndex = sel;
            invquantplus.Text = "";
            invquantsous.Text = "";
            putstock1.Text = "";
        }

        private void stockmin_Click(object sender, RoutedEventArgs e)
        {
            int a = list[gridinventory.SelectedIndex];
            if (isinteger(putstockmin.Text) == false || putstockmin.Text=="")
            {
                resinventory.Text = "Le champs est mal entré";
                return;
            }
            using (HttpClient client = new HttpClient())
            {

                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"PutStock/putstockmin?idstock={a}&i={putstockmin.Text}";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
                sel = gridinventory.SelectedIndex;
                inventoryrefresh_Click(sender, e);

                return;

            }
        }

        private void gridinventory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(gridinventory.SelectedIndex >= 0)
            {
                int b = liststmin[gridinventory.SelectedIndex];
                putstockmin.Text = b.ToString();
                int c = listst[gridinventory.SelectedIndex];
                putstock1.Text = c.ToString();
            }
            
        }

        private void invretmenu_Click(object sender, RoutedEventArgs e)
        {
            win2 win = new win2();
            win.Show();
            this.Close();
        }

        

        private void putstock_Click(object sender, RoutedEventArgs e)
        {
            int a = list[gridinventory.SelectedIndex];
            if (isinteger(putstock1.Text) == false || putstock1.Text == "")
            {
                resinventory.Text = "Le champs est mal entré";
                return;
            }
            using (HttpClient client = new HttpClient())
            {

                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"PutStock/putstock?idstock={a}&i={putstock1.Text}";
                HttpResponseMessage response = client.PutAsync(parameters, null).Result;
                sel = gridinventory.SelectedIndex;
                inventoryrefresh_Click(sender, e);

                return;

            }
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


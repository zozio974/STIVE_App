using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
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
using System.Xml.Linq;



namespace AppCUBES
{

    /// <summary>
    /// Interaction logic for inventaire.xaml
    /// </summary>
    public partial class inventwin : Window
    {
        List<int> list = new List<int>();
        List<int> listactual = new List<int>();
        List<int> listmin = new List<int>();

        int resup = 0;
        int refam = 0;
        int rename = 0;
        JArray detail = new JArray();
        List<int> listidsup = new List<int>();
        List<int> listidfam = new List<int>();
        List<string> listnamesup = new List<string>();
        List<string> listnamefam = new List<string>();
        int sel = 0;
        public inventwin()
        {
            list.Clear();
            InitializeComponent();

            List<int> listidstock = new List<int>();

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
                    listidstock.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    listactual.Add(Convert.ToInt32(detail[i]["stockActual"]));
                    listmin.Add(Convert.ToInt32(detail[i]["stockMin"]));
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
                JArray detail10 = JArray.Parse(json);
                for (int i = 0; i < detail10.Count; i++)
                {
                    listidsup.Add(Convert.ToInt32(detail10[i]["idProvider"]));
                    listidfam.Add(Convert.ToInt32(detail10[i]["idFamily"]));

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
                inv.Add(new Inventaire(detail[i]["nameArticle"].ToString(), detail[i]["stockActual"].ToString(), detail[i]["stockProv"].ToString(), detail[i]["stockMin"].ToString(), listnamesup[i], detail[i]["dateFill"].ToString(), listnamefam[i], detail[i]["priceSup"].ToString(), detail[i]["price"].ToString(), detail[i]["volume"].ToString(), detail[i]["degree"].ToString(), detail[i]["grape"].ToString(), detail[i]["ladder"].ToString()));

            }
            gridinventory.ItemsSource = inv;
        }
        public int getIdsup(string namesup)
        {
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getidsupplier?name={namesup}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                return Convert.ToInt32(json);

            }

        }
        public int getIdfam(string namefam)
        {
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/getidfamily?name={namefam}";
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
                string parameters = $"Tri/displayarticlebySupplier?ID={va}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidstock.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    listactual.Add(Convert.ToInt32(detail[i]["stockActual"]));
                    listmin.Add(Convert.ToInt32(detail[i]["stockMin"]));

                }


            }
            display_inventaire(listidstock);
        }
        public void display_inventairebyfam()
        {
            List<int> listidstock = new List<int>();

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                int va = getIdfam(trichoicefam.SelectedValue.ToString());
                string parameters = $"Tri/displayarticlebyFamily?ID={va}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidstock.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    listactual.Add(Convert.ToInt32(detail[i]["stockActual"]));
                    listmin.Add(Convert.ToInt32(detail[i]["stockMin"]));

                }


            }
            display_inventaire(listidstock);
        }
        public void display_inventairebyfamsup()
        {
            List<int> listidstock = new List<int>();

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                int va = getIdsup(trichoicesup.SelectedValue.ToString());
                int vb = getIdfam(trichoicefam.SelectedValue.ToString());
                string parameters = $"Tri/displayarticlebySupFam?idsup={va}&idfam={vb}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidstock.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    listactual.Add(Convert.ToInt32(detail[i]["stockActual"]));
                    listmin.Add(Convert.ToInt32(detail[i]["stockMin"]));

                }


            }
            display_inventaire(listidstock);
        }

        public void display_inventairebyname()
        {
            List<int> listidstock = new List<int>();

            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Display/displayarticlebyname?name={researchname.Text}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                detail = JArray.Parse(json);
                for (int i = 0; i < detail.Count; i++)
                {
                    listidstock.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                    listactual.Add(Convert.ToInt32(detail[i]["stockActual"]));
                    listmin.Add(Convert.ToInt32(detail[i]["stockMin"]));

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
            listactual.Clear();
            listmin.Clear();
            listidfam.Clear();
            listidsup.Clear();
            listnamefam.Clear();
            listnamesup.Clear();

            List<int> listidstock = new List<int>();
            if (refam == 0 && resup == 0 && rename == 0)
            {
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
                        listidstock.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                        list.Add(Convert.ToInt32(detail[i]["iD_Article"]));
                        listactual.Add(Convert.ToInt32(detail[i]["stockActual"]));
                        listmin.Add(Convert.ToInt32(detail[i]["stockMin"]));


                    }


                }
                display_inventaire(listidstock);


            }
            if (refam == 1 && resup == 0 && rename == 0)
            {
                display_inventairebyfam();

            }
            if (refam == 0 && resup == 1 && rename == 0)
            {
                display_inventairebysup();
            }
            if (refam == 1 && resup == 1 && rename == 0)
            {
                display_inventairebyfamsup();
            }
            if (rename == 1)
            {
                display_inventairebyname();

            }

            gridinventory.SelectedIndex = sel;
            invquantplus.Text = "";
            invquantsous.Text = "";
            putstock1.Text = "";
            putstockmin.Text = "";




        }

        private void stockmin_Click(object sender, RoutedEventArgs e)
        {
            int a = list[gridinventory.SelectedIndex];
            if (isinteger(putstockmin.Text) == false || putstockmin.Text == "")
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
            if (gridinventory.SelectedIndex >= 0)
            {
                int resactual = listactual[gridinventory.SelectedIndex];
                putstock1.Text = resactual.ToString();
                int resamin = listmin[gridinventory.SelectedIndex];
                putstockmin.Text = resamin.ToString();
            }

        }

        private void invretmenu_Click(object sender, RoutedEventArgs e)
        {
            Connect.idjobuser = 0;
            Connect.iduser = 0;
            MainWindow win = new MainWindow();
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

        private void buttontrisup_Click(object sender, RoutedEventArgs e)
        {
            if (trichoicesup.SelectedValue == null)
            {
                resinventory.Text = "Aucun champs sélectionné";
                return;
            }
            resup = 1;
            rename = 0;
            researchname.Text = "";
            inventoryrefresh_Click(sender, e);


        }

        private void buttontrifam_Click(object sender, RoutedEventArgs e)
        {
            if (trichoicefam.SelectedValue == null)
            {
                resinventory.Text = "Aucun champs sélectionné";
                return;
            }
            refam = 1;
            rename = 0;
            researchname.Text = "";
            inventoryrefresh_Click(sender, e);
        }

        private void detribut_Click(object sender, RoutedEventArgs e)
        {
            refam = 0;
            resup = 0;
            rename = 0;
            researchname.Text = "";
            inventoryrefresh_Click(sender, e);

        }

        private void researchnamebut_Click(object sender, RoutedEventArgs e)
        {
            string json = "";
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Check/articlebynameexist?name={researchname.Text}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                json = response.Content.ReadAsStringAsync().Result;

            }

            if (researchname.Text == "" || json == "false")
            {
                resinventory.Text = "L'article n'existe pas";
                return;
            }
            rename = 1;
            inventoryrefresh_Click(sender, e);
        }

    }

    }
   

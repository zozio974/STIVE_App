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
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppCUBES.command;
using Newtonsoft.Json.Linq;

namespace AppCUBES
{
    static class Connect
    {
        public static int iduser;
        public static int idjobuser;

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Connect/connectemp?username={login.Text}&password={password.Password}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = response.Content.ReadAsStringAsync().Result;
                if (json == "false")
                {
                    res.Text = "Mauvais identifiant ou mot de passe, réessayez";
                    return;

                }
            }
            using (HttpClient client = new HttpClient())
            {
                string Url = "https://localhost:7279/";
                client.BaseAddress = new Uri(Url);
                string parameters = $"Connect/getjobbylogin?name={login.Text}";
                HttpResponseMessage response = client.GetAsync(parameters).Result;
                string json = $"[{response.Content.ReadAsStringAsync().Result}]";
                JArray detail = JArray.Parse(json);
                Connect.iduser = Convert.ToInt32(detail[0]["iD_User"]);
                Connect.idjobuser = Convert.ToInt32(detail[0]["idjob"]);
                
                if (Connect.idjobuser == 1)
                {
                    win1 win1 = new win1();
                    win1.Show();
                    this.Close();
                    return;

                }
                if (Connect.idjobuser == 2)
                {
                    win2 win1 = new win2();
                    win1.Show();
                    this.Close();
                    return;
                }
                if (Connect.idjobuser == 3)
                {
                    inventory win1 = new inventory();
                    win1.Show();
                    this.Close();
                    return;

                }
                if (Connect.idjobuser == 4)
                {
                    dispcommand win1 = new dispcommand();
                    win1.Show();
                    this.Close();
                    return;
                }
            }
           
        }

       
    }
}

using DefaultWidgets.Utils;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace GrassCutter_Proxy
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowVM { Host = "ubuntu.local", Port = "11451" };
        }
    }

    public class MainWindowVM : ObservableObject
    {
        private Common.ProxyController Controller;
        public MainWindowVM()
        {
            LoadedCMD = new RelayCommand(PageLoaded);
            StartCMD = new RelayCommand(StartProxy);
            TestCMD = new AsyncRelayCommand(TestAsync);


            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, error) =>
            {
                return true;
            };
        }
        public ICommand LoadedCMD { get; }
        public ICommand StartCMD { get; }
        public ICommand TestCMD { get; }

        private string _port;

        public string Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        private string _host;

        public string Host
        {
            get => _host;
            set => SetProperty(ref _host, value);
        }

        private bool _state;

        public bool State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        private async Task TestAsync()
        {

            var request = new Request();
            try
            {
                var r=await request.Get("https://ys.mihoyo.com/hk4e_global/mdk/shield/api/loadConfig?client=3&game_key=hk4e_global");
            
            
                MessageBox.Show(r);
            
            }catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void PageLoaded()
        {
            State = false;

        }

        public void StartProxy()
        {
            if (State)
            {
                Controller.Stop();
                State = false;

            }
            else
            {
                Controller = new Common.ProxyController(port: Port, host: Host);
                Common.Global.controller = Controller;

                Controller.Start();
                State = true;
            }
        }
    }


}

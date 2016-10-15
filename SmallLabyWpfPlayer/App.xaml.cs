using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SmallLabyWpfPlayer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var model = new ClientModel();
            var main_view_model = new MainViewModel(model);
            main_view_model.PlayerName = "<enter player name>";// ConfigurationManager.AppSettings.Get("PathToData");

            var main_view = new MainView();
            main_view.Closing += (o, ce) => { main_view_model.Disconnect(); };
            main_view.DataContext = main_view_model;
            main_view.Show();
        }
    }
}

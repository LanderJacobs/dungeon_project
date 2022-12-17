using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Dungeon_WPF.ViewModels;
using Dungeon_WPF.Data;

namespace Dungeon_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DatabaseFacade facade = new DatabaseFacade(new DungeonEntities());
            facade.EnsureCreated();

            Views.StartView view = new Views.StartView();
            StartViewModel vm = new StartViewModel(view);
            view.DataContext = vm;
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            view.Show();
        }
    }
}

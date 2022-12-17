using Dungeon_WPF.ViewModels;
using Dungeon_WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dungeon_WPF.HelperFiles
{
    public class HelpMethods
    {
        public void Message(string message)
        {
            PopupView view = new PopupView();
            PopupViewModel vm = new PopupViewModel(view,message);
            view.DataContext = vm;
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            view.ShowDialog();
        }    
    }
}

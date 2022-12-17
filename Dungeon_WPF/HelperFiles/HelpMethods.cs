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

        public bool AskQuestion(string question, string yes, string no)
        {
            ChoiceView view = new ChoiceView();
            ChoiceViewModel vm = new ChoiceViewModel(view, question, yes, no);
            view.DataContext = vm;
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            view.ShowDialog();

            return view.DialogResult == true? true: false;
        }
    }
}

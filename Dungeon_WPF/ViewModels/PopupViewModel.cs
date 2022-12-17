using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dungeon_WPF.HelperFiles;

namespace Dungeon_WPF.ViewModels
{
    public class PopupViewModel: BasisViewModel
    {
        public Window view;
        private string _text;
        
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                NotifyPropertyChanged();
            }
        }

        #region helperfunctions
        public override string this[string columnName]
        {
            get
            {
                return "";
            }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            switch (parameter.ToString())
            {
                case "Close":
                    view.Close();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public PopupViewModel(Window _view, string message)
        {
            view = _view;
            Text = message;
        }
    }
}

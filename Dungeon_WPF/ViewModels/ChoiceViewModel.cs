using Dungeon_WPF.HelperFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dungeon_WPF.ViewModels
{
    public class ChoiceViewModel: BasisViewModel
    {
        public Window view;
        private string _question;
        private string _yes;
        private string _no;

        public string Question
        {
            get { return _question; }
            set
            {
                _question = value;
                NotifyPropertyChanged();
            }
        }
        public string No
        {
            get { return _no; }
            set
            {
                _no = value;
                NotifyPropertyChanged();
            }
        }
        public string Yes
        {
            get { return _yes; }
            set
            {
                _yes = value;
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
                case "Deny":
                    view.DialogResult = false;
                    view.Close();
                    break;
                case "Admit":
                    view.DialogResult = true;
                    view.Close();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public ChoiceViewModel(Window _view, string question, string trueAnswer, string falseAnswer)
        {
            view = _view;
            Yes = trueAnswer + " [x]";
            No = falseAnswer + " [w]";
            Question = question;
        }
    }
}

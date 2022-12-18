using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dungeon_WPF.HelperFiles;
using Dungeon_WPF.Views;

namespace Dungeon_WPF.ViewModels
{
    internal class StartViewModel: BasisViewModel
    {
        public Window view;

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
                case "Start":
                    SelectionView _view = new SelectionView();
                    SelectionViewModel vm = new SelectionViewModel(_view);
                    _view.DataContext = vm;
                    _view.WindowStartupLocation= WindowStartupLocation.CenterScreen;
                    _view.Show();
                    view.Close();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public StartViewModel(Window _view)
        {
            view = _view;
        }
    }
}

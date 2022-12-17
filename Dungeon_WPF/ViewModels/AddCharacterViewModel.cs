using Dungeon_WPF.HelperFiles;
using Dungeon_WPF.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dungeon_WPF.ViewModels
{
    public class AddCharacterViewModel: BasisViewModel
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
                case "Cancel":
                    OpenSelectionView();
                    break;
                case "Add":
                    //write add method
                    OpenSelectionView();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public AddCharacterViewModel(Window _view)
        {
            view = _view;
        }

        public void OpenSelectionView()
        {
            SelectionView _view = new SelectionView();
            SelectionViewModel vm = new SelectionViewModel(_view);
            _view.DataContext = vm;
            _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _view.Show();
            view.Close();
        }
    }
}

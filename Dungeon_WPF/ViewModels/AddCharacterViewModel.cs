using Dungeon_WPF.HelperFiles;
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
                case "Start":
                    //write your method;
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
    }
}

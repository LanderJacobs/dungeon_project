using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dungeon_WPF.Data;
using Dungeon_WPF.Data.UnitOfWork;
using Dungeon_WPF.DomainModels;
using Dungeon_WPF.HelperFiles;
using Dungeon_WPF.Views;

namespace Dungeon_WPF.ViewModels
{
    public class SelectionViewModel: BasisViewModel
    {
        public Window view;
        IUnitOfWork unitofwork = new UnitOfWork(new DungeonEntities());
        private List<Character> _characterlist;
        private Character _selectedcharacter;

        public Character SelectedCharacter
        {
            get { return _selectedcharacter; }
            set
            {
                _selectedcharacter = value;
                NotifyPropertyChanged();
            }
        }
        public List<Character> CharacterList
        {
            get { return _characterlist; }
            set 
            { 
                _characterlist = value;
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
                case "Add":
                    AddCharacterView _view = new AddCharacterView();
                    AddCharacterViewModel vm = new AddCharacterViewModel(_view);
                    _view.DataContext = vm;
                    _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    _view.Show();
                    view.Close();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public SelectionViewModel(Window _view)
        {
            view = _view;
            CharacterList = unitofwork.CharacterRepo.GetAll().ToList();
        }
    }
}

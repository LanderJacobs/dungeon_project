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
        HelpMethods help = new HelpMethods();
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
                    CreateCharacter();
                    break;
                case "Choose":
                    OpenDungeons();
                    break;
                case "Delete":
                    DeleteCharacter();
                    break;
                case "GoUp":
                    try
                    {
                        GoUp();
                    }
                    catch (Exception)
                    {
                        help.Message("There is a problem with navigating the characters");
                    }
                    break;
                case "GoDown":
                    try
                    {
                        GoDown();
                    }
                    catch (Exception)
                    {
                        help.Message("There is a problem with navigating the characters");
                    }
                    break;
                case "Close":
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
            SelectedCharacter = CharacterList[0];
        }

        public void OpenDungeons()
        {
            if (SelectedCharacter != null)
            {
                DungeonSelectionView _view = new DungeonSelectionView();
                DungeonSelectionViewModel vm = new DungeonSelectionViewModel(_view, SelectedCharacter);
                _view.DataContext = vm;
                _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _view.Show();
                view.Close();
            }
            else
            {
                help.Message("Don't forget to select a character first");
            }
        }

        public void DeleteCharacter()
        {
            if (SelectedCharacter != null)
            {
                bool answer = help.AskQuestion($"Are you sure you want to delete {SelectedCharacter.Name}?", "Definitely", "No");
                if (answer)
                {
                    unitofwork.CharacterRepo.Delete(SelectedCharacter);
                    int save = unitofwork.Save();
                    if (save > 0)
                    {
                        help.Message($"{SelectedCharacter.Name} was deleted");
                        SelectedCharacter = null;
                        CharacterList = unitofwork.CharacterRepo.GetAll().ToList();
                    }
                    else
                    {
                        help.Message($"Oops, we couldn't delete {SelectedCharacter.Name} right now");
                    }
                }

                try
                {
                    SelectedCharacter = CharacterList[0];
                }
                catch (Exception)
                {
                    help.Message("You need to first make a character to play");
                    CreateCharacter();
                }

            }
            else
            {
                help.Message("First select a character before deleting it");
            }
        }

        public void CreateCharacter()
        {
            AddCharacterView _view = new AddCharacterView();
            AddCharacterViewModel vm = new AddCharacterViewModel(_view);
            _view.DataContext = vm;
            _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _view.Show();
            view.Close();
        }

        public void GoUp()
        {
            for (int i = 0; i < CharacterList.Count; i++)
            {
                if (SelectedCharacter.Equals(CharacterList[i]))
                {
                    if (i == 0)
                    {
                        SelectedCharacter = CharacterList[CharacterList.Count - 1];
                    }
                    else
                    {
                        SelectedCharacter = CharacterList[i-1];
                    }
                    break;
                }
            }
        }

        public void GoDown()
        {
            for (int i = 0; i < CharacterList.Count; i++)
            {
                if (SelectedCharacter == CharacterList[i])
                {
                    if (i == CharacterList.Count - 1)
                    {
                        SelectedCharacter = CharacterList[0];
                    }
                    else
                    {
                        SelectedCharacter = CharacterList[i+ 1];
                    }
                    break;
                }
            }
        }
    }
}

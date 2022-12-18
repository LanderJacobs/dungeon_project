using System;
using System.Collections.Generic;
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
    public class DungeonSelectionViewModel: BasisViewModel
    {
        public Window view;
        HelpMethods help = new HelpMethods();
        IUnitOfWork unitofwork = new UnitOfWork(new DungeonEntities());
        public Character character;
        private string _name;
        private string _attack;
        private string _health;
        private string _speed;
        private string _money;
        private List<Dungeon> _dungeonlist;
        private Dungeon _selecteddungeon;
        private string _showbuttons;

        public string ShowButtons
        {
            get { return _showbuttons; }
            set 
            { 
                _showbuttons = value;
                NotifyPropertyChanged();
            }
        }
        public Dungeon SelectedDungeon
        {
            get { return _selecteddungeon; }
            set
            {
                _selecteddungeon = value;
                NotifyPropertyChanged();
            }
        }
        public List<Dungeon> DungeonList
        {
            get { return _dungeonlist; }
            set
            {
                _dungeonlist = value;
                NotifyPropertyChanged();
            }
        }
        public string Money
        {
            get { return _money; }
            set
            {
                _money = value;
                CheckMoney();
                NotifyPropertyChanged();
            }
        }
        public string Speed
        {
            get { return _speed; }
            set 
            { 
                _speed = value;
                NotifyPropertyChanged();
            }
        }
        public string Health
        {
            get { return _health; }
            set
            {
                _health = value;
                NotifyPropertyChanged();
            }
        }
        public string Attack
        {
            get { return _attack; }
            set
            {
                _attack = value;
                NotifyPropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set 
            { 
                _name = value;
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
                case "Choose":
                    OpenDungeon();
                    break;
                case "Cancel":
                    SelectionView _view = new SelectionView();
                    SelectionViewModel vm = new SelectionViewModel(_view);
                    _view.DataContext = vm;
                    _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    _view.Show();
                    view.Close();
                    break;
                case "AddAttack":
                    AddAttack();
                    break;
                case "AddHealth":
                    AddHealth();
                    break;
                case "AddSpeed":
                    AddSpeed();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public DungeonSelectionViewModel(Window _view, Character _character)
        {
            view = _view;
            // This is done so we can see the updated Money-count
            character = unitofwork.CharacterRepo.Get(x => x.Id == _character.Id);
            Name = character.Name;
            Money = character.Money.ToString();
            Attack= character.Attack.ToString();
            Health = character.Health.ToString();
            Speed= character.Speed.ToString();
            CheckMoney();

            DungeonList = unitofwork.DungeonRepo.GetAll().ToList();
        }

        public void OpenDungeon()
        {
            if (SelectedDungeon != null)
            {
                DungeonView _view = new DungeonView();
                DungeonViewModel vm = new DungeonViewModel(_view, SelectedDungeon, character);
                _view.DataContext = vm;
                _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _view.Show();
                view.Close();
            }
            else
            {
                help.Message("Don't forget to select a Dungeon first!");
            }
        }

        public void AddAttack()
        {
            bool answer = help.AskQuestion("Are you sure you want to improve your Attack, this will cost you 50 gold", "Yes", "No");
            if (answer)
            {
                character.Attack++;
                character.Money -= 50;
                unitofwork.CharacterRepo.Update(character);
                int save = unitofwork.Save();
                if (save > 0)
                {
                    Money = character.Money.ToString();
                    Attack = character.Attack.ToString();
                    help.Message("Successfully improved your Attack!");
                }
                else
                {
                    help.Message("Oops something went wrong, cannot upgrade your Attack right now");
                    character.Attack--;
                    character.Money += 50;
                }
            }
        }

        public void AddHealth()
        {
            bool answer = help.AskQuestion("Are you sure you want to improve your Health, this will cost you 50 gold", "Yes", "No");
            if (answer)
            {
                character.Health++;
                character.Money -= 50;
                unitofwork.CharacterRepo.Update(character);
                int save = unitofwork.Save();
                if (save > 0)
                {
                    Money = character.Money.ToString();
                    Health = character.Health.ToString();
                    help.Message("Successfully improved your Health!");
                }
                else
                {
                    help.Message("Oops something went wrong, cannot upgrade your Health right now");
                    character.Health--;
                    character.Money += 50;
                }
            }
        }

        public void AddSpeed()
        {
            bool answer = help.AskQuestion("Are you sure you want to improve your Speed, this will cost you 50 gold", "Yes", "No");
            if (answer)
            {
                character.Speed++;
                character.Money -= 50;
                unitofwork.CharacterRepo.Update(character);
                int save = unitofwork.Save();
                if (save > 0)
                {
                    Money = character.Money.ToString();
                    Speed = character.Speed.ToString();
                    help.Message("Successfully improved your Speed!");
                }
                else
                {
                    help.Message("Oops something went wrong, cannot upgrade your Speed right now");
                    character.Speed--;
                    character.Money += 50;
                }
            }
        }

        public void CheckMoney()
        {
            if (character.Money >= 50)
            {
                ShowButtons = "Visible";
            }
            else
            {
                ShowButtons = "Hidden";
            }
        }
    }
}

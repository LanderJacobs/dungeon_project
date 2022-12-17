using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Dungeon_WPF.DomainModels;
using Dungeon_WPF.HelperFiles;

namespace Dungeon_WPF.ViewModels
{
    public class DungeonSelectionViewModel: BasisViewModel
    {
        public Window view;
        public Character character;
        private string _name;
        private string _attack;
        private string _health;
        private string _speed;
        private string _money;
        
        public string Money
        {
            get { return _money; }
            set
            {
                _money = value;
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
                case "Start":
                    //write your method;
                    break;
                default:
                    break;
            }
        }

        #endregion

        public DungeonSelectionViewModel(Window _view, Character _character)
        {
            view = _view;
            character = _character;
            Name = character.Name;
            Money = character.Money.ToString();
            Attack= character.Attack.ToString();
            Health = character.Health.ToString();
            Speed= character.Speed.ToString();
        }
    }
}

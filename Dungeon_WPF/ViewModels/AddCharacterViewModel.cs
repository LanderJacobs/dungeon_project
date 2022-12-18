using Dungeon_WPF.Data;
using Dungeon_WPF.Data.UnitOfWork;
using Dungeon_WPF.DomainModels;
using Dungeon_WPF.HelperFiles;
using Dungeon_WPF.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dungeon_WPF.ViewModels
{
    public class AddCharacterViewModel: BasisViewModel
    {
        public Window view;
        IUnitOfWork unitofwork = new UnitOfWork(new DungeonEntities());
        HelpMethods help = new HelpMethods();

        private List<string> _classlist;
        private string _text;
        private string _name;
        private string _classname;
        private int _points; 
        private int _attack; 
        private int _health; 
        private int _speed;

        public List<string> ClassList
        {
            get { return _classlist; }
            set
            {
                _classlist = value;
                NotifyPropertyChanged();
            }
        }
        public int Speed
        {
            get { return _speed; }
            set 
            { 
                _speed = value;
                NotifyPropertyChanged();
            }
        }
        public int Health
        {
            get { return _health; }
            set 
            { 
                _health = value;
                NotifyPropertyChanged();
            }
        }
        public int Attack
        {
            get { return _attack; }
            set
            {
                _attack = value;
                NotifyPropertyChanged();
            }
        }
        public int Points
        {
            get { return _points; }
            set 
            { 
                _points = value;
                NotifyPropertyChanged();
            }
        }
        public string ClassName
        {
            get { return _classname; }
            set 
            { 
                _classname = value;
                ChangeText();
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
                if (columnName == "Name" && string.IsNullOrEmpty(Name))
                {
                    return "Don't forget to fill in a name";
                }
                if (columnName == "ClassName" && string.IsNullOrEmpty(ClassName))
                {
                    return "Don't forget to select a Class";
                }
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
                    AddCharacter();
                    break;
                case "RemoveAttack":
                    RemoveAttack();
                    break;
                case "AddAttack":
                    AddAttack();
                    break;
                case "RemoveHealth":
                    RemoveHealth();
                    break;
                case "AddHealth":
                    AddHealth();
                    break;
                case "RemoveSpeed":
                    RemoveSpeed();
                    break;
                case "AddSpeed":
                    AddSpeed();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public AddCharacterViewModel(Window _view)
        {
            view = _view;

            ClassList = new List<string>();
            ClassList.Add("Knight");
            ClassList.Add("Fighter");
            ClassList.Add("Rogue");

            Text = "Select a class to get more information";
            Points = 10;
            Attack = 5;
            Speed = 5;
            Health = 10;
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

        public void ChangeText()
        {
            if (ClassName == "Knight")
            {
                Text = "The knight fights using his attack stat";
            }
            else if (ClassName == "Fighter")
            {
                Text = "The Fighter fights using his health stat";
            }
            else if (ClassName == "Rogue")
            {
                Text = "The Rogue fights using his speed stat";
            }
            else
            {
                Text = "Select a Class to see what it attacks whith";
            }
        }

        public void RemoveAttack()
        {
            if (Attack <= 5)
            {
                help.Message("Attack can not go lower");
            }
            else
            {
                Attack--;
                Points++;
            }
        }

        public void AddAttack()
        {
            if (Points <= 0)
            {
                help.Message("You don't have enough points to add");
            }
            else
            {
                Attack++;
                Points--;
            }
        }

        public void RemoveHealth()
        {
            if (Health <= 10)
            {
                help.Message("Health can not go lower");
            }
            else
            {
                Health--;
                Points++;
            }
        }

        public void AddHealth()
        {
            if (Points <= 0)
            {
                help.Message("You don't have enough points to add");
            }
            else
            {
                Health++;
                Points--;
            }
        }

        public void RemoveSpeed()
        {
            if (Speed <= 5)
            {
                help.Message("Speed can not go lower");
            }
            else
            {
                Speed--;
                Points++;
            }
        }

        public void AddSpeed()
        {
            if (Points <= 0)
            {
                help.Message("You don't have enough points to add");
            }
            else
            {
                Speed++;
                Points--;
            }
        }

        public void AddCharacter()
        {
            if (this.IsGeldig())
            {
                Character NewCharacter = new Character{ 
                    Name = this.Name,
                    ClassName = this.ClassName,
                    Money = 0,
                    Attack = this.Attack,
                    Health = this.Health,
                    Speed = this.Speed
                };

                unitofwork.CharacterRepo.Add(NewCharacter);
                int save = unitofwork.Save();
                if (save > 0)
                {
                    help.Message("Your character has been added");
                    OpenSelectionView();
                }
                else
                {
                    help.Message("An error has occured, please try again later");
                }
            }
            else
            {
                help.Message(this.Error);
            }
        }
    }
}

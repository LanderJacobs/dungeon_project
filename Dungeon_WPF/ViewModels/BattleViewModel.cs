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
using Microsoft.VisualBasic;

namespace Dungeon_WPF.ViewModels
{
    public class BattleViewModel : BasisViewModel
    {
        public Window view;
        IUnitOfWork unitofwork = new UnitOfWork(new DungeonEntities());
        public Character character;
        public Enemy enemy;
        private string _text;
        private int _health;
        private int _enemyHealth;
        private string _name;
        private string _enemyName;
        private bool _allowbutton;
        public int turn = 1;
        public bool block = false;

        public bool AllowButton
        {
            get { return _allowbutton; }
            set 
            { 
                _allowbutton = value;
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
        public int EnemyHealth
        {
            get { return _enemyHealth; }
            set
            {
                _enemyHealth = value;
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
        public string EnemyName
        {
            get { return _enemyName; }
            set 
            { 
                _enemyName = value;
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
                case "Fight":
                    AllowButton == false;
                    if (character.Speed <= enemy.Speed)
                    {
                        Attack();
                        EnemyAttack();
                    }
                    else
                    {
                        EnemyAttack();
                        Attack();
                    }
                    EndTurn();
                    break;
                case "Block":
                    break;
                case "Heal":
                    break;
                case "Run":
                    break;
                default:
                    break;
            }
        }

        #endregion

        public BattleViewModel(Window _view, ref Character _character, int dungeonID)
        {
            view = _view;
            AllowButton = true;
            Text = "You encountered an enemy";
            character = _character;
            Name = character.Name;
            Health = character.CurrentHealth;
            List<Enemy> list = unitofwork.EnemyRepo.GetAllWithRequirements(x => x.DungeonID == dungeonID).ToList();
            
            //get one enemy
            Random r = new Random();
            int RandomEnemy = r.Next(0, list.Count());
            enemy = list[RandomEnemy];
            enemy.Dazed = false;
            EnemyName = enemy.Name;
            EnemyHealth = enemy.Health;

        }

        public void EndBattle(bool won)
        {
            if (won == true)
            {
                view.DialogResult = true;
            }
            else
            {
                view.DialogResult = false;
            }
            view.Close();
        }

        public void EndTurn()
        {
            turn++;
            block = false;
        }

        public void CheckHealth()
        {
            if (EnemyHealth <= 0)
            {
                EndBattle(true);
            }
            else if (character.CurrentHealth <= 0)
            {
                EndBattle(false);
            }
        }

        public void EnemyAttack()
        {
            if (enemy.Dazed == true)
            {
                Text = "The enemy is dazed right now and cannot attack this turn!";
                enemy.Dazed = false;
            }
            else
            {
                if (enemy.Run(turn, EnemyHealth) == true)
                {
                    EndBattle(true);
                }

            }
        }

        public void Attack()
        {
            EnemyHealth -= character.DamageCalculated();
        }
    }
}

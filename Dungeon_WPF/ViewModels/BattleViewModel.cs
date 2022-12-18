using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon.Primitives;
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
        HelpMethods help = new HelpMethods();
        public Character character;
        public Enemy enemy;
        private string _text;
        private string _text2;
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
        public string Text2
        {
            get { return _text2; }
            set
            {
                _text2 = value;
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
                if (Health > character.Health)
                {
                    Health = character.Health;
                    character.CurrentHealth = character.Health;
                }
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
                    AllowButton = false;
                    if (character.Speed <= enemy.Speed)
                    {
                        EnemyAttack();
                        bool check = CheckHealth();
                        if (check)
                        {
                            Attack();
                            CheckHealth();
                        }
                    }
                    else
                    {
                        EnemyAttack();
                        bool check = CheckHealth();
                        if (check)
                        {
                            Attack();
                            CheckHealth();
                        }
                    }
                    EndTurn();
                    break;
                case "Block":
                    AllowButton = false;
                    if (character.Speed <= enemy.Speed)
                    {
                        EnemyAttack();
                        bool check = CheckHealth();
                        if (check)
                        {
                            Block();
                            CheckHealth();
                        }
                    }
                    else
                    {
                        Block();
                        bool check = CheckHealth();
                        if (check)
                        {
                            EnemyAttack();
                            CheckHealth();
                        }
                    }
                    EndTurn();
                    break;
                case "Heal":
                    AllowButton = false;
                    if (character.Speed <= enemy.Speed)
                    {
                        EnemyAttack();
                        bool check = CheckHealth();
                        if (check)
                        {
                            Heal();
                            CheckHealth();
                        }
                    }
                    else
                    {
                        Heal();
                        bool check = CheckHealth();
                        if (check)
                        {
                            EnemyAttack();
                            CheckHealth();
                        }
                    }
                    EndTurn();
                    break;
                case "Run":
                    AllowButton = false;
                    if (character.Speed <= enemy.Speed)
                    {
                        EnemyAttack();
                        bool check = CheckHealth();
                        if (check)
                        {
                            Run();
                            CheckHealth();
                        }
                    }
                    else
                    {
                        Run();
                        bool check = CheckHealth();
                        if (check)
                        {
                            EnemyAttack();
                            CheckHealth();
                        }
                    }
                    EndTurn();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public BattleViewModel(Window _view, ref Character _character, Enemy _enemy)
        {
            view = _view;
            AllowButton = true;
            Text = "You encountered an enemy";
            character = _character;

            Name = character.Name;
            Health = character.CurrentHealth;

            //get one enemy
            enemy = _enemy;
            enemy.Dazed = false;
            EnemyName = enemy.Name;
            EnemyHealth = enemy.Health;

        }

        public void EndBattle(bool _won, string message)
        {
            help.Message(message);
            character.Victory = _won;
            view.Close();
        }

        public void EndTurn()
        {
            turn++;
            block = false;
            AllowButton = true;
        }

        public bool CheckHealth()
        {
            if (EnemyHealth <= 0)
            {
                EndBattle(true, "You won this battle");
                return false;
            }
            else if (character.CurrentHealth <= 0)
            {
                EndBattle(false, "Oh No, you blacked out and lost all your loot!");
                return false;
            }
            return true;
        }

        public void EnemyAttack()
        {
            if (enemy.Dazed == true)
            {
                Text2 = "The enemy is dazed right now and cannot attack this turn!";
                enemy.Dazed = false;
            }
            else
            {
                if (enemy.Run(turn, EnemyHealth) == true)
                {
                    EndBattle(true, $"{EnemyName} ran away");
                }

                if (enemy.DoDamage(turn, EnemyHealth) == true)
                {
                    if (block == true)
                    {
                        enemy.Dazed = true;
                        Text2 = $"{EnemyName} was dazed by your block";
                    }
                    else
                    {
                        int damage = enemy.DealDamage(turn);
                        character.CurrentHealth -= damage;
                        Health -= damage;
                        Text2 = $"{EnemyName} dealt {damage} damage to you";
                    }
                }
                else
                {
                    if (enemy.Kind == "Small")
                    {
                        Text2 = $"{EnemyName} is unsure of what to do";
                    }
                    else if (enemy.Kind == "Medium")
                    {
                        Text2 = $"{EnemyName} is watching you carefully";
                    }
                    else
                    {
                        Text2 = $"{EnemyName} has an idea of how it is gonna take you down";
                    }
                }
            }
        }

        public void Attack()
        {
            int damage = character.DamageCalculated();
            EnemyHealth -= damage;
            Text = $"you dealt {damage} to {EnemyName}";
        }

        public void Block()
        {
            if (character.Block() == true)
            {
                block = true;

                if (character.Speed <= enemy.Speed)
                {
                    Text = "You tried to block the attack, but you were to slow!";
                }
                else
                {
                    Text = "You block the incoming attack";
                }
            }
            else
            {
                Text = "You were not able to block the next attack";
            }

        }

        public void Heal()
        {
            bool check = CheckHealth();
            if (check)
            {
                Random r = new Random();
                int heal = r.Next(5, 10);
                character.CurrentHealth += heal;
                Health += heal;
                Text = $"You just Healed {heal} health back!";
            }
        }

        public void Run()
        {
            bool check = CheckHealth();
            if (check)
            {
                Random r = new Random();
                int RunChance = r.Next(1, 10);

                if (character.Speed < enemy.Speed)
                {
                    if (RunChance <= 1)
                    {
                        EndBattle(false, $"You were able to run away, even though {EnemyName} is faster than you");
                    }
                    else
                    {
                        Text = $"You couldn't run away, {EnemyName} is faster than you";
                    }
                }
                if (character.Speed == enemy.Speed)
                {
                    if (RunChance <= 5)
                    {
                        EndBattle(false, "You were able to run away");
                    }
                    else
                    {
                        Text = "You were not able to run away, but you have a good chance of escaping";
                    }
                }
                if (character.Speed > enemy.Speed)
                {
                    if (RunChance <= 9)
                    {
                        EndBattle(false, "You were able to run away easily");
                    }
                    else
                    {
                        Text = $"You weren't able to run away, {EnemyName} did have a hard time catching up to you";
                    }
                }
            }
        }
    }
}

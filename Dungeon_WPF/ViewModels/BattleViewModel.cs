using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Ribbon.Primitives;
using System.Windows.Threading;
using Dungeon_WPF.Data;
using Dungeon_WPF.Data.UnitOfWork;
using Dungeon_WPF.DomainModels;
using Dungeon_WPF.HelperFiles;
using Dungeon_WPF.Views;
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
        public Thread charMoveThread, roundThread;
        public int turn = 1;
        public bool block = false;

        private string _text;
        private string _text2;
        private int _health;
        private int _enemyHealth;
        private string _enemyName;
        private bool _allowbutton;
        private string _charImage;
        private int _maxHealth;
        private int _enemyMaxHealth;

        public int EnemyMaxHealth
        {
            get { return _enemyMaxHealth; }
            set
            {
                _enemyMaxHealth = value;
                NotifyPropertyChanged();
            }
        }
        public int MaxHealth
        {
            get { return _maxHealth; }
            set
            {
                _maxHealth = value;
                NotifyPropertyChanged();
            }
        }
        public String CharImage
        {
            get { return _charImage; }
            set 
            {
                _charImage = value;
                NotifyPropertyChanged();
            }
        }
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
                if (_enemyHealth <= 0)
                {
                    _enemyHealth = 0;
                }
                if (_enemyHealth > enemy.Health)
                {
                    _enemyHealth = enemy.Health;
                }
                NotifyPropertyChanged();
            }
        }
        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                if (_health > character.Health)
                {
                    _health = character.Health;
                    character.CurrentHealth = character.Health;
                }
                if (_health <= 0)
                {
                    _health= 0;
                }
                character.CurrentHealth = _health;
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

                    if (AllowButton)
                    {
                        AllowButton = false;

                        roundThread = new Thread(new ThreadStart(AttackRound));
                        roundThread.IsBackground = true;
                        roundThread.Start();
                    }

                    break;
                case "Block":
                    
                    if (AllowButton)
                    {
                        AllowButton = false;

                        roundThread = new Thread(new ThreadStart(BlockRound));
                        roundThread.IsBackground = true;
                        roundThread.Start();
                    }

                    break;
                case "Heal":
                    
                    if (AllowButton)
                    {
                        AllowButton = false;

                        roundThread = new Thread(new ThreadStart(HealRound));
                        roundThread.IsBackground = true;
                        roundThread.Start();
                    }

                    break;
                case "Run":

                    if (AllowButton)
                    {
                        AllowButton = false;

                        roundThread = new Thread(new ThreadStart(RunRound));
                        roundThread.IsBackground = true;
                        roundThread.Start();
                    }

                    break;
                default:
                    charMoveThread.Interrupt();
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

            MaxHealth = character.Health;
            Health = character.CurrentHealth;

            //get one enemy
            enemy = _enemy;
            enemy.Dazed = false;
            EnemyName = enemy.Name;
            EnemyHealth = enemy.Health;
            EnemyMaxHealth = enemy.Health;
            Text2 = $"{enemy.Name} noticed you!";

            //start threads
            charMoveThread = new Thread(new ThreadStart(CharMove));
            charMoveThread.IsBackground = true;
            charMoveThread.Start();
        }

        //methods

        public void EndBattle(bool _won, string message)
        {
            charMoveThread.Interrupt();

            Application.Current.Dispatcher.Invoke(() =>
            {
                help.Message(message);
            });

            //help.Message(message);

            character.Victory = _won;

            Application.Current.Dispatcher.Invoke(() =>
            {
                view.Close();
            });

            roundThread.Interrupt();
            roundThread.Abort();
        }

        public void EndRound()
        {
            //set text to 'wait'-modus?
            turn++;
            block = false;
            AllowButton = true;
        }

        public void CheckHealth()
        {
            Thread.Sleep(2000);
            if (EnemyHealth <= 0)
            {
                EndBattle(true, "You won this battle");
            }
            else if (character.CurrentHealth <= 0)
            {
                EndBattle(false, "Oh no, you blacked out and lost all your loot!");
            }
        }

        public void EnemyAttack()
        {
            try
            {
                if (enemy.Dazed == true)
                {
                    WriteEnemyMessage("The enemy is dazed right now and cannot attack this turn!");
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

                            WriteEnemyMessage($"{EnemyName} was dazed by your block");
                        }
                        else
                        {
                            int damage = enemy.DealDamage(turn);
                            Health -= damage;

                            WriteEnemyMessage($"{EnemyName} dealt {damage} damage to you");
                        }
                    }
                    else
                    {
                        if (enemy.Kind == "Small")
                        {
                            WriteEnemyMessage($"{EnemyName} is unsure of what to do");
                        }
                        else if (enemy.Kind == "Medium")
                        {
                            WriteEnemyMessage($"{EnemyName} is watching you carefully");
                        }
                        else
                        {
                            WriteEnemyMessage($"{EnemyName} has an idea of how it is going to take you down");
                        }
                    }
                }

                CheckHealth();
            }
            catch (Exception ex)
            {
                CheckHealth();
            }
        }

        public void Attack()
        {
            try
            {
                charMoveThread.Interrupt();

                //animation attack move
                for (int i = 1; i <= 3; i++)
                {
                    Thread.Sleep(500);
                    CharImage = character.LinkImage(false, i);
                }
                Thread.Sleep(500);

                int damage = character.DamageCalculated();
                EnemyHealth -= damage;

                WriteCharMessage($"You dealt {damage} damage to {EnemyName}");

                charMoveThread = new Thread(new ThreadStart(CharMove));
                charMoveThread.IsBackground = true;
                charMoveThread.Start();

                CheckHealth();
            }
            catch (Exception ex)
            {
                CheckHealth();
            }
        }

        public void Block()
        {
            try
            {
                if (character.Speed <= enemy.Speed)
                {
                    WriteCharMessage("You tried to block the attack, but you were to slow!");
                }
                else
                {
                    if (character.Block() == true)
                    {
                        block = true;

                        WriteCharMessage("You prepare to block the incoming attack");
                    }
                    else
                    {
                        WriteCharMessage("You failed to block the next attack");
                    }
                }

                CheckHealth();
            }
            catch (Exception ex)
            {
                CheckHealth();
            }
        }

        public void Heal()
        {
            try
            {
                Random r = new Random();
                int heal = r.Next(5, 10);
                Health += heal;

                WriteCharMessage($"You just Healed {heal} health back!");

                CheckHealth();
            }
            catch (Exception ex)
            {
                CheckHealth();
            }
        }

        public void Run()
        {
            try
            {
                Random r = new Random();
                int RunChance = r.Next(1, 10);

                if (character.Speed < enemy.Speed)
                {
                    if (RunChance <= 1)
                    {
                        EndBattle(false, $"You were barely able to escape, even though {EnemyName} is faster than you");
                    }
                    else
                    {
                        WriteCharMessage($"You couldn't run away this time, {EnemyName} is faster than you");
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
                        WriteCharMessage("You were not able to run away, but you have a good chance of escaping");
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
                        WriteCharMessage($"You weren't able to run away, {EnemyName} did have a hard time catching up to you");
                    }
                }

                CheckHealth();
            }
            catch (Exception ex)
            {
                CheckHealth();
            }
        }

        //methods called in threads

        public void CharMove()
        {
            try
            {
                while (true)
                {
                    for (int i = 1; i <= 3; i++)
                    {
                        CharImage = character.LinkImage(true, i);
                        Thread.Sleep(100);
                    }
                    CharImage = character.LinkImage(true, 2);
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Character attacked/stopped moving");
            }
        }

        public void AttackRound()
        {
            try
            {
                if (character.Speed <= enemy.Speed)
                {
                    EnemyAttack();
                    Attack();
                }
                else
                {
                    Attack();
                    EnemyAttack();
                }
                EndRound();
            }
            catch (Exception)
            {

            }
        }

        public void BlockRound()
        {
            try
            {
                if (character.Speed < enemy.Speed)
                {
                    EnemyAttack();
                    Block();
                }
                else
                {
                    Block();
                    EnemyAttack();
                }
                EndRound();
            }
            catch (Exception)
            {

            }
        }

        public void HealRound()
        {
            try
            {
                if (character.Speed <= enemy.Speed)
                {
                    EnemyAttack();
                    Heal();
                }
                else
                {
                    Heal();
                    EnemyAttack();
                }
                EndRound();
            }
            catch (Exception)
            {

            }
        }

        public void RunRound()
        {
            try
            {
                if (character.Speed <= enemy.Speed)
                {
                    EnemyAttack();
                    Run();
                }
                else
                {
                    Run();
                    EnemyAttack();
                }
                EndRound();
            }
            catch (Exception)
            {

            }
        }

        public void WriteCharMessage(string text)
        {
            Text = "";
            for (int i = 0; i < text.Length; i++)
            {
                Text += text[i];
                Thread.Sleep(20);
            }
        }

        public void WriteEnemyMessage(string text)
        {
            Text2 = "";
            for (int i = 0; i < text.Length; i++)
            {
                Text2 += text[i];
                Thread.Sleep(20);
            }
        }
    }
}

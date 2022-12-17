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
    public class DungeonViewModel: BasisViewModel
    {
        public Window view;
        public HelpMethods help = new HelpMethods();
        IUnitOfWork unitofwork = new UnitOfWork(new DungeonEntities());
        public Dungeon dungeon;
        public Character character;
        private string _text = "";
        private int _counter;
        private int _loot;

        public int Loot
        {
            get { return _loot; }
            set 
            { 
                _loot = value;
                NotifyPropertyChanged();
            }
        }
        public int Counter
        {
            get { return _counter; }
            set
            {
                _counter = value;
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
                case "Step":
                    TakeStep();
                    break;
                case "Run":
                    Run();
                    break;
                default:
                    break;
            }
        }

        #endregion

        public DungeonViewModel(Window _view, Dungeon _dungeon, Character _character)
        {
            view = _view;
            Counter = 1;
            Text = "Take a step";
            dungeon = _dungeon;
            this.character = _character;
            character.CurrentHealth = character.Health;
        }

        public void Run()
        {
            bool answer =  help.AskQuestion("Are you sure you want to run? You will not take your loot with you...", "Yes!", "No!");
            if (answer == true)
            {
                EndDungeon();
            }
        }

        public void TakeStep()
        {
            Random r = new Random();
            int Chance = r.Next(0, dungeon.EnemyChance + dungeon.ShortCutChance + dungeon.LootChance + dungeon.NothingChance);

            if (Chance <= dungeon.EnemyChance)
            {
                //get attacked by enemy
                Text = "You just encountered an enemy!";

                BattleView _view = new BattleView();
                BattleViewModel vm = new BattleViewModel(_view, ref character, dungeon.Id);
                _view.DataContext = vm;
                _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                _view.ShowDialog();

                if (_view.DialogResult == true)
                {
                    Counter++;
                    Text = "You won this fight!";
                }

                if (character.CurrentHealth <= 0)
                {
                    help.Message("Oh No, you blacked out and lost all your loot!");
                    EndDungeon();
                }
            }
            else if (Chance <= dungeon.EnemyChance + dungeon.LootChance)
            {
                //get loot (gold)
                int minGold = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(character.Attack - (character.Attack / 2))));
                int maxGold = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(character.Attack + (character.Attack / 2))));
                int loot = r.Next(minGold, maxGold);
                Loot += loot;
                Counter++;
                Text = $"You found {loot} gold on the ground";
            }
            else if (Chance <= dungeon.EnemyChance + dungeon.LootChance + dungeon.ShortCutChance)
            {
                //take multiple steps (= a shortcut)
                Counter += 5;
                Text = "You found a shortcut and took multiple steps at once";
            }
            else
            {
                Counter++;
                Text = "Nothing happened!";
            }

            if (Counter >= dungeon.MaxSteps)
            {
                help.Message("Well done, you finished this dungeon alive!");
                character.Money += Loot;
                // adding the loot to the character
                unitofwork.CharacterRepo.Update(character);
                EndDungeon();
            }
        }

        public void EndDungeon()
        {
            DungeonSelectionView _view = new DungeonSelectionView();
            DungeonSelectionViewModel vm = new DungeonSelectionViewModel(_view, character);
            _view.DataContext = vm;
            _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _view.Show();
            view.Close();
        }
    }
}

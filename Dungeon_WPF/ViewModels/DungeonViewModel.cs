using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
        public Thread stepThread;
        public bool skipText;

        private string _text = "";
        private int _counter = 1;
        private int _loot;
        private bool _buttonAllowed;
        private string _dungeonName;
        private string _charImage;
        private int _maxSteps;

        public int MaxSteps
        {
            get { return _maxSteps; }
            set
            {
                _maxSteps = value;
                NotifyPropertyChanged();
            }
        }
        public string CharImage
        {
            get { return _charImage; }
            set 
            { 
                _charImage = value;
                NotifyPropertyChanged();
            }
        }
        public string DungeonName
        {
            get { return _dungeonName; }
            set 
            {
                _dungeonName = value;
                NotifyPropertyChanged();
            }
        }
        public bool ButtonAllowed
        {
            get { return _buttonAllowed; }
            set
            {
                _buttonAllowed = value;
                NotifyPropertyChanged();
            }
        }
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
                if (_counter <= 1)
                {
                    _counter = 1;
                }
                if (_counter >= dungeon.MaxSteps)
                {
                    _counter = dungeon.MaxSteps;
                }
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

                    if (ButtonAllowed)
                    {
                        ButtonAllowed = false;

                        stepThread = new Thread(new ThreadStart(TakeStep));
                        stepThread.IsBackground = true;
                        stepThread.Start();
                    }else if (!skipText)
                    {
                        skipText = true;
                    }

                    break;
                case "Run":
                    if (ButtonAllowed)
                    {
                        Run();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        public DungeonViewModel(Window _view, Dungeon _dungeon, Character _character)
        {
            view = _view;
            Text = "Take a step";

            dungeon = _dungeon;
            DungeonName = dungeon.Name;
            MaxSteps = dungeon.MaxSteps;

            this.character = _character;
            character.CurrentHealth = character.Health;
            CharImage = character.ImageLink;

            ButtonAllowed = true;
            skipText = false;
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
            try
            {
                Random r = new Random();
                int Chance = r.Next(0, dungeon.EnemyChance + dungeon.ShortCutChance + dungeon.LootChance + dungeon.NothingChance);

                //little animation
                Thread.Sleep(200);
                CharImage = character.LinkImage("rest", 2);
                Thread.Sleep(200);
                CharImage = character.LinkImage("rest", 1);
                Thread.Sleep(200);

                if (Chance <= dungeon.EnemyChance)
                {
                    //get attacked by enemy
                    WriteText("You just encountered an enemy!");

                    //enemy is created here for gold calculation
                    List<Enemy> list = unitofwork.EnemyRepo.GetAllWithRequirements(x => x.DungeonID == dungeon.Id).ToList();

                    int RandomEnemy = r.Next(0, list.Count());
                    Enemy enemy = list[RandomEnemy];

                    //animation before finding the enemy
                    Thread.Sleep(2000);

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        BattleView _view = new BattleView();
                        BattleViewModel vm = new BattleViewModel(_view, ref character, enemy);
                        _view.DataContext = vm;
                        _view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                        _view.ShowDialog();
                    });

                    // the rogue part is just a fun detail, the idea is that a rogue can sneak past an enemy and keep its progression
                    if (character.Victory == true || character.ClassName == "Rogue")
                    {
                        Counter++;
                    }
                    if (character.Victory == true)
                    {
                        int difference = Convert.ToInt32((dungeon.LootChance / (dungeon.LootChance + dungeon.EnemyChance + dungeon.ShortCutChance + dungeon.NothingChance)) * enemy.Attack);
                        int minGold = enemy.Attack - difference;
                        int maxGold = enemy.Attack + difference;
                        int enemyloot = r.Next(minGold, maxGold);
                        Loot += enemyloot;
                    }

                    if (character.CurrentHealth <= 0)
                    {
                        Thread.Sleep(2000);

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            EndDungeon();
                        });
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

                    WriteText($"You found {loot} gold on the ground");
                }
                else if (Chance <= dungeon.EnemyChance + dungeon.LootChance + dungeon.ShortCutChance)
                {
                    //take multiple steps (= a shortcut)
                    Counter += 5;
                    WriteText("You found a shortcut and took multiple steps at once");
                }
                else
                {
                    Counter++;
                    if (Counter % 2 == 0)
                    {
                        WriteText("Nothing happened!");
                    }
                    else
                    {
                        WriteText("This place is really peaceful, sometimes...");
                    }
                }

                if (Counter >= dungeon.MaxSteps)
                {
                    Thread.Sleep(2000);

                    character.Money += Loot;
                    
                    // adding the loot to the character
                    unitofwork.CharacterRepo.Update(character);
                    int save = unitofwork.Save();
                    if (save > 0)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            help.Message("Well done, you finished this dungeon alive!\nYou took all the loot with you");
                            EndDungeon();
                        });
                    }
                }

                ButtonAllowed = true;
            }
            catch (Exception ex)
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    help.Message($"something went wrong: {ex.Message}");
                });

                ButtonAllowed = true;
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

        public void WriteText(string text)
        {
            Text = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (!skipText)
                {
                    Text += text[i];
                    Thread.Sleep(20);
                }
                else
                {
                    Text = text;
                    skipText = false;
                    break;
                }
            }
        }
    }
}

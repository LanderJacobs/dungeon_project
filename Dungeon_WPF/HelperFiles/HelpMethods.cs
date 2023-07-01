using Dungeon_WPF.Data;
using Dungeon_WPF.Data.UnitOfWork;
using Dungeon_WPF.DomainModels;
using Dungeon_WPF.ViewModels;
using Dungeon_WPF.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dungeon_WPF.HelperFiles
{
    public class HelpMethods
    {
        IUnitOfWork unitofwork = new UnitOfWork(new DungeonEntities());

        public void Message(string message)
        {
            PopupView view = new PopupView();
            PopupViewModel vm = new PopupViewModel(view,message);
            view.DataContext = vm;
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            view.ShowDialog();
        }    

        public bool AskQuestion(string question, string yes, string no)
        {
            ChoiceView view = new ChoiceView();
            ChoiceViewModel vm = new ChoiceViewModel(view, question, yes, no);
            view.DataContext = vm;
            view.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            view.ShowDialog();

            return view.DialogResult == true? true: false;
        }

        public void InsertUponCreate()
        {
            List<Dungeon> test = unitofwork.DungeonRepo.GetAll().ToList();

            if (test.Count < 1)
            {
                unitofwork.CharacterRepo.Add(new Character { Name = "Harry", ClassName = "Rogue", Money = 0, Attack = 6, Health = 12, Speed = 15 });
                unitofwork.CharacterRepo.Add(new Character { Name = "Mary", ClassName = "Knight", Money = 50, Attack = 10, Health = 15, Speed = 5 });
                unitofwork.CharacterRepo.Add(new Character { Name = "Barry", ClassName = "Fighter", Money = 1000, Attack = 7, Health = 20, Speed = 7 });
                unitofwork.DungeonRepo.Add(new Dungeon { Name = "Graveyard", MaxSteps = 50, EnemyChance = 10, LootChance = 5, ShortCutChance = 2, NothingChance = 30 });
                unitofwork.DungeonRepo.Add(new Dungeon { Name = "Backyard", MaxSteps = 10, EnemyChance = 2, LootChance = 1, ShortCutChance = 0, NothingChance = 20 });
                unitofwork.DungeonRepo.Add(new Dungeon { Name = "GoldCave", MaxSteps = 30, EnemyChance = 1, LootChance = 313, ShortCutChance = 50, NothingChance = 1 });
                unitofwork.EnemyRepo.Add(new Enemy { Name = "Bat", Kind = "Small", Health = 10, Attack = 2, Speed = 16, AttackChance = 2, RestChance = 14, RunChance = 10, Attack2 = 2, Attack2Chance = 2, DungeonID = 1 });
                unitofwork.EnemyRepo.Add(new Enemy { Name = "Zombie", Kind = "Medium", Health = 16, Attack = 8, Speed = 4, AttackChance = 10, RestChance = 10, RunChance = 10, Attack2 = 8, Attack2Chance = 10, DungeonID = 1 });
                unitofwork.EnemyRepo.Add(new Enemy { Name = "Big Zombie", Kind = "Big", Health = 20, Attack = 14, Speed = 6, AttackChance = 14, RestChance = 4, RunChance = 0, Attack2 = 18, Attack2Chance = 18, DungeonID = 1 });
                unitofwork.EnemyRepo.Add(new Enemy { Name = "Snail", Kind = "Small", Health = 6, Attack = 1, Speed = 1, AttackChance = 1, RestChance = 10, RunChance = 1, Attack2 = 0, Attack2Chance = 0, DungeonID = 2 });
                unitofwork.EnemyRepo.Add(new Enemy { Name = "Butterfly", Kind = "Small", Health = 10, Attack = 2, Speed = 20, AttackChance = 2, RestChance = 8, RunChance = 10, Attack2 = 0, Attack2Chance = 0, DungeonID = 2 });
                unitofwork.EnemyRepo.Add(new Enemy { Name = "GOD", Kind = "Big", Health = 999, Attack = 999, Speed = 999, AttackChance = 999, RestChance = 0, RunChance = 0, Attack2 = 9999, Attack2Chance = 9999, DungeonID = 3 });

                int save = unitofwork.Save();
            }
        }

    }
}

using Dungeon_WPF.Data.Repository;
using Dungeon_WPF.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.Data.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        public IRepository<Character> _CharacterRepo { get; set; }
        public IRepository<Enemy> _EnemyRepo { get; set; }
        public IRepository<Dungeon> _DungeonRepo { get; set; }

        public DungeonEntities DungeonEntities { get; }
        public UnitOfWork(DungeonEntities dungeonEntities) 
        {
            this.DungeonEntities = dungeonEntities;
        }

        public IRepository<Character> CharacterRepo
        {
            get
            {
                if (_CharacterRepo == null)
                {
                    _CharacterRepo = new Repository<Character>(DungeonEntities);
                }
                return _CharacterRepo;
            }
        }

        public IRepository<Enemy> EnemyRepo
        {
            get
            {
                if (_EnemyRepo == null)
                {
                    _EnemyRepo = new Repository<Enemy>(DungeonEntities);
                }
                return _EnemyRepo;
            }
        }

        public IRepository<Dungeon> DungeonRepo
        {
            get
            {
                if (_DungeonRepo == null)
                {
                    _DungeonRepo = new Repository<Dungeon>(DungeonEntities);
                }
                return _DungeonRepo;
            }
        }

        public void Dispose()
        {
            DungeonEntities.Dispose();
        }

        public int Save()
        {
            return DungeonEntities.SaveChanges();
        }
    }
}

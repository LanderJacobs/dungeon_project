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
        public IRepository<Character> _CharacterRepository { get; set; }
        public IRepository<Enemy> _EnemyRepository { get; set; }
        public IRepository<Dungeon> _DungeonRepository { get; set; }

        public DungeonEntities DungeonEntities { get; }
        public UnitOfWork(DungeonEntities dungeonEntities) 
        {
            this.DungeonEntities = dungeonEntities;
        }

        public IRepository<Character> CharacterRepository
        {
            get
            {
                if (_CharacterRepository == null)
                {
                    _CharacterRepository = new Repository<Character>(DungeonEntities);
                }
                return _CharacterRepository;
            }
        }

        public IRepository<Enemy> EnemyRepository
        {
            get
            {
                if (_EnemyRepository == null)
                {
                    _EnemyRepository = new Repository<Enemy>(DungeonEntities);
                }
                return _EnemyRepository;
            }
        }

        public IRepository<Dungeon> DungeonRepository
        {
            get
            {
                if (_DungeonRepository == null)
                {
                    _DungeonRepository = new Repository<Dungeon>(DungeonEntities);
                }
                return _DungeonRepository;
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

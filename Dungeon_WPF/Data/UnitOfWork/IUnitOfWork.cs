using Dungeon_WPF.Data.Repository;
using Dungeon_WPF.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.Data.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IRepository<Character> CharacterRepo { get; }
        IRepository<Enemy> EnemyRepo { get; }
        IRepository<Dungeon> DungeonRepo { get; }

        int Save();
    }
}

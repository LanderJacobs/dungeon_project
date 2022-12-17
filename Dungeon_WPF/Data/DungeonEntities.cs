using Dungeon_WPF.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.Data
{
    public class DungeonEntities:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = DataFile.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .HasDiscriminator<string>("class")
                .HasValue<Knight>("knight")
                .HasValue<Fighter>("fighter")
                .HasValue<Rogue>("rogue");

            modelBuilder.Entity<Enemy>()
                .HasDiscriminator<string>("kind")
                .HasValue<SmallEnemy>("small")
                .HasValue<MediumEnemy>("medium")
                .HasValue<BigEnemy>("big");
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Dungeon> Dungeons { get; set; }
    }
}

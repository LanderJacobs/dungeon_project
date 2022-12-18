using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class Dungeon
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int MaxSteps { get; set; }
        [Required]
        public int EnemyChance { get; set; }
        [Required]
        public int LootChance { get; set; }
        [Required]
        public int ShortCutChance { get; set; }
        [Required]
        public int NothingChance { get; set; }

        public ICollection<Enemy> Enemies { get; set; }
    }
}

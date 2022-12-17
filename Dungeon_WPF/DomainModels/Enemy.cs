using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class Enemy
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Health { get; set; }
        [Required]
        public int Attack { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public int AttackChance { get; set; }
        [Required]
        public int RestChance { get; set; }
        [NotMapped]
        public bool Dazed { get; set; }
        [Required]
        public int DungeonID { get; set;}

        [ForeignKey("DungeonID")]
        public Dungeon Dungeon { get; set; }
    }
}

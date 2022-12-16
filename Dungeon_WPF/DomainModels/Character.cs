using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Money { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int Health { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class Character
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Money { get; set; }
        [Required]
        public int Attack { get; set; }
        [Required]
        public int Speed { get; set; }
        [Required]
        public int Health { get; set; }
        [NotMapped]
        public int CurrentHealth { get; set; }

        public virtual int DamageCalculated()
        {
            return 5;
        }
    }
}

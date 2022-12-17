using System;
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
        public string ClassName { get; set; }
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

        //methods
        //you'll see the use of "this.ClassName", this used to be done with child-classes but was deleted because of too many errors
        public virtual int DamageCalculated()
        {
            double damage = 5;
            if (this.ClassName == "Knight")
            {
                damage = Math.Ceiling(0.5 * this.Attack);
                return Convert.ToInt32(damage);
            }
            else if (this.ClassName == "Fighter")
            {
                damage = Math.Ceiling(0.5 * this.Health);
                return Convert.ToInt32(damage);
            }
            else if (this.ClassName == "Rogue")
            {
                damage = Math.Ceiling(0.5 * this.Speed);
                return Convert.ToInt32(damage);
            }
            return Convert.ToInt32(damage);
        }
    }
}

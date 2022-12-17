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
        public string Kind { get; set; }
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
        [Required]
        public int RunChance { get; set; }
        [Required]
        public int Attack2 { get; set; }
        [Required]
        public int Attack2Chance { get; set; }
        [NotMapped]
        public bool Dazed { get; set; }
        [Required]
        public int DungeonID { get; set;}

        [ForeignKey("DungeonID")]
        public Dungeon Dungeon { get; set; }

        //methods
        //you'll see the use of "this.Kind", this used to be done with child-classes but was deleted because of too many errors
        public bool DoDamage(int turn, int currentHealth)
        {
            Random r = new Random();

            if (this.Kind == "Small")
            {
                if (currentHealth < this.Health)
                {
                    return false;
                }

                int SmallChance = r.Next(0, this.AttackChance + this.RestChance + this.RunChance);

                return SmallChance <= this.AttackChance ? true : false;
            }
            else if (this.Kind == "Medium")
            {
                int MediumChance = r.Next(0, this.AttackChance + this.RestChance);
                return MediumChance <= this.AttackChance ? true : false;
            }
            else if (this.Kind == "Big")
            {
                int BigChance = r.Next(0, this.AttackChance + this.RestChance);
                return BigChance <= this.AttackChance ? true : false;
            }

            int Chance = r.Next(0, this.AttackChance + this.RestChance);
            return Chance <= this.AttackChance ? true : false;
        }

        public bool Run(int turn, int currentHealth)
        {
            Random r = new Random();
            if (this.Kind == "Small")
            {
                int Chance = r.Next(0, this.RestChance + this.RunChance);

                return Chance <= this.RunChance ? true : false;
            }
            else if (this.Kind == "Medium")
            {
                if (currentHealth < this.Health && turn < 5)
                {
                    return true;
                }

                return false;
            }
            else if (this.Kind == "Big")
            {
                return false;
            }
            return true;
        }

        public int DealDamage(int turn)
        {
            if (this.Kind == "Small")
            {
                return Convert.ToInt32(Math.Floor(this.Attack * 0.5));
            }
            else if (this.Kind == "Medium")
            {
                return this.Attack;
            }
            else if (this.Kind == "Big")
            {
                Random r = new Random();
                int Chance = r.Next(0, this.AttackChance + this.Attack2Chance);
                if (turn > 5 && Chance < this.Attack2Chance)
                {
                    return Attack2;
                }
                return Attack;
            }

            return 5;
        }
    }
}

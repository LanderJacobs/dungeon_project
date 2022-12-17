using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class SmallEnemy: Enemy
    {
        [Required]
        public int RunChance { get; set; }

        public override bool Run(int turn)
        {
            Random r = new Random();
            int Chance = r.Next(0, this.RestChance + this.RunChance);

            return Chance <= this.RunChance ? true : false;
        }

        public override bool DoDamage(int turn)
        {
            if (this.CurrentHealth < this.Health)
            {
                return false;
            }

            Random r = new Random();
            int Chance = r.Next(0, this.AttackChance + this.RestChance + this.RunChance);

            return Chance <= this.AttackChance ? true : false;
        }

        public override int DealDamage(int turn)
        {
            return base.DealDamage(turn);
        }
    }
}

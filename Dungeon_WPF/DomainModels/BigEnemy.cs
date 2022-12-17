using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class BigEnemy: Enemy
    {
        [Required]
        public int Attack2 { get; set; }
        [Required]
        public int Attack2Chance { get; set; }

        public override bool Run(int turn)
        {
            return false;
        }

        public override bool DoDamage(int turn)
        {
            return base.DoDamage(turn);
        }

        public override int DealDamage(int turn)
        {
            Random r = new Random();
            int Chance = r.Next(0, this.AttackChance + this.Attack2Chance);
            if (turn > 5 && Chance < this.Attack2Chance)
            {
                return Attack2;
            }
            return Attack;
        }
    }
}

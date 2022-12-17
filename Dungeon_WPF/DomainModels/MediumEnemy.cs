using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class MediumEnemy: Enemy
    {
        public override bool Run(int turn)
        {
            if (this.CurrentHealth < this.Health && turn < 5)
            {
                return true;
            }

            return false;
        }

        public override bool DoDamage(int turn)
        {
            return base.DoDamage(turn);
        }


        public override int DealDamage(int turn)
        {
            return base.DealDamage(turn);
        }
    }
}

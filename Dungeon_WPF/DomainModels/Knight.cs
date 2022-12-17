﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_WPF.DomainModels
{
    public class Knight: Character
    {
        public override int DamageCalculated()
        {
            double damage = Math.Ceiling(0.5 * this.Attack);
            return Convert.ToInt32(damage);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
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
        [NotMapped]
        public bool Victory { get; set; }
        [NotMapped]
        public String ImageLink {
            get
            {
                // if I ever find a way that can correctly check the existence of a file, not File.exists() apparently,
                // I'll use path as a variable to check the existence;
                String path = @"..\HelperFiles\images\character\" + this.ClassName.ToLower() + @"\rest_1.png";
                return path;
            }
        }

        //methods
        //you'll see the use of "this.ClassName", this used to be done with child-classes but was deleted because of too many errors
        public int DamageCalculated()
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

        public bool Block()
        {
            Random r = new Random();
            int Chance = r.Next(0, this.Attack + this.Health + this.Speed);
            if (this.ClassName == "Knight")
            {
                if (Chance <= Math.Ceiling(Convert.ToDouble(this.Attack + (this.Health / 2))))
                {
                    return true;
                }
                return false;
            }
            else if (this.ClassName == "Fighter")
            {
                if (Chance <= Math.Ceiling(Convert.ToDouble(this.Health + (this.Health / 2))))
                {
                    return true;
                }
                return false;
            }
            else if (this.ClassName == "Rogue")
            {
                if (Chance <= Math.Ceiling(Convert.ToDouble(this.Speed + (this.Health / 2))))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public String LinkImage(bool rest, int stage)
        {
            String form = rest ? "rest_" : "attack_";
            form += stage.ToString() + ".png";
            return @"..\HelperFiles\images\character\" + this.ClassName.ToLower() + @"\" + form;
        }
    }
}

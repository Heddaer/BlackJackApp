using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace UtilityLib.Game
{
    public class PlayerStats
    {
        [Key]
        public int PlayerStatsId { get; set; }
        public int Losses { get; set; }
        public int Wins { get; set; }
        public int PlayerID { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

       // public double Coins {  get; set; } 

        public PlayerStats() { }

        public override string ToString()
        {
            return string.Format(Losses + " Losses | " + Wins + " Wins ");
        }
    }
}

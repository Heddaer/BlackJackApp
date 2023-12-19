using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityLib.Data_Management;

namespace UtilityLib.Game
{
    public class BettingData
    {
        [Key]
        public int BettingDataID { get; set; }
        public double Bet { get; set; }
        public DateTime DateTime { get; set; }
        public int PlayerID { get; set; }

        [ForeignKey("PlayerID")]
        public Player Player { get; set; }

        public BettingData(double bet) 
        {
            Bet = bet;
            DateTime = DateTime.Now;
        }

        public override string ToString()
        {
            return string.Format("${0} | [{1}]", Bet, DateTime);
        }

    }
}

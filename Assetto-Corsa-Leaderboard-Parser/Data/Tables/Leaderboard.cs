using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assetto_Corsa_Leaderboard_Parser.Data.Tables
{
    public class Leaderboard
    {
        public int UID { get; set; }
        public string? Name { get; set; }
        public DateTime? Date { get; set; }
        public string? Car { get; set; }
        public TimeSpan? Duration { get; set; }
        public decimal? ScorePerMinute { get; set; }
        public decimal? Score { get; set; }
    }
}

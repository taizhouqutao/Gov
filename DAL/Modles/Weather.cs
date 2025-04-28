using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
    public class Weather : Basic
    {
        [Column("Id")]
        public int Id { get; set; }

        [Column("WeatherDate")]
        public DateTime WeatherDate { get; set; }

        [Column("TextDay")]
        public string? TextDay { get; set; }

        [Column("TextNight")]
        public string? TextNight { get; set; }

        [Column("High")]
        public Decimal? High { get; set; }

        [Column("Low")]
        public Decimal? Low { get; set; }

        [Column("WcDay")]
        public string? WcDay { get; set; }

        [Column("WdDay")]
        public string? WdDay { get; set; }
        
        [Column("WcNight")]
        public string? WcNight { get; set; }

        [Column("WdNight")]
        public string? WdNight { get; set; }

        [Column("Week")]
        public string? Week { get; set; }
    }
}
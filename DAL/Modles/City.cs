using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class City : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("CityName")]
    public required string CityName { get; set; }

    [Column("CitySort")]
    public required int CitySort { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class Duty : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("StartDate")]
    public required DateTime StartDate { get; set; }

    [Column("EndDate")]
    public required DateTime EndDate { get; set; }
    
    [Column("AllDay")]
    public required bool AllDay{ get; set; }

    [Column("ContactId")]
    public required int ContactId{ get; set; }

    [Column("CityId")]
    public int? CityId { get; set; } = null;
  }
}
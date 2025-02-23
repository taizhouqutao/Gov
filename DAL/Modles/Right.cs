using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class Right : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("RightName")]
    public required string RightName { get; set; }

    [Column("RightCode")]
    public required string RightCode { get; set; }

    [Column("RightType")]
    public required string RightType { get; set; }

    [Column("FatherId")]
    public required int FatherId { get; set; }
  }
}
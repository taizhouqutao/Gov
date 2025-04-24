using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class BizLog : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("ModelTitle")]
    public required string ModelTitle{ get; set; }

    [Column("ActionType")]
    public required string ActionType{ get; set; }

    [Column("ActionDesc")]
    public string? ActionDesc{ get; set; }

    [Column("OptUserName")]
    public required string OptUserName { get; set; }

    [Column("ActionRemark")]
    public string? ActionRemark { get; set; }

    [Column("ActionJson")]
    public string? ActionJson { get; set; }
  }
}
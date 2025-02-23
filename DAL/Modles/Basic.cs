using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class Basic
  {
    [Column("IfDel")]
    public required int IfDel { get; set; }

    [Column("CreateTime")]
    public required DateTime CreateTime { get; set; }

    [Column("CreateUserId")]
    public required int CreateUserId { get; set; }

    [Column("UpdateTime")]
    public DateTime? UpdateTime { get; set; }

    [Column("UpdateUserId")]
    public int? UpdateUserId { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class New : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("NewTitle")]
    public required string NewTitle { get; set; }

    [Column("NewContent")]
    public required string NewContent { get; set; }

    [Column("IsPublic")]
    public required int IsPublic { get; set; }

    [Column("PublicTime")]
    public DateTime? PublicTime { get; set; }=null;

    [Column("PublicUserId")]
    public int? PublicUserId { get; set; }=null;

    [Column("NewTypeId")]
    public int? NewTypeId{get;set;}=null;
  }
}
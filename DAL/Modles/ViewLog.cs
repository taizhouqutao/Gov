
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class ViewLog : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("NewId")]
    public required int NewId { get; set; }

    [Column("NewTypeId")]
    public required int NewTypeId { get; set; }

    /// <summary>
    /// 日志类型 1查看 2留言
    /// </summary>
    [Column("ViewType")]
    public required int ViewType { get; set; }

    [Column("ViewData")]
    public required DateTime ViewData { get; set; }

    [Column("ViewCount")]
    public required int ViewCount { get; set; }
  }
}
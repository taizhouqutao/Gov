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

    /// <summary>
    /// 权限类型 0页面 1地址 3按钮
    /// </summary>
    [Column("RightType")]
    public required string RightType { get; set; }

    [Column("FatherId")]
    public required int FatherId { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class Comment : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("NewId")]
    public int NewId { get; set; }

    [Column("PersonName")]
    public required string PersonName  { get; set; }

    [Column("PersonCellphone")]
    public string? PersonCellphone  { get; set; }

    [Column("Content")]
    public required string Content{get;set;}

    /// <summary>
    /// 留言角色类型（0前台用户 1后台用户）
    /// </summary>
    [Column("RoleType")]
    public required int RoleType{get;set;}

    [Column("UserId")]
    public int? UserId{get;set;}

    [Column("FatherComment")]
    public required int FatherCommentId{get;set;}

    [Column("CityId")]
    public int? CityId { get; set; } = null;

    [Column("IsShow")]
    public required int IsShow{get;set;}

    [Column("IfDeal")]
    public required int IfDeal{get;set;}
  }
}
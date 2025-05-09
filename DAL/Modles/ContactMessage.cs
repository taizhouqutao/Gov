using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class ContactMessage:Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("PersonName")]
    public required string PersonName{ get; set; }

    [Column("PersonCellphone")]
    public string? PersonCellphone  { get; set; }

    [Column("Content")]
    public required string Content{get;set;}

    [Column("ContactId")]
    public required int ContactId{get;set;}

    [Column("IfDeal")]
    public required int IfDeal{get;set;}

    [Column("FatherContactMessageId")]
    public required int FatherContactMessageId{get;set;}

    /// <summary>
    /// 留言角色类型（0前台用户 1后台用户）
    /// </summary>
    [Column("RoleType")]
    public required int RoleType{get;set;}

    [Column("UserId")]
    public int? UserId{get;set;}
  }
}
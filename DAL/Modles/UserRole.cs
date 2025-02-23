using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class UserRole : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("UserId")]
    public required int UserId { get; set; }

    [Column("RoleId")]
    public required int RoleId { get; set; }
  }
}
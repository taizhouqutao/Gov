using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class RightRole : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("RightId")]
    public required int RightId { get; set; }

    [Column("RoleId")]
    public required int RoleId { get; set; }
  }
}
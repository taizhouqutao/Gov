using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class Role : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("RoleName")]
    public required string RoleName { get; set; }
  }
}
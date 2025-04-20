using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class NewType: Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("NewTypeName")]
    public required string NewTypeName { get; set; }
  }
}
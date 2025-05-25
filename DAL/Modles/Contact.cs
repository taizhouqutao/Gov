using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class Contact : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("PersonName")]
    public required string PersonName { get; set; }

    [Column("Post")]
    public required string Post { get; set; }

    [Column("Cellphone")]
    public string? Cellphone { get; set; }

    [Column("Depent")]
    public string? Depent { get; set; }

    [Column("PersonHead")]
    public string? PersonHead { get; set; }

    [Column("Desc")]
    public string? Desc { get; set; }
  }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Modles
{
  public class Carousel : Basic
  {
    [Column("Id")]
    public int Id { get; set; }

    [Column("LinkUrl")]
    public string? LinkUrl { get; set; }

    [Column("ImageUrl")]
    public required string ImageUrl { get; set; }

    [Column("Title")]
    public required string Title { get; set; }

    [Column("IsPublic")]
    public required int IsPublic { get; set; }

    [Column("PublicTime")]
    public DateTime? PublicTime { get; set; }=null;

    [Column("PublicUserId")]
    public int? PublicUserId { get; set; }=null;

    [Column("CityId")]
    public int? CityId { get; set; } = null;
  }
}
namespace Common
{
  [Serializable]
  public class CarouselReqDto
  {
    public int? isPublic { get; set; }

    public List<int>? ids { get; set; }
    public int? id { get; set; }

    public string? linkUrl { get; set; }

    public string? imageUrl { get; set; }

    public string? title { get; set; }

    public List<int>? cityIds { get; set; }

    public int? cityId { get; set; }
  }

  [Serializable]
  public class CarouselPage
  {
    public List<CityResDto>? Citys { get; set; }
  }

  [Serializable]
  public class CarouselPlusDto : BasicDto
  {
    public int Id { get; set; }

    public string? LinkUrl { get; set; }

    public required string ImageUrl { get; set; }
    public required string Title { get; set; }

    public required int IsPublic { get; set; }

    public DateTime? PublicTime { get; set; } = null;

    public int? PublicUserId { get; set; } = null;

    public int? CityId { get; set; } = null;

    public string? CityName { get; set; } = null;

    public required int IfDel { get; set; }
  }
}
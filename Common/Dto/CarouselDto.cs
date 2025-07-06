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

    public int? cityId{ get; set; }
  }

  [Serializable]
  public class CarouselPage
  {
    public List<CityResDto>? Citys { get; set; }
  }
}
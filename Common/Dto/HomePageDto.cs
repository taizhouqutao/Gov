namespace Common
{
  [Serializable]
  public class HomePageDto
  {
    public required string NewTypeName { get; set; }
    public List<HomeNewItemDto>? News { get; set; }
    public List<CarouselDto>? Carousels { get; set; }
  }

  [Serializable]
  public class HomeNewItemDto
  {
    public required int Id { get; set; }

    public required string NewTitle { get; set; }

    public required string NewContent { get; set; }

    public DateTime? PublicTime { get; set; } = null;
  }

  [Serializable]
  public class CarouselDto
  {
    public string? LinkUrl { get; set; }

    public required string ImageUrl { get; set; }

    public required string Title { get; set; }
  }

  [Serializable]
  public class SetCityPageDto
  {
    public required List<CityResDto> Citys{ get; set; }
  }
}
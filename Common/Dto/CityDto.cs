namespace Common
{
  [Serializable]
  public class CityReqDto
  {
    public string? CityName { get; set; }

    public int? Id { get; set; }

    public int? CitySort { get; set; }

    public List<int>? Ids { get; set; }
  }

  public class CityResDto
  {
    public int cityid { get; set; }
    public string cityName { get; set; }
    public int ifCheck { get; set; }
  }

  public class SetCityDto
  {
    public int cityId { get; set; }
  }
}
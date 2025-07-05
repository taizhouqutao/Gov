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
}
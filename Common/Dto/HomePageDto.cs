namespace Common
{
    [Serializable]
    public class HomePageDto
    {
      public required string NewTypeName { get; set; }
      public List<HomeNewItemDto>? News { get; set; }
    }

    [Serializable]
    public class HomeNewItemDto
    {
      public required int Id { get; set; }

      public required string NewTitle { get; set; }

      public required string NewContent { get; set; }

      public DateTime? PublicTime { get; set; }=null;
    }
}
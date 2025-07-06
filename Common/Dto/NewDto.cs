namespace Common
{
  [Serializable]
  public class NewReqDto
  {
    public int? id { get; set; } = null;

    public List<int>? ids { get; set; } = null;

    public string? newTitle { get; set; } = null;

    public string? newContent { get; set; } = null;

    public int? isPublic { get; set; } = null;

    public int? newTypeId { get; set; } = null;

    public List<int>? cityIds { get; set; } = null;

    public int? cityId { get; set; } = null;
  }

  [Serializable]
  public class NewListDto : BasicDto
  {
    public int Id { get; set; }

    public required string NewTitle { get; set; }

    public required int IsPublic { get; set; }

    public string? CityName { get; set; } = null;

    public DateTime? PublicTime { get; set; } = null;

    public int? PublicUserId { get; set; } = null;
  }

  [Serializable]
  public class NewPage
  {
    public required string Title { get; set; }
    public required int NewTypeId { get; set; }

    public List<CityResDto>? Citys { get; set; }

    public int? TotalCount { get; set; }

    public int? PageCount { get; set; }
  }

  [Serializable]
  public class NewDetailPage
  {
    public int Id { get; set; }

    public int TotalCount { get; set; }

    public required string NewTitle { get; set; }

    public required string NewContent { get; set; }

    public DateTime? PublicTime { get; set; } = null;

    public bool IfHiddenComment { get; set; }
  }

  [Serializable]
  public class NewPlusDto:BasicDto
  {
    public int Id { get; set; }

    public required string NewTitle { get; set; }

    public required string NewContent { get; set; }

    public required int IsPublic { get; set; }

    public DateTime? PublicTime { get; set; } = null;

    public int? PublicUserId { get; set; } = null;

    public int? NewTypeId { get; set; } = null;

    public int ViewCount { get; set; } = 0;

    public int CommentCount { get; set; } = 0;

    public int? CityId { get; set; } = null;

    public string? CityName { get; set; } = null;

    public required int IfDel { get; set; }
  }
}
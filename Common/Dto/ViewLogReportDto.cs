namespace Common
{
  [Serializable]
  public class ViewLogReportResDto
  {
    public List<string> newTypeName { get; set; }

    public List<ViewLogReportResItemDto> details { get; set; }
  }

  [Serializable]
  public class ViewLogReportResItemDto
  {
    public required string name { get; set; }

    public required int value { get; set; }
  }

  [Serializable]
  public class ViewLogReportReqDto
  {
    public int? newTypeId { get; set; }

    public int? viewType { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }
  }

  [Serializable]
  public class ViewLogLineResDto
  {
    public required List<string> DayOfWeek { get; set; }

    public required List<int> ViewCount { get; set; }
  }
}
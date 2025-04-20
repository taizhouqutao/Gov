namespace Common
{
  public class BasicDto
  {
    public required DateTime CreateTime { get; set; }

    public required int CreateUserId { get; set; }

    public DateTime? UpdateTime { get; set; }

    public int? UpdateUserId { get; set; }
  }
}
namespace Common
{
  [Serializable]
  public class DutyReqDto
  {
    public DateTime? startDate { get; set; }

    public DateTime? endDate { get; set; }

    public string? startDateStr { get; set; }

    public string? endDateStr { get; set; }

    public bool? allDay { get; set; }

    public List<int>? contactIds { get; set; }

    public List<int>? cityIds { get; set; }
  }

    [Serializable]
    public class DutyDetailDto
    {
      public int id { get; set; }

      public required DateTime startDate { get; set; }

      public required DateTime endDate { get; set; }
      
      public required bool allDay{ get; set; }

      public required int contactId{ get; set; }

      public required string personName{ get; set; }

      public string? post{ get; set; }
    }

    [Serializable]
    public class DutyContactDto
    {
        public int? id{get;set;}

        public string? personName { get; set; }

        public string? post{ get; set; }

        public bool? ifDuty{get;set;}
    }
}
namespace Common
{
    [Serializable]
    public class ContactReqDto
    {
        public int? id { get; set; }

        public List<int>? ids { get; set; }

        public string? personName { get; set; }

        public string? post { get; set; }

        public string? depent { get; set; }

        public string? personHead { get; set; }

        public string? personDesc { get; set; }

        public string? cellphone { get; set; }

        public List<int>? cityIds { get; set; }

        public int cityId{ get; set; }
    }

    [Serializable]
    public class ContactPageDto
    {
        public required List<ContactPageItemDto> contactList { get; set; }

        public List<CityResDto>? Citys { get; set; }
    }

    [Serializable]
    public class ContactPageItemDto
    {
        public int id { get; set; }

        public required string personName { get; set; }

        public required string post { get; set; }

        public string? depent { get; set; }

        public string? personHead { get; set; }

        public string? cityName{ get; set; }
    }

    [Serializable]
    public class ContactPageDetailDto
    {
        public int id { get; set; }

        public required string personName { get; set; }

        public required string post { get; set; }

        public string? depent { get; set; }

        public string? personHead { get; set; }

        public string? desc { get; set; }

        public int TotalCount { get; set; }
    }

    [Serializable]
    public class ContactMessageDto
    {
        public required string personName { get; set; }

        public required string createTime { get; set; }

        public required string content { get; set; }

        public List<ContactMessageReplyDto>? replys { get; set; }
    }

    [Serializable]
    public class ContactMessageAddDto
    {
        public required int contactId { get; set; }

        public required string personName { get; set; }

        public required string personCellphone { get; set; }

        public required string content { get; set; }
    }

    [Serializable]
    public class ContactMessageReplyDto
    {
        public required string personName { get; set; }
        public required string createTime { get; set; }

        public required string content { get; set; }
    }

    [Serializable]
    public class ContactMessageReqDto
    {
        public int? id { get; set; }

        public int? isShow { get; set; }

        public List<int>? ids { get; set; }
        public int? contactId { get; set; }

        public int? ifDeal { get; set; }

        public int? fatherContactMessageId { get; set; }

        public List<int>? fatherContactMessageIds { get; set; }

        public List<int>? cityIds { get; set; } = null;
    }

    [Serializable]
    public class ContactMessageResDto : BasicDto
    {
        public int Id { get; set; }

        public required string PersonName { get; set; }

        public string? PersonCellphone { get; set; }

        public required string Content { get; set; }

        public required int ContactId { get; set; }

        public required string ContactName { get; set; }

        public required int IfDeal { get; set; }

        public required int FatherContactMessageId { get; set; }

        public int? IsShow { get; set; }

        public required int RoleType { get; set; }

        public int? UserId { get; set; }

        public string? CityName{ get; set; }
    }

    [Serializable]
    public class ContactPlusDto : BasicDto
    {
        public int Id { get; set; }

        public required string PersonName { get; set; }

        public required string Post { get; set; }

        public string? Cellphone { get; set; }

        public string? Depent { get; set; }

        public string? PersonHead { get; set; }

        public string? Desc { get; set; }

        public int? CityId { get; set; } = null;
        
        public required int IfDel { get; set; }
        
        public string? CityName { get; set; } = null;
    }
}
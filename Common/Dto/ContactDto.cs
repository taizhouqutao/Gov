namespace Common
{
    [Serializable]
    public class ContactReqDto
    {
        public int? id{get;set;}

        public string? personName { get; set; }

        public string? post{ get; set; }

        public string? depent{ get; set; }

        public string? personHead{ get; set; }

        public string? personDesc{ get; set; }
    }

    [Serializable]
    public class ContactPageDto
    {
        public required List<ContactPageItemDto> contactList {get;set;}
    }

    [Serializable]
    public class ContactPageItemDto
    {
        public int id { get; set; }

        public required string personName { get; set; }

        public required string post{ get; set; }

        public string? depent{ get; set; }

        public string? personHead{ get; set; }
    }

    [Serializable]
    public class ContactPageDetailDto
    {
        public int id { get; set; }

        public required string personName { get; set; }

        public required string post{ get; set; }

        public string? depent{ get; set; }

        public string? personHead{ get; set; }

        public string? desc { get; set; }
    }

    [Serializable]
    public class ContactMessageDto
    {
        public required string visitorName{ get; set; }

        public required string createTime{ get; set; }

        public required string content{ get; set; }
    }

   [Serializable]
    public class ContactMessageReqDto
    {
        public int? contactId{ get; set; }
    }
}
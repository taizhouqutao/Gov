namespace Common
{
    [Serializable]
    public class ContactReqDto
    {

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
}
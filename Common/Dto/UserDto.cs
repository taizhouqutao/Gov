namespace Common
{
    [Serializable]
    public class UserDto
    {
        public string? userHead { get; set; }
    }
    [Serializable]
    public class UserReqDto
    {
        public int? id { get; set; }

        public List<int>? ids { get; set; }

        public int? enable { get; set; }

        public int? idNot { get; set; }

        public string? realName { get; set; }

        public string? userName { get; set; }

        public string? passWord { get; set; }

        public string? userEmail { get; set; }

        public string? userPost { get; set; }

        public string? userHead { get; set; }

        public List<int>? roles { get; set; }

        public List<int>? cityIds { get; set; }
    }

    [Serializable]
    public class UserResDto : BasicDto
    {
        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string RealName { get; set; }

        public required string UserEmail { get; set; }

        public string? UserPost { get; set; }

        public string? UserHead { get; set; }

        public required int Enable { get; set; }

        public List<int>? roles { get; set; }

        public List<int>? cityIds { get; set; }

        public string? CityNames { get; set; }
    }
}
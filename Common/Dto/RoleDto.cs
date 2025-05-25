namespace Common
{
    [Serializable]
    public class RoleReqDto
    {
        public int? id { get; set; } = null;

        public List<int>? ids { get; set; } = null;

        public string? roleName { get; set; } = null;

        public int? userId { get; set; } = null;
    }

    [Serializable]
    public class RoleUserResDto
    {
        public int roleId { get; set; }

        public int ifCheck { get; set; }

        public required string roleName { get; set; }
    }
}
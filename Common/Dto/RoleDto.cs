namespace Common
{
    [Serializable]
    public class RoleReqDto
   {
        public int? id { get; set; } = null;

        public List<int>? ids { get; set; } = null;

        public string? roleName { get; set; } = null;
    } 
}
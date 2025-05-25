namespace Common
{
    [Serializable]
    public class RightTreeDto : BasicDto
    {
        public int? id { get; set; }

        public string? rightName { get; set; }

        public string? rightCode { get; set; }

        /// <summary>
        /// 权限类型 0页面 1地址 3按钮
        /// </summary>
        public string? rightType { get; set; }

        public int? fatherId { get; set; }

        public int? ifchecked { get; set; }

        public List<RightTreeDto>? children { get; set; }
    }

    [Serializable]
    public class RightReqDto
    {
        public int roleId { get; set; }

        public int? UserId { get; set; }

        public List<int>? rightIds { get; set; }
    }
}
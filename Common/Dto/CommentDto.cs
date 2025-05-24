namespace Common
{
    [Serializable]
    public class CommentReqDto
    {
        public int? newId { get; set; }

        public List<int>? ids { get; set; }

        public int? id { get; set; }

        public int? newTypeId { get; set; }

        public List<int>? newTypeIds { get; set; }

        public int? isShow { get; set; }

        public int? fatherCommentId { get; set; }

        public string? content { get; set; }

        public int? ifDeal { get; set; }
    }

    [Serializable]
    public class CommentResDto
    {
        public int Id { get; set; }

        public int? NewId { get; set; }

        public string? NewTitle { get; set; }

        public string? PersonName { get; set; }

        public string? PersonCellphone { get; set; }

        public string? Content { get; set; }

        public int? RoleType { get; set; }

        public int? UserId { get; set; }

        public int? FatherCommentId { get; set; }

        public int? IsShow { get; set; }

        public int? IfDeal { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int? UpdateUserId { get; set; }
    }

    [Serializable]
    public class CommentResDetailDto : CommentResDto
    {
        public string? NewContent { get; set; }

        public string? PersonHead { get; set; }

        public List<CommentResDealDto>? Deals { get; set; }
    }

    [Serializable]
    public class CommentResDealDto
    {
        public string? PersonHead { get; set; }

        public string? PersonName { get; set; }

        public int? UserId { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateUserId { get; set; }

        public string? Content { get; set; }

        public int? RoleType { get; set; }
    }

    [Serializable]
    public class CommentGroupResDto
    {
        public int? newTypeId { get; set; }

        public required string newTypeName { get; set; }

        public required string Link { get; set; }

        public int count { get; set; }

        public string createTime { get; set; }
    }
}
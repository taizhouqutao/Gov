namespace Common
{
    [Serializable]
    public class CommentReqDto
    {
        public int? newId { get; set; }

        public List<int>? ids{ get; set; }

        public int? id {get;set;}

        public int? newTypeId { get; set; }

        public int? isShow { get; set; }
    } 

    [Serializable]
    public class CommentResDto
    {
        public int Id { get; set; }

        public int? NewId { get; set; }

        public string? NewTitle{ get; set; }

        public string? PersonName  { get; set; }

        public string? PersonCellphone  { get; set; }

        public string? Content{get;set;}

        public int? RoleType{get;set;}

        public int? UserId{get;set;}

        public int? FatherCommentId{get;set;}

        public int? IsShow{get;set;}

        public int? IfDeal{get;set;}

        public DateTime CreateTime { get; set; }

        public int CreateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int? UpdateUserId { get; set; }
    } 

    [Serializable]
    public class CommentResDetailDto:CommentResDto
    {
        
    }
}
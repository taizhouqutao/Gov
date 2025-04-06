namespace Common
{
    public class PageList<T>
    {
        public required List<T> Datas{get;set;}
        public int TotalCount{get;set;}
    }

    public class PageReq<T>
    {
        public int PageSize{get;set;}

        public int PageIndex{get;set;}

        public required T Query{get;set;}
    }
}
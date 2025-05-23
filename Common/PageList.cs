﻿namespace Common
{
    [Serializable]
    public class PageList<T>
    {
        public required List<T> data{get;set;}
        public int recordsTotal{get;set;}

        public int recordsFiltered{get;set;}

        public int draw{get;set;}
    }
    [Serializable]
    public class PageReq<T>
    {
        public int draw{get;set;}

        public int start{get;set;}

        public int length{get;set;}

        public PageSearchReq? search{get;set;}

        public List<PageOrderReq>? order{get;set;}

        public T? Query{get;set;}
    }
    [Serializable]
    public class PageOrderReq
    {
        public string? dir{get;set;}
        public int? column{get;set;}

        public string? columnName { get; set; }
    }
    [Serializable]
    public class PageSearchReq
    {
        public string? value{get;set;}
        public bool? regex{get;set;}
    }
}
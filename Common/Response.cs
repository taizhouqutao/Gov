using Microsoft.VisualBasic;

namespace Common
{
  public class Response<T>
  {
    public int IfSuccess{get;set;}
    public string? Msg{get;set;}

    public T? Data{get;set;}
  }

  public class Response
  {
    public int IfSuccess{get;set;}
    public string? Msg{get;set;}
  }
}
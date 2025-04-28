namespace Common
{
    public static class HtmlHelp
    {
        public static string GetString(string HTMLString)
        {
            if (string.IsNullOrEmpty(HTMLString)) return string.Empty;

            // 移除HTML标签
            string result = System.Text.RegularExpressions.Regex.Replace(HTMLString, "<[^>]+>", "");
            
            // 替换HTML实体
            result = System.Web.HttpUtility.HtmlDecode(result);
            
            // 移除多余空白字符
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ");
            
            return result.Trim();
        }
    }
}
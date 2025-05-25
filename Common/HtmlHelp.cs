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

        public static string MaskChineseName(string name)
        {
            if (string.IsNullOrEmpty(name)) return string.Empty;

            // 如果名字长度小于2，直接返回原名字
            if (name.Length < 2) return name;

            // 将名字转换为字符数组
            char[] nameChars = name.ToCharArray();

            // 将第二个字符替换为*
            nameChars[1] = '*';

            return new string(nameChars);
        }

        public static List<int> GetShowCommentTypes()
        {
            return new List<int>() { 12, 1, 10, 3 };
        }

        public static string GetTabName(int NewTypeId)
        {
            switch (NewTypeId)
            {
                case 12:
                    return "在线反馈";
                case 1:
                    return "在线反馈";
                case 3:
                    return "信息反馈";
                case 10:
                    return "问题反馈";
                default:
                    return "";
            }
        }
    }
}
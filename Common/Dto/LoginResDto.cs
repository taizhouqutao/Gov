namespace Common
{
    public class LoginResDto
    {
        public required string UserName { get; set; }

        public required string RealName { get; set; }
    }

    public class LoginReqDto
    {
        public required string UserName { get; set; }

        public required string PassWord { get; set; }
    }
}
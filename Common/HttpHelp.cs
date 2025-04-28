namespace Common
{
    public static class HttpHelp
    {
        public static async Task<string> GetAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    throw new Exception($"HTTP请求失败: {e.Message}");
                }
            }
        }
    }
}
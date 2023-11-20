
namespace BlazorDemo.Server
{
    public class LoginResult
    {
        public bool Authorized { get; set; } = false;

        public string Token { get; set; } = null;

        public string Message { get; set; } = null;

        public string DisplayName { get; set; } = "";
    }
}

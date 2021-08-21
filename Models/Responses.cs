namespace Server
{
    public class ErrorResponse
    {
        public string ErrorMessage { get; set; }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
        public string TwoFactorCode {get; set; }
    }
}
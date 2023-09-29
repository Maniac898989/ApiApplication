namespace ApiApplication.Models
{
    public class AuthToken
    {
        public bool succeeded { get; set; }
        public string uuid { get; set; }
        public string auth_token { get; set; }
    }
}

namespace ST.SolutionHub.Entities.AuthenticationModels
{
    public class AuthToken
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

using System.Collections.Generic;

namespace ST.SolutionHub.Models
{
    public class AuthenticateResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

        public string ProfileImgUrl { get; set; }
    }
}

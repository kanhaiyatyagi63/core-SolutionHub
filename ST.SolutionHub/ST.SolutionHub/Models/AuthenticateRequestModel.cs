using System.ComponentModel.DataAnnotations;

namespace ST.SolutionHub.Models
{
    public class AuthenticateRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

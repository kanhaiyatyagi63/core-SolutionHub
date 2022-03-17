using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ST.SolutionHub.Entities.ProjectTypeModels
{
    public class ProjectTypeAddModel : ProjectType
    {
        [Required]
        public IFormFile Attachment { get; set; }
    }
}

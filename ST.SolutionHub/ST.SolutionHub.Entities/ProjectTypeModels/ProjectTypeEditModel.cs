using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ST.SolutionHub.Entities.ProjectTypeModels
{
    public class ProjectTypeEditModel : ProjectType
    {
        public bool IsAttachmentRemoved { get; set; }
        public IFormFile Attachment { get; set; }
    }
}

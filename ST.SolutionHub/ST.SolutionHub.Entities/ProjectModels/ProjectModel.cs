using System.ComponentModel.DataAnnotations;

namespace ST.SolutionHub.Entities.ProjectModels
{
    public class ProjectModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ClientInformation { get; set; }
        public string DeploymentDetails { get; set; }
        public string Files { get; set; }
        public string Videos { get; set; }
        [Required]
        public int TypeId { get; set; }
    }
}

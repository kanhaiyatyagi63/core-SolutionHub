using System.ComponentModel.DataAnnotations;

namespace ST.SolutionHub.Entities.ProjectTypeModels
{
    public class ProjectType
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string HTML { get; set; }
    }
}   

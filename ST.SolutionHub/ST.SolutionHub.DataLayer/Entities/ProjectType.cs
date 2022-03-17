using ST.SolutionHub.DataLayer.Entities.Base;
using System.Collections.Generic;

namespace ST.SolutionHub.DataLayer.Entities
{
    public class ProjectType : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HTML { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}

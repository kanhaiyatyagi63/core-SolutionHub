using ST.SolutionHub.DataLayer.Entities.Base;

namespace ST.SolutionHub.DataLayer.Entities
{
    public class Project : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ClientInformation { get; set; }
        public string DeploymentDetails { get; set; }
        public string Files { get; set; }
        public string Videos { get; set; }

        public int TypeId { get; set; }
        public ProjectType Type { get; set; }
    }
}

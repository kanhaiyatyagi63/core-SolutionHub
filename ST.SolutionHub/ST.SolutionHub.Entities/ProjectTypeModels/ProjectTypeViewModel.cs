using System.Linq;

namespace ST.SolutionHub.Entities.ProjectTypeModels
{
    public class ProjectTypeViewModel : ProjectType
    {
        public string ShortName
        {
            get
            {
                return Name?.Split(" ").FirstOrDefault();
            }
        }
    }
}

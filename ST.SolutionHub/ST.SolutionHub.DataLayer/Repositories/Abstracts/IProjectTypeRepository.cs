using ST.SolutionHub.DataLayer.Abstracts;
using ST.SolutionHub.DataLayer.Entities;
using System.Threading.Tasks;

namespace ST.SolutionHub.DataLayer.Repositories.Abstracts
{
    public interface IProjectTypeRepository : IRepository<ProjectType, int>
    {
        Task<ProjectType> GetProjectTypeById(int id);

        Task<ProjectType> GetDeletedProjectTypeById(int id);
    }
}

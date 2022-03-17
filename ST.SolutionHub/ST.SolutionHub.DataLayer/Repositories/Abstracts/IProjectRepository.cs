using ST.SolutionHub.DataLayer.Abstracts;
using ST.SolutionHub.DataLayer.Entities;
using System.Threading.Tasks;

namespace ST.SolutionHub.DataLayer.Repositories.Abstracts
{
    public interface IProjectRepository : IRepository<Project, int>
    {
        Task<Project> GetProjectById(int id);

        Task<Project> GetDeletedProjectById(int id);
    }
}

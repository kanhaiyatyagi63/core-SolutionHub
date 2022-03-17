using Microsoft.EntityFrameworkCore;
using ST.SolutionHub.DataLayer.Entities;
using ST.SolutionHub.DataLayer.Repositories.Abstracts;
using System.Linq;
using System.Threading.Tasks;

namespace ST.SolutionHub.DataLayer.Repositories
{
    public class ProjectRepository : Repository<Project, int>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<Project> GetProjectById(int id)
        {
            return await GetQueryable().Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<Project> GetDeletedProjectById(int id)
        {
            return await GetQueryable().Where(x => x.Id == id && x.IsDeleted).FirstOrDefaultAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ST.SolutionHub.DataLayer.Entities;
using ST.SolutionHub.DataLayer.Repositories.Abstracts;
using System.Linq;
using System.Threading.Tasks;

namespace ST.SolutionHub.DataLayer.Repositories
{
    public class ProjectTypeRepository : Repository<ProjectType, int>, IProjectTypeRepository
    {
        public ProjectTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<ProjectType> GetProjectTypeById(int id)
        {
            return await GetQueryable().Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
        }

        public async Task<ProjectType> GetDeletedProjectTypeById(int id)
        {
            return await GetQueryable().Where(x => x.Id == id && x.IsDeleted).FirstOrDefaultAsync();
        }
    }
}

using ST.SolutionHub.DataLayer.Entities;
using ST.SolutionHub.DataLayer.Repositories.Abstracts;

namespace ST.SolutionHub.DataLayer.Repositories
{
    class RefreshTokenRepositry : Repository<RefreshToken, long>, IRefreshTokenRepositry
    {
        public RefreshTokenRepositry(ApplicationDbContext context) : base(context)
        {
        }
    }
}

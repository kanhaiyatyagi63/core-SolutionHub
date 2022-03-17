using ST.SolutionHub.DataLayer.Repositories.Abstracts;
using System;
using System.Threading.Tasks;

namespace ST.SolutionHub.DataLayer.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        public IRefreshTokenRepositry RefreshTokenRepositry { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectTypeRepository ProjectTypeRepository { get; }


        void BeginTransaction();
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void Commit();
        Task CommitAsync();
        void Rollback();
    }
}

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using ST.SolutionHub.DataLayer.Abstracts;
using ST.SolutionHub.DataLayer.Repositories.Abstracts;
using System;
using System.Threading.Tasks;

namespace ST.SolutionHub.DataLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dataContext;
        private IDbContextTransaction _dbTransaction;
        private readonly ILogger<UnitOfWork> _logger;

        public IRefreshTokenRepositry RefreshTokenRepositry { get; }
        public IProjectRepository ProjectRepository { get; }
        public IProjectTypeRepository ProjectTypeRepository { get; }

        public UnitOfWork(ApplicationDbContext dataContext,
             ILogger<UnitOfWork> logger, 
             IRefreshTokenRepositry refreshTokenRepositry,
             IProjectRepository projectRepository,
             IProjectTypeRepository projectTypeRepository)
        {
            RefreshTokenRepositry = refreshTokenRepositry;
            ProjectRepository = projectRepository;
            ProjectTypeRepository = projectTypeRepository;
            _dataContext = dataContext;
            _logger = logger;
        }

        public void BeginTransaction()
        {
            _dbTransaction = _dataContext.Database.BeginTransaction();
        }

        public int SaveChanges()
        {
            return _dataContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }

        public void Commit()
        {
            _dbTransaction.Commit();
        }
        public async Task CommitAsync()
        {
            try
            {
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured on SaveChanges.");
                throw;
            }
        }
        public void Rollback()
        {
            _dbTransaction.Rollback();
        }
        void IDisposable.Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }
    }
}

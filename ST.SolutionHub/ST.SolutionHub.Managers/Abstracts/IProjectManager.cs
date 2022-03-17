using ST.SolutionHub.Entities.ProjectModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST.SolutionHub.Managers.Abstracts
{
    public interface IProjectManager
    {
        Task<IEnumerable<ProjectViewModel>> Get();
        Task<ProjectViewModel> GetAsync(int id);
        Task<ProjectViewModel> AddAsync(ProjectAddModel model);
        Task<ProjectViewModel> UpdateAsync(ProjectEditModel model);
        Task DeleteAsync(int[] ids);
        Task UnDeleteAsync(int[] ids);
        Task PermanentDeleteAsync(int[] ids);
    }
}

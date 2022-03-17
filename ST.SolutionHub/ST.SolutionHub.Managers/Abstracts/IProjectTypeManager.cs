using Microsoft.AspNetCore.Http;
using ST.SolutionHub.Entities.ProjectTypeModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST.SolutionHub.Managers.Abstracts
{
    public interface IProjectTypeManager
    {
        Task<IEnumerable<ProjectTypeViewModel>> Get();
        Task<ProjectTypeViewModel> GetAsync(int id);
        Task<ProjectTypeViewModel> AddAsync(ProjectTypeAddModel model);
        Task<ProjectTypeViewModel> UpdateAsync(ProjectTypeEditModel model);
        Task DeleteAsync(int[] ids);
        Task UnDeleteAsync(int[] ids);
        Task PermanentDeleteAsync(int[] ids);
        string GetImagePath(IFormFile fromFile, string forlderName);
        bool DeleteImagePath(string filePath);
    }
}

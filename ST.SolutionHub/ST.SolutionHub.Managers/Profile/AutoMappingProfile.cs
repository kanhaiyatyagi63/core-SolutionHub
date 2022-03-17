using AutoMapper;
using ST.SolutionHub.Entities.ProjectModels;
using ST.SolutionHub.Entities.ProjectTypeModels;

namespace ST.SolutionHub.Managers
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            #region project
            CreateMap<DataLayer.Entities.Project, ProjectViewModel>();
            CreateMap<DataLayer.Entities.ProjectType, ProjectTypeViewModel>();
            #endregion

            #region projecttype
            CreateMap<DataLayer.Entities.ProjectType, ProjectTypeViewModel>();
            #endregion
        }
    }
}

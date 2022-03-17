using AutoMapper;
using Microsoft.Extensions.Logging;
using ST.SolutionHub.DataLayer.Abstracts;
using ST.SolutionHub.DataLayer.Entities;
using ST.SolutionHub.Entities.ProjectModels;
using ST.SolutionHub.Managers.Abstracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ST.SolutionHub.Managers
{
    public class ProjectManager : IProjectManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly ILogger _logger;

        public ProjectManager(IUnitOfWork unitOfWork, IMapper mapper,ILogger<ProjectManager> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ProjectViewModel>> Get()
        {
            var projects = await _unitOfWork.ProjectRepository.GetListAsync(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<ProjectViewModel>>(projects);
        }

        public async Task<ProjectViewModel> GetAsync(int id)
        {
            var project = await _unitOfWork.ProjectRepository.GetProjectById(id);
            if (project == null)
                throw new Exception("Project does not exists");

            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<ProjectViewModel> AddAsync(ProjectAddModel model)
        {
            _logger.LogDebug($"AddProject: Project creation for label name  {model.Name} started");

            _logger.LogDebug("AddProject: Validation Starts");
            if (await _unitOfWork.ProjectRepository.AnyAsync(x => x.Id != model.Id &&
                                                                      x.Name == model.Name.Trim() &&
                                                                      x.TypeId == model.TypeId))
            {
                _logger.LogDebug($"A Project with label name {model.Name} already exists");
                throw new ArgumentException($"A Project with label name '{model.Name}' already exists");
            }

            var project = await _unitOfWork.ProjectRepository.InsertAsync(new Project
            {
                Name = model.Name,
                TypeId = model.TypeId,
                ClientInformation = model.ClientInformation,
                DeploymentDetails = model.DeploymentDetails,
                Description = model.Description,
                Files = model.Files,
                Videos = model.Videos
            });
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProjectViewModel>(project);
        }

        public async Task<ProjectViewModel> UpdateAsync(ProjectEditModel model)
        {
            _logger.LogDebug($"Updateproject: Project updation for name  {model.Name} started");
            _logger.LogDebug("Updateproject: Validation Starts");

            var project = await _unitOfWork.ProjectRepository.GetProjectById(model.Id);
            if (project == null)
            {
                _logger.LogDebug($"Project does mot esists");
                throw new ArgumentException("Project does not esists");
            }
            if (await _unitOfWork.ProjectRepository.AnyAsync(x => x.Id != model.Id &&
                                                                      x.Name == model.Name.Trim() &&
                                                                      x.TypeId == model.TypeId))
            {
                _logger.LogDebug($"A Project with name {model.Name} already exists");
                throw new ArgumentException($"A Project with name '{model.Name}' already exists");
            }

            project.Name = model.Name.Trim();
            project.Description = model.Description;
            project.Videos = model.Videos;
            project.Files = model.Files;
            project.DeploymentDetails = model.DeploymentDetails;
            project.ClientInformation = model.ClientInformation;

            _logger.LogDebug("Updateproject: Commiting updates");
            await _unitOfWork.CommitAsync();

            _logger.LogDebug("Updateproject: Records updated successfully");

            return _mapper.Map<ProjectViewModel>(project);

        }

        public async Task DeleteAsync(int[] ids)
        {
            _logger.LogDebug("Deleteproject(s): Process started");
            foreach (var projectId in ids)
            {
                var project = await _unitOfWork.ProjectRepository.GetProjectById(projectId);
                _logger.LogDebug($"Deleteproject(s): Validating Project for Id:  {projectId}");
                if (project == null)
                {
                    _logger.LogDebug($"Deleteproject(s): Project validation failed for Id {projectId}. Error: Invalid Project");
                    throw new Exception("Project deletion failed. Error: Invalid Project");
                }
                if (project.IsDeleted)
                {
                    _logger.LogDebug($"Deleteproject(s): Project validation failed for Id {projectId}. Error: Project is already deleted");
                    throw new Exception("project deletion failed. Error:Project is already deleted");
                }

                _unitOfWork.ProjectRepository.Delete(project);
            }

            _logger.LogDebug($"Deleteproject(s): Commiting changes");
            await _unitOfWork.CommitAsync();
            _logger.LogDebug($"Deleteproject(s): Record(s) deleted successfully");
        }

        public async Task UnDeleteAsync(int[] ids)
        {
            _logger.LogDebug("UnDeleteproject(s): Process started");
            foreach (var projectId in ids)
            {
                var project = await _unitOfWork.ProjectRepository.GetDeletedProjectById(projectId);
                _logger.LogDebug($"UnDeleteproject(s): Validating Project for Id:  {projectId}");
                if (project == null)
                {
                    _logger.LogDebug($"UnDelete project(s): Project validation failed for Id {projectId}. Error: Invalid Project ");
                    throw new Exception("project undeletion failed. Error: Invalid Project");
                }
                if (!project.IsDeleted)
                {
                    _logger.LogDebug($"UnDeleteproject(s): Project validation failed for Id {projectId}. Error: Project is already undeleted");
                    throw new Exception("project undeletion failed. Error: Project is already undeleted");
                }

                _unitOfWork.ProjectRepository.UnDelete(project);

            }

            _logger.LogDebug($"UnDeleteproject(s): Committing changes");
            await _unitOfWork.CommitAsync();
            _logger.LogDebug($"UnDeleteproject(s): Record(s) has/have been undeleted successfully");
        }

        public async Task PermanentDeleteAsync(int[] ids)
        {
            _logger.LogDebug("PermanentDeleteproject(s): Process started");
            foreach (var projectId in ids)
            {
                var project = await _unitOfWork.ProjectRepository.GetProjectById(projectId);
                _logger.LogDebug($"Deleteproject(s): Validating Project for Id:  {projectId}");
                if (project == null)
                {
                    _logger.LogDebug($"Permanentproject(s): Project validation failed for Id {projectId}. Error: Invalid Project");
                    throw new Exception("Permanent project deletion failed. Error: Invalid Project");
                }
                _unitOfWork.ProjectRepository.Remove(project);
            }

            _logger.LogDebug($"PermanentDeleteproject(s): Commiting changes");
            await _unitOfWork.CommitAsync();
            _logger.LogDebug($"PermanentDeleteproject(s): Record(s) deleted successfully");
        }
    }
}

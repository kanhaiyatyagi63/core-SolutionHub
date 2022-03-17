using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ST.SolutionHub.DataLayer.Abstracts;
using ST.SolutionHub.Entities.ProjectTypeModels;
using ST.SolutionHub.Managers.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ST.SolutionHub.Managers
{
    public class ProjectTypeManager : IProjectTypeManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public readonly ILogger _logger;

        public ProjectTypeManager(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProjectTypeManager> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<IEnumerable<ProjectTypeViewModel>> Get()
        {
            var projectTypes = await _unitOfWork.ProjectTypeRepository.GetListAsync(x => !x.IsDeleted);
            return _mapper.Map<IEnumerable<ProjectTypeViewModel>>(projectTypes);
        }

        public async Task<ProjectTypeViewModel> GetAsync(int id)
        {
            var projectType = await _unitOfWork.ProjectTypeRepository.GetProjectTypeById(id);
            if (projectType == null)
                throw new Exception("projectType does not exists");

            return _mapper.Map<ProjectTypeViewModel>(projectType);
        }

        public async Task<ProjectTypeViewModel> AddAsync(ProjectTypeAddModel model)
        {
            _logger.LogDebug($"AddProjectType: projectType creation for label name  {model.Name} started");

            _logger.LogDebug("AddProjectType: Validation Starts");
            if (await _unitOfWork.ProjectTypeRepository.AnyAsync(x => x.Id != model.Id &&
                                                                      x.Name == model.Name.Trim()))
            {
                _logger.LogDebug($"A projectType with label name {model.Name} already exists");
                throw new ArgumentException($"A projectType with label name '{model.Name}' already exists");
            }

            var projectType = await _unitOfWork.ProjectTypeRepository.InsertAsync(new DataLayer.Entities.ProjectType
            {
                Name = model.Name,
                HTML = model.HTML,
                Description = model.Description
            });
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ProjectTypeViewModel>(projectType);
        }

        public async Task<ProjectTypeViewModel> UpdateAsync(ProjectTypeEditModel model)
        {
            _logger.LogDebug($"Updateproject: projectType updation for name  {model.Name} started");
            _logger.LogDebug("Updateproject: Validation Starts");

            var projectType = await _unitOfWork.ProjectTypeRepository.GetProjectTypeById(model.Id);
            if (projectType == null)
            {
                _logger.LogDebug($"projectType does not esists");
                throw new ArgumentException("projectType does not esists");
            }
            if (await _unitOfWork.ProjectTypeRepository.AnyAsync(x => x.Id != model.Id &&
                                                                      x.Name == model.Name.Trim()))
            {
                _logger.LogDebug($"A projectType with name {model.Name} already exists");
                throw new ArgumentException($"A projectType with name '{model.Name}' already exists");
            }

            projectType.Name = model.Name.Trim();
            projectType.Description = model.Description;
            projectType.HTML = model.HTML;

            _logger.LogDebug("Updateproject: Commiting updates");
            await _unitOfWork.CommitAsync();

            _logger.LogDebug("Updateproject: Records updated successfully");

            return _mapper.Map<ProjectTypeViewModel>(projectType);

        }

        public async Task DeleteAsync(int[] ids)
        {
            _logger.LogDebug("Deleteproject(s): Process started");
            foreach (var projectTypeId in ids)
            {
                var projectType = await _unitOfWork.ProjectTypeRepository.GetProjectTypeById(projectTypeId);
                _logger.LogDebug($"Deleteproject(s): Validating projectType for Id:  {projectTypeId}");
                if (projectType == null)
                {
                    _logger.LogDebug($"Deleteproject(s): projectType validation failed for Id {projectTypeId}. Error: Invalid projectType");
                    throw new Exception("projectType deletion failed. Error: Invalid projectType");
                }
                if (projectType.IsDeleted)
                {
                    _logger.LogDebug($"Deleteproject(s): projectType validation failed for Id {projectTypeId}. Error: projectType is already deleted");
                    throw new Exception("projectType deletion failed. Error:projectType is already deleted");
                }

                _unitOfWork.ProjectTypeRepository.Delete(projectType);
            }

            _logger.LogDebug($"Deleteproject(s): Commiting changes");
            await _unitOfWork.CommitAsync();
            _logger.LogDebug($"Deleteproject(s): Record(s) deleted successfully");
        }

        public async Task UnDeleteAsync(int[] ids)
        {
            _logger.LogDebug("UnDeleteproject(s): Process started");
            foreach (var projectTypeId in ids)
            {
                var projectType = await _unitOfWork.ProjectTypeRepository.GetDeletedProjectTypeById(projectTypeId);
                _logger.LogDebug($"UnDeleteproject(s): Validating projectType for Id:  {projectTypeId}");
                if (projectType == null)
                {
                    _logger.LogDebug($"UnDelete projectType(s): projectType validation failed for Id {projectTypeId}. Error: Invalid projectType ");
                    throw new Exception("projectType undeletion failed. Error: Invalid projectType");
                }
                if (!projectType.IsDeleted)
                {
                    _logger.LogDebug($"UnDeleteproject(s): projectType validation failed for Id {projectTypeId}. Error: projectType is already undeleted");
                    throw new Exception("projectType undeletion failed. Error: projectType is already undeleted");
                }

                _unitOfWork.ProjectTypeRepository.UnDelete(projectType);

            }

            _logger.LogDebug($"UnDeleteprojectTypes(s): Committing changes");
            await _unitOfWork.CommitAsync();
            _logger.LogDebug($"UnDeleteprojectTypes(s): Record(s) has/have been undeleted successfully");
        }

        public async Task PermanentDeleteAsync(int[] ids)
        {
            _logger.LogDebug("PermanentDeleteprojectTypes(s): Process started");
            foreach (var projectTypeId in ids)
            {
                var projectType = await _unitOfWork.ProjectTypeRepository.GetProjectTypeById(projectTypeId);
                _logger.LogDebug($"Deleteproject(s): Validating projectType for Id:  {projectTypeId}");
                if (projectType == null)
                {
                    _logger.LogDebug($"Permanentproject(s): projectType validation failed for Id {projectTypeId}. Error: Invalid projectType");
                    throw new Exception("Permanent projectType deletion failed. Error: Invalid projectType");
                }
                _unitOfWork.ProjectTypeRepository.Remove(projectType);
            }

            _logger.LogDebug($"PermanentDeleteproject(s): Commiting changes");
            await _unitOfWork.CommitAsync();
            _logger.LogDebug($"PermanentDeleteproject(s): Record(s) deleted successfully");
        }

        public string GetImagePath(IFormFile fromFile, string forlderName)
        {
            try
            {
                var file = fromFile;
                var folderName = Path.Combine("StaticFiles", forlderName);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file != null)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return dbPath;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteImagePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;
            try
            {
                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (File.Exists(rootPath))
                {
                    // If file found, delete it    
                    File.Delete(rootPath);
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST.SolutionHub.Entities.ProjectTypeModels;
using ST.SolutionHub.Extensions;
using ST.SolutionHub.Managers.Abstracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ST.SolutionHub.Controllers
{
    public class ProjectTypeController : BaseController
    {
        private readonly IProjectTypeManager _projectTypeManager;
        private readonly ILogger<ProjectTypeController> _logger;


        public ProjectTypeController(IProjectTypeManager projectTypeManager, ILogger<ProjectTypeController> logger)
        {
            _projectTypeManager = projectTypeManager;
            _logger = logger;
        }

        [HttpGet, Route("get")]
        [ProducesResponseType(typeof(IEnumerable<ProjectTypeViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await _projectTypeManager.Get();
            return Ok(response);
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(ProjectTypeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest(new ArgumentException(nameof(id)).Message);
            var response = await _projectTypeManager.GetAsync(id);
            return Ok(response);
        }

        [HttpPost, Route("post")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromForm] ProjectTypeAddModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            try
            {
                model.HTML = _projectTypeManager.GetImagePath(model.Attachment, "ProjectTypes");
                await _projectTypeManager.AddAsync(model);
                return Ok("Project type has been added successfully!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("put")]
        [ProducesResponseType(typeof(ProjectTypeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromForm] ProjectTypeEditModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.GetErrorList());
                if (model.IsAttachmentRemoved)
                {
                    _projectTypeManager.DeleteImagePath(model.HTML);
                    model.HTML = _projectTypeManager.GetImagePath(model.Attachment, "ProjectTypes");
                }
                return Ok(await _projectTypeManager.UpdateAsync(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] int[] ids)
        {
            if (ids == null || !ids.Any())
                return BadRequest(new ArgumentException("Request parameter can't be null/empty").Message);
            try
            {
                await _projectTypeManager.DeleteAsync(ids);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while deleting Case Region");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("undeleted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UnDelete([FromQuery] int[] ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                    return BadRequest(new ArgumentException("Request parameter can't be null/empty").Message);
                await _projectTypeManager.UnDeleteAsync(ids);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete, Route("permanentdelete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PermanentDelete([FromQuery] int[] ids)
        {
            try
            {
                if (ids == null || !ids.Any())
                    return BadRequest(new ArgumentException("Request parameter can't be null/empty").Message);
                await _projectTypeManager.PermanentDeleteAsync(ids);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
    }
}

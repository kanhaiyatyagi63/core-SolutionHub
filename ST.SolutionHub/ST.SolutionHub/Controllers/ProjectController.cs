using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST.SolutionHub.Entities.ProjectModels;
using ST.SolutionHub.Extensions;
using ST.SolutionHub.Managers.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ST.SolutionHub.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectManager _projectManager;
        private readonly ILogger<ProjectController> _logger;


        public ProjectController(IProjectManager projectManager, ILogger<ProjectController> logger)
        {
            _projectManager = projectManager;
            _logger = logger;
        }

        [HttpGet, Route("get")]
        [ProducesResponseType(typeof(IEnumerable<ProjectViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var response = await _projectManager.Get();
            return Ok(response);
        }

        [HttpGet, Route("{id}")]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0)
                return BadRequest(new ArgumentException(nameof(id)).Message);
            var response = await _projectManager.GetAsync(id);
            return Ok(response);
        }

        [HttpPost, Route("post")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProjectAddModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorList());
            }

            try
            {
                await _projectManager.AddAsync(model);
                return Ok("Project has been added successfully!!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut, Route("put")]
        [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] ProjectEditModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState.GetErrorList());
                return Ok(await _projectManager.UpdateAsync(model));
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
                await _projectManager.DeleteAsync(ids);
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
                await _projectManager.UnDeleteAsync(ids);
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
                await _projectManager.PermanentDeleteAsync(ids);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

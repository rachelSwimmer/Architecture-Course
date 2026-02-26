using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;
using server.Services;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly ILogger<PackageController> _logger;


        public PackageController(IPackageService packageService, ILogger<PackageController> logger)
        {
            _packageService = packageService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PackageResponseDtos>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PackageResponseDtos>>> GetAll()
        {
            var packages = await _packageService.GetAll();
            return Ok(packages);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PackageResponseDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var package = await _packageService.GetById(id);
            if (package == null)
            {
                return NotFound(new { message = $"Package with ID {id} not exist" });
            }
            return Ok(package);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PackageResponseDtos), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddPackage([FromBody] PackageCreateDtos createDto)
        {
            try
            {
                var package = await _packageService.AddPackage(createDto);
                //return CreatedAtAction(nameof(GetById), new { id = category.Id, category });
                return Ok(package);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { messege = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PackageResponseDtos), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePackage(int id)
        {
            var reasult = await _packageService.DeletePackage(id);
            if (!reasult)
            {
                return NotFound(new { message = $"Package with Id {id} not found" });
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PackageResponseDtos), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePackage([FromRoute] int id, [FromBody] PackageUpdateDtos updateDto)
        {
            try
            {
                var package = await _packageService.UpdatePackage(id, updateDto);
                return Ok(package);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { messege = ex.Message });
            }
        }
        [HttpGet("sortBy")]
        [ProducesResponseType(typeof(PackageResponseDtos), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PackageResponseDtos>>> SortPackages([FromQuery] string ? sortBy)
        {
            var packages = await _packageService.SortPackages(sortBy);
            return Ok(packages);
        }

    }
}

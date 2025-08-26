using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleManagement.Api.Models;
using VehicleManagement.Api.Repositories;
using VehicleManagement.Api.Services.Interfaces;
using VehicleManagement.Api.Services;
using VehicleManagement.Api.Repositories.Interfaces;

namespace VehicleManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IVehicleService vehicleService, ILogger<VehiclesController> logger)
        {
            _vehicleService = vehicleService;
            _logger = logger;
        }

        // GET /api/vehicles?plate=ABC1234&ids=id1,id2
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Vehicle>>> Get([FromQuery] string? plate, [FromQuery] string? ids)
        {
            _logger.LogInformation("[GET] /api/vehicles called with plate={Plate} ids={Ids}", plate, ids);
            try
            {
                var vehicles = await _vehicleService.GetVehiclesAsync(plate, ids);
                _logger.LogInformation("[GET] /api/vehicles success: {Count} vehicles returned", vehicles?.Count() ?? 0);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GET] /api/vehicles failed: {Message}", ex.Message);
                return StatusCode(500, new ErrorResponse("Erro ao buscar veículos."));
            }
        }

        // POST /api/vehicles
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            _logger.LogInformation("[POST] /api/vehicles called with plate={Plate}, model={Model}, year={Year}", vehicle.Plate, vehicle.Model, vehicle.Year);
            if (string.IsNullOrWhiteSpace(vehicle.Model))
            {
                _logger.LogWarning("[POST] /api/vehicles failed: Model is required");
                return BadRequest(new ErrorResponse("Model is required."));
            }
            if (vehicle.Year < 2020)
            {
                _logger.LogWarning("[POST] /api/vehicles failed: Year must be >= 2020");
                return BadRequest(new ErrorResponse("Year must be >= 2020."));
            }
            if (string.IsNullOrWhiteSpace(vehicle.Plate))
            {
                _logger.LogWarning("[POST] /api/vehicles failed: Plate is required");
                return BadRequest(new ErrorResponse("Plate is required."));
            }
            try
            {
                vehicle.CreatedAt = DateTime.UtcNow;
                await _vehicleService.CreateVehicleAsync(vehicle);
                _logger.LogInformation("[POST] /api/vehicles success: Vehicle created with plate={Plate}", vehicle.Plate);
                return CreatedAtAction(nameof(Get), new { plate = vehicle.Plate }, vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[POST] /api/vehicles failed: {Message}", ex.Message);
                return StatusCode(500, new ErrorResponse("Erro ao criar veículo."));
            }
        }

        // PUT /api/vehicles/{id}
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(string id, [FromBody] Vehicle vehicle)
        {
            _logger.LogInformation("[PUT] /api/vehicles/{Id} called", id);
            if (string.IsNullOrWhiteSpace(vehicle.Model))
            {
                _logger.LogWarning("[PUT] /api/vehicles/{Id} failed: Model is required", id);
                return BadRequest(new ErrorResponse("Model is required."));
            }
            if (vehicle.Year < 2020)
            {
                _logger.LogWarning("[PUT] /api/vehicles/{Id} failed: Year must be >= 2020", id);
                return BadRequest(new ErrorResponse("Year must be >= 2020."));
            }
            if (string.IsNullOrWhiteSpace(vehicle.Plate))
            {
                _logger.LogWarning("[PUT] /api/vehicles/{Id} failed: Plate is required", id);
                return BadRequest(new ErrorResponse("Plate is required."));
            }
            try
            {
                var existing = await _vehicleService.GetVehicleByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("[PUT] /api/vehicles/{Id} failed: Vehicle not found", id);
                    return NotFound(new ErrorResponse("Vehicle not found."));
                }
                vehicle.Id = id;
                vehicle.UpdatedAt = DateTime.UtcNow;
                await _vehicleService.UpdateVehicleAsync(id, vehicle);
                _logger.LogInformation("[PUT] /api/vehicles/{Id} success: Vehicle updated", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[PUT] /api/vehicles/{Id} failed: {Message}", id, ex.Message);
                return StatusCode(500, new ErrorResponse("Erro ao atualizar veículo."));
            }
        }

        // DELETE /api/vehicles/{id}
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("[DELETE] /api/vehicles/{Id} called", id);
            try
            {
                var existing = await _vehicleService.GetVehicleByIdAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("[DELETE] /api/vehicles/{Id} failed: Vehicle not found", id);
                    return NotFound(new ErrorResponse("Vehicle not found."));
                }
                await _vehicleService.DeleteVehicleAsync(id);
                _logger.LogInformation("[DELETE] /api/vehicles/{Id} success: Vehicle deleted", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[DELETE] /api/vehicles/{Id} failed: {Message}", id, ex.Message);
                return StatusCode(500, new ErrorResponse("Erro ao deletar veículo."));
            }
        }

        // POST /api/vehicles/{id}/file
        [HttpPost("{id}/file")]
        [Authorize]
        public async Task<IActionResult> PostFile(string id, [FromServices] MinioService minioService, [FromServices] IVehicleFileRepository fileRepository)
        {
            _logger.LogInformation("[POST] /api/vehicles/{Id}/file called", id);
            try
            {
                var fileName = $"{id}/{Guid.NewGuid()}";
                var uploadUrl = await minioService.GetPresignedUploadUrlAsync(fileName);
                var file = new VehicleFile
                {
                    VehicleId = id,
                    FileName = fileName,
                    FileUrl = uploadUrl,
                    UploadedAt = DateTime.UtcNow
                };
                await fileRepository.CreateAsync(file);
                _logger.LogInformation("[POST] /api/vehicles/{Id}/file success: Upload URL generated", id);
                return Ok(new { uploadUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[POST] /api/vehicles/{Id}/file failed: {Message}", id, ex.Message);
                return StatusCode(500, new ErrorResponse("Erro ao gerar upload de arquivo."));
            }
        }

        // GET /api/vehicles/{id}/file
        [HttpGet("{id}/file")]
        [Authorize]
        public async Task<IActionResult> GetFiles(string id, [FromServices] MinioService minioService, [FromServices] IVehicleFileRepository fileRepository)
        {
            _logger.LogInformation("[GET] /api/vehicles/{Id}/file called", id);
            try
            {
                var files = await fileRepository.GetByVehicleIdAsync(id);
                _logger.LogInformation("[GET] /api/vehicles/{Id}/file success: {Count} files returned", id, files?.Count() ?? 0);
                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GET] /api/vehicles/{Id}/file failed: {Message}", id, ex.Message);
                return StatusCode(500, new ErrorResponse("Erro ao buscar arquivos."));
            }
        }
    }
}

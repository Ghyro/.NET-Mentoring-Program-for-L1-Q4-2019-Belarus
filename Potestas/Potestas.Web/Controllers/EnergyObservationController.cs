using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Potestas.Web.Interfaces;
using Potestas.Web.Models;

namespace Potestas.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyObservationController : ControllerBase
    {
        private readonly IEnergyObservationService _service;
        private readonly ILogger _logger;

        public EnergyObservationController(IEnergyObservationService service, ILogger<EnergyObservationController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Try to get all observations from database");

            var observations = await _service.GetAllObservationsAsync();

            _logger.LogInformation("Finished to fetch all observations from database");

            return Ok(observations);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] FlashObservationViewModel energyObservation)
        {
            _logger.LogInformation("Try to post new observation to database.");

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Unable to save observation with ID {energyObservation.Id} due to model state");

                return BadRequest(ModelState);
            }

            try
            {
                await _service.AddObservationAsync(energyObservation);

                _logger.LogError($"Observation with id {energyObservation.Id} has been created successfully");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to save observation with id {energyObservation.Id} due to: {ex.StackTrace}");

                return BadRequest("Cannot create new Flash Observation. See your request.");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] FlashObservationViewModel energyObservation)
        {
            _logger.LogInformation("Try to remove observation from database.");

            if (!ModelState.IsValid)
            {
                _logger.LogError($"Unable to remove observation with ID {energyObservation.Id} due to model state");

                return BadRequest(ModelState);
            }
                
            try
            {
                await _service.DeleteObservationAsync(energyObservation);

                _logger.LogError($"Observation with id {energyObservation.Id} has been created successfully");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to remove observation with id {energyObservation.Id} due to: {ex.StackTrace}");

                return BadRequest($"Cannot remove Flash Observation with id: {energyObservation.Id}");
            }
        }

        [HttpDelete("Clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete()
        {
            _logger.LogInformation("Try to clear database");

            await _service.ClearObservationsAsync();

            _logger.LogInformation("The database has been cleared successfully");

            return NoContent();
        }
    }
}

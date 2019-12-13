using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Potestas.Web.Interfaces;
using Potestas.Web.Models;

namespace Potestas.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyObservationController : ControllerBase
    {
        private readonly IEnergyObservationService _service;

        public EnergyObservationController(IEnergyObservationService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllObservationsAsync());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] FlashObservationViewModel energyObservation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.AddObservationAsync(energyObservation);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Cannot create new Flash Observation. See your request.");
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] FlashObservationViewModel flashObservation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _service.DeleteObservationAsync(flashObservation);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest($"Cannot remove Flash Observation with id: {flashObservation.Id}");
            }
        }

        [HttpDelete("Clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete()
        {
            await _service.ClearObservationsAsync();

            return NoContent();
        }
    }
}

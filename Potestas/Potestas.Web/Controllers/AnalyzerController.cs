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
    public class AnalyzerController : ControllerBase
    {
        private readonly IAnalyzer _analyzer;

        public AnalyzerController(IAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        [HttpGet("GetAverageEnergyAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAverageEnergyAsync()
        {
            return Ok(await _analyzer.GetAverageEnergyAsync());
        }

        [HttpGet("GetDistributionByCoordinatesAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDistributionByCoordinatesAsync()
        {
            return Ok(await _analyzer.GetDistributionByCoordinatesAsync());
        }

        [HttpGet("GetDistributionByEnergyValueAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDistributionByEnergyValueAsync()
        {
            return Ok(await _analyzer.GetDistributionByEnergyValueAsync());
        }

        [HttpGet("GetDistributionByObservationTimeAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDistributionByObservationTimeAsync()
        {
            return Ok(await _analyzer.GetDistributionByObservationTimeAsync());
        }

        [HttpGet("GetMaxEnergyAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyAsync()
        {
            return Ok(await _analyzer.GetMaxEnergyAsync());
        }

        [HttpGet("GetMaxEnergyPositionAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyPositionAsync()
        {
            return Ok(await _analyzer.GetMaxEnergyPositionAsync());
        }

        [HttpGet("GetMaxEnergyTimeAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyTimeAsync()
        {
            return Ok(await _analyzer.GetMaxEnergyTimeAsync());
        }

        [HttpGet("GetMinEnergyAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyAsync()
        {
            return Ok(await _analyzer.GetMinEnergyAsync());
        }

        [HttpGet("GetMinEnergyPositionAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyPositionAsync()
        {
            return Ok(await _analyzer.GetMinEnergyPositionAsync());
        }

        [HttpGet("GetMinEnergyTimeAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyTimeAsync()
        {
            return Ok(await _analyzer.GetMinEnergyTimeAsync());
        }

        //[HttpGet]
        //[Route("api/Analyzer/{paramOne}/{paramTwo}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetAverageEnergyAsyncCoordinates(CoordinatesViewModel cor1, CoordinatesViewModel cor2)
        //{
        //    return Ok(await _analyzer.GetAverageEnergyAsync(cor1, cor2));
        //}

        [HttpGet("GetAverageEnergyAsyncDateTimeWithDoubleDateTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAverageEnergyAsyncDateTime(DateTime dt1, DateTime dt2)
        {
            return Ok(await _analyzer.GetAverageEnergyAsync(dt1, dt2));
        }

        [HttpGet("GetMaxEnergyAsyncCoordinatesWithCoordinates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyAsyncCoordinates(CoordinatesViewModel cor)
        {
            return Ok(await _analyzer.GetMaxEnergyAsync(cor));
        }

        [HttpGet("GetMaxEnergyAsyncDateTimeWithDateTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMaxEnergyAsyncDateTime(DateTime dt)
        {
            return Ok(await _analyzer.GetMaxEnergyAsync(dt));
        }

        [HttpGet("GetMinEnergyAsyncCoordinatesWithCoordinates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyAsyncCoordinates(CoordinatesViewModel cor)
        {
            return Ok(await _analyzer.GetMinEnergyAsync(cor));
        }

        [HttpGet("GetMinEnergyAsyncDateTimeWithDateTime")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMinEnergyAsyncDateTime(DateTime dt)
        {
            return Ok(await _analyzer.GetMinEnergyAsync(dt));
        }
    }
}

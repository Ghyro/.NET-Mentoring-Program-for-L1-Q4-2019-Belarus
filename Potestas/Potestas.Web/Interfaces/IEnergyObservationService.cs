using System.Collections.Generic;
using System.Threading.Tasks;
using Potestas.Web.Models;

namespace Potestas.Web.Interfaces
{
    public interface IEnergyObservationService
    {
        Task<IEnumerable<FlashObservationViewModel>> GetAllObservationsAsync();

        Task AddObservationAsync(FlashObservationViewModel flashObservation);

        Task DeleteObservationAsync(FlashObservationViewModel flashObservation);

        Task ClearObservationsAsync();
    }
}

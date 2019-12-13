using AutoMapper;
using Potestas.Interfaces;
using Potestas.Observations;
using Potestas.Web.Models;

namespace Potestas.Web.Mappers
{
    public class EnergyObservationMapper : Profile
    {
        public EnergyObservationMapper()
        {
            CreateMap<CoordinatesViewModel, IEnergyObservation>();

            CreateMap<CoordinatesViewModel, Coordinates>().ReverseMap();

            CreateMap<CoordinatesViewModel, Coordinates>()
                .ConstructUsing(src => new Coordinates
                {
                    Id = src.Id,
                    X = src.X,
                    Y = src.Y
                });

            CreateMap<FlashObservation, FlashObservationViewModel>()
                .ForMember(d => d.ObservationPoint, opt => opt.MapFrom(src => src.ObservationPoint))
                .ReverseMap()
                .ForPath(d => d.ObservationPoint, opt => opt.MapFrom(src => src.ObservationPoint));

            CreateMap<FlashObservation, IEnergyObservation>()
                .ConstructUsing(src => new FlashObservation
                {
                    Id = src.Id,
                    ObservationPoint = new Coordinates { Id = src.ObservationPoint.Id, X = src.ObservationPoint.X, Y = src.ObservationPoint.Y },
                    EstimatedValue = src.EstimatedValue,
                    ObservationTime = src.ObservationTime
                });
        }
    }
}

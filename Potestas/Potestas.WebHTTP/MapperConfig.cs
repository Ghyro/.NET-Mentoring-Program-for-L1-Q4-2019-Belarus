using AutoMapper;

namespace Potestas.WebHTTP
{
    public class MapperConfig
    {
        public static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Coordinates, Models.Coordinates>();
                cfg.CreateMap<Models.Coordinates, Coordinates>();

                cfg.CreateMap<Observations.FlashObservation, Models.FlashObservation>();
                cfg.CreateMap<Models.FlashObservation, Observations.FlashObservation>();
            });
            configuration.AssertConfigurationIsValid();

            var mapper = configuration.CreateMapper();

            return mapper;
        }

    }
}

using AutoMapper;
using MoneyManager.Application.Extensions;
using MoneyManager.Application.Mappings;

namespace MoneyManager.Application.Tests.Fixtures
{
    public class MapperFixture
    {
        public IMapper Mapper { get; }

        public MapperFixture()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddApplicationProfile();
            });

            Mapper = mapperConfig.CreateMapper();
        }
    }
}

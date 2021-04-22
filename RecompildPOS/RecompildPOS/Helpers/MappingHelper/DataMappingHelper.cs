using AutoMapper;

namespace RecompildPOS.Helpers.MappingHelper
{
    public class DataMappingHelper<T, TV>
    {
        public T MapModel(TV v)
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<TV, T>();
            });
            var iMapper = config.CreateMapper();
            var value = iMapper.Map<TV, T>(v);
            return value;
        }
    }
}
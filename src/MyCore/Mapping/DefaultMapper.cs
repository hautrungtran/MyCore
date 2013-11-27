using AutoMapper;

namespace MyCore.Mapping {
    public class DefaultMapper : IMapper {
        #region Implementation of IMapper

        public TDestination MapTo<TSource, TDestination>(TSource source) {
            return Mapper.Map<TDestination>(source);
        }
        public TDestination UpdateTo<TSource, TDestination>(TSource source, TDestination destination) {
            return Mapper.Map(source, destination);
        }

        #endregion
    }
}
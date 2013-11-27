namespace MyCore.Mapping {
    public interface IMapper {
        TDestination MapTo<TSource, TDestination>(TSource source);
        TDestination UpdateTo<TSource, TDestination>(TSource source, TDestination destination);
    }
}
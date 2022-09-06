namespace Brunsker.Bsnotas.Application.AutoMapperConfigurations
{
    public static class AutoMapperExtension
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
            =>
            AutoMapperConfig.Mapper.Map<TSource, TDestination>(source);

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
            =>
            AutoMapperConfig.Mapper.Map(source, destination);
    }
}

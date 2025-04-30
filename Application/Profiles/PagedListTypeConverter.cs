using AutoMapper;
using Domain.Shared;

namespace Application.Profiles;

public class PagedListTypeConverter<TSource, TDestination>: ITypeConverter<PagedList<TSource>, PagedList<TDestination>> where TSource : class where TDestination : class
{
    public PagedList<TDestination> Convert(PagedList<TSource> source, PagedList<TDestination> destination, ResolutionContext context)
    {
        var mappedItems = context.Mapper.Map<List<TDestination>>(source.ToList());
        return new PagedList<TDestination>(
          mappedItems,
          source.MetaData.TotalCount,
          source.MetaData.CurrentPage,
          source.MetaData.PageSize
      );
    }
}

namespace Core.SeedWork
{
    public interface IOrderedFilter
    {
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
    }

    public interface IPagedFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public enum OrderByTypes
    {
        Price,
        Relevance,
        Alphabetic,
        MostViews,
        MostStarred
    }

    public enum OrderDirectionTypes
    {
        Asc,
        Desc
    }

    public static class IFilterExtensions
    {
        public static OrderDirectionTypes GetOrderDirection(this IOrderedFilter filter) =>
            (filter.OrderDirection ?? string.Empty).ToUpper() switch
            {
                "ASC" => OrderDirectionTypes.Asc,
                "DESC" => OrderDirectionTypes.Desc,
                _ => OrderDirectionTypes.Desc
            };

        public static OrderByTypes GetOrderBy(this IOrderedFilter filter) =>
            (filter.OrderBy ?? string.Empty).ToUpper() switch
            {
                "PRICE" => OrderByTypes.Price,                
                "ALPHABETIC" => OrderByTypes.Alphabetic,                
                _ => OrderByTypes.Relevance
            };
    }
}
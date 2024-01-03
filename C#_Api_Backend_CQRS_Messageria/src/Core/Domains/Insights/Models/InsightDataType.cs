using Core.SeedWork;

namespace Core.Models.Core.Insights
{
    public class InsightDataType : Enumeration
    {
        public InsightDataType(int id, string name) : base(id, name) { }
        public static InsightDataType CatalogOpen = new InsightDataType(1, "CatalogOpen");
    }

    public enum InsightDataTypeEnum
    {
        CatalogOpen = 1
    }
}
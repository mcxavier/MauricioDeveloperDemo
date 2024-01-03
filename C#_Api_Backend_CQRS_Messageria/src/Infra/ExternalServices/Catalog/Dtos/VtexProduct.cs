using System;

namespace CoreService.IntegrationsViewModels
{

    public class VtexProduct
    {

        public int? Id { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public int? DepartamentId { get; set; }

        public int? CategoryId { get; set; }

        public int? BrandId { get; set; }

        public string BrandName { get; set; }

        public string LinkId { get; set; }

        public string RefId { get; set; }

        public bool IsVisible { get; set; }

        public string Description { get; set; }

        public string DescriptionShort { get; set; }

        public DateTimeOffset ReleaseDate { get; set; }

        public string KeyWords { get; set; }

        public bool IsActive { get; set; }

        public string TaxCode { get; set; }

    }

}
using CoreService.IntegrationsViewModels;

namespace CoreService.Converters
{

    public static class VtexConverter
    {

        public static VtexProductModel ToProduct(this VtexProductModel product)
        {
            return new VtexProductModel{
                Name        = product.Name,
                Description = product.Description,
                IsActive = product.IsActive,
                Category = product.Category
                //CostPrice = price;
                //PictureUrl = pictureUrl;
            };
        }

        //public static List<VtexProductModel> ToProducts(this List<Address> productList)
        //{  return productList.ConvertAll(ToProduct).ToList(); }

    }

}
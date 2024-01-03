using Core.SeedWork;

namespace Core.Models.Core.Products
{
    public class ProductSpecificationType : Enumeration
    {
        public ProductSpecificationType(int id, string name) : base(id, name) { }

        public static readonly ProductSpecificationType Size = new ProductSpecificationType(1, nameof(Size));

        public static readonly ProductSpecificationType Color = new ProductSpecificationType(2, nameof(Color));

        public static readonly ProductSpecificationType Gender = new ProductSpecificationType(3, nameof(Gender));

        public static readonly ProductSpecificationType SimpleColor = new ProductSpecificationType(4, nameof(SimpleColor));

        public static readonly ProductSpecificationType Others = new ProductSpecificationType(50, nameof(Others));

        public static int FromString(string nameType)
        {
            int typeId = new int();
            switch (nameType)
            {
                case "SIZE":
                    typeId = Size.Id;
                    break;

                case "COLOR":
                    typeId = Color.Id;
                    break;

                case "GENDER":
                    typeId = Gender.Id;
                    break;

                case "SIMPLECOLOR":
                    typeId = SimpleColor.Id;
                    break;

                case "OTHERS":
                    typeId = Others.Id;
                    break;
            }
            return typeId;
        }
    }
}
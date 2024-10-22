using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.DtoLayer.CatalogDtos.ProductDetailDtos
{
    public class GetByIdProductDetailDto
    {
        public string ProductDetailId { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public string ProductInfo { get; set; }
    }
}

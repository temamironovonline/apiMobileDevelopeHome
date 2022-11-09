using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace apiMobileDevelope.Models
{
    public class WarehouseProduct
    {
        public WarehouseProduct(Warehouse warehouse)
        {
            productId = warehouse.Code_Product;
            productName = warehouse.Name_Product;
            productType = warehouse.Type_Product;
            productCount = warehouse.Count_Product;
            productPrice = warehouse.Price_Product;
            productPhoto = warehouse.Photo_Product;
        }

        public int productId { get; set; }
        public string productName { get; set; }
        public string productType { get; set; }
        public int productCount { get; set; }
        public int productPrice { get; set; }
        public byte[] productPhoto { get; set; }
    }
}
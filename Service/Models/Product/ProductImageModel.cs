using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
namespace Service.Models.Product
{
    public class ProductImageModel
    {
        public long ProductId { get; set; }

        public IFormFileCollection? ProductImage { get; set; }
    }
}

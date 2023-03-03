using Microsoft.AspNetCore.Http;
using OnSale.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public interface IImageHelper
    {
        Task<string> UploadImageAsync(IFormFile imageFile, string containerName);
        void DeleteImage(string pathImage);
        void DeleteListImage(ICollection<ProductImage> ListpathImage);
    }
}

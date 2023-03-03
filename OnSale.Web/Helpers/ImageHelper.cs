using Microsoft.AspNetCore.Http;
using OnSale.Common.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnSale.Web.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public void DeleteImage(string pathImage)
        {
            File.Delete($"{Directory.GetCurrentDirectory()}\\wwwroot{pathImage.Substring(1)}");
        }

        public void DeleteListImage(ICollection<ProductImage> ListpathImage)
        {
            foreach (var item in ListpathImage)
            {
                File.Delete($"{Directory.GetCurrentDirectory()}\\wwwroot{item.ImageUrl.Substring(1)}");
            }
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string containerName)
        {
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var directory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\images\\{containerName}";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"wwwroot\\images\\{containerName}",
                file);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"~/images/{containerName}/{file}";
        }
    }
}

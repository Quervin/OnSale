﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnSale.Common.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://onsalehidalgo.azurewebsites.net/images/noimage.png"
            : $"https://onsale.blob.core.windows.net/products/{ImageId}";
        
        [Display(Name = "Image")]
        public string ImageFullPathApi => string.IsNullOrEmpty(ImageUrl)
            ? $"https://onsalehidalgo.azurewebsites.net/images/noimage.png"
            : $"https://onsalehidalgo.azurewebsites.net{ImageUrl.Substring(1)}";


    }

}

﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.DtoLayer.CatalogDtos.BrandDtos
{
    public class CreateBrandDto
    {
        public string BrandName { get; set; }
        public IFormFile Image { get; set; }
    }
}

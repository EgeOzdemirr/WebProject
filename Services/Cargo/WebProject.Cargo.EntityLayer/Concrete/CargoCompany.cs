﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProject.Cargo.EntityLayer.Concrete
{
    public class CargoCompany
    {
        public int CargoCompanyId { get; set; }
        public string CargoCompanyName { get; set; }
        public List<CargoDetail> CargoDetails { get; set; }
    }
}

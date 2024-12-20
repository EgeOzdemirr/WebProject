﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Cargo.DataAccessLayer.Abstract;
using WebProject.Cargo.DataAccessLayer.Concrete;
using WebProject.Cargo.DataAccessLayer.Repositories;
using WebProject.Cargo.EntityLayer.Concrete;

namespace WebProject.Cargo.DataAccessLayer.EntityFramework
{
    public class EfCargoDetailDal:GenericRepository<CargoDetail>, ICargoDetailDal
    {
        public EfCargoDetailDal(CargoContext context) : base(context)
        {
            
        }
    }
}

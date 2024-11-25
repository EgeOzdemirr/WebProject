﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebProject.Cargo.BusinessLayer.Abstract;
using WebProject.Cargo.DataAccessLayer.Abstract;
using WebProject.Cargo.EntityLayer.Concrete;

namespace WebProject.Cargo.BusinessLayer.Concrete
{
    public class CargoCustomerManager : ICargoCustomerService
    {
        private readonly ICargoCustomerDal _cargoCustomerDal;
        public CargoCustomerManager(ICargoCustomerDal cargoCustomerDal)
        {
            _cargoCustomerDal = cargoCustomerDal;
        }
        public void TDelete(int id)
        {
            _cargoCustomerDal.Delete(id);
        }
        public List<CargoCustomer> TGetAll()
        {
            return _cargoCustomerDal.GetAll();
        }
        public CargoCustomer TGetById(int id)
        {
            return _cargoCustomerDal.GetById(id);
        }

        public CargoCustomer TGetCargoCustomerById(string id)
        {
            return _cargoCustomerDal.GetCargoCustomerById(id);
        }

        public void TInsert(CargoCustomer entity)
        {
            _cargoCustomerDal.Insert(entity);
        }
        public void TUpdate(CargoCustomer entity)
        {
            _cargoCustomerDal.Update(entity);
        }
    }
}

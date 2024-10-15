using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Cargo.BusinessLayer.Abstract;
using WebProject.Cargo.DataAccessLayer.Abstract;
using WebProject.Cargo.EntityLayer.Concrete;

namespace WebProject.Cargo.BusinessLayer.Concrete
{
    public class CargoOperationManager : ICargoOperationService
    {
        private readonly ICargoOperationDal _cargoOperationDal;
        public CargoOperationManager(ICargoOperationDal cargoOperationDal)
        {
            _cargoOperationDal = cargoOperationDal;
        }
        public void TDelete(int id)
        {
            _cargoOperationDal.Delete(id);
        }
        public List<CargoOperation> TGetAll()
        {
            return _cargoOperationDal.GetAll();
        }
        public CargoOperation TGetById(int id)
        {
            return _cargoOperationDal.GetById(id);
        }
        public void TInsert(CargoOperation entity)
        {
            _cargoOperationDal.Insert(entity);
        }
        public void TUpdate(CargoOperation entity)
        {
            _cargoOperationDal.Update(entity);
        }
    }
}

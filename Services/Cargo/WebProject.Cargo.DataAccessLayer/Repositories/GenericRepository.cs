using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Cargo.DataAccessLayer.Abstract;
using WebProject.Cargo.DataAccessLayer.Concrete;

namespace WebProject.Cargo.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly CargoContext _cargoContext;

        public GenericRepository(CargoContext cargoContext)
        {
            _cargoContext = cargoContext;
        }

        public void Delete(int id)
        {
            var values = _cargoContext.Set<T>().Find(id);
            _cargoContext.Set<T>().Remove(values);
            _cargoContext.SaveChanges();
        }

        public List<T> GetAll()
        {
            var values = _cargoContext.Set<T>().ToList();
            return values;
        }

        public T GetById(int id)
        {
            var value = _cargoContext.Set<T>().Find(id);
            return value;
        }

        public void Insert(T entity)
        {
            _cargoContext.Set<T>().Add(entity);
            _cargoContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _cargoContext.Set<T>().Update(entity);
            _cargoContext.SaveChanges();
        }
    }
}

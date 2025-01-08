using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebProject.Cargo.BusinessLayer.Abstract;
using WebProject.Cargo.DtoLayer.Dtos.CargoCustomerDtos;
using WebProject.Cargo.EntityLayer.Concrete;

namespace WebProject.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoCustomersController : ControllerBase
    {
        private readonly ICargoCustomerService _cargoCustomerService;

        public CargoCustomersController(ICargoCustomerService customerService)
        {
            _cargoCustomerService = customerService;
        }
        [HttpGet]
        public IActionResult GetCargoCustomerList()
        {
            var values = _cargoCustomerService.TGetAll();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateCargoCustomer(CreateCargoCustomerDto createCargoCustomerDto)
        {
            CargoCustomer cargoCustomer = new CargoCustomer()
            {
                Name = createCargoCustomerDto.Name,
                SurName = createCargoCustomerDto.Surname,
                Email = createCargoCustomerDto.Email,
                Phone = createCargoCustomerDto.Phone,
                City = createCargoCustomerDto.City,
                District = createCargoCustomerDto.District,
                Address = createCargoCustomerDto.Address,
                UserCustomerId = createCargoCustomerDto.UserCustomerId
            };
            _cargoCustomerService.TInsert(cargoCustomer);
            return Ok("Kargo Müşterisi Başarıyla Oluşturuldu");
        }
        [HttpDelete("RemoveCargoCustomer/{id}")]
        public IActionResult RemoveCargoCustomer(int id)
        {
            _cargoCustomerService.TDelete(id);
            return Ok("Kargo Müşterisi Başarıyla Silindi");
        }
        [HttpGet("GetCargoCustomerById/{id}")]
        public IActionResult GetCargoCustomerById(int id)
        {
            var value = _cargoCustomerService.TGetById(id);
            return Ok(value);
        }
        [HttpPut]
        public IActionResult UpdateCargoCustomer(UpdateCargoCustomerDto updateCargoCustomerDto)
        {
            CargoCustomer cargoCustomer = new CargoCustomer()
            {
                CargoCustomerId = updateCargoCustomerDto.CargoCustomerId,
                Name = updateCargoCustomerDto.Name,
                SurName = updateCargoCustomerDto.Surname,
                Email = updateCargoCustomerDto.Email,
                Phone = updateCargoCustomerDto.Phone,
                City = updateCargoCustomerDto.City,
                District = updateCargoCustomerDto.District,
                Address = updateCargoCustomerDto.Address,
                UserCustomerId = updateCargoCustomerDto.UserCustomerId
            };
            _cargoCustomerService.TUpdate(cargoCustomer);
            return Ok("Kargo Müşterisi Başarıyla Güncellendi");
        }
        [HttpGet("GetCargoCustomerByUserId/{id}")]
        public IActionResult GetCargoCustomerByUserId(string id)
        {
            var values = _cargoCustomerService.TGetCargoCustomerById(id);
            return Ok(values);
        }
    }
}

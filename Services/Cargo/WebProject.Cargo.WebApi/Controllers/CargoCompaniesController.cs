﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebProject.Cargo.BusinessLayer.Abstract;
using WebProject.Cargo.DtoLayer.Dtos.CargoCompanyDtos;
using WebProject.Cargo.EntityLayer.Concrete;

namespace WebProject.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoCompaniesController : ControllerBase
    {
        private readonly ICargoCompanyService _cargoCompanyService;
        public CargoCompaniesController(ICargoCompanyService cargoCompanyService)
        {
            _cargoCompanyService = cargoCompanyService;
        }
        [HttpGet]
        public IActionResult GetCargoCompanyList()
        {
            var values = _cargoCompanyService.TGetAll();
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateCargoCompany(CreateCargoCompanyDto createCargoCompanyDto)
        {
            CargoCompany cargoCompany = new CargoCompany()
            {
                CargoCompanyName = createCargoCompanyDto.CargoCompanyName,
            };
            _cargoCompanyService.TInsert(cargoCompany);
            return Ok("Kargo Şirketi Başarıyla Oluşturuldu");
        }
        [HttpDelete("RemoveCargoCompany/{id}")]
        public IActionResult RemoveCargoCompany(int id)
        {
            _cargoCompanyService.TDelete(id);
            return Ok("Kargo Şirketi Başarıyla Silindi");
        }
        [HttpGet("GetCargoCompanyById/{id}")]
        public IActionResult GetCargoCompanyById(int id)
        {
            var value = _cargoCompanyService.TGetById(id);
            return Ok(value);
        }
        [HttpPut]
        public IActionResult UpdateCargoCompany(UpdateCargoCompanyDto updateCargoCompanyDto)
        {
            CargoCompany cargoCompany = new CargoCompany()
            {
                CargoCompanyId = updateCargoCompanyDto.CargoCompanyId,
                CargoCompanyName = updateCargoCompanyDto.CargoCompanyName
            };
            _cargoCompanyService.TUpdate(cargoCompany);
            return Ok("Kargo Şirketi Başarıyla Güncellendi");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Order.Application.Features.CQRS.Commands.AddressCommands;
using WebProject.Order.Application.Interfaces;
using WebProject.Order.Domain.Entities;

namespace WebProject.Order.Application.Features.CQRS.Handlers.AddressHandlers
{
    public class CreateAddressCommandHandler
    {
        private readonly IRepository<Address> _repository;

        public CreateAddressCommandHandler(IRepository<Address> repository)
        {
            _repository = repository;
        }
        public async Task Handle(CreateAddressCommand createAddressCommand)
        {
            await _repository.CreateAsync(new Address
            {
                UserId = createAddressCommand.UserId,
                Name = createAddressCommand.Name,
                Surname = createAddressCommand.Surname,
                Email = createAddressCommand.Email,
                Phone = createAddressCommand.Phone,
                AddressLine1 = createAddressCommand.AddressLine1,
                AddressLine2 = createAddressCommand.AddressLine2,
                Country = createAddressCommand.Country,
                City = createAddressCommand.City,
                District = createAddressCommand.District,
                ZipCode = createAddressCommand.ZipCode
            });
        }
    }
}

using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Order.Application.Features.Mediator.Commands.OrderingCommands;
using WebProject.Order.Application.Interfaces;
using WebProject.Order.Domain.Entities;

namespace WebProject.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class CreateOrderingCommandHandler : IRequestHandler<CreateOrderingCommand>
    {
        private readonly IRepository<Ordering> _repository;

        public CreateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task Handle(CreateOrderingCommand request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new Ordering
            {
                UserId = request.UserId,
                TotalPrice = request.TotalPrice,
                OrderDate = request.OrderDate,
            });
        }
    }
}

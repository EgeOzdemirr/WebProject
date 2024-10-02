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
    public class UpdateOrderingCommandHandler : IRequestHandler<UpdateOrderingCommand>
    {
        private readonly IRepository<Ordering> _repository;
        public UpdateOrderingCommandHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateOrderingCommand request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetByIdAsync(request.OrderingId);
            values.UserId = request.UserId;
            values.OrderDate = request.OrderDate;
            values.TotalPrice = request.TotalPrice;
            await _repository.UpdateAsync(values);
        }
    }
}

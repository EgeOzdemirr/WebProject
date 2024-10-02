using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Order.Application.Features.Mediator.Queries.OrderingQueries;
using WebProject.Order.Application.Features.Mediator.Results.OrderingResults;
using WebProject.Order.Application.Interfaces;
using WebProject.Order.Domain.Entities;

namespace WebProject.Order.Application.Features.Mediator.Handlers.OrderingHandlers
{
    public class GetOrderingQueryHandler : IRequestHandler<GetOrderingQuery, List<GetOrderingQueryResult>>
    {
        private readonly IRepository<Ordering> _repository;

        public GetOrderingQueryHandler(IRepository<Ordering> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetOrderingQueryResult>> Handle(GetOrderingQuery request, CancellationToken cancellationToken)
        {
            var values = await _repository.GetAllAsync();
            return values.Select(x => new GetOrderingQueryResult
            {
                OrderingId = x.OrderingId,
                UserId = x.UserId,
                TotalPrice = x.TotalPrice,
                OrderDate = x.OrderDate,
            }).ToList();
        }
    }
}

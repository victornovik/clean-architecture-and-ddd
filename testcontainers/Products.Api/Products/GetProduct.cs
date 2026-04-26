using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Products.Api.Entities;

namespace Products.Api.Products;

public class GetProduct
{
    public sealed record Query : IRequest<ProductResponse>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Query, ProductResponse>
    {
        public async Task<ProductResponse> Handle(Query request, CancellationToken ct)
        {
            var product = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, ct);
            if (product is null)
            {
                throw new ApplicationException("Product not found");
            }

            return product.Adapt<ProductResponse>();
        }
    }
}

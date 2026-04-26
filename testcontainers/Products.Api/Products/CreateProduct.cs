using MediatR;
using Products.Api.Entities;

namespace Products.Api.Products;

public class CreateProduct
{
    public sealed class Command : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Command, Guid>
    {
        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Category = request.Category,
                Price = request.Price,
                CreatedOnUtc = DateTime.UtcNow
            };

            dbContext.Add(product);

            await dbContext.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}

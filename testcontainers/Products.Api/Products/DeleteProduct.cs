using MediatR;

namespace Products.Api.Products;

public class DeleteProduct
{
    public sealed record Command : IRequest
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler(ApplicationDbContext dbContext) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var product = dbContext.Products.FirstOrDefault(p => p.Id == request.Id);

            if (product is null)
            {
                throw new ApplicationException("Product not found");
            }

            dbContext.Remove(product);

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}

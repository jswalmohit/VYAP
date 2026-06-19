namespace ShopManagementSystem.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;
using ShopManagementSystem.Infrastructure.Persistence;

public class SaleRepository : ISaleRepository
{
    private readonly AppDbContext _context;

    public SaleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Sale?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetByProductIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AsNoTracking().Where(x => x.ProductId == productId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AsNoTracking().Where(x => x.CustomerId == customerId).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Sale>> GetByInvoiceNoAsync(string invoiceNo, CancellationToken cancellationToken = default)
    {
        return await _context.Sales.AsNoTracking().Where(x => x.InvoiceNo == invoiceNo).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        _context.Sales.Update(sale);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var sale = await _context.Sales.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (sale is not null)
        {
            _context.Sales.Remove(sale);
        }
    }
}

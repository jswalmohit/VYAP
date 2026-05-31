using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Domain.Entities;
using ShopManagementSystem.Domain.Interfaces;
using ShopManagementSystem.Infrastructure.Persistence;

namespace ShopManagementSystem.Infrastructure.Repositories;

public class BillRepository : Repository<Bill>, IBillRepository
{
    public BillRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Bill>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(b => b.Customer)
            .Include(b => b.BillItems)
                .ThenInclude(bi => bi.Product)
            .Where(b => b.CustomerId == customerId)
            .OrderByDescending(b => b.BillDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Bill?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(b => b.Customer)
            .Include(b => b.BillItems)
                .ThenInclude(bi => bi.Product)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<string> GenerateBillNumberAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow;
        var prefix = $"BILL-{today:yyyyMMdd}";

        var lastBillNumber = await DbSet
            .Where(b => b.BillNumber.StartsWith(prefix))
            .OrderByDescending(b => b.BillNumber)
            .Select(b => b.BillNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var sequence = 1;
        if (!string.IsNullOrEmpty(lastBillNumber))
        {
            var lastSequencePart = lastBillNumber.Split('-').LastOrDefault();
            if (int.TryParse(lastSequencePart, out var lastSequence))
            {
                sequence = lastSequence + 1;
            }
        }

        return $"{prefix}-{sequence:D4}";
    }

    public override async Task<IReadOnlyList<Bill>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(b => b.Customer)
            .Include(b => b.BillItems)
                .ThenInclude(bi => bi.Product)
            .OrderByDescending(b => b.BillDate)
            .ToListAsync(cancellationToken);
    }
}

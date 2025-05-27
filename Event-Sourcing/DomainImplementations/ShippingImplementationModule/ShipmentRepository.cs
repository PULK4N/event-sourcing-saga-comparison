using Microsoft.EntityFrameworkCore;
using ShippingModule;

namespace ShippingImplementationModule;

public class ShipmentRepository
{
    private readonly ApplicationDbContext _context;

    public ShipmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task WriteDetails(ShipmentStateData item)
    {
        var items = _context.Shipments.Where(x => x.Id == item.Id);
        _context.RemoveRange(items);
        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task<ShipmentStateData> ReadDetails(Guid id)
    {
        return await _context.Shipments.FirstOrDefaultAsync(x => x.Id == id);
    }
}

using InventoryModule;
using Microsoft.EntityFrameworkCore;

namespace InventoryImplementationModule;

public class InventoryRepository
{
    private readonly ApplicationDbContext _context;

    public InventoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task WriteDetails(InventoryStateData item)
    {
        var items = _context.Inventories.Where(x => x.Id == item.Id);
        _context.RemoveRange(items);
        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task<InventoryStateData> ReadDetails(Guid id)
    {
        return await _context.Inventories.FirstOrDefaultAsync(x => x.Id == id);
    }
}

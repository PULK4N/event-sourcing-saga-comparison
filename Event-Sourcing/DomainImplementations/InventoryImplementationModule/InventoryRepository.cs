using InventoryImplementationModule.Models;
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

    public async Task WriteDetails(Inventory item)
    {
        var items = _context.Inventories.Where(x => x.Id == item.Id);
        _context.RemoveRange(items);
        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task<Inventory> ReadDetails(Guid id)
    {
        return await _context.Inventories.FirstOrDefaultAsync(x => x.Id == id);
    }
}

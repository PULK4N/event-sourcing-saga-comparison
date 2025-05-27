using Microsoft.EntityFrameworkCore;
using OrderModule.Models;

namespace OrderImplementationModule;

public class OrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task WriteDetails(Order item)
    {
        var items = _context.Orders.Where(x => x.Id == item.Id);
        _context.RemoveRange(items);
        await _context.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task<Order> ReadDetails(Guid id)
    {
        return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
    }
}

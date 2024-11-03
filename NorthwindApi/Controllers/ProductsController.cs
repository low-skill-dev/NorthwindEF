using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using NorthwindApi.DbContexts;
using NorthwindModels;

namespace NorthwindApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ProductsController : ControllerBase
{
	// pre-compiled query
	// stored procedure
	// split query

	private readonly NorthwindContext _context;
	public ProductsController(NorthwindContext context)
	{
		_context = context;
	}

	private static readonly Func<NorthwindContext, string, IAsyncEnumerable<Supplier>>
		GetSuppliersAndProductsByFirstSupplierName =
			EF.CompileAsyncQuery((NorthwindContext ctx, string s) =>
				ctx.Suppliers.Where(x => x.ContactName.Contains(s)).Take(100)
				.Include(x => x.Products).AsSplitQuery());

	[HttpGet]
	public async Task<IActionResult> GetProducts()
	{
		//var req = _context.Orders.AsSplitQuery().OrderBy(x => x.CustomerId).Join(
		//	_context.Customers, x => x.CustomerId, x => x.CustomerId, (o, c) =>
		//	new { o.OrderId, o.OrderDate, c.CustomerId, c.ContactName });

		//var req = _context.Orders.Include(x=> x.Customer).AsSplitQuery().Select(x=> 
		//	new { x.OrderId, x.OrderDate, x.Customer.CustomerId, x.Customer.ContactName })
		//	.OrderBy(x=> x.CustomerId);

		var req = _context.Suppliers.Include(x => x.Products).AsSplitQuery();

		var res = await req.ToListAsync();

		return Ok(res);
	}

	[HttpGet]
	public async Task<IActionResult> GetProductsPreComp()
	{
		//var req = _context.Orders.AsSplitQuery().OrderBy(x => x.CustomerId).Join(
		//	_context.Customers, x => x.CustomerId, x => x.CustomerId, (o, c) =>
		//	new { o.OrderId, o.OrderDate, c.CustomerId, c.ContactName });

		//var req = _context.Orders.Include(x=> x.Customer).AsSplitQuery().Select(x=> 
		//	new { x.OrderId, x.OrderDate, x.Customer.CustomerId, x.Customer.ContactName })
		//	.OrderBy(x=> x.CustomerId);

		//var req = _context.Suppliers.Include(x => x.Products).AsSplitQuery();

		//var res = await req.ToListAsync();

		return Ok(GetSuppliersAndProductsByFirstSupplierName(_context, "A").ToBlockingEnumerable().ToList());
	}
}

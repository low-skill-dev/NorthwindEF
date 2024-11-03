using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

	[HttpGet]
	public async Task<IActionResult> GetProducts()
	{
		//var req = _context.Orders.AsSplitQuery().OrderBy(x => x.CustomerId).Join(
		//	_context.Customers, x => x.CustomerId, x => x.CustomerId, (o, c) =>
		//	new { o.OrderId, o.OrderDate, c.CustomerId, c.ContactName });

		//var req = _context.Orders.Include(x=> x.Customer).AsSplitQuery().Select(x=> 
		//	new { x.OrderId, x.OrderDate, x.Customer.CustomerId, x.Customer.ContactName })
		//	.OrderBy(x=> x.CustomerId);

		var req = _context.Suppliers.Include(x => x.Products).AsSplitQuery().Select(x =>
			new { x.SupplierId, x.ContactName, x.Products });

		var res = await req.ToListAsync();

		return Ok(res);
	}
}

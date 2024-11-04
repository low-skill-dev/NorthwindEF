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
	// split query
	// pre-compiled query
	// stored procedure

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
	public async Task<IActionResult> Methdod1()
	{
		var req = _context.Suppliers.Include(x => x.Products).AsSplitQuery();

		var res = await req.ToListAsync();

		return Ok(res);
	}

	[HttpGet]
	public async Task<IActionResult> Methdod2()
	{
		return Ok(GetSuppliersAndProductsByFirstSupplierName(_context, "A"));
	}

	[HttpGet]
	public async Task<IActionResult> Methdod3()
	{
		return Ok(await _context.Set<Test>().FromSqlRaw("SELECT * FROM GetSuppByParams(20, 27)").ToListAsync());
	}
}

class Test
{
	public int s_id { get; set; }
	public string s_name { get; set; }
}
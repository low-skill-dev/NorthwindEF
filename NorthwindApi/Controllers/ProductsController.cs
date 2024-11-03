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
	public async Task<List<Product>> GetProducts()
	{
		return await _context.Products.ToListAsync();
	}
}

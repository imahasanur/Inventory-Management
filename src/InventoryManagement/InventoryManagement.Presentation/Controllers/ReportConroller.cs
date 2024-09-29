using Autofac;
using Mapster;
using InventoryManagement.Presentation.Models;
using InventoryManagement.Service.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.Linq.Dynamic.Core;
using System.Text;
using Humanizer;
using InventoryManagement.Presentation.Others;
using System.Drawing;
using InventoryManagement.Data.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace InventoryManagement.Presentation.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<ReportController> _logger;
        private readonly LinkGenerator _linkGenerator;
		public ReportController(ILifetimeScope scope,
            ILogger<ReportController> logger,
            LinkGenerator linkGenerator)
        {
            _scope = scope;
            _logger = logger;
            _linkGenerator = linkGenerator;
        }


		public async Task<IActionResult> GetInventory()
		{
			var url = _linkGenerator.GetUriByAction(HttpContext, controller: "Report", action: "GetInventory");
			if (url is null)
			{
				return View();
			}
			var viewModel = new ReportModel { EndpointUrl = url };
			return View(viewModel);
		}


		[HttpPost]
		public async Task<IActionResult> GetInventory([FromBody] TabulatorQueryDto dto)
		{
			var model = new ProductsModel();
			model.Resolve(_scope);
			var allProductDetails = await model.GetProductsAsync(dto.Page, dto.Size);

			var dataSet = allProductDetails;
			var queryable = dataSet.data.AsQueryable();
			var count = 0;

			IQueryable<ProductsDto>? filteredData = null;

			if (dto.Filters.Count > 0)
			{
				var expression = ExpressionMaker(
					new List<string> { "id", "photoUrl", "name", "categoryName", "categoryId", "unitPrice", "stockLevel", "createdAtUtc" },
					new List<string>(),
					dto.Filters
				);
				filteredData = queryable.Where(expression);
				count = allProductDetails.total;
			}

			if (dto.Sorters.Count > 0)
			{
				var elem = dto.Sorters[0];
				var expression = $"x => {elem.Field.Pascalize()} {elem.Dir.ToUpper()}";

				if (filteredData is null)
				{
					filteredData = queryable.OrderBy(expression);
					count = allProductDetails.total;
				}
				else
				{
					filteredData = filteredData.OrderBy(expression);
				}
			}

			if (filteredData is null)
			{
				filteredData = queryable;
				count = allProductDetails.total;
			}

			var totalPages = (int)Math.Ceiling(count / (decimal)dto.Size);
			return Ok(new { data = filteredData, last_row = count, last_page = totalPages });

		}

        public async Task<IActionResult> GetSales()
        {
            var url = _linkGenerator.GetUriByAction(HttpContext, controller: "Report", action: "GetSales");
            if (url is null)
            {
                return View();
            }
            var viewModel = new ReportModel { EndpointUrl = url };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetSales([FromBody] TabulatorQueryDto dto)
        {
            var model = new TransactionsModel();
            model.Resolve(_scope);
            var allTransactionDetails = await model.GetAllTransactionAsync();
            var filterTransaction = new List<TransactionsDto>();
            foreach(var transaction in allTransactionDetails)
            {
                if (transaction.TransactionType == "sale")
                    filterTransaction.Add(transaction);
            }

            var dataSet = filterTransaction;
            var queryable = dataSet.AsQueryable();
            var count = 0;

            IQueryable<TransactionsDto>? filteredData = null;

            if (dto.Filters.Count > 0)
            {
                var expression = ExpressionMaker(
                    new List<string> { "id", "productId", "saleOrderId", "quantity", "totalAmount", "transactionType", "createdAtUtc" },
                    new List<string>(),
                    dto.Filters
                );
                filteredData = queryable.Where(expression);
                count = filteredData.Count();
            }

            if (dto.Sorters.Count > 0)
            {
                var elem = dto.Sorters[0];
                var expression = $"x => {elem.Field.Pascalize()} {elem.Dir.ToUpper()}";

                if (filteredData is null)
                {
                    filteredData = queryable.OrderBy(expression);
                    count = queryable.Count();
                }
                else
                {
                    filteredData = filteredData.OrderBy(expression);
                }
            }

            if (filteredData is null)
            {
                filteredData = queryable.Paginate(dto.Page, dto.Size);
                count = queryable.Count();
            }
            else
            {
                filteredData = filteredData.Paginate(dto.Page, dto.Size);
            }

            var totalPages = (int)Math.Ceiling(count / (decimal)dto.Size);

            return Ok(new { data = filteredData, last_row = count, last_page = totalPages });

        }

		public async Task<IActionResult> GetPurchases()
		{
			var url = _linkGenerator.GetUriByAction(HttpContext, controller: "Report", action: "GetPurchases");
			if (url is null)
			{
				return View();
			}
			var viewModel = new ReportModel { EndpointUrl = url };
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> GetPurchases([FromBody] TabulatorQueryDto dto)
		{
			var model = new TransactionsModel();
			model.Resolve(_scope);
			var allTransactionDetails = await model.GetAllTransactionAsync();
			var filterTransaction = new List<TransactionsDto>();
			foreach (var transaction in allTransactionDetails)
			{
				if (transaction.TransactionType == "purchase")
					filterTransaction.Add(transaction);
			}

			var dataSet = filterTransaction;
			var queryable = dataSet.AsQueryable();
			var count = 0;

			IQueryable<TransactionsDto>? filteredData = null;

			if (dto.Filters.Count > 0)
			{
				var expression = ExpressionMaker(
					new List<string> { "id", "productId", "purchaseOrderId", "quantity", "totalAmount", "transactionType", "createdAtUtc" },
					new List<string>(),
					dto.Filters
				);
				filteredData = queryable.Where(expression);
				count = filteredData.Count();
			}

			if (dto.Sorters.Count > 0)
			{
				var elem = dto.Sorters[0];
				var expression = $"x => {elem.Field.Pascalize()} {elem.Dir.ToUpper()}";

				if (filteredData is null)
				{
					filteredData = queryable.OrderBy(expression);
					count = queryable.Count();
				}
				else
				{
					filteredData = filteredData.OrderBy(expression);
				}
			}

			if (filteredData is null)
			{
				filteredData = queryable.Paginate(dto.Page, dto.Size);
				count = queryable.Count();
			}
			else
			{
				filteredData = filteredData.Paginate(dto.Page, dto.Size);
			}

			var totalPages = (int)Math.Ceiling(count / (decimal)dto.Size);

			return Ok(new { data = filteredData, last_row = count, last_page = totalPages });

		}
		private static string ExpressionMaker(IList<string> allowedColumns, IList<string> enumColumns, IList<TabulatorFilterDto> filters)
        {
            var expression = new StringBuilder();
            expression.Append("x => ");

            for (var i = 0; i < filters.Count; i++)
            {
                var filter = filters[i];
                var fieldInPascalCase = filter.Field.Pascalize();

                if (allowedColumns.Contains(filter.Field) is false)
                {
                    continue;
                }

                if (enumColumns.Contains(filter.Field))
                {
                    expression.Append($"{fieldInPascalCase} = {fieldInPascalCase}.{filter.Value.Pascalize()}");
                }
                else if (filter.Type == FilterHelper.Like)
                {
                    if (fieldInPascalCase == "Id" || fieldInPascalCase == "PurchaseOrderId" || fieldInPascalCase == "SaleOrderId" || fieldInPascalCase == "ProductId" || fieldInPascalCase == "CategoryId")
                    {
                        expression.Append(
                            $"""{fieldInPascalCase}.ToString().Contains("{filter.Value}", StringComparison.InvariantCultureIgnoreCase)"""
                        );
                    }
                    else
                    {
                        expression.Append(
                            $"""{fieldInPascalCase}.Contains("{filter.Value}", StringComparison.InvariantCultureIgnoreCase)"""
                        );
                    }
                }
                else if (fieldInPascalCase == "CreatedAtUtc")
                {
                    var dateTimeValue = DateTime.ParseExact(filter.Value, "dd/MM/yyyy", null);
                    expression.Append($"{fieldInPascalCase} {filter.Type} DateTime({dateTimeValue.Year}, {dateTimeValue.Month}, {dateTimeValue.Day})");
                }
                else
                {
                    expression.Append($"{fieldInPascalCase} {filter.Type} {filter.Value}");
                }

                if (i + 1 != filters.Count)
                {
                    expression.Append(" && ");
                }
            }

            return expression.ToString();
        }


	}
}
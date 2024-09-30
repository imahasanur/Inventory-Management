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
using Microsoft.AspNetCore.Authorization;
using InventoryManagement.Data.Membership;
using Microsoft.AspNetCore.Identity;
using InventoryManagement.Data.Extensions;

namespace InventoryManagement.Presentation.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<TransactionController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly string _imageDirectory = "wwwroot/uploads";
		public TransactionController(ILifetimeScope scope,
            ILogger<TransactionController> logger,
            SignInManager<ApplicationUser> signInManager,
            LinkGenerator linkGenerator)
        {
            _scope = scope;
            _logger = logger;
            _signInManager = signInManager;
            _linkGenerator = linkGenerator;
        }

        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Get()
		{
			var url = _linkGenerator.GetUriByAction(HttpContext, controller: "Transaction", action: "Get");
			if (url is null)
			{
				return View();
			}
			var viewModel = new TransactionsModel { EndpointUrl = url };
			return View(viewModel);
		}

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
		public async Task<IActionResult> Get([FromBody] TabulatorQueryDto dto)
		{
			var model = new TransactionsModel();
			model.Resolve(_scope);
			var allTransactionDetails = await model.GetAllTransactionAsync();
            var filterTransactionDetails = new List<TransactionsDto>();
            var email = _signInManager.Context.User.Identity?.Name;
            if (email != "admin@gmail.com")
            {
                foreach (var transaction in allTransactionDetails)
                {
                    if (transaction.User == email)
                    {
                        filterTransactionDetails.Add(transaction);
                    }
                }
            }
            else
            {
                filterTransactionDetails = (List<TransactionsDto>)allTransactionDetails;
            }

            var dataSet = filterTransactionDetails;
			var queryable = dataSet.AsQueryable();
			var count = 0;

			IQueryable<TransactionsDto>? filteredData = null;

			if (dto.Filters.Count > 0)
			{
				var expression = ExpressionMaker(
					new List<string> { "id","productId", "purchaseOrderId", "saleOrderId", "quantity","totalAmount","transactionType","createdAtUtc","id" },
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
					if (fieldInPascalCase == "Id" || fieldInPascalCase == "PurchaseOrderId" || fieldInPascalCase == "SaleOrderId" || fieldInPascalCase == "ProductId")
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

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var model = new DeleteTransactionModel();
				model.Resolve(_scope);
				await model.DeleteByIdAsync(id);
				return Json(new { success = true, message = "Transaction deleted successfully" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error deleting transaction record: " + ex.Message });
			}
			return RedirectToAction("Get");
		}
	}
}
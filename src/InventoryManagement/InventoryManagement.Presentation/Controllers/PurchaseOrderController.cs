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
    public class PurchaseOrderController : Controller
    {
        private readonly ILifetimeScope _scope;
        private readonly ILogger<PurchaseOrderController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private SignInManager<ApplicationUser> _signInManager;
        public PurchaseOrderController(ILifetimeScope scope,
            ILogger<PurchaseOrderController> logger,
            SignInManager<ApplicationUser> signInManager,
            LinkGenerator linkGenerator)
        {
            _scope = scope;
            _logger = logger;
            _signInManager = signInManager;
            _linkGenerator = linkGenerator;
        }

        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Create()
        {
            var model = new CreatePurchaseOrderModel();
			var suppliersModel = new SuppliersModel();
            var productsModel = new ProductsModel();

            suppliersModel.Resolve(_scope);
			var suppliers = await suppliersModel.GetAllSupplierAsync();
			productsModel.Resolve(_scope);
			var products = await productsModel.GetAllProductAsync();
            model.Resolve(_scope);
			model.Products = products;
			model.Suppliers = suppliers;
			return View(model);
        }

        [Authorize(Policy = "UserPolicy")]
        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreatePurchaseOrderModel model)
		{
			if (!ModelState.IsValid)
			{
                _logger.LogInformation("Model state is not valid");
				return View(model);
			}

            model.CreatedAtUtc = DateTime.UtcNow;
			model.Status = "pending";
			model.TotalAmount = model.UnitPrice * model.Quantity;
			model.User = _signInManager.Context.User.Identity?.Name;
            var orderDto = await model.BuildAdapter().AdaptToTypeAsync<CreatePurchaseOrderDto>();
			model.Resolve(_scope);
			await model.CreatePurchaseOrderAsync(orderDto);

			return RedirectToAction("Create");
		}

        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> Get()
		{
			var url = _linkGenerator.GetUriByAction(HttpContext, controller: "PurchaseOrder", action: "Get");
			if (url is null)
			{
				return View();
			}
			var viewModel = new PurchaseOrdersModel { EndpointUrl = url };
			return View(viewModel);
		}

        [Authorize(Policy = "UserPolicy")]
        [HttpPost]
		public async Task<IActionResult> Get([FromBody] TabulatorQueryDto dto)
		{
			var model = new PurchaseOrdersModel();
			model.Resolve(_scope);
			var allPurchaseDetails = await model.GetAllPurchaseOrderAsync();
            var filterPurchaseDetails = new List<PurchaseOrdersDto>();
			var email = _signInManager.Context.User.Identity?.Name;
			if(email != "admin@gmail.com")
			{
                foreach (var purchase in allPurchaseDetails)
                {
                    if(purchase.User == email)
					{
						filterPurchaseDetails.Add(purchase);
					}
				}
            }
			else
			{
				filterPurchaseDetails = (List<PurchaseOrdersDto>)allPurchaseDetails;
			}
            

            var dataSet = filterPurchaseDetails;
			var queryable = dataSet.AsQueryable();
			var count = 0;

			IQueryable<PurchaseOrdersDto>? filteredData = null;

			if (dto.Filters.Count > 0)
			{
				var expression = ExpressionMaker(
					new List<string> { "id","supplierId", "supplierName", "productId", "productName", "quantity", "unitPrice","totalAmount","status", "createdAtUtc", "id" },
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
					if (fieldInPascalCase == "Id" || fieldInPascalCase == "ProductId" || fieldInPascalCase == "SupplierId")
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
        public async Task<IActionResult> Edit(Guid id)
		{
			if (id == Guid.Empty)
			{
				id = (Guid)TempData["Id"];
			}
			else
			{
				TempData["Id"] = id;
			}
			var model = new EditPurchaseOrderModel();
			model.Resolve(_scope);
			model = await model.GetPurchaseOrderByIdAsync(id);

			TempData["CreatedAtUtc"] = model.CreatedAtUtc;
			var suppliersModel = new SuppliersModel();
			var productsModel = new ProductsModel();

			suppliersModel.Resolve(_scope);
			var suppliers = await suppliersModel.GetAllSupplierAsync();
			productsModel.Resolve(_scope);
			var products = await productsModel.GetAllProductAsync();
			model.Products = products;
			model.Suppliers = suppliers;
			TempData["Status"] = model.Status;
			TempData["User"] = model.User;
			return View(model);
		}

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost, ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(EditPurchaseOrderModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			if (TempData.ContainsKey("CreatedAtUtc"))
			{
				model.CreatedAtUtc = (DateTime)TempData["CreatedAtUtc"];
			}

			model.UpdatedAtUtc = DateTime.UtcNow;
			model.TotalAmount = model.Quantity * model.UnitPrice;
			model.User = (string?)TempData["User"];
			var response = await model.BuildAdapter().AdaptToTypeAsync<EditPurchaseOrderDto>();
			model.Resolve(_scope);
			await model.EditPurchaseOrderAsync(response);

			var tempStatus = TempData["Status"]?.ToString()?.Trim();
			var modelStatus = model.Status?.Trim();

			//updating product table stock level and Transaction table
			if (!string.Equals(tempStatus, modelStatus, StringComparison.OrdinalIgnoreCase) && modelStatus == "approved")
			{
				var productModel = new EditProductModel();
				productModel.Resolve(_scope);
				productModel = await productModel.GetProductByIdAsync(model.ProductId);

				productModel.UpdatedAtUtc = DateTime.UtcNow;
				var productResponse = await productModel.BuildAdapter().AdaptToTypeAsync<EditProductDto>();
				productResponse.StockLevel = productResponse.StockLevel + model.Quantity;
				productModel.Resolve(_scope);
				await productModel.EditProductAsync(productResponse);

				var transactionModel = new CreateTransactionModel();
				transactionModel.Resolve(_scope);

				transactionModel.ProductId = model.ProductId;
				transactionModel.PurchaseOrderId = model.Id;
				transactionModel.Quantity = model.Quantity;
				transactionModel.TotalAmount = model.TotalAmount;
				transactionModel.TransactionType = "purchase";
				transactionModel.CreatedAtUtc = DateTime.UtcNow;
				transactionModel.User = model.User;

				var transactionDto = await transactionModel.BuildAdapter().AdaptToTypeAsync<CreateTransactionDto>();
				transactionModel.Resolve(_scope);
				await transactionModel.CreateTransactionAsync(transactionDto);
			}
			else if (!string.Equals(tempStatus, modelStatus, StringComparison.OrdinalIgnoreCase) && modelStatus == "pending")
			{
				var productModel = new EditProductModel();
				productModel.Resolve(_scope);
				productModel = await productModel.GetProductByIdAsync(model.ProductId);

				productModel.UpdatedAtUtc = DateTime.UtcNow;
				var productResponse = await productModel.BuildAdapter().AdaptToTypeAsync<EditProductDto>();
				productResponse.StockLevel = productResponse.StockLevel - model.Quantity;
				productModel.Resolve(_scope);
				await productModel.EditProductAsync(productResponse);

				var transactionModel = new DeleteTransactionModel();
				transactionModel.Resolve(_scope);
				await transactionModel.DeleteByTransactionProductIdAsync(model.ProductId);
			}
			return RedirectToAction("Get");
		}

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				var model = new DeletePurchaseOrderModel();
				model.Resolve(_scope);
				await model.DeleteByIdAsync(id);
				return Json(new { success = true, message = "Purchase Order deleted successfully" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error deleting Purchase Order: " + ex.Message });
			}
			return RedirectToAction("Get");
		}
	}
}
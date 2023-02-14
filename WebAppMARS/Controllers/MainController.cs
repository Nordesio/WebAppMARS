using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Contracts.BindingModels;
using Contracts.ViewModels;
using Contracts.BusinessLogics;
namespace WebAppMARS.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IProductLogic _product;
        private readonly ISaleLogic _sale;
        private readonly ISalesPointLogic _salesPoint;
        public MainController(IProductLogic product, ISaleLogic sale, ISalesPointLogic salesPoint)
        {
            _product = product;
            _sale = sale;
            _salesPoint = salesPoint;
        }
        [HttpGet]
        public List<SaleViewModel> GetSaleList() => _sale.Read(null)?.ToList();
        [HttpGet]
        public SaleViewModel GetSale(int saleId) => _sale.Read(new
       SaleBindingModel
        { Id = saleId })?[0];
        [HttpPost]
        public void CreateSale(SaleBindingModel model) =>
        _sale.CreateOrUpdate(model);

        [HttpGet]
        public List<SalesPointViewModel> GetSalesPointList() => _salesPoint.Read(null)?.ToList();
        [HttpGet]
        public SalesPointViewModel GetSalesPoint(int salesPointId) => _salesPoint.Read(new
       SalesPointBindingModel
        { Id = salesPointId })?[0];
        [HttpPost]
        public void CreateSalesPoint(SalesPointBindingModel model) =>
        _salesPoint.CreateOrUpdate(model);


        [HttpGet]
        public List<ProductViewModel> GetProducts(int buyerId) => _product.Read(new
       ProductBindingModel
        { Id = buyerId });

        [HttpGet]
        public List<ProductViewModel> GetProductList() => _product.Read(null)?.ToList();
        [HttpPost]
        public void CreateProduct(ProductBindingModel model) =>
       _product.CreateOrUpdate(model);
    }
}

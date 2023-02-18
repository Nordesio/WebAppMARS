using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using Contracts.BindingModels;
using Contracts.ViewModels;
using Contracts.BusinessLogics;
namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductLogic _product;

        public HomeController(ILogger<HomeController> logger, IProductLogic product)
        {
            _logger = logger;
            _product = product;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Product()
        {
            return View();
        }
        
        [HttpPost]
        public void Product(string name, float price)
        {
            if(!string.IsNullOrEmpty(name) && price != 0)
            {
                var model = new ProductBindingModel();
                model.Name = name;
                model.Price = price;
                _product.CreateOrUpdate(model);
                Response.Redirect("Product");
                return;
            }
            throw new Exception("Введите название, цену");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

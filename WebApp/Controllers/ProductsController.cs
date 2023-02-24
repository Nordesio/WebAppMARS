using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatabaseImplement;
using DatabaseImplement.Models;
using DatabaseImplement.Implements;
using Contracts.BusinessLogics;
using Contracts.StorageContracts;
using Contracts.ViewModels;
using Contracts.BindingModels;
namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductLogic _productLogic;
        private readonly IProductStorage _productStorage;
        public ProductsController(IProductLogic productLogic, IProductStorage productStorage)
        {
            _productLogic = productLogic;
            _productStorage = productStorage;
        }
        public async Task<IActionResult> Index()
        {
            return View(_productLogic.Read(null)?.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductBindingModel product)
        {
            _productLogic.CreateOrUpdate(product);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string name, int price)
        {
            
            var model = new ProductBindingModel();
            model.Id = id;
            model.Name = name;
            model.Price = price;
            return View(_productLogic.Read(model)[0]);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ProductBindingModel model)
        {

            _productLogic.Delete(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string name, int price)
        {
            var model = new ProductBindingModel();
            model.Id = id;
            model.Name = name;
            model.Price = price;
            return View(_productLogic.Read(model)[0]);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductBindingModel product)
        {
            _productLogic.CreateOrUpdate(product);
            return RedirectToAction(nameof(Index));
        }
    }
}

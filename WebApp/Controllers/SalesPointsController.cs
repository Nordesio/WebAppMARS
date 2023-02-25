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
using static DatabaseImplement.Models.Product;
namespace WebApp.Controllers
{
    public class SalesPointsController : Controller
    {
        private readonly ISalesPointLogic _salesPointLogic;
        private readonly IProductLogic _productLogic;
      //  private Dictionary<int, (string, int)> providedProducts;
        private int Id { set { id = value; } }
        private int? id;



        public SalesPointsController(ISalesPointLogic salesPointLogic, IProductLogic productLogic)
        {

            _salesPointLogic = salesPointLogic;
            _productLogic = productLogic;
            //ViewBag.Products = new SelectList(_productLogic.Read(null), "Id", "Name");
        }
        public async Task<IActionResult> Index()
        {
            //SalesPointViewModel view = _salesPointLogic.Read(new SalesPointBindingModel { Id = id.Value })?[0];
            return View(_salesPointLogic.Read(null));
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var pr = new List<ProductViewModel>();
            var prnew = new ProductViewModel();
            prnew.Name = "Нет продукта";
            prnew.Id = 10000000;
            pr.Add(prnew);
            pr.AddRange(_productLogic.Read(null));
            ViewBag.Products = pr;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name, List<int> Product, List<int> Price)
        {
            List<ProductBindingModel> pbm = new List<ProductBindingModel>();
            foreach(int ch in Product)
            {
                if(ch != 10000000)
                {
                    var qwe = new ProductBindingModel();
                    qwe.Id = ch;
                    pbm.Add(qwe);
                }
            }
            for(int i = Product.Count() - 1; i > 0; i--)
            {
                if(Product[i] == 10000000)
                {
                    Product.RemoveAt(i);
                }
            }
            List<ProductViewModel> pr = new List<ProductViewModel>();
            foreach(var ch in pbm)
            {
               var list =  _productLogic.Read(ch);
               pr.Add(list[0]);
            }
            var model = new SalesPointBindingModel();
            model.Name = name;
            Dictionary<int, (string, int)> dict = new Dictionary<int, (string, int)>();
             for(int i = 0; i < Product.Count(); i++)
             {
                 dict.Add(Product[i], (pr[i].Name, Price[i]));
             }
            
            model.ProvidedProducts = dict;
            _salesPointLogic.CreateOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string name, Dictionary<int, (string, int)> providedProducts)
        {
            var model = new SalesPointBindingModel();

            model.Id = id;
            model.Name = name;
            model.ProvidedProducts = providedProducts;
            var pr = new List<ProductViewModel>();
            var prnew = new ProductViewModel();
            prnew.Name = "Нет продукта";
            prnew.Id = 10000000;
            pr.Add(prnew);
            pr.AddRange(_productLogic.Read(null));
            ViewBag.Products = pr;
            var model2 = _salesPointLogic.Read(model)[0];
            Dictionary<int, (string, int)> dict = new Dictionary<int, (string, int)>();
            return View(_salesPointLogic.Read(model)[0]);
        }
        public async Task<IActionResult> Edit(int? id,string name, List<int> Product, List<int> Price)
        {
            List<ProductBindingModel> pbm = new List<ProductBindingModel>();
            foreach (int ch in Product)
            {
                if (ch != 10000000)
                {
                    var qwe = new ProductBindingModel();
                    qwe.Id = ch;
                    pbm.Add(qwe);
                }
            }
            for (int i = Product.Count() - 1; i > 0; i--)
            {
                if (Product[i] == 10000000)
                {
                    Product.RemoveAt(i);
                }
            }
            List<ProductViewModel> pr = new List<ProductViewModel>();
            foreach (var ch in pbm)
            {
                var list = _productLogic.Read(ch);
                pr.Add(list[0]);
            }
            var model = new SalesPointBindingModel();
            model.Name = name;
            Dictionary<int, (string, int)> dict = new Dictionary<int, (string, int)>();
            for (int i = 0; i < Product.Count(); i++)
            {
                dict.Add(Product[i], (pr[i].Name, Price[i]));
            }

            model.ProvidedProducts = dict;
            model.Id = id;
            _salesPointLogic.CreateOrUpdate(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id, string name, Dictionary<int, (string, int)> dict)
        {

            var model = new SalesPointBindingModel();
            model.Id = id;
            model.Name = name;
            model.ProvidedProducts = dict;
            return View(_salesPointLogic.Read(model)[0]);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(SalesPointBindingModel model)
        {

            _salesPointLogic.Delete(model);
            return RedirectToAction(nameof(Index));
        }
    }
}

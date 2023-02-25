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
    public class BuyerAndSalesController : Controller
    {
        private readonly IBuyerLogic _buyerLogic;
        private readonly IBuyerStorage _buyerStorage;
        private readonly ISaleLogic _saleLogic;
        private readonly ISaleStorage _saleStorage;
        private readonly IProductLogic _productLogic;
        private readonly IProductStorage _productStorage;
        private readonly ISalesPointLogic _salesPointLogic;
        private readonly ISalesPointStorage _salesPointStorage;
        public BuyerAndSalesController(IBuyerLogic buyerLogic, IBuyerStorage buyerStorage, ISaleLogic saleLogic, ISaleStorage saleStorage,
            IProductLogic productLogic, IProductStorage productStorage, ISalesPointLogic salesPointLogic, ISalesPointStorage salesPointStorage)
        {
            _buyerLogic = buyerLogic;
            _buyerStorage = buyerStorage;
            _productLogic = productLogic;
            _productStorage = productStorage;
            _saleLogic = saleLogic;
            _saleStorage = saleStorage;
            _salesPointLogic = salesPointLogic;
            _salesPointStorage = salesPointStorage;
        }
        [HttpGet]
        public async Task<IActionResult> BuyerIndex()
        {
            return View(_buyerLogic.Read(null)?.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> BuyerDelete(int? id, string name, string password, Dictionary<int, int> salesIds)
        {

            var model = new BuyerBindingModel();
            model.Id = id;
            model.Name = name;
            model.Password = password;
            model.SalesIds = salesIds;
            return View(_buyerLogic.Read(model)[0]);
        }
        [HttpPost, ActionName("BuyerDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(BuyerBindingModel model)
        {

            _buyerLogic.Delete(model);
            return RedirectToAction(nameof(BuyerIndex));
        }
        [HttpGet]
        public async Task<IActionResult> BuyerEdit(int? id, string name, string password)
        {
            var model = new BuyerBindingModel();
            model.Id = id;
            model.Password = password;
            return View(_buyerLogic.Read(model)[0]);
        }
        [HttpPost]
        public async Task<IActionResult> BuyerEdit(BuyerBindingModel buyer)
        {
            _buyerLogic.CreateOrUpdate(buyer, true);
            return RedirectToAction(nameof(BuyerIndex));
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string name, string password)
        {
            var buyer = new BuyerBindingModel();
            buyer.Name = name;
            buyer.Password = password;
            _buyerLogic.CreateOrUpdate(buyer, false);
            return RedirectToAction(nameof(Enter));
        }
        [HttpGet]
        public async Task<IActionResult> Enter()
        {
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Enter(string name, string password)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(password))
            {
                var buyer = new BuyerBindingModel();
                buyer.Name = name;
                buyer.Password = password;
                var list = _buyerLogic.Read(buyer);
                if(list !=null && list.Count > 0)
                {
                    Program.Buyer = list[0];
                }
                if(Program.Buyer == null)
                {
                    throw new Exception("Неверный логин или пароль");
                }
                
                return RedirectToAction(nameof(SaleIndex));
            }
            throw new Exception("Введите логин/пароль");
        }

        [HttpGet]
        public async Task<IActionResult> SaleIndex()
        {

            return View(_saleLogic.Read(null)?.ToList());
        }
        [HttpGet]
        public async Task<IActionResult> SaleCreate()
        {
            ViewBag.SalesPoint = _salesPointLogic.Read(null);
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
        public async Task<IActionResult> SaleCreate(int salesPointId, List<int> Product, List<int> Count)
        {
            var sale = new SaleBindingModel();
            sale.Date = DateTime.Now.ToShortDateString();
            sale.Time = DateTime.Now.ToString("HH:mm:ss");
            sale.BuyerId = Program.Buyer.Id;
            sale.SalesPointId = salesPointId;
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
            var dict = new Dictionary<int, (string, int, decimal)>();
            decimal sum = 0;
            for(int i = 0; i < Product.Count(); i++)
            {
                decimal temp = pr[i].Price * Count[i];
                dict.Add(Product[i], (pr[i].Name, Count[i], temp));
                sum += temp;
            }
            sale.SalesData = dict;
            sale.TotalAmount = sum;
            _saleLogic.CreateOrUpdate(sale);
            return RedirectToAction(nameof(SaleIndex));
        }
        [HttpPost]
        public decimal Calc(decimal Count, int Product)
        {
            var prod = new ProductBindingModel();
            prod.Id = Product;
            var pr = _productLogic.Read(prod)[0];

            return 100;
        }
        [HttpGet]
        public async Task<IActionResult> SaleDelete(int? id, string date, string time, int salesPointId, int buyerId, Dictionary<int, (string, int, decimal)> dict, decimal totalAmount)
        {

            var model = new SaleBindingModel();
            model.Id = id;
            model.Date = date;
            model.Time = time;
            model.SalesPointId = salesPointId;
            model.BuyerId = buyerId;
            model.SalesData = dict;
            model.TotalAmount = totalAmount;
            return View(_saleLogic.Read(model)[0]);
        }
        [HttpPost, ActionName("SaleDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(SaleBindingModel model)
        {

            _saleLogic.Delete(model);
            return RedirectToAction(nameof(SaleIndex));
        }
        [HttpGet]
        public async Task<IActionResult> SaleEdit(int? id, Dictionary<int, (string, int, decimal)> dict)
        {
            ViewBag.SalesPoint = _salesPointLogic.Read(null);
            var model = new SaleBindingModel();
            model.Id = id;
            model.SalesData = dict;
            var pr = new List<ProductViewModel>();
            var prnew = new ProductViewModel();
            prnew.Name = "Нет продукта";
            prnew.Id = 10000000;
            pr.Add(prnew);
            pr.AddRange(_productLogic.Read(null));
            ViewBag.Products = pr;
            return View(_saleLogic.Read(model)[0]);
        }
        [HttpPost]
        public async Task<IActionResult> SaleEdit(int? id, int salesPointId, List<int> Product, List<int> Count)
        {
            var sale = new SaleBindingModel();
            sale.Date = DateTime.Now.ToShortDateString();
            sale.Time = DateTime.Now.ToString("HH:mm:ss");
            sale.BuyerId = Program.Buyer.Id;
            sale.SalesPointId = salesPointId;

            List<ProductBindingModel> pbm = new List<ProductBindingModel>();

            for (int i = Product.Count() - 1; i > 0; i--)
            {
                if (Product[i] == 10000000)
                {
                    Product.RemoveAt(i);
                }
            }
            foreach (int ch in Product)
            {
                    var qwe = new ProductBindingModel();
                    qwe.Id = ch;
                    pbm.Add(qwe);

            }

            List<ProductViewModel> pr = new List<ProductViewModel>();
            foreach (var ch in pbm)
            {
                var list = _productLogic.Read(ch);
                pr.Add(list[0]);
            }
            var dict = new Dictionary<int, (string, int, decimal)>();
            decimal sum = 0;
            for (int i = 0; i < Product.Count(); i++)
            {
                decimal temp = pr[i].Price * Count[i];
                dict.Add(Product[i], (pr[i].Name, Count[i], temp));
                sum += temp;
            }
            sale.SalesData = dict;
            sale.TotalAmount = sum;
            sale.Id = id;
            _saleLogic.CreateOrUpdate(sale);
            return RedirectToAction(nameof(SaleIndex));
        }

        [HttpGet]
        public async Task<IActionResult> Filling()
        {
            var pvm1 = new ProductBindingModel();
            var pvm2 = new ProductBindingModel();
            var pvm3 = new ProductBindingModel();

            pvm1.Name = "хлеб";
            pvm2.Name = "сыр";
            pvm3.Name = "масло";

            pvm1.Price = 50;
            pvm2.Price = 100;
            pvm3.Price = 150;
            _productLogic.CreateOrUpdate(pvm1);
            _productLogic.CreateOrUpdate(pvm2);
            _productLogic.CreateOrUpdate(pvm3);

            var bvm1 = new BuyerBindingModel();
            var bvm2 = new BuyerBindingModel();
            bvm1.Name = "Дмитрий";
            bvm2.Name = "Антон";
            bvm1.Password = "1111";
            bvm2.Password = "2222";

            _buyerLogic.CreateOrUpdate(bvm1, false);
            _buyerLogic.CreateOrUpdate(bvm2, false);

            var spvm1 = new SalesPointBindingModel();
            var spvm2 = new SalesPointBindingModel();
            spvm1.Name = "Пятерочка";
            spvm2.Name = "Магнит";
            Dictionary<int, (string, int)> dict = new Dictionary<int, (string, int)>();
            dict.Add(1, ("хлеб", 10));
            dict.Add(2, ("сыр", 20));
            spvm1.ProvidedProducts = dict;
            dict.Add(3, ("масло", 30));
            spvm2.ProvidedProducts = dict;
            _salesPointLogic.CreateOrUpdate(spvm1);
            _salesPointLogic.CreateOrUpdate(spvm2);

            var svm1 = new SaleBindingModel();
            var svm2 = new SaleBindingModel();
            svm1.Date = DateTime.Now.ToShortDateString();
            svm1.Time = DateTime.Now.ToString("HH:mm:ss");
            svm1.SalesPointId = 1;
            svm1.BuyerId = 1;
            Dictionary<int, (string, int, decimal)> dict2 = new Dictionary<int, (string, int, decimal)>();
            dict2.Add(1, ("хлеб", 5, 250));
            dict2.Add(2, ("сыр", 5, 500));
            svm1.SalesData = dict2;
            svm1.TotalAmount = 750;
            svm2.Date = DateTime.Now.ToShortDateString();
            svm2.Time = DateTime.Now.ToString("HH:mm:ss");
            svm2.SalesPointId = 2;
            svm2.BuyerId = 2;
            Dictionary<int, (string, int, decimal)> dict3 = new Dictionary<int, (string, int, decimal)>();
            dict3.Add(1, ("хлеб", 5, 250));
            dict3.Add(2, ("масло", 5, 750));
            svm2.SalesData = dict3;
            svm2.TotalAmount = 1000;
            _saleLogic.CreateOrUpdate(svm1);
            _saleLogic.CreateOrUpdate(svm2);
            
            return RedirectToAction(nameof(BuyerIndex));
        }
    }
}

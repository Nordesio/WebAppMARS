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
    }
}

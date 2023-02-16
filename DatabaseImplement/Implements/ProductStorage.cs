using DatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.BindingModels;
using Contracts.StorageContracts;
using Contracts.ViewModels;

namespace DatabaseImplement.Implements
{
    public class ProductStorage : IProductStorage
    {
        public List<ProductViewModel> GetFullList()
        {
            using var context = new SalesDatabase();
            return context.Products
            .Select(CreateModel)
            .ToList();
        }
        public List<ProductViewModel> GetFilteredList(ProductBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
            return context.Products
            .Where(rec => rec.Name.Contains(model.Name))
            .Select(CreateModel)
            .ToList();
        }
        public ProductViewModel GetElement(ProductBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
            var product = context.Products
            .FirstOrDefault(rec => rec.Name == model.Name || rec.Id == model.Id);
            return product != null ? CreateModel(product) : null;
        }
        public void Insert(ProductBindingModel model)
        {
            using var context = new SalesDatabase();
            context.Products.Add(CreateModel(model, new Product()));
            context.SaveChanges();
        }
        public void Update(ProductBindingModel model)
        {
            using var context = new SalesDatabase();
            var element = context.Products.FirstOrDefault(rec => rec.Id == model.Id);
            if(element == null)
            {
                throw new Exception("Элемент не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();
        }
        public void Delete(ProductBindingModel model)
        {
            using var context = new SalesDatabase();
            Product element = context.Products.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Products.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
        private static Product CreateModel(ProductBindingModel model, Product product)
        {
            product.Name = model.Name;
            product.Price = model.Price;
            return product;
        }
        private static ProductViewModel CreateModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,            };
        }
    }
}

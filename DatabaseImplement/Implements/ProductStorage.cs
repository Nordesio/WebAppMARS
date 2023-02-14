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
            .Include(rec => rec.ProvidedProducts)
            .Include(rec => rec.SalesDatas)
            .ToList()
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
           .Include(rec => rec.ProvidedProducts)
            .Include(rec => rec.SalesDatas)
            .Where(rec => rec.Name.Contains(model.Name))
            .ToList()
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
            .Include(rec => rec.ProvidedProducts)
            .Include(rec => rec.SalesDatas)
            .FirstOrDefault(rec => rec.Name == model.Name || rec.Id == model.Id);
            return product != null ? CreateModel(product) : null;
        }
        public void Insert(ProductBindingModel model)
        {
            using var context = new SalesDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                Product product = new Product()
                {
                    Name = model.Name,
                    Price = model.Price
                };
                context.Products.Add(product);
                context.SaveChanges();
                CreateModel(model, product, context);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Update(ProductBindingModel model)
        {
            using var context = new SalesDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Products.FirstOrDefault(rec => rec.Id == model.Id);
                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
                CreateModel(model, element, context);
                context.SaveChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
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
        private Product CreateModel(ProductBindingModel model, Product product, SalesDatabase context)
        {
            product.Name = model.Name;
            product.Price = model.Price;



            if (model.Id.HasValue)
            {
                var providedProducts = context.ProvidedProducts.Where(rec => rec.Id == model.Id).ToList();
                context.ProvidedProducts.RemoveRange(providedProducts);
                context.SaveChanges();
            }

            return product;
        }

        public void BindingOrder(int productId, int orderId)
        {
            var context = new SalesDatabase();
            context.ProvidedProducts.Add(new ProvidedProducts
            {
                ProductId = productId
            });
            context.SaveChanges();

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

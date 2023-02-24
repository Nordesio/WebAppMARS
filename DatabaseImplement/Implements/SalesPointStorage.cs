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
    public class SalesPointStorage : ISalesPointStorage
    {
        public List<SalesPointViewModel> GetFullList()
        {
            using (var context = new SalesDatabase())
            {
                return context.SalesPoints
                .Include(rec => rec.ProvidedProducts)
                .ThenInclude(rec => rec.Product)
                .ToList()
                .Select(CreateModel)
                .ToList();
            }
        }
        public List<SalesPointViewModel> GetFilteredList(SalesPointBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new SalesDatabase())
            {
                return context.SalesPoints
                .Include(rec => rec.ProvidedProducts)
                .ThenInclude(rec => rec.Product)
                .Where(rec => rec.Name.Contains(model.Name))
                .ToList()
                .Select(CreateModel)
                .ToList();
            }
        }
        public SalesPointViewModel GetElement(SalesPointBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
                var salesPoint = context.SalesPoints
                .Include(rec => rec.ProvidedProducts)
                .ThenInclude(rec => rec.Product)
                .FirstOrDefault(rec => rec.Name == model.Name || rec.Id == model.Id);
            return salesPoint != null ? CreateModel(salesPoint) : null;
        }
        public void Insert(SalesPointBindingModel model)
        {
            using var context = new SalesDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                SalesPoint salesPoint = new SalesPoint()
                {
                    Name = model.Name
                };
                context.SalesPoints.Add(salesPoint);
                context.SaveChanges();
                CreateModel(model, salesPoint, context);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Update(SalesPointBindingModel model)
        {
            using var context = new SalesDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.SalesPoints.FirstOrDefault(rec => rec.Id == model.Id);
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
        public void Delete(SalesPointBindingModel model)
        {
            using var context = new SalesDatabase();
            SalesPoint element = context.SalesPoints.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.SalesPoints.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        private static SalesPoint CreateModel(SalesPointBindingModel model, SalesPoint salesPoint, SalesDatabase context)
        {
            salesPoint.Name = model.Name;
            if (model.Id.HasValue)
            {
                var providedProducts = context.ProvidedProducts.Where(rec => rec.SalesPointId == model.Id.Value).ToList();
                context.ProvidedProducts.RemoveRange(providedProducts.Where(rec => !model.ProvidedProducts.ContainsKey(rec.ProductId)).ToList());
                context.SaveChanges();
                foreach (var update in providedProducts)
                {
                    update.ProductQuntity = model.ProvidedProducts[update.ProductId].Item2;
                    model.ProvidedProducts.Remove(update.ProductId);
                }
                context.SaveChanges();
            }
            foreach (var fc in model.ProvidedProducts)
            {
                context.ProvidedProducts.Add(new ProvidedProducts
                {
                    SalesPointId = salesPoint.Id,
                    ProductId = fc.Key,
                    ProductQuntity = fc.Value.Item2
                });
                context.SaveChanges();
            }
            return salesPoint;
        }
        private static SalesPointViewModel CreateModel(SalesPoint salesPoint)
        {
            return new SalesPointViewModel
            {
                Id = salesPoint.Id,
                Name = salesPoint.Name,
                ProvidedProducts = salesPoint.ProvidedProducts
                .ToDictionary(rec => rec.ProductId, rec => (rec.Product?.Name, rec.ProductQuntity))
            };
        }
    }
}

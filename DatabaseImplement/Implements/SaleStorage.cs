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
    public class SaleStorage : ISaleStorage
    {
        public List<SaleViewModel> GetFullList()
        {
            using (var context = new SalesDatabase())
            {
                return context.Sales
                .Include(rec => rec.SalesData)
                .ThenInclude(rec => rec.Product)
                
                .ToList()
                .Select(CreateModel)
                .ToList();
            }
        }
        public List<SaleViewModel> GetFilteredList(SaleBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new SalesDatabase())
            {
                return context.Sales
                .Include(rec => rec.SalesData)
                .ThenInclude(rec => rec.Product)
                
                .Where(rec => rec.BuyerId == model.BuyerId)
                .ToList()
                .Select(CreateModel)
                .ToList();
            }
        }
        public SaleViewModel GetElement(SaleBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
            var sale = context.Sales
            .Include(rec => rec.SalesData)
            .ThenInclude(rec => rec.Product)
            
            .FirstOrDefault(rec => rec.BuyerId == model.BuyerId || rec.Id == model.Id);
            return sale != null ? CreateModel(sale) : null;
        }
        public void Insert(SaleBindingModel model)
        {
            using var context = new SalesDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                Sale sale = new Sale()
                {
                    Date = model.Date,
                    Time = model.Time,
                    BuyerId = model.BuyerId,
                    SalesPointId = model.SalesPointId,
                    TotalAmount = model.TotalAmount
                };
                context.Sales.Add(sale);
                context.SaveChanges();
                CreateModel(model, sale, context);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
        public void Update(SaleBindingModel model)
        {
            using var context = new SalesDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var element = context.Sales.FirstOrDefault(rec => rec.Id == model.Id);
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
        public void Delete(SaleBindingModel model)
        {
            using var context = new SalesDatabase();
            Sale element = context.Sales.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Sales.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        private static Sale CreateModel(SaleBindingModel model, Sale sale, SalesDatabase context)
        {
            sale.Date = model.Date;
            sale.Time = model.Time;
            sale.BuyerId = model.BuyerId;
            sale.SalesPointId = model.SalesPointId;
            sale.TotalAmount = model.TotalAmount;
            if (model.Id.HasValue)
            {
                var salesData = context.SalesDatas.Where(rec => rec.SaleId == model.Id.Value).ToList();
                context.SalesDatas.RemoveRange(salesData.Where(rec => !model.SalesData.ContainsKey(rec.ProductId)).ToList());
                context.SaveChanges();
                salesData = context.SalesDatas.Where(rec => rec.SaleId == model.Id.Value).ToList();
                foreach (var update in salesData)
                {
                    update.ProductQuantity = model.SalesData[update.ProductId].Item2;
                    update.ProductIdAmount = model.SalesData[update.ProductId].Item3;
                    model.SalesData.Remove(update.ProductId);
                }
                context.SaveChanges();
            }
            foreach (var fc in model.SalesData)
            {
                context.SalesDatas.Add(new SalesData
                {
                    SaleId = sale.Id,
                    ProductId = fc.Key,
                    ProductQuantity = fc.Value.Item2,
                    ProductIdAmount = fc.Value.Item3
                });
                context.SaveChanges();
            }
            

            return sale;
        }
        private static SaleViewModel CreateModel(Sale sale)
        {
            return new SaleViewModel
            {
                Id = sale.Id,
                Date = sale.Date,
                Time = sale.Time,
                BuyerId = (int)sale.BuyerId,
                SalesPointId = sale.SalesPointId,
                TotalAmount = sale.TotalAmount,
                SalesData = sale.SalesData
                .ToDictionary(rec => rec.ProductId, rec => (rec.Product?.Name, rec.ProductQuantity, rec.ProductIdAmount))
            };
        }

    }
}

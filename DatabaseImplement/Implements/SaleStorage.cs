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
            using var context = new SalesDatabase();
            return context.Sales
            .ToList()
            .Select(CreateModel)
            .ToList();
        }
        public List<SaleViewModel> GetFilteredList(SaleBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
            return context.Sales
            .ToList()
            .Select(CreateModel)
            .ToList();
        }
        public SaleViewModel GetElement(SaleBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
            var sale = context.Sales
            .FirstOrDefault(rec =>  rec.Id == model.Id);
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
                    SalesPointId = model.SalesPointId,
                    BuyerId = model.BuyerId,
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
        private Sale CreateModel(SaleBindingModel model, Sale subject, SalesDatabase context)
        {
            subject.Date = model.Date;
            subject.Time = model.Time;
            subject.SalesPointId = model.SalesPointId;
            subject.BuyerId = model.BuyerId;
            subject.TotalAmount = model.TotalAmount;

            if (model.Id.HasValue)
            {
                var salesData = context.SalesDatas.Where(rec => rec.Id == model.Id).ToList();
                context.SalesDatas.RemoveRange(salesData);
                context.SaveChanges();
            }
            return subject;
        }
        
        private static SaleViewModel CreateModel(Sale sale)
        {
            return new SaleViewModel
            {
                Id = sale.Id,
                Date = sale.Date,
                Time = sale.Time,
                SalesPointId = sale.SalesPointId,
                BuyerId = sale.BuyerId,
                SalesData = sale.SalesData
                .ToDictionary(recSS => recSS.ProductQuantity, recSS => recSS.ProductIdAmount),
                TotalAmount = sale.TotalAmount

            };
        }

    }
}

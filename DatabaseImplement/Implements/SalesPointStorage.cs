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
                .Select(rec => new SalesPointViewModel
                {
                    Id = rec.Id,
                    Name = rec.Name,
                    ProvidedProducts = rec.ProvidedProducts.ToDictionary(rec => rec.ProductId, rec => rec.ProductQuntity)
                }).ToList();
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
                 .Select(rec => new SalesPointViewModel
                 {
                     Id = rec.Id,
                     Name = rec.Name,
                     ProvidedProducts = rec.ProvidedProducts.ToDictionary(rec => rec.ProductId, rec => rec.ProductQuntity)
                 }).ToList();
            }
        }
        public SalesPointViewModel GetElement(SalesPointBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using (var context = new SalesDatabase())
            {
                var subject = context.SalesPoints
                .FirstOrDefault(rec => rec.Name == model.Name || rec.Id == model.Id);
                return subject != null ?
               new SalesPointViewModel
               {
                   Id = subject.Id,
                   Name = subject.Name,
                   ProvidedProducts = subject.ProvidedProducts.ToDictionary(rec => rec.ProductId, rec => rec.ProductQuntity)
               } :
                null;
            }
        }
        public void Insert(SalesPointBindingModel model)
        {
            using (var context = new SalesDatabase())
            {
                context.SalesPoints.Add(CreateModel(model, new SalesPoint()));
                context.SaveChanges();
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

        private SalesPoint CreateModel(SalesPointBindingModel model, SalesPoint subject)
        {
            subject.Name = model.Name;
            return subject;
        }
    }
}

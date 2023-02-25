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
    public class BuyerStorage : IBuyerStorage
    {

        public List<BuyerViewModel> GetFullList()
        {
            using var context = new SalesDatabase();
            return context.Buyers
            .Select(CreateModel)
            .ToList();
        }
        public List<BuyerViewModel> GetFilteredList(BuyerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
            return context.Buyers
            .Where(rec => rec.Name.Contains(model.Name))
            .Select(CreateModel)
            .ToList();
        }
        public BuyerViewModel GetElement(BuyerBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            using var context = new SalesDatabase();
            var buyer = context.Buyers
            .FirstOrDefault(rec => rec.Name == model.Name || rec.Id == model.Id);
            return buyer != null ? CreateModel(buyer) : null;
        }
        public void Insert(BuyerBindingModel model)
        {
            using var context = new SalesDatabase();

            context.Buyers.Add(CreateModel(model, new Buyer()));
            context.SaveChanges();

        }

        public void Update(BuyerBindingModel model)
        {
            using var context = new SalesDatabase();

            var element = context.Buyers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Покупатель не найден");
            }
            CreateModel(model, element);
            context.SaveChanges();

        }
        public void Delete(BuyerBindingModel model)
        {
            using var context = new SalesDatabase();
            Buyer element = context.Buyers.FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Buyers.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Клиент не найден");
            }
        }

        private Buyer CreateModel(BuyerBindingModel model, Buyer buyer)
        {
            buyer.Name = model.Name;
            buyer.Password = model.Password;
            return buyer;
        }
        private static BuyerViewModel CreateModel(Buyer buyer)
        {
            return new BuyerViewModel
            {
                Id = buyer.Id,
                Name = buyer.Name,
                Password = buyer.Password
            };
        }
    }
}

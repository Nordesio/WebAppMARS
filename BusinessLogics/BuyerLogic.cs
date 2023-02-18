using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.BindingModels;
using Contracts.ViewModels;
using Contracts.StorageContracts;
using Contracts.BusinessLogics;

namespace BusinessLogics
{
    public class BuyerLogic : IBuyerLogic
    {
        private readonly IBuyerStorage _buyerStorage;

        public BuyerLogic(IBuyerStorage buyerStorage)
        {
            _buyerStorage = buyerStorage;
        }

        public List<BuyerViewModel> Read(BuyerBindingModel model)
        {
            if (model == null)
            {
                return _buyerStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<BuyerViewModel> { _buyerStorage.GetElement(model) };
            }
            return _buyerStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(BuyerBindingModel model, bool update = false)
        {
            var element = _buyerStorage.GetElement(new BuyerBindingModel
            {
                Name = model.Name,
                Password = model.Password
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть клиент с таким именем");
            }
            if (update)
            {
                _buyerStorage.Update(model);
            }
            else
            {
                _buyerStorage.Insert(model);
            }
        }
        public void Delete(BuyerBindingModel model)
        {
            var element = _buyerStorage.GetElement(new BuyerBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Клиент не найден");
            }
            _buyerStorage.Delete(model);
        }
    }
}

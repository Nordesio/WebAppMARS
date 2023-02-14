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
    public class SaleLogic : ISaleLogic
    {
        private readonly ISaleStorage _saleStorage;

        public SaleLogic(ISaleStorage saleStorage)
        {
            _saleStorage = saleStorage;
        }

        public List<SaleViewModel> Read(SaleBindingModel model)
        {
            if (model == null)
            {
                return _saleStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<SaleViewModel> { _saleStorage.GetElement(model) };
            }
            return _saleStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(SaleBindingModel model)
        {
            var element = _saleStorage.GetElement(new SaleBindingModel
            {
                Id = model.Id
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такая покупка");
            }
            if (model.Id.HasValue)
            {
                _saleStorage.Update(model);
            }
            else
            {
                _saleStorage.Insert(model);
            }
        }
        public void Delete(SaleBindingModel model)
        {
            var element = _saleStorage.GetElement(new SaleBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Клиент не найден");
            }
            _saleStorage.Delete(model);
        }
    }
}

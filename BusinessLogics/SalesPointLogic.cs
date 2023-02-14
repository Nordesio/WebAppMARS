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
    public class SalesPointLogic : ISalesPointLogic
    {
        private readonly ISalesPointStorage _salesPointStorage;

        public SalesPointLogic(ISalesPointStorage salesPointStorage)
        {
            _salesPointStorage = salesPointStorage;
        }

        public List<SalesPointViewModel> Read(SalesPointBindingModel model)
        {
            if (model == null)
            {
                return _salesPointStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<SalesPointViewModel> { _salesPointStorage.GetElement(model) };
            }
            return _salesPointStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(SalesPointBindingModel model)
        {
            var element = _salesPointStorage.GetElement(new SalesPointBindingModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такая точка");
            }
            if (model.Id.HasValue)
            {
                _salesPointStorage.Update(model);
            }
            else
            {
                _salesPointStorage.Insert(model);
            }
        }
        public void Delete(SalesPointBindingModel model)
        {
            var element = _salesPointStorage.GetElement(new SalesPointBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Клиент не найден");
            }
            _salesPointStorage.Delete(model);
        }
    }
}

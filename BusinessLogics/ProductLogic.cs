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
    public class ProductLogic : IProductLogic
    {
        private readonly IProductStorage _productStorage;

        public ProductLogic(IProductStorage productStorage)
        {
            _productStorage = productStorage;
        }

        public List<ProductViewModel> Read(ProductBindingModel model)
        {
            if (model == null)
            {
                return _productStorage.GetFullList();
            }
            if (model.Id.HasValue)
            {
                return new List<ProductViewModel> { _productStorage.GetElement(model) };
            }
            return _productStorage.GetFilteredList(model);
        }

        public void CreateOrUpdate(ProductBindingModel model)
        {
            var element = _productStorage.GetElement(new ProductBindingModel
            {
                Name = model.Name
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть продукт с таким названием");
            }
            if (model.Id.HasValue)
            {
                _productStorage.Update(model);
            }
            else
            {
                _productStorage.Insert(model);
            }
        }
        public void Delete(ProductBindingModel model)
        {
            var element = _productStorage.GetElement(new ProductBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Клиент не найден");
            }
            _productStorage.Delete(model);
        }
    }
}

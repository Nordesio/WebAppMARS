using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.BindingModels;
using Contracts.ViewModels;
namespace Contracts.BusinessLogics
{
    public interface IProductLogic
    {
        List<ProductViewModel> Read(ProductBindingModel model);
        void CreateOrUpdate(ProductBindingModel model);
        void Delete(ProductBindingModel model);
    }
}

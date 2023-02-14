using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.BindingModels;
using Contracts.ViewModels;
namespace Contracts.BusinessLogics
{
    public interface ISaleLogic
    {
        List<SaleViewModel> Read(SaleBindingModel model);
        void CreateOrUpdate(SaleBindingModel model);
        void Delete(SaleBindingModel model);
    }
}

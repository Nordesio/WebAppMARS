using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.BindingModels;
using Contracts.ViewModels;

namespace Contracts.BusinessLogics
{
    public interface IBuyerLogic
    {
        List<BuyerViewModel> Read(BuyerBindingModel model);
        void CreateOrUpdate(BuyerBindingModel model, bool update);
        void Delete(BuyerBindingModel model);
    }
}

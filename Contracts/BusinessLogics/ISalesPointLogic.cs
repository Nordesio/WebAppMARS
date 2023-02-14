using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.BindingModels;
using Contracts.ViewModels;
namespace Contracts.BusinessLogics
{
    public interface ISalesPointLogic
    {
        List<SalesPointViewModel> Read(SalesPointBindingModel model);
        void CreateOrUpdate(SalesPointBindingModel model);
        void Delete(SalesPointBindingModel model);
    }
}

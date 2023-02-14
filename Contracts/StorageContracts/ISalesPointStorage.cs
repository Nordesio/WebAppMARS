using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.ViewModels;
using Contracts.BindingModels;

namespace Contracts.StorageContracts
{
    public interface ISalesPointStorage
    {
        List<SalesPointViewModel> GetFullList();
        List<SalesPointViewModel> GetFilteredList(SalesPointBindingModel model);
        SalesPointViewModel GetElement(SalesPointBindingModel model);
        void Insert(SalesPointBindingModel model);
        void Update(SalesPointBindingModel model);
        void Delete(SalesPointBindingModel model);
    }
}

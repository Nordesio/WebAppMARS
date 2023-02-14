using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.ViewModels;
using Contracts.BindingModels;

namespace Contracts.StorageContracts
{
    public interface ISaleStorage
    {
        List<SaleViewModel> GetFullList();
        List<SaleViewModel> GetFilteredList(SaleBindingModel model);
        SaleViewModel GetElement(SaleBindingModel model);
        void Insert(SaleBindingModel model);
        void Update(SaleBindingModel model);
        void Delete(SaleBindingModel model);
    }
}

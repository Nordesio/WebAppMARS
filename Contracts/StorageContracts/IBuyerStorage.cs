using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.ViewModels;
using Contracts.BindingModels;

namespace Contracts.StorageContracts
{
    public interface IBuyerStorage
    {
        List<BuyerViewModel> GetFullList();
        List<BuyerViewModel> GetFilteredList(BuyerBindingModel model);
        BuyerViewModel GetElement(BuyerBindingModel model);
        void Insert(BuyerBindingModel model);
        void Update(BuyerBindingModel model);
        void Delete(BuyerBindingModel model);
    }
}

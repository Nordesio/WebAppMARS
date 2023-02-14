using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contracts.BindingModels;
using Contracts.ViewModels;
using Contracts.BusinessLogics;
namespace WebAppMARS.Controllers
{
    [Route("api/[controller]/[action]")]
      [ApiController]
      public class BuyerController : ControllerBase
      {
        private readonly IBuyerLogic _logic;
        public BuyerController(IBuyerLogic logic)
        {
            _logic = logic;
        }
        [HttpGet]
        public BuyerViewModel Logic(string name)
        {
            var list = _logic.Read(new BuyerBindingModel { Name = name });
            return (list != null && list.Count > 0) ? list[0] : null;
        }
        [HttpPost]
        public void Register(BuyerBindingModel model) =>
            _logic.CreateOrUpdate(model);
        [HttpPost]
        public void UpdateData(BuyerBindingModel model) =>
            _logic.CreateOrUpdate(model);
      }
}

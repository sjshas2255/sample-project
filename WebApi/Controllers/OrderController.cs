
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.GlobalException;


namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    [GenericExceptionHandler]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<HttpResponseMessage> GetOrders(string status = null, DateTime? orderDate = null)
        {
            var orders = await _orderService.GetOrders(status, orderDate);
            var orderModels = orders.Select(o => new OrderModel(o)).ToList();
            return Found(orderModels);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<HttpResponseMessage> GetOrder(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
                return DoesNotExist();
            return Found(new OrderModel(order));
        }

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateOrder([FromBody] OrderModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var order = await _orderService.CreateOrder(model.ToEntity());
            return Found(new OrderModel(order));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<HttpResponseMessage> UpdateOrder(int id, [FromBody] OrderModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var updated = await _orderService.UpdateOrder(id, model.ToEntity());
            if (updated == null)
                return DoesNotExist();
            return Found(new OrderModel(updated));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<HttpResponseMessage> DeleteOrder(int id)
        {
            var deleted = await _orderService.DeleteOrder(id);
            if (!deleted)
                return DoesNotExist();
            return Found();
        }
    }
}
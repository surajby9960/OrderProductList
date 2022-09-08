using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderProductList.Model;
using OrderProductList.Repositories.Interfaces;

namespace OrderProductList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository repo;
        public OrderController(IOrderRepository repo)
        {
            this.repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrder()
        {
            try
            {
                var order = await repo.GetAllOrder();
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await repo.GetOrderById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertOrder(Order order)
        {
            try
            {
                var res = await repo.InsertOrder(order);
                return StatusCode(200, "Inserted Succefully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            try

            {
                var res = await repo.UpdateOrder(order);
                return StatusCode(200, "Updated Succesfully");

            }
            catch (Exception ex)

            {
                return StatusCode(500, ex.Message);


            }
        }
        [HttpDelete("id")]
        public async Task<IActionResult> DeleteById(int id)
        {
            try
            {
               await repo.DeleteOrder(id);
                return StatusCode(200, "Deleted Succesfully");
             }catch(Exception  ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}

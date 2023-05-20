using Grpc.Net.Client;
using ITI.gRPC.Client.Models;
using ITI.gRPC.Inventory.Protos;
using ITI.gRPC.Payment.Protos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.gRPC.Client.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GrpcChannel _inventoryChannel;
        private readonly GrpcChannel _paymentChannel;

        public OrderController()
        {
            _inventoryChannel = GrpcChannel.ForAddress("https://localhost:7106");
            _paymentChannel = GrpcChannel.ForAddress("https://localhost:7224");
        }

        [HttpPost]
        public IActionResult CreateOrder(Order order)
        {
            var inventoryClient = new InventoryService.InventoryServiceClient(_inventoryChannel);
            var paymentClient = new PaymentService.PaymentServiceClient(_paymentChannel);
            
            var inventoryResponse = inventoryClient.AddNewItem(new InventoryMessage()
            {
                ItemID = order.ItemId,
                Quantity = order.Quantity,
                
            });

            var paymentResponse = paymentClient.NewPayment(new PaymentMessage()
            {
                UserID = order.UserId,
                Cost = order.Cost,
                
            });



            if (inventoryResponse.Success && paymentResponse.Success)
            {
                return Ok("Order has been created successfully!");
            }

            return BadRequest($"Sorry! there is something went wrong while creating the order: {(!inventoryResponse.Success? inventoryResponse.Message : string.Empty)}, {(!paymentResponse.Success ? paymentResponse.Message : string.Empty)}");
        }
    }
}

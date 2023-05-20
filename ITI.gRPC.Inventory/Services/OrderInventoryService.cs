using Grpc.Core;
using ITI.gRPC.Inventory.Models;
using ITI.gRPC.Inventory.Protos;
using static ITI.gRPC.Inventory.Protos.InventoryService;

namespace ITI.gRPC.Inventory.Services
{
    public class OrderInventoryService : InventoryServiceBase
    {
        private static List<Item> _items;
        public OrderInventoryService()
        {
            _items = new List<Item>()
            {
                new Item {Id = 1, Quantity = 10, Price = 500 },
                new Item {Id = 2, Quantity = 20, Price = 1000 },
                new Item {Id = 3, Quantity = 30, Price = 1500 },
            };
        }

        public override Task<InventoryResponse> AddNewItem(InventoryMessage request, ServerCallContext context)
        {
            bool success = false;
            string message = "";

            Item? item = _items.FirstOrDefault(i => i.Id == request.ItemID);

            if (item != null)
            {
                if (item.Quantity >= request.Quantity) 
                {
                    item.Quantity -= request.Quantity;

                    success = true;
                    message = "Item has been added successfully!";
                }
                else
                {
                    message = "Sorry! There is no enough quantity in the inventory.";
                }
            }
            else
            {
                message = "Can not find this item!";
            }

            var response = new InventoryResponse() { Success = success, Message = message};

            return Task.FromResult(response);
        }
    }
}

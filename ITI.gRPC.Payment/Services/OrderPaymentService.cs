using Grpc.Core;
using ITI.gRPC.Payment.Models;
using ITI.gRPC.Payment.Protos;
using static ITI.gRPC.Payment.Protos.PaymentService;

namespace ITI.gRPC.Payment.Services
{
    public class OrderPaymentService : PaymentServiceBase
    {
        private static List<User> _users;
        public OrderPaymentService() 
        {
            _users = new List<User>() 
            {
                new User{ Id = 1, Balance = 1000},
                new User{ Id = 2, Balance = 2000},
                new User{ Id = 3, Balance = 3000}
            };
        }

        public override Task<PaymentResponse> NewPayment(PaymentMessage request, ServerCallContext context)
        {
            bool success = false;
            string message = "";

            User? user = _users.FirstOrDefault(u => u.Id == request.UserID);

            if (user != null)
            {
                if (user.Balance > request.Cost) 
                {
                    user.Balance -= request.Cost;
                    
                    success = true;
                    message = "Payment Completed Successfully!";
                }
                else
                {
                    message = "Sorry! Not enough balance.";
                }
            }
            else
            {
                message = "User Not Found!";
            }

            var response = new PaymentResponse() { Success = success, Message = message};

            return Task.FromResult(response);
        }
    }
}

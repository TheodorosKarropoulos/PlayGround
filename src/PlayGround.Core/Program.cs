using System;
using System.Linq;

namespace PlayGround.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            var lazyLoading = new LazyLoading();
            var customer = lazyLoading.GetCustomerById(Guid.NewGuid());

            var orders = customer.CustomerOrders;
            var orderList = orders.OrderList.Value;
            
            Console.WriteLine($"Customer {customer.Name} has {orderList.Count()} orders");
        }
    }
}

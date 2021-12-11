using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace PlayGround.Core
{
    public class LazyLoading
    {
        public Customer GetCustomerById(Guid customerId)
        {
            return new Customer
            {
                Id = customerId,
                Name = "John Doe",
            };
        }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        private readonly Lazy<Orders> Orders;

        public Customer()
        {
            Orders = new Lazy<Orders>(GetMockOrders);
        }
        
        public Orders CustomerOrders => Orders?.Value;

        private static Orders GetMockOrders()
        {
            return new Orders
            {
                OrderList = new Lazy<IEnumerable<Order>>(GetMockOrder)
            };
        }

        private static IEnumerable<Order> GetMockOrder()
        {
            return new List<Order>
            {
                new Order
                {
                    OrderId = Guid.NewGuid(),
                    OrderLines = new Lazy<OrderLines>(() => new OrderLines
                    {
                        Lines = new List<OrderLine>
                        {
                            new OrderLine
                            {
                                Item = new Item(Guid.NewGuid(), "Coffee Mug"),
                                LineId = Guid.NewGuid(),
                                LineValue = new OrderLineValue
                                {
                                    Quantity = 1m,
                                    Money = new Money(12.99m, Currency.Euro)
                                }
                            }
                        }
                    })
                }
            };
        }
    }

    public class Orders
    {
        public Lazy<IEnumerable<Order>> OrderList { get; set; }
    }

    public class Order
    {
        public Guid OrderId { get; set; }
        public Lazy<OrderLines> OrderLines { get; set; }
    }

    public class OrderLines
    {
        public IEnumerable<OrderLine> Lines { get; set; }
    }

    public class OrderLine
    {
        public Guid LineId { get; set; }
        public Item Item { get; set; }
        public OrderLineValue LineValue { get; set; }
    }

    public class OrderLineValue
    {
        public decimal Quantity { get; set; }
        public Money Money { get; set; }
    }

    public class Item
    {
        private Guid ItemId { get; }
        private string Name { get; }

        public Item(Guid itemId, string name)
        {
            ItemId = itemId;
            Name = name;
        }
    }

    public class Money
    {
        private decimal Price { get; }
        private Currency Currency { get; }

        public Money(decimal price, Currency currency)
        {
            Price = price;
            Currency = currency;
        }
    }

    public enum Currency
    {
        PoundSterling = 826,
        UsDollar = 840,
        Euro = 978
    }
}
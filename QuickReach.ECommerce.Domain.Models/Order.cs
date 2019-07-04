using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Order")]
    public class Order : EntityBase
    {
        public Order(int customerId)
        {
            CustomerId = customerId;
            this.Items = new List<OrderItem>();
        }

        public int CustomerId { get; set; }

        //[ForeignKey("CustomerId")]
        public int CartId { get; set; }
        //[ForeignKey("CartId")]
        public List<OrderItem> Items { get; set; }

        public void AddItem(OrderItem item)
        {
            ((ICollection<OrderItem>)this.Items).Add(item);
        }

        public OrderItem GetItem(int itemId)
        {
            return ((ICollection<OrderItem>)this.Items).FirstOrDefault(ci => ci.Id == itemId);
        }
        public void RemoveItem(int itemId)
        {
            var item = this.GetItem(itemId);
            ((ICollection<OrderItem>)this.Items).Remove(item);
        }
    }
}

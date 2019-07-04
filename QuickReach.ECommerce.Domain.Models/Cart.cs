using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Cart")]
    public class Cart : EntityBase //parent
    {
        public Cart(int customerId)
        {
            CustomerId = customerId;
            this.Items = new List<CartItem>();
        }
        public int CustomerId { get; set; }
        public List<CartItem> Items { get; set; }

        public void AddItem(CartItem item)
        {
            ((ICollection<CartItem>)this.Items).Add(item);
        }

        public CartItem GetItem(int itemId)
        {
            return ((ICollection<CartItem>)this.Items).FirstOrDefault(ci => ci.Id == itemId);
        }
        public void RemoveItem(int itemId)
        {
            var item = this.GetItem(itemId);
            ((ICollection<CartItem>)this.Items).Remove(item);
        }

    }
}

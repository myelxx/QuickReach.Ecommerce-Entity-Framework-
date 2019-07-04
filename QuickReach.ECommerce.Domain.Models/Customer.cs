using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Customer")]
    public class Customer : EntityBase
    {
        public Customer()
        {
            Carts = new List<Cart>();
        }

        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string SecurityNumber { get; set; }
        [Required]
        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "Expiration should match a valid MM/YY value")]
        public string Expiration { get; set; }
        [Required]
        public string CardHolderName { get; set; }
        public int CardType { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public List<Cart> Carts { get; set; }

        public void AddCart(Cart item)
        {
            ((ICollection<Cart>)this.Carts).Add(item);
        }

        public Cart GetCart(int cartId)
        {
            return ((ICollection<Cart>)this.Carts).FirstOrDefault(c => c.ID == cartId);
        }
        public void RemoveCart(int cartId)
        {
            var cart = this.GetCart(cartId);
            ((ICollection<Cart>)this.Carts).Remove(cart);
        }
    }
}

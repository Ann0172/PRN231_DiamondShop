using System;
using System.Collections.Generic;

namespace DiamondShop.Repository.Models;

public partial class Account
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string AvatarUrl { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<Order> OrderCustomers { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderDeliveryStaffs { get; set; } = new List<Order>();

    public virtual ICollection<Order> OrderSalesStaffs { get; set; } = new List<Order>();
}

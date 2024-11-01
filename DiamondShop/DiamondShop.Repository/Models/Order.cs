using System;
using System.Collections.Generic;

namespace DiamondShop.Repository.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public string? PayMethod { get; set; }

    public string Status { get; set; } = null!;

    public DateOnly CreatedDate { get; set; }

    public long TotalPrice { get; set; }

    public Guid CustomerId { get; set; }

    public Guid? SalesStaffId { get; set; }

    public Guid? DeliveryStaffId { get; set; }

    public virtual Account Customer { get; set; } = null!;

    public virtual Account? DeliveryStaff { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Account? SalesStaff { get; set; }
}

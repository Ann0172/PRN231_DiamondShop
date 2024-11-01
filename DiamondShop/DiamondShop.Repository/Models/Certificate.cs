using System;
using System.Collections.Generic;

namespace DiamondShop.Repository.Models;

public partial class Certificate
{
    public Guid Id { get; set; }

    public string? ReportNumber { get; set; }

    public string Origin { get; set; } = null!;

    public string Shape { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Clarity { get; set; } = null!;

    public string Cut { get; set; } = null!;

    public string CaratWeight { get; set; } = null!;

    public DateTime DateOfIssue { get; set; }

    public string Status { get; set; } = null!;

    public virtual Diamond? Diamond { get; set; }
}

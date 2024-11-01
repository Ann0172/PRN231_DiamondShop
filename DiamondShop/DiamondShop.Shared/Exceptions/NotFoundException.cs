using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondShop.Shared.Exceptions
{
    public class NotFoundException(string message) : ApplicationException(message)
    {
    }
}

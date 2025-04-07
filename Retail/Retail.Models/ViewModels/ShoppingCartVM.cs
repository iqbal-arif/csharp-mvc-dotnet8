using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail.Models.ViewModels
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

        // Adding OrderHeader in the ShoppingCartVm
        public OrderHeader OrderHeader { get; set; }


    }
}

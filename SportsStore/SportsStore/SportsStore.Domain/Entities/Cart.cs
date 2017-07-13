using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines
        {
            get
            {
                return lineCollection;
            }
        }

        public void AddItem(Product product, int qantity)
        {
            throw new NotImplementedException();
        }

        public void RemoveLine(Product product)
        {
            throw new NotImplementedException();
        }

        public decimal ComputeTotalValue()
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}

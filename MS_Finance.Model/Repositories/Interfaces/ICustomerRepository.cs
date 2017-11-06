using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        void CreateCustomer(Customer customer);

        Customer GetCustomerByNIC(string nic);

        Customer GetSingle(string id);
    }
}

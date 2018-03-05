using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface ICustomerService
    {
        IList<Customer> GetAll();

        Customer GetById(string id);

        void Create(Customer customer);

        void Update(Customer customer);

        void Attach(Customer customer);
        //void Delete(string id);



        bool CreateCustomer(CustomerModel customerModel);

        Customer IsCustomerExist(string customerNIC);

        void AddRange(IEnumerable<Customer> customers);
    }
}

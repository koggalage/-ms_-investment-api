using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Repositories
{
    public class CustomerRepository
    {
        AuthContext _context;

        public CustomerRepository()
        {
            _context = new AuthContext();
        }

        public void CreateCustomer(Customer customer) 
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public Customer GetCustomerByNIC(string nic)
        {
            return _context.Customers.Where(x => x.NIC == nic).FirstOrDefault();
        }

        public Customer GetSingle(string id)
        {
            return _context.Customers.Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
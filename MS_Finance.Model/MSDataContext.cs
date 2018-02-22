using Microsoft.AspNet.Identity.EntityFramework;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model
{
    public class MSDataContext : DbContext
    {
        public MSDataContext()
            : base("MSDataContext")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static MSDataContext Create()
        {
            return new MSDataContext();
        }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Guarantor> Guarantors { get; set; }

        public DbSet<Broker> brokers { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<ContractInstallment> ContractInstallments { get; set; }

        public DbSet<ContractFine> ContractFines { get; set; }

        public DbSet<Excess> Excesses { get; set; }

        public DbSet<FinePayment> FinePayments { get; set; }

        public DbSet<ContractSettlement> ContractSettlements { get; set; }

        public DbSet<ContractFile> ContractFiles { get; set; }

        public DbSet<ContractRate> ContractRates { get; set; }

        public DbSet<Location> Locations { get; set; }
    }
}

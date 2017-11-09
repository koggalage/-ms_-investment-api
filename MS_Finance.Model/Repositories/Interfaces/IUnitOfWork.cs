﻿using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        int Commit();


        IRepository<T> GetRepository<T>();
        
        #region IRepositories

        IRepository<Broker> Brokers { get; }

        IRepository<Customer> Customers { get; }

        IRepository<Contract> Contracts { get; }

        IRepository<Guarantor> Guarantors { get; }

        IRepository<ContractInstallment> ContractInstallments { get; }

        #endregion
    }
}
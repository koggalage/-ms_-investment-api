﻿using MS_Finance.Model.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.OA
{
    public class UnitOfWork : IUnitOfWork
    {
        protected MSDataContext Context;

        public UnitOfWork(MSDataContext context)
        {
            this.Context = context;
        }

        public int Commit() 
        {
            return Context.SaveChanges();    
        }

        public IRepository<T> GetRepository<T>()
        {
            IRepository<T> result = null;
            var property = this.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == (typeof(IRepository<T>)));
            if (property != null) { result = property.GetValue(this) as IRepository<T>; }

            return result;
        }

        private IRepository<Customer> _Customers;
        public IRepository<Customer> Customers
        {
            get { return _Customers ?? (_Customers = new BaseRepository<Customer>(Context)); }
        }

        private IRepository<Broker> _Brokers;
        public IRepository<Broker> Brokers
        {
            get { return _Brokers ?? (_Brokers = new BaseRepository<Broker>(Context)); }
        }

        private IRepository<Contract> _Contracts;
        public IRepository<Contract> Contracts
        {
            get { return _Contracts ?? (_Contracts = new BaseRepository<Contract>(Context)); }
        }

        private IRepository<Guarantor> _Guarantors;
        public IRepository<Guarantor> Guarantors
        {
            get { return _Guarantors ?? (_Guarantors = new BaseRepository<Guarantor>(Context)); }
        }

        private IRepository<ContractInstallment> _ContractInstallments;
        public IRepository<ContractInstallment> ContractInstallments
        {
            get { return _ContractInstallments ?? (_ContractInstallments = new BaseRepository<ContractInstallment>(Context)); }
        }
    }
}

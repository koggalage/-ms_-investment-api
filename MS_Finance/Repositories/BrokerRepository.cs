﻿using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Repositories
{
    public class BrokerRepository
    {
         AuthContext _context;

         public BrokerRepository()
        {
            _context = new AuthContext();
        }

        public void CreateBroker(Broker broker) 
        {
            _context.brokers.Add(broker);
            _context.SaveChanges();
        }

        public Broker GetSingle(string id)
        {
            return _context.brokers.Where(x => x.Id == id).FirstOrDefault();
        }

        public Broker GetBrokerByNIC(string nic)
        {
            return _context.brokers.Where(x => x.NIC == nic).FirstOrDefault();
        }
    }
}
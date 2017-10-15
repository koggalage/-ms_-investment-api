using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Services
{
    public class BrokerService
    {
        private BrokerRepository _brokerRepository;

        public BrokerService()
        {
            _brokerRepository = new BrokerRepository();
        }
        
        public bool CreateBroker(BrokerModel brokerModel)
        {
            var broker = new Broker()
            {
                Name = brokerModel.Name,
                Address = brokerModel.Address,
                ContactNo = brokerModel.ContactNo,
                NIC = brokerModel.NIC,
                Occupation = brokerModel.Occupation,
                CreatedDate = DateTime.Now
            };

            _brokerRepository.CreateBroker(broker);

            return true;
        }

        public Broker IsBrokerExist(string brokerNIC)
        {
            var brokerByNIC = _brokerRepository.GetBrokerByNIC(brokerNIC);

            return brokerByNIC;
        }

    }
}
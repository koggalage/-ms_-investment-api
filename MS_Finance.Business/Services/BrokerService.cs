using MS_Finance.Business.Interfaces;
using MS_Finance.Business.Services;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MS_Finance.Business
{
    public class BrokerService : DefaultPersistentService<Broker>,  IBrokerService
    {

        public BrokerService(IUnitOfWork UoW)
            : base(UoW)
        {

        }


        public IList<Broker> GetAll()
        {
            return base
                .GetAll()
                .ToList();
        }

        public Broker GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(Broker broker)
        {
            base.Add(broker);
        }

        public void Update(Broker broker)
        {
            base.Update(broker);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Attach(Broker broker)
        {
            base.Attach(broker);
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
                CreatedDate = DateTime.Now,
                CreatedByUserId = brokerModel.CreatedByUserId,
                CreatedByUserName = brokerModel.CreatedByUserName
            };

            base.Add(broker);
            //_brokerRepository.CreateBroker(broker);

            return true;
        }

        public Broker IsBrokerExist(string brokerNIC)
        {
            //var brokerByNIC = _brokerRepository.GetBrokerByNIC(brokerNIC);
            //return brokerByNIC;

            return base.GetAll()
                .Where(x => x.NIC == brokerNIC).FirstOrDefault();
        }

    }
}
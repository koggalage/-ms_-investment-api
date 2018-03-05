using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IBrokerService
    {
        IList<Broker> GetAll();

        Broker GetById(string id);

        void Create(Broker broker);

        void Update(Broker broker);

        void Attach(Broker broker);


        bool CreateBroker(BrokerModel brokerModel);

        Broker IsBrokerExist(string brokerNIC);

        void AddRange(IEnumerable<Broker> brokers);
    }
}

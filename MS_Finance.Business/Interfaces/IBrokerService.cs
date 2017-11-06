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
        bool CreateBroker(BrokerModel brokerModel);

        Broker IsBrokerExist(string brokerNIC);
    }
}

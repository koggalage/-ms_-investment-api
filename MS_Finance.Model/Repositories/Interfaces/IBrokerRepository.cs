using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Model.Repositories.Interfaces
{
    public interface IBrokerRepository
    {
        void CreateBroker(Broker broker);

        Broker GetSingle(string id);

        Broker GetBrokerByNIC(string nic);
    }
}

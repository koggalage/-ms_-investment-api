using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Exceptions
{
    public class ContractServiceException : Exception
    {
        public ContractServiceException()
            :base()
        {

        }

        public ContractServiceException(string message)
            :base(message)
        {

        }
    }
}

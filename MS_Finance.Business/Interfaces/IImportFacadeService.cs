using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Interfaces
{
    public interface IImportFacadeService
    {

        void ImportCustomers(string filePath);

        void ImportGuarentors(string filePath);

        void ImportBrokers(string filePath);

        void ImportContracts(string filePath);

        void ImportInstallments(string filePath);
    }
}

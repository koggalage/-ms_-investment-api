using ExcelDataReader;
using MS_Finance.Business.Interfaces;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS_Finance.Business.Services
{
    public class ImportFacadeService : IImportFacadeService
    {
        protected ICustomerService CustomerService;
        protected IContractsService ContractsService;
        protected IBrokerService BrokerService;
        protected IGuarantorService GuarantorService;
        protected IUnitOfWork UoW;
        protected IInstalmentService InstallmentService;

        public ImportFacadeService(ICustomerService CustomerService, 
            IContractsService ContractsService, 
            IBrokerService BrokerService, 
            IGuarantorService GuarantorService, 
            IUnitOfWork UoW,
            IInstalmentService InstallmentService)
        {
            this.CustomerService    = CustomerService;
            this.ContractsService   = ContractsService;
            this.BrokerService      = BrokerService;
            this.GuarantorService   = GuarantorService;
            this.UoW                = UoW;
            this.InstallmentService = InstallmentService;
        }


        public void ImportCustomers(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            try
            {
                var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                var xlCustomers = result.Tables["Guarentors"];
                var listCustomers = GetCustomers(xlCustomers);

                CustomerService.AddRange(listCustomers);

            }
            finally
            {
                excelReader.Close();
            }

        }

        public void ImportGuarentors(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            try
            {
                var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                var xlGuarentors = result.Tables["Guarentors"];
                var listGuarentors = GetGuarentors(xlGuarentors);

                GuarantorService.AddRange(listGuarentors);
            }
            finally
            {
                excelReader.Close();
            }
        }

        public void ImportBrokers(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            try
            {
                var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                var xlBrokers = result.Tables["Brokers"];
                var listBrokers = GetBrokers(xlBrokers);

                BrokerService.AddRange(listBrokers);
            }
            finally
            {
                excelReader.Close();
            }
        }

        public void ImportContracts(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            try
            {
                var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                var xlContracts = result.Tables["Contracts"];
                var listContracts = GetContracts(xlContracts);

                UoW.Contracts.AddRange(listContracts);
                UoW.Commit();
            }
            finally
            {
                excelReader.Close();
            }
        }

        public void ImportInstallments(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            try
            {
                var result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                var xlInstalments = result.Tables["Instalments"];
                var listContractInstallments = GetContractInstallments(xlInstalments);

                UoW.ContractInstallments.AddRange(listContractInstallments);
                UoW.Commit();
            }
            finally
            {
                excelReader.Close();
            }
        }




        private IEnumerable<Customer> GetCustomers(DataTable xlsCustomers)
        {
            var table1Reader = xlsCustomers.CreateDataReader();
            var customers = new List<Customer>();

            do
            {
                while (table1Reader.Read())
                {

                    var existingCustomer = CustomerService.IsCustomerExist(table1Reader.GetString(1).Trim());

                    if (existingCustomer != null)
                        continue;

                    var customer = new Customer()
                    {
                        Name = table1Reader.GetString(0),
                        NIC = table1Reader.GetString(1),
                        CreatedDate = table1Reader.GetDateTime(2),
                        Address = table1Reader.GetString(3),
                        MobileNumber = table1Reader.GetDouble(4).ToString(),
                        Occupation = table1Reader.GetString(5),
                        Description = table1Reader.GetString(6)
                    };


                    customers.Add(customer);
                }
            } while (table1Reader.NextResult());

            return customers.GroupBy(c => c.NIC).Select(v => v.First()).ToList();
        }

        private IEnumerable<Guarantor> GetGuarentors(DataTable xlsGuarentors)
        {
            var table1Reader = xlsGuarentors.CreateDataReader();

            var guarentors = new List<Guarantor>();

            do
            {
                while (table1Reader.Read())
                {

                    var existingGuarantor = GuarantorService.IsGuarantorExist(table1Reader.GetString(0).Trim());
                    if (existingGuarantor != null)
                        continue;

                    guarentors.Add(new Guarantor()
                    {
                        NIC         = table1Reader.GetString(0),
                        Name        = table1Reader.GetString(1),
                        CreatedDate = table1Reader.GetDateTime(2),
                        Address     = table1Reader.GetString(3),
                        ContactNo   = table1Reader.GetDouble(4).ToString(),
                        Occupation  = table1Reader.GetString(5),
                        Description = table1Reader.GetString(6)
                    });
                }
            } while (table1Reader.NextResult());

            return guarentors.GroupBy(x => x.NIC).Select(v => v.First()).ToList();
        }

        private IEnumerable<Broker> GetBrokers(DataTable xlsBrokers)
        {
            var table1Reader = xlsBrokers.CreateDataReader();

            var brokers = new List<Broker>();

            do
            {
                while (table1Reader.Read())
                {
                    var existingBroker = BrokerService.IsBrokerExist(table1Reader.GetString(1).Trim());
                    if (existingBroker != null)
                        continue;

                    brokers.Add(new Broker()
                    {
                        Name        = table1Reader.GetString(0),
                        NIC         = table1Reader.GetString(1),
                        CreatedDate = table1Reader.GetDateTime(2),
                        Address     = table1Reader.GetString(3),
                        ContactNo   = table1Reader.GetDouble(4).ToString(),
                        Occupation  = table1Reader.GetString(5),
                        Description = table1Reader.GetString(6)
                    });
                }
            } while (table1Reader.NextResult());

            return brokers.GroupBy(c => c.NIC).Select(c => c.First()).ToList();
        }

        private IEnumerable<Contract> GetContracts(DataTable xlsContract)
        {
            var table1Reader = xlsContract.CreateDataReader();

            var contracts = new List<Contract>();

            do
            {
                while (table1Reader.Read())
                {
                    var customer = CustomerService.IsCustomerExist(table1Reader.GetString(7));
                    var existingOpenContract = ContractsService.GetAll().Where(x => x.Customer.Id == customer.Id && x.IsOpen).FirstOrDefault();
                    if (existingOpenContract != null)
                        continue;

                    var brokerNic = table1Reader.GetString(6).Trim();
                    var customerNic = table1Reader.GetString(7).Trim();
                    var guarantorNic = table1Reader.GetString(8).Trim();

                    contracts.Add(new Contract()
                    {
                        Amount                  = Convert.ToDecimal(table1Reader.GetDouble(0)),
                        NoOfInstallments        = Convert.ToInt32(table1Reader.GetDouble(1)),
                        Insallment              = Convert.ToDecimal(table1Reader.GetDouble(2)),
                        VehicleNo               = table1Reader.GetString(3),
                        IsOpen                  = Convert.ToBoolean(table1Reader.GetDouble(4)),
                        LicenceExpireDate       = table1Reader.GetDateTime(5),
                        Broker                  = UoW.Brokers.GetAll().Where(c => c.NIC == brokerNic).FirstOrDefault(),
                        Customer                = UoW.Customers.GetAll().Where(c => c.NIC == customerNic).FirstOrDefault(),
                        Guarantor               = UoW.Guarantors.GetAll().Where(c => c.NIC == guarantorNic).FirstOrDefault(),
                        CreatedOn               = table1Reader.GetDateTime(9),
                        DocumentCharge          = Convert.ToDecimal(table1Reader.GetDouble(10)),
                        Description             = table1Reader.GetString(11),
                        ContractNo              = GenerateContractNumber(table1Reader.GetString(12), table1Reader.GetDateTime(9))
                    });
                }
            } while (table1Reader.NextResult());

            return contracts;
        }

        private IEnumerable<ContractInstallment> GetContractInstallments(DataTable xlsContractInstallment)
        {
            var table1Reader = xlsContractInstallment.CreateDataReader();

            var contractInstallments = new List<ContractInstallment>();


            do
            {
                while (table1Reader.Read())
                {
                    var customerNic = table1Reader.GetString(0).Trim();
                    var vehicleNo = table1Reader.GetString(1).Trim();
                    var dueDate = table1Reader.GetDateTime(4);

                    var customer = CustomerService.IsCustomerExist(customerNic);
                    var contract = UoW.Contracts.GetAll().Where(x => x.Customer.Id == customer.Id && x.VehicleNo == vehicleNo).FirstOrDefault();
                    if (contract == null)
                        continue;

                    var existingIstalment = InstallmentService.GetAll().Where(x => x.Contract.Id == contract.Id && x.DueDate == dueDate).FirstOrDefault();
                    if (existingIstalment != null)
                        continue;

                    var installment = new ContractInstallment()
                    {
                        PaidAmount      = Convert.ToDecimal(table1Reader.GetDouble(2)),
                        UnsettleAmount  = Convert.ToDecimal(table1Reader.GetDouble(3)),
                        DueDate         = table1Reader.GetDateTime(4),
                        LateDays        = Convert.ToInt32(table1Reader.GetDouble(6)),
                        Fine            = Convert.ToDecimal(table1Reader.GetDouble(7)),
                        Paid            = Convert.ToInt32(table1Reader.GetDouble(8)),
                        Contract        = contract,
                        Approved        = true
                    };

                    if(!table1Reader.IsDBNull(5))
                        installment.PaidDate = table1Reader.GetDateTime(5);

                    contractInstallments.Add(installment);
                }
            } while (table1Reader.NextResult());

            return contractInstallments;
        }

        private string GenerateContractNumber(string code, DateTime year)
        {
            var currentYear = year.Year.ToString();
            var lastGeneratedNumber =
                ContractsService.GetAll()
                .AsEnumerable()
                .Where(x => x.ContractNo != null && x.ContractNo.StartsWith(code) && x.ContractNo.Split(new char[] { '-' })[1] == currentYear)
                .OrderByDescending(c => c.ContractNo.Split(new char[] { '-' })[2])
                .Select(c => c.ContractNo)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(lastGeneratedNumber))
                return string.Format("{0}-{1}-{2}", code, currentYear, "1");

            var newNumber = string.Format("{0}-{1}-{2}", code, currentYear, (int.Parse(lastGeneratedNumber.Split(new char[] { '-' })[2]) + 1).ToString());

            return newNumber;
        }

    }
}

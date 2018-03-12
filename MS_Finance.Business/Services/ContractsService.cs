using MS_Finance.Business.Exceptions;
using MS_Finance.Business.Interfaces;
using MS_Finance.Business.Models.EnumsAndConstants;
using MS_Finance.Business.Services;
using MS_Finance.Model.Models;
using MS_Finance.Model.Repositories.Interfaces;
using MS_Finance.Model.Repositories.OA;
using MS_Finance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MS_Finance.Services
{
    public class ContractsService : DefaultPersistentService<Contract>, IContractsService
    {
        protected ICustomerService CustomerService;
        protected IBrokerService BrokerService;
        protected IGuarantorService GuarantorService;
        protected IContractRateService ContractRateService;

        public ContractsService(IUnitOfWork UoW,
            ICustomerService CustomerService,
            IBrokerService BrokerService,
            IGuarantorService GuarantorService,
            IContractRateService ContractRateService)
            : base(UoW)
        {
            this.CustomerService = CustomerService;
            this.BrokerService = BrokerService;
            this.GuarantorService = GuarantorService;
            this.ContractRateService = ContractRateService;
        }


        public IQueryable<Contract> GetAll()
        {
            return base
                .GetAll();
        }

        public IQueryable<Contract> GetAllWithIncludes(params Expression<Func<Contract, object>>[] properties)
        {
            return base.GetAllWithIncludes(properties);
        }

        public Contract GetById(string id)
        {
            return base.GetSingle(x => x.Id == id);
        }

        public void Create(Contract contract)
        {
            base.Add(contract);
        }

        public void Update(Contract contract)
        {
            base.Update(contract);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public GetCustomerDetailsVM GetCustomerDetailsModel()
        {
            var customersList = CustomerService.GetAll();

            var Model = new GetCustomerDetailsVM();

            Model.CustomerDetails = new List<CustomerModel>();
            foreach (var customer in customersList)
            {
                Model.CustomerDetails.Add(new CustomerModel()
                {
                    Name = customer.Name,
                    NIC = customer.NIC
                });
            }

            return Model;
        }

        public List<SearchOptionsModel> GetContractsBySearchTerm(string searchTerm)
        {
            searchTerm = !string.IsNullOrEmpty(searchTerm) ? searchTerm.ToLower() : string.Empty;

            var result = (from a in base.GetAll()
                          where (a.VehicleNo.ToLower() == searchTerm || a.Customer.NIC.ToLower() == searchTerm || a.Customer.Name.ToLower().Contains(searchTerm) || a.Customer.MobileNumber == searchTerm || (!string.IsNullOrEmpty(a.ContractNo) && a.ContractNo.ToLower().Equals(searchTerm)))
                          select new SearchOptionsModel { VehicleNumber = a.VehicleNo, Name = a.Customer.Name, NIC = a.Customer.NIC, ContractId = a.Id })
                         .ToList();

            return result;
        }

        public Contract CreateContract(ContractModel contractModel)
        {
            var customer = UoW.Customers.GetSingle(x => x.Id == contractModel.CustomerId);
            var broker = UoW.Brokers.GetSingle(x => x.Id == contractModel.BrokerId);
            var guarantor = UoW.Guarantors.GetSingle(x => x.Id == contractModel.GuarantorId);

            var contract = new Contract()
            {
                ContractNo          = contractModel.ContractNo,
                Amount              = contractModel.Amount,
                DocumentCharge      = (GetRate((int)ContractRateType.DocumentCharges, DateTime.Now) * contractModel.Amount),
                NoOfInstallments    = contractModel.NoOfInstallments,
                Insallment          = contractModel.Insallment,
                Type                = contractModel.Type,
                VehicleNo           = contractModel.VehicleNo,
                IsOpen              = true,
                CreatedByUserId     = contractModel.CreatedByUserId,
                CreatedByUserName   = contractModel.CreatedByUserName,
                LicenceExpireDate   = DateTime.Now.AddYears(2),
                CreatedOn           = DateTime.Now,
                Customer            = customer,
                Broker              = broker,
                Guarantor           = guarantor,
                Description         = contractModel.Description
            };

            if (CustomerHasRunningContract(customer.Id))
                throw new ContractServiceException("Customer already has running contract");

            var emptyInstalments = CreateEmptyInstalmentsForContract(contractModel, contract.Id);
            contract.ContractInstallments = emptyInstalments;
            UoW.Contracts.Add(contract);
            UoW.Commit();

            return contract;
        }

        private bool CustomerHasRunningContract(string customerId)
        {
            var contract = this.GetAll().Where(x => x.Customer.Id == customerId && x.IsOpen).FirstOrDefault();
            return contract != null ? contract.IsOpen : false;
        }

        private List<ContractInstallment> CreateEmptyInstalmentsForContract(ContractModel contractModel, string contractId)
        {

            var noOfInstalments = contractModel.NoOfInstallments;
            var instalments = new List<ContractInstallment>();
            var dueDate = DateTime.Now.AddMonths(1);

            while (noOfInstalments > 0)
            {
                var instalment = new ContractInstallment()
                {
                    DueDate             = dueDate,
                    Paid                = (int)InstalmentPaymentStatus.NotPaid,
                    CreatedByUserId     = contractModel.CreatedByUserId,
                    CreatedByUserName   = contractModel.CreatedByUserName
                };

                dueDate = dueDate.AddMonths(1);
                instalments.Add(instalment);
                noOfInstalments--;
            }

            return instalments;
        }

        public GetBrokerDetailsVM GetBrokersModel()
        {
            var brokersList = BrokerService.GetAll();

            var Model = new GetBrokerDetailsVM();

            Model.BrokerDetails = new List<BrokerModel>();
            foreach (var broker in brokersList)
            {
                Model.BrokerDetails.Add(new BrokerModel()
                {
                    Name = broker.Name,
                    NIC = broker.NIC
                });
            }

            return Model;
        }

        public List<ContractModel> GetActiveContracts()
        {
            return base.GetAll().Where(x => x.IsOpen).Select(x => new ContractModel()
            {
                Id              = x.Id,
                Amount          = x.Amount,
                VehicleNo       = x.VehicleNo,
                CustomerName    = x.Customer != null ? x.Customer.Name : string.Empty,
                ContractNo      = x.Customer != null ? x.Customer.MobileNumber : string.Empty,
            }).ToList();
        }

        public ContractReportModel GetOpenOrClosedContracts(bool open = true)
        {
            var model = new ContractReportModel();


            var contracts = base.GetAll().Where(x => x.IsOpen == open).Select(x => new ContractModel()
            {
                Id = x.Id,
                Amount = x.Amount,
                VehicleNo = x.VehicleNo,
                CustomerName = x.Customer != null ? x.Customer.Name : string.Empty,
                ContractNo = x.Customer != null ? x.Customer.MobileNumber : string.Empty,
            }).OrderBy(c => c.CustomerName).ToList();

            model.Contracts = contracts;
            model.NoOfContracts = contracts.Count;

            return model;
        }


        public List<Customer> GetCustomersForOpenContractsModel()
        {
            var result = (from a in base.GetAll()
                          where a.IsOpen == true
                          select a.Customer)
              .GroupBy(a => a.Id)
              .Select(s => s.FirstOrDefault())
              .ToList();


            return result;
        }

        public List<ContractModel> GetVehicleNoByCustomerIdModel(string customerId)
        {
            return base.GetAll().Where(c => c.Customer.Id == customerId && c.IsOpen)
                .Select(x => new ContractModel()
                {
                    Id = x.Id,
                    VehicleNo = x.VehicleNo
                }).ToList();
        }


        public decimal GetMonthlyInstallmentModel(decimal Amount, int NoOfInstallments)
        {
            //double interestRate = (NoOfInstallments <= 6) ? 0.30 : (NoOfInstallments > 6) ? 0.36 : double.NaN;
            var rate = NoOfInstallments <= 6 ? GetRate((int)ContractRateType.InterestForShortTerm, DateTime.Now) : GetRate((int)ContractRateType.InterestForLongTerm, DateTime.Now);

            decimal interestRate = rate;
            decimal Insallment = ((Amount * Convert.ToDecimal(interestRate)) + Amount) / NoOfInstallments;
            Insallment = Math.Round(Insallment, 2);
            return Insallment;
        }

        public decimal GetDocumentCharge(decimal amount)
        {
            return amount * GetRate((int)ContractRateType.DocumentCharges, DateTime.Now);
        }

        public int GetRunningContractsCount(DateTime from, DateTime to)
        {
            return this.GetAll().Where(x => x.IsOpen).Count();
        }

        public void UploadContractFiles(string contractId, string imageName, string createdByUserId, string createdByUserName)
        {
            //string localhostPath = "http://localhost:53438/ContractFiles/";

            var contract = UoW.Contracts.GetSingle(x => x.Id == contractId);

            var contractFile = new ContractFile()
            {
                Contract = contract,
                FilePath = imageName,
                CreatedByUserId = createdByUserId,
                CreatedByUserName = createdByUserName,
                CreatedOn = DateTime.Now
            };

            UoW.ContractFiles.Add(contractFile);
            UoW.Commit();
            //_fileUploadService.
        }

        public decimal GetRate(int type, DateTime validFor)
        {
            var historicalRate = ContractRateService.GetAll().Where(x => x.Type == type && validFor >= x.ValidFrom && validFor <= x.ValidUntil).FirstOrDefault();

            if (historicalRate != null)
                return historicalRate.Value / 100;

            var currentRate = ContractRateService.GetAll().Where(x => x.Type == type && validFor >= x.ValidFrom && !x.ValidUntil.HasValue).FirstOrDefault();

            if (currentRate != null)
                return currentRate.Value / 100;

            throw new ContractServiceException("No rate has been define");
        }


        public DocumentChargeRecordModel GetDocumentChargeReport(DateTime from, DateTime to)
        {
            var payments = this.GetAll()
                            .Where(x => x.CreatedOn >= from && x.CreatedOn <= to)
                            .Select(c => new DocumentChargeRecord() { VehicleNo = c.VehicleNo, Customer = c.Customer.Name, Amount = c.DocumentCharge, PaidDate = c.CreatedOn })
                            .ToList();

            var result = new DocumentChargeRecordModel()
            {
                DocChargeRecords = payments,
                Total            = payments.Select(x => x.Amount).ToList().Sum()
            };

            return result;
        }

        public string GenerateContractNumber(string code)
        {
            var currentYear = DateTime.Now.Year.ToString();
            var lastGeneratedNumber = 
                this.GetAll()
                .AsEnumerable()
                .Where(x => x.ContractNo != null && x.ContractNo.StartsWith(code) && x.ContractNo.Split(new char[] { '-' })[1] == currentYear)
                .OrderByDescending(c => c.ContractNo.Split(new char[] { '-' })[2])
                .Select(c => c.ContractNo)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(lastGeneratedNumber))
                return string.Format("{0}-{1}-{2}", code, currentYear, "1");

            var newNumber = string.Format("{0}-{1}-{2}", code, currentYear,(int.Parse(lastGeneratedNumber.Split(new char[] { '-' })[2]) + 1).ToString());

            return newNumber;
        }

        public void AddRange(IEnumerable<Contract> contracts)
        {
            base.AddRange(contracts);
        }

    }
}
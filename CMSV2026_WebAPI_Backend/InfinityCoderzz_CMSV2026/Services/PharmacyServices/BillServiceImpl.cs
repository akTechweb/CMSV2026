using InfinityCoderzz_CMSV2026.Models.pharmacist;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices
{
    public class BillServiceImpl : IPharmacyBillService
    {
        private readonly IPharmacyBillRepository _billRepository;

        public BillServiceImpl(IPharmacyBillRepository billRepository)
        {
            _billRepository = billRepository;
        }

        public Task<IEnumerable<BillViewModel>>     GetAllBills()                                          => _billRepository.GetAllBills();
        public Task<IEnumerable<PatientLookup>>     GetPatients()                                          => _billRepository.GetPatients();
        public Task<IEnumerable<MedicineLookup>>    GetMedicinesForBilling()                               => _billRepository.GetMedicinesForBilling();
        public Task<int>                             CreateBill(CreateBillViewModel model, int staffId)    => _billRepository.CreateBill(model, staffId);
        public Task<BillViewModel?>                  GetBillById(int billId)                               => _billRepository.GetBillById(billId);
        public Task<IEnumerable<BillItemViewModel>> GetBillItems(int billId)                               => _billRepository.GetBillItems(billId);
        public Task<BillPrescriptionLink?>           GetBillPrescriptionLink(int billId)                   => _billRepository.GetBillPrescriptionLink(billId);
        public Task                                  CancelBill(int billId, int staffId, string? reason)  => _billRepository.CancelBill(billId, staffId, reason);
    }
}

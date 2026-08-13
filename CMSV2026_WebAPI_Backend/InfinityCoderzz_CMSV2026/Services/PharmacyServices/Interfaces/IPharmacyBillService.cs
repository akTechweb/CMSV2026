using InfinityCoderzz_CMSV2026.Models.pharmacist;

namespace InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces
{
    public interface IPharmacyBillService
    {
        Task<IEnumerable<BillViewModel>> GetAllBills();
        Task<IEnumerable<PatientLookup>> GetPatients();
        Task<IEnumerable<MedicineLookup>> GetMedicinesForBilling();
        Task<int> CreateBill(CreateBillViewModel model, int staffId);
        Task<BillViewModel?> GetBillById(int billId);
        Task<IEnumerable<BillItemViewModel>> GetBillItems(int billId);
        Task<BillPrescriptionLink?> GetBillPrescriptionLink(int billId);
        Task CancelBill(int billId, int staffId, string? reason);
    }
}

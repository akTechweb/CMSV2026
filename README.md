# InfinityCoderzz CMS — Clinic Management System

> A full-stack, role-based Clinic Management System connecting Reception, Doctor, Laboratory, and Pharmacy workflows.

## 📌 Project at a Glance

InfinityCoderzz CMS brings four operational areas into one connected workflow:

```text
Receptionist → Doctor → Laboratory
                     ↓
                  Pharmacy
                     ↓
              Billing / Reports
```

| Module | Screens documented | Key workflows |
|---|---:|---|
| 🛎️ Receptionist | 17 | Patient registration, appointments, billing, reports, visits, patient directory |
| 🩺 Doctor | 8 | Appointment queue, consultation, lab orders, prescriptions, reports, history |
| 🧪 Laboratory | 10 | Pending tests, result entry, billing, completed reports, patient search |
| 💊 Pharmacy | 10 sub-modules / multiple UI states | Medicines, stock, prescriptions, FEFO dispensing, billing, reports, audit/inventory |

The module presentations supplied with the project document 17 Receptionist screens, 8 Doctor screens, 10 Laboratory screens, and 10 Pharmacy sub-modules. fileciteturn3file3 fileciteturn3file4 fileciteturn3file5

---

# 🧭 End-to-End Workflow

```text
┌──────────────────┐
│   RECEPTIONIST   │
│ Register Patient │
│ Book Appointment │
│ Billing          │
└────────┬─────────┘
         ↓
┌──────────────────┐
│      DOCTOR      │
│ Consultation     │
│ Lab Orders       │
│ Prescription     │
└───────┬───┬──────┘
        │   │
   Lab  │   │ Prescription
        ↓   ↓
┌──────────┐ ┌────────────┐
│   LAB    │ │  PHARMACY  │
│ Results  │ │ FEFO       │
│ Billing  │ │ Dispensing  │
└────┬─────┘ │ Billing    │
     │       └─────┬──────┘
     └───────┬─────┘
             ↓
       Reports / History
```

---

# 🔐 Shared Authentication

The application uses a shared, role-aware login flow. The Pharmacy architecture documentation describes session-based authentication and role validation before redirecting the user to the appropriate dashboard. fileciteturn3file5

![Shared Role-Based Login](docs/screenshots/shared/login.png)

---

# 🛎️ Receptionist Module

**17 documented screens · `/reception`**

The complete Receptionist screenshot sequence is included below. The dedicated Receptionist module presentation explicitly labels the workflow as 17 steps. fileciteturn3file3

<details open>
<summary><strong>View all 17 Receptionist screens</strong></summary>

### 01 — Dashboard
![Receptionist Dashboard](docs/screenshots/receptionist/01-dashboard.png)

### 02 — Quick Patient Search
![Quick Patient Search](docs/screenshots/receptionist/02-quick-patient-search.png)

### 03 — Find Patient for Appointment
![Find Patient](docs/screenshots/receptionist/03-find-patient.png)

### 04 — Choose Appointment Slot
![Choose Slot](docs/screenshots/receptionist/04-choose-slot.png)

### 05 — Appointment Booked Confirmation
![Appointment Confirmation](docs/screenshots/receptionist/05-appointment-confirmation.png)

### 06 — Patient Registration: Personal Details
![Registration Personal Details](docs/screenshots/receptionist/06-registration-personal.png)

### 07 — Patient Registration: Contact Details
![Registration Contact Details](docs/screenshots/receptionist/07-registration-contact.png)

### 08 — Registration Confirmation
![Registration Confirmation](docs/screenshots/receptionist/08-registration-confirmation.png)

### 09 — Appointment for New Patient
![New Patient Appointment](docs/screenshots/receptionist/09-new-patient-appointment.png)

### 10 — Second Appointment Booked
![Second Appointment](docs/screenshots/receptionist/10-second-appointment.png)

### 11 — Create Bill from Appointment
![Create Bill](docs/screenshots/receptionist/11-create-bill.png)

### 12 — Bill Details & Payment History
![Bill Details](docs/screenshots/receptionist/12-bill-payment-history.png)

### 13 — Printable Invoice
![Printable Invoice](docs/screenshots/receptionist/13-printable-invoice.png)

### 14 — Reports Dashboard
![Reports Dashboard](docs/screenshots/receptionist/14-reports-dashboard.png)

### 15 — Patient Visits Log
![Patient Visits](docs/screenshots/receptionist/15-patient-visits.png)

### 16 — Patients Directory
![Patients Directory](docs/screenshots/receptionist/16-patients-directory.png)

### 17 — Patient Edit & Audit
![Patient Edit and Audit](docs/screenshots/receptionist/17-patients-edit-audit.png)

</details>

### Receptionist workflow

```text
Register Patient
      ↓
Book Appointment
      ↓
Appointment Queue
      ↓
Create Bill
      ↓
Payment / Invoice
      ↓
Reports / Visits / Patient Directory
```

---

# 🩺 Doctor Module

**8 documented screens · `/doctor`**

The Doctor presentation documents an eight-step workflow from dashboard and appointment queue through consultation, lab/prescription entry, summary, PDF report, and patient history. fileciteturn3file4

<details open>
<summary><strong>View all 8 Doctor screens</strong></summary>

### 01 — Doctor Dashboard
![Doctor Dashboard](docs/screenshots/doctor/01-dashboard.png)

### 02 — Appointments Queue
![Appointments Queue](docs/screenshots/doctor/02-appointments-queue.png)

### 03 — Consultation: Patient Intake
![Consultation Intake](docs/screenshots/doctor/03-consultation-intake.png)

### 04 — Lab Tests & Prescription
![Lab Tests and Prescription](docs/screenshots/doctor/04-lab-tests-prescription.png)

### 05 — Consultation Summary
![Consultation Summary](docs/screenshots/doctor/05-consultation-summary.png)

### 06 — Downloadable Consultation Report
![Consultation PDF](docs/screenshots/doctor/06-consultation-pdf.png)

### 07 — Patient History Search
![Patient History Search](docs/screenshots/doctor/07-patient-history-search.png)

### 08 — Patient History & Lab Results
![Patient History Lab Results](docs/screenshots/doctor/08-patient-history-lab-results.png)

</details>

### Doctor workflow

```text
Appointments
     ↓
Consultation
   ┌─┴───────────┐
   ↓             ↓
Lab Orders   Prescription
   ↓             ↓
Laboratory    Pharmacy
```

---

# 🧪 Laboratory / Lab Technician Module

**10 documented screens · `/lab`**

The Lab section below uses the **actual Lab Technician application screenshots supplied in this conversation**, plus the login screen needed to complete the documented 10-screen flow. fileciteturn3file5

<details open>
<summary><strong>View all 10 Laboratory screens</strong></summary>

### 01 — Lab Technician Login
![Lab Login](docs/screenshots/lab/10-login.png)

### 02 — Lab Dashboard
![Lab Dashboard](docs/screenshots/lab/01-dashboard.jpeg)

### 03 — Pending Tests
![Pending Tests](docs/screenshots/lab/02-pending-tests.jpeg)

### 04 — Enter Lab Result
![Enter Lab Result](docs/screenshots/lab/03-enter-result.jpeg)

### 05 — Lab Billing: Unbilled Requests
![Lab Billing Unbilled](docs/screenshots/lab/04-billing-unbilled.jpeg)

### 06 — Lab Billing: Payment State
![Lab Billing Payment State](docs/screenshots/lab/05-billing-paid-state.jpeg)

### 07 — Lab Bill Detail
![Lab Bill Detail](docs/screenshots/lab/06-billing-detail.jpeg)

### 08 — Completed Reports
![Completed Reports](docs/screenshots/lab/07-completed-reports.jpeg)

### 09 — Laboratory Report Detail
![Laboratory Report Detail](docs/screenshots/lab/08-report-detail.jpeg)

### 10 — Patient Search
![Lab Patient Search](docs/screenshots/lab/09-patient-search.png)

</details>

### Laboratory workflow

```text
Doctor Orders Test
       ↓
Pending Tests
       ↓
Enter Result
       ↓
Completed Report
       ↓
Lab Billing
       ↓
Patient Search / Report Lookup
```

---

# 💊 Pharmacy Module

**10 documented sub-modules · `/pharmacy`**

The Pharmacy presentation describes the complete medication lifecycle: authentication, dashboard, medicines, stock, prescriptions, dispensing, billing, reports, inventory log, and audit log. fileciteturn3file5

<details open>
<summary><strong>View all Pharmacy UI screenshots</strong></summary>

## Dashboard
![Pharmacy Dashboard](docs/screenshots/pharmacy/01-dashboard.png)

## Medicine Catalogue
![Medicine Catalogue](docs/screenshots/pharmacy/02-medicine-catalogue.png)

## Medicine Management — Add
![Add Medicine](docs/screenshots/pharmacy/03-add-medicine.png)

## Medicine Management — Edit
![Edit Medicine](docs/screenshots/pharmacy/04-edit-medicine.png)

## Stock Inventory
![Stock Inventory](docs/screenshots/pharmacy/05-stock-inventory.png)

## Stock Management — Add Batch
![Add Stock Batch](docs/screenshots/pharmacy/06-add-stock-batch.png)

## Stock Management — Edit Batch
![Edit Stock Batch](docs/screenshots/pharmacy/07-edit-stock-batch.png)

## Prescription List
![Prescription List](docs/screenshots/pharmacy/08-prescription-list.png)

## Prescription Detail
![Prescription Detail](docs/screenshots/pharmacy/09-prescription-detail.png)

## Dispensing Queue
![Dispensing Queue](docs/screenshots/pharmacy/10-dispensing-queue.png)

## Confirm Dispense & Bill
![Confirm Dispense and Bill](docs/screenshots/pharmacy/11-confirm-dispense-bill.png)

## Dispensing History
![Dispensing History](docs/screenshots/pharmacy/12-dispensing-history.png)

## OTC Pharmacy Bill
![OTC Pharmacy Bill](docs/screenshots/pharmacy/13-otc-pharmacy-bill.png)

## Bill Detail
![Bill Detail](docs/screenshots/pharmacy/14-bill-detail.png)

## PDF Invoice
![PDF Invoice](docs/screenshots/pharmacy/15-pdf-invoice.png)

## Reports Dashboard
![Reports Dashboard](docs/screenshots/pharmacy/16-reports-dashboard.png)

## Audit Log
![Audit Log](docs/screenshots/pharmacy/17-audit-log.png)

## Inventory Log
![Inventory Log](docs/screenshots/pharmacy/18-inventory-log.png)

</details>

### Pharmacy workflow

```text
Prescription
     ↓
Stock Check
     ↓
FEFO Deduction
     ↓
Medicine Dispensing
     ↓
Bill Creation
     ↓
PDF Invoice
     ↓
Reports / Inventory Log / Audit Log
```

---

# ⭐ Key Engineering Features

## FEFO — First Expired, First Out

The Pharmacy module uses expiry-aware stock selection so earlier-expiring batches are consumed first.

## Atomic Dispensing Transaction

The documented Pharmacy dispensing flow chains the database operations for dispensing, stock deduction, bill creation, bill items, prescription status, and prescription/bill linking inside a transaction. The presentation states that a failure rolls the transaction back. fileciteturn3file5

## Auditability

The Pharmacy module includes inventory and audit logs for stock movement and pharmacist activity. fileciteturn3file5

## PDF Generation

The project uses QuestPDF for generated invoices/reports.

## Reporting

The project includes operational dashboards, reports, CSV export, and Chart.js visualizations.

---

# 🧱 Architecture

```text
Angular Frontend
      ↓
ASP.NET Core Web API
      ↓
Service Layer
      ↓
Repository Layer
      ↓
Dapper / SQL Operations
      ↓
Microsoft SQL Server
```

The supplied Pharmacy architecture documentation describes a five-layer design around Angular, ASP.NET Core controllers, services, repositories, and SQL Server stored procedures. fileciteturn3file5

---

# 🛠️ Technology Stack

| Layer | Technology |
|---|---|
| Frontend | Angular / TypeScript |
| UI | Bootstrap / responsive components |
| Reactive | RxJS |
| Charts | Chart.js |
| Backend | ASP.NET Core Web API / C# |
| Data Access | Dapper / SQL Server stored procedures |
| Database | Microsoft SQL Server |
| Authentication | Session-based role authentication |
| Documents | QuestPDF |
| Reporting | CSV export + dashboards |

---

# 📁 Repository Layout

```text
CMSV2026/
├── README.md
├── docs/
│   └── screenshots/
│       ├── shared/
│       ├── receptionist/
│       ├── doctor/
│       ├── lab/
│       └── pharmacy/
├── frontend/
├── backend/
└── database/
```

---

# 🔒 Public GitHub Security Checklist

Before publishing/maintaining the repository publicly:

- [ ] Remove database passwords from committed configuration
- [ ] Remove SMTP passwords / app passwords
- [ ] Rotate credentials that were previously committed
- [ ] Use environment variables or .NET User Secrets
- [ ] Add local configuration to `.gitignore`
- [ ] Remove `node_modules`, `bin`, `obj`, `.vs`, and generated output
- [ ] Check Git history for secrets
- [ ] Enable GitHub secret scanning / push protection where available

> **Never commit production credentials, database passwords, API keys, SMTP credentials, or authentication tokens.**

---

# 👨‍💻 InfinityCoderzz — CMS V2026

`Angular` · `ASP.NET Core` · `C#` · `SQL Server` · `Dapper` · `QuestPDF` · `Chart.js`

---

## Screenshot coverage

This repository package includes the complete documented screen sequence for Receptionist and Doctor, the 10-screen Laboratory flow using the Lab Technician screenshots supplied in the conversation, and the full set of meaningful Pharmacy UI screenshots from the supplied project walkthrough.

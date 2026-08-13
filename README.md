# InfinityCoderzz CMS --- Clinic Management System

> A full-stack clinic management platform connecting **Receptionist,
> Doctor, Laboratory, and Pharmacy** workflows in one system.

## 🚀 Project Overview

InfinityCoderzz CMS is a role-based healthcare operations platform
covering patient registration, appointments, clinical consultation,
laboratory processing, prescriptions, pharmacy inventory, FEFO
dispensing, billing, PDF documents, reporting, and auditability.

### Core modules

  -----------------------------------------------------------------------
  Module                              Scope
  ----------------------------------- -----------------------------------
  🛎️ Receptionist                     Registration, appointments,
                                      billing, reports, visits, patient
                                      directory

  🩺 Doctor                           Appointment queue, consultation,
                                      lab orders, prescriptions, reports,
                                      history

  🧪 Laboratory                       Pending tests, result entry, lab
                                      billing, completed reports, patient
                                      search

  💊 Pharmacy                         Medicines, stock, prescriptions,
                                      FEFO dispensing, billing, reports,
                                      logs
  -----------------------------------------------------------------------

------------------------------------------------------------------------

## 🔄 End-to-End Workflow

``` text
Patient Registration
        ↓
Appointment Booking
        ↓
Doctor Consultation
   ┌────┴───────────┐
   ↓                ↓
Lab Test Order    Prescription
   ↓                ↓
Lab Result       Pharmacy Dispensing
   └────┬───────────┘
        ↓
     Billing
        ↓
Reports / History / Audit
```

------------------------------------------------------------------------

# 🔐 Authentication & Role-Based Access

The application uses role-based access for the operational users. The
shared authentication screen is shown below.

`<img src="docs/screenshots/shared/authentication.png" alt="Authentication" width="100%">`{=html}

**Roles:** Receptionist · Doctor · Lab Technician / Lab Admin ·
Pharmacist

------------------------------------------------------------------------

# 🛎️ Receptionist Module

The Receptionist module covers the complete front-desk workflow from
patient registration and appointment scheduling through billing,
reports, visits, and patient-directory management.

### Complete Receptionist Screens

## Receptionist Dashboard

`<img src="docs/screenshots/receptionist/01-dashboard.png" alt="Receptionist Dashboard" width="100%">`{=html}

## Dashboard --- Quick Patient Search

`<img src="docs/screenshots/receptionist/02-dashboard-quick-patient-search.png" alt="Dashboard — Quick Patient Search" width="100%">`{=html}

## Book Appointment --- Find Patient

`<img src="docs/screenshots/receptionist/03-book-appointment-find-patient.png" alt="Book Appointment — Find Patient" width="100%">`{=html}

## Book Appointment --- Choose Slot

`<img src="docs/screenshots/receptionist/04-book-appointment-choose-slot.png" alt="Book Appointment — Choose Slot" width="100%">`{=html}

## Appointment Booked Confirmation

`<img src="docs/screenshots/receptionist/05-appointment-booked-confirmation.png" alt="Appointment Booked Confirmation" width="100%">`{=html}

## Register Patient --- Personal Details

`<img src="docs/screenshots/receptionist/06-register-patient-personal-details.png" alt="Register Patient — Personal Details" width="100%">`{=html}

## Register Patient --- Contact Details

`<img src="docs/screenshots/receptionist/07-register-patient-contact-details.png" alt="Register Patient — Contact Details" width="100%">`{=html}

## Registration Confirmation

`<img src="docs/screenshots/receptionist/08-registration-confirmation.png" alt="Registration Confirmation" width="100%">`{=html}

## Book Appointment for New Patient

`<img src="docs/screenshots/receptionist/09-book-appointment-new-patient.png" alt="Book Appointment for New Patient" width="100%">`{=html}

## Second Appointment Booked

`<img src="docs/screenshots/receptionist/10-second-appointment-booked.png" alt="Second Appointment Booked" width="100%">`{=html}

## Create Bill from Appointment

`<img src="docs/screenshots/receptionist/11-create-bill-from-appointment.png" alt="Create Bill from Appointment" width="100%">`{=html}

## Bill Details & Payment History

`<img src="docs/screenshots/receptionist/12-bill-details-payment-history.png" alt="Bill Details & Payment History" width="100%">`{=html}

## Printable Invoice

`<img src="docs/screenshots/receptionist/13-printable-invoice.png" alt="Printable Invoice" width="100%">`{=html}

## Reports Dashboard

`<img src="docs/screenshots/receptionist/14-reports-dashboard.png" alt="Reports Dashboard" width="100%">`{=html}

## Patient Visits Log

`<img src="docs/screenshots/receptionist/15-patient-visits-log.png" alt="Patient Visits Log" width="100%">`{=html}

## Patients Directory

`<img src="docs/screenshots/receptionist/16-patients-directory.png" alt="Patients Directory" width="100%">`{=html}

## Patients Directory --- Edit & Audit

`<img src="docs/screenshots/receptionist/17-patients-directory-edit-audit.png" alt="Patients Directory — Edit & Audit" width="100%">`{=html}

### Receptionist Workflow

``` text
Register Patient → Book Appointment → Manage Visits → Create Bill → Invoice → Reports / Patient Directory
```

------------------------------------------------------------------------

# 🩺 Doctor Module

The Doctor module covers appointment management, patient intake,
consultation, laboratory orders, prescriptions, consultation reports,
and patient history.

### Complete Doctor Screens

## Doctor Dashboard

`<img src="docs/screenshots/doctor/01-dashboard.png" alt="Doctor Dashboard" width="100%">`{=html}

## Appointments Queue

`<img src="docs/screenshots/doctor/02-appointments-queue.png" alt="Appointments Queue" width="100%">`{=html}

## Consultation --- Patient Intake

`<img src="docs/screenshots/doctor/03-consultation-patient-intake.png" alt="Consultation — Patient Intake" width="100%">`{=html}

## Lab Tests & Prescription

`<img src="docs/screenshots/doctor/04-lab-tests-prescription.png" alt="Lab Tests & Prescription" width="100%">`{=html}

## Consultation Summary

`<img src="docs/screenshots/doctor/05-consultation-summary.png" alt="Consultation Summary" width="100%">`{=html}

## Downloadable Consultation Report

`<img src="docs/screenshots/doctor/06-downloadable-consultation-report.png" alt="Downloadable Consultation Report" width="100%">`{=html}

## Patient History & Reports --- Search

`<img src="docs/screenshots/doctor/07-patient-history-search.png" alt="Patient History & Reports — Search" width="100%">`{=html}

## Patient History & Reports --- Lab Results

`<img src="docs/screenshots/doctor/08-patient-history-lab-results.png" alt="Patient History & Reports — Lab Results" width="100%">`{=html}

### Doctor Workflow

``` text
Appointment Queue
      ↓
Patient Intake
      ↓
Consultation
   ┌──┴───────────┐
   ↓              ↓
Lab Orders     Prescription
   ↓              ↓
Laboratory     Pharmacy
   └──────┬───────┘
          ↓
Consultation Summary / PDF / Patient History
```

------------------------------------------------------------------------

# 🧪 Laboratory Module --- Lab Technician

These are the **actual Lab Technician application screenshots supplied
for this README update**, rather than substituted presentation mockups.

### Complete Laboratory Screens

## Lab Technician Dashboard

`<img src="docs/screenshots/lab/01-dashboard.jpeg" alt="Lab Technician Dashboard" width="100%">`{=html}

## Pending Tests

`<img src="docs/screenshots/lab/02-pending-tests.jpeg" alt="Pending Tests" width="100%">`{=html}

## Enter Lab Result

`<img src="docs/screenshots/lab/03-enter-result.jpeg" alt="Enter Lab Result" width="100%">`{=html}

## Lab Billing --- Unbilled Requests

`<img src="docs/screenshots/lab/04-billing-unbilled.jpeg" alt="Lab Billing — Unbilled Requests" width="100%">`{=html}

## Lab Billing --- Bill Detail / Payment Status

`<img src="docs/screenshots/lab/05-billing-paid.jpeg" alt="Lab Billing — Bill Detail / Payment Status" width="100%">`{=html}

## Completed Reports

`<img src="docs/screenshots/lab/06-completed-reports.jpeg" alt="Completed Reports" width="100%">`{=html}

## Laboratory Report Detail

`<img src="docs/screenshots/lab/07-report-detail.jpeg" alt="Laboratory Report Detail" width="100%">`{=html}

## Patient Search

`<img src="docs/screenshots/lab/08-patient-search.png" alt="Patient Search" width="100%">`{=html}

### Laboratory Workflow

``` text
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

------------------------------------------------------------------------

# 💊 Pharmacy Module

The Pharmacy module covers medicine catalogue management, stock/batch
management, prescription processing, FEFO dispensing, billing,
reporting, and inventory/audit logging.

### Complete Pharmacy Screens & Engineering Views

## Pharmacy Dashboard

`<img src="docs/screenshots/pharmacy/01-pharmacy-dashboard.png" alt="Pharmacy Dashboard" width="100%">`{=html}

## Medicine Catalogue

`<img src="docs/screenshots/pharmacy/02-medicine-catalogue.png" alt="Medicine Catalogue" width="100%">`{=html}

## Medicine Management

`<img src="docs/screenshots/pharmacy/03-medicine-management.png" alt="Medicine Management" width="100%">`{=html}

## Stock Inventory

`<img src="docs/screenshots/pharmacy/04-stock-inventory.png" alt="Stock Inventory" width="100%">`{=html}

## Stock Management

`<img src="docs/screenshots/pharmacy/05-stock-management.png" alt="Stock Management" width="100%">`{=html}

## Prescription Management

`<img src="docs/screenshots/pharmacy/06-prescription-management.png" alt="Prescription Management" width="100%">`{=html}

## Medicine Dispensing --- Core Flow

`<img src="docs/screenshots/pharmacy/07-medicine-dispensing-core-flow.png" alt="Medicine Dispensing — Core Flow" width="100%">`{=html}

## Dispensing --- Atomic SQL Transaction

`<img src="docs/screenshots/pharmacy/08-atomic-dispensing-transaction.png" alt="Dispensing — Atomic SQL Transaction" width="100%">`{=html}

## Dispensing History & OTC Billing

`<img src="docs/screenshots/pharmacy/09-dispensing-history-otc-billing.png" alt="Dispensing History & OTC Billing" width="100%">`{=html}

## Billing System & PDF Invoice

`<img src="docs/screenshots/pharmacy/10-billing-system-pdf-invoice.png" alt="Billing System & PDF Invoice" width="100%">`{=html}

## Reports & Analytics

`<img src="docs/screenshots/pharmacy/11-reports-analytics.png" alt="Reports & Analytics" width="100%">`{=html}

## Inventory Log & Audit Log

`<img src="docs/screenshots/pharmacy/12-inventory-audit-log.png" alt="Inventory Log & Audit Log" width="100%">`{=html}

### Pharmacy Workflow

``` text
Prescription
    ↓
Stock Check
    ↓
FEFO Selection
    ↓
Dispensing
    ↓
Bill Creation
    ↓
PDF Invoice
    ↓
Reports / Inventory Log / Audit Log
```

------------------------------------------------------------------------

# 🧱 Architecture

``` text
Angular Frontend
      ↓
ASP.NET Core Web API
      ↓
Service Layer
      ↓
Repository Layer
      ↓
SQL Server / Stored Procedures
```

The project materials describe a shared architecture using Angular,
ASP.NET Core Web API, SQL Server/stored procedures, role-based session
authentication, QuestPDF for generated documents, and Chart.js for
analytics.

------------------------------------------------------------------------

# 🛠️ Technology Stack

  Layer                  Technology
  ---------------------- -----------------------------------
  Frontend               Angular 19, TypeScript
  UI                     Bootstrap
  Reactive Programming   RxJS
  Charts                 Chart.js
  Backend                ASP.NET Core Web API
  Language               C#
  Data Access            Dapper / Stored Procedures
  Database               Microsoft SQL Server
  Authentication         Session-based role authentication
  PDF Generation         QuestPDF
  Reporting              Chart.js + CSV export

> Keep version numbers synchronized with the actual versions committed
> in the repository.

------------------------------------------------------------------------

# ⭐ Key Engineering Highlights

## FEFO Medicine Dispensing

Pharmacy dispensing follows **First Expired, First Out (FEFO)** so
earlier-expiring batches are selected before later-expiring stock.

``` text
Medicine Batches
      ↓
Sort by Expiry Date ASC
      ↓
Select Earliest Expiry
      ↓
Deduct Required Quantity
      ↓
Continue to Next Batch if Required
```

## Atomic Dispensing Transaction

The pharmacy workflow is designed around an atomic database transaction
so related stock, dispensing, billing, and prescription updates do not
leave partial state when an operation fails.

## Audit & Inventory Traceability

The Pharmacy workflow includes inventory and audit logging for stock
movement and user activity.

------------------------------------------------------------------------

# 📊 Project Scope

The supplied project presentation summarizes the system as four core
modules with 45+ screens, 37+ API endpoints, and 6 report types.

  Area                         Documented Scope
  ----------------- ---------------------------
  Core modules                                4
  Overall screens                           45+
  Doctor                              8 screens
  Receptionist                       17 screens
  Laboratory                         10 screens
  Pharmacy            10 documented sub-modules
  API endpoints                             37+
  Report types                                6

------------------------------------------------------------------------

# 📁 Recommended Repository Structure

``` text
CMSV2026/
│
├── README.md
│
├── docs/
│   └── screenshots/
│       ├── shared/
│       ├── receptionist/
│       ├── doctor/
│       ├── lab/
│       └── pharmacy/
│
├── frontend/
├── backend/
└── database/
```

------------------------------------------------------------------------

# 🔒 Public GitHub Security Checklist

Before publishing or keeping this repository public:

-   [ ] Remove database passwords from committed configuration
-   [ ] Remove SMTP passwords / app passwords
-   [ ] Rotate credentials that were ever pushed publicly
-   [ ] Use environment variables or .NET User Secrets
-   [ ] Add local configuration files to `.gitignore`
-   [ ] Remove `node_modules`, `bin`, `obj`, `.vs`, and generated build
    output
-   [ ] Check Git history for previously committed secrets
-   [ ] Enable GitHub secret scanning / push protection where available

> **Never commit production credentials, database passwords, API keys,
> SMTP credentials, or authentication tokens.**

------------------------------------------------------------------------

# 🧑‍💻 Portfolio Positioning

This repository demonstrates a multi-role, end-to-end business
application rather than a collection of isolated CRUD pages. The
strongest areas to highlight in a resume are:

-   Role-based multi-module architecture
-   Angular + ASP.NET Core full-stack development
-   SQL Server stored-procedure/data-access layer
-   Cross-module clinical workflows
-   Transaction-safe pharmacy dispensing
-   FEFO inventory logic
-   PDF invoice/report generation
-   Reporting and analytics
-   Inventory and audit traceability

------------------------------------------------------------------------

# 👨‍💻 InfinityCoderzz CMS V2026

**Angular · ASP.NET Core · C# · SQL Server · Dapper · QuestPDF ·
Chart.js**

------------------------------------------------------------------------

## 📸 Screenshot Notes

-   Receptionist and Doctor images are rendered directly from the
    uploaded module presentation decks.
-   Pharmacy images are rendered directly from the uploaded CMS V2026
    presentation deck.
-   Laboratory images are the actual Lab Technician screenshots supplied
    separately for this README update.
-   All image references use repository-relative paths under
    `docs/screenshots/`, so they render correctly on GitHub when the
    entire folder is committed.





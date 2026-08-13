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

![Authentication](docs/screenshots/shared/authentication.png)

**Roles:** Receptionist · Doctor · Lab Technician / Lab Admin ·
Pharmacist

------------------------------------------------------------------------

# 🛎️ Receptionist Module

The Receptionist module covers the complete front-desk workflow from
patient registration and appointment scheduling through billing,
reports, visits, and patient-directory management.

### Complete Receptionist Screens

## Receptionist Dashboard

![Receptionist Dashboard](docs/screenshots/receptionist/01-dashboard.png)

## Dashboard --- Quick Patient Search

![Dashboard — Quick Patient Search](docs/screenshots/receptionist/02-dashboard-quick-patient-search.png)

## Book Appointment --- Find Patient

![Book Appointment — Find Patient](docs/screenshots/receptionist/03-book-appointment-find-patient.png)

## Book Appointment --- Choose Slot

![Book Appointment — Choose Slot](docs/screenshots/receptionist/04-book-appointment-choose-slot.png)

## Appointment Booked Confirmation

![Appointment Booked Confirmation](docs/screenshots/receptionist/05-appointment-booked-confirmation.png)

## Register Patient --- Personal Details

![Register Patient — Personal Details](docs/screenshots/receptionist/06-register-patient-personal-details.png)

## Register Patient --- Contact Details

![Register Patient — Contact Details](docs/screenshots/receptionist/07-register-patient-contact-details.png)

## Registration Confirmation

![Registration Confirmation](docs/screenshots/receptionist/08-registration-confirmation.png)

## Book Appointment for New Patient

![Book Appointment for New Patient](docs/screenshots/receptionist/09-book-appointment-new-patient.png)

## Second Appointment Booked

![Second Appointment Booked](docs/screenshots/receptionist/10-second-appointment-booked.png)

## Create Bill from Appointment

![Create Bill from Appointment](docs/screenshots/receptionist/11-create-bill-from-appointment.png)

## Bill Details & Payment History

![Bill Details & Payment History](docs/screenshots/receptionist/12-bill-details-payment-history.png)

## Printable Invoice

![Printable Invoice](docs/screenshots/receptionist/13-printable-invoice.png)

## Reports Dashboard

![Reports Dashboard](docs/screenshots/receptionist/14-reports-dashboard.png)

## Patient Visits Log

![Patient Visits Log](docs/screenshots/receptionist/15-patient-visits-log.png)

## Patients Directory

![Patients Directory](docs/screenshots/receptionist/16-patients-directory.png)

## Patients Directory --- Edit & Audit

![Patients Directory — Edit & Audit](docs/screenshots/receptionist/17-patients-directory-edit-audit.png)

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

![Doctor Dashboard](docs/screenshots/doctor/01-dashboard.png)

## Appointments Queue

![Appointments Queue](docs/screenshots/doctor/02-appointments-queue.png)

## Consultation --- Patient Intake

![Consultation — Patient Intake](docs/screenshots/doctor/03-consultation-patient-intake.png)

## Lab Tests & Prescription

![Lab Tests & Prescription](docs/screenshots/doctor/04-lab-tests-prescription.png)

## Consultation Summary

![Consultation Summary](docs/screenshots/doctor/05-consultation-summary.png)

## Downloadable Consultation Report

![Downloadable Consultation Report](docs/screenshots/doctor/06-downloadable-consultation-report.png)

## Patient History & Reports --- Search

![Patient History & Reports — Search](docs/screenshots/doctor/07-patient-history-search.png)

## Patient History & Reports --- Lab Results

![Patient History & Reports — Lab Results](docs/screenshots/doctor/08-patient-history-lab-results.png)

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

![Lab Technician Dashboard](docs/screenshots/lab/01-dashboard.jpeg)

## Pending Tests

![Pending Tests](docs/screenshots/lab/02-pending-tests.jpeg)

## Enter Lab Result

![Enter Lab Result](docs/screenshots/lab/03-enter-result.jpeg)

## Lab Billing --- Unbilled Requests

![Lab Billing — Unbilled Requests](docs/screenshots/lab/04-billing-unbilled.jpeg)

## Lab Billing --- Bill Detail / Payment Status

![Lab Billing — Bill Detail / Payment Status](docs/screenshots/lab/05-billing-paid.jpeg)

## Completed Reports

![Completed Reports](docs/screenshots/lab/06-completed-reports.jpeg)

## Laboratory Report Detail

![Laboratory Report Detail](docs/screenshots/lab/07-report-detail.jpeg)

## Patient Search

![Patient Search](docs/screenshots/lab/08-patient-search.png)

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

![Pharmacy Dashboard](docs/screenshots/pharmacy/01-pharmacy-dashboard.png)

## Medicine Catalogue

![Medicine Catalogue](docs/screenshots/pharmacy/02-medicine-catalogue.png)

## Medicine Management

![Medicine Management](docs/screenshots/pharmacy/03-medicine-management.png)

## Stock Inventory

![Stock Inventory](docs/screenshots/pharmacy/04-stock-inventory.png)

## Stock Management

![Stock Management](docs/screenshots/pharmacy/05-stock-management.png)

## Prescription Management

![Prescription Management](docs/screenshots/pharmacy/06-prescription-management.png)

## Medicine Dispensing --- Core Flow

![Medicine Dispensing — Core Flow](docs/screenshots/pharmacy/07-medicine-dispensing-core-flow.png)

## Dispensing --- Atomic SQL Transaction

![Dispensing — Atomic SQL Transaction](docs/screenshots/pharmacy/08-atomic-dispensing-transaction.png)

## Dispensing History & OTC Billing

![Dispensing History & OTC Billing](docs/screenshots/pharmacy/09-dispensing-history-otc-billing.png)

## Billing System & PDF Invoice

![Billing System & PDF Invoice](docs/screenshots/pharmacy/10-billing-system-pdf-invoice.png)

## Reports & Analytics

![Reports & Analytics](docs/screenshots/pharmacy/11-reports-analytics.png)

## Inventory Log & Audit Log

![Inventory Log & Audit Log](docs/screenshots/pharmacy/12-inventory-audit-log.png)

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






#  CMS — Clinic Management System

> A full-stack, role-based Clinic Management System connecting Reception, Doctor, Laboratory, and Pharmacy workflows.

## 📌 Project at a Glance


[![Angular](https://img.shields.io/badge/Angular-19-DD0031?logo=angular&logoColor=white)](https://angular.dev/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet)
[![C%23](https://img.shields.io/badge/C%23-Backend-239120?logo=csharp&logoColor=white)](https://dotnet.microsoft.com/languages/csharp)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![Dapper](https://img.shields.io/badge/Dapper-Data%20Access-512BD4)](https://github.com/DapperLib/Dapper)

---

## Overview

InfinityCoderzz CMS V2026 is a multi-role healthcare operations platform designed around connected real-world workflows rather than isolated CRUD screens.

The system brings together:

- **Receptionist operations** — patient registration, appointments, visits, billing, invoices, and reporting
- **Doctor operations** — appointment queue, consultation, laboratory orders, prescriptions, reports, and patient history
- **Laboratory operations** — pending tests, result entry, laboratory billing, completed reports, and patient search
- **Pharmacy operations** — medicine catalogue, batch inventory, prescription processing, FEFO dispensing, billing, reporting, inventory logs, and audit logs

The project materials document **4 core modules, 45+ application screens, 37+ API endpoints, and 6 report types**.

---

## Product Workflow

```text
                         ┌─────────────────────┐
                         │   Authentication     │
                         │  Role-based Access   │
                         └──────────┬──────────┘
                                    │
             ┌──────────────────────┼──────────────────────┐
             │                      │                      │
             ▼                      ▼                      ▼
      ┌─────────────┐       ┌─────────────┐       ┌─────────────┐
      │ Reception   │       │   Doctor    │       │ Laboratory  │
      │             │       │             │       │             │
      │ Registration│──────►│ Consultation│──────►│ Test/Result │
      │ Appointment │       │ Prescription│       │ Billing     │
      │ Billing     │       │ Lab Orders  │       │ Reports     │
      └─────────────┘       └──────┬──────┘       └─────────────┘
                                   │
                                   │ Prescription
                                   ▼
                            ┌──────────────┐
                            │   Pharmacy   │
                            │              │
                            │ Medicines    │
                            │ Stock        │
                            │ FEFO         │
                            │ Dispensing   │
                            │ Billing      │
                            │ Reporting    │
                            └──────┬───────┘
                                   │
                                   ▼
                            ┌──────────────┐
                            │ SQL Server   │
                            │ Stored Procs │
                            └──────────────┘
```

---

# Architecture

```text
┌─────────────────────────────────────────────┐
│              Angular Frontend               │
│ TypeScript · RxJS · Forms · Route Guards    │
└──────────────────────┬──────────────────────┘
                       │ HTTP / REST
                       ▼
┌─────────────────────────────────────────────┐
│           ASP.NET Core Web API              │
│ Controllers · Session Auth · C#             │
└──────────────────────┬──────────────────────┘
                       ▼
┌─────────────────────────────────────────────┐
│               Service Layer                 │
│        Business Logic / Interfaces          │
└──────────────────────┬──────────────────────┘
                       ▼
┌─────────────────────────────────────────────┐
│             Repository Layer                │
│           Data Access / Dapper              │
└──────────────────────┬──────────────────────┘
                       ▼
┌─────────────────────────────────────────────┐
│              Microsoft SQL Server           │
│        Relational Data / Stored Procedures  │
└─────────────────────────────────────────────┘
```

### Architectural characteristics

- Role-based access with dedicated workflows
- Angular route guards and HTTP interceptors
- ASP.NET Core Web API
- Service and repository separation
- Dapper-based data access
- SQL Server stored procedures
- Session-based authentication
- Server-side PDF generation with QuestPDF
- Dashboard/report visualization with Chart.js
- Inventory and audit logging

---

# Technology Stack

| Layer | Technology |
|---|---|
| Frontend | Angular 19, TypeScript |
| UI | Bootstrap |
| Reactive Programming | RxJS |
| Routing & Access | Angular Router, Route Guards, HTTP Interceptors |
| Backend | ASP.NET Core Web API |
| Language | C# |
| Data Access | Dapper, SQL Server Stored Procedures |
| Database | Microsoft SQL Server |
| Authentication | Session-based role authentication |
| PDF Generation | QuestPDF |
| Analytics | Chart.js |
| Reporting | CSV export + dashboard analytics |

> Keep version numbers synchronized with the versions committed in the repository.

---

# Role-Based Access

The platform provides dedicated experiences for:

| Role | Primary Responsibilities |
|---|---|
| **Receptionist** | Patient registration, appointments, visits, billing, reports |
| **Doctor** | Appointments, consultation, lab orders, prescriptions, history |
| **Lab Technician** | Pending tests, results, billing, reports, patient search |
| **Pharmacist** | Medicines, stock, prescriptions, dispensing, billing, reports |

The application uses a shared login flow followed by role-specific access and dashboards.

## Authentication

![Authentication](docs/screenshots/shared/authentication.png)

---

# 🛎️ Receptionist Module

The Receptionist module manages the front-desk workflow from patient registration and appointment scheduling through billing, invoices, visits, reports, and patient-directory operations.

## Dashboard

![Receptionist Dashboard](docs/screenshots/receptionist/01-dashboard.png)

## Dashboard — Quick Patient Search

![Dashboard — Quick Patient Search](docs/screenshots/receptionist/02-quick-patient-search.png)

## Book Appointment — Find Patient

![Book Appointment — Find Patient](docs/screenshots/receptionist/03-find-patient.png)

## Book Appointment — Choose Slot

![Book Appointment — Choose Slot](docs/screenshots/receptionist/04-choose-slot.png)

## Appointment Booked Confirmation

![Appointment Booked Confirmation](docs/screenshots/receptionist/05-appointment-confirmation.png)

## Register Patient — Personal Details

![Register Patient — Personal Details](docs/screenshots/receptionist/06-registration-personal.png)

## Register Patient — Contact Details

![Register Patient — Contact Details](docs/screenshots/receptionist/07-registration-contact.png)

## Registration Confirmation

![Registration Confirmation](docs/screenshots/receptionist/08-registration-confirmation.png)

## Book Appointment for New Patient

![Book Appointment for New Patient](docs/screenshots/receptionist/09-new-patient-appointment.png)

## Second Appointment Booked

![Second Appointment Booked](docs/screenshots/receptionist/10-second-appointment.png)

## Create Bill from Appointment

![Create Bill from Appointment](docs/screenshots/receptionist/11-create-bill.png)

## Bill Details & Payment History

![Bill Details & Payment History](docs/screenshots/receptionist/12-bill-payment-history.png)

## Printable Invoice

![Printable Invoice](docs/screenshots/receptionist/13-printable-invoice.png)

## Reports Dashboard

![Reports Dashboard](docs/screenshots/receptionist/14-reports-dashboard.png)

## Patient Visits Log

![Patient Visits Log](docs/screenshots/receptionist/15-patient-visits.png)


## Patients Directory — Edit & Audit

![Patients Directory — Edit & Audit](docs/screenshots/receptionist/17-patients-edit-audit.png)

### Receptionist workflow

```text
Register Patient
      ↓
Book Appointment
      ↓
Manage Visits
      ↓
Create Bill
      ↓
Payment / Invoice
      ↓
Reports / Patient Directory
```

---

# 💊 Pharmacy Module

The Pharmacy module manages medication operations from medicine catalogue and batch inventory through prescription processing, FEFO dispensing, billing, reporting, and traceability.

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

## Medicine Dispensing — Core Flow

![Medicine Dispensing — Core Flow](docs/screenshots/pharmacy/07-medicine-dispensing-core-flow.png)


## Dispensing History & OTC Billing

![Dispensing History & OTC Billing](docs/screenshots/pharmacy/09-dispensing-history-otc-billing.png)

## Billing System & PDF Invoice

![Billing System & PDF Invoice](docs/screenshots/pharmacy/10-billing-system-pdf-invoice.png)

## Reports & Analytics

![Reports & Analytics](docs/screenshots/pharmacy/11-reports-analytics.png)

## Inventory Log & Audit Log

![Inventory Log & Audit Log](docs/screenshots/pharmacy/12-inventory-audit-log.png)

---

## FEFO Dispensing

The pharmacy workflow applies **FEFO (First Expired, First Out)** so earlier-expiring medicine batches are selected before later-expiring stock.

```text
Medicine Batches
      ↓
Sort by Expiry Date
      ↓
Select Earliest Expiry
      ↓
Deduct Required Quantity
      ↓
Continue to Next Batch if Required
```

## Atomic Dispensing Workflow

The documented dispensing workflow groups related operations into a database transaction so a failure can roll back the operation rather than leaving partially updated dispensing, stock, billing, or prescription state.

```text
Create Dispensing
       ↓
Validate / Deduct Stock
       ↓
Create Dispensing Items
       ↓
Create Bill
       ↓
Create Bill Items
       ↓
Update Prescription
       ↓
Link Prescription & Bill
```

## Inventory & Audit Traceability

The Pharmacy workflow includes inventory and audit logging for operational traceability, including stock movements and pharmacist actions.

---

## Reporting & Analytics

The project includes role-specific operational reporting and dashboard visualization.

Examples include:

- Appointment and operational summaries
- Laboratory workload and completed-report views
- Pharmacy sales summary
- Medicine-wise sales
- Stock status
- Expiry monitoring
- Low-stock monitoring
- Dispensing reports
- CSV export
- Dashboard charts

---

# 🩺 Doctor Module

The Doctor module supports the clinical workflow from appointment queue through patient intake, consultation, laboratory orders, prescriptions, reports, and patient history.

## Dashboard

![Doctor Dashboard](docs/screenshots/doctor/01-dashboard.png)

## Appointments Queue

![Appointments Queue](docs/screenshots/doctor/02-appointments-queue.png)

## Consultation — Patient Intake

![Consultation — Patient Intake](docs/screenshots/doctor/03-consultation-intake.png)

## Laboratory Tests & Prescription

![Laboratory Tests & Prescription](docs/screenshots/doctor/04-lab-tests-prescription.png)

## Consultation Summary

![Consultation Summary](docs/screenshots/doctor/05-consultation-summary.png)

## Downloadable Consultation Report

![Downloadable Consultation Report](docs/screenshots/doctor/06-consultation-pdf.png)

## Patient History — Search

![Patient History — Search](docs/screenshots/doctor/07-patient-history-search.png)

## Patient History — Laboratory Results

![Patient History — Laboratory Results](docs/screenshots/doctor/08-patient-history-lab-results.png)

### Doctor workflow

```text
Appointment Queue
       ↓
Patient Intake
       ↓
Consultation
   ┌───┴─────────────┐
   ▼                 ▼
Lab Orders       Prescription
   ▼                 ▼
Laboratory        Pharmacy
   └───────┬─────────┘
           ▼
Consultation Summary
           ↓
     PDF / Patient History
```

---

# 🧪 Laboratory Module

The Laboratory module manages diagnostic processing from pending test requests through result entry, billing, completed reports, and patient search.

The screenshots in this section are the **actual Lab Technician application screenshots supplied for the project**.

## Lab Technician Dashboard

![Lab Technician Dashboard](docs/screenshots/lab/01-dashboard.jpeg)

## Pending Tests

![Pending Tests](docs/screenshots/lab/02-pending-tests.jpeg)

## Enter Laboratory Result

![Enter Laboratory Result](docs/screenshots/lab/03-enter-result.jpeg)

## Laboratory Billing — Unbilled Requests

![Laboratory Billing — Unbilled Requests](docs/screenshots/lab/04-billing-unbilled.jpeg)

## Laboratory Billing — Bill Detail / Payment Status

![Laboratory Billing — Bill Detail / Payment Status](docs/screenshots/lab/06-billing-detail.jpeg)

## Completed Reports

![Completed Reports](docs/screenshots/lab/07-completed-reports.jpeg)

## Laboratory Report Detail

![Laboratory Report Detail](docs/screenshots/lab/08-report-detail.jpeg)

## Patient Search

![Patient Search](docs/screenshots/lab/09-patient-search.png)

### Laboratory workflow

```text
Doctor Orders Test
       ↓
Pending Tests
       ↓
Result Entry
       ↓
Completed Report
       ↓
Laboratory Billing
       ↓
Patient Search / Report Lookup
```

---



# Database & Data Layer

The system uses Microsoft SQL Server with relational tables and stored procedures to support core business operations.

The database script supplied with the project is:

```text
ScriptDBFinal(6).sql
```

The data-access flow is:

```text
Angular
   ↓
ASP.NET Core API
   ↓
Service
   ↓
Repository
   ↓
Dapper
   ↓
SQL Server Stored Procedure
   ↓
Database
```

---

# Project Structure

A recommended repository layout is:

```text
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
│   └── Angular application
│
├── backend/
│   └── ASP.NET Core Web API
│
└── database/
    └── SQL Server scripts
```

---

## Local Development

## Prerequisites

- Node.js and npm
- Angular CLI compatible with the project version
- .NET 8 SDK
- Microsoft SQL Server
- SQL Server Management Studio or equivalent SQL client

## Database

Create/configure the SQL Server database and execute:

```text
ScriptDBFinal(6).sql
```

## Backend

From the ASP.NET Core API project:

```bash
dotnet restore
dotnet run
```

The project uses environment-specific configuration for the database connection and other infrastructure settings.

## Frontend

From the Angular project:

```bash
npm install
npm start
```

Configure the frontend API URL to match the backend launch URL.

> Do not commit real credentials or environment-specific secrets to the repository.

---

## API Documentation

The backend exposes REST endpoints for the application's business modules and is documented through Swagger/OpenAPI when the API is running.

Typical local Swagger URL:

```text
https://localhost:<backend-port>/swagger
```

Use the actual launch port defined by the project rather than hard-coding a production URL.

---

## Engineering Highlights

### 1. Multi-role application architecture

A single platform provides separate workflows for four operational roles while sharing core patient, appointment, clinical, laboratory, billing, and pharmacy data.

### 2. Layered backend

Controllers, services, repositories, and database operations are separated to keep transport, business logic, and persistence responsibilities distinct.

### 3. Transaction-aware pharmacy workflow

The dispensing flow coordinates stock deduction, dispensing records, billing, and prescription state as a transactional workflow.

### 4. FEFO inventory logic

Expiry-aware batch selection helps prioritize earlier-expiring medicines.

### 5. Document generation

The application supports generated clinical/billing documents, including PDF workflows using QuestPDF.

### 6. Operational traceability

Inventory and audit logging provide visibility into pharmacy stock movement and user activity.

### 7. Cross-module workflow design

The system connects:

```text
Reception
   ↓
Doctor
   ↓
Laboratory
   ↓
Pharmacy
   ↓
Billing / Reports / History
```

This makes the project representative of a business workflow spanning multiple functional domains.

---

## Project Scope

| Area | Scope |
|---|---:|
| Core modules | **4** |
| Documented application screens | **45+** |
| Doctor screens | **8** |
| Receptionist screens | **17** |
| Laboratory screens | **10** |
| Pharmacy | **10 documented sub-modules** |
| API endpoints | **37+** |
| Report types | **6** |



## Portfolio Summary

InfinityCoderzz CMS V2026 demonstrates full-stack engineering across:

- Angular frontend development
- ASP.NET Core Web API development
- C# service and repository architecture
- SQL Server relational data design
- Stored-procedure-based data operations
- Role-based application workflows
- Cross-module business processes
- Transaction-aware inventory and billing
- FEFO stock allocation
- PDF document generation
- Reporting and analytics
- Inventory and audit traceability

The project is intended to demonstrate the ability to design and implement a **non-trivial, multi-role business application** from frontend workflows through API services and database operations.

---



## Author

## InfinityCoderzz CMSV2026

**Full-Stack Clinic Management System**

`Angular` · `TypeScript` · `ASP.NET Core` · `C#` · `SQL Server` · `Dapper` · `QuestPDF` · `Chart.js`

---

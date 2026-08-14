# InfinityCoderzz CMS --- Clinic Management System

> A full-stack, role-based Clinic Management System built with Angular 20, ASP.NET Core Web API, C#, and Microsoft SQL Server.

InfinityCoderzz CMS connects four operational modules into a single clinic workflow:

**Reception → Doctor → Laboratory → Pharmacy**

The project documents **45+ application screens** across four core modules: **8 Doctor screens, 17 Receptionist screens, 10 Laboratory screens, and 10 Pharmacy screens**. The workflows cover appointments, consultation, patient registration, diagnostics, medicine inventory, dispensing, billing, reporting, and patient history.

------------------------------------------------------------------------

## ✨ Why this project stands out

-   Role-based access for Doctor, Receptionist, Lab Technician, and Pharmacist
    users
-   Angular frontend with route-guarded module access
-   ASP.NET Core Web API backend
-   Session-based authentication
-   Repository + Service architecture
-   SQL Server with stored-procedure-driven database operations
-   Cross-module patient, appointment, prescription, laboratory, and
    billing workflows
-   PDF generation with QuestPDF
-   Dashboard analytics with Chart.js
-   Pharmacy inventory and expiry tracking
-   FEFO medicine dispensing
-   Atomic dispensing/billing transaction
-   Audit and inventory logs
-   CSV report export

The project walkthrough describes a shared architecture across all four
modules, including Angular, ASP.NET Core Web API, SQL Server stored
procedures, session authentication, QuestPDF, and reporting.

------------------------------------------------------------------------

# 🏗️ High-Level Workflow

``` text
                         ┌─────────────────┐
                         │  Authentication │
                         └────────┬────────┘
                                  │
             ┌────────────────────┼────────────────────┐
             │                    │                    │
             ▼                    ▼                    ▼
      ┌─────────────┐     ┌─────────────┐     ┌─────────────┐
      │ Reception   │     │   Doctor    │     │     Lab     │
      │             │     │             │     │             │
      │ Registration│────►│ Consultation│────►│ Lab Tests   │
      │ Appointment │     │ Prescription│     │ Results     │
      │ Billing     │     │ History     │     │ Billing     │
      └─────────────┘     └──────┬──────┘     └─────────────┘
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
                          │ Reports      │
                          └──────┬───────┘
                                 │
                                 ▼
                          ┌──────────────┐
                          │ SQL Server   │
                          │ Stored Procs │
                          └──────────────┘
```

------------------------------------------------------------------------

# 📸 Module Screenshots

The screenshots below are selected from the supplied CMS V2026 project
walkthrough and focus on the most important user journeys rather than
every individual screen.

------------------------------------------------------------------------

## 🛎️ Receptionist Module

**17 screens · `/reception`**

The receptionist workflow covers the front desk: dashboard, patient
registration, appointment booking, billing/invoice, reports, visits, and
patient directory.

### Dashboard & Patient Search

The dashboard provides patient/appointment/collection summaries and
quick patient lookup.

![Receptionist
Dashboard](docs/screenshots/receptionist/01-dashboard.png)

### Patient Registration

Registration generates an MMR patient code and captures personal/contact
information before confirmation.

![Patient
Registration](docs/screenshots/receptionist/02-registration.png)

### Appointment Booking

Appointments can be filtered, created for a patient, assigned to a
department/doctor, and booked into an available slot.

![Appointment
Booking](docs/screenshots/receptionist/03-appointments.png)

### Billing & Invoice

Completed appointments can flow into billing, payment history, and
printable/downloadable invoice generation.

![Reception Billing](docs/screenshots/receptionist/04-billing.png)

### Reports, Visits & Patient Directory

The module also provides filterable reporting, visit history, and a
searchable patient directory.

![Reception Reports](docs/screenshots/receptionist/05-reports.png)

------------------------------------------------------------------------

## 🩺 Doctor Module

**8 screens · `/doctor`**

The doctor workflow covers appointments, consultation, lab orders,
prescriptions, consultation reports, and patient history.

### Dashboard & Appointment Queue

The doctor dashboard provides today's appointment count and shortcuts
into the appointment queue and patient history.

![Doctor Dashboard](docs/screenshots/doctor/01-dashboard.png)

### Consultation, Lab Tests & Prescription

The consultation workflow captures symptoms, diagnosis, notes, follow-up
dates, lab tests, and prescription items.

![Doctor Consultation](docs/screenshots/doctor/02-consultation.png)

### Consultation Summary & PDF Report

The doctor can review the consultation and export a timestamped PDF
containing the clinical summary and medicine list.

![Doctor Report](docs/screenshots/doctor/03-report.png)

### Patient History & Lab Results

Patient history can be searched by MMR code, with access to previous
laboratory results and their status.

![Patient History](docs/screenshots/doctor/04-history.png)

------------------------------------------------------------------------

## 🧪 Laboratory Module

**10 screens · `/lab`**

The laboratory workflow covers dashboard operations, pending tests,
result entry, billing, completed reports, and patient search.

### Lab Dashboard

The dashboard summarizes pending, completed, in-process, and sample
information, with a workload visualization.

![Lab Dashboard](docs/screenshots/lab/01-dashboard.png)

### Pending Tests & Result Entry

Lab technicians can work from a pending-test queue and enter the result,
observation, and remarks for each ordered test.

![Lab Results](docs/screenshots/lab/02-results.png)

### Laboratory Billing

Completed tests can be converted into bills, with bill status and
itemized bill details.

![Lab Billing](docs/screenshots/lab/03-billing.png)

### Completed Reports & Patient Search

Completed reports show results, abnormal flags, ordering doctor,
reference ranges, and patient-search functionality.

![Lab Reports](docs/screenshots/lab/04-reports.png)

------------------------------------------------------------------------

## 💊 Pharmacy Module

**10 screens · `/pharmacy`**

The pharmacy module manages the medication lifecycle from stock intake
through prescription dispensing, billing, analytics, inventory logs, and
audit logs.

### Pharmacy Dashboard

The dashboard combines live KPIs, revenue/dispensing charts, low-stock
alerts, and expiring-stock alerts.

![Pharmacy Dashboard](docs/screenshots/pharmacy/01-dashboard.png)

### Medicine Management

Medicines support creation/editing, generated medicine codes,
validation, and soft disabling instead of physical deletion.

![Medicine Management](docs/screenshots/pharmacy/02-medicines.png)

### Stock & Batch Management

Medicine stock is tracked by batch, quantity, purchase date, expiry
date, and reorder status, with low-stock and expiry views.

![Stock Management](docs/screenshots/pharmacy/03-stock.png)

### Prescription & FEFO Dispensing

Prescription dispensing performs a stock check, uses **FEFO (First
Expired First Out)**, creates the bill, and updates prescription state.
The documented flow runs as an atomic seven-step database transaction
with rollback on failure.

![Medicine Dispensing](docs/screenshots/pharmacy/04-dispensing.png)

### Billing & PDF Invoice

Pharmacy billing supports bill details, invoice printing/download,
cancellation, and prescription linkage; PDF invoices are generated with
QuestPDF.

![Pharmacy Billing](docs/screenshots/pharmacy/05-billing.png)

### Reports & Analytics

The pharmacy reporting layer includes sales summary, medicine-wise
sales, stock status, expiry, low-stock, and dispensing reports, with CSV
export.

![Pharmacy Reports](docs/screenshots/pharmacy/06-reports.png)

------------------------------------------------------------------------

# 📘 API Documentation

The ASP.NET Core backend exposes RESTful endpoints for the application's business modules and can be explored through Swagger/OpenAPI while running locally.

```text
https://localhost:<backend-port>/swagger
```

Use the actual HTTPS port shown by the ASP.NET Core launch profile.

# 🔐 Authentication & Role-Based Access

All four modules use the same role-guarded login flow. The supplied
walkthrough describes credential validation, server-side session
creation, and redirection to the appropriate role dashboard.

``` text
Credentials
    ↓
Role Validation
    ↓
Server Session
    ↓
Role-Based Dashboard
```

Supported roles:

-   Doctor
-   Receptionist
-   Lab Technician
-   Pharmacist

------------------------------------------------------------------------

# 🧱 Backend Architecture

The pharmacy architecture documentation shows a five-layer structure:

``` text
Angular Frontend
      ↓
ASP.NET Core Controller
      ↓
Service Interface / Implementation
      ↓
Repository Interface / Implementation
      ↓
SQL Server Stored Procedures
```

The backend separates controllers, services, and repositories through interfaces and dependency injection, with database operations routed through SQL Server stored procedures.

------------------------------------------------------------------------

# 💊 Pharmacy Engineering Highlights

One of the strongest engineering workflows in the project is
prescription dispensing.

### FEFO

Medicine batches are ordered by expiry date so the earliest-expiring
stock is consumed first.

### Atomic transaction

The documented dispensing flow contains seven database steps and rolls
back the complete operation if any step fails. This prevents partial
stock/billing state.

### Traceability

Inventory and audit logs record stock movements and pharmacist actions
such as login, medicine creation, dispensing, billing, and bill
cancellation.

------------------------------------------------------------------------

# 🛠️ Technology Stack

  -----------------------------------------------------------------------
  Layer                               Technologies
  ----------------------------------- -----------------------------------
  Frontend                            Angular, TypeScript, Standalone
                                      Components, RxJS, Reactive Forms

  UI / Analytics                      Bootstrap, Chart.js, Font Awesome

  Backend                             ASP.NET Core Web API, C#

  Architecture                        Controller → Service → Repository

  Database                            Microsoft SQL Server

  Data Access                         Microsoft.Data.SqlClient (ADO.NET) / Stored Procedures

  Authentication                      Session-based role authentication

  Documents                           QuestPDF

  Reporting                           Chart.js + CSV Export
  -----------------------------------------------------------------------

The frontend is based on **Angular 20** and the backend targets **.NET 8 / ASP.NET Core Web API**. Database operations use **Microsoft.Data.SqlClient (ADO.NET)** and SQL Server stored procedures.

------------------------------------------------------------------------

# 📂 Repository Contents

The repository contains the application source, database script, and documentation assets. The screenshot paths used by this README are kept under `docs/screenshots/` so GitHub can render them directly.

```text
README.md
docs/
└── screenshots/
    ├── receptionist/
    ├── doctor/
    ├── lab/
    └── pharmacy/

Angular frontend
ASP.NET Core Web API
SQL Server database script
```

------------------------------------------------------------------------

# 🚀 Key Business Workflows

### Patient → Appointment → Consultation

``` text
Register Patient
      ↓
Book Appointment
      ↓
Doctor Appointment Queue
      ↓
Consultation
```

### Consultation → Laboratory

``` text
Doctor
  ↓
Order Lab Test
  ↓
Lab Pending Queue
  ↓
Enter Result
  ↓
Completed Report
```

### Consultation → Pharmacy

``` text
Doctor
  ↓
Prescription
  ↓
Pharmacy Prescription Queue
  ↓
Stock Check
  ↓
FEFO Dispensing
  ↓
Pharmacy Bill
  ↓
PDF Invoice
```

------------------------------------------------------------------------

# 📊 Project Scope

| Area | Scope |
|---|---:|
| Core modules | **4** |
| Documented application screens | **45+** |
| Doctor screens | **8** |
| Receptionist screens | **17** |
| Laboratory screens | **10** |
| Pharmacy screens | **10** |
| API endpoints | **80+** |
| Documented report types | **6** |

# 🚀 Local Development

## Prerequisites

- .NET 8 SDK
- Node.js and npm
- Angular CLI 20
- Microsoft SQL Server
- SQL Server Management Studio (SSMS) or another SQL client

## Database

Create/configure the SQL Server database and execute:

```text
ScriptDBFinal(6).sql
```

Configure the database connection using local/environment-specific configuration.

> **Do not commit database passwords, API keys, SMTP credentials, access tokens, or other secrets to the repository.**

## Backend — ASP.NET Core Web API

Navigate to the ASP.NET Core Web API project directory and run:

```bash
dotnet restore
dotnet run
```

Use the HTTPS URL shown by the ASP.NET Core launch profile.

Swagger/OpenAPI is available at:

```text
https://localhost:<backend-port>/swagger
```

## Frontend — Angular

Navigate to the Angular project directory and run:

```bash
npm install
npm start
```

The project's `package.json` defines the `start` script for the Angular development server.

Configure the Angular API base URL to match the backend HTTPS URL.

The Angular development server is typically available at:

```text
http://localhost:4200
```

# 🔒 Public GitHub Security Checklist

Before keeping this repository public:

-   [ ] Remove database passwords from committed configuration
-   [ ] Remove SMTP passwords / app passwords
-   [ ] Rotate any credential that was previously pushed
-   [ ] Use environment variables or .NET User Secrets for secrets
-   [ ] Add development configuration to `.gitignore`
-   [ ] Remove `node_modules`, `bin`, `obj`, `.vs`, and generated build
    output
-   [ ] Check Git history for previously committed secrets
-   [ ] Enable GitHub secret scanning/push protection where available

**Never commit production credentials, API keys, database passwords,
SMTP passwords, or tokens to this repository.**

------------------------------------------------------------------------

# 📌 Project Status

**Portfolio / Full-Stack Engineering Project**

This project demonstrates a connected, multi-role business application
with clinical workflows, transactional billing, laboratory processing,
pharmacy inventory, FEFO dispensing, reporting, PDF generation, and
auditability.

------------------------------------------------------------------------

# 👨‍💻 Team InfinityCoderzz

**InfinityCoderzz --- CMS V2026**

Built with:

`Angular 20` · `ASP.NET Core Web API` · `C#` · `SQL Server` · `QuestPDF` · `Chart.js`
· `Chart.js`

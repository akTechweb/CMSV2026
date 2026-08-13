# CMSV2026


# InfinityCoderzz CMS — Full-Stack Clinic Management System

> A modular, role-based healthcare management platform built with **Angular 20**, **ASP.NET Core 8 Web API**, **Dapper**, and **Microsoft SQL Server**.

InfinityCoderzz CMS is a full-stack Clinic Management System designed to digitize and integrate day-to-day healthcare operations across **reception, doctors, laboratory technicians, and pharmacy staff**.

The system follows a layered architecture with a dedicated Angular frontend, RESTful ASP.NET Core Web API, repository/service layers, SQL Server stored procedures, role-based authorization, session-based authentication, reporting, billing, inventory management, prescription workflows, and PDF generation.

---

## 🚀 Project Highlights

* Full-stack **Angular + ASP.NET Core Web API** architecture
* Role-based application workflows for:

  * Doctor
  * Receptionist
  * Lab Technician
  * Pharmacist
* Server-side session authentication with secure cookie handling
* Angular route guards for authentication and role isolation
* RESTful API architecture with Swagger/OpenAPI documentation
* Repository → Service → Controller separation
* Dapper-based data access
* SQL Server relational database with **32+ core tables**
* **100+ stored procedures** supporting transactional and reporting workflows
* Patient registration and management
* Appointment scheduling and cancellation
* Doctor consultation workflow
* Patient history and medical reports
* Laboratory test management and result entry
* Laboratory billing and PDF report generation
* Pharmacy medicine catalog management
* Medicine batch and inventory management
* Low-stock, expiring, and expired inventory tracking
* Prescription management
* Prescription dispensing and stock validation
* Pharmacy billing and invoice generation
* Payment processing
* Audit logging and inventory logging
* Sales, stock, expiry, medicine-wise, and dispensing reports
* CSV report export
* PDF invoice generation using QuestPDF
* Angular charts and dashboard visualizations
* Voice-search UI support
* Reusable Angular shared components
* Lazy-loaded pharmacy modules/components
* Centralized HTTP credential and unauthorized-response handling
* Responsive role-specific dashboards

---

## 🏗️ Architecture

The application is organized as a multi-layer full-stack system:

```text
┌───────────────────────────────────────────────────────────┐
│                    Angular 20 Frontend                    │
│                                                           │
│  Authentication │ Doctor │ Reception │ Laboratory         │
│  Pharmacy │ Reports │ Billing │ Shared Components         │
└─────────────────────────────┬─────────────────────────────┘
                              │
                         HTTP / REST
                              │
                              ▼
┌───────────────────────────────────────────────────────────┐
│                 ASP.NET Core 8 Web API                    │
│                                                           │
│  Controllers → Services → Repositories → Dapper          │
│                                                           │
│  Authentication │ Session │ CORS │ Swagger │ PDF          │
└─────────────────────────────┬─────────────────────────────┘
                              │
                         SQL / Dapper
                              │
                              ▼
┌───────────────────────────────────────────────────────────┐
│                     Microsoft SQL Server                  │
│                                                           │
│  32+ Tables │ 100+ Stored Procedures │ Relationships      │
│  Transactions │ Reporting │ Audit & Inventory Logs        │
└───────────────────────────────────────────────────────────┘
```

### Architectural principles

The backend separates responsibilities into:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Dapper
    ↓
Stored Procedure
    ↓
SQL Server
```

This separation keeps HTTP concerns, business logic, data-access logic, and database operations independently maintainable.

---

# 👥 Application Roles

## Doctor

Doctors can:

* View their dashboard
* View today's and tomorrow's appointments
* Open consultation workflows
* Record consultation information
* Search patient history
* Review patient reports
* Access laboratory-related patient information

## Receptionist

Receptionists can:

* View operational dashboard
* Register patients
* Search and manage patients
* Schedule appointments
* View booked appointments
* Manage billing workflows
* View patient visits
* Generate operational reports

## Lab Technician

Laboratory users can:

* View laboratory dashboard
* View pending laboratory tests
* Search patients
* Enter laboratory results
* Review completed reports
* Generate laboratory bills
* Track payment status
* Generate/download PDF documents
* Resend report notifications

## Pharmacist

Pharmacy users can:

* View pharmacy dashboard
* Manage medicines
* Manage medicine categories and manufacturers
* Manage medicine batches
* Monitor inventory
* Identify low-stock medicines
* Identify expiring medicines
* Identify expired medicines
* Manage prescriptions
* Validate prescription stock
* Dispense medicines
* Generate pharmacy bills
* Cancel bills with a reason
* View dispensing history
* Review audit and inventory logs
* Generate operational reports
* Export reports as CSV
* Generate PDF invoices

---

# 🔐 Authentication & Authorization

The application uses **server-side session authentication** rather than storing authentication tokens in the browser.

After successful authentication, the API establishes a session cookie:

```text
User Login
    ↓
ASP.NET Core API
    ↓
Server-side Session
    ↓
Secure Session Cookie
    ↓
Angular HTTP Requests
```

The Angular application additionally implements:

* Authentication guard
* Role guard
* Credential interceptor
* Unauthorized-response interceptor
* Role-aware dashboard routing

Example role isolation:

```text
/doctor/*      → Doctor
/reception/*   → Receptionist
/lab/*         → Lab Technician
/pharmacy/*    → Pharmacist
```

Unauthorized users are redirected to the appropriate login/dashboard flow.

> **Important:** Client-side role information is used for UI routing only. Authentication is enforced by the backend session.

---

# 🧩 Major Modules

## 1. Patient Management

Provides a centralized patient workflow including:

* Patient registration
* Patient search
* Patient profiles
* Patient codes/MMR generation
* Patient updates
* Visit history
* Patient reports

---

## 2. Appointment Management

Supports:

* Appointment creation
* Doctor selection
* Department selection
* Appointment date/time selection
* Booked-slot validation
* Appointment cancellation
* Appointment filtering

---

## 3. Doctor & Consultation

The doctor workflow supports:

* Doctor dashboard
* Appointment queue
* Consultation setup
* Consultation submission
* Patient history search
* Patient report access

---

## 4. Laboratory Management

The laboratory module covers:

```text
Patient
   ↓
Doctor Consultation
   ↓
Lab Request
   ↓
Lab Test
   ↓
Lab Result
   ↓
Lab Report
   ↓
Lab Billing
```

Features include:

* Pending test queue
* Result entry
* Result details
* Report search
* Billing
* Payment status
* PDF generation
* Report notifications

---

## 5. Pharmacy Management

The pharmacy module is one of the largest parts of the system.

### Medicine Management

* Medicine catalog
* Categories
* Manufacturers
* Medicine creation
* Medicine editing
* Medicine disabling
* Medicine search

### Inventory Management

* Medicine batches
* Stock quantity tracking
* Stock updates
* Low-stock monitoring
* Expiry monitoring
* Expired-stock monitoring
* Inventory logging

### Prescription Workflow

```text
Doctor
  ↓
Prescription
  ↓
Prescription Items
  ↓
Stock Validation
  ↓
Medicine Dispensing
  ↓
Pharmacy Bill
  ↓
Invoice
```

### Pharmacy Reporting

Supported reports include:

* Sales summary
* Medicine-wise sales
* Stock status
* Expiry report
* Low-stock report
* Dispensing report

Reports can also be exported as CSV.

---

# 💳 Billing & Payments

The system contains billing workflows for both clinical/laboratory operations and pharmacy operations.

Capabilities include:

* Bill creation
* Bill items
* Payment processing
* Payment status tracking
* Bill cancellation
* Prescription-to-bill linking
* Invoice generation
* PDF invoice generation

PDF generation is handled server-side using **QuestPDF**.

---

# 📊 Reporting & Analytics

The application provides role-specific operational reporting.

Examples include:

```text
Pharmacy
├── Sales Summary
├── Medicine-wise Sales
├── Stock Status
├── Expiry
├── Low Stock
└── Dispensing

Reception
└── Operational Reports

Laboratory
├── Completed Reports
└── Billing Reports
```

Dashboard visualizations use **Chart.js** on the Angular frontend.

---

# 📝 Audit & Inventory Logging

The pharmacy system maintains operational traceability through:

* Audit logs
* Inventory logs
* Dispensing history
* Bill history
* Stock changes

This provides a foundation for tracking important business operations and improving system accountability.

---

# 🛠️ Technology Stack

## Frontend

| Technology          | Purpose                        |
| ------------------- | ------------------------------ |
| Angular 20          | SPA frontend                   |
| TypeScript          | Application development        |
| RxJS                | Reactive programming           |
| Angular Router      | Routing/navigation             |
| Angular Guards      | Authentication/role protection |
| Angular HTTP Client | API communication              |
| Bootstrap 4         | UI/layout                      |
| Chart.js            | Dashboard visualization        |
| Font Awesome        | Icons                          |
| jsPDF               | Client-side PDF utilities      |
| jsPDF AutoTable     | Tabular PDF generation         |

## Backend

| Technology               | Purpose                    |
| ------------------------ | -------------------------- |
| ASP.NET Core 8           | REST API                   |
| C#                       | Backend development        |
| Dapper 2.1               | Data access                |
| Microsoft.Data.SqlClient | SQL Server connectivity    |
| Swagger / OpenAPI        | API documentation          |
| QuestPDF                 | Server-side PDF generation |
| ASP.NET Core Session     | Authentication state       |
| CORS                     | Frontend/API integration   |

## Database

| Technology            | Purpose                            |
| --------------------- | ---------------------------------- |
| Microsoft SQL Server  | Primary database                   |
| T-SQL                 | Database programming               |
| Stored Procedures     | Data operations/business workflows |
| Relational Modeling   | Entity relationships               |
| Indexes & Constraints | Data integrity/performance         |

---

# 🗄️ Database Design

The supplied database contains **32 core tables**, including:

```text
Users
Roles
Staff
Doctors
DoctorSchedules
DoctorQualifications
Qualifications
Departments

Patients
PatientVisits
Appointments
Consultations

LabCategories
LabTests
LabRequests
LabRequestItems
LabResults
ReportNotifications

Bills
BillItems
Payments

Medicines
MedicineCategories
Manufacturers
MedicineStock
MedicineInventoryLogs

Prescriptions
PrescriptionItems
MedicineDispensing
MedicineDispensingItems

PharmacyBillPrescription
AuditLogs
```

The database is backed by **100+ stored procedures** for operations such as:

* Patient registration
* Patient search
* Appointment booking
* Appointment cancellation
* Doctor login
* Consultation creation
* Laboratory workflows
* Prescription creation
* Medicine management
* Stock management
* Prescription stock validation
* Medicine dispensing
* Pharmacy billing
* Payment processing
* Dashboard statistics
* Audit logging
* Inventory logging
* Reporting

This allows the application to keep complex database operations centralized and consistent.

---

# 📁 Backend Structure

```text
InfinityCoderzz_CMSV2026/
│
├── Controllers/
│   ├── AppointmentsController.cs
│   ├── PatientsController.cs
│   ├── PatientVisitsController.cs
│   ├── DoctorController.cs
│   ├── LabTechnicianController.cs
│   ├── BillsController.cs
│   ├── ReportsController.cs
│   ├── LoginController.cs
│   └── Pharmacy/
│
├── DTOs/
│   └── Pharmacy/
│
├── Models/
│   ├── Patient.cs
│   ├── Doctor.cs
│   ├── Appointment.cs
│   ├── Bill.cs
│   ├── Prescription.cs
│   └── pharmacist/
│
├── Repositories/
│   ├── PatientRepository.cs
│   ├── AppointmentRepository.cs
│   ├── DoctorRepository.cs
│   ├── BillRepository.cs
│   └── PharmacistRepositories/
│
├── Services/
│   ├── PatientServices.cs
│   ├── AppointmentService.cs
│   ├── DoctorService.cs
│   ├── BillService.cs
│   ├── LabTechnicianService.cs
│   └── PharmacyServices/
│
├── Helpers/
├── Program.cs
├── appsettings.json
└── InfinityCoderzz_CMSV2026.csproj
```

---

# 📁 Angular Structure

```text
src/app/
│
├── auth/
│   └── login/
│
├── doctor/
│   ├── dashboard/
│   ├── appointments/
│   ├── consultation/
│   └── patient-search/
│
├── lab/
│   ├── dashboard/
│   ├── pending-tests/
│   ├── results/
│   ├── billing/
│   ├── reports/
│   └── patient-search/
│
├── reception/
│   ├── dashboard/
│   ├── patients/
│   ├── appointments/
│   ├── bills/
│   ├── visits/
│   ├── reports/
│   └── register-patient/
│
├── pharmacy/
│   ├── dashboard/
│   ├── medicine/
│   ├── medicine-add/
│   ├── medicine-edit/
│   ├── medicine-stock/
│   ├── prescription/
│   ├── dispensing/
│   ├── billing/
│   ├── reports/
│   ├── inventory-log/
│   ├── audit-log/
│   └── bill-invoice/
│
├── guards/
├── interceptors/
├── services/
├── shared/
└── app.routes.ts
```

---

# 🔌 API Design

The backend exposes REST endpoints grouped by business capability.

Examples:

```http
POST   /api/login

GET    /api/patients
POST   /api/patients
PUT    /api/patients/{id}

GET    /api/appointments
POST   /api/appointments
POST   /api/appointments/{id}/cancel

GET    /api/doctor/dashboard
GET    /api/doctor/appointments
POST   /api/doctor/consultation

GET    /api/labtechnician/dashboard
GET    /api/labtechnician/pending-tests
POST   /api/labtechnician/results

GET    /api/pharmacist/dashboard
GET    /api/pharmacist/medicines
POST   /api/pharmacist/medicines

GET    /api/pharmacist/medicine-stock
POST   /api/pharmacist/medicine-stock

GET    /api/pharmacist/prescriptions
POST   /api/pharmacist/prescriptions/{id}/dispense

GET    /api/pharmacist/dispensing/history

GET    /api/pharmacist/bills
POST   /api/pharmacist/bills

GET    /api/pharmacist/reports
GET    /api/pharmacist/reports/export
```

The complete API can be explored through Swagger.

---

# 📖 API Documentation

When the backend is running, Swagger UI is available at:

```text
https://localhost:7037/swagger
```

Swagger provides interactive documentation for the available API endpoints and allows developers to execute requests directly from the browser.

---

# ⚙️ Local Development Setup

## Prerequisites

Install:

* .NET 8 SDK
* Node.js
* npm
* Angular CLI 20
* Microsoft SQL Server
* SQL Server Management Studio or another SQL client
* Git

---

# 1. Clone the repositories

```bash
git clone <backend-repository-url>
git clone <frontend-repository-url>
```

The recommended setup keeps the frontend and backend as separate repositories.

---

# 2. Configure SQL Server

Create the database:

```sql
CREATE DATABASE CMSv2026db;
```

Then execute the supplied:

```text
ScriptDBFinal(6).sql
```

script against `CMSv2026db`.

The script creates the database schema, tables, stored procedures, indexes, constraints, and related database objects required by the application.

---

# 3. Configure the Backend

Navigate to the API project:

```bash
cd InfinityCoderzz_CMSV2026
```

Restore dependencies:

```bash
dotnet restore
```

Configure the database connection using environment-specific configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<SQL_SERVER>;Database=CMSv2026db;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

Run the API:

```bash
dotnet run
```

The API should then be available through the HTTPS launch profile.

Swagger:

```text
https://localhost:7037/swagger
```

---

# 4. Configure the Angular Frontend

Navigate to the Angular application:

```bash
cd CMSV2026_Angular_FINAL_INTEGRATED
```

Install dependencies:

```bash
npm install
```

The development environment is configured to use:

```text
https://localhost:7037/api
```

Start Angular:

```bash
npm start
```

The frontend will normally be available at:

```text
http://localhost:4200
```

---

# 🔄 Frontend ↔ Backend Communication

The Angular application communicates with the ASP.NET Core API through the configured environment:

```typescript
apiUrl: 'https://localhost:7037/api'
```

Authentication requests use server-side session cookies.

Because the frontend and backend run on different origins during development, the backend is configured with CORS and credential support.

```text
Angular :4200
     │
     │ HTTP + Credentials
     ▼
ASP.NET Core :7037
     │
     │ Session
     ▼
SQL Server
```

---

# 🧪 Testing the Application

Recommended testing flow:

### Authentication

```text
Login
 ↓
Role Detection
 ↓
Role-specific Dashboard
```

### Reception

```text
Register Patient
 ↓
Create Appointment
 ↓
View Appointment
 ↓
Billing
```

### Doctor

```text
View Appointments
 ↓
Open Consultation
 ↓
Submit Consultation
 ↓
Review Patient History
```

### Laboratory

```text
View Pending Tests
 ↓
Enter Result
 ↓
Review Report
 ↓
Generate Bill
 ↓
Generate PDF
```

### Pharmacy

```text
View Prescription
 ↓
Check Stock
 ↓
Dispense Medicine
 ↓
Generate Bill
 ↓
Generate Invoice
 ↓
Review Reports
```

---

# 🔒 Security Notes

This project uses session-based authentication and role-aware application routing.

For any public deployment, the following should be implemented/configured appropriately:

* HTTPS
* Secure cookies
* HttpOnly cookies
* Environment-specific configuration
* Secret management
* Database credentials outside source control
* SMTP credentials outside source control
* Production CORS allowlist
* Proper production logging
* Input validation
* Rate limiting where appropriate
* Security headers
* Database least-privilege access

### ⚠️ Before publishing to GitHub

**Do not commit credentials or secrets.**

The development archive currently contains sensitive configuration in `appsettings.json`. Before making the repository public:

1. Remove SMTP credentials.
2. Remove database credentials/host-specific configuration.
3. Rotate any credentials that have already been exposed.
4. Move secrets to environment variables, .NET User Secrets, or a production secret manager.
5. Add sensitive configuration files to `.gitignore`.

Example:

```gitignore
# .NET
appsettings.Development.json
*.user
.vs/
bin/
obj/

# Angular
node_modules/
.angular/
dist/

# Environment / secrets
.env
.env.*
```

**Never publish a real SMTP app password, database password, API key, token, or private credential in GitHub.**

---

# 🧹 Repository Hygiene

For a professional public GitHub repository, generated files should not be committed.

Avoid committing:

```text
node_modules/
.angular/
dist/
bin/
obj/
.vs/
```

The repository should contain the source code, configuration templates, database scripts, documentation, tests, and CI/CD configuration rather than local IDE/build artifacts.

---

# 📈 Engineering Decisions

### Why Angular?

Angular provides:

* Structured application architecture
* Strong TypeScript support
* Dependency injection
* Route guards
* Reusable components
* HTTP interceptors
* Lazy loading
* Maintainable enterprise-scale organization

### Why ASP.NET Core Web API?

The API provides:

* Strongly typed backend development
* RESTful architecture
* Dependency injection
* Middleware pipeline
* Session management
* Swagger/OpenAPI support
* High-performance HTTP APIs

### Why Dapper?

Dapper provides lightweight SQL mapping while keeping explicit control over SQL Server operations and stored procedures.

This is particularly suitable for a system where database workflows are intentionally modeled through stored procedures.

### Why Stored Procedures?

The database layer centralizes many complex operations such as:

* Appointment booking
* Prescription workflows
* Stock validation
* Billing
* Reporting
* Audit logging

This also allows the application to reuse well-defined database operations from multiple backend services.

---

# 🎯 Key Engineering Concepts Demonstrated

This project demonstrates practical experience with:

* Full-stack application architecture
* REST API design
* Layered architecture
* Dependency Injection
* Repository pattern
* Service layer pattern
* DTO-based API contracts
* Role-based access control
* Session authentication
* Angular route guards
* HTTP interceptors
* CORS configuration
* SQL Server relational modeling
* Stored procedures
* Transactional business workflows
* Inventory management
* Billing systems
* Reporting systems
* PDF generation
* CSV export
* Audit logging
* Dashboard analytics
* Lazy loading
* Reusable frontend components
* Error/unauthorized request handling

---

# 🧭 Future Improvements

Potential next steps for production hardening include:

* JWT/OIDC authentication for distributed deployments
* ASP.NET Core Identity integration
* Automated unit and integration test coverage
* API integration tests
* Angular component/e2e testing
* Centralized structured logging
* Global exception handling middleware
* Redis-backed distributed sessions
* Dockerized development environment
* CI/CD deployment pipeline
* Health checks and observability
* OpenTelemetry tracing
* Production secret management
* Database migration/versioning strategy
* Automated API contract testing
* Performance benchmarking
* Accessibility improvements
* Production deployment on Azure/AWS/GCP

---

# 📌 Project Status

**Portfolio / Full-Stack Engineering Project**

The project is structured as a multi-role clinic management platform with working frontend, backend, and database layers.

It is intended to demonstrate the design and implementation of a non-trivial business application involving multiple user roles, transactional workflows, reporting, inventory, billing, authentication, and database-driven operations.

---

# 👨‍💻 Author

**InfinityCoderzz**

Full-Stack Development Project

Technologies:

```text
Angular
TypeScript
ASP.NET Core
C#
Dapper
SQL Server
REST APIs
Swagger
QuestPDF
Chart.js
```

---

# ⭐ If you find this project useful

Consider starring the repository and exploring the architecture, API contracts, database procedures, and role-specific workflows.

---

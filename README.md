#  CMS — Healthcare Management System

> A modular healthcare management platform built with **Angular 20**, **TypeScript**, and a role-based architecture for managing clinical, laboratory, reception, and pharmacy workflows.

![Angular](https://img.shields.io/badge/Angular-20-DD0031?logo=angular\&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-5.9-3178C6?logo=typescript\&logoColor=white)
![RxJS](https://img.shields.io/badge/RxJS-7.8-B7178C?logo=reactivex\&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-4.6-7952B3?logo=bootstrap\&logoColor=white)
![Chart.js](https://img.shields.io/badge/Chart.js-4.5-FF6384?logo=chart.js\&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?logo=microsoftsqlserver\&logoColor=white)

## Overview

**CMS** is a full-featured healthcare management system designed to digitize and coordinate day-to-day hospital and clinic operations across multiple user roles.

The application follows a **role-based modular architecture**, providing separate workflows and dashboards for:

* **Doctors**
* **Lab Technicians**
* **Receptionists**
* **Pharmacists**

The frontend is implemented using **Angular 20 standalone components**, with centralized services, route guards, HTTP interceptors, reusable UI components, and lazy-loaded feature modules.

The system is designed around a session-authenticated Web API and a relational **Microsoft SQL Server** database.

---

## Key Objectives

The project focuses on solving common healthcare workflow problems through a centralized application:

* Patient registration and management
* Appointment scheduling and tracking
* Doctor consultation workflows
* Laboratory test requests and results
* Pharmacy inventory management
* Prescription and medicine dispensing
* Billing and payment workflows
* Operational reports and dashboards
* Audit and inventory tracking
* Role-based access control
* PDF-based invoice/report generation

The architecture emphasizes **separation of concerns, reusable components, secure route access, maintainability, and extensibility**.

---

# Core Modules


## 1. Pharmacy Module

The Pharmacy module provides end-to-end medicine and pharmacy operations.

### Features

* Pharmacy dashboard
* Medicine management
* Medicine creation and editing
* Stock management
* Low-stock monitoring
* Expiring medicine tracking
* Expired medicine tracking
* Prescription management
* Medicine dispensing
* Dispensing history
* Pharmacy billing
* Invoice generation
* Inventory logs
* Audit logs
* Pharmacy reports

### Routes

```text
/pharmacy/dashboard
/pharmacy/medicine
/pharmacy/medicine/add
/pharmacy/medicine/edit/:id
/pharmacy/stock
/pharmacy/stock/add
/pharmacy/stock/edit/:id
/pharmacy/stock/low-stock
/pharmacy/stock/expiring
/pharmacy/stock/expired
/pharmacy/prescription
/pharmacy/prescription/:id
/pharmacy/dispensing
/pharmacy/dispensing/history
/pharmacy/dispensing/:id/items
/pharmacy/bills
/pharmacy/bills/create
/pharmacy/bills/:id
/pharmacy/reports
/pharmacy/inventory-log
/pharmacy/audit-log
```

<details open>
<summary><strong>View all Pharmacy UI screenshots</strong></summary>


## Pharmacy Dashboard

The dashboard combines medicine/stock KPIs, pending prescriptions,
revenue, dispensing charts, low-stock alerts, and expiry alerts.

![Pharmacy Dashboard](docs/screenshots/pharmacy/01-pharmacy-dashboard.png)

## Medicine Management

Medicine records support creation, editing, generated medicine codes,
validation, and soft disabling.

![Medicine Management](docs/screenshots/pharmacy/02-medicine-catalogue.png)

![Medicine Management](docs/screenshots/pharmacy/03-medicine-management.png)

## Stock & Batch Management

Stock is tracked by batch and expiry, with low-stock, expiring-soon, and
expired views.

![Stock Management](docs/screenshots/pharmacy/04-stock-inventory.png)

![Stock Management](docs/screenshots/pharmacy/05-stock-management.png)

## Prescription Dispensing

The core dispensing workflow performs stock checking, applies **FEFO
(First Expired First Out)**, deducts stock, creates billing information,
and updates the prescription status.

![Medicine Dispensing](docs/screenshots/pharmacy/06-prescription-management.png)

![Medicine Dispensing](docs/screenshots/pharmacy/07-medicine-dispensing-core-flow.png)

## Pharmacy Billing & PDF Invoice

Pharmacy bills support detailed billing, invoice generation,
printing/download, and prescription linkage.

![Pharmacy Billing](docs/screenshots/pharmacy/09-dispensing-history-otc-billing.png)

![Pharmacy Billing](docs/screenshots/pharmacy/10-billing-system-pdf-invoice.png)

## Reports & Analytics

The pharmacy reporting layer covers sales, medicine-wise sales, stock
status, expiry, low-stock, and dispensing reports with CSV export.

![Pharmacy Reports](docs/screenshots/pharmacy/11-reports-analytics.png)


## Inventory & Audit log 

![Pharmacy Reports](docs/screenshots/pharmacy/12-inventory-audit-log.png)


</details>

---

## 2. Reception Module

The Reception module manages front-desk and patient-flow operations.

### Features

* Reception dashboard
* Patient registration
* Patient search and management
* Appointment management
* Patient visits
* Billing information
* Operational reports

### Routes

```text
/reception/dashboard
/reception/register-patient
/reception/patients
/reception/appointments
/reception/bills
/reception/visits
/reception/reports
```

<details open>
<summary><strong>View all 17 Receptionist screens</strong></summary>

### 01 — Dashboard
![Receptionist Dashboard](docs/screenshots/receptionist/01-dashboard.png)

### 02 — Quick Patient Search
![Quick Patient Search](docs/screenshots/receptionist/02-quick-patient-search.png)


### 04 — Choose Appointment Slot
![Choose Slot](docs/screenshots/receptionist/04-choose-slot.png)

### 09 — Appointment for New Patient
![New Patient Appointment](docs/screenshots/receptionist/09-new-patient-appointment.png)


### 11 — Create Bill from Appointment
![Create Bill](docs/screenshots/receptionist/11-create-bill.png)

### 12 — Bill Details & Payment History
![Bill Details](docs/screenshots/receptionist/12-bill-payment-history.png)

### 13 — Printable Invoice
![Printable Invoice](docs/screenshots/receptionist/13-printable-invoice.png)

### 15 — Patient Visits Log
![Patient Visits](docs/screenshots/receptionist/15-patient-visits.png)


</details>

---

## 3. Doctor Module

The Doctor workspace provides tools for managing clinical activities.

### Features

* Doctor dashboard
* Appointment management
* Patient search
* Consultation workflow
* Patient/visit information
* Laboratory request interaction
* Consultation notes and follow-up information
* Role-protected navigation

### Routes

```text
/doctor/dashboard
/doctor/appointments
/doctor/consultation/:appointmentId
/doctor/patient-search
```

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

---


## 4. Laboratory Module

The Laboratory workspace supports the lifecycle of diagnostic requests and results.

### Features

* Laboratory dashboard
* Pending test management
* Test result entry
* Patient search
* Laboratory billing
* Laboratory reports
* Test/request tracking

### Routes

```text
/lab/dashboard
/lab/pending-tests
/lab/results/:requestItemId
/lab/billing
/lab/reports
/lab/patient-search
```

<details open>
<summary><strong>View all 10 Laboratory screens</strong></summary>


### 02 — Lab Dashboard
![Lab Dashboard](docs/screenshots/lab/01-dashboard.jpeg)

### 03 — Pending Tests
![Pending Tests](docs/screenshots/lab/02-pending-tests.jpeg)

### 04 — Enter Lab Result
![Enter Lab Result](docs/screenshots/lab/03-enter-result.jpeg)


### 07 — Lab Bill Detail
![Lab Bill Detail](docs/screenshots/lab/06-billing-detail.jpeg)



### 09 — Laboratory Report Detail
![Laboratory Report Detail](docs/screenshots/lab/08-report-detail.jpeg)


</details>

---



# Architecture

The frontend uses a feature-oriented Angular architecture.

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
│   ├── visits/
│   ├── bills/
│   └── reports/
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
│   └── audit-log/
│
├── guards/
│   ├── auth-guard.ts
│   └── role-guard.ts
│
├── interceptors/
│   ├── credentials-interceptor.ts
│   └── unauthorized-interceptor.ts
│
├── services/
│   ├── auth-service.ts
│   ├── doctor-service.ts
│   ├── lab-service.ts
│   ├── reception-service.ts
│   └── pharmacy-service.ts
│
└── shared/
    ├── animations/
    ├── count-up/
    ├── header/
    ├── notfound/
    ├── skeleton/
    ├── stat-card/
    ├── toast/
    └── voice-search/
```

---

# Technology Stack

## Frontend

| Technology       | Purpose                               |
| ---------------- | ------------------------------------- |
| Angular 20       | Application framework                 |
| TypeScript 5.9   | Application development               |
| RxJS 7.8         | Reactive programming and HTTP streams |
| Angular Router   | Navigation and protected routes       |
| Angular Forms    | Form handling and validation          |
| Bootstrap 4.6    | Responsive UI                         |
| Font Awesome 4.7 | Icons                                 |
| Chart.js 4.5     | Dashboard visualization               |
| jsPDF            | PDF generation                        |
| jsPDF AutoTable  | Tabular PDF documents                 |

## Backend Integration

The Angular application communicates with a separate **ASP.NET Core Web API**.

The API is accessed through Angular services and configured through environment-specific API URLs.

```text
Angular Application
        │
        │ HTTP / HTTPS
        ▼
ASP.NET Core Web API
        │
        ▼
Microsoft SQL Server
```

---

# Authentication & Authorization

The application uses **server-side session authentication with HTTP cookies** rather than JWT-based authentication.

### Authentication flow

```text
User
 │
 ▼
Login Page
 │
 │ POST /api/login
 ▼
ASP.NET Core API
 │
 │ Session Cookie
 ▼
Angular Application
 │
 ├── Auth Guard
 ├── Role Guard
 └── Credentials Interceptor
       │
       ▼
Protected API Endpoints
```

The application supports four primary roles:

```text
Doctor
Lab Technician
Receptionist
Pharmacist
```

Angular route guards prevent unauthorized navigation.

For example:

```text
/doctor/*       → Doctor
/lab/*          → Lab Technician
/reception/*    → Receptionist
/pharmacy/*     → Pharmacist
```

The credentials interceptor ensures authenticated API requests include the required session credentials.

The unauthorized interceptor handles HTTP `401` responses and redirects the user back to the login flow.

> **Important:** Role information stored in `localStorage` is used for client-side navigation and UI behavior only. The server-side session remains the authentication mechanism for API requests.

---

# Database

The supplied SQL Server schema is designed around healthcare operational workflows and contains **32 relational tables**.

Major database domains include:

### Identity & Access

```text
Users
Roles
Staff
```

### Patients & Clinical Operations

```text
Patients
PatientVisits
Appointments
Consultations
Doctors
Departments
DoctorSchedules
DoctorQualifications
Qualifications
```

### Laboratory

```text
LabCategories
LabTests
LabRequests
LabRequestItems
LabResults
```

### Pharmacy

```text
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
```

### Billing & Payments

```text
Bills
BillItems
Payments
```

### Auditing & Notifications

```text
AuditLogs
ReportNotifications
```

The database uses primary keys, foreign-key relationships, unique constraints, identity columns, and relational integrity rules to model dependencies between healthcare entities.

---

# Project Structure

```text
CMSV2026_Angular_FINAL_INTEGRATED/
│
├── public/
│
├── src/
│   ├── app/
│   │   ├── auth/
│   │   ├── doctor/
│   │   ├── lab/
│   │   ├── pharmacy/
│   │   ├── reception/
│   │   ├── guards/
│   │   ├── interceptors/
│   │   ├── services/
│   │   └── shared/
│   │
│   ├── environments/
│   │   ├── environment.ts
│   │   └── environment.prod.ts
│   │
│   ├── main.ts
│   ├── index.html
│   └── styles.scss
│
├── angular.json
├── package.json
├── package-lock.json
├── tsconfig.json
└── README.md
```

---

# Getting Started

## Prerequisites

Install the following before running the application:

* **Node.js** — LTS version recommended
* **npm**
* **Angular CLI 20**
* **.NET SDK** compatible with the backend
* **Microsoft SQL Server**
* **SQL Server Management Studio (SSMS)** or another SQL client

Verify the installations:

```bash
node --version
npm --version
ng version
dotnet --version
```

---

# Backend Setup

The Angular application expects the ASP.NET Core Web API to be available separately.

The development API URL used by the frontend is:

```text
https://localhost:7037/api
```

If your backend uses a different URL or port, update:

```text
src/environments/environment.ts
```

Example:

```typescript
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7037/api'
};
```

Because the application uses secure session cookies during development, the backend should be run through its HTTPS profile.

For local ASP.NET Core development:

```bash
dotnet dev-certs https --trust
dotnet run --launch-profile https
```

---

# Database Setup

Create a SQL Server database named:

```text
CMSv2026db
```

Then execute the supplied SQL script:

```text
ScriptDBFinal(10).sql
```

The script creates the application's relational schema, constraints, indexes, and associated database objects.

> Do not commit production credentials, connection strings, passwords, API secrets, or real patient information to the repository.

---

# Frontend Installation

Clone the repository:

```bash
git clone <YOUR_REPOSITORY_URL>
cd CMSV2026_Angular_FINAL_INTEGRATED
```

Install dependencies:

```bash
npm install
```

Start the Angular development server:

```bash
npm start
```

Or:

```bash
ng serve
```

The application will normally be available at:

```text
http://localhost:4200
```

---

# Production Build

Create an optimized production build:

```bash
npm run build
```

Angular will generate the production artifacts under:

```text
dist/
```

Before deployment, configure:

```text
src/environments/environment.prod.ts
```

with the correct production API endpoint.

Example:

```typescript
export const environment = {
  production: true,
  apiUrl: '/api'
};
```

---

# Available NPM Commands

| Command         | Description                            |
| --------------- | -------------------------------------- |
| `npm start`     | Start Angular development server       |
| `npm run build` | Create production build                |
| `npm run watch` | Build continuously in development mode |
| `npm test`      | Run Angular unit tests                 |
| `ng serve`      | Start development server directly      |
| `ng build`      | Build Angular application directly     |

---

# API Integration

Backend communication is organized through dedicated services rather than embedding API calls directly throughout the UI.

```text
AuthService
DoctorService
LabService
ReceptionService
PharmacyService
PDFExportService
```

This separation keeps presentation components focused on UI behavior while API communication remains centralized and easier to maintain.

---

# Engineering Practices Demonstrated

This project demonstrates several practices relevant to production-oriented frontend development:

* Feature-based Angular architecture
* Standalone Angular components
* Lazy-loaded feature components
* Route guards
* Role-based authorization at the routing layer
* HTTP interceptors
* Centralized API services
* Environment-specific configuration
* Reusable shared components
* Reactive programming with RxJS
* Responsive UI design
* Dashboard visualization
* PDF document generation
* Form validation
* Loading/skeleton states
* Toast-based user feedback
* Error/unauthorized handling
* Inventory and audit tracking
* Relational database design

---

# Security Considerations

The project is structured with several security-oriented controls:

* Session-based authentication
* Secure HTTPS development configuration
* Credentialed API requests
* Authentication guards
* Role-based route guards
* Unauthorized-response interception
* Separation of authentication state from UI role state
* Server-side authorization dependency

For production deployment, additional controls should be implemented according to the deployment environment and organizational security requirements, including:

* HTTPS everywhere
* Secure cookie configuration
* CSRF protection where applicable
* Strong password hashing
* Secrets management
* Production CORS configuration
* Server-side authorization on every protected endpoint
* Input validation
* Rate limiting
* Security headers
* Centralized logging and monitoring
* Backup and recovery policies
* Data protection and healthcare privacy requirements

---

# Testing

The project includes Angular's testing infrastructure and test dependencies:

```text
Jasmine
Karma
Angular testing utilities
```

Run tests with:

```bash
npm test
```

For a production-grade deployment, the test suite should be expanded to cover:

* Authentication flows
* Route guards
* Role authorization
* Service/API behavior
* Form validation
* Pharmacy inventory calculations
* Billing calculations
* Component interactions
* Error handling
* Critical end-to-end workflows

---

# Development Workflow

A recommended development workflow is:

```text
1. Create feature branch
        ↓
2. Implement feature
        ↓
3. Add/update tests
        ↓
4. Run lint/build/tests
        ↓
5. Review changes
        ↓
6. Commit with meaningful message
        ↓
7. Open Pull Request
        ↓
8. Code review
        ↓
9. Merge
```

Example branch naming:

```text
feature/pharmacy-inventory
feature/doctor-consultation
feature/lab-results
fix/session-authentication
refactor/pharmacy-service
```

Example commit messages:

```text
feat: add pharmacy stock monitoring
feat: implement doctor consultation workflow
fix: handle unauthorized API responses
refactor: centralize pharmacy API calls
test: add medicine validation coverage
docs: update local development setup
```

---

# Screens & User Experience

The application provides role-specific dashboards and navigation instead of exposing every feature to every user.

This approach improves:

* Usability
* Information hierarchy
* Security boundaries
* Maintainability
* Role-specific workflows

Shared UI functionality includes reusable:

* Header/navigation
* Statistics cards
* Toast notifications
* Skeleton/loading states
* Not-found page
* Animations
* Voice-search component

---

# Project Highlights

### Modular Architecture

Each business domain is isolated into its own feature area, making the application easier to extend without creating a monolithic component structure.

### Role-Based Workflows

Doctors, laboratory technicians, receptionists, and pharmacists receive different workflows and protected application routes.

### Healthcare Workflow Coverage

The system connects patient management, appointments, consultations, laboratory processing, pharmacy operations, and billing into a unified workflow.

### Session-Based Security

Authentication is handled through server-side sessions and credentialed HTTP requests rather than relying on client-side authentication state.

### Data-Driven Dashboards

Dashboard components provide operational summaries and visualizations for different roles.

### Pharmacy Management

The pharmacy module covers the complete medicine lifecycle from inventory management through prescription processing, dispensing, billing, reporting, and auditing.

### Document Generation

The application includes PDF generation capabilities for operational documents such as invoices and reports.

---

# Future Improvements

Potential next steps for taking the project closer to enterprise production standards include:

* Comprehensive unit and integration test coverage
* End-to-end testing with Playwright or Cypress
* Strongly typed API DTO models
* Global error handling strategy
* Centralized application state where appropriate
* Accessibility improvements following WCAG guidelines
* CI/CD pipeline
* Docker-based development and deployment
* Automated database migrations
* API documentation with OpenAPI/Swagger
* Structured application logging
* Performance monitoring
* Security scanning
* Automated dependency updates
* Production observability and monitoring
* Containerized deployment
* Cloud deployment configuration

---

# Repository Hygiene

The repository should **not** contain:

```text
node_modules/
dist/
.angular/
cache/
environment secrets
database passwords
API keys
private certificates
real patient data
production credentials
```

Use environment variables, deployment configuration, or a secure secrets manager for sensitive configuration.

---

# Portfolio / Engineering Context

This project demonstrates practical experience building a multi-role business application rather than a single-page CRUD demonstration.

It brings together:

```text
Frontend Architecture
        +
Authentication
        +
Authorization
        +
REST API Integration
        +
Relational Database Design
        +
Healthcare Workflows
        +
Reporting
        +
PDF Generation
        +
Inventory Management
        +
Billing
        +
Auditability
```

The project is particularly useful as a portfolio demonstration of **Angular application architecture, TypeScript development, API integration, role-based access control, database-driven application design, and enterprise-style feature organization**.

---


## Author

Built as a full-stack healthcare management application demonstrating modern Angular development, modular frontend architecture, API integration, role-based workflows, and relational data management.

---

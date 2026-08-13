# InfinityCoderzz CMS — Web API (converted from MVC)

This is your original **InfinityCoderzz_CMSV2026** Clinic Management System, converted
from an ASP.NET Core **MVC** app into an ASP.NET Core **Web API**, with **Swagger**
enabled so every endpoint can be browsed and tested from the browser.

## What changed

- **Models, Repositories, Services** — untouched. All business logic and data access
  (Dapper + your stored procedures) is exactly as before.
- **Controllers** — rewritten to inherit `ControllerBase` (not `Controller`) with
  `[ApiController]` + `[Route("api/[controller]")]`, and every `View(...)` /
  `RedirectToAction(...)` replaced with `Ok(...)`, `NotFound()`, `BadRequest(...)`,
  `CreatedAtAction(...)`, etc. returning JSON.
- **Views / wwwroot Razor assets** — removed (an API has no views). `QuestPDF`
  bill-PDF generation (already done in the service layer) is kept and still returns
  a real PDF file from `GET /api/labtechnician/billing/{billId}/pdf`.
- **Rotativa** — removed; it was only used for view-to-PDF rendering, which no
  longer applies.
- **Program.cs** — `AddControllersWithViews()` → `AddControllers()`, plus
  `AddEndpointsApiExplorer()` + `AddSwaggerGen()`/`UseSwagger()`/`UseSwaggerUI()`.
  Session (`AddSession`) is kept because `Doctor`, `Login`, and `Receptionists`
  still rely on session-based login state.
- **csproj** — added `Swashbuckle.AspNetCore` for Swagger.

## Running it

```
cd InfinityCoderzz_CMSV2026
dotnet restore
dotnet run
```

Then open **`https://localhost:7037/swagger`** (or whatever port the console prints) —
it launches there automatically. Every controller/action is listed and can be
tried directly ("Try it out" → fill params → "Execute").

Update the connection string in `appsettings.json` (`ConnectionStrings:DefaultConnection`)
to point at your SQL Server instance before running — it's currently a copy of
your original `ARUNDHATHY\SQLEXPRESS` value.

> ⚠️ `appsettings.json` also still contains your Gmail SMTP **app password** in
> plain text (carried over unchanged from the original project). Before you commit
> this anywhere public, move it to `dotnet user-secrets` / environment variables
> and rotate the password.

## Session-based login (Doctor / Login / Receptionists)

`POST /api/login` and `POST /api/doctor/login` set an ASP.NET Core session cookie
on success. Swagger UI runs same-origin, so it sends that cookie automatically on
later calls in the same browser tab — log in first, then call the
dashboard/appointments/etc. endpoints right after, in the same Swagger session.

## Endpoint map (MVC action → API route)

**Appointments** (`/api/appointments`)
- `GET /api/appointments` — list/filter (was `Index`)
- `GET /api/appointments/doctors` — active doctors + departments for dropdowns
- `GET /api/appointments/create-data?patientId=` — data for a new-appointment form
- `POST /api/appointments` — book (was `Create` POST + `AppointmentSuccess`)
- `GET /api/appointments/booked-slots?doctorId=&appointmentDate=` — was `GetBookedSlots`
- `GET /api/appointments/{id}` — was `Details`
- `POST /api/appointments/{id}/cancel` — was `Cancel`

**Bills** (`/api/bills`)
- `GET /api/bills` — was `Index`
- `GET /api/bills/create-data?appointmentId=` — data for a new-bill form
- `POST /api/bills` — was `Create` POST
- `GET /api/bills/{id}` — was `Details`
- `POST /api/bills/{id}/payments` — was `ReceivePayment`

**Patients** (`/api/patients`)
- `GET /api/patients` — all patients
- `GET /api/patients/search?searchBy=&searchText=` — was `Search`
- `GET /api/patients/next-code` — the generated MMR shown on the `Create` form
- `POST /api/patients` — was `Create` POST
- `GET /api/patients/{id}` — was `Details`
- `PUT /api/patients/{id}` — was `Edit` POST

**PatientVisits** (`/api/patientvisits`)
- `GET /api/patientvisits` — was `Index`
- `GET /api/patientvisits/{id}` — was `Details`

**Login** (`/api/login`)
- `POST /api/login` — body `{ "username", "password" }`; sets session cookie,
  returns `next` (which dashboard route to call)
- `POST /api/login/logout`

**Doctor** (`/api/doctor`) — requires the session set by `/api/login` or `/api/doctor/login`
- `POST /api/doctor/login`, `POST /api/doctor/logout`
- `GET /api/doctor/dashboard`
- `GET /api/doctor/appointments?targetDay=today|tomorrow`
- `GET /api/doctor/consultation/setup?appointmentId=`
- `POST /api/doctor/consultation` — was `SubmitConsultation`; now returns the
  final summary directly instead of stashing it in `TempData`
- `GET /api/doctor/patients/search?searchKeyword=` — was `HistoryAndReports`
- `GET /api/doctor/patients/{mmrCode}/report` — was `ViewLabReportDetails` / `PrintReport`

**LabTechnician** (`/api/labtechnician`)
- `GET /api/labtechnician/dashboard`
- `GET /api/labtechnician/pending-tests?searchMMR=`
- `GET /api/labtechnician/results/{requestItemId}` — was `EnterResult` GET
- `POST /api/labtechnician/results` — was `EnterResult` POST (antiforgery removed — not applicable to an API)
- `GET /api/labtechnician/reports?searchMMR=` — was `ReportsDashboard`
- `GET /api/labtechnician/results/{resultId}/detail` — was `DownloadReportPdf` (now returns JSON data; use `.../billing/{billId}/pdf` for an actual file)
- `POST /api/labtechnician/results/{resultId}/resend-email?searchMMR=`
- `GET /api/labtechnician/billing?searchMMR=`
- `POST /api/labtechnician/billing/generate?searchMMR=` — body `{ "requestId" }`
- `PUT /api/labtechnician/billing/{billId}/payment-status` — body `{ "paymentStatus" }`
- `GET /api/labtechnician/billing/{billId}` — was `PrintBill`
- `GET /api/labtechnician/billing/{billId}/pdf` — real PDF file download (`DownloadBillPdf`, QuestPDF)
- `GET /api/labtechnician/patients/search?term=` — was `SearchPatientByMMR`

**Receptionists** (`/api/receptionists`)
- `GET /api/receptionists/dashboard`

**ReceptionistReports** (`/api/receptionistreports`)
- `GET /api/receptionistreports?reportType=&fromDate=&toDate=&doctorId=&departmentId=`

**Pharmacist** (`/api/pharmacist/...`) — requires the `PharmacistId` session set by `/api/login`
(role `Pharmacist`)

- `GET /api/pharmacist/dashboard`

- `GET /api/pharmacist/medicines?searchTerm=` — catalog list/search
- `GET /api/pharmacist/medicines/new` — categories + manufacturers + next auto-code, for a create form
- `POST /api/pharmacist/medicines` — was `Create` POST
- `GET /api/pharmacist/medicines/{id}` — was `Edit` GET
- `PUT /api/pharmacist/medicines/{id}` — was `Edit` POST
- `POST /api/pharmacist/medicines/{id}/disable`

- `GET /api/pharmacist/medicine-stock` — all batches
- `GET /api/pharmacist/medicine-stock/new` — medicines list, for a create form
- `POST /api/pharmacist/medicine-stock` — add a batch (same validation rules as the MVC version)
- `GET /api/pharmacist/medicine-stock/{id}` — was `Edit` GET
- `PUT /api/pharmacist/medicine-stock/{id}` — was `Edit` POST
- `GET /api/pharmacist/medicine-stock/low-stock`
- `GET /api/pharmacist/medicine-stock/expiring`
- `GET /api/pharmacist/medicine-stock/expired`

- `GET /api/pharmacist/prescriptions` — was `List`
- `GET /api/pharmacist/prescriptions/{id}` — was `Details` (returns prescription + items)
- `POST /api/pharmacist/prescriptions/{id}/dispense` — was `MarkDispensed`
- `PUT /api/pharmacist/prescriptions/{id}/status` — body `{ "status" }`, was `UpdateStatus`

- `GET /api/pharmacist/dispensing` — dispensable prescriptions, was `Create` GET
- `POST /api/pharmacist/dispensing` — body `{ "prescriptionId", "remarks" }`, was `DispenseAndBill`
- `GET /api/pharmacist/dispensing/history`
- `GET /api/pharmacist/dispensing/{dispenseId}/items`

- `GET /api/pharmacist/bills` — was `List`
- `GET /api/pharmacist/bills/new` — patients + billable medicines, for a create form
- `POST /api/pharmacist/bills` — was `Create` POST
- `GET /api/pharmacist/bills/{id}` — was `Details` (bill + items + prescription link)
- `POST /api/pharmacist/bills/{id}/cancel` — body `{ "reason" }`
- `GET /api/pharmacist/bills/{id}/invoice` — printable JSON payload
- `GET /api/pharmacist/bills/{id}/invoice/pdf` — real PDF file download (QuestPDF)

- `GET /api/pharmacist/inventory-logs`
- `GET /api/pharmacist/audit-logs?fromDate=&toDate=`

- `GET /api/pharmacist/reports?report=sales|medicinewise|stock|expiry|lowstock|dispensing&fromDate=&toDate=&days=`
- `GET /api/pharmacist/reports/export?report=...` — CSV file download

> ⚠️ The uploaded MVC `LoginController` set `HttpContext.Session["PharmacistId"]`
> only for the `Pharmacist` role. The API's `LoginController` had that block, so it
> has been added — `POST /api/login` with a Pharmacist account now sets
> `PharmacistId`/`PharmacistName` in session exactly like the original, which the
> pharmacy endpoints above depend on.

## Not carried over

- `Admin` dashboard was referenced by `LoginController`'s redirect logic in the
  original project but had no controller/actions in the codebase you uploaded —
  same in this version (the login response just returns its intended route name;
  you'll need to add that controller when that part of the app exists).

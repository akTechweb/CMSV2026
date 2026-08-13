using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using InfinityCoderzz_CMSV2026.Repositories;
using InfinityCoderzz_CMSV2026.Services;
using InfinityCoderzzz_CMSV2026.Repositories;
using InfinityCoderzzz_CMSV2026.Services;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories;
using InfinityCoderzz_CMSV2026.Repositories.PharmacistRepositories.Interfaces;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices;
using InfinityCoderzz_CMSV2026.Services.PharmacyServices.Interfaces;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

// Add Web API controllers (no Views)
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "InfinityCoderzz CMS API",
        Version = "v1",
        Description = "Web API for the Clinic Management System (converted from the original MVC project)."
    });

    // Allow Swagger to show the session cookie so login flows can be tested
    options.AddSecurityDefinition("SessionCookie", new OpenApiSecurityScheme
    {
        Name = ".InfinityClinic.Session",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Cookie,
        Description = "Session cookie set after a successful login (api/login or api/doctor/login)."
    });
});

// Session Services (still used by Doctor/Login/Receptionist flows)
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // The Angular app runs on a different origin/port (e.g. http://localhost:4200)
    // than this API. A cross-site fetch/XHR only carries a cookie back if it is
    // SameSite=None (Lax is silently dropped for cross-origin calls), and
    // SameSite=None requires Secure - which in turn requires HTTPS. Run this API
    // via the "https" launch profile (https://localhost:7037) when working with
    // the Angular app; see the Angular project's README for details.
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

// CORS - allows the Angular app (ng serve on :4200, or a configured prod origin)
// to call the API WITH the session cookie. Browsers reject AllowAnyOrigin()
// combined with credentialed requests, so specific origins must be listed.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Dependency Injection
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<IBillRepository, BillRepository>();
builder.Services.AddScoped<IBillService, BillService>();

builder.Services.AddScoped<IPatientVisitRepository, PatientVisitRepository>();
builder.Services.AddScoped<IPatientVisitService, PatientVisitService>();

builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();

builder.Services.AddScoped<ILabTechnicianRepository, LabTechnicianRepository>();
builder.Services.AddScoped<ILabTechnicianService, LabTechnicianService>();

// Pharmacy module
builder.Services.AddScoped<IMedicineRepository, MedicineRepositoryImpl>();
builder.Services.AddScoped<IMedicineService, MedicineServiceImpl>();

builder.Services.AddScoped<IMedicineStockRepository, MedicineStockRepositoryImpl>();
builder.Services.AddScoped<IMedicineStockService, MedicineStockServiceImpl>();

builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepositoryImpl>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionServiceImpl>();

builder.Services.AddScoped<IMedicineDispensingRepository, MedicineDispensingRepositoryImpl>();
builder.Services.AddScoped<IMedicineDispensingService, MedicineDispensingServiceImpl>();

builder.Services.AddScoped<IPharmacyBillRepository, BillRepositoryImpl>();
builder.Services.AddScoped<IPharmacyBillService, BillServiceImpl>();

builder.Services.AddScoped<IAuditLogRepository, AuditLogRepositoryImpl>();
builder.Services.AddScoped<IAuditLogService, AuditLogServiceImpl>();

builder.Services.AddScoped<IInventoryLogRepository, InventoryLogRepositoryImpl>();
builder.Services.AddScoped<IInventoryLogService, InventoryLogServiceImpl>();

builder.Services.AddScoped<IPharmacyDashboardRepository, PharmacyDashboardRepositoryImpl>();
builder.Services.AddScoped<IPharmacyDashboardService, PharmacyDashboardServiceImpl>();

builder.Services.AddScoped<IReportRepository, ReportRepositoryImpl>();
builder.Services.AddScoped<IReportService, ReportServiceImpl>();

builder.Services.AddScoped<IPharmacyLoginRepository, PharmacyLoginRepositoryImpl>();
builder.Services.AddScoped<IPharmacyLoginService, PharmacyLoginServiceImpl>();

var app = builder.Build();

// Swagger is enabled in all environments so it can be used to demo/test the API
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "InfinityCoderzz CMS API v1");
    options.RoutePrefix = "swagger";
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");

app.UseRouting();

// Session Middleware
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();

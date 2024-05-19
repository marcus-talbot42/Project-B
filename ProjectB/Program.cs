using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectB.Database;
using ProjectB.Repositories;
using ProjectB.Services;
using ProjectB.Views.Admin;
using ProjectB.Views.Debug;
using ProjectB.Views.Language;
using ProjectB.Views.Login;
using ProjectB.Views.Main;
using ProjectB.Views.Reservation;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Add in-memory database
builder.Services.AddDbContext<DatabaseContext>(options => options.UseInMemoryDatabase("ProjectBDatabase"));

// Repostitories
builder.Services.AddSingleton<GuestRepository>();
builder.Services.AddSingleton<EmployeeRepository>();
builder.Services.AddSingleton<TourRepository>();
builder.Services.AddSingleton<TranslationRepository>();

// Services
builder.Services.AddSingleton<GuestService>();
builder.Services.AddSingleton<EmployeeService>();
builder.Services.AddSingleton<TourService>();
builder.Services.AddSingleton<TranslationService>();

// Views
builder.Services.AddTransient<CreateReservationView>();
builder.Services.AddTransient<EditReservationView>();
builder.Services.AddTransient<DeleteReservationView>();
builder.Services.AddTransient<ReservationView>();
builder.Services.AddTransient<EmployeeLoginView>();
builder.Services.AddTransient<GuestLoginView>();
builder.Services.AddTransient<CreateGuestView>();
builder.Services.AddTransient<CreateEmployeeView>();
builder.Services.AddTransient<DebugView>();
builder.Services.AddTransient<MainMenuView>();
builder.Services.AddTransient<LanguageSwitcher>();

using IHost host = builder.Build();

host.Services.GetService<MainMenuView>()!.Output();
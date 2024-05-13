using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectB.Repositories;
using ProjectB.Services;
using ProjectB.Views.Admin;
using ProjectB.Views.Debug;
using ProjectB.Views.Login;
using ProjectB.Views.Main;
using ProjectB.Views.Reservation;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
// Repostitories
builder.Services.AddSingleton<GuestRepository>();
builder.Services.AddSingleton<EmployeeRepository>();
builder.Services.AddSingleton<TourRepository>();

// Services
builder.Services.AddSingleton<GuestService>();
builder.Services.AddSingleton<EmployeeService>();
builder.Services.AddSingleton<TourService>();

// Views
builder.Services.AddSingleton<CreateReservationView>();
builder.Services.AddSingleton<EditReservationView>();
builder.Services.AddSingleton<DeleteReservationView>();
builder.Services.AddSingleton<ReservationView>();
builder.Services.AddSingleton<EmployeeLoginView>();
builder.Services.AddSingleton<GuestLoginView>();
builder.Services.AddSingleton<CreateGuestView>();
builder.Services.AddSingleton<CreateEmployeeView>();
builder.Services.AddSingleton<DebugView>();
builder.Services.AddSingleton<MainMenuView>();

using IHost host = builder.Build();

host.Services.GetService<MainMenuView>()!.Output();
using ProjectB.login;
using ProjectB.Models;
using ProjectB.Repositories;
using ProjectB.Services;
using ProjectB.settings;
using ProjectB.Views.Reservation;

var guest = new Guest("1234", DateOnly.FromDateTime(DateTime.Now));

var repo = new TourRepository();
var guestRepo = new GuestRepository();
guestRepo.Save(guest);
Settings.CurrentSession = new Session(guest.GetId(), guest.GetUserRole());
var guestService = new GuestService(guestRepo);
var service = new TourService(repo);
var createView = new CreateReservationView(service, guestService);
var view = new ReservationView(createView);

view.Output();
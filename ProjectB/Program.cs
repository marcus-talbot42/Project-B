using ProjectB.Repositories;
using ProjectB.Services;
using ProjectB.Views.Reservation;

var repo = new TourRepository();
var service = new TourService(repo);
var createView = new CreateReservationView(service);
var view = new ReservationView(createView);

view.Output();
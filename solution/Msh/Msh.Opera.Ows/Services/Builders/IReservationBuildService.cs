using System.Xml.Linq;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.ReservationRequestModels;

namespace Msh.Opera.Ows.Services.Builders;

public interface IReservationBuildService
{
	XElement MakeReservation(OwsReservationRequest reqData, OwsConfig config);

	XElement GetReservationStatus(OwsBaseSession reqData, string hotelCode, string reservationId, OwsConfig config);
	XElement FetchBooking(OwsBaseSession reqData, string reservationId, OwsConfig config);

	XElement UpdatePackages(OwsPackageRequest reqData, OwsConfig config);
	XElement AddBookingComments(OwsAddBookingCommentRequest reqData, OwsConfig config);
	XElement AddPayment(OwsAddPaymentRequest reqData, OwsConfig config);
}
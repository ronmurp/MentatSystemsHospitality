using Msh.Common.Logger;
using Msh.Opera.Ows.Models;
using Msh.Opera.Ows.Models.ReservationRequestModels;
using Msh.Opera.Ows.Models.ReservationResponseModels;

namespace Msh.Opera.Ows.Services;

/// <summary>
/// Calls OWS cloud services for reservations
/// </summary>
public interface IOperaReservationService
{
	(OwsReservation owsReservation, OwsResult owsResult) CreateBooking(OwsReservationRequest reqData, IXmlRedactor redactor);
	Task<(OwsReservation owsReservation, OwsResult owsResult)> CreateBookingAsync(OwsReservationRequest reqData, IXmlRedactor redactor);


	(OwsReservation owsReservation, OwsResult owsResult) ModifyBooking(OwsReservationRequest reqData, IXmlRedactor redactor);
	Task<(OwsReservation owsReservation, OwsResult owsResult)> ModifyBookingAsync(OwsReservationRequest reqData, IXmlRedactor redactor);


	(OwsReservation owsReservation, OwsResult owsResult) FetchBooking(OwsBaseSession reqData, string reservationId);
	(OwsPackageExtraRes owsPackageExtraRes, OwsResult owsResult) UpdatePackage(OwsPackageRequest reqData);

	(CommentList list, OwsResult owsResult) AddBookingComments(OwsAddBookingCommentRequest reqData);
	Task<(CommentList list, OwsResult owsResult)> AddBookingCommentsAsync(OwsAddBookingCommentRequest reqData);

	(string resvId, OwsResult owsResult) AddBookingPayment(OwsAddPaymentRequest reqData);
	Task<(string resvId, OwsResult owsResult)> AddBookingPaymentAsync(OwsAddPaymentRequest reqData);

	string LastRequest { get; }
       
}
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
	Task<(OwsReservation owsReservation, OwsResult owsResult)> CreateBookingAsync(OwsReservationRequest reqData, IXmlRedactor redactor);

	Task<(OwsReservation owsReservation, OwsResult owsResult)> ModifyBookingAsync(OwsReservationRequest reqData, IXmlRedactor redactor);

	Task<(OwsReservation owsReservation, OwsResult owsResult)> FetchBookingAsync(OwsBaseSession reqData, string reservationId);

	Task<(OwsPackageExtraRes owsPackageExtraRes, OwsResult owsResult)> UpdatePackageAsync(OwsPackageRequest reqData);

	Task<(CommentList list, OwsResult owsResult)> AddBookingCommentsAsync(OwsAddBookingCommentRequest reqData);

	Task<(string resvId, OwsResult owsResult)> AddBookingPaymentAsync(OwsAddPaymentRequest reqData);

	Task<(OwsReservation owsReservation, OwsResult owsResult)> GetReservationStatusAsync(OwsBaseSession reqData, string hotelCode, string reservationId, IXmlRedactor redactor, OwsConfig config);

	string LastRequest { get; }
       
}
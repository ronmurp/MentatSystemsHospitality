namespace Msh.Opera.Ows.Models.ReservationRequestModels;

/// <summary>
/// Mimics Micros reservation service auto generated action. Keep case for direct injection.
/// </summary>
public enum OwsReservationActionType
{
	ADD,

	EDIT, // Modify booking

	CANCEL,

	CHECKIN,

	CHECKOUT,

	NOSHOW,

	REINSTATE,
}
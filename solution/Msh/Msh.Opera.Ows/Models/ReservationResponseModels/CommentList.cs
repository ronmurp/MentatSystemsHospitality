namespace Msh.Opera.Ows.Models.ReservationResponseModels;

public class CommentList
{
	public void Add(OwsComment comment) => Comments.Add(comment);
	public void Add(string comment) => Comments.Add(new OwsComment { Text = comment});

	public void Add(string comment, bool guestViewable) => Comments.Add(new OwsComment { Text = comment, GuestViewable = guestViewable });

	public List<OwsComment> Comments { get; set; } = new List<OwsComment>();


}
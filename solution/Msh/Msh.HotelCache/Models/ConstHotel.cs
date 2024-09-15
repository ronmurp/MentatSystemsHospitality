namespace Msh.HotelCache.Models;

public static class ConstHotel
{
    public static class Cache
    {
	    public const string TestModel = "TestModel";

		public const string Hotel = "Hotel";
        public const string RoomTypes = "RoomTypes";
        public const string RatePlans = "RatePlans";
        public const string RoomTypeFilters = "RoomTypeFilters";
        public const string Extras = "Extras";
        public const string Specials = "Specials";

		public const string DiscountConfig = "DiscountConfig";
        public const string DiscountGroups = "DiscountGroups";
        public const string DiscountCodes = "DiscountCodes";
    }

    /// <summary>
    /// Validation error messages
    /// </summary>
    public static class Vem
    {
		// ModelState.AddModelError
		public const string GeneralSummary = "Please resolve the issues below";

		// General ...
		public const string CodeRequired = "A Code value is required to uniquely identify an item.";

		public const string CodeLength35 = "A Code should be length 3, or at most 5 characters.";
		public const string NameLength350 = "A Name should be between 3 and 50 characters.";

		// Passwords
		public const string PasswordLength = "A Password should be at least 12 characters";
        public const int PasswordMin = 12;
        public const int PasswordMax = 100;
	}
}
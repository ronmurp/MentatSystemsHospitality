namespace Msh.HotelCache.Models.Companies;

public enum CompanyAuthMethod
{
	Email,      // ProfileId and Email must match
	Password    // Password auth via Opera - needs Email and Password and return verifies ProfileId
}
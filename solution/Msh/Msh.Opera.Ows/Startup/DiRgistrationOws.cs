﻿using Microsoft.Extensions.DependencyInjection;
using Msh.Opera.Ows.Cache;
using Msh.Opera.Ows.Services;
using Msh.Opera.Ows.Services.Builders;
using Msh.Opera.Ows.Services.CustomTest;

namespace Msh.Opera.Ows.Startup;

public static class DiRegistrationCom
{
	public static void RegisterOwsServices(this IServiceCollection services)
	{

		services.AddScoped<IOwsRepoService, OwsRepoService>();
		services.AddScoped<IOwsCacheService, OwsCacheService>();
		services.AddScoped<CustomAvailabilityService, CustomAvailabilityService>();

		services.AddScoped<IOwsPostService, OwsPostService>(); // singleton

		services.AddScoped<ISoapEnvelopeService, SoapEnvelopeService>();
		services.AddScoped<IAvailabilityBuildService, AvailabilityBuildService>();
		services.AddScoped<IOperaAvailabilityService, OperaAvailabilityService>();
		services.AddScoped<IReservationBuildService, ReservationBuildService>();
		services.AddScoped<IOperaReservationService, OperaReservationService>();
		services.AddScoped<IInformationBuildService, InformationBuildService>();
		services.AddScoped<IOperaInformationService, OperaInformationService>();
		services.AddScoped<ISecurityBuildService, SecurityBuildService>();
		services.AddScoped<IOperaSecurityService, OperaSecurityService>();
		services.AddScoped<IOperaNameService, OperaNameService>();
		services.AddScoped<INameBuildService, NameBuildService>();
	}
}
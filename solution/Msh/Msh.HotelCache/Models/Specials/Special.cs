﻿using System.ComponentModel;
using Msh.Common.Attributes;
using Msh.Common.Models.Dates;
using System.ComponentModel.DataAnnotations;

namespace Msh.HotelCache.Models.Specials;

public class Special
{
	[Required]
	[Display(Name = "Special Code"), Info("code")]
	public string Code { get; set; } = string.Empty;

	[Display(Name = "Text"), Info("text")]
	public string? Text { get; set; } = string.Empty;

	[Display(Name = "Enabled"), Info("enabled")]
	public bool Enabled { get; set; }

	

	[Display(Name = "Item Type"), Info("item-type")]
	public SpecialItemType ItemType { get; set; } = SpecialItemType.CheckBox;


	[Display(Name = "Selected Text"), Info("selected-text")]
	public string? SelectedText { get; set; } = string.Empty;

	[Display(Name = "Short Text"), Info("short-text")]
	public string? ShortText { get; set; } = string.Empty;


	[Display(Name = "Is Valued"), Info("is-valued")]
	public bool IsValued { get; set; } = false;


	[Display(Name = "Single Line"), Info("single-line")]
	public bool SingleLine { get; set; } = false;


	[Display(Name = "Select Option"), Info("select-option")]
	public List<SelectOption> Options { get; set; } = [];


	[Display(Name = "Item Dates"), Info("item-dates")]
	public List<ItemDate> ItemDates { get; set; } = [];



	[Display(Name = "Warning Text"), Info("warning-text")]
	public string? WarningText { get; set; } = string.Empty;


	[Display(Name = "Room Type Codes"), Info("room-type-codes")]
	public List<string> RoomTypeCodes { get; set; } = [];


	[Display(Name = "Rate Plan Codes"), Info("rate-plan-codes")]
	public List<string> RatePlanCodes { get; set; } = [];


	[Display(Name = "Adults Only"), Info("adults-only")]
	public bool AdultsOnly { get; set; }


	[Display(Name = "Disabled Text"), Info("disabled-text")]
	public string DisabledText { get; set; } = string.Empty;


	[Display(Name = "Notes"), Info("notes")]
	[DataType(DataType.MultilineText)]
	[Category("TextArea")]
	public string? Notes { get; set; } = string.Empty;

}
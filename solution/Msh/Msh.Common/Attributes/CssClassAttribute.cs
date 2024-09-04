﻿using System.Diagnostics.CodeAnalysis;

namespace Msh.Common.Attributes;

/// <summary>
/// Specifies a description for a property or event.
/// </summary>
[AttributeUsage(AttributeTargets.All)]
public class CssClassAttribute : Attribute
{
	/// <summary>
	/// Specifies the default value for the <see cref='System.ComponentModel.DescriptionAttribute'/>,
	/// which is an empty string (""). This <see langword='static'/> field is read-only.
	/// </summary>
	public static readonly CssClassAttribute Default = new CssClassAttribute();

	public CssClassAttribute() : this(string.Empty)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref='System.ComponentModel.DescriptionAttribute'/> class.
	/// </summary>
	public CssClassAttribute(string description)
	{
		DescriptionValue = description;
	}

	/// <summary>
	/// Gets the description stored in this attribute.
	/// </summary>
	public virtual string CssClass => DescriptionValue;

	/// <summary>
	/// Read/Write property that directly modifies the string stored in the description
	/// attribute. The default implementation of the <see cref="CssClass"/> property
	/// simply returns this value.
	/// </summary>
	protected string DescriptionValue { get; set; }

	public override bool Equals([NotNullWhen(true)] object? obj) =>
		obj is CssClassAttribute other && other.CssClass == CssClass;

	public override int GetHashCode() => CssClass?.GetHashCode() ?? 0;

	public override bool IsDefaultAttribute() => Equals(Default);
}
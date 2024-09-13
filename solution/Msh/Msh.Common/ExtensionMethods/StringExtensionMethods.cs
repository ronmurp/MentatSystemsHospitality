using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using PhoneNumbers;
using System.Text.Json;

namespace Msh.Common.ExtensionMethods
{
	public static class StringExtensionMethods
	{
		public static string EncodeSquareBrackets(this string input)
		{
			var s = input.Replace("[", "%5B");
			var sout = s.Replace("]", "%5D");
			return sout;
		}
		public static string MultipleReplace(this string text, Dictionary<string, string> replacements)
		{
			var pattern = "(" + string.Join("|", replacements.Keys.ToArray()) + ")";

			return Regex.Replace(text, pattern, m => replacements[m.Value]);
		}
		public static bool ValidateEmail(this string email, string emailAddressRegEx) => Regex.Match(email, emailAddressRegEx).Success;

		public static bool ValidateRegEx(this string value, string regexPattern) => Regex.Match(value, regexPattern).Success;

		public static bool EqualsAnyCase(this string text, string value) =>
			text.Equals(value, StringComparison.InvariantCultureIgnoreCase);

		public static bool EqualsAnyCaseNotEmpty(this string text, string value) =>
			!string.IsNullOrEmpty(text) && text.Equals(value, StringComparison.InvariantCultureIgnoreCase);

		public static bool Empty(this string text) =>
			string.IsNullOrWhiteSpace(text);

		public static string[] SplitCharTrim(this string source, string splitOnString) =>
			source.Split(splitOnString.ToCharArray())
				.Select(line => line.Trim())
				.Where(line => !string.IsNullOrEmpty(line))
				.ToArray();

		public static string[] SplitComma(this string source) =>
			source.Split(",".ToCharArray());

		public static string[] SplitCommaTrim(this string source) =>
			source.Split(",".ToCharArray())
				.Select(line => line.Trim())
				.Where(line => !string.IsNullOrEmpty(line))
				.ToArray();

		public static string[] SplitCommaTrimUpper(this string source) =>
			source.Split(",".ToCharArray())
				.Select(line => line.Trim().ToUpper())
				.Where(line => !string.IsNullOrEmpty(line))
				.ToArray();

		public static string[] SplitCommaTrimLower(this string source) =>
			source.Split(",".ToCharArray())
				.Select(line => line.Trim().ToUpper())
				.Where(line => !string.IsNullOrEmpty(line))
				.ToArray();

		public static List<KeyValuePair<string, int>> SplitSemiColonColon(this string line)
		{
			var list = new List<KeyValuePair<string, int>>();
			var a = line.SplitCharTrim(";");
			foreach (var item in a)
			{
				var b = item.SplitCharTrim(":");
				list.Add(new KeyValuePair<string, int>(b[0], int.Parse(b[1])));
			}

			return list;
		}

		public static List<int> SplitIntList(this string input, string delimiter)
		{
			var list = new List<int>();
			var a = input.SplitCharTrim(delimiter);
			foreach (var s in a)
			{
				if (int.TryParse(s, out int value))
				{
					list.Add(value);
				}
			}

			return list;
		}

		public static List<List<int>> ListOfListOfInt(this string input)
		{
			var result = new List<List<int>>();
			var a = SplitCharTrim(input, ";");
			foreach (var s in a)
			{
				var list = s.SplitIntList(",");
				if (list.Count > 0)
					result.Add(list);
			}

			return result;
		}

		/// <summary>
		/// Splits test into lines (e.g. reading all text when ReadAllLines isn't possible because it's already read)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static IEnumerable<string> SplitToLines(this string input)
		{
			if (input == null)
			{
				yield break;
			}

			using (System.IO.StringReader reader = new System.IO.StringReader(input))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}


		/// <summary>
		/// Return a random value from an array, with defaults for 0 or 1 values
		/// </summary>
		/// <param name="values"></param>
		/// <param name="rand"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		public static string RandText(this string[] values, Random rand, string defaultValue = "") =>
			values.Length == 0
				? defaultValue // Default for no values
				: values.Length == 1
					? values[0] // Default for one value
					: values[rand.Next(0, values.Length)]; // Intended action for multiple values

		// Todo - System.Linq.Dynamic.DynamicExpression - Wait for .NET6
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="object"></param>
		/// <returns></returns>
		private static string ReplaceMacro(this string value, object @object)
		{
			throw new NotImplementedException("ReplaceMacro needs .NET 6? see https://dotnetfiddle.net/MoqJFk and https://stackoverflow.com/questions/39874172/dynamic-string-interpolation");
			//return Regex.Replace(value, @"\$(.+?)\$",
			//    match => {
			//        var p = Expression.Parameter(@object.GetType(), @object.GetType().Name);
			//        //Console.WriteLine("{0} {1}",job.GetType(), job.GetType().Name);
			//        var e = System.Linq.Dynamic.DynamicExpression.ParseLambda(new[] { p }, null, match.Groups[1].Value);
			//        return (e.Compile().DynamicInvoke(@object) ?? "").ToString();
			//    });
		}



		/// <summary>
		/// Regex to match keywords of the format {variable}
		/// </summary>
		private static readonly Regex TextTemplateRegEx = new Regex(@"{(?<prop>\w+)}", RegexOptions.Compiled);

		/// <summary>
		/// Replaces all the items in the template string with format "{variable}" using the value from the data
		/// </summary>
		/// <param name="templateString">string template</param>
		/// <param name="model">The data to fill into the template</param>
		/// <returns></returns>
		public static string FormatTemplate(this string templateString, object model)
		{
			if (model == null)
			{
				return templateString;
			}

			PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

			if (!properties.Any())
			{
				return templateString;
			}

			return TextTemplateRegEx.Replace(
				templateString,
				match =>
				{
					PropertyInfo property = properties.FirstOrDefault(propertyInfo =>
						propertyInfo.Name.Equals(match.Groups["prop"].Value, StringComparison.OrdinalIgnoreCase));

					if (property == null)
					{
						return string.Empty;
					}

					object value = property.GetValue(model, null);

					return value == null ? string.Empty : value.ToString();
				});
		}

		/// <summary>
		/// Termminate any non-terminated string
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string AddPeriod(this string text) =>
			!string.IsNullOrEmpty(text)
				? (text.EndsWith(".") || text.EndsWith(",") || text.EndsWith(";") || text.EndsWith("?") ||
				   text.EndsWith("!")
					? text
					: $"{text}.")
				: text;

		/// <summary>
		/// Useful for creating a title from a property name
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string SplitUpperCase(this string value) =>
			string.Join(" ", Regex.Split(value, @"(?<!^)(?=[A-Z](?![A-Z]|$))"));

		public static string ToCamelCase(this string str) =>
			string.IsNullOrEmpty(str) || str.Length < 2
				? str.ToLowerInvariant()
				: char.ToLowerInvariant(str[0]) + str.Substring(1);

		/// <summary>
		/// Return ProperCase/TitleCase of a string
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static string ProperCase(string s)
		{
			//return Microsoft.VisualBasic.Strings.StrConv(s, VbStrConv.ProperCase);
			string rText = s;
			try
			{
				System.Globalization.CultureInfo cultureInfo =
					System.Threading.Thread.CurrentThread.CurrentCulture;
				System.Globalization.TextInfo TextInfo = cultureInfo.TextInfo;
				rText = TextInfo.ToTitleCase(s);
			}
			catch (Exception ex)
			{
				//WbsLogger.Error(LogCodes.StringProperCase, ex, "ProperCase of " + s);
			}
			return rText;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="version"></param>
		/// <returns></returns>
		public static (int major, int minor) SplitVersion(this string version)
		{
			var major = 0;
			var minor = 0;

			var a = version.Split(".".ToCharArray());

			major = a.Length > 0 && int.TryParse(a[0], out major)
				? major
				: 0;

			minor = a.Length > 1 && int.TryParse(a[1], out minor)
				? minor
				: 0;

			return (major, minor);
		}

		public static string ReplaceInvalidChars(this string filename, string replaceChar)
		{
			return string.Join(replaceChar, filename.Split(Path.GetInvalidFileNameChars()));
		}

		public static int GetStableHashCode(this string str)
		{
			unchecked
			{
				int hash1 = 5381;
				int hash2 = hash1;

				for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
				{
					hash1 = ((hash1 << 5) + hash1) ^ str[i];
					if (i == str.Length - 1 || str[i + 1] == '\0')
						break;
					hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
				}

				return hash1 + (hash2 * 1566083941);
			}
		}

		public static string RemoveQuotes(this string s) => s.Trim().Trim('"').Trim();

		public static string RemoveCommaQuotes(this string s) => s.Trim().Trim(',').Trim('"').Trim();

		//public static string DestructPcModelState(this string json)
		//{
		//	var obj = JsonConvert.DeserializeObject(json);

		//	var js = JsonConvert.SerializeObject(obj, Formatting.Indented);
		//	string[] seperatingTags = { Environment.NewLine.ToString() };
		//	var lines = js.Split(seperatingTags, StringSplitOptions.None);
		//	var list = new List<string>();
		//	var found = false;
		//	for (var i = 0; i < lines.Length; i++)
		//	{
		//		var line = lines[i].Trim();
		//		if (line.StartsWith("\"Message\":"))
		//		{
		//			list.Add(line.Replace("\"Message\":", "").RemoveCommaQuotes());
		//			continue;
		//		}
		//		if (line.StartsWith("\"ModelState\""))
		//		{
		//			found = true;
		//			continue;
		//		}

		//		if (found && line.Contains("model."))
		//		{
		//			list.Add(lines[i + 1].RemoveCommaQuotes());
		//		}
		//	}

		//	return string.Join(" ", list.Distinct());

		//}

		//public static string DestructPcErrors(this string json)
		//{
			
		//	var obj = JsonSerializer.Deserialize(json);

		//	var js = JsonConvert.SerializeObject(obj, Formatting.Indented);
		//	string[] seperatingTags = { Environment.NewLine.ToString() };
		//	var lines = js.Split(seperatingTags, StringSplitOptions.None);
		//	var list = new List<string>();
		//	var found = false;
		//	for (var i = 0; i < lines.Length; i++)
		//	{
		//		var line = lines[i].Trim();
		//		if (line.StartsWith("\"Clients\":"))
		//		{
		//			list.Add(line.Replace("\"Message\":", "").RemoveCommaQuotes());
		//			continue;
		//		}
		//	}

		//	return string.Join(" ", list.Distinct());

		//}

		/// <summary>
		/// Start with upper, then any upper, lower, digit.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static bool ValidWbsCode(this string text) =>
			(new Regex("^[A-Z][a-zA-Z0-9]*$")).IsMatch(text);

		public static string GetFullName(this string firstName, string lastName) => string.Join(" ", $"{firstName} {lastName}".Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

		public static string FirstChar(this object obj) => $"{obj}".Substring(0, 1);

		public static string TakeN(this string input, int n) => new string(input.Take(n).ToArray());


		/// <summary>
		/// Take an array of bools and convert to a string of binary digits representing their truth state.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string BoolToBinaryString(this bool[] input)
		{
			var sb = new StringBuilder();
			foreach (var b in input)
			{
				sb.Append(b ? "1" : "0");
			}

			return sb.ToString();
		}

		/// <summary>
		/// Take the last four digits of a credit card number
		/// </summary>
		/// <param name="number"></param>
		/// <returns></returns>
		public static string LastFourStarred(this string number)
		{
			if (number.Length > 4)
				return new string('*', number.Length - 4) + number.Substring(number.Length - 4);
			else
				return string.Empty;
		}

		public static List<string> SplitIntoChunks(this string str, int chunkSize)
		{
			List<string> chunks = new List<string>();
			if (chunkSize <= 0)
			{
				chunks.Add(str);
				return chunks;
			}



			for (int i = 0; i < str.Length; i += chunkSize)
			{
				int length = Math.Min(chunkSize, str.Length - i);
				chunks.Add(str.Substring(i, length));
			}

			return chunks;
		}

		public static (string phone, bool isValid) InternationalTelephone(this string inputPhoneNumber, string countryCode)
		{
			if (string.IsNullOrEmpty(inputPhoneNumber))
			{
				return (string.Empty, false);
			}

			try
			{
				var trimmedPhoneNumber = inputPhoneNumber.TrimStart(new Char[] { '0' });
				var phoneNumberUtil = PhoneNumberUtil.GetInstance();
				var phoneNumber = phoneNumberUtil.Parse(trimmedPhoneNumber, countryCode);
				var formattedPhoneNumber = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);
				var isValid = phoneNumberUtil.IsValidNumber(phoneNumber);

				return (isValid ? formattedPhoneNumber : inputPhoneNumber, isValid);
			}
			catch
			{
				return (inputPhoneNumber, false);
			}
		}

		/// <summary>
		/// Clean ":" from user Ip which may occur on localhost
		/// </summary>
		/// <param name="userIp"></param>
		/// <returns></returns>
		public static string CleanIp(this string userIp) => userIp.Replace(":", "_");
	}
}

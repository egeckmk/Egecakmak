using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Egecakmak.Utility;
using Egecakmak.Utility.Models;

namespace Egecakmak.Utility.Helper
{
	public static class Extensions
	{
		#region [ kontroller isXXX ]

		/// <summary>
		/// Returns the Boolean Value. 
		/// Default: False
		/// </summary>
		/// <param name="val">input Value. Return False if 'val' is NOT a Boolean</param>
		/// <returns></returns>
    public static bool IsTrue(this object val)
		{
			Boolean rv = false;
			try
			{
				rv = Convert.ToBoolean(val);
			}
			catch { }
			return rv;
		}

		/// <summary>
		/// Signed integer mı?
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    public static bool IsSigned(this object obj)
		{
			return (obj is ValueType && (obj is Int32 || obj is Int64 || obj is Int16 || obj is IntPtr || obj is decimal || obj is SByte));
		}

		/// <summary>
		/// Sayı mı
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    public static bool IsNumeric(this object obj)
		{
			return (obj is ValueType && (obj is Int32 || obj is Int64 || obj is Int16 || obj is decimal || obj is Byte || obj is SByte || obj is double));
		}

		/// <summary>
		/// Boş mu_ Tipine göre kontrol eder.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    public static bool IsEmpty(this object obj)
		{
			return (obj != null && (
      (obj is String && ((string)obj).Length == 0) ||
      (obj is StringBuilder && ((StringBuilder)obj).Length == 0) ||
			//(obj is ICollection && ((ICollection)obj).Count == 0) ||
      (obj is IList && ((IList)obj).Count == 0) ||
      (obj is Array && ((Array)obj).Length == 0) ||
      (IsSigned(obj) && obj == (ValueType)(-1)) ||
      (obj is ValueType && obj == (ValueType)(0)) ||
      (obj is DBNull && obj == DBNull.Value) ||
      (obj is Guid && ((Guid)obj) == Guid.Empty)
    ));
		}

		/// <summary>
		/// null veya boş mu
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    public static bool IsNullTypeOrEmpty(this object obj)
		{
			return (IsAssigned(obj) || IsEmpty(obj));
		}

		/// <summary>
		/// null ve DBNull kontrolü (sadece detay generic null kontrol için IsEmpty kullan)
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    public static bool IsAssigned(this object obj)
		{
			return (obj != null && !(obj is DBNull));
		}

		/// <summary>
		/// String Value atanmış mı ve boş mu kontrolü
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static bool IsAssigned(this string value)
		{
			return (!string.IsNullOrEmpty(value));
		}

		/// <summary>
		/// null veya DBNull kontrolü
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
    public static bool IsNotAssigned(this object obj)
		{
			return (obj == null || obj is DBNull);
		}

		/// <summary>
		/// String Value atanmamış mı ve boş mu kontrolü
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static bool IsNotAssigned(this string value)
		{
			return (string.IsNullOrEmpty(value));
		}

		/// <summary>
		/// regex kontrolü
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
    public static bool IsAlphaNumeric(this string input)
		{
			Regex regex = new Regex("[^a-zA-Z0-9]");
			return !regex.IsMatch(input);
		}

		/// <summary>
		/// regex kontrolü
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
    public static bool IsInteger(this string input)
		{
			Regex regex = new Regex("[^0-9-]");
			Regex regex2 = new Regex("^-[0-9]+$|^[0-9]+$");
			return !regex.IsMatch(input) && regex2.IsMatch(input);
		}

		/// <summary>
		/// regex kontrolü
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
    public static bool IsNaturalNumber(this string input)
		{
			Regex regex = new Regex("[^0-9]");
			Regex regex2 = new Regex("0*[0-9][0-9]*");
			return !regex.IsMatch(input) && regex2.IsMatch(input);
		}

		/// <summary>
		/// regex kontrolü
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
    public static bool IsAlpha(this string input)
		{
			Regex regex = new Regex("[^a-zA-Z]");
			return !regex.IsMatch(input);
		}

		#endregion

    #region [ Html Formatting ]
		/// <summary>
		/// Convert Carriage Return to BR Tag
		/// </summary>
		/// <param name="szWindowsText"></param>
		/// <returns></returns>
    public static string CrLf2HtmlBRTag(this string szWindowsText)
		{
			return (szWindowsText ?? "").Replace("\r\n", "<br />").Replace("\n", "<br />");
		}

		/// <summary>
		/// Convert BR Tag to Carriage Return
		/// </summary>
		/// <param name="szHtmlText"></param>
		/// <returns></returns>
    public static string HtmlBRTag2CrLf(this string szHtmlText)
		{
			return (szHtmlText ?? "").Replace("<br />", "\r\n").Replace("<br>", "\r\n");
		}

		#endregion

    #region [ Type Conversion ]

		/// <summary>
		/// Nullable Safe Convert (Sorunsuz çevrim için)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <param name="defaultValueIfNull">Eğer değer yoksa veya null ise dönecek değer</param>
		/// <param name="handleError">Hata varsa fırlatsın mı yoksa default değeri mi dönsün?</param>
		/// <returns></returns>
    public static T ChangeType<T>(this object value, T defaultValueIfNull = default(T), bool handleError = true)
		{
			try
			{
				var t = typeof(T);

				if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
				{
					if (value == null)
					{
						return default(T);
					}

					t = Nullable.GetUnderlyingType(t);;
				}

				return (T)Convert.ChangeType(value, t);
			}
			catch
			{
				if (!handleError)
					throw;
				else
					return defaultValueIfNull;
			}
		}

		#endregion

    #region [ enum extender ]
		/// <summary>
		/// MAMACI: same as EnumValue.GetEnumAttribute(). Returns the Attribute. If not defined, returns ToString()
		/// </summary>
		/// <param name="enumValue"></param>
		/// <returns></returns>
    public static string Description(this Enum enumValue)
		{
			try
			{
				var enumType = enumValue.GetType();
				var field = enumType.GetField(enumValue.ToString());
				var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
				return attributes.Length == 0 ? enumValue.ToString() : ((DescriptionAttribute)attributes[0]).Description;
			}
			catch { return enumValue.ToString(); }
		}

		/// <summary>
		/// MAMACI: same as EnumValue.Description(). Returns the Attribute. If not defined, returns ToString()
		/// </summary>
		/// <param name="enumValue"></param>
		/// <param name="ordinalPosition">Birden fazla Attribute varsa sırası (0 based)</param>
		/// <returns></returns>
    public static string GetEnumAttribute(this Enum enumValue, int ordinalPosition = 0)
		{
			try
			{
				var enumType = enumValue.GetType();
				var field = enumType.GetField(enumValue.ToString());
				var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
				return attributes.Length < (ordinalPosition + 1) ? enumValue.ToString() : ((DescriptionAttribute)attributes[ordinalPosition]).Description;
			}
			catch { return enumValue.ToString(); }
		}

		/// <summary>
		/// Verilen string değerden Enum'ı döner
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
    public static T GetEnumItemFromString<T>(this string value) where T : struct
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

		/// <summary>
		/// Enum Tanımını Döner (Description Attribute varsa yok ToString() döner)
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static string GetEnumDescription(this Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes =
          (DescriptionAttribute[])fi.GetCustomAttributes(
          typeof(DescriptionAttribute),
          false);

			if (attributes != null &&
          attributes.Length > 0)
			return attributes[0].Description;
			else
				return value.ToString();
		}

		/// <summary>
		/// returns enum attribute
		/// </summary>
		/// <typeparam name="TAttribute"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static TAttribute GetAttribute<TAttribute>(this Enum value)
				where TAttribute : Attribute
		{
			var type = value.GetType();
			var name = Enum.GetName(type, value);
			return type.GetField(name)
					.GetCustomAttributes(false)
					.OfType<TAttribute>()
					.SingleOrDefault();
		}

		/// <summary>
		/// Return list of enums as casted
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static IEnumerable<T> EnumGetEnumerable<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>();
		}

		#endregion

		#region [ String Extensions ]
		/// <summary>
		/// Proper Case'e cevirme
		/// </summary>
		/// <param name="stringInput"></param>
		/// <returns></returns>
		public static string ToProperCase(this string stringInput)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			bool fEmptyBefore = true;
			foreach (char ch in stringInput)
			{
				char chThis = ch;
				if (Char.IsWhiteSpace(chThis))
					fEmptyBefore = true;
				else
				{
					if (Char.IsLetter(chThis) && fEmptyBefore)
						chThis = Char.ToUpper(chThis);
					else
						chThis = Char.ToLower(chThis);
					fEmptyBefore = false;
				}

				sb.Append(chThis);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Sql Quoted String yapar... başına ve sonuna ' ekler
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static string QuotedStr(this string value)
		{
			return value.QuotedStr(true);
		}

		/// <summary>
		/// Sql Quoted String yapar
		/// </summary>
		/// <param name="value"></param>
		/// <param name="AddQuotation">başına ve sonuna ' eklesin mi</param>
		/// <returns></returns>
    public static string QuotedStr(this string value, bool AddQuotation)
		{
			return string.Format((AddQuotation ? "'{0}'" : "{0}"), value.ToString("").Replace("'", "''"));
		}

		/// <summary>
		/// string Contains fonksiyonu gelişmiş hali. Null kontrolü yapar
		/// </summary>
		/// <param name="source"></param>
		/// <param name="toCheck"></param>
		/// <param name="comp"></param>
		/// <returns></returns>
    public static bool ContainsEx(this string source, string toCheck, StringComparison comp)
		{
			if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
				return false;
			return source.IndexOf(toCheck, comp) >= 0;
		}
		/// <summary>
		/// List%lt;String> metodunun gelişmiş hali
		/// </summary>
		/// <param name="source"></param>
		/// <param name="toCheck"></param>
		/// <param name="comp">Default StringComparison.InvariantCultureIgnoreCase</param>
		/// <returns></returns>
		public static bool ContainsEx(this List<string> source, string toCheck, StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
		{
			if (string.IsNullOrEmpty(toCheck) || source == null || source.Count == 0)
				return false;
			return source.Any(s => s.IndexOf(toCheck, comp) >= 0);
		}

		/// <summary>
		/// Regex kullanarak replace yapar. Özellikle büyük harf duyarlılığı olmaması gerektiğinde işe yarar. 
		/// </summary>
		/// <param name="input">Arama yapılacak metin</param>
		/// <param name="searchString">aranacak kelime veya regular expression</param>
		/// <param name="replaceString">bulunanların yerine konacak metin</param>
		/// <param name="regexOptions">RegEx seçenekleri. Default 'IgnoreCase | MultiLine'. Birden fazla ise or '|' ile ayır. Örnek web veya email kontrolü yaparsan CultureInvariant ekle.</param>
		/// <returns></returns>
		public static string ReplaceEx(this string input, string searchString, string replaceString, RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline)
		{
			return Regex.Replace(input, searchString, replaceString, regexOptions);
		}

		/// <summary>
		/// Şablon içinde öneki (prefix) ve soneki (suffix) ile çevrili (wrap) olan alan adını (columname) veri (datarow) değeri ile değiştirerek geri döndürür.
		/// <br /><i>Örnek Kullanım:</i><br />
		/// 				DataTable dt = BLL.ExecSqlDataTable("DBConnStr", "select * from GM_ITEM(nolock)", null);
		/// 				foreach(DataRow dr in dt.Rows)
		/// 				 .... dr.ToStringEx("@@ITEMID@@ > @@ITEMCODE@@ > @@ITEMNAME@@ > @@PRICE@@ @@PRICE_CURR@@\n")...;
		/// </summary>
		/// <param name="datarow">Data Satırı</param>
		/// <param name="inputstring">Parametreleri içinde bulunduran string</param>
		/// <param name="prefix">Örnek @@ALANDI@@ aranacaksa prefix='@@' olacak</param>
		/// <param name="suffix">Örnek @@ALANDI@@ aranacaksa suffix='@@' olacak</param>
		/// <param name="dateTimeFormat">C# Tarih formatı (Default: dd.MM.yyyy)</param>
		/// <param name="decimalFormat">C# Decimal/Double formatı (Default: N2)</param>
		/// <returns></returns>
    public static string ToStringEx(this DataRow datarow, string inputstring, string prefix = "@@", string suffix = "@@", string dateTimeFormat = "ddd.MM.yyyy", string decimalFormat = "N2")
		{
			if (datarow == null) return "";
			if (string.IsNullOrEmpty(inputstring)) return "";
			if (prefix == null) prefix = "";
			if (suffix == null) suffix = "";
			DataColumnCollection allpi = datarow.Table.Columns;
			foreach (DataColumn pi in allpi)
			{
				try
				{
					if (pi.DataType == typeof(System.DateTime))
						inputstring = inputstring.ReplaceEx(prefix + pi.ColumnName + suffix, datarow[pi.ColumnName].IsAssigned() ? datarow[pi.ColumnName].ToDateTime().ToString(dateTimeFormat) : "");
					else if (pi.DataType == typeof(System.Double))
						inputstring = inputstring.ReplaceEx(prefix + pi.ColumnName + suffix, datarow[pi.ColumnName].IsAssigned() ? ((double)datarow[pi.ColumnName]).ToString(decimalFormat) : "");
					else if (pi.DataType == typeof(System.Decimal))
						inputstring = inputstring.ReplaceEx(prefix + pi.ColumnName + suffix, datarow[pi.ColumnName].IsAssigned() ? ((decimal)datarow[pi.ColumnName]).ToString(decimalFormat) : "");
					else
						inputstring = inputstring.ReplaceEx(prefix + pi.ColumnName + suffix, datarow[pi.ColumnName].ToString(""));
				}
				catch { }
			}

			return inputstring;
		}

		/// <summary>
		/// Şablon içinde öneki (prefix) ve soneki (suffix) ile çevrili (wrap) olan alan adını (columname) veri (datarow) değeri ile değiştirerek geri döndürür.
		/// <br /><i>Örnek Kullanım:</i><br />
		/// 				ds.ToStringEx("@@ITEMID@@ > @@ITEMCODE@@ > @@ITEMNAME@@ > @@PRICE@@ @@PRICE_CURR@@\n")
		/// </summary>
		/// <param name="entity">Data objesi</param>
		/// <param name="inputstring">Parametreleri içinde bulunduran string</param>
		/// <param name="prefix">Örnek @@ALANDI@@ aranacaksa prefix='@@' olacak</param>
		/// <param name="suffix">Örnek @@ALANDI@@ aranacaksa suffix='@@' olacak</param>
		/// <param name="dateTimeFormat">C# Tarih formatı (Default: dd.MM.yyyy)</param>
		/// <param name="decimalFormat">C# Decimal/Double formatı (Default: N2)</param>
		/// <returns></returns>
    public static string ToStringEx<T>(this T entity, string inputstring, string prefix = "@@", string suffix = "@@", string dateTimeFormat = "ddd.MM.yyyy", string decimalFormat = "N2") where T : new()
		{
			if (entity == null) return "";
			if (string.IsNullOrEmpty(inputstring)) return "";
			if (prefix == null) prefix = "";
			if (suffix == null) suffix = "";
			PropertyInfo[] allpi = entity.GetType().GetProperties();
			foreach (PropertyInfo pi in allpi)
			{
				try
				{
					if (pi.PropertyType.IsInterface == false)
					{
						object value = pi.GetValue(entity);
						if (pi.PropertyType == typeof(System.DateTime))
							inputstring = inputstring.ReplaceEx(prefix + pi.Name + suffix, value != null ? ((DateTime)value).ToString(dateTimeFormat) : "");
						else if (pi.PropertyType == typeof(System.Double))
							inputstring = inputstring.ReplaceEx(prefix + pi.Name + suffix, value != null ? ((double)value).ToString(decimalFormat) : "");
						else if (pi.PropertyType == typeof(System.Decimal))
							inputstring = inputstring.ReplaceEx(prefix + pi.Name + suffix, value != null ? ((decimal)value).ToString(decimalFormat) : "");
						else
							inputstring = inputstring.ReplaceEx(prefix + pi.Name + suffix, value.ToString(""));
					}
				}
				catch { }
			}

			return inputstring;
		}

		/// <summary>
		/// String uzunluğu verilen aşarsa substring alır. Null kontrolü vb vardır. Null ise Null döner.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxlength"></param>
		/// <returns></returns>
    public static string ToMaxLenString(this string value, int maxlength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length > maxlength && maxlength > 0 ? value.Substring(0, maxlength) : value;
		}

		#endregion

    #region [ .ToXXX Extensions ]
		/// <summary>
		/// Return Guid of String. If Error, return new Guid()
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Guid of string. If Error, return new Guid()</returns>
    public static Guid ToGuid(this string value)
		{
			try
			{
				return new Guid(value);
			}
			catch
			{
				return new Guid();
			}
		}

		/// <summary>
		/// Metin içerisinden sadece rakam olanları döndürür... 
		/// Örn: (506) 837-1010 ise 5068371010 döner. 
		/// Boşsa default döner.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static string ToDigitsOnly(this string value, string defVal = "0")
		{
			if (string.IsNullOrEmpty(value)) return defVal;
			string tmp = new string(value.Where(c => char.IsDigit(c)).ToArray());
			if (string.IsNullOrEmpty(tmp)) return defVal;
			return tmp;
		}

		/// <summary>
		/// Herhangi bir nesneyi String'e çevirir. null ise default değeri atar.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer, eğer gönderilen null ise</param>
		/// <returns></returns>
    public static string ToString<T>(this T value, T defVal)
		{
			return value?.ToString() ?? defVal.ToString();
		}

		/// <summary>
		/// default 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static decimal ToDecimal(this object value) { return value.ToDecimal(0); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static decimal ToDecimal(this object value, decimal defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			decimal tmp = defVal;
			decimal.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// default 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static double ToDouble(this object value) { return value.ToDouble(0); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static double ToDouble(this object value, double defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			double tmp = defVal;
			double.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// default 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static long ToLong(this object value) { return value.ToLong(0); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static long ToLong(this object value, long defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			long tmp = defVal;
			long.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// default 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static int ToInt(this object value) { return value.ToInt(0); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static int ToInt(this object value, int defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			int tmp = defVal;
			int.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// default 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static Int16 ToInt16(this object value) { return value.ToInt16(0); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static Int16 ToInt16(this object value, Int16 defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			Int16 tmp = defVal;
			Int16.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// default 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static Int64 ToInt64(this object value) { return value.ToInt64(0); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static Int64 ToInt64(this object value, Int64 defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			Int64 tmp = defVal;
			Int64.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// default 0
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static Byte ToByte(this object value) { return value.ToByte(0); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static Byte ToByte(this object value, Byte defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			Byte tmp = defVal;
			Byte.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// default false
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static bool ToBool(this object value) { return value.ToBool(false); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static bool ToBool(this object value, bool defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			bool tmp = defVal;
			tmp = Convert.ToBoolean(value);
			return tmp;
		}

		/// <summary>
		/// default is NOW
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
    public static DateTime ToDateTime(this object value) { return value.ToDateTime(DateTime.Now); }
		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="value"></param>
		/// <param name="defVal">Standart değer</param>
		/// <returns></returns>
    public static DateTime ToDateTime(this object value, DateTime defVal)
		{
			if (value.IsNotAssigned()) return defVal;
			DateTime tmp = defVal;
			DateTime.TryParse(value.ToString(), out tmp);
			return tmp;
		}

		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
    public static byte[] ToByteArray(this string str)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			return aSCIIEncoding.GetBytes(str);
		}

		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="str"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
    public static byte[] ToByteArray(this string str, Encoding encoding)
		{
			return encoding.GetBytes(str);
		}

    /// <summary>
    /// Geliştirilmiş To<em>XXX</em> değeri
    /// </summary>
    /// <param name="byteArray"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string ToStr(this byte[] byteArray, Encoding encoding)
    {
      return encoding.GetString(byteArray);
    }

    /// <summary>
    /// ByteArray To String
    /// </summary>
    /// <param name="byteArray"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string ToString(this byte[] byteArray, Encoding encoding)
    {
      return encoding.GetString(byteArray);
    }


    /// <summary>
    /// convert byte to hex string
    /// </summary>
    /// <param name="buff"></param>
    /// <returns></returns>
    public static string ToHexString(this byte[] buff)
    {
      string sbinary = "";
      for (int i = 0; i < buff.Length; i++)
        sbinary += buff[i].ToString("x2"); /* hex format */
      return sbinary;
    }


		/// <summary>
		/// DotNoetCore da olmayan extension
		/// </summary>
		/// <param name="value"></param>
		/// <param name="dateFormat">Date Format. Default: dd.MM.yyyy</param>
		/// <returns></returns>
		public static string ToShortDateString(this DateTime value, string dateFormat = "dd.MM.yyyy")
		{
			return value.ToString(dateFormat);
		}

		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="data"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		public static MemoryStream ToMemoryStream(this string data, Encoding encoding)
		{
			MemoryStream result;
			if (encoding.IsAssigned())
			{
				result = new MemoryStream(data.ToByteArray(encoding));
			}
			else
			{
				result = new MemoryStream(data.ToByteArray());
			}

			return result;
		}

		/// <summary>
		/// Geliştirilmiş To<em>XXX</em> değeri
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
    public static MemoryStream ToMemoryStream(this string data)
		{
			return new MemoryStream(data.ToByteArray());
		}

		/// <summary>
		/// İçinde , veya ; olanları birbirinden ayırır.
		/// </summary>
		/// <param name="csvString"></param>
		/// <returns></returns>
    public static string[] ToArrayByCsv(this string csvString)
		{
			if (string.IsNullOrEmpty(csvString)) return new string[] { };
			return csvString.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
		}

		#endregion

    #region [ Sql Based Extensions ]

		/// <summary>
		/// return US formatted number with give number of precision. If NULL then assumes the number as 0
		/// </summary>
		/// <param name="value"></param>
		/// <param name="precision"></param>
		/// <returns></returns>
    public static string ToUSSqlNumber(this decimal? value, int precision = 4)
		{
			if (value == null) value = (decimal)0;
			return ToUSSqlNumber((decimal)value, precision);
		}

		/// <summary>
		/// return US formatted number with give number of precision. If NULL then assumes the number as 0
		/// </summary>
		/// <param name="value"></param>
		/// <param name="precision"></param>
		/// <returns></returns>
    public static string ToUSSqlNumber(this decimal value, int precision = 4)
		{
			return string.Format(new System.Globalization.CultureInfo("en-US").NumberFormat, "{0:#0.".PadRight(6 + precision, '0') + "}", value);
		}

		/// <summary>
		/// return US formatted number with give number of precision. If NULL then assumes the number as 0
		/// </summary>
		/// <param name="value"></param>
		/// <param name="precision"></param>
		/// <returns></returns>
    public static string ToUSSqlNumber(this double? value, int precision = 4)
		{
			if (value == null) value = (double)0;
			return ToUSSqlNumber((double)value, precision);
		}

		/// <summary>
		/// return US formatted number with give number of precision. If NULL then assumes the number as 0
		/// </summary>
		/// <param name="value"></param>
		/// <param name="precision"></param>
		/// <returns></returns>
    public static string ToUSSqlNumber(this double value, int precision = 4)
		{
			return string.Format(new System.Globalization.CultureInfo("en-US").NumberFormat, "{0:#0.".PadRight(6 + precision, '0') + "}", value);
		}

		/// <summary>
		/// returns the datetime as string in given format. If date is null returns NULL otherwise formatted string without quotes
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
    public static string ToMsSqlDate(this DateTime? value, string format = "yyyy-MM-dd HH:mm")
		{
			if (value == null) return "NULL";
			return ToMsSqlDate(value, format);
		}

		/// <summary>
		/// returns the datetime as string in given format. If date is null returns NULL otherwise formatted string without quotes
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		/// <returns></returns>
    public static string ToSqlDate(this DateTime value, string format = "yyyy-MM-dd HH:mm")
		{
			return string.Format(new System.Globalization.CultureInfo("en-US").DateTimeFormat, "{0:" + format + "}", value);
		}

		#endregion

    #region [ DateTime Extensions ]
		public static int WeekOfDate(this DateTime value)
		{
			if (value.IsNotAssigned())
				return 0;
			else
				return System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(value.ToDateTime(),
          System.Globalization.CalendarWeekRule.FirstFourDayWeek,
          DayOfWeek.Monday);
		}

		#endregion

    #region [ Data To Entity İşlemleri ]
		/// <summary>
		/// XmlSerializer marifetiyle DataTable'ı çevirir........
		///Step 1.  Extract the XML Schema. 
		///DataTable has two handy methods to extract Xml and Xml Schema. I extracted the Xml Schema to be able to generate a C# class using the xsd.exe.
		///		string path4Xsd = "Your File Path"; 
		///		myDataTable.WriteXml(path4Xsd); 
		///		myDataTable.WriteXmlSchema(path4Xsd);
		///Step 2. Generate C# Class using Xsd.exe that ships with the .NET Framework. 
		///		C:\temp>xsd mydatatable.xsd /l:cs /c 
		///		Microsoft (R) Xml Schemas/DataTypes support utility 
		///		[Microsoft (R) .NET Framework, Version 2.0.50727.42] 
		///		Copyright (C) Microsoft Corporation. All rights reserved. 
		///		Writing file 'C:\temp\mydatatableclass.cs'.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataTable"></param>
		/// <param name="path4Xsd">Nereye kaydedileceği örn: c:\mytable.xsd... daha sonra cs i xsd mydatatable.xsd /l:cs /c  ile oluşturabilirsin</param>
		/// <returns></returns>
    public static void SerializeDataTableToXML<T>(this DataTable dataTable, string path4Xsd, bool WriteSchema)
		{
			/*
			 * http://geekswithblogs.net/shahed/archive/2008/03/22/120709.aspx
		Step 1.  Extract the XML Schema. 
		DataTable has two handy methods to extract Xml and Xml Schema. I extracted the Xml Schema to be able to generate a C# class using the xsd.exe.

				string path4Xsd = "Your File Path"; 
				myDataTable.WriteXml(path4Xsd); 
				myDataTable.WriteXmlSchema(path4Xsd);


		Step 2. Generate C# Class using Xsd.exe that ships with the .NET Framework. 

				C:\temp>xsd mydatatable.xsd /l:cs /c 
				Microsoft (R) Xml Schemas/DataTypes support utility 
				[Microsoft (R) .NET Framework, Version 2.0.50727.42] 
				Copyright (C) Microsoft Corporation. All rights reserved. 
				Writing file 'C:\temp\mydatatableclass.cs'.
			 * */
      if (WriteSchema)
				dataTable.WriteXmlSchema(path4Xsd);
			else
				dataTable.WriteXml(path4Xsd);
			return;

			//try
			//{
			//	using (MemoryStream ms = new MemoryStream())
			//	{
			//		dataTable.WriteXml(ms);
			//		Type thetype = obj.GetType();
			//		XmlSerializer x = new XmlSerializer(thetype);
			//		ms.Position = 0;
			//		return (T)x.Deserialize(ms);
			//	}
			//}
			//catch
			//{
			//	return default(T);
			//}
		}

		/// <summary>
		/// Datatable'ı List olarak Entity'e çevirir. CreateItemFromRow kullanır
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="table"></param>
		/// <returns></returns>
    public static List<T> ToList<T>(this DataTable table) where T : new()
		{
			//IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
			List<T> result = new List<T>();

			foreach (var row in table.Rows)
			{
				var item = CreateItemFromRow<T>((DataRow)row);
				result.Add(item);
			}

			return result;
		}

		/// <summary>
		/// Eğer null değilse kaynak entity'de hedef entity ile aynı olan özellikleri kopyalar ve geriye hedefi döndürür. hedef null ise atamasını new ile yapar.
		/// </summary>
		/// <typeparam name="TSource">Kaynak Entity</typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="source"></param>
		/// <param name="target"></param>
    public static TTarget CopyToAnotherEntity<TSource, TTarget>(this TSource source, TTarget target) where TTarget : new()
		{
			string propertyname = ""; int debugline = 0;
			try
			{
				if (source == null) return target;
				debugline = 5;
				if (target == null) target = new TTarget();
				debugline = 10;
				var sourceProperties = typeof(TSource).GetProperties().Where(p => p.CanRead);

				foreach (var property in sourceProperties)
				{
					propertyname = property.Name;
					debugline = 20;
					var targetProperty = typeof(TTarget).GetProperty(property.Name);

					debugline = 30;
					if (targetProperty != null && targetProperty.CanWrite && targetProperty.PropertyType.IsAssignableFrom(property.PropertyType))
					{
						try
						{
							debugline = 40;
							var value = property.GetValue(source, null);

							debugline = 50;
							targetProperty.SetValue(target, value, null);
						}
						catch { }
					}
				}

				return target;
			}
			catch (Exception ex)
			{
				throw new Exception("CopyValues.debugline:" + debugline + ".propertyname:" + propertyname, ex);
			}
		}

		/// <summary>
		/// List entity'i başka bir List entity olarak geri verir.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TTarget"></typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
    public static List<TTarget> CopyToAnotherEntityList<TSource, TTarget>(this List<TSource> source) where TTarget : new()
		{
			int sirano = 1;
			try
			{
				List<TTarget> target = new List<TTarget>();
				foreach (TSource item in source)
				{
					//TTarget newtarget = new TTarget();
					//item.CopyToAnotherEntity(newtarget);
					//target.Add(newtarget);
					target.Add(item.CopyToAnotherEntity(new TTarget()));
					sirano++;
				}

				return target;
			}
			catch (Exception ex)
			{
				throw new MyException("(CopyToAnotherEntityList) Error in line {0}. Error Message: {1}", sirano, ex.AllMessages());
			}
		}

		/// <summary>
		/// Herhangi bir Entity'i diğer Entity'e yazar (Public alanları dikkate alır - BindingFlags.Public | BindingFlags.Instance)
		/// </summary>
		/// <param name="srcEntity"></param>
    public static U ToAnotherEntity<T, U>(this T srcEntity)
		where T : new()
		where U : new()
		{
			if (srcEntity is DataRow) throw new ArgumentException("Datarow için ToEntity kullan", "srcEntity");
			U destEntity = new U();
			if (srcEntity == null) return destEntity;
			PropertyInfo[] dbprops = srcEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			PropertyInfo[] scprops = destEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo dbpi in dbprops)
			{
				var hedef = scprops.FirstOrDefault(p => p.Name == dbpi.Name);
				if (hedef != null && hedef.CanWrite && dbpi.CanRead && hedef.PropertyType.IsAssignableFrom(dbpi.PropertyType))
				{
					hedef.SetValue(destEntity, dbpi.GetValue(srcEntity, null), null);
				}
			}

			return destEntity;
		}

		/// <summary>
		/// Herhangi bir Entity'i diğer Entity'e yazar
		/// </summary>
		/// <param name="srcEntity"></param>
    public static T ToAnotherEntity<T>(this object srcEntity)
		where T : new()
		{
			if (srcEntity is DataRow) throw new ArgumentException("Datarow için ToEntity kullan", "srcEntity");
			T destEntity = new T();
			PropertyInfo[] dbprops = srcEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			PropertyInfo[] scprops = destEntity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo dbpi in dbprops)
			{
				var hedef = scprops.FirstOrDefault(p => p.Name == dbpi.Name);
				if (hedef != null && hedef.CanWrite && dbpi.CanRead)
				{
					hedef.SetValue(destEntity, dbpi.GetValue(srcEntity, null), null);
				}
			}

			return destEntity;
		}

		/// <summary>
		/// DataTable'dan List<Entity> oluşturur (her bir satırı ToEntity ile çevirir)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataTable"></param>
		/// <returns></returns>
    public static List<T> DataTableToEntity<T>(this DataTable dataTable) where T : new()
		{
			try
			{
				List<T> lst = new List<T>();
				foreach (DataRow dr in dataTable.Rows)
				{
					T o = dr.ToEntity<T>();
					lst.Add(o);
				}

				return lst;
			}
			catch
			{
				return default(List<T>);
			}
		}

		/// <summary>
		/// CreateItemFromRow ile datarow entity'e çevirir
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <returns></returns>
    public static T ToEntity<T>(this DataRow row) where T : new()
		{
			var item = CreateItemFromRow<T>((DataRow)row);

			return item;
		}

		/// <summary>
		/// Datarow'dan Entity'e değer aktarır
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <returns></returns>
    public static T CreateItemFromRow<T>(DataRow row) where T : new()
		{
			IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
			T item = new T();
			foreach (var property in properties)
			{
				try
				{
					if (row.Table.Columns.IndexOf(property.Name) > -1)
					{
						if (row[property.Name] == DBNull.Value)
						{
							try
							{
								property.SetValue(item, null, null);
							}
							catch { }
						}
						else if (property.PropertyType.UnderlyingSystemType.FullName == "System.Xml.Linq.XElement")
							property.SetValue(item, System.Xml.Linq.XElement.Parse(row[property.Name].ToString()), null);
						else if (property.PropertyType.UnderlyingSystemType == typeof(System.Double).UnderlyingSystemType)
							property.SetValue(item, row[property.Name].ToDouble(), null);
						else if (property.PropertyType.UnderlyingSystemType == typeof(System.Decimal).UnderlyingSystemType)
							property.SetValue(item, row[property.Name].ToDecimal(), null);
						else if (property.PropertyType.UnderlyingSystemType == typeof(System.Char).UnderlyingSystemType)
							property.SetValue(item, row[property.Name].ToString().Length == 0 ? '\0' : row[property.Name].ToString()[0], null);
						else
							property.SetValue(item, row[property.Name], null);
					}
				}
				catch (Exception e)
				{
					try
					{
						property.SetValue(item, row[property.Name].ToString(), null);
					}
					catch
					{
						System.Diagnostics.Debug.WriteLine(e.Message, "CreateItemFromRow > Error in " + property.Name);
					}
				}
			}

			return item;
		}

		/// <summary>
		/// Data Row Field'ı istenen değere çevirir.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row">DataRow</param>
		/// <param name="fieldName"></param>
		/// <param name="valueIfNull"></param>
		/// <returns></returns>
    public static T GetValue<T>(this DataRow row, string fieldName, T valueIfNull)
		{
			if (row.Table.Columns.IndexOf(fieldName) == -1)
				return valueIfNull;
			else if (row[fieldName] == null || row[fieldName] == DBNull.Value)
				return valueIfNull;
			else
				return row[fieldName].ChangeType<T>();
		}

    #endregion

    #region [ Error Messages ]
    /// <summary>
    /// Get all main and inner messages...
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="seperator"></param>
    /// <returns></returns>
    public static string AllMessages(this Exception ex, string seperator = "\n")
    {
      try
      {
        if (ex == null) return "";
        StringBuilder sb = new StringBuilder();
        sb.Append(ex.Message + seperator);
        if (ex.InnerException != null)
        {
          sb.Append(ex.InnerException.AllMessages(seperator));
        }
        return sb.ToString();
      }
      catch
      {
        return ex.Message;
      }
    }

    #endregion
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace Egecakmak.Utility.Models
{
	/// <summary>
	/// Çift kayıt hataları için
	/// </summary>
	public class MyDuplicateRecordException : Exception
	{
		public MyDuplicateRecordException() { }
		public MyDuplicateRecordException(string Message, string FieldName, object FieldValue) : base(Message) { this.DuplicateField = FieldName; this.DuplicateValue = FieldValue; }

		public string DuplicateField { get; set; }
		public object DuplicateValue { get; set; }

		/// <summary>
		/// Özel formatlanmış mesaj
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format(this.Message + ". Fieldname: {0} - Value: {1}", this.DuplicateField, this.DuplicateValue);
		}
	}

	/// <summary>
	/// Gelişmiş hata mesajı. Eğer args varsa mesaj içindeki {...} leri formatlar.  Aynen string.Format gibi hazırlar.
	/// </summary>
	public class MyException : Exception
	{
		public MyException()
			: base() { }

		public MyException(string message)
			: base(message) { }

		public MyException(string format, params object[] args)
			: base(string.Format(format, args)) { }

		public MyException(string message, Exception innerException)
			: base(message, innerException) { }

		public MyException(string format, Exception innerException, params object[] args)
			: base(string.Format(format, args), innerException) { }
	}

	/// <summary>
	/// MyException ile aynı fakat ErroHandler'da ayırabilmek için farklı bir sınıf yaptım
	/// </summary>
	public class MyExceptionDoNotLog : MyException
	{
		public MyExceptionDoNotLog()
			: base() { }

		public MyExceptionDoNotLog(string message)
			: base(message) { }

		public MyExceptionDoNotLog(string format, params object[] args)
			: base(format, args) { }

		public MyExceptionDoNotLog(string message, Exception innerException)
			: base(message, innerException) { }

		public MyExceptionDoNotLog(string format, Exception innerException, params object[] args)
			: base(format, innerException, args) { }
	}

	/// <summary>
	/// MyException ile aynı fakat ErroHandler'da ayırabilmek için farklı bir sınıf yaptım
	/// </summary>
	public class MySQLException : MyException
	{
		public string ConnectionString { get; set; }
		public string SQLStatement { get; set; }

		public MySQLException()
			: base() { }

		public MySQLException(string message)
			: base(message) { }

		public MySQLException(string format, params object[] args)
			: base(format, args) { }

		public MySQLException(string message, Exception innerException)
			: base(message, innerException) { }

		public MySQLException(string format, Exception innerException, params object[] args)
			: base(format, innerException, args) { }
	}


	/// <summary>
	/// Extends the WebException to get content and details from Response if not null.
	/// Usage: 
	/// 			catch (WebException ew1) { throw new MyWebException(ew1); }
	/// </summary>
	[Serializable]
	public class MyWebException : System.Net.WebException
	{
		public MyWebException(System.Net.WebException exception) : base(exception.Message, exception.InnerException, exception.Status, exception.Response)
		{
			this.ContentLength = base.Response?.ContentLength ?? 0;
			this.ContentType = base.Response?.ContentType;

			try
			{
				using (StreamReader streamReader = new StreamReader(base.Response.GetResponseStream(), true))
				{
					this.Content = streamReader.ReadToEnd();
				}
			}
			catch
			{
			}

		}

		public long ContentLength { get; set; }
		public string ContentType { get; set; }
		public string Content { get; set; }
	}


}

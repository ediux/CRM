using System;
namespace My.Core.Infrastructures.Logs
{
	/// <summary>
	/// 定義一組提供系統操作紀錄的統一寫入介面。
	/// </summary>
	public interface ILogWriter : IDisposable
	{
		/// <summary>
		/// Writes the log.
		/// </summary>
		/// <returns>The log.</returns>
		/// <param name="path">Path.</param>
		/// <param name="Message">Message.</param>
		void WriteLog(string path, string Message);

		/// <summary>
		/// Gets or sets the type of the logger instance.
		/// </summary>
		/// <value>The type of the logger instance.</value>
		Type LoggerInstanceType { get; set; }

		/// <summary>
		/// Error the specified Message.
		/// </summary>
		/// <param name="Message">Message.</param>
		void Error(object Message);
		/// <summary>
		/// Error the specified Message and ex.
		/// </summary>
		/// <param name="Message">Message.</param>
		/// <param name="ex">Ex.</param>
		void Error(object Message, Exception ex);
		/// <summary>
		/// Errors the format.
		/// </summary>
		/// <returns>The format.</returns>
		/// <param name="Format">Format.</param>
		/// <param name="values">Values.</param>
		void ErrorFormat(string Format, params object[] values);

		/// <summary>
		/// Info the specified Message.
		/// </summary>
		/// <param name="Message">Message.</param>
		void Info(object Message);
		/// <summary>
		/// Info the specified Message and ex.
		/// </summary>
		/// <param name="Message">Message.</param>
		/// <param name="ex">Ex.</param>
		void Info(object Message, Exception ex);
		/// <summary>
		/// Infos the format.
		/// </summary>
		/// <returns>The format.</returns>
		/// <param name="Format">Format.</param>
		/// <param name="values">Values.</param>
		void InfoFormat(string Format, params object[] values);

		/// <summary>
		/// Debug the specified Message.
		/// </summary>
		/// <param name="Message">Message.</param>
		void Debug(object Message);
		/// <summary>
		/// Debug the specified Message and ex.
		/// </summary>
		/// <param name="Message">Message.</param>
		/// <param name="ex">Ex.</param>
		void Debug(object Message, Exception ex);
		/// <summary>
		/// Debugs the format.
		/// </summary>
		/// <returns>The format.</returns>
		/// <param name="Format">Format.</param>
		/// <param name="values">Values.</param>
		void DebugFormat(string Format, params object[] values);

		/// <summary>
		/// Warn the specified Message.
		/// </summary>
		/// <param name="Message">Message.</param>
		void Warn(object Message);
		/// <summary>
		/// Warn the specified Message and ex.
		/// </summary>
		/// <param name="Message">Message.</param>
		/// <param name="ex">Ex.</param>
		void Warn(object Message, Exception ex);
		/// <summary>
		/// Warns the format.
		/// </summary>
		/// <returns>The format.</returns>
		/// <param name="Format">Format.</param>
		/// <param name="values">Values.</param>
		void WarnFormat(string Format, params object[] values);

		/// <summary>
		/// Fatal the specified Message.
		/// </summary>
		/// <param name="Message">Message.</param>
		void Fatal(object Message);
		/// <summary>
		/// Fatal the specified Message and ex.
		/// </summary>
		/// <param name="Message">Message.</param>
		/// <param name="ex">Ex.</param>
		void Fatal(object Message, Exception ex);
		/// <summary>
		/// Fatals the format.
		/// </summary>
		/// <returns>The format.</returns>
		/// <param name="Format">Format.</param>
		/// <param name="values">Values.</param>
		void FatalFormat(string Format, params object[] values);
	}
}


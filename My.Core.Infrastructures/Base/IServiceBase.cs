using System;
using My.Core.Infrastructures.IoC;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.DAL
{
	/// <summary>
	/// 定義一組統一之邏輯服務層介面
	/// </summary>
	public interface IServiceBase
	{
		/// <summary>
		/// 取得或設定注入式相依性主機執行個體。
		/// </summary>
		/// <value>The host.</value>
		IDIHost Host { get; set; }
		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		ILogWriter Logger { get; set; }
	}
}


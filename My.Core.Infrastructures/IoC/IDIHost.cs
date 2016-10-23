using System;
using System.Collections.Generic;
using Autofac;

namespace My.Core.Infrastructures.IoC
{
	/// <summary>
	/// 定義注入式相依性處理主機物件的介面。
	/// </summary>
	public interface IDIHost
	{
		/// <summary>
		/// Gets the container.
		/// </summary>
		/// <value>The container.</value>
		IContainer Container { get; }
		/// <summary>
		/// Gets the host environment.
		/// </summary>
		/// <value>The host environment.</value>
		IDictionary<string, object> HostEnvironment { get; }
		/// <summary>
		/// Starts the host.
		/// </summary>
		/// <returns>The host.</returns>
		/// <param name="host">Host.</param>
		/// <param name="ExcludeFiles">Exclude files.</param>
		void StartHost(object host, params string[] ExcludeFiles);
	}
}


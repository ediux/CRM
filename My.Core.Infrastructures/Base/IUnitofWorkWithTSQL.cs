using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.DAL
{
    /// <summary>
    /// Unit of work with tsql.
    /// 使用TSQL的單元操作統一介面
    /// </summary>
    public interface IUnitofWorkWithTSQL<TDbContext> : IUnitofWork where TDbContext : class
    {
        /// <summary>
        /// Begins the transcation.
        /// </summary>
        /// <returns>The transcation.</returns>
        void BeginTranscation();
        /// <summary>
        /// Commits the transcation.
        /// </summary>
        /// <returns>The transcation.</returns>
        void CommitTranscation();
        /// <summary>
        /// Opens the database.
        /// </summary>
        /// <returns>The database.</returns>
        void OpenDatabase();
        /// <summary>
        /// Closes the database.
        /// </summary>
        /// <returns>The database.</returns>
        void CloseDatabase();

        /// <summary>
        /// 復原交易
        /// </summary>
        void RollbackTranscation();
        /// <summary>
        /// Executes the function.
        /// </summary>
        /// <returns>The function.</returns>
        /// <param name="FNName">FNN ame.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        TEntity ExecuteFunction<TEntity>(string FNName, object paramters, bool EnableTranscationScope = false) where TEntity : IDataModel;

        /// <summary>
        /// Executes the function async.
        /// </summary>
        /// <returns>The function async.</returns>
        /// <param name="FNName">FNN ame.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        Task<TEntity> ExecuteFunctionAsync<TEntity>(string FNName, object paramters, bool EnableTranscationScope = false) where TEntity : IDataModel;

        /// <summary>
        /// Executes the stored procedure.
        /// </summary>
        /// <returns>The stored procedure.</returns>
        /// <param name="SPName">SPN ame.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        void ExecuteStoredProcedure(string SPName, object paramters, bool EnableTranscationScope = false);

        /// <summary>
        /// Executes the stored procedure async.
        /// </summary>
        /// <returns>The stored procedure async.</returns>
        /// <param name="SPName">SPN ame.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        Task ExecuteStoredProcedureAsync(string SPName, object paramters, bool EnableTranscationScope = false);

        /// <summary>
        /// Executes the text command.
        /// </summary>
        /// <returns>The text command.</returns>
        /// <param name="TSQL">Tsql.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        IEnumerable<TEntity> ExecuteTextCommand<TEntity>(string TSQL, object paramters, bool EnableTranscationScope = false) where TEntity : IDataModel;

        /// <summary>
        /// Executes the text command async.
        /// </summary>
        /// <returns>The text command async.</returns>
        /// <param name="TSQL">Tsql.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        Task<IEnumerable<TEntity>> ExecuteTextCommandAsync<TEntity>(string TSQL, object paramters, bool EnableTranscationScope = false) where TEntity : IDataModel;

        /// <summary>
        /// Executes the table direct.
        /// </summary>
        /// <returns>The table direct.</returns>
        /// <param name="TSQL">Tsql.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        IEnumerable<TEntity> ExecuteTableDirect<TEntity>(string TSQL, object paramters, bool EnableTranscationScope = false) where TEntity : IDataModel;

        /// <summary>
        /// Executes the table direct async.
        /// </summary>
        /// <returns>The table direct async.</returns>
        /// <param name="TSQL">Tsql.</param>
        /// <param name="paramters">Paramters.</param>
        /// <param name="EnableTranscationScope">Enable transcation scope.</param>
        /// <typeparam name="TEntity">The 1st type parameter.</typeparam>
        Task<IEnumerable<TEntity>> ExecuteTableDirectAsync<TEntity>(string TSQL, object paramters, bool EnableTranscationScope = false) where TEntity : IDataModel;
    }
}


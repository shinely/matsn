using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    interface IDBHelper
    {
        /// <summary>
        /// 自定义执行sql操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        T Execute<T>(String sql, Func<DbCommand, T> func, params DbParameter[] parameters);

        /// <summary>
        /// 执行增删改的sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteNonQuery(String sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行返回只有一个结果的sql操作
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object ExecuteScalar(String sql, params DbParameter[] parameters);

        /// <summary>
        /// 执行查询操作，返回一个可读的DataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DbDataReader ExecuteReader(String sql, params DbParameter[] parameters);
    }
}

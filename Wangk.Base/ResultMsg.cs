using System;
using System.Collections.Generic;
using System.Text;

namespace Wangk.Base
{
    /// <summary>
    /// 请求结果
    /// </summary>
    public class ResultMsg<T>
    {

        /// <summary>
        /// 请求处理是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 请求处理失败时的消息提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回的实体数据
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 总记录数, 用于分页查询
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// 无参构造, 用于序列化
        /// </summary>
        public ResultMsg()
        {
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="value">返回的结果</param>
        public ResultMsg(T value)
        {
            Success = true;

            Data = value;
        }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="value">返回的结果</param>
        /// <param name="count">总记录数</param>
        public ResultMsg(T value, int count)
        {
            Success = true;

            Data = value;

            TotalCount = count;
        }

        /// <summary>
        /// 执行失败
        /// </summary>
        public ResultMsg(Exception e)
        {
            Success = false;

            Message = e.Message;
        }

        /// <summary>
        /// 自动包装返回结果
        /// </summary>
        public static implicit operator ResultMsg<T>(T value)
        {
            return new ResultMsg<T>(value);
        }

        /// <summary>
        /// 自动包装异常返回结果
        /// </summary>
        public static implicit operator ResultMsg<T>(Exception e)
        {
            return new ResultMsg<T>(e);
        }

    }
}

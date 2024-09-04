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
        /// 执行成功
        /// </summary>
        public static int SUCCESSCode = 200;

        /// <summary>
        /// 执行失败
        /// </summary>
        public static int FAILCode = 404;

        /// <summary>
        /// 请求处理状态
        /// 200: 请求处理成功
        /// 404: 请求处理失败
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 请求处理是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 请求处理消息
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
            Code = SUCCESSCode;
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
            Code = SUCCESSCode;
            Success = true;

            Data = value;

            TotalCount = count;
        }

        /// <summary>
        /// 执行失败
        /// </summary>
        public ResultMsg(Exception e)
        {
            Code = FAILCode;
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

    }
}

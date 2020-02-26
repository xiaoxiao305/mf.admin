using System;

namespace MF.Data
{
    [Serializable]
    public class Result<T>
    {
        public int Code { get; set; }
        public T R { get; set; }
        public string Message { get; set; }
        public Exception Ex { get; set; }
    }
    /// <summary>
    /// 主管理返回数据接收类
    /// </summary>
    [Serializable]
    public class TResult<T>
    {
        public string Key { get; set; }
        public T Res { get; set; }
    }
    /// <summary>
    /// 主管理返回数据带Error接收类
    /// </summary>
    [Serializable]
    public class TResult_Err<T>
    {
        public string Key { get; set; }
        public object[] Res { get; set; }
    }
}

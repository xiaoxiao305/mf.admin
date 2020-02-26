using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MF.Admin.DAL
{

    public class StringHelper
    {
        private static readonly Regex ZHCN_REG = new Regex(@"[\u4e00-\u9fa5]");

        public static string ZhCnToUnicode(string value)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(value))
            {
                for (int i = 0; i < value.Length; i++)
                {
                    if (ZHCN_REG.IsMatch(value[i].ToString()))
                    {
                        sb.Append("\\u");
                        sb.Append(((int)value[i]).ToString("x"));
                    }
                    else
                    {
                        sb.Append(value[i]);
                    }
                }
            }
            return sb.ToString();
        }
    }

    public enum TOpeart
    {
        /// <summary>
        /// 等于
        /// </summary>
        EQ = 1,
        /// <summary>
        /// 模糊查询
        /// </summary>
        LK = 2,
        /// <summary>
        /// 闭区间区域 between and
        /// </summary>
        BT = 3,
        /// <summary>
        /// 小于
        /// </summary>
        LT = 4,
        /// <summary>
        /// 大于
        /// </summary>
        GT = 5,
        /// <summary>
        /// 小于等于
        /// </summary>
        LTEQ = 6,
        /// <summary>
        /// 大于等于
        /// </summary>
        GTEQ = 7,
        /// <summary>
        /// 排序
        /// </summary>
        SORT = 8,
        /// <summary>
        /// in查询方式
        /// </summary>
        IN = 9,
            /// <summary>
            /// notin查询方式
            /// </summary>
        NOTIN = 10
    }

    public class SearchCondition<T> : SearchCondition where T : class
    {

        new public static SearchCondition<T> Current
        {
            get
            {
                return new SearchCondition<T>();
            }
        }

        private static string GetKey(Expression expression)
        {
            var lamdba = expression as LambdaExpression;
            if (lamdba == null)
            {
                throw new ArgumentException("error expression only can use LambdaExpression", "expression");
            }
            var member = lamdba.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException("error expression only can use memberexpression", "expression");
            }
            return member.Member.Name;
        }

        public SearchCondition Add<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            base.Add(GetKey(expression), value);
            return this;
        }
        public SearchCondition AddInt<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            base.AddInt(GetKey(expression), value);
            return this;
        }
        public SearchCondition AddNumber<TProperty>(Expression<Func<T, TProperty>> expression, TProperty value)
        {
            base.AddNumber(GetKey(expression), value);
            return this;
        }
        public SearchCondition Add<TProperty>(Expression<Func<T, TProperty>> expression, TOpeart opeart, TProperty value)
        {
            base.Add(GetKey(expression), opeart, value);
            return this;
        }

        public SearchCondition Add<TProperty>(Expression<Func<T, TProperty>> expression, params TProperty[] arry)
        {

            base.Add(GetKey(expression), arry);
            return this;
        }

        public SearchCondition AddBetween<TProperty>(Expression<Func<T, TProperty>> expression, TProperty between, TProperty and)
        {
            base.AddBetween(GetKey(expression), between, and);
            return this;
        }
        public SearchCondition AddArray<TProperty>(Expression<Func<T, TProperty>> expression, TProperty array)
        {
            base.AddArray(GetKey(expression), array);
            return this;
        }

        public SearchCondition AddSort<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            base.AddSort(GetKey(expression));
            return this;
        }

        public SearchCondition AddSort<TProperty>(Expression<Func<T, TProperty>> expression, bool asc)
        {
            base.AddSort(GetKey(expression), asc);
            return this;
        }

    }
    public class SearchCondition
    {
        #region class
        private sealed class _SearchObj
        {

            public _SearchObj(string key, object value) : this(key, null, value)
            {

            }

            public _SearchObj(string key, TOpeart? opt, object value)
            {
                this.Key = key;
                this.Opt = opt;
                this.Value = value;
            }

            public string Key { get; set; }

            public TOpeart? Opt { get; set; }

            public object Value { get; set; }


        }

        private class _SearchJsonConverter : JsonConverter
        {

            private int? _token;

            public _SearchJsonConverter()
            {

            }

            public _SearchJsonConverter(int token)
            {
                this._token = token;
            }

            public override bool CanConvert(Type objectType)
            {
                return true;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            private void WriteValue(JsonWriter writer, object obj)
            {
                if (obj == null)
                {
                    this.WriteNullValue(writer, obj);
                }
                else if (obj is DateTime)
                {
                    var date = Convert.ToDateTime(obj);
                    this.WriteValue(writer, date.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (obj is ValueType)
                {
                    this.WriteValueTypeValue(writer, obj);
                }
                else if (obj is object[])
                {
                    this.WriteArryValue(writer, obj);
                }
                else
                {
                    this.WriteObjValue(writer, obj);
                }
            }

            private void WriteValueTypeValue(JsonWriter writer, object obj)
            {
                writer.WriteRawValue(obj.ToString());
            }

            private void WriteObjValue(JsonWriter writer, object obj)
            {
                writer.WriteValue(obj.ToString());
            }

            private void WriteNullValue(JsonWriter writer, object obj)
            {
                writer.WriteRaw("Null");
            }

            private void WriteArryValue(JsonWriter writer, object obj)
            {
                var arry = obj as object[];
                writer.WriteStartArray();
                foreach (var item in arry)
                {
                    this.WriteValue(writer, item);
                }
                writer.WriteEndArray();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var obj = value as SearchCondition;
                writer.WriteStartArray();

                foreach (var o in obj)
                {
                    var item = o as _SearchObj;
                    writer.WriteStartArray();
                    writer.WriteValue(item.Key);
                    this.WriteValue(writer, item.Value);
                    if (item.Opt != null)
                    {
                        writer.WriteValue(item.Opt);
                    }
                    writer.WriteEndArray();
                }
                if (this._token == null)
                {
                    this.WriteValue(writer, obj.Sign);
                }
                writer.WriteEndArray();
            }
        }
        #endregion

        private bool _readyAppendChannel = false;
        private bool _readyAppendPage = false;

        public static SearchCondition Current
        {
            get
            {
                return new SearchCondition();
            }
        }

        private const string P = "P";
        private const string CHANNEL_ID = "ChannelId";        

        private IList<_SearchObj> _list = new List<_SearchObj>();


        public SearchCondition AddChannel(string value)
        {
            if (!this._readyAppendChannel)
            {
                this._list.Insert(0, new _SearchObj(CHANNEL_ID, value));
                this._readyAppendChannel = true;
            }
            return this;

        }

        public SearchCondition AddPage(int page, int size)
        {
            if (!this._readyAppendPage)
            {
                this._list.Add(new _SearchObj(P, new object[] { page, size }));
                this._readyAppendPage = true;
            }
            return this;
        }


        public SearchCondition AddBetween(string key, object between, object and)
        {
            this.Add(key, new object[] { between, and });
            return this;
        }
        public SearchCondition AddArray(string key, object array)
        {
            if (!this._readyAppendPage)
            {
                this._list.Add(new _SearchObj(P, array));
                this._readyAppendPage = true;
            }
            return this;
        }
        public SearchCondition AddSort(string key)
        {
            return this.AddSort(key, false);
        }

        public SearchCondition AddSort(string key, bool asc)
        {
            var Sort = asc ? 1 : 2;
            this._list.Add(new _SearchObj(key, TOpeart.SORT, Sort));
            return this;
        }

        public SearchCondition Add(string key, object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                this._list.Add(new _SearchObj(key, value));
            return this;
        }
        /// <summary>
        /// 正整数，不能为0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SearchCondition AddInt(string key, object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && int.Parse(value.ToString()) >0)
                this._list.Add(new _SearchObj(key, value));
            return this;
        }
        /// <summary>
        /// 非负数，允许为0
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public SearchCondition AddNumber(string key, object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()) && int.Parse(value.ToString()) >= 0)
                this._list.Add(new _SearchObj(key, value));
            return this;
        }
        public SearchCondition Add(string key, params object[] arry)
        {
            this._list.Add(new _SearchObj(key, null, arry));
            return this;
        }

        public SearchCondition Add(string key, TOpeart opt, object value)
        {
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                this._list.Add(new _SearchObj(key, opt, value));
            return this;
        }


        public string Sign
        {
            get
            {
                var message = JsonConvert.SerializeObject(this, new _SearchJsonConverter(0)).ToLower();
                message = StringHelper.ZhCnToUnicode(message);
                var md5 = MD5.Create(); 
                var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(BaseDAL.WebKey + message));
                var sb = new StringBuilder();
                foreach (var item in buffer)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
        }


        public IEnumerator GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new _SearchJsonConverter());
        }
    }

    public sealed class ArrySerialization : IEnumerable
    {

        private const string SALT = "95e7924a-51ee-49dc-b396-2e816780ca0e";


        private List<object> _list = new List<object>();

        private sealed class _ArryJsonConverter : JsonConverter
        {
            private int? _token;

            public _ArryJsonConverter()
            {

            }

            public _ArryJsonConverter(int token)
            {
                this._token = token;
            }

            public override bool CanConvert(Type objectType)
            {
                return true;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            private void WriteValue(JsonWriter writer, object obj)
            {
                if (obj == null)
                {
                    this.WriteNullValue(writer, obj);
                }
                else if (obj is DateTime)
                {
                    var date = Convert.ToDateTime(obj);
                    this.WriteValue(writer, date.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (obj is ValueType)
                {
                    this.WriteValueTypeValue(writer, obj);
                }
                else if (obj is object[])
                {
                    this.WriteArryValue(writer, obj);
                }
                else
                {
                    this.WriteObjValue(writer, obj);
                }
            }

            private void WriteValueTypeValue(JsonWriter writer, object obj)
            {
                writer.WriteRawValue(obj.ToString());
            }

            private void WriteObjValue(JsonWriter writer, object obj)
            {
                writer.WriteValue(obj.ToString());
            }

            private void WriteNullValue(JsonWriter writer, object obj)
            {
                writer.WriteRaw("Null");
            }

            private void WriteArryValue(JsonWriter writer, object obj)
            {
                var arry = obj as object[];
                writer.WriteStartArray();
                foreach (var item in arry)
                {
                    this.WriteValue(writer, item);
                }
                writer.WriteEndArray();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var obj = value as ArrySerialization;
                writer.WriteStartArray();

                foreach (var o in obj)
                {
                    writer.WriteValue(o);
                }
                if (this._token == null)
                {
                    this.WriteValue(writer, obj.Sign);
                }
                writer.WriteEndArray();
            }
        }

        public static ArrySerialization Current
        {
            get
            {
                return new ArrySerialization();
            }
        }

        public ArrySerialization Add(object value)
        {
            this._list.Add(value);
            return this;
        }

        public ArrySerialization AddRange(params object[] args)
        {
            this._list.AddRange(args);
            return this;
        }


        public string Sign
        {
            get
            {
                var message = (SALT + JsonConvert.SerializeObject(this, new _ArryJsonConverter(0))).ToLower();
                message = StringHelper.ZhCnToUnicode(message);
                var md5 = MD5.Create();
                var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(message));
                var sb = new StringBuilder();
                foreach (var item in buffer)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new _ArryJsonConverter());
        }

    }



}

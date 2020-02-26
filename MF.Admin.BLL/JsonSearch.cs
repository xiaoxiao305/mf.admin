using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography;

namespace MF.Admin.BLL
{
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
        SORT = 8
    }



    public sealed class SearchCondition
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
               
                //var obj = value as SearchCondition;
                //writer.WriteStartArray();

                //foreach (var o in obj)
                //{
                //    var item = o as _SearchObj;
                //    writer.WriteStartArray();
                //    writer.WriteValue(item.Key);
                //    this.WriteValue(writer, item.Value);
                //    if (item.Opt != null)
                //    {
                //        writer.WriteValue(item.Opt);
                //    }
                //    writer.WriteEndArray();
                //}
                //if (this._token == null)
                //{
                //    this.WriteValue(writer, obj.Sign);
                //}
                //writer.WriteEndArray();

                string json = existingValue as string;
                reader.Read();
                foreach (var j in json)
                {
                    reader.Read();
                    reader.ReadAsString();
                     
                }


                throw new NotImplementedException();
            }

            private void WriteValue(JsonWriter writer, object obj)
            {
                if (obj == null)
                {
                    this.WriteNullValue(writer, obj);
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
                writer.WriteRawValue($"{obj}");
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
        private const string SALT = "08ffca00-3050-4217-a2df-1c0b4493e604";

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

        public SearchCondition Add(string key, params object[] arry)
        {
            this._list.Add(new _SearchObj(key, null, arry));
            return this;
        }

        public SearchCondition Add(string key, TOpeart opt, object value)
        {
            this._list.Add(new _SearchObj(key, opt, value));
            return this;
        }


        public string Sign
        {
            get
            {
                var message = JsonConvert.SerializeObject(this, new _SearchJsonConverter(0)).ToLower();
                var md5 = MD5.Create();
                //Base.WriteLog("SALT + message:", SALT + message);
                var buffer = md5.ComputeHash(Encoding.UTF8.GetBytes(SALT + message));
                var sb = new StringBuilder();
                foreach (var item in buffer)
                {
                    sb.Append(item.ToString("x2"));
                }
                //Base.WriteLog("sign:", sb.ToString());
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


}

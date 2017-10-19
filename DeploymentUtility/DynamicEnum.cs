using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
namespace DeploymentUtility
{
    /// <summary>
    /// 动态映射枚举
    /// </summary>
    public class DynamicEnum : DynamicObject
    {
        private string _Key { get; set; }
        private Dictionary<string, DynamicEnumValue> _dicEnumSource { get; set; }

        protected DynamicEnum()
        {
            this._Key = "";
        }
        public DynamicEnum(Dictionary<string, object> dicEnumSource)
        {
            this._Key = "";
            this._dicEnumSource = dicEnumSource.ToDictionary(t => t.Key, t => new DynamicEnumValue(t.Key, t.Value));
        }
        private DynamicEnum(DynamicEnum dynamicEnum, string key)
        {

            this._Key = (dynamicEnum._Key + "." + key).TrimStart('.');
            this._dicEnumSource = dynamicEnum._dicEnumSource;
        }

        public static implicit operator int(DynamicEnum dynamicEnum)
        {
            throw new Exception("枚举值读取异常：" + dynamicEnum._Key);
        }
        public static implicit operator string(DynamicEnum dynamicEnum)
        {
            throw new Exception("枚举值读取异常：" + dynamicEnum._Key);
        }

        public DynamicEnumValue this[string key]
        {
            get
            {
                lock (_dicEnumSource)
                {
                    var newKey = (this._Key + "." + key).TrimStart('.');
                    try
                    {
                        return _dicEnumSource[newKey];
                    }
                    catch
                    {
                        throw new Exception("枚举值读取异常：" + newKey);
                    }
                }

            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            lock (_dicEnumSource)
            {
                result = new DynamicEnum(this, binder.Name);

                if (this._dicEnumSource.ContainsKey(((DynamicEnum)result)._Key))
                    result = _dicEnumSource[((DynamicEnum)result)._Key];
            }
            return true;
        }


        public string[] GetAllKeys()
        {

            return this._dicEnumSource
                .Where(t => t.Key.StartsWith(_Key))
                .Select(t => (t.Key.Substring(this._Key.Length)).TrimStart('.'))
                .ToArray();
        }
        public DynamicEnumValue TryParse(object value, DynamicEnumValue defaulValue = null)
        {
            var result = defaulValue ?? new DynamicEnumValue("", "");

            var item = this._dicEnumSource.Where(t => t.Key.StartsWith(this._Key) && t.Value.EqualValue(value)).FirstOrDefault();
            if (item.Key != null)
                result = item.Value;

            return result;
        }

        public override string ToString()
        {
            return this._Key;
        }

        protected void SetEnumSource(Dictionary<string, object> dicEnumSource)
        {
            this._dicEnumSource = dicEnumSource.ToDictionary(t => t.Key, t => new DynamicEnumValue(t.Key, t.Value));
        }

    }

    public class DynamicEnumValue
    {
        private string _Name { get; set; }
        private object _Value { get; set; }
        public DynamicEnumValue(string name, object value)
        {
            this._Name = name;
            this._Value = value;
        }
        public bool EqualValue(object value)
        {
            return _Value.ToString() == value.ToString();
        }
        public static implicit operator int(DynamicEnumValue enumValue)
        {
            return Convert.ToInt32(enumValue._Value);
        }
        public static implicit operator string(DynamicEnumValue enumValue)
        {
            return enumValue._Value.ToString();
        }
        public override string ToString()
        {
            return _Name;
        }
    }


}

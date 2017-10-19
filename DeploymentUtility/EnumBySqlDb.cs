using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace DeploymentUtility
{
    /// <summary>
    /// 枚举（映射到SqlServer)
    /// </summary>
    public class EnumBySqlDb : DynamicEnum
    {
        private byte[] _RVersion { get; set; }
        private EnumBySqlDbCfg _cfg { get; set; }
        private Dictionary<string, object> _dicEnumSource { get; set; }

        private EnumBySqlDb()
        {
        }
        private EnumBySqlDb(EnumBySqlDbCfg cfg)
        {
            this._RVersion = new byte[8];
            this._cfg = cfg;
            this._dicEnumSource = new Dictionary<string, object>();
        }

        public static EnumBySqlDb Create(EnumBySqlDbCfg cfg)
        {
            var objEnum = new EnumBySqlDb(cfg);
           
            objEnum.Refresh();
            return objEnum;
        }

        /// <summary>
        /// 刷新映射的数据
        /// </summary>
        public void Refresh()
        {
            lock (_dicEnumSource)
            {
                var dic = this.GetDicEnumSource();
                foreach (var item in dic)
                {
                    if (_dicEnumSource.ContainsKey(item.Key))
                        _dicEnumSource[item.Key] = item.Value;
                    else
                        _dicEnumSource.Add(item.Key, item.Value);
                }
                base.SetEnumSource(this._dicEnumSource);
            }
        }
        private Dictionary<string, object> GetDicEnumSource()
        {
            try
            {
                var dic = new Dictionary<string, object>() { };
                var tb = new DataTable();

                var pRVersion = new SqlParameter("@RVersion", this._RVersion);
                string sql = string.Format("SELECT {0},{1},{2} FROM  {3} WHERE {2}>@RVersion ORDER BY {2} ASC", _cfg.Cfg_Column_Key, _cfg.Cfg_Column_Value, _cfg.Cfg_Column_RVersion, _cfg.Cfg_Table);
                var cmd = new SqlCommand(sql, _cfg.Cfg_Conn);
                cmd.Parameters.Add(pRVersion);
                var sda = new SqlDataAdapter(cmd);
                sda.Fill(tb);

                foreach (DataRow row in tb.Rows)
                {
                    dic.Add(row[_cfg.Cfg_Column_Key].ToString(), row[_cfg.Cfg_Column_Value].ToString());
                    this._RVersion = (byte[])row[_cfg.Cfg_Column_RVersion];
                }
                return dic;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("配置异常,请检查配置是否正确：{0}", ex.Message));
            }
        }
    }
    /// <summary>
    /// 枚举配置（映射到SqlServer)
    /// </summary>
    public class EnumBySqlDbCfg
    {
        /// <summary>
        /// 数据链接配置
        /// </summary>
        public SqlConnection Cfg_Conn { get; set; }
        /// <summary>
        /// 枚举表配置
        /// </summary>
        public string Cfg_Table { get; set; }
        /// <summary>
        /// 枚举键配置
        /// </summary>
        public string Cfg_Column_Key { get; set; }
        /// <summary>
        /// 枚举值配置
        /// </summary>
        public string Cfg_Column_Value { get; set; }
        /// <summary>
        /// 数据版本配置
        /// </summary>
        public string Cfg_Column_RVersion { get; set; }
    }
}

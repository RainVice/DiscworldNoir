using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Mono.Data.Sqlite;
using Script.Data;
using UnityEngine;

namespace DB
{
    public class QueryWapper<T> : BaseWapper<List<T>,T> where T : BaseData, new()
    {

        // 数据集
        private List<T> m_Datas = new();

        // 条件
        private List<Tuple<string, string, object>> m_Conditions = new();

        // 连接符
        private List<string> m_Connector = new();

        // 判断相等
        public QueryWapper<T> eq(string key, object value)
        {
            IsEqual();
            m_Conditions.Add(new Tuple<string, string, object>(key, "=", value));
            return this;
        }

        // 不等于
        public QueryWapper<T> ne(string key, object value)
        {
            IsEqual();
            m_Conditions.Add(new Tuple<string, string, object>(key, "!=", value));
            return this;
        }

        // 大于
        public QueryWapper<T> gt(string key, object value)
        {
            IsEqual();
            m_Conditions.Add(new Tuple<string, string, object>(key, ">", value));
            return this;
        }

        // 小于
        public QueryWapper<T> lt(string key, object value)
        {
            IsEqual();
            m_Conditions.Add(new Tuple<string, string, object>(key, "<", value));
            return this;
        }

        // 大于等于
        public QueryWapper<T> ge(string key, object value)
        {
            IsEqual();
            m_Conditions.Add(new Tuple<string, string, object>(key, ">=", value));
            return this;
        }

        // 小于等于
        public QueryWapper<T> le(string key, object value)
        {
            IsEqual();
            m_Conditions.Add(new Tuple<string, string, object>(key, "<=", value));
            return this;
        }

        // 与
        public QueryWapper<T> and()
        {
            IsLess();
            m_Connector.Add("AND");
            return this;
        }

        // 或
        public QueryWapper<T> or()
        {
            IsLess();
            m_Connector.Add("OR");
            return this;
        }

        // 判断条件与连接符是否等长
        private void IsEqual()
        {
            if (m_Connector.Count != m_Conditions.Count)
            {
                throw new InvalidOperationException("需要使用连接符");
            }
        }

        // 判断连接符是否少于条件
        private void IsLess()
        {
            if (m_Conditions.Count == m_Connector.Count)
            {
                throw new InvalidOperationException("数据量与连接符不匹配");
            }
        }

        public override List<T> Do()
        {
            var sql = $"SELECT * FROM {typeof(T).Name}";
            if (m_Conditions.Count > 0)
            {
                sql += " WHERE ";
                for (int i = 0; i < m_Conditions.Count; i++)
                {
                    sql += m_Conditions[i].Item1 + " " + m_Conditions[i].Item2 + " " + DBUtil.TypeToValue(m_Conditions[i].Item3.GetType(), m_Conditions[i].Item3.ToString());
                    if (i < m_Conditions.Count - 1)
                    {
                        sql += $" {m_Connector[i]} ";
                    }
                }
            }
            var datas = DBUtil.Instance.Query<T>(typeof(T), sql);
            return datas;
        }
    }
}
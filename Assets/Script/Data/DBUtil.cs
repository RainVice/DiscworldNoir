using System;
using System.Collections.Generic;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine;

namespace DB
{
    public class DBUtil
    {
        public static DBUtil Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new DBUtil();
                }

                return m_Instance;
            }
        }

        private static DBUtil m_Instance;

        // 连接池
        private Dictionary<Type, SqliteConnection> m_ConnectionPool = new();

        // 连接
        private SqliteConnection m_Connection;

        // 查询
        public List<T> Query<T>(Type type, string sql) where T : BaseData, new()
        {
            List<T> datas = new();
            Debug.Log(sql);
            IsNull(type);
            var command = m_Connection.CreateCommand();
            command.CommandText = sql;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Debug.Log("获取数据");
                var data = new T();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader.GetValue(i);
                    if (value == DBNull.Value)
                    {
                        value = null;
                    }
                    var property = data.GetType().GetField(name);
                    property.SetValue(data, value);
                }
                datas.Add(data);
            }
            reader.Close();
            return datas;
        }


        // 添加
        public int Insert(Type type, string sql)
        {
            Debug.Log(sql);
            IsNull(type);
            var command = new SqliteCommand(sql, m_Connection);
            var result = command.ExecuteNonQuery();
            return result;
        }

        private void IsNull(Type type)
        {
            if (m_Connection == null)
            {
                var path = Application.persistentDataPath + "/DiscworldNoir.db";
                m_Connection = new SqliteConnection("Data Source=" + path);
                m_Connection.Open();
            }

            // 判断表是否存在
            var command = m_Connection.CreateCommand();
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{type.Name}';";
            var result = command.ExecuteScalar();
            
            // 没有就创建表
            if (result == null || result.ToString() != type.Name)
            {
                // 创建表
                var sql = $"CREATE TABLE {type.Name} (";
                var fieldInfos = type.GetFields();
                for (var i = 0; i < fieldInfos.Length; i++)
                {
                    var field = fieldInfos[i];
                    if (!field.IsPublic) continue;

                    var typeToString = GetTypeToString(field.FieldType);
                    sql += $"{field.Name} {typeToString}";
                    
                    if (i != fieldInfos.Length - 1)
                    {
                        sql += ",";
                    }
                }
                sql += ")";
                Debug.Log(sql);
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 数据类型转Sql类型
        /// </summary>
        /// <param name="fieldType">数据类型</param>
        /// <returns>sql值类型</returns>
        public static string GetTypeToString(Type fieldType)
        {

            if (fieldType.Name == "String")
            {
                return "varchar(255)";
            }

            if (fieldType.Name == "Int32" || fieldType.IsEnum)
            {
                return "int";
            }

            return fieldType.Name;
        }
        
        /// <summary>
        /// 判断数据值转Sql类型值
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string TypeToValue(Type type, string value)
        {
            return GetTypeToString(type) switch
            {
                "varchar(255)" => $"'{value}'",
                _ => value
            };
        }
        
    }
}
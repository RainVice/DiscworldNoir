using System;
using System.Collections.Generic;
using System.Reflection;
using DB.Constraint;
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
                if (m_Instance is null)
                {
                    m_Instance = new DBUtil();
                }

                return m_Instance;
            }
        }

        private static DBUtil m_Instance;

        // 连接池
        // private Dictionary<Type, SqliteConnection> m_ConnectionPool = new();

        // 连接
        private SqliteConnection m_Connection;

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="sql">sql语句</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>查询到的结果</returns>
        public List<T> Query<T>(Type type, string sql) where T : BaseData, new()
        {
            List<T> datas = new();
            Debug.Log($"Select: {sql}");
            IsNull(type);
            var command = m_Connection.CreateCommand();
            command.CommandText = sql;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var data = new T();
                var fieldInfos = typeof(T).GetFields();
                for (var i = 0; i < fieldInfos.Length; i++)
                {
                    var field = fieldInfos[i];
                    if (!field.IsPublic) continue;
                    object value;
                    try
                    {
                        value = reader[field.Name];
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    var property = data.GetType().GetField(field.Name);
                    if (field.FieldType == typeof(bool))
                    {
                        value = Convert.ToBoolean(value);
                    }
                    property.SetValue(data,value);
                }
                datas.Add(data);
            }

            reader.Close();
            return datas;
        }


        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public int Insert(Type type, string sql)
        {
            Debug.Log($"Insert: {sql}");
            IsNull(type);
            var command = new SqliteCommand(sql, m_Connection);
            var result = command.ExecuteNonQuery();
            return result;
        }

        /// <summary>
        /// 判断是否存在表，不在则创建表
        /// </summary>
        /// <param name="type"></param>
        private void IsNull(Type type)
        {
            if (m_Connection is null)
            {
                var path = Application.streamingAssetsPath + "/DiscworldNoir.db";
                m_Connection = new SqliteConnection("Data Source=" + path);
                m_Connection.Open();
            }

            // 判断表是否存在
            var command = m_Connection.CreateCommand();
            command.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{type.Name}';";
            var result = command.ExecuteScalar();

            // 没有就创建表
            if (result is null || result.ToString() != type.Name)
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
                    if (field.GetCustomAttribute<PrimaryKey>() is not null)
                    {
                        sql += " PRIMARY KEY";
                    }

                    if (i != fieldInfos.Length - 1)
                    {
                        sql += ",";
                    }
                }

                sql += ")";
                Debug.Log($"Create Table: {sql}");
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
            var fieldTypeName = fieldType.Name;
            if (fieldTypeName == "Int32" || fieldType.IsEnum)
            {
                return "int";
            }

            return fieldTypeName switch
            {
                "String" => "varchar(255)",
                "Single" => "float",
                "Boolean" => "boolean",
                _ => fieldTypeName
            };
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
        
        ~ DBUtil()
        {
            m_Connection.Close();
        }

    }
}
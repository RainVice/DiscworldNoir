using Mono.Data.Sqlite;
using UnityEngine;

namespace DB
{
    public class DBUtil : MonoBehaviour
    {
        // 数据库连接对象
        private SqliteConnection m_Connection;
        // 数据库命令
        private SqliteCommand m_Command;
        // 数据读取定义
        private SqliteDataReader m_Reader;
        // 本地数据库
        private string m_DBName;
    }
}
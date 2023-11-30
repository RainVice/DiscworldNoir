using System.Collections.Generic;

namespace DB
{
    public class InsertWapper<T> : BaseWapper<bool,T> where T : BaseData
    {
        public InsertWapper() { }

        public InsertWapper(T t)
        {
            this.t = t;
        }

        public bool Do(IEnumerable<T> ts)
        {
            return Do(new List<T>(ts));
        }
        
        public bool Do(List<T> ts)
        {
            if (ts == null) return false;
            // 生成sql语句
            var sql = $"INSERT INTO {typeof(T).Name} VALUES ";
            for (var i = 0; i < ts.Count; i++)
            {
                var data = ts[i];
                sql += GenerateSqlValue(data);
                if (i != ts.Count - 1) sql += ",";
            }
            var result = DBUtil.Instance.Insert(typeof(T), sql);
            return result == 0;
        }

        public bool Do(T t)
        {
            this.t = t;
            return Do();
        }

        public override bool Do()
        {
            if (t == null) return false;
            // 生成sql语句
            var sql = $"INSERT INTO {typeof(T).Name} VALUES ";
            sql += GenerateSqlValue(t);
            var result = DBUtil.Instance.Insert(typeof(T), sql);
            return result == 1;
        }

        private string GenerateSqlValue(T data)
        {
            var temp = "";
            temp += "(";
            var fieldInfos = data.GetType().GetFields();
            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var field = fieldInfos[i];
                if (!field.IsPublic) continue;
                string value;
                if (field.FieldType.IsEnum)
                {
                    value = ((int)field.GetValue(data)).ToString();
                }
                else
                {
                    value = field.GetValue(data).ToString();
                }
                temp += DBUtil.TypeToValue(field.FieldType, value);
                if (i != fieldInfos.Length - 1)
                {
                    temp += ",";
                }
            }
            temp += ")";
            return temp;
        }
    }
}
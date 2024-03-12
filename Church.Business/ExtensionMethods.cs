using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Church.Data;
using static Church.Business.QueryHelper;

namespace Church.Business
{
    public static class ExtensionMethods
    {
        public static Column GetName<T>(Expression<Func<T, object>> f)
        {
            Column column = new Column();

            var body = f.Body;
            if (body.NodeType == ExpressionType.Convert)
                body = ((UnaryExpression)body).Operand;

            MemberExpression mb = (body as MemberExpression);

            if (mb != null)
            {
                column.Table = mb.Expression.Type.Name;
                column.Name = mb.Member.Name;
                var cAttr = (body as MemberExpression).Member.GetCustomAttributes(typeof(DTOAtribute), true);
                var As = cAttr.Length > 0 ? (cAttr[0] as DTOAtribute).Name : null;
                column.As = As;
            }

            return column;
        }

        public static string SelectColumns<T>(this IQueryable<T> source, params Expression<Func<T, object>>[] stringProperties)
        {
            string className = typeof(T).Name;
            string sFields = " ";

            foreach (var stringProperty in stringProperties)
            {
                Column column = GetName(stringProperty);
                string table = column.Table != className ? column.Table : className;
                string property = column.Name;
                sFields += string.Format("{0}.{1} {2}, ", table, property, string.IsNullOrEmpty(column.As) ? string.Empty : "AS " + column.As);
            }

            sFields = sFields.Substring(0, sFields.Length - 1);
            sFields = sFields.Remove(sFields.Length - 1);
            return sFields;
        }

        public static string GetJoin<T>(this IQueryable<T> source, Expression<Func<T, object>> Joins, string type, string OtherTable)
        {
            string sJoins = string.Empty;
            string MainTable = typeof(T).Name;
            OtherTable = string.IsNullOrEmpty(OtherTable) ? MainTable : OtherTable;
            string property = GetName(Joins).Name;
            sJoins += string.Format(" {0} {1} ON {2}.{3} = {1}.{3}", type, OtherTable, MainTable, property);
            return sJoins;
        }


        public static string RemoveXChar(this string Loquesea)
        {
            return Loquesea.Replace("X", "Y");
        }

        public static string GetCondition<T>(this IQueryable<T> source, List<object> values, params Expression<Func<T, object>>[] Conditions)
        {
            string className = typeof(T).Name;
            string Where = string.Empty;
            for (int index = 0; index < Conditions.Length; index++)
            {
                Column column = GetName(Conditions[index]);
                string Type = values[index].GetType().ToString();
                string property = column.Name;
                string table = column.Table != className ? column.Table : className;

                switch (Type)
                {
                    case "System.Boolean":
                        Where += ((index == 0) || (index > 0 && Where == "")) ? string.Empty : " AND";
                        Where += string.Format(" ( {0}.{1} = {2} )", table, property, values[index].ToString().ToLower().Contains("true") ? 1 : 0);
                        break;
                    case "System.String":
                        Where += ((index == 0) || (index > 0 && Where == "")) ? string.Empty : " AND";
                        Where += string.Format(" ( '{2}' = '' OR {0}.{1} LIKE '%{2}%' )", table, property, values[index].ToString());
                        break;
                    case "System.DateTime":
                        if (!Convert.ToDateTime(values[index]).Equals(default(DateTime)))
                        {
                            string sqlDate = Convert.ToDateTime(values[index].ToString()).ToString("yyyy-MM-dd");
                            Where += ((index == 0) || (index > 0 && Where == "")) ? string.Empty : " AND";
                            Where += string.Format(" ( '{2}' IS NULL OR Cast({0}.{1} as date)= Cast('{2}' as date))", table, property, sqlDate);
                        }
                        break;
                    case "System.Int32":
                        Where += ((index == 0) || (index > 0 && Where == "")) ? string.Empty : " AND";
                        Where += string.Format(" ( {2} = 0 OR {0}.{1} = {2} ) ", table, property, int.Parse(values[index].ToString()));
                        break;
                }
            }

            return Where;
        }
    }
}

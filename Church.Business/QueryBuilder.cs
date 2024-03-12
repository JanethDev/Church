using System.Diagnostics;

namespace Church.Business
{
    public class QueryBuilder
    {      
        public string GetSorting(string SortColumn, string SortOrder, string sSortingOrder)
        {           
            if (sSortingOrder.Contains("_Desc"))
            {
                SortOrder = "DESC";
                SortColumn = sSortingOrder.Replace("_Desc", "");
            }
            else
            {
                SortColumn = sSortingOrder != "" ? sSortingOrder : SortColumn;
            }
            
            return SortColumn + " " + SortOrder;
        }

        public string GetQuery(string table, string Fields, string Joins, string Filters, string Sort, int PageNumber, int PageSize)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();

            string query = "SELECT * FROM ( SELECT ROW_NUMBER() OVER (ORDER BY " + Sort + ") AS ROWNUM,Count(*) over() AS TotalCount,";
            query += Fields;
            query += " FROM " + table;
            query += " ";
            query += Joins;
            query += " WHERE";
            query += Filters;
            query += ") AS RESULT WHERE ROWNUM BETWEEN ((" + PageNumber + " - 1) * " + PageSize + " + 1) AND (" + PageNumber + " *" + PageSize + ")";
            query += " ORDER BY  ROWNUM ASC";
            sp.Stop();
            return query;
        }     
    }
}

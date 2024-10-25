using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;
using System.Data;

namespace church.backend.Controllers
{
    public class ExcelController : Controller
    {
        [HttpGet]
        [Route("excel/customer")]
        public string excelClient()
        {
            string query = string.Empty;
            string filePath = @"C:\Users\alexis.gonzalez\Downloads\Relacion Clientes 2024.xlsx";
            string nombreHoja = "COBRANZA";
            DataTable tablaExcel = ExcelReader(filePath, nombreHoja);
            foreach (DataRow row in tablaExcel.Rows)
            {
                query += "INSERT INTO [cat_customers]([customer_number],[name],[psurname],[msurname],[phone],[email])\n";
                query += "VALUES (";

                query += "'";
                query += row.ItemArray[1];
                query += "'";
                query += ",";

                string fullName = row.ItemArray[2]!.ToString()!;
                string name = string.Empty;
                string father = string.Empty;
                string mother = string.Empty;


                var splitName = fullName.Split(' ');
                if (splitName.Length == 3)
                {
                    father = splitName[0];
                    mother = splitName[1];
                    name = splitName[2];
                }

                if (splitName.Length == 2)
                {
                    father = splitName[0];
                    name = splitName[1];
                }

                if (splitName.Length == 1)
                {
                    name = splitName[0];
                }

                if (splitName.Length > 3)
                {
                    father = splitName[0];
                    mother = splitName[1];

                    for (int i = 2; i < splitName.Length; i++)
                    {
                        name += splitName[i] + " ";
                    }
                }


                if (string.IsNullOrEmpty(name))
                {
                    query += "null";
                }
                else
                {
                    query += "'";
                    query += name;
                    query += "'";
                }
                query += ",";

                if (string.IsNullOrEmpty(father))
                {
                    query += "null";
                }
                else
                {
                    query += "'";
                    query += father;
                    query += "'";
                }
                query += ",";

                if (string.IsNullOrEmpty(mother))
                {
                    query += "null";
                }
                else
                {
                    query += "'";
                    query += mother;
                    query += "'";
                }
                query += ",";


                string phone = row.ItemArray[3]!.ToString()!;

                if (string.IsNullOrEmpty(phone))
                {
                    query += "null";
                }
                else
                {
                    phone = phone.Replace(" ", "");
                    phone = phone.Replace(")", "");
                    phone = phone.Replace("(", "");
                    phone = phone.Replace("-", "");
                    phone = phone.Replace(".", "");
                    phone = phone.Replace("CEL", "");
                    phone = phone.Replace("TEL", "");

                    if (phone.Length <= 10)
                    {
                        query += "'";
                        query += phone;
                        query += "'";
                    }
                    else
                    {
                        query += "null";
                    }
                }
                query += ",";

                string email = row.ItemArray[4]!.ToString()!;

                if (string.IsNullOrEmpty(email))
                {
                    query += "null";
                }
                else
                {
                    phone = phone.Replace(" ", "");
                    phone = phone.Replace(")", "");
                    phone = phone.Replace("(", "");
                    phone = phone.Replace("-", "");
                    phone = phone.Replace(".", "");
                    phone = phone.Replace("CEL", "");
                    phone = phone.Replace("TEL", "");
                    query += "'";
                    query += email;
                    query += "'";
                }

                query += ")\n";
            }
            return query;
        }

        [HttpGet]
        [Route("excel/update/sell/crypts")]
        public string excelSellCrypts()
        {
            string query = string.Empty;
            string filePath = @"C:\Users\alexis.gonzalez\Downloads\Relacion Clientes 2024.xlsx";
            string nombreHoja = "COBRANZA";
            DataTable tablaExcel = ExcelReader(filePath, nombreHoja);
            query += @"create table #temp(
	id int,
	full_position varchar(100),
	full_position_format varchar (100)
)
	
INSERT INTO #temp
SELECT
	[id]
	,[full_position]
	,CONCAT(
		[zone],
		(CASE
			WHEN LEN(position) = 3 THEN LEFT(position, 1) + RIGHT('0' + SUBSTRING(position, 2, LEN(position)), 3)
			ELSE LEFT(position, 1) + RIGHT('00' + SUBSTRING(position, 2, LEN(position)), 3)
		END)
	) as 'full_position_format'
FROM [cat_crypts]

UPDATE [cat_crypts]
SET  [cat_status_id]  = '1010' -- vendido
FROM [cat_crypts] c
INNER JOIN #temp t ON t.id = c.id
WHERE t.full_position_format in (";
            query += "\n\t";
            foreach (DataRow row in tablaExcel.Rows)
            {
                string position = row.ItemArray[18]!.ToString()!;
                if (!string.IsNullOrWhiteSpace(position))
                {
                    if (!row.ItemArray[19]!.ToString()!.ToLower().Contains("cancel"))
                    {
                        var splitPosition = position.Split('-');
                        if(splitPosition.Length > 1) 
                        {
                            query += "'";
                            query += splitPosition[0];
                            query += "'";
                            query += ",";
                            query += "\n\t";
                        }
                        else
                        {
                            query += "'";
                            query += position;
                            query += "'";
                            query += ",";
                            query += "\n\t";
                        }
                    }
                }
            }
            query = query.Remove(query.Length - 1);
            query = query.Remove(query.Length - 1);
            query = query.Remove(query.Length - 1);
            query += "\n";
            query += ")\n\n";
            query += "DROP TABLE #temp";
            return query;
        }

        private DataTable ExcelReader(string filePath, string hoja)
        {
            string extension = Path.GetExtension(filePath);
            string connectionString = string.Empty;

            if (extension == ".xls")
            {
                connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else if (extension == ".xlsx")
            {
                connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\"";
            }
            else
            {
                throw new Exception("El formato del archivo no es válido.");
            }

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand($"SELECT * FROM [{hoja}$]", connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable data = new DataTable();
                adapter.Fill(data);
                connection.Close();
                return data;
            }
        }
    }
}

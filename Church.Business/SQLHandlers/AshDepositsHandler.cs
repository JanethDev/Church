using Church.Data;
using Dapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace Church.Business.SQLHandlers
{
    public class AshDepositsHandler : SqlMapper.TypeHandler<List<tblAshDeposits>>
    {
        public override List<tblAshDeposits> Parse(object value)
        {
            return JsonConvert.DeserializeObject<List<tblAshDeposits>>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, List<tblAshDeposits> value)
        {
            parameter.Value = value;
        }
    }
}

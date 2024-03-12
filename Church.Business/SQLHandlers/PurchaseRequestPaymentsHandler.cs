using Church.Data;
using Dapper;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;

namespace Church.Business.SQLHandlers
{
    public class PurchaseRequestPaymentsHandler : SqlMapper.TypeHandler<List<tblPurchaseRequestPayments>>
    {
        public override List<tblPurchaseRequestPayments> Parse(object value)
        {
            return JsonConvert.DeserializeObject<List<tblPurchaseRequestPayments>>(value.ToString());
        }

        public override void SetValue(IDbDataParameter parameter, List<tblPurchaseRequestPayments> value)
        {
            parameter.Value = value;
        }
    }
}

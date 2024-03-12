using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class ChurchDB : DbContext
    {
        public ChurchDB()
            : base("DefaultConnection")
        {
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 180;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            Database.SetInitializer<ChurchDB>(null);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<tblUsers> tblUsers { get; set; }

        public DbSet<tblRoles> tblRoles { get; set; }
        public DbSet<tblCivilStatus> tblCivilStatus { get; set; }
        public DbSet<tblStates> tblStates { get; set; }
        public DbSet<tblCustomers> tblCustomers { get; set; }
        public DbSet<tblTowns> tblTowns { get; set; }
        public DbSet<tblMaintenanceFee> tblMaintenanceFee { get; set; }
        public DbSet<tblFederalTax> tblFederalTax { get; set; }
        public DbSet<tblExchangeRates> tblExchangeRates { get; set; }
        public DbSet<tblCurrency> tblCurrency { get; set; }
        public DbSet<tblAnniversary> tblAnniversary { get; set; }
        public DbSet<tblCrypts> tblCrypts { get; set; }
        public DbSet<tblPurchasesRequests> tblPurchasesRequests { get; set; }
        public DbSet<tblPurchasesRequestsQuotation> tblPurchasesRequestsQuotation { get; set; }
        public DbSet<tblContracts> tblContracts { get; set; }
        public DbSet<tblBeneficiaryCustomers> tblBeneficiaryCustomers { get; set; }
        public DbSet<tblAshDeposits> tblAshDeposits { get; set; }
        public DbSet<tblDiscounts> tblDiscounts { get; set; }
        public DbSet<tblCommissionAgents> tblCommissionAgents { get; set; }
        public DbSet<tblCathedrals> tblCathedrals { get; set; }
        public DbSet<tblPurchasesRequestsAshDeposits> tblPurchasesRequestsAshDeposits { get; set; }
        public DbSet<tblCryptPositionTypes> tblCryptPositionTypes { get; set; }
        public DbSet<tblIntentionsType> tblIntentionsType { get; set; }
        public DbSet<tblIntentions> tblIntentions { get; set; }
        public DbSet<tblCountedDiscount> tblCountedDiscount { get; set; }
        public DbSet<tblSchedule> tblSchedule { get; set; }
        public DbSet<tblPurchaseRequestPayments> tblPurchaseRequestPayments { get; set; }
        public DbSet<tblCities> tblCities { get; set; }
        public DbSet<tblPayments> tblPayments { get; set; }
        public DbSet<tblPushNotificationsMessages> tblPushNotificationsMessages { get; set; }
        public DbSet<tblReceipts> tblReceipts { get; set; }
        //metodos para obtener tabla 
        public TOutput FunctionTableValue<TOutput>(string functionName, SqlParameter[] parameters)
        {
            parameters = parameters ?? new SqlParameter[] { };

            //string commandText = String.Format("SELECT * FROM  " + Table , String.Format("{0}({1})", functionName, String.Join(",", parameters.Select(x => x.ParameterName))));
            //string commandText = String.Format("SELECT * FROM dbo.{0}", String.Format("{0}({1})", functionName, String.Join(",", parameters.Select(x => x.ParameterName))));
            string commandText = String.Format("exec {0}", String.Format("{0} {1}",functionName, String.Join(",",parameters.Select(x => x.ParameterName))));


            return ObjectContext.ExecuteStoreQuery<TOutput>(commandText, parameters[0],parameters[1],parameters[2]).FirstOrDefault();
        }

        private ObjectContext ObjectContext
        {
            get { return (this as IObjectContextAdapter).ObjectContext; }
        }
    }
}

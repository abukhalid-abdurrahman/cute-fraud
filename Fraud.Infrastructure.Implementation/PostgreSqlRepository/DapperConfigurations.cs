using System;
using Dapper;
using DapperExtensions.Mapper;
using Fraud.Concerns.Extensions;

namespace Fraud.Infrastructure.Implementation.PostgreSqlRepository
{
    public class DapperConfigurations
    {
        public static void Configure()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.SqliteDialect();
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(DapperClassMapper<>);
        }

        private class DapperClassMapper<T> : ClassMapper<T> where T: class
        {
            public override void Table(string tableName)
            {
                if (tableName.Equals("Action", StringComparison.CurrentCultureIgnoreCase))
                {
                    base.Table("actions");
                }
                else if (tableName.Equals("EventHistory", StringComparison.CurrentCultureIgnoreCase))
                {
                    base.Table("events_history");
                }                
                else if (tableName.Equals("Events", StringComparison.CurrentCultureIgnoreCase))
                {
                    base.Table("events");
                }                
                else if (tableName.Equals("Order", StringComparison.CurrentCultureIgnoreCase))
                {
                    base.Table("orders");
                }                
                else if (tableName.Equals("Scenario", StringComparison.CurrentCultureIgnoreCase))
                {
                    base.Table("scenarios");
                }                
                else if (tableName.Equals("State", StringComparison.CurrentCultureIgnoreCase))
                {
                    base.Table("states");
                }           
                else if (tableName.Equals("Users", StringComparison.CurrentCultureIgnoreCase))
                {
                    base.Table("users");
                }
                else
                    base.Table(tableName);

                foreach (var prop in EntityType.GetProperties())
                {
                    if (prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                        Map(prop).Column(prop.Name.ToSnakeCase()).Key(KeyType.Identity);
                    else
                        Map(prop).Column(prop.Name.ToSnakeCase());
                }

                var props = Properties;
            }

        }
    }
}

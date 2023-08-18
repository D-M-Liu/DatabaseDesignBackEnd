using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Context
{
    public class ModelContextDesignFac : IDesignTimeDbContextFactory<ModelContext>
    {
        public ModelContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ModelContext> builder = new DbContextOptionsBuilder<ModelContext>();
            string connStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=123.57.140.176)(PORT=1521))" +
                "(CONNECT_DATA=(SERVICE_NAME=ORCL)));User ID=C##CAR;password=Tj123456";
            builder.UseOracle(connStr);
            ModelContext ctx = new ModelContext(builder.Options);
            return ctx;
        }
    }
}

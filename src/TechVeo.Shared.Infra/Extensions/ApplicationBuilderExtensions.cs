using Microsoft.Extensions.DependencyInjection;
using TechVeo.Shared.Infra.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSharedInfra(this IApplicationBuilder app)
        {
            app.UseMiddleware<TechVeo.Shared.Infra.EventualConsistency.Middleware>();

            return app;
        }

        public static IApplicationBuilder RunMigration<DbContext>(this IApplicationBuilder app) where DbContext : TechVeoContext
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DbContext>();
            context.Database.Migrate();
            return app;
        }
    }
}

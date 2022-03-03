using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Alliander.Connectivity.Registry.Functions.Startup))]

namespace Alliander.Connectivity.Registry.Functions
{
    /// <summary>
    /// Function dependency injection configuration.
    /// </summary>
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddSingleton(sp => FromNode());
            builder.Services.AddSingleton(sp => FromRelationship());
            builder.Services.AddSingleton(sp => FromContainerRecipe());
        }

        public static INodes FromNode()
        {
            return new Nodes();
        }

        public static IRelationship FromRelationship()
        {
            return new Relationship();
        }

        public static IContainerRecipe FromContainerRecipe()
        {
            return new ContainerRecipe();
        }
    }
}
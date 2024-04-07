using Autofac;
using Autofac.Features.AttributeFilters;
using InventoryService.API.Serilog;
using InventoryService.Business.Services;
using InventoryService.Infraestructure.Services.Cache.Contract;
using InventoryService.Infraestructure.Services.Cache.Implementation;
using InventoryService.Infraestructure.Services.DataBase.Contract;
using InventoryService.Infraestructure.Services.DataBase.Implementation;

namespace InventoryService.API.IoCContainer
{
    public static class IoCContainer
    {
        public static ContainerBuilder BuildContext(this ContainerBuilder builder, IConfiguration configuration)
        {
            RegisterClients(builder, configuration);
            RegisterServices(builder, configuration);
            RegisterRepositories(builder, configuration);
            builder.Register(_ => new LogCreator(configuration)).SingleInstance();

            return builder;
        }

        private static void RegisterClients(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<MemoryCacheManager>().As<ICache>().SingleInstance();
        }

        private static void RegisterServices(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<InventoryServiceHandler>();
        }

        private static void RegisterRepositories(ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterType<FileDataBase>().As<IDataBase>();
        }
    }
}

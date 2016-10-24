[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Hungry.Services.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Hungry.Services.App_Start.NinjectWebCommon), "Stop")]

namespace Hungry.Services.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Data;
    using System.Data.Entity;
    using Infrastructure.Cache;
    using ServiceStack.Redis;
    using Infrastructure.Upload;
    using CloudinaryDotNet;
    using System.Configuration;
    public static class NinjectWebCommon
    {
        private const int DefaultBufferSize = 100;
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IHungryData>().To<HungryData>().InRequestScope();
            kernel.Bind<DbContext>().To<HungryContext>();

            kernel.Bind<IBufferedCache>().To<BufferedRedisCache>().InSingletonScope()
                .WithConstructorArgument(DefaultBufferSize);
            kernel.Bind<IRedisClient>().To<RedisClient>();


            var cloudinaryAccount = new Account(
                ConfigurationManager.AppSettings.Get("CloudinaryName"),
                ConfigurationManager.AppSettings.Get("CloudinaryKey"),
                ConfigurationManager.AppSettings.Get("CloudinarySecret")
            );
            kernel.Bind<IImageUploadService>().To<CloudinaryService>().InSingletonScope();
            kernel.Bind<Cloudinary>().To<Cloudinary>()
                .WithConstructorArgument(typeof(Account), cloudinaryAccount);
        }
    }
}

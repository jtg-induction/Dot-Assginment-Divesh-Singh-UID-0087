using RestaurantManagement.Data;
using RestaurantManagement.repository;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;
using Unity;
using Unity.Lifetime;

namespace RestaurantManagement
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            // 2. Register the Repository Layer
            container.RegisterType<IUserRepository, UserRepository>();

            // 3. Register the Service Layer
            container.RegisterType<IUserService, UserService>();
            // 4. Register your Password Hasher
            container.RegisterType<IPasswordService, PasswordService>();
            container.RegisterType<ITokenService, TokenService>();
            container.RegisterType<ITokenRepository, TokenRepository>();
            container.RegisterType<IObtainJwtService, ObtainJwtService>();
        }
    }
}
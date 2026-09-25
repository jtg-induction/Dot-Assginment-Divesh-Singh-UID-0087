using RestaurantManagement.Data;
using RestaurantManagement.Helper;
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
        public static void RegisterTypes(IUnityContainer container)
        {
            // 1. Register Data Context with HTTP Request Scope
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            // 2. Register the Repository Layer with HTTP Request Scope
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITokenRepository, TokenRepository>(new HierarchicalLifetimeManager());

            // 3. Register the Service Layer
            container.RegisterType<IUserService, UserService>();
            // 4. Register your Password Hasher
            container.RegisterType<IPasswordService, PasswordService>();
            container.RegisterType<ITokenService, TokenService>();
            container.RegisterType<ITokenRepository, TokenRepository>();
            container.RegisterType<IObtainJwtService, ObtainJwtService>();
            container.RegisterType<IRestaurantRepository, RestaurantRepository>();
            container.RegisterType<IRestaurantService, RestaurantService>();
            container.RegisterType<IMenuRepository, MenuRepository>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IClaimHelper, ClaimHelper>();
        }
    }
}

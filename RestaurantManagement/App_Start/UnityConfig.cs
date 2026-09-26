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
        //lazy container just create container at the asking of first instance not jus while while application spinup
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
            // 1. Register Data Context with HTTP Request Scope it create one instance of an dbcontext for per http request
            container.RegisterType<ApplicationDbContext>(new HierarchicalLifetimeManager());

            /* 2. Register the Repository Layer  all are transient by default it means if
             an repo call create an new instance and then dispose after the excution.
             */
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<ITokenRepository, TokenRepository>();
            container.RegisterType<IRestaurantRepository, RestaurantRepository>();
            container.RegisterType<IMenuRepository, MenuRepository>();
            container.RegisterType<IOrderItemRepository, OrderItemRepository>();
            container.RegisterType<IAddressRepository, AddressRepository>();
            container.RegisterType<IUserAddressRepository, UserAddressRepository>();
            container.RegisterType<IRestaurantOwnerRepository, RestaurantOwnerRepository>();
            container.RegisterType<IOrderRepository, OrderRepository>();
            // 3. Register the Service Layer
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IPasswordService, PasswordService>();
            container.RegisterType<ITokenService, TokenService>();
            container.RegisterType<IObtainJwtService, ObtainJwtService>();
            container.RegisterType<IRestaurantService, RestaurantService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IAddressService, AddressService>();
            container.RegisterType<IUserAddressService, UserAddressService>();
            //4. claim just used for finding out usefull cliam from jwt
            container.RegisterType<IClaimHelper, ClaimHelper>();

        }
    }
}

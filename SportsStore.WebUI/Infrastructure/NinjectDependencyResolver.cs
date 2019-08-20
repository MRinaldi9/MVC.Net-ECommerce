using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Ninject;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using System.Configuration;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            this.kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            //Put bindings here

            //Fake repos for debugging
            //Mock<IProductRepository> mockRepository = new Mock<IProductRepository>();
            //mockRepository.Setup(repos => repos.Products).Returns(new List<Product>
            //{
            //    new Product {Name = "Football",Price = 25},
            //    new Product {Name = "Surf Board",Price = 179},
            //    new Product {Name = "Running shoes",Price = 95},
            //});

            ////Every time i call an object that implement IProductRepository i obtain the same mock object
            //kernel.Bind<IProductRepository>().ToConstant(mockRepository.Object);

            //Set the DI that every time an IProductRepository is called it will be of the class EFProductRepos
            kernel.Bind<IProductRepository>().To<EFProductRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager.AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>().WithConstructorArgument("settings", emailSettings);

        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}
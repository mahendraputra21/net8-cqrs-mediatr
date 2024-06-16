using NetArchTest.Rules;
using System.Reflection;

namespace DewaEShop.Test
{
    public class ArchitectureTest
    {
        [Fact]
        public void DomainLayerShouldNotHaveAnyDependencies()
        {
            var domainAssembly = Assembly.Load("DewaEShop.Domain");
            var result = Types
                .InAssembly(domainAssembly)
                .ShouldNot()
                .HaveDependencyOnAny("DewaEShop.Application.Mapper", "DewaEShop.Infrastructure.Repositories")
                .GetResult();

            Assert.True(result.IsSuccessful);

        }
    }
}
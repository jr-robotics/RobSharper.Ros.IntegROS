using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace RosComponentTesting
{
    public static class DependencyResolver
    {
        private static readonly ServiceCollection _services = new ServiceCollection();
        public static IServiceCollection Services => _services;
    }
}
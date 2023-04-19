﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Emit;

namespace Net.EntityFramework.CodeGenerator.Core
{
    internal class PackageModuleIntentsProvider : IPackageModuleIntentsProvider
    {
        private readonly IDbContextModelExtractor _ModelExtractor;
        private readonly IPackageModuleBuilderProvider _moduleBuilderProvider;
        public PackageModuleIntentsProvider(IDbContextModelExtractor model, IPackageModuleBuilderProvider moduleBuilderProvider)
        {
            _ModelExtractor = model;
            _moduleBuilderProvider = moduleBuilderProvider;
        }

        public IEnumerable<IPackageModuleIntent> Get()
        {
            var builders = _moduleBuilderProvider.Get().ToList();

            foreach (var moduleBuilder in builders) 
                moduleBuilder.Services.AddSingleton(_ModelExtractor);


            foreach (var moduleBuilder in builders)
            {
                foreach (var module in moduleBuilder.Build().Get())
                    yield return module;

            }
                
        }
    }
}

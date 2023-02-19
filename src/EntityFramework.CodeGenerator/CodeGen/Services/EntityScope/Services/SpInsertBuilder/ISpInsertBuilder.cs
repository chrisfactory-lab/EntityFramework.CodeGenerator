﻿using Microsoft.Extensions.DependencyInjection;

namespace EntityFramework.CodeGenerator
{
    public interface ISpInsertBuilder : ISqlGenActionBuilder
    {
    }

    internal class SpInsertBuilder : SqlGenActionBuilder, ISpInsertBuilder
    { 
        protected override void LoadDefaultServices()
        {
            base.LoadDefaultServices();
            this.Services.AddSingleton<ISpNameProvider, SpInsertNameProvider>();
        }

        public override ISqlGenActionProvider Build()
        {

            Services.AddSingleton<ISqlGenActionProvider, SpInsertSqlGenActionProvider>();

            var provider = Services.BuildServiceProvider();
            return provider.GetRequiredService<ISqlGenActionProvider>();
        }
    }



    internal class SpInsertSqlGenActionProvider : ISqlGenActionProvider
    {
        private readonly IServiceProvider _provider;
        private readonly IEntityTypeTable _entity;
        private readonly ISqlFileInfoFactory _fileInfoProvider;
        private readonly ISpNameProvider _spNameProvider;
        private readonly IDbContextModelExtractor _modelExtractor;
        public SpInsertSqlGenActionProvider(
            IServiceProvider provider,
            IEntityTypeTable entity,
            ISqlFileInfoFactory fileInfoProvider,
            ISpNameProvider spNameProvider,
            IDbContextModelExtractor modelExtractor)
        {
            _provider = provider;
            _entity = entity;
            _fileInfoProvider = fileInfoProvider;
            _spNameProvider = spNameProvider;
            _modelExtractor = modelExtractor;
        }
        public IEnumerable<ISqlGenAction> Get()
        {
            var spName = _spNameProvider.Get();

            var rootSqlPath = _modelExtractor.DefaultSqlTargetOutput.RootPath;
            var tablePattern = _modelExtractor.DefaultSqlTargetOutput.StoredProceduresPatternPath;
            var name = _entity.Table.Name;
            var schema = _entity.Table.Schema;

            var fi = _fileInfoProvider.CreateFileInfo(rootSqlPath, tablePattern, schema, name, spName);

            yield return _provider.GetRequiredService<ICreateFileActionBuilder>()
              .UseFileAction(new SpInsert(_entity, spName))
              .UseTargetFiles(fi)
              .Build();

        }
    }
}
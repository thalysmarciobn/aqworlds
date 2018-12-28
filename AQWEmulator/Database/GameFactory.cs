using AQWEmulator.Database.Maps;
using AQWEmulator.Xml.Game;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace AQWEmulator.Database
{
    public static class GameFactory
    {
        public static ISessionFactory Session { get; private set; }

        public static bool Build(ServerDatabase settings, bool install)
        {
            try
            {
                var config = Fluently.Configure().Database(
                        MySQLConfiguration.Standard.ConnectionString(c => c.Server(settings.Host)
                            .Database(settings.Database)
                            .Username(settings.Username)
                            .Password(settings.Password))
                    )
                    .Cache(c => c.UseQueryCache().UseSecondLevelCache().UseMinimalPuts())
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<AreaMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<AreaMonsterMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<MonsterMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AccessLogMap>())
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<AlertLogMap>())
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<ClassSkillMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<EnhancementMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<FactionMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<HairMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<ItemClassMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<ItemMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<PatternMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<ServerMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<AuraMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<AuraEffectMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<SkillMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<SkillAuraMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<CharacterItemMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<CharacterMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .Mappings(m =>
                        m.FluentMappings.AddFromAssemblyOf<ShopMap>().Conventions
                            .Add(DefaultCascade.All(), DefaultLazy.Never()))
                    .BuildConfiguration();
                if (install)
                {
                    var exporter = new SchemaExport(config);
                    exporter.Execute(true, true, false);
                }

                Session = config.BuildSessionFactory();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void Update(IModel model)
        {
            using (var session = Session.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(model);
                    transaction.Commit();
                }

                session.Dispose();
            }
        }

        public static void Save(IModel model)
        {
            using (var session = Session.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(model);
                    transaction.Commit();
                }

                session.Dispose();
            }
        }
    }
}
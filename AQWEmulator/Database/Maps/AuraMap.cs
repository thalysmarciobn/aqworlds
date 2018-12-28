using AQWEmulator.Database.Models;
 using FluentNHibernate.Mapping;
 
 namespace AQWEmulator.Database.Maps
 {
     public class AuraMap : ClassMap<AuraModel>
     {
         public AuraMap()
         {
             Table("hikari_auras");
             Id(x => x.Id).Column("id");
             Map(x => x.Name).Column("Name");
             Map(x => x.Duration).Column("Duration");
             Map(x => x.Category).Column("Category");
             Map(x => x.DamageIncrease).Column("Damage_Increanse");
             Map(x => x.DamageTakenDecrease).Column("Damage_Taken_Decreanse");
             HasMany(x => x.Effects).KeyColumn("Aura_ID");
         }
     }
 }
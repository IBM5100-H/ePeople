﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class EpeopleEntities : DbContext
    {
        public EpeopleEntities()
            : base("name=EpeopleEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Administrators> Administrators { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Parent> Parent { get; set; }
        public virtual DbSet<PerformanceMX> PerformanceMX { get; set; }
        public virtual DbSet<Picture> Picture { get; set; }
        public virtual DbSet<PictureType> PictureType { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<VoterMX> VoterMX { get; set; }
        public virtual DbSet<V_certificate> V_certificate { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Voter> Voter { get; set; }
    }
}

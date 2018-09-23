namespace Jwt.Repository.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialBuild : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Client",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ClientId = c.String(),
                    Name = c.String(),
                    Secret = c.String(),
                    CreatedBy = c.Long(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedBy = c.Long(),
                    UpdatedDate = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Profile",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProfileTypeId = c.Long(nullable: false),
                    ProfileStatusTypeId = c.Long(nullable: false),
                    Username = c.String(),
                    Email = c.String(),
                    FirstName = c.String(),
                    LastName = c.String(),
                    Salt = c.String(),
                    PasswordHash = c.String(),
                    FailedLoginCount = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedBy = c.Long(),
                    UpdatedDate = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Session",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ClientId = c.Long(nullable: false),
                    ProfileId = c.Long(nullable: false),
                    Token = c.String(),
                    ExpirationDate = c.DateTime(nullable: false),
                    IsForceExpired = c.Boolean(nullable: false),
                    CreatedBy = c.Long(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedBy = c.Long(),
                    UpdatedDate = c.DateTime(),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Session");
            DropTable("dbo.Profile");
            DropTable("dbo.Client");
        }
    }
}

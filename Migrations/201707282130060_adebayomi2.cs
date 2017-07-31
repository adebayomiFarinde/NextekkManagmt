namespace NextekkManagmt.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adebayomi2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserEmployees", "NumberOfChildren", c => c.Int(nullable: false));
            AddColumn("dbo.UserEmployees", "DateOfEmployment", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserEmployees", "PromotedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserEmployees", "AnnualSalary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.UserEmployees", "NumberOfChildrenID");
            DropColumn("dbo.UserEmployees", "Date_Of_Employment");
            DropColumn("dbo.UserEmployees", "Promoted_At");
            DropColumn("dbo.UserEmployees", "Annual_Salary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserEmployees", "Annual_Salary", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.UserEmployees", "Promoted_At", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserEmployees", "Date_Of_Employment", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserEmployees", "NumberOfChildrenID", c => c.Int(nullable: false));
            DropColumn("dbo.UserEmployees", "AnnualSalary");
            DropColumn("dbo.UserEmployees", "PromotedAt");
            DropColumn("dbo.UserEmployees", "DateOfEmployment");
            DropColumn("dbo.UserEmployees", "NumberOfChildren");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace RealEstate.Data.Models
{
    public partial class RealEstateContext : DbContext
    {
        public RealEstateContext()
        {
        }

        public RealEstateContext(DbContextOptions<RealEstateContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CancelledContract> CancelledContracts { get; set; }
        public virtual DbSet<CancelledContractBill> CancelledContractBills { get; set; }
        public virtual DbSet<CompanyBill> CompanyBills { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<ContractAccessoriesView> ContractAccessoriesViews { get; set; }
        public virtual DbSet<ContractDetail> ContractDetails { get; set; }
        public virtual DbSet<ContractDetailBill> ContractDetailBills { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DailyReport> DailyReports { get; set; }
        public virtual DbSet<Dblog> Dblogs { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeePenalty> EmployeePenalties { get; set; }
        public virtual DbSet<EmployeeReward> EmployeeRewards { get; set; }
        public virtual DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
        public virtual DbSet<FileContract> FileContracts { get; set; }
        public virtual DbSet<GoodsType> GoodsTypes { get; set; }
        public virtual DbSet<Mail> Mail { get; set; }
        public virtual DbSet<MaterialsAllocation> MaterialsAllocations { get; set; }
        public virtual DbSet<MaterialsPurchase> MaterialsPurchases { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<ProgramDetail> ProgramDetails { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectExpense> ProjectExpenses { get; set; }
        public virtual DbSet<ProjectUnit> ProjectUnits { get; set; }
        public virtual DbSet<ProjectUnitDescription> ProjectUnitDescriptions { get; set; }
        public virtual DbSet<ProjectVisit> ProjectVisits { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<SiteRep> SiteReps { get; set; }
        public virtual DbSet<Supervisor> Supervisors { get; set; }
        public virtual DbSet<SupervisorDetail> SupervisorDetails { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SupplierPayment> SupplierPayments { get; set; }
        public virtual DbSet<ViewCancelledContract> ViewCancelledContracts { get; set; }
        public virtual DbSet<ViewCustomerDatum> ViewCustomerData { get; set; }
        public virtual DbSet<ViewDailyReport> ViewDailyReports { get; set; }
        public virtual DbSet<ViewPayInstallment> ViewPayInstallments { get; set; }
        public virtual DbSet<ViewSupervisor> ViewSupervisors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=.;Database=RealEstate;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<CancelledContract>(entity =>
            {
                entity.ToTable("CancelledContract");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Customer).IsRequired();

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Project).IsRequired();
            });

            modelBuilder.Entity<CancelledContractBill>(entity =>
            {
                entity.ToTable("CancelledContractBill");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.CancelledContractBills)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_CancelledContractBill_CancelledContract");
            });

            modelBuilder.Entity<CompanyBill>(entity =>
            {
                entity.ToTable("CompanyBill");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.ToTable("Contract");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.NationalNumber).IsRequired();

                entity.Property(e => e.Phone).IsRequired();

                entity.Property(e => e.Program).IsRequired();

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ProjectUnitId).HasColumnName("ProjectUnitID");

                entity.HasOne(d => d.ProjectUnit)
                    .WithMany(p => p.Contracts)
                    .HasForeignKey(d => d.ProjectUnitId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Contract_ProjectUnit");
            });

            modelBuilder.Entity<ContractAccessoriesView>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ContractAccessoriesView");

                entity.Property(e => e.ContractDetailBillDate).HasColumnType("date");

                entity.Property(e => e.ContractDetailDate).HasColumnType("date");

                entity.Property(e => e.ContractDetailId).HasColumnName("ContractDetailID");

                entity.Property(e => e.ContractDetailName).IsRequired();

                entity.Property(e => e.ContractName).IsRequired();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NationalNumber).IsRequired();

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ProjectName).IsRequired();
            });

            modelBuilder.Entity<ContractDetail>(entity =>
            {
                entity.ToTable("ContractDetail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.ContractDetails)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_ContractDetail_Contract");
            });

            modelBuilder.Entity<ContractDetailBill>(entity =>
            {
                entity.ToTable("ContractDetailBill");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContractDetailId).HasColumnName("ContractDetailID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.ContractDetail)
                    .WithMany(p => p.ContractDetailBills)
                    .HasForeignKey(d => d.ContractDetailId)
                    .HasConstraintName("FK_ContractDetailBill_ContractDetail");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<DailyReport>(entity =>
            {
                entity.ToTable("DailyReport");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.DailyReports)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DailyReport_Employee");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.DailyReports)
                    .HasForeignKey(d => d.SupervisorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DailyReport_Supervisor");
            });

            modelBuilder.Entity<Dblog>(entity =>
            {
                entity.ToTable("DBLog");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Notes).IsRequired();

                entity.Property(e => e.Operation)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.WorkSince).HasColumnType("date");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_Employee_Department");
            });

            modelBuilder.Entity<EmployeePenalty>(entity =>
            {
                entity.ToTable("EmployeePenalty");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeSalaryId).HasColumnName("EmployeeSalaryID");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.EmployeeSalary)
                    .WithMany(p => p.EmployeePenalties)
                    .HasForeignKey(d => d.EmployeeSalaryId)
                    .HasConstraintName("FK_EmployeePenalty_EmployeeSalary");
            });

            modelBuilder.Entity<EmployeeReward>(entity =>
            {
                entity.ToTable("EmployeeReward");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EmployeeSalaryId).HasColumnName("EmployeeSalaryID");

                entity.Property(e => e.Name).IsRequired();

                entity.HasOne(d => d.EmployeeSalary)
                    .WithMany(p => p.EmployeeRewards)
                    .HasForeignKey(d => d.EmployeeSalaryId)
                    .HasConstraintName("FK_EmployeeRewards_EmployeeSalary");
            });

            modelBuilder.Entity<EmployeeSalary>(entity =>
            {
                entity.ToTable("EmployeeSalary");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeSalaries)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_EmployeeSalary_Employee");
            });

            modelBuilder.Entity<FileContract>(entity =>
            {
                entity.ToTable("FileContract");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.FileContracts)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_File_Contract");
            });

            modelBuilder.Entity<GoodsType>(entity =>
            {
                entity.ToTable("GoodsType");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Mail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.ReceiveDate).HasColumnType("date");

                entity.Property(e => e.SentDate).HasColumnType("date");
            });

            modelBuilder.Entity<MaterialsAllocation>(entity =>
            {
                entity.ToTable("MaterialsAllocation");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.PurchaseId).HasColumnName("PurchaseID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.MaterialsAllocations)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ItemAllocation_Project");

                entity.HasOne(d => d.Purchase)
                    .WithMany(p => p.MaterialsAllocations)
                    .HasForeignKey(d => d.PurchaseId)
                    .HasConstraintName("FK_ItemAllocation_ItemPurchase");
            });

            modelBuilder.Entity<MaterialsPurchase>(entity =>
            {
                entity.ToTable("MaterialsPurchase");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.Property(e => e.TotalPrice).HasComputedColumnSql("([Quantity]*[UnitPrice])", false);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.MaterialsPurchases)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_Purchase_Item");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.MaterialsPurchases)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_ItemPurchase_Dealer");
            });

            modelBuilder.Entity<Program>(entity =>
            {
                entity.ToTable("Program");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.ProjectUnitDescriptionId).HasColumnName("ProjectUnitDescriptionID");

                entity.HasOne(d => d.ProjectUnitDescription)
                    .WithMany(p => p.Programs)
                    .HasForeignKey(d => d.ProjectUnitDescriptionId)
                    .HasConstraintName("FK_Program_ProjectUnitDescription");
            });

            modelBuilder.Entity<ProgramDetail>(entity =>
            {
                entity.ToTable("ProgramDetail");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.ProgramId).HasColumnName("ProgramID");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.ProgramDetails)
                    .HasForeignKey(d => d.ProgramId)
                    .HasConstraintName("FK_ProgramDetail_Program");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ProjectExpense>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.ItemId).HasColumnName("ItemID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.SiteRepId).HasColumnName("SiteRepID");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.ProjectExpenses)
                    .HasForeignKey(d => d.ItemId)
                    .HasConstraintName("FK_ProjectExpenses_EnumItem");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectExpenses)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectExpenses_Project");

                entity.HasOne(d => d.SiteRep)
                    .WithMany(p => p.ProjectExpenses)
                    .HasForeignKey(d => d.SiteRepId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_ProjectExpenses_SiteRep");
            });

            modelBuilder.Entity<ProjectUnit>(entity =>
            {
                entity.ToTable("ProjectUnit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ProjectUnitDescriptionId).HasColumnName("ProjectUnitDescriptionID");

                entity.HasOne(d => d.ProjectUnitDescription)
                    .WithMany(p => p.ProjectUnits)
                    .HasForeignKey(d => d.ProjectUnitDescriptionId)
                    .HasConstraintName("FK_ProjectUnit_ProjectUnitDescription");
            });

            modelBuilder.Entity<ProjectUnitDescription>(entity =>
            {
                entity.ToTable("ProjectUnitDescription");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FlatId).HasColumnName("FlatID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectUnitDescriptions)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectUnitDescription_Project");
            });

            modelBuilder.Entity<ProjectVisit>(entity =>
            {
                entity.ToTable("ProjectVisit");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ProjectVisits)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_ProjectVisits_Customer");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectVisits)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_ProjectVisits_Project");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Question1)
                    .IsRequired()
                    .HasColumnName("Question");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_Questions_Customer");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK_Questions_Employee");
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_Customer");

                entity.HasOne(d => d.ProjectUnitDescription)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ProjectUnitDescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_ProjectUnitDescription");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Value).IsRequired();
            });

            modelBuilder.Entity<SiteRep>(entity =>
            {
                entity.ToTable("SiteRep");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Supervisor>(entity =>
            {
                entity.ToTable("Supervisor");
            });

            modelBuilder.Entity<SupervisorDetail>(entity =>
            {
                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.SupervisorDetails)
                    .HasForeignKey(d => d.SupervisorId)
                    .HasConstraintName("FK_SupervisorDetails_Supervisor");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("Supplier");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Speciality).IsRequired();
            });

            modelBuilder.Entity<SupplierPayment>(entity =>
            {
                entity.ToTable("SupplierPayment");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.SupplierPayments)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK_DealerPayment_Dealer1");
            });

            modelBuilder.Entity<ViewCancelledContract>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewCancelledContract");

                entity.Property(e => e.Customer).IsRequired();

                entity.Property(e => e.Date).HasMaxLength(4000);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(e => e.Project).IsRequired();
            });

            modelBuilder.Entity<ViewCustomerDatum>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewCustomerData");

                entity.Property(e => e.Date).HasMaxLength(4000);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.NationalNumber).IsRequired();

                entity.Property(e => e.Phone).IsRequired();

                entity.Property(e => e.Program).IsRequired();

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ProjectUnitId).HasColumnName("ProjectUnitID");

                entity.Property(e => e.Stock)
                    .IsRequired()
                    .HasMaxLength(11);
            });

            modelBuilder.Entity<ViewDailyReport>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewDailyReport");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EmployeeName).IsRequired();
            });

            modelBuilder.Entity<ViewPayInstallment>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewPayInstallments");

                entity.Property(e => e.ContractDetailBillDate).HasColumnType("date");

                entity.Property(e => e.ContractDetailDate).HasColumnType("date");

                entity.Property(e => e.ContractId).HasColumnName("ContractID");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<ViewSupervisor>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewSupervisor");

                entity.Property(e => e.Name).HasColumnName("NAME");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Battery> Batteries { get; set; }

    public virtual DbSet<BatterySwitchStation> BatterySwitchStations { get; set; }

    public virtual DbSet<BatteryType> BatteryTypes { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeSwitchStation> EmployeeSwitchStations { get; set; }

    public virtual DbSet<Kpi> Kpis { get; set; }

    public virtual DbSet<MaintenanceItem> MaintenanceItems { get; set; }

    public virtual DbSet<MaintenanceItemEmployee> MaintenanceItemEmployees { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<SwitchLog> SwitchLogs { get; set; }

    public virtual DbSet<SwitchRequest> SwitchRequests { get; set; }

    public virtual DbSet<SwitchRequestEmployee> SwitchRequestEmployees { get; set; }

    public virtual DbSet<SwitchStation> SwitchStations { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    public virtual DbSet<VehicleOwner> VehicleOwners { get; set; }

    public virtual DbSet<VehicleParam> VehicleParams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=123.57.140.176)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User ID=C##CAR;password=Tj123456");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##CAR")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Battery>(entity =>
        {
            entity.HasKey(e => e.BatteryId).HasName("SYS_C008811");

            entity.ToTable("BATTERY");

            entity.Property(e => e.BatteryId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'bt_tl1_num0.0.0' ")
                .HasColumnName("BATTERY_ID");
            entity.Property(e => e.AvailableStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'y' ")
                .IsFixedLength()
                .HasColumnName("AVAILABLE_STATUS");
            entity.Property(e => e.BatteryTypeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'tp0.00.0' ")
                .HasColumnName("BATTERY_TYPE_ID");
            entity.Property(e => e.CurrChargeTimes)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CURR_CHARGE_TIMES");
            entity.Property(e => e.CurrentCapacity)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasDefaultValueSql("'0' ")
                .HasColumnName("CURRENT_CAPACITY");
            entity.Property(e => e.ManufacturingDate)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("MANUFACTURING_DATE");

            entity.HasOne(d => d.BatteryType).WithMany(p => p.Batteries)
                .HasForeignKey(d => d.BatteryTypeId)
                .HasConstraintName("FK_BATTERY_TYPE");
        });

        modelBuilder.Entity<BatterySwitchStation>(entity =>
        {
            entity.HasKey(e => e.BatteryId).HasName("SYS_C008835");

            entity.ToTable("BATTERY_SWITCH_STATION");

            entity.Property(e => e.BatteryId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'bt_tl1_num0.0.0' ")
                .HasColumnName("BATTERY_ID");
            entity.Property(e => e.StationId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'st0' ")
                .HasColumnName("STATION_ID");

            entity.HasOne(d => d.Battery).WithOne(p => p.BatterySwitchStation)
                .HasForeignKey<BatterySwitchStation>(d => d.BatteryId)
                .HasConstraintName("FK_BATTERY_BATTERY_SWITCH_STATION");

            entity.HasOne(d => d.Station).WithMany(p => p.BatterySwitchStations)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK_BATTERY_SWITCH_STATION_SWITCH_STATION");
        });

        modelBuilder.Entity<BatteryType>(entity =>
        {
            entity.HasKey(e => e.BatteryTypeId).HasName("SYS_C008805");

            entity.ToTable("BATTERY_TYPE");

            entity.Property(e => e.BatteryTypeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'tp0.00.0' ")
                .HasColumnName("BATTERY_TYPE_ID");
            entity.Property(e => e.MaxChargeTiems)
                .HasDefaultValueSql("1000 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MAX_CHARGE_TIEMS");
            entity.Property(e => e.TotalCapacity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'98.67kwh' ")
                .HasColumnName("TOTAL_CAPACITY");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("SYS_C008830");

            entity.ToTable("EMPLOYEE");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.CreateTime)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("CREATE_TIME");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'m' ")
                .IsFixedLength()
                .HasColumnName("GENDER");
            entity.Property(e => e.IdentityNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'1xxxxxxxxxxxxxxxxx' ")
                .HasColumnName("IDENTITY_NUMBER");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'����' ")
                .HasColumnName("NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'123456' ")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'+86'")
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.Positions)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'_'")
                .HasColumnName("POSITIONS");
            entity.Property(e => e.ProfilePhoto)
                .HasColumnType("BLOB")
                .HasColumnName("PROFILE_PHOTO");
            entity.Property(e => e.Salary)
                .HasColumnType("NUMBER")
                .HasColumnName("SALARY");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'ep_0.00' ")
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<EmployeeSwitchStation>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("SYS_C008855");

            entity.ToTable("EMPLOYEE_SWITCH_STATION");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.StationId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'st0' ")
                .HasColumnName("STATION_ID");

            entity.HasOne(d => d.Employee).WithOne(p => p.EmployeeSwitchStation)
                .HasForeignKey<EmployeeSwitchStation>(d => d.EmployeeId)
                .HasConstraintName("FK_EMPLOYEE_SWITCH_STATION_EMPLOYEE");

            entity.HasOne(d => d.Station).WithMany(p => p.EmployeeSwitchStations)
                .HasForeignKey(d => d.StationId)
                .HasConstraintName("FK_EMPLOYEE_SWITCH_STATION_SWITCH_STATION");
        });

        modelBuilder.Entity<Kpi>(entity =>
        {
            entity.HasKey(e => e.KpiId).HasName("SYS_C008840");

            entity.ToTable("KPI");

            entity.Property(e => e.KpiId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'pf. no. 0__0' ")
                .HasColumnName("KPI_ID");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.Score)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SCORE");
            entity.Property(e => e.ServiceFrequency)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SERVICE_FREQUENCY");
            entity.Property(e => e.TotalPerformance)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TOTAL_PERFORMANCE");

            entity.HasOne(d => d.Employee).WithMany(p => p.Kpis)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_KPI_EMPLOYEE");
        });

        modelBuilder.Entity<MaintenanceItem>(entity =>
        {
            entity.HasKey(e => e.MaintenanceItemId).HasName("SYS_C008851");

            entity.ToTable("MAINTENANCE_ITEM");

            entity.HasIndex(e => e.MaintenanceLocation, "SYS_C008852").IsUnique();

            entity.Property(e => e.MaintenanceItemId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'mti_ep_0x548151' ")
                .HasColumnName("MAINTENANCE_ITEM_ID");
            entity.Property(e => e.Evaluations)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EVALUATIONS");
            entity.Property(e => e.MaintenanceLocation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("MAINTENANCE_LOCATION");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'0'")
                .IsFixedLength()
                .HasColumnName("ORDER_STATUS");
            entity.Property(e => e.OrderSubmissionTime)
                .HasPrecision(6)
                .HasColumnName("ORDER_SUBMISSION_TIME");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.ServiceTime)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("SERVICE_TIME");
            entity.Property(e => e.VehicleId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'vc. lc.0' ")
                .HasColumnName("VEHICLE_ID");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.MaintenanceItems)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK_MAINTENANCE_ITEM_VEHICLE");
        });

        modelBuilder.Entity<MaintenanceItemEmployee>(entity =>
        {
            entity.HasKey(e => e.MaintenanceItemId).HasName("SYS_C008859");

            entity.ToTable("MAINTENANCE_ITEM_EMPLOYEE");

            entity.Property(e => e.MaintenanceItemId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'mti_ep_0x548151' ")
                .HasColumnName("MAINTENANCE_ITEM_ID");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("EMPLOYEE_ID");

            entity.HasOne(d => d.Employee).WithMany(p => p.MaintenanceItemEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_MAINTENANCE_ITEM_EMPLOYEE_EMPLOYEE");

            entity.HasOne(d => d.MaintenanceItem).WithOne(p => p.MaintenanceItemEmployee)
                .HasForeignKey<MaintenanceItemEmployee>(d => d.MaintenanceItemId)
                .HasConstraintName("FK_MAINTENANCE_ITEM_EMPLOYEE_MAINTENANCE_ITEM");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.AnnouncementId).HasName("SYS_C008833");

            entity.ToTable("NEWS");

            entity.Property(e => e.AnnouncementId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'news. 0' ")
                .HasColumnName("ANNOUNCEMENT_ID");
            entity.Property(e => e.Contents)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONTENTS");
            entity.Property(e => e.Likes)
                .HasDefaultValueSql("0")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("LIKES");
            entity.Property(e => e.PublishPos)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PUBLISH_POS");
            entity.Property(e => e.PublishTime)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("PUBLISH_TIME");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'money talks.' ")
                .HasColumnName("TITLE");
            entity.Property(e => e.ViewCount)
                .HasDefaultValueSql("0\n")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("VIEW_COUNT");
        });

        modelBuilder.Entity<SwitchLog>(entity =>
        {
            entity.HasKey(e => e.SwitchServiceId).HasName("SYS_C008872");

            entity.ToTable("SWITCH_LOG");

            entity.HasIndex(e => e.Longtitude, "SYS_C008873").IsUnique();

            entity.HasIndex(e => e.Latitude, "SYS_C008874").IsUnique();

            entity.Property(e => e.SwitchServiceId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'sw_0x_017' ")
                .HasColumnName("SWITCH_SERVICE_ID");
            entity.Property(e => e.BatteryIdOff)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'bt_tl1_num0.0.0' ")
                .HasColumnName("BATTERY_ID_OFF");
            entity.Property(e => e.BatteryIdOn)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'bt_tl1_num0.0.1' ")
                .HasColumnName("BATTERY_ID_ON");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("EMPLOYEE_ID");
            entity.Property(e => e.Evaluations)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EVALUATIONS");
            entity.Property(e => e.Latitude)
                .HasDefaultValueSql("90.00 ")
                .HasColumnType("NUMBER")
                .HasColumnName("LATITUDE");
            entity.Property(e => e.Longtitude)
                .HasDefaultValueSql("90.00 ")
                .HasColumnType("NUMBER")
                .HasColumnName("LONGTITUDE");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("POSITION");
            entity.Property(e => e.SwitchTime)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("SWITCH_TIME");
            entity.Property(e => e.VehicleId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'vc. lc.0' ")
                .HasColumnName("VEHICLE_ID");

            entity.HasOne(d => d.BatteryIdOffNavigation).WithMany(p => p.SwitchLogBatteryIdOffNavigations)
                .HasForeignKey(d => d.BatteryIdOff)
                .HasConstraintName("FK_SWITCH_LOG_BATTERY_OUT");

            entity.HasOne(d => d.BatteryIdOnNavigation).WithMany(p => p.SwitchLogBatteryIdOnNavigations)
                .HasForeignKey(d => d.BatteryIdOn)
                .HasConstraintName("FK_SWITCH_LOG_BATTERY_IN");

            entity.HasOne(d => d.Employee).WithMany(p => p.SwitchLogs)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_SWITCH_LOG_EMPLOYEE");

            entity.HasOne(d => d.LatitudeNavigation).WithOne(p => p.SwitchLogLatitudeNavigation)
                .HasPrincipalKey<SwitchRequest>(p => p.Latitude)
                .HasForeignKey<SwitchLog>(d => d.Latitude)
                .HasConstraintName("FK_SWITCH_LOG_SWITCH_REQUEST_LAT");

            entity.HasOne(d => d.LongtitudeNavigation).WithOne(p => p.SwitchLogLongtitudeNavigation)
                .HasPrincipalKey<SwitchRequest>(p => p.Longtitude)
                .HasForeignKey<SwitchLog>(d => d.Longtitude)
                .HasConstraintName("FK_SWITCH_LOG_SWITCH_REQUEST_LON");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.SwitchLogs)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("FK_SWITCH_LOG_VEHICLE");
        });

        modelBuilder.Entity<SwitchRequest>(entity =>
        {
            entity.HasKey(e => e.SwitchRequestId).HasName("SYS_C008772");

            entity.ToTable("SWITCH_REQUEST");

            entity.HasIndex(e => e.Longtitude, "SYS_C008773").IsUnique();

            entity.HasIndex(e => e.Latitude, "SYS_C008774").IsUnique();

            entity.Property(e => e.SwitchRequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'sr_0x_1207xw1' ")
                .HasColumnName("SWITCH_REQUEST_ID");
            entity.Property(e => e.Latitude)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("90.00 ")
                .HasColumnType("NUMBER")
                .HasColumnName("LATITUDE");
            entity.Property(e => e.Longtitude)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("90.00 ")
                .HasColumnType("NUMBER")
                .HasColumnName("LONGTITUDE");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("POSITION");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("REMARKS");
            entity.Property(e => e.RequestTime)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("REQUEST_TIME");
            entity.Property(e => e.SwitchType)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'l' ")
                .IsFixedLength()
                .HasColumnName("SWITCH_TYPE");
            entity.Property(e => e.VehicleId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'vc. lc.0' ")
                .HasColumnName("VEHICLE_ID");
        });

        modelBuilder.Entity<SwitchRequestEmployee>(entity =>
        {
            entity.HasKey(e => e.SwitchRequestId).HasName("SYS_C008863");

            entity.ToTable("SWITCH_REQUEST_EMPLOYEE");

            entity.Property(e => e.SwitchRequestId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'sr_0x_1207xw1' ")
                .HasColumnName("SWITCH_REQUEST_ID");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("EMPLOYEE_ID");

            entity.HasOne(d => d.Employee).WithMany(p => p.SwitchRequestEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_SWITCH_REQUEST_EMPLOYEE_EMPLOYEE");
        });

        modelBuilder.Entity<SwitchStation>(entity =>
        {
            entity.HasKey(e => e.StationId).HasName("SYS_C008800");

            entity.ToTable("SWITCH_STATION");

            entity.HasIndex(e => e.Longtitude, "SYS_C008801").IsUnique();

            entity.HasIndex(e => e.Latitude, "SYS_C008802").IsUnique();

            entity.Property(e => e.StationId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'st0' ")
                .HasColumnName("STATION_ID");
            entity.Property(e => e.AvailableBatteryCount)
                .HasDefaultValueSql("0\n")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("AVAILABLE_BATTERY_COUNT");
            entity.Property(e => e.BatteryCapacity)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("BATTERY_CAPACITY");
            entity.Property(e => e.ElectricityFee)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("ELECTRICITY_FEE");
            entity.Property(e => e.FaliureStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'n' ")
                .IsFixedLength()
                .HasColumnName("FALIURE_STATUS");
            entity.Property(e => e.Latitude)
                .HasDefaultValueSql("90.00 ")
                .HasColumnType("NUMBER")
                .HasColumnName("LATITUDE");
            entity.Property(e => e.Longtitude)
                .HasDefaultValueSql("90.00 ")
                .HasColumnType("NUMBER")
                .HasColumnName("LONGTITUDE");
            entity.Property(e => e.QueueLength)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER(38)")
                .HasColumnName("QUEUE_LENGTH");
            entity.Property(e => e.ServiceFee)
                .HasDefaultValueSql("0 ")
                .HasColumnType("NUMBER")
                .HasColumnName("SERVICE_FEE");
            entity.Property(e => e.StationName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STATION_NAME");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("SYS_C008845");

            entity.ToTable("VEHICLE");

            entity.Property(e => e.VehicleId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'vc. lc.0' ")
                .HasColumnName("VEHICLE_ID");
            entity.Property(e => e.BatteryId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("null")
                .HasColumnName("BATTERY_ID");
            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("OWNER_ID");
            entity.Property(e => e.PurchaseDate)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("PURCHASE_DATE");
            entity.Property(e => e.VehicleModel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'fu_wl_lp1_num0.0.0' ")
                .HasColumnName("VEHICLE_MODEL");

            entity.HasOne(d => d.Battery).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.BatteryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_VEHICLE_BATTERY");

            entity.HasOne(d => d.Owner).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.OwnerId)
                .HasConstraintName("FK_VEHICLE_OWNER");

            entity.HasOne(d => d.VehicleModelNavigation).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.VehicleModel)
                .HasConstraintName("FK_VEHICLE_VEHICLE_PARAM");
        });

        modelBuilder.Entity<VehicleOwner>(entity =>
        {
            entity.HasKey(e => e.OwnerId).HasName("SYS_C008823");

            entity.ToTable("VEHICLE_OWNER");

            entity.Property(e => e.OwnerId)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'no. 0' ")
                .HasColumnName("OWNER_ID");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Birthday)
                .HasPrecision(6)
                .HasColumnName("BIRTHDAY");
            entity.Property(e => e.CreateTime)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("CREATE_TIME");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'wl@car.com' ")
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("'m' ")
                .IsFixedLength()
                .HasColumnName("GENDER");
            entity.Property(e => e.Nickname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'user_0.00' ")
                .HasColumnName("NICKNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'123456' ")
                .HasColumnName("PASSWORD");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'+86'")
                .HasColumnName("PHONE_NUMBER");
            entity.Property(e => e.ProfilePhoto)
                .HasColumnType("BLOB")
                .HasColumnName("PROFILE_PHOTO");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'����' ")
                .HasColumnName("USERNAME");
        });

        modelBuilder.Entity<VehicleParam>(entity =>
        {
            entity.HasKey(e => e.VehicleModel).HasName("SYS_C008816");

            entity.ToTable("VEHICLE_PARAM");

            entity.Property(e => e.VehicleModel)
                .HasMaxLength(50)
                .IsUnicode(false)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("'fu_wl_lp1_num0.0.0' ")
                .HasColumnName("VEHICLE_MODEL");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'nio inc.' ")
                .HasColumnName("MANUFACTURER");
            entity.Property(e => e.MaxSpeed)
                .HasColumnType("NUMBER")
                .HasColumnName("MAX_SPEED");
            entity.Property(e => e.ServiceTerm)
                .HasPrecision(6)
                .HasDefaultValueSql("systimestamp ")
                .HasColumnName("SERVICE_TERM");
            entity.Property(e => e.Transmission)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'gt0.00' ")
                .HasColumnName("TRANSMISSION");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

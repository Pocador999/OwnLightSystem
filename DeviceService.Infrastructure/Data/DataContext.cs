using DeviceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceAction> DeviceActions { get; set; }
    public DbSet<DeviceType> DeviceTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Device>()
            .HasOne<DeviceType>()
            .WithMany(dt => dt.Devices)
            .HasForeignKey(d => d.DeviceTypeId);
    }
}

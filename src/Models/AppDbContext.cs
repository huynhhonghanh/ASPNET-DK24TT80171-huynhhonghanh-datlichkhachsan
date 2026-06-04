using Microsoft.EntityFrameworkCore;

namespace DatPhongKhachSanVungTau.Models;

/// <summary>
/// DbContext chính — quản lý 5 bảng: NguoiDung, KhachSan, LoaiPhong, Phong, DatPhong.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<NguoiDung> NguoiDung { get; set; }
    public DbSet<KhachSan> KhachSan { get; set; }
    public DbSet<LoaiPhong> LoaiPhong { get; set; }
    public DbSet<Phong> Phong { get; set; }
    public DbSet<DatPhong> DatPhong { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- NguoiDung ---
        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasIndex(e => e.TenDangNhap).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.VaiTro).HasDefaultValue(HangSo.VaiTroKhachHang);
            entity.Property(e => e.TrangThaiTaiKhoan).HasDefaultValue(HangSo.TrangThaiTaiKhoanHoatDong);
        });

        // --- KhachSan ---
        modelBuilder.Entity<KhachSan>(entity =>
        {
            entity.HasIndex(e => e.KhuVuc);
        });

        // --- LoaiPhong ---
        modelBuilder.Entity<LoaiPhong>(entity =>
        {
            entity.HasIndex(e => e.TenLoaiPhong);
        });

        // --- Phong ---
        modelBuilder.Entity<Phong>(entity =>
        {
            entity.Property(e => e.GiaMotDem).HasColumnType("decimal(18,0)");

            // Số phòng duy nhất trong cùng khách sạn
            entity.HasIndex(e => new { e.MaKhachSan, e.SoPhong }).IsUnique();

            entity.HasOne(e => e.KhachSan)
                .WithMany(k => k.DanhSachPhong)
                .HasForeignKey(e => e.MaKhachSan)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.LoaiPhong)
                .WithMany(l => l.DanhSachPhong)
                .HasForeignKey(e => e.MaLoaiPhong)
                .OnDelete(DeleteBehavior.Restrict);

            entity.Property(e => e.TrangThai).HasDefaultValue(HangSo.TrangThaiPhongConTrong);
        });

        // --- DatPhong ---
        modelBuilder.Entity<DatPhong>(entity =>
        {
            entity.Property(e => e.TongTien).HasColumnType("decimal(18,0)");
            entity.Property(e => e.NgayDat).HasDefaultValueSql("GETDATE()");

            entity.HasOne(e => e.NguoiDung)
                .WithMany(n => n.DanhSachDatPhong)
                .HasForeignKey(e => e.MaNguoiDung)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Phong)
                .WithMany(p => p.DanhSachDatPhong)
                .HasForeignKey(e => e.MaPhong)
                .OnDelete(DeleteBehavior.Restrict);

            // Index hỗ trợ truy vấn kiểm tra phòng trống theo ngày
            entity.HasIndex(e => new { e.MaPhong, e.NgayNhanPhong, e.NgayTraPhong });
            entity.HasIndex(e => e.TrangThai);
        });
    }
}

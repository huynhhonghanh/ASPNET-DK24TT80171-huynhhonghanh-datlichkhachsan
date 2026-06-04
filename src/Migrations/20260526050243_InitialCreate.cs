using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatPhongKhachSanVungTau.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KhachSan",
                columns: table => new
                {
                    MaKhachSan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenKhachSan = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    KhuVuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DiaChiChiTiet = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    XepHang = table.Column<int>(type: "int", nullable: false),
                    KhoangCachBien = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachSan", x => x.MaKhachSan);
                });

            migrationBuilder.CreateTable(
                name: "LoaiPhong",
                columns: table => new
                {
                    MaLoaiPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenLoaiPhong = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    SoNguoiToiDa = table.Column<int>(type: "int", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiPhong", x => x.MaLoaiPhong);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "KhachHang"),
                    TrangThaiTaiKhoan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "HoatDong")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.MaNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "Phong",
                columns: table => new
                {
                    MaPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoPhong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MaKhachSan = table.Column<int>(type: "int", nullable: false),
                    MaLoaiPhong = table.Column<int>(type: "int", nullable: false),
                    GiaMotDem = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    HuongPhong = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TienNghi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "Còn trống")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phong", x => x.MaPhong);
                    table.ForeignKey(
                        name: "FK_Phong_KhachSan_MaKhachSan",
                        column: x => x.MaKhachSan,
                        principalTable: "KhachSan",
                        principalColumn: "MaKhachSan",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Phong_LoaiPhong_MaLoaiPhong",
                        column: x => x.MaLoaiPhong,
                        principalTable: "LoaiPhong",
                        principalColumn: "MaLoaiPhong",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DatPhong",
                columns: table => new
                {
                    MaDatPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<int>(type: "int", nullable: false),
                    MaPhong = table.Column<int>(type: "int", nullable: false),
                    NgayNhanPhong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTraPhong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoDem = table.Column<int>(type: "int", nullable: false),
                    SoKhach = table.Column<int>(type: "int", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhuongThucThanhToan = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatPhong", x => x.MaDatPhong);
                    table.ForeignKey(
                        name: "FK_DatPhong_NguoiDung_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "MaNguoiDung",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DatPhong_Phong_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "Phong",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_MaNguoiDung",
                table: "DatPhong",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_MaPhong_NgayNhanPhong_NgayTraPhong",
                table: "DatPhong",
                columns: new[] { "MaPhong", "NgayNhanPhong", "NgayTraPhong" });

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_TrangThai",
                table: "DatPhong",
                column: "TrangThai");

            migrationBuilder.CreateIndex(
                name: "IX_KhachSan_KhuVuc",
                table: "KhachSan",
                column: "KhuVuc");

            migrationBuilder.CreateIndex(
                name: "IX_LoaiPhong_TenLoaiPhong",
                table: "LoaiPhong",
                column: "TenLoaiPhong");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_Email",
                table: "NguoiDung",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDung_TenDangNhap",
                table: "NguoiDung",
                column: "TenDangNhap",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phong_MaKhachSan_SoPhong",
                table: "Phong",
                columns: new[] { "MaKhachSan", "SoPhong" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Phong_MaLoaiPhong",
                table: "Phong",
                column: "MaLoaiPhong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatPhong");

            migrationBuilder.DropTable(
                name: "NguoiDung");

            migrationBuilder.DropTable(
                name: "Phong");

            migrationBuilder.DropTable(
                name: "KhachSan");

            migrationBuilder.DropTable(
                name: "LoaiPhong");
        }
    }
}

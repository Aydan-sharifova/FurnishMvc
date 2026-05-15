using FurnishMvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FurnishMvc.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.26")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("FurnishMvc.Models.Category", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer");

            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

            b.Property<string>("Icon")
                .HasColumnType("text");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("Slug")
                .IsRequired()
                .HasColumnType("text");

            b.HasKey("Id");

            b.ToTable("Categories");
        });

        modelBuilder.Entity("FurnishMvc.Models.Product", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer");

            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

            b.Property<int>("CategoryId")
                .HasColumnType("integer");

            b.Property<string>("Description")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("ImageUrl")
                .IsRequired()
                .HasColumnType("text");

            b.Property<bool>("IsFeatured")
                .HasColumnType("boolean");

            b.Property<string>("Name")
                .IsRequired()
                .HasColumnType("text");

            b.Property<decimal>("OldPrice")
                .HasPrecision(18, 2)
                .HasColumnType("numeric(18,2)");

            b.Property<decimal>("Price")
                .HasPrecision(18, 2)
                .HasColumnType("numeric(18,2)");

            b.Property<int>("StockCount")
                .HasColumnType("integer");

            b.HasKey("Id");

            b.HasIndex("CategoryId");

            b.ToTable("Products");
        });

        modelBuilder.Entity("FurnishMvc.Models.Slider", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("integer");

            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

            b.Property<string>("ButtonLink")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("ButtonText")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("Description")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("DiscountText")
                .IsRequired()
                .HasColumnType("text");

            b.Property<string>("ImageUrl")
                .IsRequired()
                .HasColumnType("text");

            b.Property<bool>("IsActive")
                .HasColumnType("boolean");

            b.Property<int>("Order")
                .HasColumnType("integer");

            b.Property<decimal>("Price")
                .HasPrecision(18, 2)
                .HasColumnType("numeric(18,2)");

            b.Property<string>("Title")
                .IsRequired()
                .HasColumnType("text");

            b.HasKey("Id");

            b.ToTable("Sliders");
        });

        modelBuilder.Entity("FurnishMvc.Models.Product", b =>
        {
            b.HasOne("FurnishMvc.Models.Category", "Category")
                .WithMany("Products")
                .HasForeignKey("CategoryId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Category");
        });

        modelBuilder.Entity("FurnishMvc.Models.Category", b =>
        {
            b.Navigation("Products");
        });
#pragma warning restore 612, 618
    }
}

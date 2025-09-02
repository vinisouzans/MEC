using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MEC.Migrations
{
    /// <inheritdoc />
    public partial class CriarProjetos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NomeCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelefoneCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnderecoEntrega = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataAssinatura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataPagamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValorMateriais = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorMaoObra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ValorFrete = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MargemLucro = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItensProjeto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjetoId = table.Column<int>(type: "int", nullable: false),
                    ProdutoId = table.Column<int>(type: "int", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    MetrosLineares = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItensProjeto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItensProjeto_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItensProjeto_Projetos_ProjetoId",
                        column: x => x.ProjetoId,
                        principalTable: "Projetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItensProjeto_ProdutoId",
                table: "ItensProjeto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItensProjeto_ProjetoId",
                table: "ItensProjeto",
                column: "ProjetoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItensProjeto");

            migrationBuilder.DropTable(
                name: "Projetos");
        }
    }
}

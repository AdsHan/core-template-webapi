using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace EntregaFutura.Repository.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Imagens",
                columns: table => new
                {
                    ImagemId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ImagemUrl = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagens", x => x.ImagemId);
                });

            migrationBuilder.CreateTable(
                name: "Observacoes",
                columns: table => new
                {
                    ObservacaoId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Observacao = table.Column<string>(maxLength: 65000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observacoes", x => x.ObservacaoId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Entregas",
                columns: table => new
                {
                    EntregaId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendedorId = table.Column<string>(nullable: false),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 90, nullable: false),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    DataInclusao = table.Column<DateTime>(nullable: false),
                    DataAbertura = table.Column<DateTime>(nullable: false),
                    DataEncerramento = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entregas", x => x.EntregaId);
                    table.ForeignKey(
                        name: "FK_Entregas_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ListasPreco",
                columns: table => new
                {
                    ListaPrecoId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendedorId = table.Column<string>(nullable: false),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Referencia = table.Column<string>(maxLength: 30, nullable: false),
                    Descricao = table.Column<string>(maxLength: 90, nullable: false),
                    DataInclusao = table.Column<DateTime>(nullable: false),
                    DataValidade = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsAtivo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListasPreco", x => x.ListaPrecoId);
                    table.ForeignKey(
                        name: "FK_ListasPreco_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    VendedorId = table.Column<string>(nullable: true),
                    ListaPrecoPadraoId = table.Column<int>(nullable: true),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Descricao = table.Column<string>(maxLength: 90, nullable: false),
                    Contato = table.Column<string>(maxLength: 30, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsAtivo = table.Column<bool>(nullable: false),
                    IsContratoAtivo = table.Column<bool>(nullable: false),
                    ImagemUrl = table.Column<string>(nullable: true),
                    DataUltimoAcesso = table.Column<DateTime>(nullable: false),
                    DataInclusao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_ListasPreco_ListaPrecoPadraoId",
                        column: x => x.ListaPrecoPadraoId,
                        principalTable: "ListasPreco",
                        principalColumn: "ListaPrecoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetUsers_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    PedidoId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendedorId = table.Column<string>(nullable: false),
                    ClienteId = table.Column<string>(nullable: false),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    DataInclusao = table.Column<DateTime>(nullable: false),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    ValorMercadoria = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.PedidoId);
                    table.ForeignKey(
                        name: "FK_Pedidos_AspNetUsers_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pedidos_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_AspNetUsers_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosGrupo",
                columns: table => new
                {
                    ProdutoGrupoId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendedorId = table.Column<string>(nullable: false),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Referencia = table.Column<string>(maxLength: 30, nullable: true),
                    Descricao = table.Column<string>(maxLength: 90, nullable: false),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    DataInclusao = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosGrupo", x => x.ProdutoGrupoId);
                    table.ForeignKey(
                        name: "FK_ProdutosGrupo_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProdutosGrupo_AspNetUsers_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VendedorId = table.Column<string>(nullable: false),
                    ProdutoGrupoId = table.Column<int>(nullable: false),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Referencia = table.Column<string>(maxLength: 30, nullable: true),
                    Descricao = table.Column<string>(maxLength: 90, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    QuantidadeMinima = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    DataInclusao = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsAtivo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.ProdutoId);
                    table.ForeignKey(
                        name: "FK_Produtos_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produtos_ProdutosGrupo_ProdutoGrupoId",
                        column: x => x.ProdutoGrupoId,
                        principalTable: "ProdutosGrupo",
                        principalColumn: "ProdutoGrupoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Produtos_AspNetUsers_VendedorId",
                        column: x => x.VendedorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListasPrecoProduto",
                columns: table => new
                {
                    ListaPrecoProdutoId = table.Column<int>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: false),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Descricao = table.Column<string>(maxLength: 90, nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    QuantidadeMinima = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    ListaPrecoModelListaPrecoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListasPrecoProduto", x => new { x.ListaPrecoProdutoId, x.ProdutoId });
                    table.ForeignKey(
                        name: "FK_ListasPrecoProduto_ListasPreco_ListaPrecoModelListaPrecoId",
                        column: x => x.ListaPrecoModelListaPrecoId,
                        principalTable: "ListasPreco",
                        principalColumn: "ListaPrecoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListasPrecoProduto_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ListasPrecoProduto_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PedidosItem",
                columns: table => new
                {
                    PedidoId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false),
                    ProdutoId = table.Column<int>(nullable: false),
                    ObservacaoId = table.Column<int>(nullable: true),
                    Quantidade = table.Column<decimal>(type: "decimal(8,3)", nullable: false),
                    ValorUnitario = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    ValorMercadoria = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosItem", x => new { x.PedidoId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_PedidosItem_Observacoes_ObservacaoId",
                        column: x => x.ObservacaoId,
                        principalTable: "Observacoes",
                        principalColumn: "ObservacaoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidosItem_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "PedidoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidosItem_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdutosImagem",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(nullable: false),
                    ImagemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutosImagem", x => new { x.ProdutoId, x.ImagemId });
                    table.ForeignKey(
                        name: "FK_ProdutosImagem_Imagens_ImagemId",
                        column: x => x.ImagemId,
                        principalTable: "Imagens",
                        principalColumn: "ImagemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutosImagem_Produtos_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produtos",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ListaPrecoPadraoId",
                table: "AspNetUsers",
                column: "ListaPrecoPadraoId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ObservacaoId",
                table: "AspNetUsers",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VendedorId",
                table: "AspNetUsers",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_ObservacaoId",
                table: "Entregas",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Entregas_VendedorId",
                table: "Entregas",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ListasPreco_ObservacaoId",
                table: "ListasPreco",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListasPreco_VendedorId",
                table: "ListasPreco",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ListasPrecoProduto_ListaPrecoModelListaPrecoId",
                table: "ListasPrecoProduto",
                column: "ListaPrecoModelListaPrecoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListasPrecoProduto_ObservacaoId",
                table: "ListasPrecoProduto",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ListasPrecoProduto_ProdutoId",
                table: "ListasPrecoProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ObservacaoId",
                table: "Pedidos",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_VendedorId",
                table: "Pedidos",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosItem_ObservacaoId",
                table: "PedidosItem",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosItem_ProdutoId",
                table: "PedidosItem",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_ObservacaoId",
                table: "Produtos",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_ProdutoGrupoId",
                table: "Produtos",
                column: "ProdutoGrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_VendedorId",
                table: "Produtos",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosGrupo_ObservacaoId",
                table: "ProdutosGrupo",
                column: "ObservacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosGrupo_VendedorId",
                table: "ProdutosGrupo",
                column: "VendedorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutosImagem_ImagemId",
                table: "ProdutosImagem",
                column: "ImagemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Entregas_AspNetUsers_VendedorId",
                table: "Entregas",
                column: "VendedorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListasPreco_AspNetUsers_VendedorId",
                table: "ListasPreco",
                column: "VendedorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql("Insert into AspNetRoles(Id, Name) Values(1, 'Admin')");
            migrationBuilder.Sql("Insert into AspNetRoles(Id, Name) Values(2, 'Vendedor')");
            migrationBuilder.Sql("Insert into AspNetRoles(Id, Name) Values(3, 'Cliente')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListasPreco_AspNetUsers_VendedorId",
                table: "ListasPreco");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Entregas");

            migrationBuilder.DropTable(
                name: "ListasPrecoProduto");

            migrationBuilder.DropTable(
                name: "PedidosItem");

            migrationBuilder.DropTable(
                name: "ProdutosImagem");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Imagens");

            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "ProdutosGrupo");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ListasPreco");

            migrationBuilder.DropTable(
                name: "Observacoes");
        }
    }
}

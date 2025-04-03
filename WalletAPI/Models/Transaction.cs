using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletAPI.Models;

public partial class Transaction
{
    [Key]
    public int Id { get; set; }

    public int WalletId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    public int TypeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("TypeId")]
    [InverseProperty("Transactions")]
    public virtual TypeTransaction Type { get; set; } = null!;

    [ForeignKey("WalletId")]
    [InverseProperty("Transactions")]
    public virtual Wallet Wallet { get; set; } = null!;
}

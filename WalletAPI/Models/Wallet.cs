using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WalletAPI.Models;

[Index("DocumentId", Name = "UQ__Wallets__1ABEEF0E9D5B5335", IsUnique = true)]
public partial class Wallet
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string DocumentId { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Balance { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Wallet")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

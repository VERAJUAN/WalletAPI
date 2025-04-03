using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WalletAPI.Models;

[Table("TypeTransaction")]
[Index("Name", Name = "UQ__TypeTran__737584F6F750F441", IsUnique = true)]
public partial class TypeTransaction
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Type")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

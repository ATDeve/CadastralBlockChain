using System;

namespace СadastralBlockChain.Models;

public class BlockListModel
{
    public string Time => IsNew ? "(новый)" : CreatedAt.ToString();
    public DateTime CreatedAt { get; set; }
    public BlockData Block { get; set; }
    public bool IsNew { get; set; }

    public string Cred { get; set; }
}
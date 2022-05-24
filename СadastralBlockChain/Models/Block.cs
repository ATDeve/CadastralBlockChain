using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace СadastralBlockChain.Models;

/// <summary>
/// Блок из цепочки блоков.
/// </summary>
public class Block
{
    /// <summary>
    /// Момент создания блока.
    /// </summary>
    [JsonProperty]
    public DateTime CreatedOn { get; private set; }

    /// <summary>
    /// Хеш блока.
    /// </summary>
    [JsonProperty]
    public string Hash { get; private set; }

    /// <summary>
    /// Хеш предыдущего блока.
    /// </summary>
    [JsonProperty]
    public string PreviousHash { get; private set; }

    /// <summary>
    /// Данные блока.
    /// </summary>
    [JsonIgnore]
    public BlockData Data => JsonConvert.DeserializeObject<BlockData>(_dataString);

    /// <summary>
    /// Данные блока.
    /// </summary>
    [JsonProperty]
    private string _dataString;

    /// <summary>
    /// Создать экземпляр блока.
    /// </summary>
    /// <param name="previousBlock">Предыдущий блок.</param>
    /// <param name="data">Данные, сохраняемые в блоке.</param>
    public Block(Block previousBlock, BlockData data, string cred)
    {
        CreatedOn = DateTime.Now.ToUniversalTime();
        PreviousHash = previousBlock.Hash;
        _dataString = JsonConvert.SerializeObject(data);
        var stringData = PreviousHash + _dataString + Sha512(cred);
        Hash = Sha512(stringData);
    }

    /// <summary>
    /// Создать экземпляр блока.
    /// </summary>
    /// <param name="data">Данные, сохраняемые в блоке.</param>
    public Block(BlockData data, string cred)
    {
        CreatedOn = DateTime.Now.ToUniversalTime();
        _dataString = JsonConvert.SerializeObject(data);
        Hash = Sha512(_dataString + Sha512(cred));
    }

    [JsonConstructor]
    private Block()
    {

    }

    private string Sha512(string input)
        => Convert.ToBase64String(
            SHA512.HashData(
                Encoding.UTF8.GetBytes(input)));

    public bool Validate(string cred)
    {
        if (string.IsNullOrEmpty(PreviousHash))
        {
            return Hash == Sha512(_dataString + Sha512(cred));
        }
        else
        {
            var stringData = PreviousHash + _dataString + Sha512(cred);
            return Hash == Sha512(stringData);
        }
    }
}
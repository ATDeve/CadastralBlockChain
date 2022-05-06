using System;
using System.Collections.Generic;
using System.Linq;

namespace СadastralBlockChain.Models;

public class LandBlockModel
{
    public LandBlockModel(int height, int width)
    {
        Height = height;
        Width = width;
        LandType = LandType.None;
        CadastralNumber = "1";
    }

    public LandBlockModel()
    {

    }

    public string CadastralNumber { get; set; }

    public LandBlockModel[] Children { get; set; } = Array.Empty<LandBlockModel>();

    public int X { get; set; }

    public int Y { get; set; }

    public int Height { get; set; }

    public int Width { get; set; }

    public LandType LandType { get; set; }

    public string LandTypeString => LandType switch
    {
        LandType.None => "Не используется",
        LandType.Rent => "Сдается в аренду",
        LandType.Sent => "Продано",
        _ => throw new ArgumentOutOfRangeException()
    };

    public void SetChilden(IEnumerable<LandBlockModel> children)
    {
        Children = children.Select((x, i) =>
        {
            x.CadastralNumber = $"{CadastralNumber}.{i + 1}";

            return x;
        }).ToArray();
    }
}
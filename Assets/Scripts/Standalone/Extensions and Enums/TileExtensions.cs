using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class TileExtensions
{
    public static uniqueTileIndex Left(this TileType @this, float cubeSize)
    {
        var leftPosition = new Vector2(@this.Position.x - cubeSize, @this.Position.y);
        return new uniqueTileIndex(leftPosition);
    }
    public static uniqueTileIndex Right(this TileType @this, float cubeSize)
    {
        var rightPosition = new Vector2(@this.Position.x + cubeSize, @this.Position.y);
        return new uniqueTileIndex(rightPosition);
    }
    public static uniqueTileIndex Top(this TileType @this, float cubeSize)
    {
        var topPosition = new Vector2(@this.Position.x, @this.Position.y + cubeSize);
        return new uniqueTileIndex(topPosition);
    }
    public static uniqueTileIndex Bottom(this TileType @this, float cubeSize)
    {
        var bottomPosition = new Vector2(@this.Position.x, @this.Position.y - cubeSize);
        return new uniqueTileIndex(bottomPosition);
    }
}

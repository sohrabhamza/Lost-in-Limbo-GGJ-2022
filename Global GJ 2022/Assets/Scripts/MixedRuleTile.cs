using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Mixed Rule Tile", menuName = "2D/Tiles/Mixed Rule Tile")]
public class MixedRuleTile : RuleTile<MixedRuleTile.Neighbor> {
    public int tileFamily;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
        public const int SharedFamily = 5;
        public const int NotSharedFamily = 6;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        MixedRuleTile mTile = tile as MixedRuleTile;
        
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
            case Neighbor.SharedFamily:
                return mTile != null && mTile.tileFamily == this.tileFamily;
            case Neighbor.NotSharedFamily:
                return mTile == null ? true : mTile.tileFamily != this.tileFamily;
        }

        return base.RuleMatch(neighbor, tile);
    }
}
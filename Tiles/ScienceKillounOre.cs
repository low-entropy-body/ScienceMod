using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace Science.Tiles
{
    public class ScienceKillounOre:ModTile
    {
        public override void SetDefaults()
        {
            TileID.Sets.Ore[Type] = true;
             
            Main.tileSpelunker[Type] = true; //贴图会受到洞穴探险药水高亮的影响  

            Main.tileValue[Type] = 410; // 金属探测器的值 https://terraria.gamepedia.com/Metal_Detector
            Main.tileShine2[Type] = true; // Modifies the draw color slightly.
            Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
            Main.tileMergeDirt[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;

            ModTranslation name = CreateMapEntryName();
            AddMapEntry(new Color(230, 102, 24), name);

            dustType = DustID.Platinum;
            drop = ModContent.ItemType<Items.Placeable.ScienceKillounOre>();

            soundType = SoundID.Tink;
            soundStyle = 1;
            minPick = 90;
        }
    }
}

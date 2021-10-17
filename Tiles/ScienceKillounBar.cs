using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;

namespace Science.Tiles
{
    public class ScienceKillounBar : ModTile 
    {
        public override void SetDefaults()
        {
			Main.tileShine[Type] = 1100;
			Main.tileSolid[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			drop = ModContent.ItemType<Items.Placeable.ScienceKillounBar>();
			minPick = 70;
			ModTranslation name =  CreateMapEntryName();
			AddMapEntry(new Color(230, 102, 24),name);
		}


		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.5f;
			g = 0.5f;
			b = 0.5f;
		}

		public override bool Drop(int i, int j)
		{
			Tile t = Main.tile[i, j];
			int style = t.frameX / 18;
			if (style == 0) // It can be useful to share a single tile with multiple styles. This code will let you drop the appropriate bar if you had multiple.
			{
				Item.NewItem(i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Placeable.ScienceKillounBar>());
			}
			return base.Drop(i, j);
		}
	}
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Science.Dusts;

namespace Science.Tiles
{
    class ScienceHighDimensionalMatterProjector:ModTile
    {
		public override void SetDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
			TileObjectData.newTile.CoordinateHeights = new[] { 18 };
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("High Dimensional Matter Projector");
			name.AddTranslation(GameCulture.Chinese, "高维物质投影仪");
			AddMapEntry(new Color(200, 200, 200), name);
			dustType = ModContent.DustType<Sparkle>();
			disableSmartCursor = true;

			//可以决定他满足哪种类型的制作条件
			adjTiles = new int[] { TileID.WorkBenches };
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 16, ModContent.ItemType<Items.Placeable.ScienceHighDimensionalMatterProjector>());
		}
	}
}


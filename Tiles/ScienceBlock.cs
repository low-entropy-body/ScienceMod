using Microsoft.Xna.Framework;
using Science.Dusts;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;


namespace Science.Tiles
{
    class ScienceBlock:ModTile
    {


		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			dustType = ModContent.DustType<Sparkle>();
			drop = ModContent.ItemType<Items.Placeable.ScienceBlock>();
			minPick = 200;
			ModTranslation text = mod.CreateTranslation("Mods.Science.MapObject.ScienceBlock");
			text.SetDefault("ScienceBlock");
			text.AddTranslation(GameCulture.Chinese, "科学方块");
			mod.AddTranslation(text);
			AddMapEntry(new Color(200, 200, 200),text);
			
		}


		/// <summary>
		/// Allows you to change how many dust particles are created when the tile at the given coordinates is hit.
		/// 可以决定当在被定坐标的弹幕被击中时创造多少粒子
		/// </summary>
		/// <param name="i">The x position in tile coordinates.</param>
		/// <param name="j">The y position in tile coordinates.</param>
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}


		/// <summary>
		/// Allows you to determine how much light this block emits. Make sure you set Main.tileLighted[Type] to true in SetDefaults for this to work.
		/// 决定亮度，需要在SetDefaults中将Main.tileLighted[Type]设置为true
		/// </summary>
		/// <param name="i">The x position in tile coordinates.</param>
		/// <param name="j">The y position in tile coordinates.</param>
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.5f;
			g = 0.5f;
			b = 0.5f;
		}



		/// <summary>
		/// Allows you to change the style of waterfall that passes through or over this type of tile.
		/// 当通过水流或者相应类型的弹幕（可能是墙壁的意思？）改变样式
		/// </summary>
		/// <param name="style"></param>
		public override void ChangeWaterfallStyle(ref int style)
		{
			//style = mod.GetWaterfallStyleSlot("ExampleWaterfallStyle");
		}


		/// <summary>
		/// Allows this tile to support a sapling that can eventually grow into a tree. The type of the sapling should be returned here. Returns -1 by default. The style parameter will determine which sapling is chosen if multiple sapling types share the same ID; even if you only have a single sapling in an ID, you must still set this to 0.
		///允许方块?长成树
		/// </summary>
		/// <param name="style"></param>
		/// <returns></returns>
		///public override int SaplingGrowthType(ref int style)
		///{
			///style = 0;
			///return ModContent.TileType<ExampleSapling>();
		///}
	}
}


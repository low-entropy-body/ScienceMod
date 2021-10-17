using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items.Placeable
{
    public class ScienceKillounOre:ModItem
    {
		public override void SetStaticDefaults()
		{
			ItemID.Sets.SortingPriorityMaterials[item.type] = 58;
			DisplayName.SetDefault("Killoun Ore");
			DisplayName.AddTranslation(GameCulture.Chinese, "喀琉恩原矿");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.ScienceKillounOre>();
			item.width = 20;
			item.height = 20;
			item.value = 3000;
		}
	}
}

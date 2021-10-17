using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
namespace Science.Items.Placeable
{
    class ScienceHighDimensionalMatterProjector:ModItem

    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("science high dimensional matter projector");
			DisplayName.AddTranslation(GameCulture.Chinese, "高维度物质投影仪");
			Tooltip.SetDefault("this is science high dimensional matter projector");
			Tooltip.AddTranslation(GameCulture.Chinese, "这是高维度物质投影仪");
		}

		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 14;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150;
			item.createTile = ModContent.TileType<Tiles.ScienceHighDimensionalMatterProjector>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}


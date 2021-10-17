using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace Science.Items.Placeable
{
    class ScienceBlock:ModItem
    {
        public override void SetStaticDefaults()
        {

            ItemID.Sets.ExtractinatorMode[item.type] = item.type;

			DisplayName.SetDefault("scientific block");
            DisplayName.AddTranslation(GameCulture.Chinese, "科学方块");

            Tooltip.SetDefault("a kind of scientific block");
            Tooltip.AddTranslation(GameCulture.Chinese, "一种很科学的方块");
        }


		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.ScienceBlock>();
		}
		

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		//对放入提炼机方法的重写
		public override void ExtractinatorUse(ref int resultType, ref int resultStack)
		{
			if (Main.rand.NextBool(30))
			{
				
				if (Main.rand.NextBool(5))
				{
					resultStack += Main.rand.Next(2);
				}
			}
		}


	}
}

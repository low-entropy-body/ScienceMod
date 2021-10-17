using Microsoft.Xna.Framework;
using Science.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items
{
	public class SciencePickaxe : ModItem
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Science Pickaxe");
			DisplayName.AddTranslation(GameCulture.Chinese, "科学镐");


			Tooltip.SetDefault("This is a modded pickaxe.");
			Tooltip.AddTranslation(GameCulture.Chinese, "这是科学稿子");
		}

		public override void SetDefaults() {
			item.damage = 20;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 10;
			//稿力
			item.pick = 220;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox) {
			if (Main.rand.NextBool(10)) {
				Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Sparkle>());
			}
		}
	}
}
using Microsoft.Xna.Framework;
using Science.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items
{
	public class ScienceHamAxe_1 : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("science hamaxe");
			DisplayName.AddTranslation(GameCulture.Chinese, "��ѧ����");

			Tooltip.SetDefault("This is a modded hamaxe.");
			Tooltip.AddTranslation(GameCulture.Chinese, "��ѧ����");
		}

		public override void SetDefaults() {
			item.damage = 25;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			//����
			//��Ϸ�е���ʾ�ĸ�������5��
			item.axe = 30;          

			//����
			item.hammer = 100;
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
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Sparkle>());
			}
		}
	}
}

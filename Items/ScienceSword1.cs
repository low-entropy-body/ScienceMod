using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework;
using Science.Utils;
using Science.Buffs;

namespace Science.Items
{
	public class ScienceSword1 : ModItem
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Science Sword");
			DisplayName.AddTranslation(GameCulture.Chinese,"��ѧ֮��");
			Tooltip.SetDefault("This is a basic modded sword.");
			Tooltip.AddTranslation(GameCulture.Chinese, "֥ʿ����������");
		}

		public override void SetDefaults() 
		{
			item.damage = 1;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 6;
			item.useAnimation = 6;
			item.useStyle = 3;
			item.knockBack = 0;
			item.value = 1;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;


			//��ս��������ʱ�ǿɷ�ת��
			item.useTurn = true;
		}


        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            base.MeleeEffects(player, hitbox);
			// �������ĻӶ��ж��������һЩ����������Ч
			Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height,
				DustID.Fire, 0, 0, 100, Color.White, 2f);

			//������
			dust.noGravity = true;


		}


        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            base.OnHitNPC(player, target, damage, knockBack, crit);
			target.AddBuff(ModContent.BuffType<ScienceBuff1>(), 600);
		}


        public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DirtBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
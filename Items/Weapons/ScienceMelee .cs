using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;
using Microsoft.Xna.Framework;

namespace Science.Items
{
    //public class ScienceSomeing : ModItem
    //{
    //    public override void SetStaticDefaults()
    //    {
    //        DisplayName.SetDefault("Science Someing");
    //        DisplayName.AddTranslation(GameCulture.Chinese, "科学某物");
    //        Tooltip.SetDefault("Difficent to give a name for this. ");
    //        Tooltip.AddTranslation(GameCulture.Chinese, "佚名");
    //    }

    //    public override void SetDefaults()
    //    {
    //        item.damage = 50;
    //        item.melee = true;
    //        item.width = 40;
    //        item.height = 40;
    //        item.useTime = 12;
    //        item.useAnimation = 12;
    //        item.useStyle = ItemUseStyleID.SwingThrow;
    //        item.knockBack = 0;
    //        item.value = 80;
    //        item.rare = ItemRarityID.Green;
    //        item.UseSound = SoundID.Item1;
    //        item.autoReuse = true;
    //        item.scale = 0.5f;

    //        //近战武器挥舞时是可否转身
    //        item.useTurn = true;
            
    //    }


    //    public override void MeleeEffects(Player player, Rectangle hitbox)
    //    {
            
    //        // 在武器的挥动判定区域添加一些火焰粒子特效
    //        Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height,
    //            DustID.Fire, 0, 0, 100, Color.White, 2f);
    //        //无重力
    //        dust.noGravity = true;
    //    }


    //    public override void AddRecipes()
    //    {
    //        ModRecipe recipe = new ModRecipe(mod);
    //        recipe.AddIngredient(ItemID.DirtBlock, 10);
    //        recipe.AddTile(TileID.WorkBenches);
    //        recipe.SetResult(this);
    //        recipe.AddRecipe();
    //    }
    //}
}
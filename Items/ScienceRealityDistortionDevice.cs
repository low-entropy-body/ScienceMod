using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Terraria;
using Science.Utils;
using Microsoft.Xna.Framework;
namespace Science.Items
{
    public class ScienceRealityDistortionDevice:ModItem
    {
        public override string Texture => (GetType().Namespace + "." + "ExampleItem").Replace('.', '/');
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reality Distortion Device");
            DisplayName.AddTranslation(GameCulture.Chinese, "现实扭曲装置");

            Tooltip.SetDefault("");
            Tooltip.AddTranslation(GameCulture.Chinese, 
                "对物理规律的一次尝试性挑战\n"+
                "放在背包中按下" +
                Science.RealityDistortionDeviceHotKey + "键,可阻挡弹幕，并且格挡伤害"
                );
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 100;
            item.rare = ItemRarityID.Orange;
        }

        /// <summary>
        /// 阻挡弹幕的持续时间
        /// </summary>
        public static int DeviceUsetTime = 60;

        /// <summary>
        /// kill弹幕的碰撞盒
        /// </summary>
        public Rectangle KillBox;
        /// <summary>
        /// 阻挡弹幕的半径
        /// </summary>
        public float ResistRadius = 16;

        //每一个实例在每一帧都会调用
        //阻挡弹幕的实现
        public override void UpdateInventory(Player player)
        {
            SciencePlayer sPlayer = player.GetModPlayer<SciencePlayer>();
            sPlayer.RealityDistortionTimer++;
            int KillBoxStartX = (int)(player.position.X - 16);
            int KillBoxStartY = (int)(player.position.Y - 16);
            int KillBoxWidth = 80;
            int KillBoxHeight = 80;
            KillBox = new Rectangle(KillBoxStartX, KillBoxStartY, KillBoxWidth, KillBoxHeight);
            if (sPlayer.RealityDistortionTimer < DeviceUsetTime && sPlayer.BeginSpawnRealityDistortion)
            {
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.hostile)
                    {
                        if (proj.Colliding(proj.Hitbox,KillBox))
                        {
                            proj.Kill();
                            //todo:粒子效果
                        }
                    }
                }
            }
            else
            {
                sPlayer.RealityDistortionTimer = 0;
                sPlayer.BeginSpawnRealityDistortion = false;
            }
        }

        //玩家的背包中只能有1个
        public override bool CanPickup(Player player)
        {
            return !ScienceUtils.HasGivenItem(player, item.type);
        }

        //如果在制作的时候背包里有，那么掉落一个
        public override void OnCraft(Recipe recipe)
        {
            if (ScienceUtils.InventoryItemCount(Main.player[Main.myPlayer], item.type) >= 1)
            {
                Main.mouseItem.stack--;
                Item.NewItem(Main.player[Main.myPlayer].position, item.type);
            }
        }

        //现实扭曲装置，克盾+夸克聚合物50+神圣锭20
        public override void AddRecipes()
        {
            ScienceModRecipe recipe = new ScienceModRecipe(mod);
            recipe.AddIngredient(ItemID.EoCShield);
            recipe.AddIngredient(ModContent.ItemType<ScienceQuarkPolymer>(), 50);
            recipe.AddIngredient(ItemID.HallowedBar, 20);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

    }
}

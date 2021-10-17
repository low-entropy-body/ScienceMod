using Terraria.ModLoader;
using Science.Items;
using Terraria.ID;
using Science.Tiles;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria;

namespace Science
{
    public class Science : Mod
    {
        public static ModHotKey RealityDistortionDeviceHotKey;
        public static Effect npcEffect;
        public static Effect ScreenFilter;

        /// <summary>
        /// 蓝色的瞄准线基本方块，大小为4 X 4
        /// </summary>
        public static Texture2D ScienceAimingLineBlue;
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(this);
            recipe.AddIngredient(ModContent.ItemType<ScienceQuarkPolymer>(), 10);
            recipe.AddTile(ModContent.TileType<ScienceHighDimensionalMatterProjector>());
            recipe.SetResult(ItemID.SoulofLight);
            recipe.AddRecipe();

            recipe = new ModRecipe(this);
            recipe.AddIngredient(ModContent.ItemType<ScienceQuarkPolymer>(), 10);
            recipe.AddTile(ModContent.TileType<ScienceHighDimensionalMatterProjector>());
            recipe.SetResult(ItemID.SoulofNight);
            recipe.AddRecipe();
        }

        public override void Load()
        {
            RealityDistortionDeviceHotKey = RegisterHotKey("ClearProjectileNearby", "R");
            ScienceAimingLineBlue = GetTexture("ScienceAimingLineBlue");
            //npcEffect = GetEffect("Effects/GrayScale");
            //npcEffect = GetEffect("Effects/RainBow");
            npcEffect = GetEffect("Effects/Outline");
            //ScreenFilter = GetEffect("Effects/MagnifyingGlass");
            // 注意设置正确的Pass名字，Scene的名字可以随便填，不和别的Mod以及原版冲突即可
            //Filters.Scene["Science:MagnifyingGlass"] = new Filter(new ScienceScreenShaderData(new Ref<Effect>(GetEffect("Effects/MagnifyingGlass")),
            //    "MagnifyingGlass"),
            //    EffectPriority.Medium);

            //Filters.Scene["Science:MagnifyingGlass"].Load();
            //ScreenFilter = GetEffect("Effects/GaussianBur");
            //Filters.Scene["Science:GaussianBur"] = new Filter(new ScienceScreenShaderData(new Ref<Effect>(GetEffect("Effects/GaussianBur")),
            //    "GaussianBur"),
            //    EffectPriority.Medium);


        }

        public override void Unload()
        {
            RealityDistortionDeviceHotKey = null;
            npcEffect = null;
            ScreenFilter = null;
            ScienceAimingLineBlue = null;
        }



        public override void PreUpdateEntities()
        {
            //if (!Filters.Scene["Science:MagnifyingGlass"].IsActive())
            //{
            //    // 开启滤镜
            //    Filters.Scene.Activate("Science:MagnifyingGlass");
            //}

            //if (!Filters.Scene["Science:GaussianBur"].IsActive())
            //{
            //    // 开启滤镜
            //    Filters.Scene.Activate("Science:GaussianBur");
            //}
        }

    }
}
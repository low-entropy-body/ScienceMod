using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items
{
    class ScienceBluePrint_1:ModItem
    {
        public override string Texture => (GetType().Namespace + "." + "ExampleItem").Replace('.', '/');
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("blueprint");
            DisplayName.AddTranslation(GameCulture.Chinese, "蓝图1");

            Tooltip.SetDefault("a blue print");
            Tooltip.AddTranslation(GameCulture.Chinese, "一个蓝图");
            
        }
        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = 100;
            item.rare = ItemRarityID.Blue;
            
        }
    }
}

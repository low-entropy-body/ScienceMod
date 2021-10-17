using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;

namespace Science.Items.Accessories
{
    class SciencePurpleStone:ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Science Purple Stone");
            DisplayName.AddTranslation(GameCulture.Chinese, "科学紫宝石");
            Tooltip.SetDefault("This is a purple stone.");
            Tooltip.AddTranslation(GameCulture.Chinese, "科学的紫宝石");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.accessory = true;
            item.width = 22;
            item.height = 22;
            // 重点在这里，这个属性设为true才能带在身上
            item.accessory = true;
            // 物品的面板防御数值，装备了以后就会增加
            item.defense = 100;
            item.rare = 13;
            item.value = Item.sellPrice(0, 5, 0, 0);
            // 这个属性代表这是专家模式专有物品，稀有度颜色会是彩虹的！
            item.expert = true;
        }

        public override void AddRecipes()
        {
            base.AddRecipes();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            // 让玩家的生命值上限增加100
            player.statLifeMax2 += 100;
            // 让玩家魔法值上限增加100
            player.statManaMax2 += 100;
        }
    }
}


using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ID;
using Terraria;
using Terraria.DataStructures;

namespace Science.Items
{
    class ScienceQuarkPolymer : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Quark polymer");
            DisplayName.AddTranslation(GameCulture.Chinese, "夸克聚合物");

            Tooltip.SetDefault("this is Quark polymer");
            Tooltip.AddTranslation(GameCulture.Chinese, "这是夸克聚合物");

            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[item.type] = true;

            //动态物品图标
            ItemID.Sets.ItemIconPulse[item.type] = true;

            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
            refItem.SetDefaults(ItemID.SoulofSight);
            item.width = refItem.width;
            item.height = refItem.height;
            item.maxStack = 9999;
            item.value = 1;
            item.rare = ItemRarityID.Orange;
        }
    }

}
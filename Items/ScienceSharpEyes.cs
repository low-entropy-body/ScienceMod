using Science.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace Science.Items
{
    public class ScienceSharpEyes:ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("science potion");
            DisplayName.AddTranslation(GameCulture.Chinese, "科学药水");

            Tooltip.SetDefault("this is a scientific potion");
            Tooltip.AddTranslation(GameCulture.Chinese, "这是科学药水");
        }
        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 24;
            item.useAnimation = 17;
            item.useTime = 17;
            item.maxStack = 30;
            item.rare = ItemRarityID.Pink;
            item.value = Item.sellPrice(0, 0, 50, 0);

            item.useStyle = ItemUseStyleID.EatingUsing;

            // 喝药的声音
            item.UseSound = SoundID.Item3;
            // 决定这个物品使用以后会不会减少，true就是使用后物品会少一个，默认为false
            item.consumable = true;
            // 决定使用动画出现后，玩家转身会不会影响动画的方向，true就是会，默认为false
            item.useTurn = true;
            // 告诉TR内部系统，这个物品是一个生命药水物品，用于TR系统的特殊目的（比如一键喝药水），默认为false
            item.potion = true;
            // 这个药水能给玩家加多少血，跟potion一起使用喝完药就会有抗药性debuff
            item.healLife = 50;
        }


        // 当物品使用的时候触发，返回值貌似是什么都不会有影响
        public override bool UseItem(Player player)
        {  
            player.AddBuff(ModContent.BuffType<ScienceBuff1>(), 36000);
            player.ClearBuff(21);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items.Armors
{
    [AutoloadEquip(EquipType.Legs)]
    public class ScienceLeggings:ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Science Leggings");
            DisplayName.AddTranslation(GameCulture.Chinese, "科学护胫");
            Tooltip.SetDefault("this is scientific leggings");
            Tooltip.AddTranslation(GameCulture.Chinese, "这是科学护胫");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Orange;

            // 防御+12
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            // 如果玩家的速度的值大于一定值，也就是玩家在移动
            if (player.velocity.Length() > 0.05f)
            {
                // 就增加全部伤害
                player.meleeDamage += 0.05f;
                player.rangedDamage += 0.05f;
                player.magicDamage += 0.05f;
                player.minionDamage += 0.05f;
                player.thrownDamage += 0.05f;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

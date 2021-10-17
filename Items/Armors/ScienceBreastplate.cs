using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items.Armors
{

    [AutoloadEquip(EquipType.Body)]
    public class ScienceBreastplate:ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Science Breastplate");
            DisplayName.AddTranslation(GameCulture.Chinese,"科学胸甲");

            Tooltip.SetDefault("this is a scientific breastplate");
            Tooltip.AddTranslation(GameCulture.Chinese, "这是科学的胸甲" + 
                "\n免疫灼烧debuff"
                + "\n+50生命上限"
                + "\n增加回血能力");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = ItemRarityID.Orange;
            // 装备的防御值
            item.defense = 75;
        }

        public override void UpdateEquip(Player player)
        {
            // 免疫灼伤debuff
            player.buffImmune[BuffID.OnFire] = true;

            // 增加生命上限50
            player.statLifeMax2 += 50;

            // 增加2点生命恢复，虽然看起来不多，其实在游戏里还挺可观的
            player.lifeRegen += 2;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

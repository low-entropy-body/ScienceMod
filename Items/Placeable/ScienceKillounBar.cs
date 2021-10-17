using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Science.Items.Placeable
{
    /// <summary>
    /// 亚克锭
    /// </summary>
    public class ScienceKillounBar:ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("KillounBar");
            DisplayName.AddTranslation(GameCulture.Chinese, "喀琉恩锭");

        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 12;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.ScienceKillounBar>();
        }

    }
}


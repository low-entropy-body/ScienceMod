using Science.Items;
using Terraria.ModLoader;

namespace Science
{
    class ScienceModRecipe:ModRecipe
    {
        public ScienceModRecipe(Mod mod) : base(mod)
        {

        }

        //在合成时不消耗蓝图
        public override int ConsumeItem(int type, int numRequired)
        {
            if (type == ModContent.ItemType<ScienceBluePrint_1>())
            {
                return 0;
            }
            else
                return numRequired;
        }
    }
}

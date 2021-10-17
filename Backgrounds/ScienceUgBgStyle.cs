using Terraria;
using Terraria.ModLoader;

namespace Science.Backgrounds
{
	public class ScienceUgBgStyle : ModUgBgStyle
	{
		public override bool ChooseBgStyle()
		{
			return Main.LocalPlayer.GetModPlayer<SciencePlayer>().ZoneScience;
		}

		public override void FillTextureArray(int[] textureSlots)
		{
			textureSlots[0] = mod.GetBackgroundSlot("Backgrounds/ScienceBiomeUG0");
			textureSlots[1] = mod.GetBackgroundSlot("Backgrounds/ScienceBiomeUG1");
			textureSlots[2] = mod.GetBackgroundSlot("Backgrounds/ScienceBiomeUG2");
			textureSlots[3] = mod.GetBackgroundSlot("Backgrounds/ScienceBiomeUG3");
		}
	}
}
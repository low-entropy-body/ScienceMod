using Terraria;
using Terraria.ModLoader;

namespace Science.Backgrounds
{
    class ScieneSurfaceBgStyle : ModSurfaceBgStyle
    {
        public override bool ChooseBgStyle()
        {
            return !Main.gameMenu && Main.LocalPlayer.GetModPlayer<SciencePlayer>().ZoneScience;
        }

		// Use this to keep far Backgrounds like the mountains.
		//允许你修改所有背景样式的透明度。一般情况下，您应该将与该样式槽相等的索引移动到接近1的位置，
		//而将所有其他索引移动到接近0的位置。transitionSpeed参数是你应该添加/减去每个元素的褪色参数
		public override void ModifyFarFades(float[] fades, float transitionSpeed)
		{
			for (int i = 0; i < fades.Length; i++)
			{
				if (i == Slot)
				{
					fades[i] += transitionSpeed;
					if (fades[i] > 1f)
					{
						fades[i] = 1f;
					}
				}
				else
				{
					fades[i] -= transitionSpeed;
					if (fades[i] < 0f)
					{
						fades[i] = 0f;
					}
				}
			}
		}

		public override int ChooseFarTexture()
		{
			return mod.GetBackgroundSlot("Backgrounds/ExampleBiomeSurfaceFar");
		}

		private static int SurfaceFrameCounter;
		private static int SurfaceFrame;
		public override int ChooseMiddleTexture()
		{
			if (++SurfaceFrameCounter > 12)
			{
				SurfaceFrame = (SurfaceFrame + 1) % 4;
				SurfaceFrameCounter = 0;
			}
			switch (SurfaceFrame)
			{
				case 0:
					return mod.GetBackgroundSlot("Backgrounds/ScienceBiomeSurfaceMid0");
				case 1:
					return mod.GetBackgroundSlot("Backgrounds/ScienceBiomeSurfaceMid1");
				case 2:
					return mod.GetBackgroundSlot("Backgrounds/ScienceBiomeSurfaceMid2");
				case 3:
					return mod.GetBackgroundSlot("Backgrounds/ScienceBiomeSurfaceMid3");
				default:
					return -1;
			}
		}

		public override int ChooseCloseTexture(ref float scale, ref double parallax, ref float a, ref float b)
		{
			return mod.GetBackgroundSlot("Backgrounds/ScienceBiomeSurfaceClose");
		}
	}
}


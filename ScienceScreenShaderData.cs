using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace Science
{
    public class ScienceScreenShaderData: ScreenShaderData
    {
        public ScienceScreenShaderData(string passName) : base(passName)
        {
        }
        public ScienceScreenShaderData(Ref<Effect> shader, string passName) : base(shader, passName)
        {
        }

        public override void Apply()
        {
            base.Apply();
        }
    }
}

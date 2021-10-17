using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;
using Science.Utils;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Science.Projectiles
{
    public class ScienceBullet : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("科学子弹");     
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
        }

		public override void SetDefaults()
		{
			projectile.width = 8;               //The width of projectile hitbox
			projectile.height = 8;              //The height of projectile hitbox
			projectile.aiStyle = -1;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = false;         //Can the projectile deal damage to enemies?
			projectile.hostile = true;         //Can the projectile deal damage to the player?          
			projectile.ranged = true;			//Is the projectile shoot by a ranged weapon?
			projectile.penetrate = -1;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.timeLeft = 600;          //The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
			projectile.alpha = 0;             //The transparency of the projectile, 255 for completely transparent. (aiStyle 1 quickly fades the projectile in) Make sure to delete this if you aren't using an aiStyle that fades in. You'll wonder why your projectile is invisible.
			projectile.light = 3f;            //How much light emit around the projectile
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;          //Can the projectile collide with tiles?
			projectile.extraUpdates = 1;            //Set to above 0 if you want the projectile to update multiple time in a frame
			//aiType = ProjectileID.Bullet;           //Act exactly like default Bullet
			
		}

        


        public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Item10 is the bounce sound you hear.
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Item10, projectile.position);
		}



        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			
			int posX = (int)(projectile.Center.X + Vector2.Normalize(oldVelocity).X * 4) / 16;
			int posY = (int)(projectile.Center.Y + Vector2.Normalize(oldVelocity).Y * 4) / 16;

			Main.NewText("碰到了");
			projectile.Kill();
			Vector2 delta = new Vector2(oldVelocity.X/Math.Abs(oldVelocity.X),oldVelocity.Y/Math.Abs(oldVelocity.Y));

			Vector2[] Delta = new Vector2[4] 
			{ 
				Vector2.Zero,
				new Vector2(delta.X, 0) , 
				new Vector2(0, delta.Y) ,
				delta 
			}; 

			foreach (Vector2 vec in Delta)
			{
				WorldGen.KillTile((int)(posX + vec.X), (int)(posY + vec.Y), noItem: false);
			}

			return false;
        }

        public override void AI()
        {
			Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height
											, DustID.Fire, 0f, 0f, 100, Color.Blue, 1f);
			// 粒子特效不受重力
			dust.noGravity = true;
			// 让粒子默认的运动速度归零
			dust.velocity *= 0;
			// 让粒子始终处于弹幕的中心位置
			dust.position = projectile.Center;
			projectile.timeLeft--;
		}
    }

    public class ScienceBlueLaser : ModProjectile
    {

        public override string Texture => "Terraria/Projectile_" + ProjectileID.GreenLaser;
		public Texture2D texture;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("blue laser");
            DisplayName.AddTranslation(GameCulture.Chinese, "蓝色激光");

        }


        public override void SetDefaults()
        {
			projectile.CloneDefaults(ProjectileID.GreenLaser);
			projectile.ranged = true;
			projectile.magic = false;
			projectile.timeLeft = 30;
			projectile.light = 0.5f;
			texture = ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.GreenLaser);
			
		}

        public override bool PreAI()
        {
            return false;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			spriteBatch.Draw(texture,
				projectile.Center - Main.screenPosition,
				new Rectangle(0, 0, (int)texture.Size().X, (int)texture.Size().Y),
				new Color(0, 0, 225, 225),
				projectile.rotation,
				texture.Size() / 2,
				1f,
				SpriteEffects.None,
				0f);
		}



		//      public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		//      {
		//	Texture2D texture = ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.GreenLaser);
		//	Rectangle rectangle = new Rectangle(0, 0, texture.Width,texture.Height);
		//	Vector2 pos = projectile.Center;
		//	spriteBatch.Draw(texture, pos, rectangle, Color.White, projectile.rotation, new Vector2(texture.Width, texture.Height) / 2, 1f, SpriteEffects.None, 0f);
		//}

		private int timer;
		
        public override void PostAI()
        {
			timer++;
			if(timer == 1)
            {
				projectile.rotation = projectile.velocity.ToRotation() + (float) Math.PI/2;
            }

			Main.player[projectile.owner].direction = (int)ScienceUtils.Get01Vec(projectile.velocity).X;
			
        }


        public override string GlowTexture => base.GlowTexture;
    }
}

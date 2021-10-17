using Science.Projectiles;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.Achievements;
using Terraria.ID;

namespace Science.Utils
{
    public class ScienceUtils
    {
        public static bool IsDimensionalPhantomArrow(Projectile proj)
        {
            return IsDimensionalPhantomArrow(proj.type);
        }

        public static bool IsDimensionalPhantomArrow(int type)
        {
            if (type == ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Blue>() ||
                type == ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Green>() ||
                type == ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Orange>() ||
                type == ModContent.ProjectileType<ScienceDimensionalPhantom_Arrow_Purple>())
            {
                return true;
            }
            return false;
        }

        public static bool HasGivenItem(Player player, int itemtype)
        {
            foreach(Item localitem in player.inventory)
            {
                if(itemtype == localitem.type)
                {
                    return true;
                }
            }
            return false;
        }

        public static int InventoryItemCount(Player player, int itemtype)
        {
            int result = 0;
            foreach(Item item in player.inventory)
            {
                if(item.type == itemtype)
                {
                    result += item.stack;
                }
            }
            
            return result;
        }

        public static Vector2 Get01Vec(Vector2 value)
        {
            return new Vector2(value.X == 0 ? 0 : value.X > 0 ? 1 : -1, value.Y == 0 ? 0 : value.Y > 0 ? 1 : -1);
        }

        public static int Get01(double value)
        {
            return value == 0 ? 0 : value > 0 ? 1 : -1;
        }

        /// <summary>
        /// 获得当前向量的渐进向量，用来在ai中画追踪弹幕的。
        /// </summary>
        /// <param name="value">你当前弹幕向量的速度</param>
        /// <param name="target">当前弹幕指向目标向量的期望速度，一般来讲就是速度是当前向量的速度然后方向指向目标，在这里我们为了安全性，会将二者速度相等</param>
        /// <param name="N">渐进因子，N越大，向量转向就越快。</param>
        /// <returns>经过转向之后的向量</returns>
        public static Vector2 GetGradualVector(Vector2 value , Vector2 target, float N)
        {
            Vector2 result;
            Vector2 NewTarget = Vector2.Normalize(target) * value.Length();
            result = (value * N + NewTarget) / (N + 1f);
            return result;
        }
        public static void print(string value)
        {
            Main.NewText(value);
        }

        public static void Expolse(Projectile projectile,int explosionRadius = 3)
        {
            
            int minTileX = (int)(projectile.position.X / 16f - (float)explosionRadius);
            int maxTileX = (int)(projectile.position.X / 16f + (float)explosionRadius);
            int minTileY = (int)(projectile.position.Y / 16f - (float)explosionRadius);
            int maxTileY = (int)(projectile.position.Y / 16f + (float)explosionRadius);
            if (minTileX < 0)
            {
                minTileX = 0;
            }
            if (maxTileX > Main.maxTilesX)
            {
                maxTileX = Main.maxTilesX;
            }
            if (minTileY < 0)
            {
                minTileY = 0;
            }
            if (maxTileY > Main.maxTilesY)
            {
                maxTileY = Main.maxTilesY;
            }
            bool canKillWalls = false;
            for (int x = minTileX; x <= maxTileX; x++)
            {
                for (int y = minTileY; y <= maxTileY; y++)
                {
                    float diffX = Math.Abs((float)x - projectile.position.X / 16f);
                    float diffY = Math.Abs((float)y - projectile.position.Y / 16f);
                    double distance = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distance < (double)explosionRadius && Main.tile[x, y] != null && Main.tile[x, y].wall == 0)
                    {
                        canKillWalls = true;
                        break;
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = true;
            for (int i = minTileX; i <= maxTileX; i++)
            {
                for (int j = minTileY; j <= maxTileY; j++)
                {
                    float diffX = Math.Abs((float)i - projectile.position.X / 16f);
                    float diffY = Math.Abs((float)j - projectile.position.Y / 16f);
                    double distanceToTile = Math.Sqrt((double)(diffX * diffX + diffY * diffY));
                    if (distanceToTile < (double)explosionRadius)
                    {
                        bool canKillTile = true;
                        if (Main.tile[i, j] != null && Main.tile[i, j].active())
                        {
                            canKillTile = true;
                            if (Main.tileDungeon[(int)Main.tile[i, j].type] || Main.tile[i, j].type == 88 || Main.tile[i, j].type == 21 || Main.tile[i, j].type == 26 || Main.tile[i, j].type == 107 || Main.tile[i, j].type == 108 || Main.tile[i, j].type == 111 || Main.tile[i, j].type == 226 || Main.tile[i, j].type == 237 || Main.tile[i, j].type == 221 || Main.tile[i, j].type == 222 || Main.tile[i, j].type == 223 || Main.tile[i, j].type == 211 || Main.tile[i, j].type == 404)
                            {
                                canKillTile = false;
                            }
                            if (!Main.hardMode && Main.tile[i, j].type == 58)
                            {
                                canKillTile = false;
                            }
                            if (!TileLoader.CanExplode(i, j))
                            {
                                canKillTile = false;
                            }
                            if (canKillTile)
                            {
                                WorldGen.KillTile(i, j, false, false, false);
                                if (!Main.tile[i, j].active() && Main.netMode != NetmodeID.SinglePlayer)
                                {
                                    NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
                                }
                            }
                        }
                        if (canKillTile)
                        {
                            for (int x = i - 1; x <= i + 1; x++)
                            {
                                for (int y = j - 1; y <= j + 1; y++)
                                {
                                    if (Main.tile[x, y] != null && Main.tile[x, y].wall > 0 && canKillWalls && WallLoader.CanExplode(x, y, Main.tile[x, y].wall))
                                    {
                                        WorldGen.KillWall(x, y, false);
                                        if (Main.tile[x, y].wall == 0 && Main.netMode != NetmodeID.SinglePlayer)
                                        {
                                            NetMessage.SendData(MessageID.TileChange, -1, -1, null, 2, (float)x, (float)y, 0f, 0, 0, 0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = false;
        }
    }
}

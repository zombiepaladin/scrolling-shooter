using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooterWindowsLibrary
{
    /// <summary>
    /// A class representing a tilemap
    /// </summary>
    public class Tilemap
    {
        /// <summary>
        /// The name of the tilemap
        /// </summary>
        public string Name;

        /// <summary>
        /// The tilemap's width, in tiles
        /// </summary>
        public int Width;

        /// <summary>
        /// The tilemap's height, in tiles
        /// </summary>
        public int Height;

        /// <summary>
        /// The width of the tilemap's tiles
        /// </summary>
        public int TileWidth;

        /// <summary>
        /// The height of the tilemap's tiles
        /// </summary>
        public int TileHeight;

        /// <summary>
        /// The tileset textures used in this tilemap
        /// </summary>
        public List<Texture2D> Textures;

        /// <summary>
        /// The total number of tiles in our tileset
        /// </summary>
        public int TileCount;

        /// <summary>
        /// The set of all tiles used by this tilemap
        /// </summary>
        public Tile[] Tiles;

        /// <summary>
        /// The total number of layers in our tilemap
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// The layers in our tilemap
        /// </summary>
        public TilemapLayer[] Layers;

        /// <summary>
        /// The total number of game object groups in our tilemap
        /// </summary>
        public int GameObjectGroupCount;

        /// <summary>
        /// The game object groups in our tilemap
        /// </summary>
        public GameObjectGroup[] GameObjectGroups;

        public Vector2 PlayerStart;

        public int PlayerLayer;

        /// <summary>
        /// Indicates if the tilemap is currently scrolling
        /// </summary>
        [ContentSerializerIgnore]
        public bool Scrolling = false;


        /// <summary>
        /// Updates the tilemap, applying scrolling 
        /// </summary>
        /// <param name="elapsedTime"></param>
        public void Update(float elapsedTime)
        {
            if (Scrolling)
            {
                for (int i = 0; i < LayerCount; i++)
                {
                    Layers[i].ScrollOffset += Layers[i].ScrollingSpeed * elapsedTime;
                }
            }
        }


        /// <summary>
        /// Draws the tilemap.  This implementation draws all 
        /// tiles.
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(float elapsedTime, SpriteBatch spriteBatch) 
        {
            for (int i = 0; i < LayerCount; i++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        int index = x + y * Width;
                        TileData tileData = Layers[i].TileData[index];
                        if (tileData.TileID != 0)
                        {
                            Tile tile = Tiles[tileData.TileID - 1];
                            spriteBatch.Draw(Textures[tile.TextureID], new Rectangle(x * TileWidth, y * TileHeight + (int)(0*Layers[i].ScrollOffset), TileWidth, TileHeight), tile.Source, Color.White, 0f, new Vector2(TileWidth / 2, TileHeight / 2), tileData.SpriteEffects, Layers[i].LayerDepth);
                        }
                    }
                }
            }
        }

    }
}

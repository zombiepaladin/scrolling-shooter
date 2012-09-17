using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using ScrollingShooterWindowsLibrary;

namespace ScrollingShooterContentPipeline
{
    /// <summary>
    /// The content pipeline equivalent of Tilemap
    /// </summary>
    [ContentSerializerRuntimeType("ScrollingShooterWindowsLibrary.Tilemap, ScrollingShooterWindowsLibrary")]
    public class TilemapContent
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
        /// A list of paths to all images used by this tilemap.
        /// This list is processed by the Tilemap Processor, and
        /// the resulting images loaded into the Texture list
        /// </summary>
        [ContentSerializerIgnore]
        public List<string> ImagePaths = new List<string>();

        /// <summary>
        /// The textures used by this tilemap.  These are represented
        /// by ExternalReferences, which are loaded from separate xnb 
        /// files; this ensures different tilemaps won't store duplicate 
        /// image data.
        /// </summary>
        public List<ExternalReference<Texture2DContent>> Textures = new List<ExternalReference<Texture2DContent>>();

        /// <summary>
        /// The total number of unique tiles used in our tilemap
        /// </summary>
        public int TileCount;

        /// <summary>
        /// The set of all unique tiles used by this tilemap
        /// </summary>
        public Tile[] Tiles;

        /// <summary>
        /// The total number of layers in our tileset
        /// </summary>
        public int LayerCount;

        /// <summary>
        /// The layers in our tileset
        /// </summary>
        public TilemapLayerContent[] Layers;

        /// <summary>
        /// The total number of game object groups in our tilemap
        /// </summary>
        public int GameObjectGroupCount;

        /// <summary>
        /// The game object groups in our tilemap
        /// </summary>
        public GameObjectGroupContent[] GameObjectGroups;

        /// <summary>
        /// The properties defined on this tilemap.  These are loaded
        /// from the tmx file, and should be converted to more efficient 
        /// and meaningful variables in the TilemapProcessor 
        /// </summary>
        [ContentSerializerIgnore]
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

    }
}

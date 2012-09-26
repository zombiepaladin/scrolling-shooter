using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ScrollingShooter
{
    /// <summary>
    /// Dummy class to stand in for file missing from commit
    /// </summary>
    public class BrainBossProtection : Enemy
    {
        public override Rectangle Bounds
        {
            get { return Rectangle.Empty; }
        }
        
        public BrainBossProtection(uint id, ContentManager content, Vector2 position )
            : base(id)
        { }

        public override void Update(float elapsedTime)
        {
            // DO NOTHING
        }

        public override void Draw(float elapsedTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            // DO NOTHING
        }

    }
}

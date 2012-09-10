<<<<<<< HEAD
=======
<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingShooter
{
    /// <summary>
    /// The different enemy types that exist in the game
    /// </summary>
    public enum EnemyType
    {
        Dart,
        JetMinion,
    }

    public abstract class Enemy : GameObject
    {
        public Enemy(uint id) : base(id) { }
    }
}
=======
>>>>>>> updating everything
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrollingShooter
{
    /// <summary>
    /// The different enemy types that exist in the game
    /// </summary>
    public enum EnemyType
    {
        Dart,
    }

    /// <summary>
    /// A base class for enemies in the game
    /// </summary>
    public abstract class Enemy : GameObject
    {
        public Enemy(uint id) : base(id) { }
    }
}
<<<<<<< HEAD
=======
>>>>>>> b5617c4156435a47d8ca773e55de9e922c9604c5
>>>>>>> updating everything

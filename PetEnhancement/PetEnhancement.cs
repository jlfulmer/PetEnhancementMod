using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;

using xTile.Tiles;
using xTile.Dimensions;
using xTile.Layers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PetEnhancement
{
    public class PetEnhancement : Mod
    {
        private const int FRIENDSHIP_POINTS = 300;

        private bool active = false;
        private PetActionHandler actionHandler = null;
        private MouseState previousMouseState;

        public override void Entry(params object[] objects)
        {
            StardewModdingAPI.Events.GameEvents.UpdateTick += EventUpdateTick;
        }


        private void EventUpdateTick(object sender, EventArgs e)
        {
            if (Game1.currentLocation == null) return;

            Farmer farmer = Game1.player;
            Pet pet = (Pet) Game1.getCharacterFromName(farmer.getPetName());

            if  (actionHandler == null && pet != null)
            {
                actionHandler = new PetActionHandler(pet);
                actionHandler.intialize();
            }

            MouseState mouseState = Mouse.GetState();
            if (mouseState.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed)
            {
                var cursorTile = Game1.currentCursorTile;
                bool intersects = Utility.doesRectangleIntersectTile(pet.GetBoundingBox(), (int)cursorTile.X, (int)cursorTile.Y);
                if (intersects && pet.friendshipTowardFarmer >= FRIENDSHIP_POINTS)
                {
                    active = !active;
                    if (active)
                    {
                        pet.jump();
                    }
                }
            }

            if (active)
            {
                actionHandler.performAction();
            }

            previousMouseState = mouseState;
        }
    }
}

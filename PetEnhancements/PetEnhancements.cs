using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Characters;

namespace PetEnhancements
{
    /// <summary>The mod entry class loaded by SMAPI.</summary>
    public class PetEnhancement : Mod
    {
        private const int FRIENDSHIP_POINTS = 0;

        private bool active;
        private PetActionHandler actionHandler;
        private Pet pet;

        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayStarted += onDayStarted;
            helper.Events.GameLoop.UpdateTicked += onUpdateTicked;
            helper.Events.Input.ButtonPressed += onButtonPressed;
        }

        /// <summary>Raised after the game begins a new day (including when the player loads a save).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void onDayStarted(object sender, DayStartedEventArgs e)
        {
            // reset pet
            this.pet = null;
        }

        /// <summary>Raised after the game state is updated (≈60 times per second).</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void onUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.currentLocation == null)
                return;

            // find pet
            if (pet == null)
            {
                this.pet = (Pet)Game1.getCharacterFromName(Game1.player.getPetName());
                if (pet != null)
                {
                    this.actionHandler = new PetActionHandler(pet);
                    this.actionHandler.intialize();
                }
                else
                    this.actionHandler = null;
            }

            // apply action
            if (this.active)
                actionHandler?.performAction();
        }

        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void onButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button == SButton.MouseRight && this.pet != null)
            {
                Vector2 cursorTile = Game1.currentCursorTile;
                bool intersects = Utility.doesRectangleIntersectTile(pet.GetBoundingBox(), (int)cursorTile.X, (int)cursorTile.Y);
                if (intersects && pet.friendshipTowardFarmer >= FRIENDSHIP_POINTS)
                {
                    active = !active;
                    if (active)
                        pet.jump();
                }
            }
        }
    }
}

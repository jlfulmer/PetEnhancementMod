using System;
using StardewValley;
using StardewValley.Characters;

namespace PetEnhancements
{
    internal class PetActionProvider
    {
        private const int ACTION_DELAY_MAX = 5;

        private Action currentAction = Action.SITTING;
        private readonly Random random;
        private readonly Pet pet;

        private float currDelay;
        private bool startedFollowing;

        public PetActionProvider(Pet pet)
        {
            this.pet = pet;
            random = new Random();
        }

        public void prepareNextAction()
        {
            float elapsed = (float)Game1.currentGameTime.ElapsedGameTime.TotalSeconds;
            currDelay -= elapsed;

            if (currDelay < 0 || !startedFollowing)
            {
                if (!Utility.tileWithinRadiusOfPlayer(pet.getTileX(), pet.getTileY(), 2, Game1.player))
                {
                    currentAction = Action.FOLLOWING;
                    startedFollowing = true;
                }
                else
                {
                    currentAction = Action.SITTING;
                }

                currDelay = random.Next(ACTION_DELAY_MAX);
            }
        }

        public Action getCurrentAction()
        {
            return currentAction;
        }

        public void setCurrentAction(Action action)
        {
            currentAction = action;
        }

        public void forceUpdate()
        {
            currDelay = -1;
        }
    }
}

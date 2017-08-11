using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Player;

namespace PointBlank.API.Unturned.Player
{
    public class UnturnedPlayerComponent : PointBlankPlayerComponent
    {
        #region Properties
        /// <summary>
        /// The instance of the player the component is attached to
        /// </summary>
        public UnturnedPlayer Player { get; private set; }
        #endregion

        #region Mono Functions
        void Awake()
        {
            SDG.Unturned.Player player = gameObject.GetComponent<SDG.Unturned.Player>();

            if(player == null)
            {
                Destroy(this);
                return;
            }
            Player = UnturnedPlayer.Get(player);
        }
        #endregion
    }
}

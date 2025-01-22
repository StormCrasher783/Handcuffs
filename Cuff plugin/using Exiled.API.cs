using Exiled.API.Features;
using Exiled.API.Interfaces;
using System;

namespace Site27CustomItems
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "Site 27 Custom Items";
        public override string Author { get; } = "StormCrasher783";
        public override string Prefix { get; } = "site27customitems";

        public override void OnEnabled()
        {
            // Register
            ItemManager.RegisterItem(new CustomItem(Config.FlashlightName, Config.FlashlightDescription, Config.FlashlightPrice, new[] { ItemType.Flashlight }, new[] { "site27handcuffflashlight" }));

            // Event handler
            Exiled.Events.Handlers.Player.UseItem += OnUseItem;
        }

        public override void OnDisabled()
        {
            // Unregister
            ItemManager.UnregisterItem(Config.FlashlightName);

            // Remove event handlers
            Exiled.Events.Handlers.Player.UseItem -= OnUseItem;
        }

        private void OnUseItem(UseItemEventArgs ev)
        {
            // Check if the item being used is the custom flashlight
            if (ev.Item.Type == ItemType.Flashlight && ev.Item.Name == Config.FlashlightName)
            {
                // Get the player who used the item
                Player player = ev.Player;

                // Check if the player is looking at another player
                Player targetPlayer = player.GetLookingAtPlayer();
                if (targetPlayer != null)
                {
                    // is blud already cuffed?
                    if (!targetPlayer.IsCuffed)
                    {
                        // put cuffs on that man
                        targetPlayer.Cuff(player);

                        player.ShowHint(Config.HandcuffMessage.Replace("{targetPlayer}", targetPlayer.Nickname), Config.HandcuffHintDuration);

                        // Loser getting cuffedd
                        targetPlayer.ShowHint("You have been handcuffed by " + player.Nickname, Config.HandcuffHintDuration);
                    }
                    else
                    {
                        // yes
                        player.ShowHint(Config.AlreadyHandcuffedMessage.Replace("{targetPlayer}", targetPlayer.Nickname), Config.HandcuffHintDuration);
                    }
                }
                else
                {
                    // You cuffed him.. Nice job
                    player.ShowHint(Config.NoPlayerLookingAtMessage, Config.HandcuffHintDuration);
                }
            }
        }
    }
    // Config

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; }
        public string FlashlightName { get; set; }
        public string FlashlightDescription { get; set; }
        public int FlashlightPrice { get; set; }
        public string HandcuffMessage { get; set; }
        public string AlreadyHandcuffedMessage { get; set; }
        public string NoPlayerLookingAtMessage { get; set; }
        public float HandcuffHintDuration { get; set; }
    }
}

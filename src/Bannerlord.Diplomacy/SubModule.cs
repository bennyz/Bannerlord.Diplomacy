﻿using Bannerlord.ButterLib.Common.Extensions;
using Bannerlord.UIExtenderEx;

using Diplomacy.CampaignBehaviors;
using Diplomacy.Event;
using Diplomacy.Models;
using Diplomacy.PatchTools;

using Microsoft.Extensions.Logging;

using Serilog.Events;

using System.Linq;

using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Diplomacy
{
    public sealed class SubModule : MBSubModuleBase
    {
        public static readonly int VersionMajor = 0;
        public static readonly int VersionMinor = 1;
        public static readonly int VersionPatch = 1;
        public static readonly string Version = $"v{VersionMajor}.{VersionMinor}.{VersionPatch}";

        public static readonly string Name = typeof(SubModule).Namespace;
        public static readonly string DisplayName = Name;
        public static readonly string HarmonyDomain = "com.zijistark.bannerlord." + Name.ToLower();

        internal static readonly Color StdTextColor = Color.FromUint(0x00F16D26); // Orange

        internal static SubModule Instance { get; set; } = default!;

        private static ILogger Log { get; set; } = default!;

        private bool _hasLoaded;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            Instance = this;

            var extender = new UIExtender(Name);
            extender.Register(typeof(SubModule).Assembly);
            extender.Enable();

            this.AddSerilogLoggerProvider($"{Name}.log", new[] { $"{Name}.*" }, config => config.MinimumLevel.Is(LogEventLevel.Verbose));
            Log = LogFactory.Get<SubModule>();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            Log.LogInformation($"Unloaded {Name} {Version}!");
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            if (!_hasLoaded)
            {
                _hasLoaded = true;

                Log = LogFactory.Get<SubModule>(); // Upgrade to dedicated log file from closed service registry
                Log.LogInformation($"Loaded {Name} {Version}!");

                InformationManager.DisplayMessage(new InformationMessage($"Loaded {DisplayName}", StdTextColor));
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (game.GameType is Campaign)
            {
                PatchManager.PatchAll(HarmonyDomain);

                Events.Instance = new Events();
                var gameStarter = (CampaignGameStarter) gameStarterObject;

                gameStarter.AddBehavior(new DiplomaticAgreementBehavior());
                gameStarter.AddBehavior(new CooldownBehavior());
                gameStarter.AddBehavior(new MessengerBehavior());

                if (Settings.Instance!.EnableWarExhaustion)
                    gameStarter.AddBehavior(new WarExhaustionBehavior());

                if (Settings.Instance!.EnableFiefFirstRight)
                    gameStarter.AddBehavior(new KeepFiefAfterSiegeBehavior());

                gameStarter.AddBehavior(new AllianceBehavior());
                gameStarter.AddBehavior(new MaintainInfluenceBehavior());
                gameStarter.AddBehavior(new ExpansionismBehavior());
                gameStarter.AddBehavior(new CivilWarBehavior());
                gameStarter.AddBehavior(new UIBehavior());

                var currentKingdomDecisionPermissionModel = GetGameModel<KingdomDecisionPermissionModel>(gameStarterObject);
                if (currentKingdomDecisionPermissionModel is null)
                    Log.LogWarning("No default KingdomDecisionPermissionModel found!");

                gameStarter.AddModel(new DiplomacyKingdomDecisionPermissionModel(currentKingdomDecisionPermissionModel));

                Log.LogDebug("Campaign session started.");
            }
        }

        private T? GetGameModel<T>(IGameStarter gameStarterObject) where T : GameModel
        {
            var models = gameStarterObject.Models.ToArray();

            for (int index = models.Length - 1; index >= 0; --index)
            {
                if (models[index] is T gameModel1)
                    return gameModel1;
            }
            return default;
        }

        public override void OnGameEnd(Game game)
        {
            base.OnGameEnd(game);

            if (game.GameType is Campaign)
            {
                //PatchManager.UnpatchAll();

                Log.LogDebug("Campaign session ended.");
            }
        }
    }
}
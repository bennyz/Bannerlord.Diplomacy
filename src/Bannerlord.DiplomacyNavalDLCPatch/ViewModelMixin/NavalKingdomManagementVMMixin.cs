using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.ViewModels;

using Diplomacy.GauntletInterfaces;

using JetBrains.Annotations;

using NavalDLC.ViewModelCollection.Kingdom;

using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ScreenSystem;

namespace DiplomacyNavalDLCPatch.ViewModelMixin
{
    [ViewModelMixin]
    [UsedImplicitly]
    internal sealed class NavalKingdomManagementVMMixin : BaseViewModelMixin<NavalKingdomManagementVM>
    {
        private readonly RebelFactionsInterface _rebelFactionsInterface;

        [DataSourceProperty]
        public string FactionsLabel { get; set; }

        public NavalKingdomManagementVMMixin(NavalKingdomManagementVM vm) : base(vm)
        {
            _rebelFactionsInterface = new RebelFactionsInterface();
            FactionsLabel = new TextObject("{=gypPPxUJ}Factions").ToString();
        }

        public override void OnFinalize()
        {
            ViewModel!.Diplomacy.OnFinalize();
        }

        [DataSourceMethod]
        [UsedImplicitly]
        public void ExecuteShowFactions()
        {
            _rebelFactionsInterface.ShowInterface(ScreenManager.TopScreen, ViewModel!.Kingdom);
        }
    }
}
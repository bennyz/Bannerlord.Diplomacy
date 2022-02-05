﻿using Diplomacy.Costs;
using Diplomacy.DiplomaticAction.Conditioning;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace Diplomacy.DiplomaticAction.WarPeace.Conditions
{
    internal sealed class HasEnoughInfluenceForWarCondition : AbstractCostCondition
    {
        private static readonly TextObject FailedConditionText = new(StringConstants.NotEnoughInfluence);

        protected override bool CanPayCost(Kingdom kingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts = false, DiplomaticPartyType kingdomPartyType = DiplomaticPartyType.Proposer) =>
            DiplomacyCostCalculator.DetermineCostForDeclaringWar(kingdom, forcePlayerCharacterCosts, kingdomPartyType).CanPayCost();

        protected override TextObject GetFailedConditionText() => FailedConditionText;
    }
}
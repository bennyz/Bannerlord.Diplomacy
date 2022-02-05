﻿using Diplomacy.Costs;
using Diplomacy.DiplomaticAction.Conditioning;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Localization;

namespace Diplomacy.DiplomaticAction.NonAggressionPact
{
    internal sealed class HasEnoughGoldCondition : AbstractCostCondition
    {
        private static readonly TextObject FailedConditionText = new(StringConstants.NotEnoughGold);

        protected override bool CanPayCost(Kingdom kingdom, Kingdom otherKingdom, bool forcePlayerCharacterCosts = false, DiplomaticPartyType kingdomPartyType = DiplomaticPartyType.Proposer) =>
            DiplomacyCostCalculator.DetermineGoldCostForFormingNonAggressionPact(kingdom, otherKingdom, forcePlayerCharacterCosts, kingdomPartyType).CanPayCost();

        protected override TextObject GetFailedConditionText() => FailedConditionText;
    }
}
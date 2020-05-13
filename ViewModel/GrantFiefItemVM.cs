﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace DiplomacyFixes.ViewModel
{
    class GrantFiefItemVM : TaleWorlds.Library.ViewModel
    {
        public Settlement Settlement { get; private set; }
        private int _prosperity;
        private string _settlementImagePath;
        private string _name;
        private int _defenders;
        private string _iconPath;
        private bool _isSelected;
        private readonly Action<GrantFiefItemVM> _onSelect;

        public GrantFiefItemVM(Settlement settlement, Action<GrantFiefItemVM> onSelect)
        {
            this.Settlement = settlement;
            this.Name = settlement.Name.ToString();
            SettlementComponent component = settlement.GetComponent<SettlementComponent>();
            this.SettlementImagePath = ((component == null) ? "placeholder" : (component.BackgroundMeshName + "_t"));
            Town component2 = settlement.GetComponent<Town>();
            if (component2 != null)
            {
                this.Prosperity = MBMath.Round(component2.Prosperity);
                this.IconPath = component2.BackgroundMeshName;
            }
            else if (settlement.IsCastle)
            {
                this.Prosperity = MBMath.Round(settlement.Prosperity);
                this.IconPath = "";
            }
            this.Garrison = this.Settlement.Town.GarrisonParty?.Party.NumberOfAllMembers ?? 0;
            this._onSelect = onSelect;
        }

        public void ExecuteLink()
        {
            Campaign.Current.EncyclopediaManager.GoToLink(this.Settlement.EncyclopediaLink);
        }

        public void OnSelect()
        {
            this._onSelect(this);
        }

        [DataSourceProperty]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (value != this._isSelected)
                {
                    this._isSelected = value;
                    base.OnPropertyChanged("IsSelected");
                }
            }
        }

        [DataSourceProperty]
        public string IconPath
        {
            get
            {
                return this._iconPath;
            }
            set
            {
                if (value != this._iconPath)
                {
                    this._iconPath = value;
                    base.OnPropertyChanged("IconPath");
                }
            }
        }

        [DataSourceProperty]
        public int Garrison
        {
            get
            {
                return this._defenders;
            }
            set
            {
                if (value != this._defenders)
                {
                    this._defenders = value;
                    base.OnPropertyChanged("Defenders");
                }
            }
        }

        [DataSourceProperty]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    base.OnPropertyChanged("Name");
                }
            }
        }

        [DataSourceProperty]
        public int Prosperity
        {
            get
            {
                return this._prosperity;
            }
            set
            {
                if (value != this._prosperity)
                {
                    this._prosperity = value;
                    base.OnPropertyChanged("Prosperity");
                }
            }
        }

        [DataSourceProperty]
        public string SettlementImagePath
        {
            get
            {
                return this._settlementImagePath;
            }
            set
            {
                if (value != this._settlementImagePath)
                {
                    this._settlementImagePath = value;
                    base.OnPropertyChanged("SettlementImagePath");
                }
            }
        }
    }
}
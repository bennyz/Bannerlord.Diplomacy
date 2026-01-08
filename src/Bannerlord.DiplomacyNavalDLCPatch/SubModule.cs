using Bannerlord.UIExtenderEx;

using TaleWorlds.MountAndBlade;

namespace DiplomacyNavalDLCPatch
{
    public class SubModule : MBSubModuleBase
    {
        public static readonly string Name = typeof(SubModule).Namespace!;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            var extender = UIExtender.Create(Name);
            extender.Register(typeof(SubModule).Assembly);
            extender.Enable();
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();
        }
    }
}
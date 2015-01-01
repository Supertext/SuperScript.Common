using System.Configuration;

namespace SuperScript.Configuration
{
    public class EmitDependentModifierElement : ModifierElement
    {
        [ConfigurationProperty("emitMode", IsRequired = false, DefaultValue = EmitMode.Always)]
        public EmitMode EmitMode
        {
            get { return (EmitMode) this["emitMode"]; }
        }
    }
}
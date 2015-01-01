using System.Configuration;

namespace SuperScript.Configuration
{
    public class EmitDependentModifiersCollection : ModifiersCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EmitDependentModifierElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmitDependentModifierElement) element).Type;
        }

        public new EmitDependentModifierElement this[string name]
        {
            get { return (EmitDependentModifierElement) BaseGet(name); }
        }
    }
}
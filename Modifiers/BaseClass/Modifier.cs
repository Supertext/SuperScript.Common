namespace SuperScript.Modifiers
{
	public abstract class ModifierBase
	{
		internal EmitMode _emitMode = EmitMode.Always;

		/// <summary>
		/// <para>Determines the mode in which this implementation emits contents.</para>
		/// <para>The default is EmitMode.Always.</para>
		/// </summary>
		public EmitMode EmitMode
		{
			get { return _emitMode; }
			set { _emitMode = value; }
		}
	}
}
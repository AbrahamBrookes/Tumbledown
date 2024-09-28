using Tumbledown.Abilities;

namespace Tumbledown {

	/**
	 * Our iSlashable interface enforces a contract for components to implement - when they player
	 * slashes, if this gameobject is in the list of slashable objects, we call the slash method
	 * on the component, receiving the Attacker at that moment.
	 */
	
	public interface iSlashable
	{
		void BeSlashed(Attacker attacker);
	}
}
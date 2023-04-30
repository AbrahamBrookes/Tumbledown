using UnityEngine;

public class Attacker : MonoBehaviour
{
	// the attack collider is a collider that overlaps the weapon mesh
	[SerializeField] private GameObject _attackCollider;

	// we need a reference to the animator so we can make animation calls
	[SerializeField] private Animator _animator;

	// a reference to the protagonist script, so we can disable movement while attacking
	[SerializeField] private Protagonist _protagonist;

	// on start, cache deps
	private void Start()
	{
		_animator = GetComponent<Animator>();
		_protagonist = GetComponent<Protagonist>();
	}

	public void EnableWeapon()
	{
		_attackCollider.SetActive(true);
	}

	public void DisableWeapon()
	{
		_attackCollider.SetActive(false);
	}

	public void Attack()
	{

		// toggle on the attack collider
		EnableWeapon();

		// disable input while attacking
		_protagonist.SetInputMapping(typeof(CurrentlyAttackingInputMapping));

		// play the attack animation
		_animator.SetBool("IsAttacking", true);
		_animator.Play("Attack", 0, 0f);

		// after a frame, flick the trigger off
		Invoke("DisableWeapon", 0.01f);
	}

	// called from an animation event, or when attacking is interrupted
	public void FinishAttacking()
	{
		// toggle off the attack collider
		DisableWeapon();

		// flick back to the general gameplay input mapping
		_protagonist.SetInputMapping(typeof(GeneralGameplayInputMapping));

		_animator.SetBool("IsAttacking", false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown.Collectables
{
	/**
	 * A Floating Feather is the most brazen of HiddenFeather variants - it just floats there and
	 * when you run into it you pick it up. It's not hidden at all! (But it may be hard to get to)
	 */
    public class FloatingFeather : MonoBehaviour
    {
		// We must have a HiddenFeather attached as well
		private HiddenFeather _hiddenFeather;

        // Start is called before the first frame update
        void Start()
        {
			// make sure we have a HiddenFeather
			_hiddenFeather = GetComponent<HiddenFeather>();

			if (_hiddenFeather == null)
			{
				Debug.LogError("FloatingFeather does not have a HiddenFeather attached!");

				// disable this component
				enabled = false;

				return;
			}
        }

		// When the player runs into this feather, we want to collect it
		private void OnTriggerEnter(Collider other) {
			// make sure we collided with the player
			if (other.gameObject.tag != "Player")
			{
				return;
			}

			// collect the feather
			_hiddenFeather.Collect();
		}
    }
}

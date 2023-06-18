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
		public HiddenFeather hiddenFeather;
		
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

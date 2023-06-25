using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tumbledown
{
	// we need a struct for tracking the location of a given xyz
	[System.Serializable]
	public struct WorldPegLocation
	{
		public bool isLeft;
		public bool isRight;
		public bool isFront;
		public bool isBack;
		public bool isTop;
	}
}

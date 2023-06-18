using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tumbledown.DataPersistence;

namespace Tumbledown.DataPersistence.Tests
{
    public class TestSaveData : SaveData
    {
		public int testInt;
		public string testString;
		public float testFloat;
		public bool testBool;

		public TestSaveData(int testInt, string testString, float testFloat, bool testBool)
		{
			this.testInt = testInt;
			this.testString = testString;
			this.testFloat = testFloat;
			this.testBool = testBool;
		}
    }
}

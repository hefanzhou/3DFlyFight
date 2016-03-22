using UnityEngine;
using System.Collections;

public class TestOverride : MonoBehaviour {

	// Use this for initialization
	void Start () {
        class3 value = new class3();
        value.DoSomething3();
	}
	

}

public class class1
{
   protected virtual void print()
    {
        Debug.Log("class1");
    }
}

public class class2 : class1
{
    public void DoSomething()
    {
        print();
    }

    protected override void print()
    {
        //Debug.Log("class2");
        base.print();
    }
}

public class class3 : class2
{
    protected override void print()
    {
        Debug.Log("class3");
    }

    public void DoSomething3()
    {
        base.print();
        print();
    }
}

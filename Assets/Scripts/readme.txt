STYLING GUIDE
------------------------------

This should hopefully provide a breif explanation of why classes are styled the way they are.

------------------------------

public class Foo
{
	public event System.Action EventsComeFirst;

	public int PropertiesSecond { get; private set; }

	[SerializeField] private bool m_membersHaveTheMPrefix = true;

	public void MostObviousFunction()
	{
		// Public functions usually come first
		// ...

		HelpExplain();
	}

	private void HelpExplain()
	{
		// Helper functions should normally come directly below
			// their first invokation 
			// ...
	}
}
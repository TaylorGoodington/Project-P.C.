using UnityEngine;

public class CombatEngine : MonoBehaviour {
	
	public static CombatEngine combatEngine;

	void Start () {
		combatEngine = GetComponent<CombatEngine>();
	}
	
	void Update () {
	
	}
	
	public void Attacking () {
		Debug.Log ("We're Attacking Now!");
	}
}

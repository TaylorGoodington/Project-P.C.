using UnityEngine;
using System.Collections.Generic;

public class SkillsController : MonoBehaviour {

    public static SkillsController skillsController;
    private SkillsDatabase skillsDatabase;
    public List<Skills> acquiredSkills;
    public List<Skills> activeSkills;

	void Start () {
        skillsController = GetComponent<SkillsController>();
        skillsDatabase = GetComponent<SkillsDatabase>();
	}
	
	void Update () {
	
	}

    //I think skills should have another key that allows me to add skills of the same together in the active skills list. So an example could be
    // dodge 1, 2, & 3 which have different names and different IDs, but the same group number. So the method will check if a skill in that group
    // exists and the trigger rates.

    //I need to have a few different damage methods. The first will be a damage that can be avoided, the second will be a damage that cant be avoided.
}

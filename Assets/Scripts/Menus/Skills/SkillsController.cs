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

    //I need to have a few different damage methods. The first will be a damage that can be avoided, the second will be a damage that cant be avoided.
}

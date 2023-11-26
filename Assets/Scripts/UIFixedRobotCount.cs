using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class  UIFixedRobotCount : MonoBehaviour
{

    float originalSize;
    

    public GameObject textFixedRobots;
    public GameObject textCount;

    Text text_fixed_robots;
    Text text_count;

    public string fixedRobots;
    public int fixedRobotsCount;

    public EnemyController enemyController;

    /* void Awake()
     {
         instance = this;
     }
    */
    public void Start()
    {
        text_fixed_robots = textFixedRobots.GetComponent<Text>();
        text_count = textCount.GetComponent<Text>();

        fixedRobotsCount = 0;
    }

    public void Update()
    {
        text_fixed_robots.text = fixedRobots;
        text_count.text = fixedRobotsCount.ToString();
    }

}

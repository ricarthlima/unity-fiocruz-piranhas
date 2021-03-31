using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class BackgroundController : MonoBehaviour
{
    private UnityArmatureComponent piranhaSwiming;
    private bool isPiranhaGoingRight;

    [SerializeField]
    private List<GameObject> listWaterObjects;

    [SerializeField]
    private List<GameObject> listTreeObjects;

    private void Start()
    {
        isPiranhaGoingRight = true;

        UnityFactory.factory.LoadDragonBonesData("Piranha/animations-fiocruz_ske");
        UnityFactory.factory.LoadTextureAtlasData("Piranha/animations-fiocruz_tex");

        var seaweedFlowerComponent = UnityFactory.factory.BuildArmatureComponent("SeaweedVegetation");
        seaweedFlowerComponent.transform.localScale = new Vector3(0.45f, 0.45f, 0f);
        seaweedFlowerComponent.transform.localPosition = new Vector3(0f, -4.3f, 0f);
        seaweedFlowerComponent.animation.Play("idle");

        piranhaSwiming = UnityFactory.factory.BuildArmatureComponent("Piranha");
        piranhaSwiming.transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
        piranhaSwiming.transform.localPosition = new Vector3(-9.5f, -3f, 0);
        piranhaSwiming.animation.Play("swimming");
    }

    // Update is called once per frame
    private void Update()
    {
        MovePiranha();
    }

    private void FixedUpdate()
    {
        MoveWater();
        //MoveTree();
    }

    private void MoveTree()
    {
        for (int i = 0; i < listTreeObjects.Count; i++)
        {
            GameObject tree = listTreeObjects[i];

            //Scale
            Vector3 scale = tree.transform.localScale;
            float gain = 0.04f * Time.deltaTime;
            tree.transform.localScale += new Vector3(scale.x * gain, scale.y * gain, scale.z * Time.deltaTime);

            //Move X
            float gainPosX = 0.3f * Time.deltaTime;
            if (i % 2 == 0)
            {
                gainPosX = gainPosX * -1;
            }
            tree.transform.localPosition += transform.right * gainPosX;

            //Move Y
            float gainPosY = 0.3f * Time.deltaTime;
            tree.transform.localPosition += -transform.up * gainPosY;

            //To reset
            if (i % 2 == 0)
            {
            }
            else
            {
                if (tree.transform.position.x >= 10 || tree.transform.position.x <= 2.2 || scale.x >= 1.3)
                {
                    tree.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    tree.transform.position = new Vector3(5f, 1f, 13.79f);
                    tree.GetComponent<SpriteRenderer>().sortingOrder = 3;
                }
            }
        }
    }

    private void MoveWater()
    {
        listWaterObjects[0].transform.position += transform.right * 1.4f * Time.deltaTime;
        listWaterObjects[1].transform.position += transform.right * 2f * Time.deltaTime;

        foreach (GameObject waterObject in listWaterObjects)
        {
            if (waterObject.transform.localPosition.x >= 9.19f)
            {
                waterObject.transform.localPosition = new Vector3(-11.25f, waterObject.transform.localPosition.y, waterObject.transform.localPosition.z);
            }
        }
    }

    private void MovePiranha()
    {
        if (isPiranhaGoingRight && piranhaSwiming.transform.localPosition.x < 9.5f)
        {
            piranhaSwiming.transform.localPosition += transform.right * 1.2f * Time.deltaTime;
            if (piranhaSwiming.transform.localPosition.x >= 9.5f)
            {
                isPiranhaGoingRight = false;
                piranhaSwiming.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            }
        }

        if (!isPiranhaGoingRight && piranhaSwiming.transform.localPosition.x > -9.5f)
        {
            piranhaSwiming.transform.localPosition += -transform.right * 1.2f * Time.deltaTime;
            if (piranhaSwiming.transform.localPosition.x <= -9.5f)
            {
                isPiranhaGoingRight = true;
                piranhaSwiming.transform.localScale = new Vector3(-0.25f, 0.25f, 0.25f);
            }
        }
    }
}
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

    private float newZ = 0;

    private void Start()
    {
        isPiranhaGoingRight = true;

        UnityFactory.factory.LoadDragonBonesData("Piranha/animations-fiocruz_ske");
        UnityFactory.factory.LoadTextureAtlasData("Piranha/animations-fiocruz_tex");

        var seaweedFlowerComponent = UnityFactory.factory.BuildArmatureComponent("SeaweedVegetation");
        seaweedFlowerComponent.transform.localScale = new Vector3(0.45f, 0.45f, 0f);
        seaweedFlowerComponent.transform.localPosition = new Vector3(-1f, -4.3f, 0f);
        seaweedFlowerComponent.animation.Play("idle");

        piranhaSwiming = UnityFactory.factory.BuildArmatureComponent("Piranha");
        piranhaSwiming.transform.localScale = new Vector3(-0.25f, 0.25f, 1f);
        piranhaSwiming.transform.localPosition = new Vector3(-9.5f, -3f, 0);
        piranhaSwiming.animation.Play("swimming");

        MakeTree();
    }

    // Update is called once per frame
    private void Update()
    {
        MovePiranha();
        MoveWater();
    }

    private void MakeTree()
    {
        this.newZ += 0.0001f;
        int factor = 1;

        for (int i = 0; i < 2; i++)
        {
            UnityArmatureComponent tree = UnityFactory.factory.BuildArmatureComponent("Tree");

            tree.transform.position = new Vector3(3.5f * factor, 1.65f, newZ);
            tree.transform.localScale = new Vector3(1f * factor, 1f, 1f);

            tree.animation.Play("movingTree");
            tree.name = "Tree" + newZ.ToString();
            StartCoroutine("DestroyTree", tree);
            factor = -1;
        }

        Invoke("MakeTree", 1.5f);
    }

    public void gameStarted()
    {
        piranhaSwiming.gameObject.SetActive(false);
    }

    private IEnumerator DestroyTree(UnityArmatureComponent tree)
    {
        yield return new WaitForSeconds(11.5f);
        Destroy(tree.gameObject);
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
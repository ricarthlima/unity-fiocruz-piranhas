using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class BackgroundComponentsController : MonoBehaviour
{
    // Start is called before the first frame update
    private List<UnityArmatureComponent> listSeaweed;

    private void Start()
    {
        float baseX = -5.5f;
        float baseY = -4.5f;
        float baseZ = 0f;

        float baseCount = 1;

        listSeaweed = new List<UnityArmatureComponent>();

        UnityFactory.factory.LoadDragonBonesData("Piranha/animations-fiocruz_ske");
        UnityFactory.factory.LoadTextureAtlasData("Piranha/animations-fiocruz_tex");

        var seaweedCurvedComponent = UnityFactory.factory.BuildArmatureComponent("SeaweedCurved");
        var seaweedFlowerComponent = UnityFactory.factory.BuildArmatureComponent("SeaweedFlower");
        var seaweedSmallComponent = UnityFactory.factory.BuildArmatureComponent("SeaweedSmall");

        seaweedCurvedComponent.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        seaweedFlowerComponent.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);

        seaweedSmallComponent.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        listSeaweed.Add(seaweedCurvedComponent);
        listSeaweed.Add(seaweedFlowerComponent);
        listSeaweed.Add(seaweedSmallComponent);

        foreach (UnityArmatureComponent comp in listSeaweed)
        {
            comp.transform.localPosition = new Vector3(baseX * baseCount, baseY, baseZ);
            comp.animation.Play("idle");
            baseCount += 0.05f;
        }

        var piranha = UnityFactory.factory.BuildArmatureComponent("Piranha");
        piranha.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        piranha.transform.localPosition = new Vector3(7.1f, -3.7f, 0);
        piranha.animation.Play("swimming");

        var piranha2 = UnityFactory.factory.BuildArmatureComponent("Piranha");
        piranha2.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        piranha2.transform.localPosition = new Vector3(3.04f, -1.31f, 0);
        piranha2.animation.Play("bite");
    }

    // Update is called once per frame
    private void Update()
    {
        foreach (UnityArmatureComponent comp in listSeaweed)
        {
            comp.transform.localPosition = new Vector3(comp.transform.localPosition.x + 0.0003f, comp.transform.localPosition.y, comp.transform.localPosition.z);
        }
    }
}
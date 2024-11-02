using BepInEx;
using TrailsFX;
using UnityEngine;
using UnityEngine.SceneManagement;

[BepInPlugin("ShadowSkin", "random cool skin", "1.0.0")]
public class SinModelSwap : BaseUnityPlugin
{
    private GameObject senHumanbot;
    private GameObject sinHumanbot;
    private GameObject senUpperCape;
    private GameObject senLowerCape;
    private GameObject senNeck;
    private GameObject senCorrupt;

    private const string beegHatPath = "World/Areas/IronFactory/Lod0/F5/RobotArmature/Body/Torso/Chest/Neck/Hhead/Corrupt/BeegHat";
    private float scaleFactor = 37.5f;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BirdCage")
        {
            FindGameObjects();

            if (senHumanbot != null && sinHumanbot != null && senUpperCape != null && senLowerCape != null && senNeck != null && senCorrupt != null)
            {
                PerformModelSwap();
                CopyAndPlaceTops();
                GiveF5Hat();
                UpdateTrailFX();
                SetHumanbotMaterialColor();
                DisableSenCorrupt();
            }
        }
    }

    void FindGameObjects()
    {
        senHumanbot = GameObject.Find("S-105.1/Humanbot_A_Geom");
        sinHumanbot = GameObject.Find("World/Areas/ERI Grave/SkyBase/bossFight/S-104/Humanbot_A_Geom");
        senUpperCape = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/UpperCape");
        senLowerCape = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/UpperCape/LowerCape");
        senNeck = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/Neck");
        senCorrupt = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1/Corrupt");
    }

    void PerformModelSwap()
    {
        ReplaceMaterials(senHumanbot, sinHumanbot);
        ReplaceSharedMesh(senHumanbot, sinHumanbot);
        DisableUpperCape();
        DisableLowerCape();
        SetSinMaterialColor();
    }

    void ReplaceMaterials(GameObject senObj, GameObject sinObj)
    {
        SkinnedMeshRenderer senRenderer = senObj.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer sinRenderer = sinObj.GetComponent<SkinnedMeshRenderer>();

        if (senRenderer != null && sinRenderer != null)
        {
            Material sinMaterial = sinRenderer.material;
            senRenderer.material = Instantiate(sinMaterial);
        }
    }

    void ReplaceSharedMesh(GameObject senObj, GameObject sinObj)
    {
        SkinnedMeshRenderer senRenderer = senObj.GetComponent<SkinnedMeshRenderer>();
        SkinnedMeshRenderer sinRenderer = sinObj.GetComponent<SkinnedMeshRenderer>();

        if (senRenderer != null && sinRenderer != null)
        {
            Mesh sinMesh = sinRenderer.sharedMesh;
            senRenderer.sharedMesh = sinMesh;
        }
    }

    void DisableUpperCape()
    {
        if (senUpperCape != null)
        {
            Renderer upperCapeRenderer = senUpperCape.GetComponent<Renderer>();
            if (upperCapeRenderer != null)
            {
                upperCapeRenderer.enabled = false;
            }
        }
    }

    void DisableLowerCape()
    {
        if (senLowerCape != null)
        {
            Renderer lowerCapeRenderer = senLowerCape.GetComponent<Renderer>();
            if (lowerCapeRenderer != null)
            {
                lowerCapeRenderer.enabled = false;
            }
        }
    }

    void SetSinMaterialColor()
    {
        SkinnedMeshRenderer sinRenderer = sinHumanbot.GetComponent<SkinnedMeshRenderer>();
        if (sinRenderer != null)
        {
            sinRenderer.material.color = new Color(0, 0, 0, 0);
        }

        GameObject sinTops = GameObject.Find("World/Areas/ERI Grave/SkyBase/bossFight/S-104/ROOT/Hips/Spine/Spine1/Tops_14");
        if (sinTops != null)
        {
            SkinnedMeshRenderer sinTopsRenderer = sinTops.GetComponent<SkinnedMeshRenderer>();
            if (sinTopsRenderer != null)
            {
                sinTopsRenderer.material.color = new Color(0, 0, 0, 0);
            }
        }
    }

    void CopyAndPlaceTops()
    {
        GameObject sinTops = GameObject.Find("World/Areas/ERI Grave/SkyBase/bossFight/S-104/ROOT/Hips/Spine/Spine1/Tops_14");

        if (sinTops != null)
        {
            GameObject newTops = Instantiate(sinTops);
            newTops.name = "Tops_14(Clone)";

            Transform senSpine1 = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1").transform;
            if (senSpine1 != null)
            {
                newTops.transform.SetParent(senSpine1, false);

                newTops.transform.localPosition = sinTops.transform.localPosition;
                newTops.transform.localRotation = sinTops.transform.localRotation;

                SkinnedMeshRenderer newTopsRenderer = newTops.GetComponent<SkinnedMeshRenderer>();
                if (newTopsRenderer != null)
                {
                    newTopsRenderer.material.color = new Color(0, 0, 0, 0);
                }
            }
            else
            {
                Debug.LogError("Sen's Spine1 not found!");
            }
        }
        else
        {
            Debug.LogError("Tops_14 not found in Sin's directory!");
        }
    }

    void GiveF5Hat()
    {
        GameObject beegHat = GameObject.Find(beegHatPath);
        if (beegHat != null)
        {
            GameObject newHat = Instantiate(beegHat);
            newHat.name = "F5Hat";

            Transform daSkirt = newHat.transform.Find("SM_Prop_Medical_Scanner_01 (1)/DA_Skirt (1)");
            if (daSkirt != null)
            {
                daSkirt.gameObject.SetActive(false);
            }

            Transform senSpine1 = GameObject.Find("S-105.1/ROOT/Hips/Spine/Spine1").transform;
            if (senSpine1 != null)
            {
                newHat.transform.SetParent(senSpine1, false);
                UpdateHatTransform(newHat);
            }
        }
    }

    void UpdateHatTransform(GameObject hat)
    {
        hat.transform.localPosition = new Vector3(0f, 0.5567f, -0.0272f);
        hat.transform.localRotation = Quaternion.Euler(270f, 328.634f, 0f);
        hat.transform.localScale = new Vector3(scaleFactor, scaleFactor, 63.4113f);
        SetHatColorToBlack(hat);
    }

    void SetHatColorToBlack(GameObject hat)
    {
        MeshRenderer meshRenderer = hat.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Material hatMaterial = meshRenderer.material;
            if (hatMaterial != null)
            {
                hatMaterial.color = new Color(0f, 0f, 0f, 0f);
            }
        }
    }

    void UpdateTrailFX()
    {
        var trailComponent = senHumanbot.GetComponent<TrailEffect>();
        if (trailComponent != null)
        {
            trailComponent.enabled = true;
            trailComponent.active = true;
            trailComponent.color = new Color(0f, 0f, 0f, 1f);
            Debug.Log("Updated TrailsEffect: enabled, _active set to true, and color set to (0, 0, 0, 1).");
        }
        else
        {
            Debug.LogError("TrailEffect component not found on Humanbot_A_Geom!");
        }
    }
    void SetHumanbotMaterialColor()
    {
        SkinnedMeshRenderer senRenderer = senHumanbot.GetComponent<SkinnedMeshRenderer>();
        if (senRenderer != null)
        {
            senRenderer.material.color = new Color(0, 0, 0, 0);
        }
    }

    void DisableSenCorrupt()
    {
        if (senCorrupt != null)
        {
            senCorrupt.SetActive(false);
        }
    }
}
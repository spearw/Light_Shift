using UnityEngine.Experimental.Rendering.LWRP;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public Material standardMaterial;
    public Material staticMaterial;
    public Material blueMaterial;
    public Material redMaterial;
    public Material yellowMaterial;
    public Material greenMaterial;
    public Material purpleMaterial;
    public Material cyanMaterial;

    public PhysicsMaterial2D purpleBouncy;

    public bool redShift = true;
    public bool greenShift = true;
    public bool blueShift = true;

    IDictionary<string, float> standardBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };
    IDictionary<string, float> blueBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 6},
                        {"gravity", -0.6f},
                        {"drag", 0.1f}
                    };
    IDictionary<string, float> greenBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 0},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };
    IDictionary<string, float> redBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 1},
                        {"gravity", 0},
                        {"drag", 0.1f}
                    };
    IDictionary<string, float> yellowBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 5},
                        {"gravity", 0},
                        {"drag", 1000}
                    };
    IDictionary<string, float> cyanBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 9},
                        {"pickup?", 1},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };
    IDictionary<string, float> purpleBlockDict = new Dictionary<string, float>()
                    {
                        {"layer", 10},
                        {"pickup?", 1},
                        {"mass", 10},
                        {"gravity", 5},
                        {"drag", 1}
                    };

    private GameObject[] standardBlocks;
    private GameObject[] blueBlocks;
    private GameObject[] greenBlocks;
    private GameObject[] redBlocks;
    private GameObject[] yellowBlocks;
    private GameObject[] cyanBlocks;
    private GameObject[] purpleBlocks;
    public UnityEngine.Experimental.Rendering.Universal.Light2D globalLight;

    void Start()
    {
        standardBlocks = GameObject.FindGameObjectsWithTag("StandardBlockChild");
        blueBlocks = GameObject.FindGameObjectsWithTag("BlueBlockChild");
        greenBlocks = GameObject.FindGameObjectsWithTag("GreenBlockChild");
        redBlocks = GameObject.FindGameObjectsWithTag("RedBlockChild");
        yellowBlocks = GameObject.FindGameObjectsWithTag("YellowBlockChild");
        cyanBlocks = GameObject.FindGameObjectsWithTag("CyanBlockChild");
        purpleBlocks = GameObject.FindGameObjectsWithTag("PurpleBlockChild");
    }

    void shift(GameObject[] blocks, IDictionary<string, float> blockBlueprint, Material newMaterial, Color color, PhysicsMaterial2D physicsMaterial = null){
        foreach(GameObject blockChild in blocks){
            var block = blockChild.transform.parent.gameObject;
            var rb = block.GetComponent<Rigidbody2D>();

            //color
            block.GetComponent<MeshRenderer>().material = newMaterial;
            //light color
            block.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().color = color;
            //layer
            block.layer = (int)blockBlueprint["layer"];
            //pickup
            if (blockBlueprint["pickup?"] == 1){
                block.tag = "pickup";
            }
            else {block.tag = "nopickup";
            }
            //optional physics material
            block.GetComponent<BoxCollider2D>().sharedMaterial = physicsMaterial;
            //physics settings
            rb.mass = blockBlueprint["mass"];
            rb.gravityScale = blockBlueprint["gravity"];
            rb.drag = blockBlueprint["drag"];
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Red shift
        if(Input.GetButtonDown("RedShift") && redShift){

            var red = new Color(1, 0.6f, 0.6f);
            globalLight.color = red;

            //standard to red
            shift(standardBlocks, redBlockDict, redMaterial, Color.red);
            //blue to purple
            shift(blueBlocks, purpleBlockDict, purpleMaterial, Color.magenta, purpleBouncy);
            //green to yellow
            shift(greenBlocks, yellowBlockDict, yellowMaterial, Color.yellow);
            //red to red
            shift(redBlocks, redBlockDict, redMaterial, Color.red);
            //cyan to white
            shift(cyanBlocks, standardBlockDict, staticMaterial, Color.white);
            //purple to purple
            shift(purpleBlocks, purpleBlockDict, purpleMaterial, Color.magenta, purpleBouncy);
            //yellow to yellow
            shift(yellowBlocks, yellowBlockDict, yellowMaterial, Color.yellow);
        }
        //Green shift
        if(Input.GetButtonDown("GreenShift") && greenShift){

            var green = new Color(0.6f, 1, 0.6f);
            globalLight.color = green;

            //standard to green
            shift(standardBlocks, greenBlockDict, greenMaterial, Color.green);
            //red to yellow
            shift(redBlocks, yellowBlockDict, yellowMaterial, Color.yellow);
            //blue to cyan
            shift(blueBlocks, cyanBlockDict, cyanMaterial, Color.cyan);
            //green to green
            shift(greenBlocks, greenBlockDict, greenMaterial, Color.green);
            //purple to white
            shift(purpleBlocks, standardBlockDict, staticMaterial, Color.white);
            //yellow to yellow
            shift(yellowBlocks, yellowBlockDict, yellowMaterial, Color.yellow);
            //cyan to cyan
            shift(cyanBlocks, cyanBlockDict, cyanMaterial, Color.cyan);
        }
        //Blue Shift
        if(Input.GetButtonDown("BlueShift") && blueShift){

            var blue = new Color(0.4f, 0.6f, 1);
            globalLight.color = blue;

            //standard to blue
            shift(standardBlocks, blueBlockDict, blueMaterial, Color.blue);
            //red to purple
            shift(redBlocks, purpleBlockDict, purpleMaterial, Color.magenta, purpleBouncy);
            //green to cyan
            shift(greenBlocks, cyanBlockDict, cyanMaterial, Color.cyan);
            //blue to blue
            shift(blueBlocks, blueBlockDict, blueMaterial, Color.blue);
            //yellow to white
            shift(yellowBlocks, standardBlockDict, staticMaterial, Color.white);
            //cyan to cyan
            shift(cyanBlocks, cyanBlockDict, cyanMaterial, Color.cyan);
            //purple to purple
            shift(purpleBlocks, purpleBlockDict, purpleMaterial, Color.magenta, purpleBouncy);
        }
        //Generic unshift 
        if(Input.GetButtonDown("UnShift")){

            globalLight.color = Color.white;

            shift(standardBlocks, standardBlockDict, standardMaterial, Color.black);
            shift(redBlocks, redBlockDict, redMaterial, Color.red);
            shift(blueBlocks, blueBlockDict, blueMaterial, Color.blue);
            shift(greenBlocks, greenBlockDict, greenMaterial, Color.green);
            shift(yellowBlocks, yellowBlockDict, yellowMaterial, Color.yellow);
            shift(cyanBlocks, cyanBlockDict, cyanMaterial, Color.cyan);
            shift(purpleBlocks, purpleBlockDict, purpleMaterial, Color.magenta, purpleBouncy);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public class PokedexHandler : MonoBehaviour {

	public Text primary;
	public Text primaryShadow;
	public Text secondary;
	public Text secondaryShadow;
	public Text tertiary;
	public Text tertiaryShadow;

	public GameObject player;
	public float activeID;



	public GameObject cancelButton;
	public Sprite cancelButtonUnselected;
	public Sprite cancelButtonSelected;

	public float cursor;

	public Image preview;
	public Sprite spritesheet;

	public Image type1;
	public Image type2;
	public Sprite typesheet;

	// Use this for initialization
	void Start () {
		
	}

	void Awake(){
		activeID = 1f;
		cursor = 1f;
		player = GameObject.FindGameObjectWithTag ("Player");
		player.GetComponent<PlayerMovement> ().canInput = false;
	}

	void OnDisable(){
		player.GetComponent<PlayerMovement> ().canInput = true;
	}
	


	public IEnumerator control(){
		cursor = 1;
		setText (1);
		StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.defaultSpeed));
		bool running = true;
		while (running) {


			// Cursor Movement
			if (Input.GetAxis ("Horizontal") < 0 && cursor > 1f) {
				cursor -= 1f;
				yield return new WaitForSeconds (0.2f);

			} else if (Input.GetAxis ("Horizontal") > 0 && cursor < 2f) {
				cursor += 1f;
				yield return new WaitForSeconds (0.2f);

			}

			if (Input.GetButton ("Back")) {
				cancelButton.GetComponent <Image>().sprite = cancelButtonSelected;
				yield return new WaitForSeconds (0.2f);
				running = false;
			}

			if (cursor == 1) {
				//Scrolling through list
				if (Input.GetAxis ("Vertical") > 0 && activeID > 1f) {
					activeID -= 1f;
					setText ((int)activeID);
					yield return new WaitForSeconds (0.2f);
				} else if (Input.GetAxis ("Vertical") < 0 && activeID < PokemonDatabase.getPokedexLength () - 1) {
					activeID += 1f;
					setText ((int)activeID);
					yield return new WaitForSeconds (0.2f);
				}

				cancelButton.GetComponent <Image>().sprite = cancelButtonUnselected;
			} else if (cursor == 2) {
				// Exiting
				cancelButton.GetComponent <Image>().sprite = cancelButtonSelected;
				if (Input.GetButton("Select")){
					running = false;


				}
			}
			yield return null;
		}

		StopCoroutine("animateIcons");
		//yield return new WaitForSeconds(sceneTransition.FadeOut());
		yield return StartCoroutine(ScreenFade.main.Fade(false, ScreenFade.defaultSpeed));
		GlobalVariables.global.resetFollower();
		this.gameObject.SetActive(false);

	}

	// Usability Functions

	private void setText(int id){
		if (id != 1f) {
			tertiary.transform.parent.gameObject.SetActive (true);
			string name0 = PokemonDatabase.getPokemonbyID (id - 1).getName () + " " + toNum (id - 1);
			tertiary.text = name0;
			tertiaryShadow.text = name0;
		} else {
			tertiary.transform.parent.gameObject.SetActive (false);
		}

		string name = PokemonDatabase.getPokemonbyID(id).getName () + " " + toNum(id);
		primary.text = name;
		primaryShadow.text = name;

		if (id != PokemonDatabase.getPokedexLength () - 1) {
			secondary.transform.parent.gameObject.SetActive (true);
			string name0 = PokemonDatabase.getPokemonbyID (id + 1).getName () + " " + toNum (id + 1);
			secondary.text = name0;
			secondaryShadow.text = name0;
		} else {
			secondary.transform.parent.gameObject.SetActive (false);
		}

		// Set Image

		string path = AssetDatabase.GetAssetPath (spritesheet);
		Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath (path).OfType<Sprite>().ToArray();


		preview.sprite = sprites [PokemonDatabase.getPokemonbyID (id).getID ()-1];
		preview.gameObject.GetComponent<RectTransform> ().sizeDelta = sprites [PokemonDatabase.getPokemonbyID (id).getID ()-1].bounds.size * 37;

		setTyping (id);

	}

	private string toNum(int n){

		if (n < 10) {
			return "00" + n;
		} else if (n < 100) {
			return "0" + n;
		} else {
			return n.ToString();
		}

	}


	private void setTyping(int id){
		
		type1.sprite = typeToImage (PokemonDatabase.getPokemonbyID(id).getType1());

		if (PokemonDatabase.getPokemonbyID (id).getType2 () == PokemonData.Type.NONE) {
			type2.gameObject.SetActive (false);
		} else {
			type2.gameObject.SetActive (true);
			type2.sprite = typeToImage (PokemonDatabase.getPokemonbyID (id).getType2 ());
		}

	}

	private Sprite typeToImage(PokemonData.Type type){

		string path = AssetDatabase.GetAssetPath (typesheet);
		Sprite[] types = AssetDatabase.LoadAllAssetsAtPath (path).OfType<Sprite>().ToArray();

		if (type == PokemonData.Type.NORMAL) {
			return types [0];
		} else if (type == PokemonData.Type.FIGHTING) {
			return types [1];
		} else if (type == PokemonData.Type.FLYING) {
			return types [2];
		} else if (type == PokemonData.Type.POISON) {
			return types [3];
		} else if (type == PokemonData.Type.GROUND) {
			return types [4];
		} else if (type == PokemonData.Type.ROCK) {
			return types [5];
		} else if (type == PokemonData.Type.BUG) {
			return types [6];
		} else if (type == PokemonData.Type.GHOST) {
			return types [7];
		} else if (type == PokemonData.Type.STEEL) {
			return types [8];
		} else if (type == PokemonData.Type.NONE) {
			return types [9];
		} else if (type == PokemonData.Type.FIRE) {
			return types [10];
		} else if (type == PokemonData.Type.WATER) {
			return types [11];
		} else if (type == PokemonData.Type.GRASS) {
			return types [12];
		} else if (type == PokemonData.Type.ELECTRIC) {
			return types [13];
		} else if (type == PokemonData.Type.PSYCHIC) {
			return types [14];
		} else if (type == PokemonData.Type.ICE) {
			return types [15];
		} else if (type == PokemonData.Type.DRAGON) {
			return types [15];
		} else if (type == PokemonData.Type.DARK) {
			return types [16];
		} else if (type == PokemonData.Type.FAIRY) {
			return types [17];
		} else {
			return types [9];
		}
	
	}

}

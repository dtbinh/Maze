using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	public int baseHealth = 100;
	public static int currentHealth;
	public Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public AudioSource playerAudio;
	public AudioClip deadClip;
	public static bool isAlive;

	FirstPersonController fpController;
	
	bool damaged;
	
	void Awake (){
		isAlive = true;
		fpController = GetComponent <FirstPersonController> ();
		currentHealth = baseHealth;
	}
	
	void Update (){
		damageImage.color = damaged ? flashColour : Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		damaged = false;
	}
	
	// Take damage from enemies
	public void TakeDamage(int amount) {
		damaged = true;
		
		currentHealth -= amount;
		
		playerAudio.Play ();
		// Dissable all children of the player object when died.
		if(currentHealth <= 0 && isAlive){
			isAlive = false;
			playerAudio.clip = deadClip;
			playerAudio.Play();
			
			fpController.enabled = false;
			foreach(Transform t in Camera.main.transform){
				t.gameObject.SetActive(false);
			}
			Invoke ("LoadResult", 2);
		}
	}

	void LoadResult(){
		GameMaster.win = false;
		Application.LoadLevel("Level_Result");
	}
}

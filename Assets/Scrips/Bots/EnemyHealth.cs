using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int startingHealth = 100;
    public float currentHealth;
    public int scoreValue = 10;
	public AudioClip deathClip;
	public GameObject explosion;
	public bool isAlive = true;
    public AudioSource enemyAudio;

    void Awake (){
        currentHealth = startingHealth;
    }

    public void TakeDamage(float amount) {
		if(!isAlive)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
        if(currentHealth <= 0){
            Death ();
        }
    }

    void Death (){
		isAlive = false;
		Destroy (Instantiate (explosion, transform.position, Quaternion.identity),1f);
		ProfileUpdate();
		foreach(Transform t in transform.GetChild(0)){
			t.gameObject.SetActive (false);
		}

		Destroy(gameObject, 1f);

		GetComponent<Rigidbody>().useGravity = true;

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }

	void ProfileUpdate(){
		GameMaster.currentScore += scoreValue;
		GameMaster.totalBotKilled++;
		switch(transform.name){
		case "AttackBot(Clone)":
			GameMaster.attackbotKilled++;
			GetComponent<BoxCollider>().enabled = false;
			GetComponent<AttackBot_AI>().enabled = false;
			GetComponent<Pathfinding>().enabled = false;
			break;
		case "SpeedBot(Clone)":
			GameMaster.speedbotKilled++;
			GetComponent<BoxCollider>().enabled = false;
			GetComponent<SpeedBot_AI>().enabled = false;
			break;
		case "DefenceBot(Clone)":
			GameMaster.defencebotKilled++;
			GetComponent<SphereCollider>().enabled = false;
			GetComponent<DefenceBot_AI>().enabled = false;
			GetComponent<Pathfinding>().enabled = false;
			break;
		}

	}
}

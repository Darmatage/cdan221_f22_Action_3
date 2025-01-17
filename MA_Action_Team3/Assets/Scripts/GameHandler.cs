using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameHandler : MonoBehaviour {

	private GameObject player;
	public static int playerHearts = 3;
	public GameObject heart1;
	public GameObject heart2;
	public GameObject heart3;
	public static float heart1fill = 1.0f;
	public static float heart2fill = 1.0f;
	public static float heart3fill = 1.0f;
	
	public static int playerHealth = 100;
	public int StartPlayerHealth = 100;
	//public GameObject healthText;
	  
	public static bool haveWisp1 =false;
	public static bool haveWisp2 =false;
	public static bool haveWisp3 =false;


	public static int gotTokens = 0;
	public GameObject tokensText;

	public bool isDefending = false;

	public static bool stairCaseUnlocked = false;
	//this is a flag check. Add to other scripts: GameHandler.stairCaseUnlocked = true;

	private string sceneName;

	//Pause menu variables
	public static bool GameisPaused = false;
	public GameObject pauseMenuUI;
	public AudioMixer mixer;
	public static float volumeLevel = 1.0f;
	private Slider sliderVolumeCtrl;

	void Awake (){
		SetLevel (volumeLevel);
		GameObject sliderTemp = GameObject.FindWithTag("PauseMenuSlider");
		if (sliderTemp != null){
			sliderVolumeCtrl = sliderTemp.GetComponent<Slider>();
			sliderVolumeCtrl.value = volumeLevel;
		}
	}


    void Start(){
		player = GameObject.FindWithTag("Player");
		sceneName = SceneManager.GetActiveScene().name;
		if (sceneName=="MainMenu"){ //uncomment these two lines when the MainMenu exists
			playerHealth = StartPlayerHealth;
		}
		
		updateStatsDisplay();
		pauseMenuUI.SetActive(false);
		GameisPaused = false;
		
		heart1.SetActive(true);
		heart2.SetActive(true);
		heart3.SetActive(true);
		
		Debug.Log("pHealth = " + playerHealth + ". Hearts = " + playerHearts +  ". h1fill = " + heart1fill + ". h2fill = " + heart2fill + ". h3fill = " + heart3fill );
               
	}

	void Update (){
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (GameisPaused){
				Resume();
			}
			else{
				Pause();
			}
		}
	}

	// The information for the Pause Menu. 

	void Pause(){
		pauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameisPaused = true;
	}

	public void Resume(){
		pauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameisPaused = false;
	}

	public void SetLevel (float sliderValue){
		mixer.SetFloat("MusicVolume", Mathf.Log10 (sliderValue) * 20);
		volumeLevel = sliderValue;
	} 


    public void playerGetTokens(int newTokens){
            gotTokens += newTokens;
            updateStatsDisplay();
    }

	public void playerGetHit(int damage){
		  
		if (isDefending == false){
			playerHealth -= damage;
			if (playerHealth > 0){
				//set value of current heart = to playerHealth amount	
				if (playerHearts == 3){
					heart3fill = (float)playerHealth / StartPlayerHealth;
				}
				else if (playerHearts == 2){
					heart2fill = (float)playerHealth / StartPlayerHealth;
				}
				else if (playerHearts == 1){
					heart1fill = (float)playerHealth / StartPlayerHealth;
				}
				
				Debug.Log("pHealth = " + playerHealth + ". Hearts = " + playerHearts +  ". h1fill = " + heart1fill + ". h2fill = " + heart2fill + ". h3fill = " + heart3fill );
                updateStatsDisplay(); // visual of currentHeart: heart3.GetComponent<Image>()
            }
			if(damage >= 0){
					// if damage hurts player, play gethit animation
					//player.GetComponent<PlayerHurt>().playerHit();
			}
        }

        if (playerHealth <= 0){
            playerHealth = 0;
			playerHearts -= 1;
			updateStatsDisplay(); // to display # of hearts based on playerHearts counter
			playerHealth = StartPlayerHealth;
			
            if (playerHearts <=0){ 
				playerDies();
			}
        }


        // if (playerHealth > StartPlayerHealth){
                  // playerHealth = StartPlayerHealth;
				  // updateStatsDisplay();
        // }

      }

    public void updateStatsDisplay(){
		if (playerHearts ==3){
			heart3.GetComponent<Image>().fillAmount = heart3fill;
		}
		else if (playerHearts == 2){
			heart3.SetActive(false);
			heart2.GetComponent<Image>().fillAmount = heart2fill;
		}
		else if (playerHearts == 1){
			heart2.SetActive(false);
			heart1.GetComponent<Image>().fillAmount = heart1fill;
		}
		else if (playerHearts == 0){
			heart1.SetActive(false);
		}

		  
            //Text healthTextTemp = healthText.GetComponent<Text>();
            //healthTextTemp.text = "HEALTH: " + playerHealth;

            Text tokensTextTemp = tokensText.GetComponent<Text>();
            tokensTextTemp.text = "ORBS: " + gotTokens;
    }

      public void playerDies(){
            //player.GetComponent<PlayerHurt>().playerDead(); // re-add for player controller
            StartCoroutine(DeathPause());
      }

      IEnumerator DeathPause(){
            //player.GetComponent<PlayerMove>().isAlive = false;
            //player.GetComponent<PlayerJump>().isAlive = false;
            yield return new WaitForSeconds(1.0f);
            SceneManager.LoadScene("EndLose");
      }

      public void StartGame() {
            SceneManager.LoadScene("Level1");
      }

      public void RestartGame() {
            SceneManager.LoadScene("MainMenu");
            playerHealth = StartPlayerHealth;
      }

      public void QuitGame() {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
      }

      public void Credits() {
            SceneManager.LoadScene("Credits");
      }
	  
	  

}

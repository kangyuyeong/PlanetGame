using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("----------------[ core ]")]
    public int score;
    public int maxLevel;
    public bool isOver;


    [Header("----------------[ Object Pooling ]")]
    public GameObject donglePrefab;
    public Transform dongleGroup;
    public List<Dongle> donglePool;
    public GameObject effectPrefab;
    public Transform effectGroup;
    public List<ParticleSystem> effectPool;
    [Range(1, 30)]
    public int poolSize; 
    public int poolCursor;
    public Dongle lastDongle;


    [Header("----------------[ Audio ]")]
    public AudioSource[] sfxPlayer;
    public AudioClip[] sfxClip;
    public enum Sfx { LevelUp, Next, Button, Over };
    int sfxCursor;


    [Header("----------------[ UI ]")]
    public GameObject startGroup;
    public GameObject endGroup;
    public Text scoreText;
    public Text maxScoreText;
    public Text subScoreText;


    [Header("----------------[ ETC ]")]
    public GameObject line;
    public GameObject bottom;



 

    void Awake(){

        Application.targetFrameRate = 60;

        donglePool = new List<Dongle>();
        effectPool = new List<ParticleSystem>();
        for(int index=0;index<poolSize;index++){
            MakeDongle();
        }

        if(!PlayerPrefs.HasKey("MaxScore")){
            PlayerPrefs.SetInt("MaxScore",0);
        }

        maxScoreText.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }

    public void GameStart()
    {
        //오브젝트 활성화
        line.SetActive(true);
        bottom.SetActive(true);
        scoreText.gameObject.SetActive(true);
        maxScoreText.gameObject.SetActive(true);
        startGroup.SetActive(false);

        //사운드 플레이
        SfxPlay(Sfx.Button);

        //게임 시작(동글 생성)
        Invoke("NextDongle", 1.5f);
    }

    Dongle MakeDongle(){
        //이펙트 생성
        GameObject instantEffectObj = Instantiate(effectPrefab, effectGroup);
        instantEffectObj.name = "Effect " + effectPool.Count;
        ParticleSystem instantEffect = instantEffectObj.GetComponent<ParticleSystem>();
        effectPool.Add(instantEffect);


        //동글 생성
        GameObject instantDongleObj = Instantiate(donglePrefab, dongleGroup);
        instantDongleObj.name = "Dongle " + donglePool.Count;
        Dongle instantDongle = instantDongleObj.GetComponent<Dongle>();
        instantDongle.manager = this;
        instantDongle.effect = instantEffect;
        donglePool.Add(instantDongle);


        return instantDongle;
    }


    Dongle GetDongle(){
        for(int index=0;index<donglePool.Count;index++){
            poolCursor = (poolCursor +1)%donglePool.Count;
            if(!donglePool[poolCursor].gameObject.activeSelf){
                return donglePool[poolCursor];
            }
        }

        return MakeDongle();

    }


    void NextDongle(){

        if(isOver){
            return;
        }
        
        lastDongle = GetDongle();

        lastDongle.level = Random.Range(0,maxLevel);
        lastDongle.gameObject.SetActive(true); 

        SfxPlay(Sfx.Next);
        StartCoroutine("WaitNext");
    }

    IEnumerator WaitNext(){
        while(lastDongle != null){
            yield return null;
        }
        yield return new WaitForSeconds(2.0f);

        NextDongle();
    }

    public void TouchDown(){

        if(lastDongle == null){
            return;
        }
        lastDongle.Drag();
    }

    public void TouchUp(){

        if(lastDongle == null){
            return;
        }
        lastDongle.Drop();
        lastDongle = null;
    }

    public void GameOver(){
        if (isOver){
            return;
        }
        isOver = true;
        
        StartCoroutine("GameOverRoutine");
    }

    IEnumerator GameOverRoutine(){
        //1. 장면 안에 활성화 되어있는 모든 동글 가져오기
        Dongle[] dongles = FindObjectsOfType<Dongle>();

        //2. 지우기 전에 모든 동글의 물리효과 비활성화
        for(int index = 0; index < dongles.Length; index++){
            dongles[index].rigid.simulated = false;
        }

        //3. 1번의 목록을 하나씩 접근해서 지우기
        for(int index = 0; index<dongles.Length;index++){
            dongles[index].Hide(Vector3.up * 100);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        //최고 점수 갱신
        int maxScore = Mathf.Max(score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);

        //게임 오버 UI 표시
        subScoreText.text = "Score : " + scoreText.text;
        endGroup.SetActive(true);
        SfxPlay(Sfx.Over);
    }

    public void Reset(){
        SfxPlay(Sfx.Button);
        StartCoroutine("ResetCoroutine");
    }

    IEnumerator ResetCoroutine(){
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }

    public void SfxPlay(Sfx type){
        switch (type){
            case Sfx.LevelUp:
                sfxPlayer[sfxCursor].clip = sfxClip[Random.Range(0,3)];
                break;
            case Sfx.Next:
                sfxPlayer[sfxCursor].clip = sfxClip[3];
                break;
            case Sfx.Button:
                sfxPlayer[sfxCursor].clip = sfxClip[4];
                break;
            case Sfx.Over:
                sfxPlayer[sfxCursor].clip = sfxClip[5];
                break;
        }

        sfxPlayer[sfxCursor].Play();
        sfxCursor++;
        sfxCursor = (sfxCursor + 1) % sfxPlayer.Length;
    }

    void Update(){
        if(Input.GetButtonDown("Cancle")){
            Application.Quit();
        }
    }
    


    void LateUpdate(){
        scoreText.text = score.ToString();

    }
}

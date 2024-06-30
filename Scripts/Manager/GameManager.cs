using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField, Tooltip("처음 게임 연출 할 것인지 정한다.")]
    private bool IsGameProduction;


    [SerializeField]
    private GameObject[] Prefabs;
    [SerializeField]
    private Vector3[] BossPosition;
    [SerializeField]
    private APlayer Player;

    [Header("UI")]
    [SerializeField]
    private Transform Canvas;
    [SerializeField]
    private GameObject ReadyPrefab;
    [SerializeField]
    private GameObject AKnockdutPrefab;
    [SerializeField]
    private GameObject DiedPrefab;
    [SerializeField]
    private float SpawnDelay = 1.0f;

    [Header("Screen Shader")]
    [SerializeField]
    private Material FullScreenMat;
    [SerializeField]
    private float FadeTime = 0.5f;

    [SerializeField] 
    private float DefaultFadeIn= 2.2f;


    private int Index;
    private AActor Boss;

    private SpriteEffect Ready;
    private SpriteEffect AKnockOut;
    private SpriteEffect Died;

    private bool IsVictory;
    private bool IsFailed;
    private bool IsPause;
    private bool ListBoss;

    private Coroutine SpawnCoroutine;
    private float DiedDelayTime = 2.5f;

    

    private void Awake()
    {
        StartCoroutine(ScreenFadeIn());
        FullScreenMat.SetFloat("_Scale", DefaultFadeIn);

    }

    private void Start()
    {
        SoundManager.gInstance.PlaySound("Ready", transform, () =>
        {
            SoundManager.gInstance.PlaySound("Go", transform, 0.7f);
        },0.7f);
    }

    private IEnumerator SpawnBoss()
    {
        if (Index < BossPosition.Length)
        {
            if (Index != 0)
            {
                yield return new WaitForSeconds(SpawnDelay);
            }

            Boss = Instantiate(Prefabs[Index], BossPosition[Index], Quaternion.identity).GetComponent<AActor>();
            SpawnCoroutine = null;
            Index++;

            if (BossPosition.Length == Index)
            {
                ListBoss = true;
            }
        }
    }

    private IEnumerator ScreenFadeIn()
    {
        if (FullScreenMat == null) yield break;

        float scale = DefaultFadeIn;
        float time = Time.time + FadeTime;
        while (time > Time.time)
        {
            FullScreenMat.SetFloat("_Scale", scale * (1.0f - ((time - Time.time)) / FadeTime));
            yield return new WaitForEndOfFrame();
        }
        FullScreenMat.SetFloat("_Scale", scale);
        if (ReadyPrefab != null)
            Ready = Instantiate(ReadyPrefab, Canvas).GetComponent<SpriteEffect>();
    }

    public IEnumerator ScreenFadeOut()
    {
        if (FullScreenMat == null) yield break;

        float scale = DefaultFadeIn;
        float time = Time.time + FadeTime;
        while (time > Time.time)
        {
            FullScreenMat.SetFloat("_Scale", scale * ((time - Time.time) / FadeTime));
            yield return new WaitForEndOfFrame();
        }
        FullScreenMat.SetFloat("_Scale", 0.0f);
        LoadingController.gCurrentSceneName("MainScene");
        SceneManager.LoadScene("LoadingScene");
    }


    private void Update()
    {
        if (SpawnCoroutine == null && Boss == null)
        {
            SpawnCoroutine = StartCoroutine(SpawnBoss());
        }
            
        if (Player != null && Boss != null)
        {
            if (Player.gIsIntro == false && Boss.gIsIntro == false)
            {
                IsGameProduction = false;
            }
        }

        if (ListBoss == true && Boss.GetComponent<StatusComponent>().IsDead == true && IsVictory == false)
        {
            AKnockOut = Instantiate(AKnockdutPrefab, Canvas).GetComponent<SpriteEffect>();
            AKnockOut.AnimEventCallback = AknockdutAnimEvent;
            SoundManager.gInstance.PlaySound("KnockOut", transform);
            IsVictory = true;
        }
        else if (Player.GetComponent<StatusComponent>().IsDead == true && IsFailed == false)
        {

            StartCoroutine(DiedCoroutine());
            IsFailed = true;
        }
    }


    private IEnumerator DiedCoroutine()
    {
        Died = Instantiate(DiedPrefab, Canvas).GetComponent<SpriteEffect>();
        yield return new WaitForSeconds(DiedDelayTime);
        StartCoroutine(ScreenFadeOut());
    }



    private void AknockdutAnimEvent()
    {
        StartCoroutine(ScreenFadeOut());
    }

    private void OnDestroy()
    {
        FullScreenMat.SetFloat("_Scale", DefaultFadeIn);
        
    }


    public bool gIsFailed => IsFailed;
    public bool gIsVictory => IsVictory;
    public bool gIsPause => IsPause;
    public bool gIsGameProduction => IsGameProduction;

    public GameObject[] gPrefabs => Prefabs;
    public Vector3[] gBossPosition => BossPosition;

}


using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Resources : MonoBehaviour
{
    public float CurrentWaterLevel = 0;
    public float CurrentMineralLevel = 0;
    public float MaxWaterLevel = 0;
    public float MaxMineralLevel = 0;
    public float LowestRootLevel = 0;
    public float TrashMultiplier = 1;

    public float TotalWater = 0;
    public float TotalMineral = 0;

    public float decreaseWaterSpeed;
    public float decreaseMineralSpeed;

    public GameObject gameOver;
    public Slider waterLevel;
    public Slider mineralLevel;
    public TextMeshProUGUI TargetWaterText;
    public TextMeshProUGUI TargetMineralText;

    public ResourcesStep[] Steps;

    public AudioSource MusicSource;
    public AudioSource TransitionSource;
    public Animator animator;
    public RootComponent root;

    private int currentStep = 0;

    public static Resources Instance;

    private void Awake()
    {
        Time.timeScale = 0.0f;
    }

    private void Start()
    {
        Instance = this;
        waterLevel.maxValue = MaxWaterLevel;
        mineralLevel.maxValue = MaxMineralLevel;

        CurrentWaterLevel = MaxWaterLevel;
        CurrentMineralLevel = MaxMineralLevel;

        TargetWaterText.text = $"{TotalWater}/{Steps[currentStep].TargetWater}";
        TargetMineralText.text = $"{TotalMineral}/{Steps[currentStep].TargetMineral}";
    }

    private void Update()
    {
        CurrentWaterLevel -= Time.deltaTime * decreaseWaterSpeed * TrashMultiplier;
        CurrentMineralLevel -= Time.deltaTime * decreaseMineralSpeed * TrashMultiplier;

        if (CurrentWaterLevel > MaxWaterLevel)
        {
            CurrentWaterLevel = MaxWaterLevel;
        }

        if (CurrentMineralLevel > MaxMineralLevel)
        {
            CurrentMineralLevel = MaxMineralLevel;
        }

        if (CurrentWaterLevel <= 0)
        {
            CurrentWaterLevel = 0;
            End();
        }

        if (CurrentMineralLevel <= 0)
        {
            CurrentMineralLevel = 0;
            End();
        }

        waterLevel.value = CurrentWaterLevel;
        mineralLevel.value = CurrentMineralLevel;
    }

    public void End()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
    }

    public bool CheckConditions()
    {
        if (TotalMineral >= Steps[currentStep].TargetMineral && TotalWater >= Steps[currentStep].TargetWater)
        {
            GoToNextStep();
            return true;
        }
        return false;
    }

    public void GiveWater(int amount)
    {
        CurrentWaterLevel += amount;
        if (currentStep >= Steps.Length)
        {
            return;
        }
        TotalWater += amount;
        TargetWaterText.text = $"{TotalWater}/{Steps[currentStep].TargetWater}";
    }

    public void GiveMineral(int amount)
    {
        CurrentMineralLevel += amount;
        if (currentStep >= Steps.Length)
        {
            return;
        }
        TotalMineral += amount;
        TargetMineralText.text = $"{TotalMineral}/{Steps[currentStep].TargetMineral}";
    }

    public void IncreaseTrashMultiplier(float amount)
    {
        TrashMultiplier += amount;
    }

    public void ProposeDepth(float depth)
    {
        if (currentStep >= Steps.Length)
        {
            return;
        }
        LowestRootLevel = Mathf.Max(LowestRootLevel, depth);
    }

    public void ResetRootPosition()
    {
        root.transform.position = Vector3.zero;
    }

    public void GoToNextStep()
    {
        if (currentStep >= Steps.Length)
        {
            TargetWaterText.text = string.Empty;
            TargetMineralText.text = string.Empty;
            return;
        }
        root.enabled = false;
        animator.SetTrigger("ChangeTree");
        ResourcesStep current = Steps[currentStep];
        currentStep++;
        if (currentStep >= Steps.Length)
        {
            TargetWaterText.text = string.Empty;
            TargetMineralText.text = string.Empty;
            return;
        }
        MusicSource.Stop();
        MusicSource.clip = Steps[currentStep].Music;

        TransitionSource.clip = Steps[currentStep - 1].Transition;
        TransitionSource.Play();

        TargetWaterText.text = $"{TotalWater}/{Steps[currentStep].TargetWater}";
        TargetMineralText.text = $"{TotalMineral}/{Steps[currentStep].TargetMineral}";
        TotalMineral -= current.TargetMineral;
        TotalWater -= current.TargetWater;
        root.currentSize = Steps[currentStep].RootSize;
        root.TruncSize = Steps[currentStep].TruncSize;
    }

    [Serializable]
    public class ResourcesStep
    {
        public int TargetWater;
        public int TargetMineral;
        public float RootSize;
        public float TruncSize;
        public AudioClip Music;
        public AudioClip Transition;
    }
}

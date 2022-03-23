using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject winCanvas;
    public GameObject loseCanvas;
    public GameObject mainMenuCanvas;
    public GameObject gameCanvas;
    public GameObject loseWords;
    public GameObject winWords;

    public Text warriorCountText;
    public Text krestCountText;
    public Text foodCountText;
    public Text waveCountText;
    public Text enemyInNextWaveCountText;

    public Text warriorFinalCountText;
    public Text krestFinalCountText;
    public Text foodFinalCountText;
    public Text waveFinalCountText;
    public Text enemyDefitedFinalCountText;
    

    public int fora;
    public int lastEnemyCount;
    public int currentEnemyCount;
    public int nextEnemyCount;
    public int enemyDefitedFinalCount;
    public int warriorCount;
    public int warriorTotalCount;
    public int krestCount;
    public int foodCount;
    public int foodTotalCount;
    public int waveCount;
    public int enemyInNextWaveCount;
    public float timeToNextWaveTimerCount;
    public float timeToNextWaveTimerCountTemp;
    public float timeToHarvestDefTimerCount;
    public float timeToHarvestTimerCount;
    public float timeToNaimWarriorTimerCount;
    public float timeToNaimWarriorDefTimerCount;
    public float timeTonaimKrestTimerCount;
    public float timeTonaimKrestDefTimerCount;
    public float waveDelay;
    public int warriorCost;
    public int krestCost;
    public int targetFoodCount;
    public int foodFromOneKrest;
    public bool isWarriorTraining;
    public bool isKrestTraining;

    public Button warriorNaimButton;
    public Button krestNaimButton;
    public Image warriorNaimTimerImage;
    public Image krestNaimTimerImage;
    public Image harvestTimerImage;
    public Image waveTimerImage;

    // Start is called before the first frame update
    void Start()
    {
        GameInitialize();
        mainMenuCanvas.GetComponent<BGMusic>().PlaySound();
    }

    // Update is called once per frame
    void Update()
    {
        HarvestTimer();
        NextWaveTimer();
        if (isWarriorTraining)
        {
            WarriorTrainingTimer();
        }
        if (isKrestTraining)
        {
            KrestTrainingTimer();
        }
    }

    // Метод GameInitialize нужен дя инициализации стартовых значений при запуске или перезапуске игры без выхода из приложения
    public void GameInitialize()
    {
        Time.timeScale = 1.0f;
        waveTimerImage.fillAmount = 100;
        harvestTimerImage.fillAmount = 100;
        warriorCountText.text = (warriorCount = 0).ToString();
        krestCountText.text = (krestCount = 0).ToString();
        foodCountText.text = (foodCount = 5).ToString();
        waveCountText.text = (waveCount = 0).ToString();
        lastEnemyCount = 0;
        fora = fora;
        waveDelay = waveDelay;
        currentEnemyCount = 1;
        nextEnemyCount = 0;
        enemyInNextWaveCountText.text = (nextEnemyCount).ToString();
        timeToHarvestTimerCount = timeToHarvestDefTimerCount;
        timeTonaimKrestTimerCount = timeTonaimKrestDefTimerCount;
        timeToNaimWarriorTimerCount = timeToNaimWarriorDefTimerCount;
        isWarriorTraining = false;
        isKrestTraining = false;
        enemyDefitedFinalCount = 0;
        timeToNextWaveTimerCount = timeToNextWaveTimerCountTemp = waveDelay;
        waveTimerImage.fillAmount = 100;
    }

    // Обновляем Панель статисткики при нажитии на кнопки найма воина(0) или кретьянина(1)
    // Также вызывается после боя(2) и после каждого сбора урожая(3)
    void UIStaticticUpdater(int t)
    {
        // Первая ветвь if сработает при нажатии кнопки найма воина
        if (t == 0)
        {
            //Обновляем количество Воинов и выводим на экран
            warriorCount += 1;
            warriorTotalCount += 1;
            warriorCountText.text = warriorCount.ToString();
            warriorNaimTimerImage.fillAmount = 1;
        }
        // Эта веть if вызовется при нажатии на кнопку найма крестьян
        else if (t == 1)
        {
            //Обновляем количество еды и выводим на экран
            foodCountText.text = foodCount.ToString();
            // обновляем количество крестьяе и выводим на экран
            krestCount += 1;
            krestCountText.text = krestCount.ToString();
            krestNaimTimerImage.fillAmount = 1;
        }
        //Данная веть кода вызовется когда таймер следующей волны обнулится и случится махач
        else if (t == 2)
        {
            //Обновляем количество воинов после махача
            warriorCount = warriorCount - enemyInNextWaveCount;
            //Проверяем количество воинов если оно не отрицательное, значит живем и обновляем в стате количество воинов,
            // иначе вызываем окно поражения
            if (warriorCount >= 0)
            {
                warriorCountText.text = warriorCount.ToString();
                enemyDefitedFinalCount = enemyDefitedFinalCount + enemyInNextWaveCount;
            }
            else 
            {
                Time.timeScale = 0;
                finalStatistic();
                loseCanvas.SetActive(true);
                loseWords.SetActive(true);
                gameCanvas.SetActive(false);
            }
        }
        // Данная ветка вызывается каждый раз при жатве хлебушка(3)
        else if (t == 3)
        {
            //Обновляем занчение еды в переменной и затем чекаем достигли ли мы цели. если да то вызываем Экран победы 
            //иначе просто обновляем поле статичтики
            foodCount = foodCount + (krestCount * foodFromOneKrest);
            foodTotalCount = foodTotalCount + (krestCount * foodFromOneKrest);
            if (foodCount >= targetFoodCount)
            {
                finalStatistic();
                loseCanvas.SetActive(true);
                winWords.SetActive(true); 
                gameCanvas.SetActive(false);
            }
            else 
            {
                foodCountText.text = foodCount.ToString();
            }
        }

    }
    //метод вызывается каждый раз когда обнуляется таймер битвы(0). Мы обновляем значения переменных Номер волны, количество врагов
    // и также обновляем значение поля время до волны
    void UIWaveUpdater(int t)
    {
        //Входной параметр определяет по какой причине вызван метод битва или просто обновление таймера волны
        if (t == 0)
        {
            //Обновляем номер олны, количество врагов в след волне и врея до след волны и все это выводим на экран
            waveCount = waveCount + 1;
            fora = fora - 1;
            waveCountText.text = waveCount.ToString();
            //
            EnemyCOuntDowner();
            enemyInNextWaveCount = nextEnemyCount;
            enemyInNextWaveCountText.text = enemyInNextWaveCount.ToString();
            //
            timeToNextWaveTimerCount = waveDelay;
            waveTimerImage.fillAmount = 100;
        }
        // Эта ветвь кода вызывается для обновления показаний таймра волны между дракаами
        else
        {
            waveTimerImage.fillAmount = timeToNextWaveTimerCount / timeToNextWaveTimerCountTemp;
        }
    }
    //Таймер сбора урожая
    void HarvestTimer()
    {
        if (krestCount > 0)
        {
            timeToHarvestTimerCount = timeToHarvestTimerCount - Time.deltaTime;
            if (timeToHarvestTimerCount <= 0)
            {
                timeToHarvestTimerCount = timeToHarvestDefTimerCount;
                harvestTimerImage.GetComponent<AudioManager>().PlaySound();
                UIStaticticUpdater(3);
                harvestTimerImage.fillAmount = timeToHarvestTimerCount / timeToHarvestDefTimerCount;
            }
            else
            {
                harvestTimerImage.fillAmount = timeToHarvestTimerCount / timeToHarvestDefTimerCount;
            }
        }
    }
    //Таймер следующей волны
    void NextWaveTimer()
    {
        timeToNextWaveTimerCount = timeToNextWaveTimerCount - Time.deltaTime;
        if (timeToNextWaveTimerCount <= 0)
        {
            waveTimerImage.GetComponent<AudioManager>().PlaySound();
            UIStaticticUpdater(2);
            UIWaveUpdater(0);
        }
        else 
        {
            UIWaveUpdater(1);
        }
    }
    //Таймер найма воина
    void WarriorTrainingTimer()
    {
        timeToNaimWarriorTimerCount = timeToNaimWarriorTimerCount - Time.deltaTime;
        if (timeToNaimWarriorTimerCount <= 0)
        {
            timeToNaimWarriorTimerCount = timeToNaimWarriorDefTimerCount;
            isWarriorTraining = false;
            warriorNaimButton.interactable = true;
            warriorNaimTimerImage.GetComponent<AudioManager>().PlaySound();
            UIStaticticUpdater(0);
        }
        else 
        {   
            warriorNaimTimerImage.fillAmount = timeToNaimWarriorTimerCount/timeToNaimWarriorDefTimerCount;
        }
    }
    //
    void KrestTrainingTimer()
    {
        timeTonaimKrestTimerCount = timeTonaimKrestTimerCount - Time.deltaTime;
        if (timeTonaimKrestTimerCount <= 0)
        {
            timeTonaimKrestTimerCount = timeTonaimKrestDefTimerCount;
            isKrestTraining = false;
            krestNaimButton.interactable = true;
            krestNaimTimerImage.GetComponent<AudioManager>().PlaySound();
            UIStaticticUpdater(1);
        }
        else
        {
            krestNaimTimerImage.fillAmount = timeTonaimKrestTimerCount / timeTonaimKrestDefTimerCount;
        }
    }
    public void WarTraining()
    {
        if (foodCount >= warriorCost)
        {
            //Обновляем количество еды и выводим на экран
            foodCount = foodCount - warriorCost;
            foodCountText.text = foodCount.ToString();
            isWarriorTraining = true;
            //Bинвертируем свойтво интерактебл кнопки
            warriorNaimButton.interactable = false;
        }
    }
    public void KrestTraining()
    {
        if (foodCount >= krestCost)
        {
            //Обновляем количество еды и выводим на экран
            foodCount = foodCount - krestCost;
            foodCountText.text = foodCount.ToString();
            isKrestTraining = true;
            //Bинвертируем свойтво интерактебл кнопки
            krestNaimButton.interactable = false;
        }
    }

    public void ExitApp()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        loseCanvas.SetActive(false);
        winWords.SetActive(false);
        loseWords.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
    }
    public void finalStatistic()
    {
        warriorFinalCountText.text = warriorTotalCount.ToString();
        krestFinalCountText.text = krestCount.ToString();
        foodFinalCountText.text = foodTotalCount.ToString();
        waveFinalCountText.text = waveCount.ToString();
        enemyDefitedFinalCountText.text = enemyDefitedFinalCount.ToString();
    }

    public void EnemyCOuntDowner()
    {
        if (fora <= 0)
        {
            nextEnemyCount = lastEnemyCount + currentEnemyCount;
            lastEnemyCount = currentEnemyCount;
            currentEnemyCount = nextEnemyCount;
        }
        else 
        {
            nextEnemyCount = 0;
        }
    }
}

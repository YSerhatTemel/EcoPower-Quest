using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelGeneratorMenu : EditorWindow
{
    private static void EnsureTexturesExist()
    {
        if (TextureGenerator.GenerateTexturesIfNeeded(true))
        {
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("EcoPower Quest/Generate Main Menu")]
    public static void GenerateMainMenu()
    {
        // Setup Camera
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            mainCam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
        }
        mainCam.backgroundColor = new Color(0.1f, 0.4f, 0.6f);
        mainCam.clearFlags = CameraClearFlags.SolidColor;

        GameObject audioObj = new GameObject("AudioManager");
        audioObj.AddComponent<AudioSource>();
        audioObj.AddComponent<AudioManager>();

        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        MainMenuManager mm = canvasObj.AddComponent<MainMenuManager>();

        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(canvasObj.transform, false);
        TextMeshProUGUI titleText = titleObj.AddComponent<TextMeshProUGUI>();
        titleText.text = "EcoPower Quest";
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.fontSize = 80;
        titleText.color = Color.white;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 0.7f);
        titleRect.anchorMax = new Vector2(1, 1);
        titleRect.offsetMin = Vector2.zero;
        titleRect.offsetMax = Vector2.zero;

        GameObject mainPanel = new GameObject("MainPanel");
        mainPanel.transform.SetParent(canvasObj.transform, false);
        RectTransform mainPanelRect = mainPanel.AddComponent<RectTransform>();
        mainPanelRect.anchorMin = Vector2.zero;
        mainPanelRect.anchorMax = Vector2.one;
        mainPanelRect.offsetMin = Vector2.zero;
        mainPanelRect.offsetMax = Vector2.zero;
        mm.mainPanel = mainPanel;

        GameObject playBtnObj = CreateUIRect("PlayBtn", mainPanel.transform, new Vector2(0.3f, 0.4f), new Vector2(0.7f, 0.6f));
        mm.playButtonBg = playBtnObj.AddComponent<Image>();
        mm.playButtonText = CreateUIText("PlayText", playBtnObj.transform, "PLAY");

        GameObject audioBtnObj = CreateUIRect("AudioBtn", mainPanel.transform, new Vector2(0.3f, 0.15f), new Vector2(0.7f, 0.35f));
        mm.audioButtonBg = audioBtnObj.AddComponent<Image>();
        mm.audioButtonText = CreateUIText("AudioText", audioBtnObj.transform, "Audio: ON");

        GameObject levelPanel = new GameObject("LevelPanel");
        levelPanel.transform.SetParent(canvasObj.transform, false);
        RectTransform levelPanelRect = levelPanel.AddComponent<RectTransform>();
        levelPanelRect.anchorMin = Vector2.zero;
        levelPanelRect.anchorMax = Vector2.one;
        levelPanelRect.offsetMin = Vector2.zero;
        levelPanelRect.offsetMax = Vector2.zero;
        mm.levelPanel = levelPanel;

        GameObject easyBtn = CreateUIRect("EasyBtn", levelPanel.transform, new Vector2(0.3f, 0.55f), new Vector2(0.7f, 0.7f));
        mm.easyButtonBg = easyBtn.AddComponent<Image>();
        CreateUIText("EasyText", easyBtn.transform, "Level 1: Easy");

        GameObject medBtn = CreateUIRect("MediumBtn", levelPanel.transform, new Vector2(0.3f, 0.35f), new Vector2(0.7f, 0.5f));
        mm.mediumButtonBg = medBtn.AddComponent<Image>();
        CreateUIText("MediumText", medBtn.transform, "Level 2: Medium");

        GameObject hardBtn = CreateUIRect("HardBtn", levelPanel.transform, new Vector2(0.3f, 0.15f), new Vector2(0.7f, 0.3f));
        mm.hardButtonBg = hardBtn.AddComponent<Image>();
        CreateUIText("HardText", hardBtn.transform, "Level 3: Hard");

        GameObject backBtn = CreateUIRect("BackBtn", levelPanel.transform, new Vector2(0.8f, 0.05f), new Vector2(0.95f, 0.15f));
        mm.backButtonBg = backBtn.AddComponent<Image>();
        CreateUIText("BackText", backBtn.transform, "Back");

        Debug.Log("EcoPower Quest Main Menu successfully generated!");
    }

    private static GameObject CreateUIRect(string name, Transform parent, Vector2 min, Vector2 max)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return obj;
    }

    private static TextMeshProUGUI CreateUIText(string name, Transform parent, string content)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        TextMeshProUGUI text = obj.AddComponent<TextMeshProUGUI>();
        text.text = content;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 40;
        text.color = Color.black;
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        return text;
    }

    [MenuItem("EcoPower Quest/Generate Level 1 (Easy)")]
    public static void GenerateLevel1()
    {
        EnsureTexturesExist();
        GenerateBaseSystem(Difficulty.Easy);
        int layer = LayerMask.NameToLayer("Ground");

        // Hand-Crafted Static Level 1
        CreatePlatform("StartPlatform", new Vector3(-8, -4, 0), new Vector2(8, 1), layer);
        CreateHazard("Spikes_1", new Vector3(-2, -6, 0), new Vector2(4, 1));
        
        CreatePlatform("Platform_2", new Vector3(2, -3, 0), new Vector2(4, 1), layer);
        CreateCheckpoint("Checkpoint", new Vector3(2, -2, 0));
        
        CreateHazard("Spikes_2", new Vector3(6, -6, 0), new Vector2(4, 1));
        CreatePlatform("Platform_3", new Vector3(10, -2, 0), new Vector2(4, 1), layer);
        
        CreateHazard("Spikes_3", new Vector3(14, -6, 0), new Vector2(4, 1));
        CreatePlatform("Platform_4", new Vector3(18, -1, 0), new Vector2(4, 1), layer);

        List<QuizQuestion> questions = new List<QuizQuestion>
        {
            new QuizQuestion { questionText = "Which item is recyclable?", options = new string[] { "Plastic Bottle", "Used Napkin", "Apple Core" }, correctOptionIndex = 0 },
            new QuizQuestion { questionText = "What color is a typical recycle bin?", options = new string[] { "Red", "Blue", "Black" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "What is 3 + 2?", options = new string[] { "4", "5", "6" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "Which is renewable energy?", options = new string[] { "Coal", "Solar", "Oil" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "What is 10 - 4?", options = new string[] { "4", "6", "8" }, correctOptionIndex = 1 }
        };
        CreateKnowledgeTerminal("Door", new Vector3(19, 0, 0), questions);
        Debug.Log("Hand-Crafted Level 1 (Easy) generated!");
    }

    [MenuItem("EcoPower Quest/Generate Level 2 (Medium)")]
    public static void GenerateLevel2()
    {
        EnsureTexturesExist();
        GenerateBaseSystem(Difficulty.Medium);
        int layer = LayerMask.NameToLayer("Ground");

        // Hand-Crafted Static Level 2 (Features Moving Platforms)
        CreatePlatform("StartPlatform", new Vector3(-8, -4, 0), new Vector2(6, 1), layer);
        
        CreateHazard("Spikes_1", new Vector3(0, -7, 0), new Vector2(10, 1));
        
        GameObject mp1 = CreatePlatform("MovingPlatform_1", new Vector3(0, -3, 0), new Vector2(3, 1), layer);
        MovingPlatform moving1 = mp1.AddComponent<MovingPlatform>();
        moving1.horizontal = true;
        moving1.distance = 3f;
        moving1.moveSpeed = 2f;

        CreatePlatform("SafePlatform_1", new Vector3(5, -2, 0), new Vector2(4, 1), layer);
        CreateCheckpoint("Checkpoint", new Vector3(5, -1, 0));

        CreateHazard("Spikes_2", new Vector3(10, -7, 0), new Vector2(6, 1));
        
        GameObject mp2 = CreatePlatform("MovingPlatform_2", new Vector3(10, -1, 0), new Vector2(3, 1), layer);
        MovingPlatform moving2 = mp2.AddComponent<MovingPlatform>();
        moving2.horizontal = false; // Vertical moving platform
        moving2.distance = 3f;
        moving2.moveSpeed = 2f;

        CreatePlatform("SafePlatform_2", new Vector3(14, 2, 0), new Vector2(5, 1), layer);

        List<QuizQuestion> questions = new List<QuizQuestion>
        {
            new QuizQuestion { questionText = "What is 5 + 7?", options = new string[] { "10", "12", "14" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "How do we save energy?", options = new string[] { "Leave lights on", "Turn off TV", "Open fridge" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "What is 3 x 4?", options = new string[] { "7", "12", "15" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "Which is bad for the ocean?", options = new string[] { "Fish", "Seaweed", "Plastic Bags" }, correctOptionIndex = 2 },
            new QuizQuestion { questionText = "What is 20 / 4?", options = new string[] { "4", "5", "6" }, correctOptionIndex = 1 }
        };
        CreateKnowledgeTerminal("Door", new Vector3(15, 3, 0), questions);
        Debug.Log("Hand-Crafted Level 2 (Medium) generated!");
    }

    [MenuItem("EcoPower Quest/Generate Level 3 (Hard)")]
    public static void GenerateLevel3()
    {
        EnsureTexturesExist();
        GenerateBaseSystem(Difficulty.Hard);
        int layer = LayerMask.NameToLayer("Ground");

        // Hand-Crafted Static Level 3 (Features Falling Platforms)
        CreatePlatform("StartPlatform", new Vector3(-8, -4, 0), new Vector2(4, 1), layer);
        
        CreateHazard("Spikes_1", new Vector3(0, -8, 0), new Vector2(10, 1)); // Pit

        GameObject fall1 = CreatePlatform("FallingPlatform_1", new Vector3(-3, -2, 0), new Vector2(2, 0.5f), layer);
        fall1.AddComponent<FallingPlatform>();

        GameObject fall2 = CreatePlatform("FallingPlatform_2", new Vector3(0, 0, 0), new Vector2(2, 0.5f), layer);
        fall2.AddComponent<FallingPlatform>();

        CreatePlatform("SafePlatform_1", new Vector3(4, 2, 0), new Vector2(4, 1), layer);
        CreateCheckpoint("Checkpoint", new Vector3(4, 3, 0));

        CreateHazard("Spikes_2", new Vector3(9, -8, 0), new Vector2(8, 1)); // Pit

        GameObject fall3 = CreatePlatform("FallingPlatform_3", new Vector3(7, 1, 0), new Vector2(2, 0.5f), layer);
        fall3.AddComponent<FallingPlatform>();

        GameObject fall4 = CreatePlatform("FallingPlatform_4", new Vector3(10, -1, 0), new Vector2(2, 0.5f), layer);
        fall4.AddComponent<FallingPlatform>();

        CreatePlatform("SafePlatform_2", new Vector3(14, -2, 0), new Vector2(5, 1), layer);

        List<QuizQuestion> questions = new List<QuizQuestion>
        {
            new QuizQuestion { questionText = "How long does plastic take to decompose?", options = new string[] { "10 years", "50 years", "450+ years" }, correctOptionIndex = 2 },
            new QuizQuestion { questionText = "Which gas do plants absorb?", options = new string[] { "Oxygen", "Carbon Dioxide", "Nitrogen" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "What is 15 x 3?", options = new string[] { "35", "45", "55" }, correctOptionIndex = 1 },
            new QuizQuestion { questionText = "What causes global warming?", options = new string[] { "Planting trees", "Windmills", "Fossil Fuels" }, correctOptionIndex = 2 },
            new QuizQuestion { questionText = "What is 100 / 5?", options = new string[] { "15", "20", "25" }, correctOptionIndex = 1 }
        };
        CreateKnowledgeTerminal("Door", new Vector3(15, -1, 0), questions);
        Debug.Log("Hand-Crafted Level 3 (Hard) generated!");
    }

    private static void GenerateBaseSystem(Difficulty diff)
    {
        GameObject gmObj = new GameObject("GameManager");
        GameManager gm = gmObj.AddComponent<GameManager>();
        gm.currentDifficulty = diff;
        gmObj.AddComponent<AudioSource>();
        gmObj.AddComponent<AudioManager>();

        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            mainCam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
        }
        mainCam.backgroundColor = new Color(0.5f, 0.8f, 1f);
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.orthographic = true;
        mainCam.orthographicSize = 7f;
        mainCam.gameObject.AddComponent<CameraController>();

        int groundLayer = LayerMask.NameToLayer("Ground");
        if (groundLayer == -1) Debug.LogWarning("Please create 'Ground' layer!");

        GenerateParallaxBackground(mainCam.transform);

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = new Vector3(-8, -2, 0);
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        
        SpriteRenderer playerSr = player.AddComponent<SpriteRenderer>();
        Sprite idleSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/player_idle.png");
        Sprite walk1Sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/player_walk1.png");
        Sprite walk2Sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/player_walk2.png");
        
        if (idleSprite != null) playerSr.sprite = idleSprite;
        playerSr.sortingOrder = 10;
        
        BoxCollider2D pCol = player.AddComponent<BoxCollider2D>();
        if (idleSprite != null) pCol.size = new Vector2(1f, 1f); 
        
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        PlayerController pc = player.AddComponent<PlayerController>();
        pc.idleSprite = idleSprite;
        pc.walkSprites = new Sprite[] { walk1Sprite, walk2Sprite };
        
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = new Vector3(0, -0.55f, 0);
        pc.groundCheck = groundCheck.transform;
        if (groundLayer != -1) pc.groundLayer = 1 << groundLayer;

        ParticleSystem ps = player.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.duration = 1f;
        main.startLifetime = 0.5f;
        main.startSpeed = 2f;
        main.startSize = 0.3f;
        main.startColor = new Color(1, 1, 1, 0.5f);
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        var emission = ps.emission;
        emission.rateOverTime = 10f;
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(0.5f, 0.1f, 1f);
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.sortingOrder = 5;

        CreateInvisibleHazard("DeathZone", new Vector3(0, -12, 0), new Vector2(200, 2));

        GenerateUI(gmObj);
    }

    private static void GenerateParallaxBackground(Transform camTransform)
    {
        Sprite bgSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/bg.png");
        if (bgSprite != null)
        {
            GameObject parentParallax = new GameObject("AI_Background_Parallax");
            parentParallax.transform.position = new Vector3(0, 0, 10);
            ParallaxBackground pb = parentParallax.AddComponent<ParallaxBackground>();
            pb.parallaxMultiplier = 0.8f;
            
            for (int i = -1; i < 4; i++)
            {
                GameObject bgObj = new GameObject($"bg_tile_{i}");
                bgObj.transform.SetParent(parentParallax.transform);
                bgObj.transform.localPosition = new Vector3(i * 25.5f, 0, 0);
                
                SpriteRenderer sr = bgObj.AddComponent<SpriteRenderer>();
                sr.sprite = bgSprite;
                sr.sortingOrder = -10;
                // Dim the background heavily so the character and spikes pop out
                sr.color = new Color(0.6f, 0.6f, 0.6f, 1f);
                bgObj.transform.localScale = new Vector3(2.5f, 1.5f, 1f);
            }
        }
    }

    private static void GenerateUI(GameObject gmObj)
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        QuizManager qm = canvasObj.AddComponent<QuizManager>();
        GameManager gm = gmObj.GetComponent<GameManager>();

        // Quiz Panel
        GameObject panelObj = new GameObject("QuizPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.1f, 0.1f);
        panelRect.anchorMax = new Vector2(0.9f, 0.9f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        qm.quizPanel = panelObj;

        GameObject qTextObj = new GameObject("QuestionText");
        qTextObj.transform.SetParent(panelObj.transform, false);
        TextMeshProUGUI qText = qTextObj.AddComponent<TextMeshProUGUI>();
        qText.text = "Question displays here";
        qText.alignment = TextAlignmentOptions.Center;
        qText.fontSize = 36;
        qText.color = Color.white;
        RectTransform qRect = qTextObj.GetComponent<RectTransform>();
        qRect.anchorMin = new Vector2(0, 0.7f);
        qRect.anchorMax = new Vector2(1, 1);
        qRect.offsetMin = Vector2.zero;
        qRect.offsetMax = Vector2.zero;
        qm.questionText = qText;

        GameObject tTextObj = new GameObject("TimerText");
        tTextObj.transform.SetParent(panelObj.transform, false);
        TextMeshProUGUI tText = tTextObj.AddComponent<TextMeshProUGUI>();
        tText.text = "Time: 10";
        tText.alignment = TextAlignmentOptions.Center;
        tText.fontSize = 24;
        tText.color = Color.red;
        RectTransform tRect = tTextObj.GetComponent<RectTransform>();
        tRect.anchorMin = new Vector2(0.8f, 0.8f);
        tRect.anchorMax = new Vector2(1, 1);
        tRect.offsetMin = Vector2.zero;
        tRect.offsetMax = Vector2.zero;
        qm.timerText = tText;

        qm.optionTexts = new TextMeshProUGUI[3];
        qm.optionBackgrounds = new Image[3];
        float width = 0.25f, spacing = 0.05f, startX = 0.05f;

        for (int i = 0; i < 3; i++)
        {
            GameObject optBgObj = new GameObject("OptionBg_" + i);
            optBgObj.transform.SetParent(panelObj.transform, false);
            Image optBg = optBgObj.AddComponent<Image>();
            optBg.color = Color.white;
            RectTransform optBgRect = optBgObj.GetComponent<RectTransform>();
            float xMin = startX + (width + spacing) * i;
            optBgRect.anchorMin = new Vector2(xMin, 0.2f);
            optBgRect.anchorMax = new Vector2(xMin + width, 0.5f);
            optBgRect.offsetMin = Vector2.zero;
            optBgRect.offsetMax = Vector2.zero;

            GameObject optTextObj = new GameObject("OptionText");
            optTextObj.transform.SetParent(optBgObj.transform, false);
            TextMeshProUGUI optText = optTextObj.AddComponent<TextMeshProUGUI>();
            optText.text = "Option " + (i+1);
            optText.alignment = TextAlignmentOptions.Center;
            optText.color = Color.black;
            RectTransform optTextRect = optTextObj.GetComponent<RectTransform>();
            optTextRect.anchorMin = Vector2.zero;
            optTextRect.anchorMax = Vector2.one;
            optTextRect.offsetMin = Vector2.zero;
            optTextRect.offsetMax = Vector2.zero;

            qm.optionBackgrounds[i] = optBg;
            qm.optionTexts[i] = optText;
        }
        panelObj.SetActive(false);

        // --- End Screen Panels --- //
        GameObject endScreenPanel = CreateEndPanelFull("EndScreenPanel", canvasObj.transform, gm);
        gm.retryPanel = endScreenPanel;
        gm.winPanel = endScreenPanel;
        endScreenPanel.SetActive(false);
    }

    private static GameObject CreateEndPanelFull(string name, Transform parent, GameManager gm)
    {
        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);
        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0, 0, 0, 0.85f);
        RectTransform pRect = panel.GetComponent<RectTransform>();
        pRect.anchorMin = new Vector2(0.2f, 0.2f);
        pRect.anchorMax = new Vector2(0.8f, 0.8f);
        pRect.offsetMin = Vector2.zero;
        pRect.offsetMax = Vector2.zero;

        GameObject tObj = new GameObject("Title");
        tObj.transform.SetParent(panel.transform, false);
        TextMeshProUGUI tText = tObj.AddComponent<TextMeshProUGUI>();
        tText.text = "Message";
        tText.alignment = TextAlignmentOptions.Center;
        tText.fontSize = 60;
        tText.color = Color.white;
        RectTransform tRect = tObj.GetComponent<RectTransform>();
        tRect.anchorMin = new Vector2(0, 0.6f);
        tRect.anchorMax = new Vector2(1, 1f);
        tRect.offsetMin = Vector2.zero;
        tRect.offsetMax = Vector2.zero;

        GameObject b1 = CreateUIRect("PlayAgainBtn", panel.transform, new Vector2(0.1f, 0.2f), new Vector2(0.45f, 0.4f));
        Image bg1 = b1.AddComponent<Image>();
        TextMeshProUGUI txt1 = CreateUIText("PlayAgainTxt", b1.transform, "Play Again");

        GameObject b2 = CreateUIRect("MainMenuBtn", panel.transform, new Vector2(0.55f, 0.2f), new Vector2(0.9f, 0.4f));
        Image bg2 = b2.AddComponent<Image>();
        TextMeshProUGUI txt2 = CreateUIText("MainMenuTxt", b2.transform, "Main Menu");

        // Assign arrays to GameManager so it can highlight them
        gm.optionBackgrounds = new Image[] { bg1, bg2 };
        gm.optionTexts = new TextMeshProUGUI[] { txt1, txt2 };

        return panel;
    }

    private static GameObject CreateEndPanel(string name, Transform parent, string titleStr, Color titleCol)
    {
        return null; // Deprecated by CreateEndPanelFull
    }

    private static GameObject CreatePlatform(string name, Vector3 pos, Vector2 size, int layer)
    {
        GameObject plat = new GameObject(name);
        plat.transform.position = pos;
        
        SpriteRenderer sr = plat.AddComponent<SpriteRenderer>();
        Sprite brickSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/brick.png");
        
        if (brickSprite != null)
        {
            sr.sprite = brickSprite;
            sr.drawMode = SpriteDrawMode.Tiled;
            sr.size = size;
        }

        BoxCollider2D col = plat.AddComponent<BoxCollider2D>();
        col.size = size;

        if (layer != -1) plat.layer = layer;
        sr.sortingOrder = 1;
        return plat;
    }

    private static GameObject CreateHazard(string name, Vector3 pos, Vector2 size)
    {
        GameObject haz = new GameObject(name);
        haz.transform.position = pos;
        
        SpriteRenderer sr = haz.AddComponent<SpriteRenderer>();
        Sprite spikeSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/spike.png");
        if (spikeSprite != null)
        {
            sr.sprite = spikeSprite;
            sr.drawMode = SpriteDrawMode.Tiled;
            sr.size = size;
        }
        
        BoxCollider2D col = haz.AddComponent<BoxCollider2D>();
        col.size = size;
        haz.AddComponent<DamageHazard>();
        sr.sortingOrder = 2;
        return haz;
    }

    private static void CreateInvisibleHazard(string name, Vector3 pos, Vector2 size)
    {
        GameObject haz = new GameObject(name);
        haz.transform.position = pos;
        BoxCollider2D col = haz.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = size;
        haz.AddComponent<DamageHazard>();
    }

    private static void CreateCheckpoint(string name, Vector3 pos)
    {
        GameObject cp = new GameObject(name);
        cp.transform.position = pos;
        
        SpriteRenderer sr = cp.AddComponent<SpriteRenderer>();
        Sprite flagSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/flag.png");
        if (flagSprite != null)
        {
            sr.sprite = flagSprite;
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = new Vector2(1, 1);
        }
        
        BoxCollider2D col = cp.AddComponent<BoxCollider2D>();
        col.isTrigger = true;
        col.size = new Vector2(1, 1);
        cp.AddComponent<Checkpoint>();
        sr.sortingOrder = 2;

        GameObject sparkles = new GameObject("Sparkles");
        sparkles.transform.SetParent(cp.transform);
        sparkles.transform.localPosition = Vector3.zero;
        ParticleSystem ps = sparkles.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.startLifetime = 1f;
        main.startSpeed = 1f;
        main.startSize = 0.2f;
        main.startColor = Color.yellow;
        var emission = ps.emission;
        emission.rateOverTime = 5f;
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.sortingOrder = 5;
    }

    private static void CreateKnowledgeTerminal(string name, Vector3 pos, List<QuizQuestion> pool)
    {
        GameObject term = new GameObject(name);
        term.transform.position = pos;
        
        SpriteRenderer sr = term.AddComponent<SpriteRenderer>();
        Sprite doorSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/door.png");
        if (doorSprite != null)
        {
            sr.sprite = doorSprite;
            sr.drawMode = SpriteDrawMode.Sliced;
            sr.size = new Vector2(1.5f, 2f);
        }
        
        BoxCollider2D col = term.AddComponent<BoxCollider2D>();
        col.size = new Vector2(1.5f, 2f);
        col.isTrigger = true;
        
        KnowledgeTerminal kt = term.AddComponent<KnowledgeTerminal>();
        kt.questionPool = pool;
        sr.sortingOrder = 0;
    }
}

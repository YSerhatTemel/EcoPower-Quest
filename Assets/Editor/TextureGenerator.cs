using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureGenerator
{
    [MenuItem("EcoPower Quest/Generate Base Textures")]
    public static void GenerateTextures()
    {
        if (GenerateTexturesIfNeeded(true)) // Force true to override old textures
        {
            AssetDatabase.Refresh();
            Debug.Log("Procedural textures generated in Assets/Sprites");
        }
    }

    public static bool GenerateTexturesIfNeeded(bool force = false)
    {
        string dir = "Assets/Sprites";
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        bool generated = false;
        if (force || !File.Exists(dir + "/brick.png")) { GenerateBrickTexture(dir + "/brick.png"); generated = true; }
        if (force || !File.Exists(dir + "/brick.png")) { GenerateBrickTexture(dir + "/brick.png"); generated = true; }
        if (force || !File.Exists(dir + "/player_idle.png")) { GeneratePlayerFrame(dir + "/player_idle.png", 0); generated = true; }
        if (force || !File.Exists(dir + "/player_walk1.png")) { GeneratePlayerFrame(dir + "/player_walk1.png", 1); generated = true; }
        if (force || !File.Exists(dir + "/player_walk2.png")) { GeneratePlayerFrame(dir + "/player_walk2.png", 2); generated = true; }
        if (force || !File.Exists(dir + "/flag.png")) { GenerateFlagTexture(dir + "/flag.png"); generated = true; }
        if (force || !File.Exists(dir + "/spike.png")) { GenerateSpikeTexture(dir + "/spike.png"); generated = true; }
        if (force || !File.Exists(dir + "/door.png")) { GenerateDoorTexture(dir + "/door.png"); generated = true; }
        
        return generated;
    }

    private static void GenerateBrickTexture(string path)
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color brickColor = new Color(0.8f, 0.3f, 0.1f);
        Color mortarColor = new Color(0.9f, 0.9f, 0.9f);
        Color shadowColor = new Color(0.5f, 0.1f, 0.05f);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (y % 32 == 0 || y % 32 == 31 || 
                   (y < 32 && (x % 32 == 0 || x % 32 == 31)) || 
                   (y >= 32 && ((x + 16) % 32 == 0 || (x + 16) % 32 == 31)))
                {
                    tex.SetPixel(x, y, mortarColor);
                }
                else
                {
                    Color c = brickColor;
                    if (Random.value > 0.8f) c *= 0.9f;
                    if (y % 32 < 3 || x % 32 > 28) c = shadowColor;
                    tex.SetPixel(x, y, c);
                }
            }
        }
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }

    private static void GeneratePlayerFrame(string path, int frameIndex)
    {
        // frameIndex: 0 = Idle, 1 = Walk1 (Right leg forward), 2 = Walk2 (Left leg forward)
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        Color shirtColor = new Color(0.9f, 0.1f, 0.1f); // Red
        Color overallColor = new Color(0.1f, 0.3f, 0.9f); // Blue
        Color skinColor = new Color(1f, 0.8f, 0.6f); // Peach
        Color hatColor = new Color(0.9f, 0.1f, 0.1f); // Red Hat
        Color hairColor = new Color(0.3f, 0.1f, 0.05f); // Brown mustache/hair
        Color shoeColor = new Color(0.4f, 0.2f, 0.1f); // Brown shoes

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                tex.SetPixel(x, y, clear);

                // Body parameters
                bool isHead = x >= 16 && x <= 44 && y >= 32 && y <= 56;
                bool isHat = x >= 12 && x <= 48 && y > 50 && y <= 60;
                bool isHatBrim = x >= 30 && x <= 56 && y >= 48 && y <= 52;
                bool isMustache = x >= 36 && x <= 52 && y >= 36 && y <= 40;
                bool isEye = x >= 40 && x <= 44 && y >= 44 && y <= 48;
                bool isNose = x >= 46 && x <= 56 && y >= 40 && y <= 46;

                // Torso
                bool isShirt = x >= 20 && x <= 40 && y >= 16 && y <= 32;
                bool isOverall = x >= 22 && x <= 38 && y >= 12 && y <= 24;
                bool isButton = (x >= 24 && x <= 26 && y >= 20 && y <= 22) || (x >= 34 && x <= 36 && y >= 20 && y <= 22);

                // Draw Head
                if (isHead) tex.SetPixel(x, y, skinColor);
                if (isNose) tex.SetPixel(x, y, skinColor);
                if (isMustache) tex.SetPixel(x, y, hairColor);
                if (isEye) tex.SetPixel(x, y, Color.black);
                if (isHat || isHatBrim) tex.SetPixel(x, y, hatColor);

                // Draw Torso
                if (isShirt && !isHead) tex.SetPixel(x, y, shirtColor);
                if (isOverall) tex.SetPixel(x, y, overallColor);
                if (isButton) tex.SetPixel(x, y, Color.yellow);

                // Draw Legs and Shoes based on Animation Frame
                if (y < 16)
                {
                    if (frameIndex == 0) // Idle
                    {
                        if (x >= 20 && x <= 28 && y >= 6 && y <= 12) tex.SetPixel(x, y, overallColor); // Back leg
                        if (x >= 32 && x <= 40 && y >= 6 && y <= 12) tex.SetPixel(x, y, overallColor); // Front leg
                        
                        if (x >= 16 && x <= 30 && y >= 0 && y <= 6) tex.SetPixel(x, y, shoeColor); // Back shoe
                        if (x >= 32 && x <= 46 && y >= 0 && y <= 6) tex.SetPixel(x, y, shoeColor); // Front shoe
                    }
                    else if (frameIndex == 1) // Walk 1 (Spread)
                    {
                        if (x >= 12 && x <= 24 && y >= 6 && y <= 16) tex.SetPixel(x, y, overallColor); // Back leg spread
                        if (x >= 36 && x <= 48 && y >= 6 && y <= 16) tex.SetPixel(x, y, overallColor); // Front leg spread
                        
                        if (x >= 8 && x <= 24 && y >= 0 && y <= 6) tex.SetPixel(x, y, shoeColor); // Back shoe
                        if (x >= 36 && x <= 54 && y >= 0 && y <= 6) tex.SetPixel(x, y, shoeColor); // Front shoe
                    }
                    else if (frameIndex == 2) // Walk 2 (Crossed)
                    {
                        if (x >= 26 && x <= 34 && y >= 6 && y <= 16) tex.SetPixel(x, y, overallColor); // Legs crossed
                        
                        if (x >= 20 && x <= 36 && y >= 0 && y <= 6) tex.SetPixel(x, y, shoeColor); // Back shoe tucked
                        if (x >= 24 && x <= 40 && y >= 0 && y <= 6) tex.SetPixel(x, y, shoeColor); // Front shoe tucked
                    }
                }
            }
        }
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }

    private static void GenerateFlagTexture(string path)
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        Color poleColor = new Color(0.7f, 0.7f, 0.7f); // Silver pole
        Color flagColor = Color.yellow;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                tex.SetPixel(x, y, clear);

                // Pole
                if (x > 8 && x < 12 && y > 0 && y < 60)
                {
                    tex.SetPixel(x, y, poleColor);
                }

                // Triangle Flag
                if (y > 30 && y < 60)
                {
                    float flagWidth = 60 - y; // Gets narrower towards top/bottom
                    if (x >= 12 && x < 12 + (60 - Mathf.Abs(45 - y) * 2))
                    {
                        Color c = flagColor;
                        if (x % 8 < 2) c *= 0.9f; // fabric fold illusion
                        tex.SetPixel(x, y, c);
                    }
                }
            }
        }
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }

    private static void GenerateSpikeTexture(string path)
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        Color baseColor = new Color(0.5f, 0.1f, 0.1f); // Dark red base
        Color spikeColor = new Color(0.8f, 0.8f, 0.8f); // Silver spike

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                tex.SetPixel(x, y, clear);

                // Solid base
                if (y < 16)
                {
                    Color c = baseColor;
                    if (y > 12) c *= 0.8f;
                    tex.SetPixel(x, y, c);
                }
                // 4 spikes per 64px block
                else if (y >= 16)
                {
                    int localX = x % 16; // 0 to 15
                    int spikeHeight = 16 - Mathf.Abs(8 - localX) * 2; // Peak at localX=8
                    if (y - 16 < spikeHeight * 2) // scale height
                    {
                        Color c = spikeColor;
                        if (localX > 8) c *= 0.7f; // 3D shadow on right side of spike
                        tex.SetPixel(x, y, c);
                    }
                }
            }
        }
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }

    private static void GenerateDoorTexture(string path)
    {
        int size = 64;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Color clear = new Color(0, 0, 0, 0);
        Color woodColor = new Color(0.4f, 0.2f, 0.1f); // Brown
        Color woodDark = new Color(0.3f, 0.15f, 0.05f); // Dark Brown
        Color knobColor = new Color(0.9f, 0.8f, 0.2f); // Gold

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                tex.SetPixel(x, y, clear);

                // Door frame (Rectangle with rounded top)
                if (x >= 8 && x <= 56 && y >= 0)
                {
                    // Rounded top
                    if (y > 48)
                    {
                        float dx = x - 32;
                        float dy = y - 48;
                        if (Mathf.Sqrt(dx * dx + dy * dy) > 24) continue;
                    }

                    Color c = woodColor;
                    
                    // Wood planks (vertical lines)
                    if (x % 12 < 2) c = woodDark;
                    // Border
                    if (x < 12 || x > 52 || y > 52) c = woodDark;

                    // Doorknob
                    if (x >= 44 && x <= 48 && y >= 28 && y <= 32) c = knobColor;

                    tex.SetPixel(x, y, c);
                }
            }
        }
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());
    }
}

﻿using Raylib_cs;
using System;
using System.Numerics;

public class Program
{
    static string title = "Game Title"; // Window title
    static int screenWidth = 800; // Screen width
    static int screenHeight = 600; // Screen height
    static int targetFps = 60; // Target frames-per-second

    static Vector2 playerPosition; // Player position
    static float playerSpeed = 200.0f; // Player speed
    static int playerSize = 50; // Player size
    static Rectangle room; // Room boundaries
    static Rectangle door; // Door boundaries

    static Vector2 npcPosition; // NPC position
    static int npcSize = 50; // NPC size
    static bool isChatting = false; // Chat state

    static void Main()
    {
        // Create a window to draw to
        Raylib.InitWindow(screenWidth, screenHeight, title);
        // Set the target frames-per-second (FPS)
        Raylib.SetTargetFPS(targetFps);
        // Setup your game
        Setup();
        // Loop so long as window should not close
        while (!Raylib.WindowShouldClose())
        {
            // Enable drawing to the canvas (window)
            Raylib.BeginDrawing();
            // Clear the canvas with one color
            Raylib.ClearBackground(Color.Black);
            // Update game state
            Update();
            // Draw game state
            Draw();
            // Stop drawing to the canvas, begin displaying the frame
            Raylib.EndDrawing();
        }
        // Close the window
        Raylib.CloseWindow();
    }

    static void Setup()
    {
        // Initialize player position
        playerPosition = new Vector2(screenWidth / 2, screenHeight / 2);
        // Define room boundaries
        room = new Rectangle(50, 50, screenWidth - 100, screenHeight - 100);
        // Define door boundaries
        door = new Rectangle(screenWidth - 100, screenHeight / 2 - 25, 50, 50);
        // Initialize NPC position
        npcPosition = new Vector2(screenWidth / 3, screenHeight / 2);
    }

    static void Update()
    {
        float deltaTime = Raylib.GetFrameTime();

        // Player movement
        if (Raylib.IsKeyDown(KeyboardKey.W) && playerPosition.Y - playerSize / 2 > room.Y)
        {
            playerPosition.Y -= playerSpeed * deltaTime;
        }
        if (Raylib.IsKeyDown(KeyboardKey.S) && playerPosition.Y + playerSize / 2 < room.Y + room.Height)
        {
            playerPosition.Y += playerSpeed * deltaTime;
        }
        if (Raylib.IsKeyDown(KeyboardKey.A) && playerPosition.X - playerSize / 2 > room.X)
        {
            playerPosition.X -= playerSpeed * deltaTime;
        }
        if (Raylib.IsKeyDown(KeyboardKey.D) && playerPosition.X + playerSize / 2 < room.X + room.Width)
        {
            playerPosition.X += playerSpeed * deltaTime;
        }

        // Check if player is colliding with the door
        Rectangle playerRect = new Rectangle(playerPosition.X - playerSize / 2, playerPosition.Y - playerSize / 2, playerSize, playerSize);
        if (Raylib.CheckCollisionRecs(playerRect, door))
        {
            Raylib.CloseWindow();
        }

        // Check if player is near the NPC and triggers chatting
        Rectangle npcRect = new Rectangle(npcPosition.X - npcSize / 2, npcPosition.Y - npcSize / 2, npcSize, npcSize);
        if (Raylib.CheckCollisionRecs(playerRect, npcRect))
        {
            isChatting = true;
        }
        else
        {
            isChatting = false;
        }
    }

    static void Draw()
    {
        // Draw room boundaries
        Raylib.DrawRectangleLinesEx(room, 3, Color.RayWhite);
        // Draw door
        Raylib.DrawRectangleRec(door, Color.White);
        // Draw player
        Raylib.DrawRectangle((int)(playerPosition.X - playerSize / 2), (int)(playerPosition.Y - playerSize / 2), playerSize, playerSize, Color.RayWhite);
        // Draw NPC
        Raylib.DrawRectangle((int)(npcPosition.X - npcSize / 2), (int)(npcPosition.Y - npcSize / 2), npcSize, npcSize, Color.Blue);

        // Draw chat box if interacting with NPC
        if (isChatting)
        {
            string message = "Listen, son, times are tough. I know it's not easy, but you have to learn to survive on your own. There's a gun on the table over there. Take it. Go out into the world and do what you can to survive.";
            DrawDialogueBox(message);
        }
    }

    static void DrawDialogueBox(string message)
    {
        int boxWidth = screenWidth - 20;
        int boxHeight = 100;
        int boxX = 10;
        int boxY = screenHeight - boxHeight - 10;

        // Draw the box background
        Raylib.DrawRectangle(boxX, boxY, boxWidth, boxHeight, Color.RayWhite);

        // Draw the box border
        Raylib.DrawRectangleLines(boxX, boxY, boxWidth, boxHeight, Color.Black);

        // Draw the wrapped message text
        DrawTextBoxed(message, new Rectangle(boxX + 10, boxY + 10, boxWidth - 20, boxHeight - 20), 20, 2, Color.Black);
    }

    static void DrawTextBoxed(string text, Rectangle rec, int fontSize, int textPadding, Color color)
    {
        int maxWidth = (int)rec.Width;
        int lineHeight = fontSize + textPadding;
        int y = (int)rec.Y;

        string[] words = text.Split(' ');
        string line = "";

        for (int i = 0; i < words.Length; i++)
        {
            string testLine = line + words[i] + " ";
            int lineWidth = Raylib.MeasureText(testLine, fontSize);

            if (lineWidth > maxWidth)
            {
                Raylib.DrawText(line, (int)rec.X, y, fontSize, color);
                line = words[i] + " ";
                y += lineHeight;

                if (y > rec.Y + rec.Height - lineHeight)
                {
                    break;
                }
            }
            else
            {
                line = testLine;
            }
        }

        Raylib.DrawText(line, (int)rec.X, y, fontSize, color);
    }
}

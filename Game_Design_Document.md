# Game Design Document (GDD) - Unity 2D Game Project

## 1. Game Overview
*   **Game Name:** EcoPower
*   **Genre:** Didactic 2D Platformer
*   **Target Audience:** School-age children
*   **Objective:** Promote environmental education and basic logical reasoning through simple platform mechanics, while enhancing inclusivity in alignment with the +Power console's vision.

## 2. Control Mechanics
The +Power console has only 3 buttons. The game is designed to be 100% playable using exactly these 3 keyboard keys:

*   **TAB (Left Button):**
    *   *In-Game:* Move the character to the Left.
    *   *Menus (Main Menu, Try Again, Level Complete):* Cycle to the previous option.
*   **BACKSPACE (Right Button):**
    *   *In-Game:* Move the character to the Right.
    *   *Menus (Main Menu, Try Again, Level Complete):* Cycle to the next option.
*   **ENTER (Center Button):**
    *   *In-Game:* Jump.
    *   *Menus (Main Menu, Try Again, Level Complete):* Confirm selection.

*(Note: The UI and gameplay seamlessly integrate this 3-button scheme, allowing full navigation without a mouse.)*

## 3. Difficulty Levels
The game features 3 progression levels, fulfilling the project requirements:

### Level 1: Easy
*   **Level Design:** Long platforms, short gaps, no moving enemies. All gaps are adjusted to ensure the stage is 100% physically passable.
*   **Didactic Challenge:** Simple questions with unlimited time (e.g., color matching or basic addition).
*   **Penalty:** Answering incorrectly only gives a warning and allows the player to try again without losing progress.

### Level 2: Medium
*   **Level Design:** Shorter platforms, introduction of moving platforms and static obstacles (like spikes). Gaps are designed to be safe but require timing.
*   **Didactic Challenge:** Basic logic or math questions with a generous time limit.
*   **Penalty:** Dying or answering incorrectly sends the player back to the last activated checkpoint, ensuring smooth progression without full level restarts.

### Level 3: Hard
*   **Level Design:** Precise jumps and platforms that fall 1-2 seconds after being stepped on.
*   **Didactic Challenge:** Complex questions with a short time limit (e.g., 10 seconds for harder equations or memory puzzles).
*   **Penalty:** Answering incorrectly or running out of time directly triggers the "Try Again" screen, forcing the player to restart from the last checkpoint.

## 4. Visuals, Audio, and Animations
*   **Graphics:** Friendly 2D style. Background layers are dynamically dimmed to increase contrast and improve gameplay clarity. Character feet are perfectly aligned with the ground for realistic interaction.
*   **Characters:** The main character is a fully animated, Mario-style human sprite featuring smooth walking frames, jumping, and idle animations.
*   **UI/UX:** Polished aesthetics for the Main Menu, Try Again, and Level Complete screens. Visual effects like yellow highlighting indicate the currently selected option, adhering strictly to the 3-button constraint.
*   **Audio:** Sound effects for jumping, dying, collecting items, and UI interactions. A cheerful, relaxing background track.
